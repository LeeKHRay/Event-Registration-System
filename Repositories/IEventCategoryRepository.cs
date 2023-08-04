using EventRegistrationSystem.Models.Events;

namespace EventRegistrationSystem.Repositories
{
    public interface IEventCategoryRepository
    {
        Task InitializeCategoriesAsync();
        Task<List<EventCategory>> GetCategoriesAsync();
    }
}
