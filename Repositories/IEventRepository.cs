using EventRegistrationSystem.Models.Events;
using System.Linq.Expressions;

namespace EventRegistrationSystem.Repositories
{
    public interface IEventRepository
    {
        Task<List<Event>> GetEventsAsync(
            Expression<Func<Event, bool>>? predicate = null, 
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null, 
            int? count = null, 
            params string[] includeProperties);
        Task<Event?> GetEventByIdAsync(int id, params string[] includeProperties);
        Task<bool> AddEventAsync(Event @event);
        Task<bool> UpdateEventAsync(Event @event);
        Task<bool> DeleteEventByIdAsync(int id);
        Task<bool> EventsExistAsync(Expression<Func<Event, bool>>? predicate = null);
    }
}
