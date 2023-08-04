using EventRegistrationSystem.Models.Events;

namespace EventRegistrationSystem.Services
{
    public interface IEventCategoryService
    {
        Task InitializeCategoriesAsync();
        Task<List<EventCategory>> GetCategoriesAsync();
    }
}
