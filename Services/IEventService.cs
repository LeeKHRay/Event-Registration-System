using EventRegistrationSystem.Models.Events;
using EventRegistrationSystem.ViewModels.Events;

namespace EventRegistrationSystem.Services
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync(EventSearchCriteria searchCriteria, params string[] includeProperties);
        Task<List<Event>> GetEnrolledEventsAsync(EventSearchCriteria searchCriteria, string? userId, params string[] includeProperties);
        Task<List<Event>> GetCreatedEventsAsync(EventSearchCriteria searchCriteria, string? organizationId, params string[] includeProperties);
        Task<List<Event>> GetNewestNEventsAsync(int n, params string[] includeProperties);
        Task<Event?> GetEventByIdAsync(int id, params string[] includeProperties);
        Task<bool> AddEventAsync(Event @event);
        Task<bool> UpdateEventAsync(Event @event);
        Task<bool> DeleteEventByIdAsync(int id);
        Task<bool> EventExistsAsync(int id, string? organizationId = null);
        Task<bool> EventsExistAsync();
        Task<bool> EnrolledEventsExistAsync(string? userId);
        Task<bool> CreatedEventsExistAsync(string? organizationId);
        Task<Dictionary<string, string>> ValidateEvent(Event @event);
        (string, List<string>)? ValidateEventDateTimePeriod(string[] propertyNames, DateTime[] dateTimes);
    }
}
