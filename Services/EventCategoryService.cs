using EventRegistrationSystem.Models.Events;
using EventRegistrationSystem.Repositories;

namespace EventRegistrationSystem.Services
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public EventCategoryService(IEventCategoryRepository eventCategoryRepository)
        {
            _eventCategoryRepository = eventCategoryRepository;
        }

        public Task InitializeCategoriesAsync() => _eventCategoryRepository.InitializeCategoriesAsync();

        public Task<List<EventCategory>> GetCategoriesAsync() => _eventCategoryRepository.GetCategoriesAsync();
    }
}
