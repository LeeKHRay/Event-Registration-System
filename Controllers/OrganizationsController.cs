using AutoMapper;
using EventRegistrationSystem.Areas.Identity.Models;
using EventRegistrationSystem.Data;
using EventRegistrationSystem.Extensions;
using EventRegistrationSystem.Filters;
using EventRegistrationSystem.Services;
using EventRegistrationSystem.ViewModels.Events;
using EventRegistrationSystem.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventRegistrationSystem.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly EventRegistrationSystemDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<OrganizationUser> _userManager;
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;
        private readonly IMapper _mapper;

        public OrganizationsController(
            EventRegistrationSystemDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<OrganizationUser> userManager,
            IEventService eventService,
            IEventCategoryService eventCategoryService,
            IMapper mapper)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
            _mapper = mapper;
        }

        // GET: Organization/{organizationName}
        [SetEventsDisplayType]
        public async Task<IActionResult> Details([FromRoute] string organizationName, [FromQuery] EventSearchCriteria searchCriteria)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            if (_signInManager.IsSignedIn(User))
            {
                if (organizationName.ToUpper() == userName?.ToUpper())
                {
                    return RedirectToAction("MyEvents", "Events");
                }
            }

            var organization = await _userManager.FindByNameAsync(organizationName);
            if (organization == null)
            {
                return NotFound();
            }
            
            bool eventsExist = await _eventService.CreatedEventsExistAsync(organization.Id);

            ViewBag.EventsExist = eventsExist;
            if (eventsExist)
            {
                // organization.Events will be loaded by this query
                await _eventService.GetCreatedEventsAsync(searchCriteria, organization.Id, "EventImages");

                ViewBag.SearchCriteria = searchCriteria;
                ViewBag.UserPreferences = HttpContext.Session.GetObjectFromJson<UserPreferences>("preferences");
            }

            var model = _mapper.Map<OrganizationDetailsViewModel>(organization);

            return View(model);
        }
    }
}
