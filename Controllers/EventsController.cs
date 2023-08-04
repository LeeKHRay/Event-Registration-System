using AutoMapper;
using EventRegistrationSystem.Areas.Identity.Models;
using EventRegistrationSystem.Extensions;
using EventRegistrationSystem.Filters;
using EventRegistrationSystem.Models.Events;
using EventRegistrationSystem.Services;
using EventRegistrationSystem.ViewModels.Events;
using EventRegistrationSystem.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EventRegistrationSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IEventEnrollmentService _eventEnrollmentService;
        private readonly IEventImageService _eventImageService;
        private readonly IMapper _mapper;

        public EventsController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEventService eventService, 
            IEventCategoryService eventCategoryService, 
            IEventEnrollmentService eventEnrollmentService,
            IEventImageService eventImageService, 
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
            _eventEnrollmentService = eventEnrollmentService;
            _eventImageService = eventImageService;
            _mapper = mapper;
        }

        [HttpGet("/api/[controller]/[action]")]
        [EventsPartialView]
        public async Task<IActionResult> EventsPartial([FromQuery] EventSearchCriteria searchCriteria, [FromQuery] string displayType)
        {
            List<Event> events = await _eventService.GetEventsAsync(searchCriteria, "EventImages");
            List<EventViewModel> viewModels = _mapper.Map<List<EventViewModel>>(events);

            return PartialView($"_Events{displayType}Partial", viewModels);
        }

        [HttpGet("/api/[controller]/[action]")]
        [Authorize(Roles = "GeneralUser")]
        [EventsPartialView]
        public async Task<IActionResult> EnrolledEventsPartial([FromQuery] EventSearchCriteria searchCriteria, [FromQuery] string displayType)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            List<Event> events = await _eventService.GetEnrolledEventsAsync(searchCriteria, userId, "EventImages");
            List<EventViewModel> viewModels = _mapper.Map<List<EventViewModel>>(events);

            return PartialView($"_Events{displayType}Partial", viewModels);
        }

        [HttpGet("/api/[controller]/[action]/{organizationName}")]
        [EventsPartialView]
        public async Task<IActionResult> CreatedEventsPartial([FromRoute] string organizationName, [FromQuery] EventSearchCriteria searchCriteria, [FromQuery] string displayType)
        {
            var organization = await _userManager.FindByNameAsync(organizationName);
            if (organization == null)
            {
                return NotFound();
            }

            List<Event> events = await _eventService.GetCreatedEventsAsync(searchCriteria, organization.Id, "EventImages");
            List<EventViewModel> viewModels = _mapper.Map<List<EventViewModel>>(events);

            return PartialView($"_Events{displayType}Partial", viewModels);
        }

        // GET: /
        [HttpGet]
        [SetEventsDisplayType]
        public async Task<IActionResult> Index(EventSearchCriteria searchCriteria)
        {
            bool eventsExist = await _eventService.EventsExistAsync();

            ViewBag.IsOrganizationUser = await _userManager.IsInRoleAsync(User, "OrganizationUser");
            ViewBag.EventsExist = eventsExist;
            if (!eventsExist)
            {
                return View();
            }

            List<Event> events = await _eventService.GetEventsAsync(searchCriteria, "EventImages");

            ViewBag.SearchCriteria = searchCriteria;
            ViewBag.UserPreferences = HttpContext.Session.GetObjectFromJson<UserPreferences>("preferences");

            List<EventViewModel> model = _mapper.Map<List<EventViewModel>>(events);

            return View(model);
        }

        // Get: Events/MyEvents
        [HttpGet]
        [Authorize]
        [SetEventsDisplayType]
        public async Task<IActionResult> MyEvents([FromQuery] EventSearchCriteria searchCriteria)
        {
            bool isOrganizationUser = await _userManager.IsInRoleAsync(User, "OrganizationUser");
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool eventsExist = isOrganizationUser ? await _eventService.CreatedEventsExistAsync(userId) : await _eventService.EnrolledEventsExistAsync(userId);

            ViewBag.IsOrganizationUser = isOrganizationUser;
            ViewBag.EventsExist = eventsExist;
            if (!eventsExist)
            {
                return View();
            }

            List<Event> events;
            if (isOrganizationUser)
            {
                ViewBag.OrganizationName = User.FindFirstValue(ClaimTypes.Name);
                events = await _eventService.GetCreatedEventsAsync(searchCriteria, userId, "EventImages");
            }
            else
            {
                events = await _eventService.GetEnrolledEventsAsync(searchCriteria, userId, "EventImages");
            }

            ViewBag.SearchCriteria = searchCriteria;
            ViewBag.UserPreferences = HttpContext.Session.GetObjectFromJson<UserPreferences>("preferences");

            List<EventViewModel> model = _mapper.Map<List<EventViewModel>>(events);

            return View(model);
        }

        // GET: Events/5
        [ServiceFilter(typeof(ValidateEventExistsFilter))]
        public async Task<IActionResult> Details(int? id)
        {
            var @event = await _eventService.GetEventByIdAsync(id!.Value, "EventCategory", "Creator", "EventEnrollments", "EventImages");

            if (_signInManager.IsSignedIn(User))
            { 
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                if (await _userManager.IsInRoleAsync(User, "OrganizationUser"))
                {
                    ViewBag.IsCreater = userId == @event!.CreatorId;
                }
                else
                {
                    if (!await _eventEnrollmentService.UserEnrolledInEvent(@event!, userId))
                    {
                        if (await _eventEnrollmentService.EventAvailable(@event!))
                        {
                            ViewBag.EnrollBtnState = 1;
                        }
                        else
                        {
                            ViewBag.EnrollBtnState = 2;
                        }
                    }
                    else
                    {
                        ViewBag.EnrollBtnState = 3;
                    }

                    ViewBag.IsCreater = false;
                }
            }
            else
            {
                ViewBag.IsCreater = false;
            }

            var model = _mapper.Map<EventDetailsViewModel>(@event);
            return View(model);
        }

        // POST: Events/Enroll
        [HttpPost]
        [Authorize(Roles = "GeneralUser")]
        [ServiceFilter(typeof(ValidateEventExistsFilter))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (await _eventEnrollmentService.EnrollEventAsync(id.Value, userId))
            {
                TempData["Message"] = "Enrolled event successfully"; // the value will be used in _MessageNotificationPartial
            }
            else
            {
                TempData["Message"] = "Fail to enroll";
            }

            return RedirectToAction("Details", new { id });
        }

        // POST: Events/CancellEnrollment
        [HttpPost]
        [Authorize(Roles = "GeneralUser")]
        [ServiceFilter(typeof(ValidateEventExistsFilter))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancellEnrollment(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (await _eventEnrollmentService.DeleteEventEnrollmentAsync(id.Value, userId))
            {
                TempData["Message"] = "Cancel enrollment successfully";
            }
            else
            {
                TempData["Message"] = "Fail to cancel enrollment";
            }

            return RedirectToAction("Details", new { id });
        }

        // GET: Events/Create
        [Authorize(Roles = "OrganizationUser")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _eventCategoryService.GetCategoriesAsync(), "Id", "Name");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [Authorize(Roles = "OrganizationUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var uploadedImages = await _eventImageService.UploadImagesAsync(model.FormImages);

                Event @event = _mapper.Map<Event>(model);
                @event.EventImages = uploadedImages;
                @event.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                await _eventService.AddEventAsync(@event);

                return RedirectToAction("Details", new { id = @event.Id });
            }

            ViewBag.Categories = new SelectList(await _eventCategoryService.GetCategoriesAsync(), "Id", "Name");

            return View(model);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "OrganizationUser")]
        [ServiceFilter(typeof(ValidateEventExistsFilter), Order = 1)]
        [ServiceFilter(typeof(AuthorizeEventFilter), Order = 2)]
        public async Task<IActionResult> Edit(int? id)
        {
            var @event = await _eventService.GetEventByIdAsync(id!.Value, "EventImages");

            EventEditFormViewModel model = _mapper.Map<EventEditFormViewModel>(@event);
            ViewBag.Images = @event!.EventImages.Select(i => new { i.Id, i.FileName });
            ViewBag.DateTimes = new
            {
                @event!.EnrollStartTime,
                @event!.EnrollEndTime,
                @event!.EventStartTime,
                @event!.EventEndTime
            };

            ViewBag.Categories = new SelectList(await _eventCategoryService.GetCategoriesAsync(), "Id", "Name");

            return View(model);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [Authorize(Roles = "OrganizationUser")]
        [ServiceFilter(typeof(ValidateEventExistsFilter), Order = 1)]
        [ServiceFilter(typeof(AuthorizeEventFilter), Order = 2)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromForm] EventEditFormViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            model.Id = id;
            Event @event = _mapper.Map<Event>(model);
            foreach (var error in await _eventService.ValidateEvent(@event))
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            if (ModelState.IsValid)
            {
                model.UploadedImageIds ??= new();

                await _eventImageService.DeleteImagesAsync(id, model.UploadedImageIds);
                var uploadedImages = await _eventImageService.GetImagesAsync(id, model.UploadedImageIds);
                uploadedImages.AddRange(await _eventImageService.UploadImagesAsync(model.FormImages));

                @event.EventImages = uploadedImages;

                try
                {
                    await _eventService.UpdateEventAsync(@event);
                    return RedirectToAction("Details", new { id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _eventService.EventExistsAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new DbUpdateConcurrencyException("Fail to update event information");
                    }
                }
            }

            ViewBag.Images = (await _eventImageService.GetImagesAsync(id)).Select(i => new { i.Id, i.FileName });
            ViewBag.Categories = new SelectList(await _eventCategoryService.GetCategoriesAsync(), "Id", "Name", model.EventCategoryId);
            var originalEvent = await _eventService.GetEventByIdAsync(id);
            ViewBag.DateTimes = new
            {
                originalEvent!.EnrollStartTime,
                originalEvent!.EnrollEndTime,
                originalEvent!.EventStartTime,
                originalEvent!.EventEndTime
            };

            return View(model);
        }

    }
}
