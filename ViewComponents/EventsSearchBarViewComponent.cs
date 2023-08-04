using EventRegistrationSystem.Areas.Identity.Models;
using EventRegistrationSystem.Extensions;
using EventRegistrationSystem.Services;
using EventRegistrationSystem.ViewModels.Events;
using EventRegistrationSystem.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventRegistrationSystem.ViewComponents
{
    public class EventsSearchBarViewComponent : ViewComponent
    {
        private readonly IEventCategoryService _eventCategoryService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public EventsSearchBarViewComponent(IEventCategoryService eventCategoryService, SignInManager<ApplicationUser> signInManager)
        {
            _eventCategoryService = eventCategoryService;
            _signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(EventSearchCriteria searchCriteria, int eventNum)
        {
            ViewBag.Categories = new SelectList(await _eventCategoryService.GetCategoriesAsync(), "Id", "Name");
            ViewBag.EventNum = eventNum; // show number of events found in search bar
            ViewBag.IsSignedIn = _signInManager.IsSignedIn(UserClaimsPrincipal);
            ViewBag.SelectedColor = ViewBag.UserPreferences?.Color ?? HttpContext.Session.GetObjectFromJson<UserPreferences>("preferences")?.Color ?? "blue";

            return View(searchCriteria);
        }
    }
}
