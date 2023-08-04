using AutoMapper;
using EventRegistrationSystem.Services;
using EventRegistrationSystem.ViewModels.Events;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationSystem.ViewComponents
{
    public class NewestEventsCarouselViewComponent : ViewComponent
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public NewestEventsCarouselViewComponent(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            var newestEvents = await _eventService.GetNewestNEventsAsync(count, "EventImages");
            var viewModel = _mapper.Map<List<EventViewModel>>(newestEvents);
            return View(viewModel);
        }
    }
}
