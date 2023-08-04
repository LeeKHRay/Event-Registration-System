using EventRegistrationSystem.Data;
using EventRegistrationSystem.Models.Events;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventRegistrationSystem.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventRegistrationSystemDbContext _context;

        public EventRepository(EventRegistrationSystemDbContext context)
        {
            _context = context;
        }

        public Task<List<Event>> GetEventsAsync(
            Expression<Func<Event, bool>>? predicate = null, 
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null, 
            int? count = null, 
            params string[] includeProperties)
        {
            IQueryable<Event> query = _context.Event;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Aggregate(query, (query, property) => query.Include(property));

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (count != null)
            {
                query = query.Take(count.Value);
            }

            return query.ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id, params string[] includeProperties)
        {
            if (includeProperties.Length == 0)
            {
                return await _context.Event.FindAsync(id);
            }

            IQueryable<Event> query = _context.Event;
            query = includeProperties.Aggregate(query, (query, property) => query.Include(property));
            return await query.SingleOrDefaultAsync(e => e.Id == id);
        }

        public Task<int> EventsExistsAsync(Expression<Func<Event, bool>>? predicate = null)
        {
            IQueryable<Event> query = _context.Event;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.CountAsync();
        }

        public Task<int> CountEventsAsync(Expression<Func<Event, bool>>? predicate = null)
        {
            IQueryable<Event> query = _context.Event;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.CountAsync();
        }

        public async Task<bool> AddEventAsync(Event @event)
        {
            if (@event == null)
            {
                return false;
            }

            _context.Add(@event);
            int count = await _context.SaveChangesAsync();
            if (count <= 0)
            {
                throw new Exception("Event or EventImage is invalid");
            }

            return true;
        }

        public async Task<bool> UpdateEventAsync(Event updatedEvent)
        {
            if (updatedEvent == null)
            {
                return false;
            }

            var @event = await _context.Event.FindAsync(updatedEvent.Id);
            if (@event == null)
            {
                return false;
            }

            // avoid updating the passed datetimes
            DateTime now = DateTime.Now;
            now = now.AddSeconds(-now.Second);

            if (@event.EnrollStartTime <= now)
            {
                updatedEvent.EnrollStartTime = @event.EnrollStartTime;
            }
            if (@event.EnrollEndTime <= now)
            {
                updatedEvent.EnrollEndTime = @event.EnrollEndTime;
            }
            if (@event.EventStartTime <= now)
            {
                updatedEvent.EventStartTime = @event.EventStartTime;
            }
            if (@event.EventEndTime <= now)
            {
                updatedEvent.EventEndTime = @event.EventEndTime;
            }

            var entry = _context.Entry(@event);
            entry.CurrentValues.SetValues(updatedEvent); // SetValues will not update navigation properties
            entry.Property(e => e.CreateTime).IsModified = false; // avoid updating CreateTime in SQL
            entry.Property(e => e.CreatorId).IsModified = false; // avoid updating CreatorId in SQL

            @event.EventImages = updatedEvent.EventImages;

            int count = await _context.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> DeleteEventByIdAsync(int id)
        {
            int count = await _context.Event.Where(e => e.Id == id).ExecuteDeleteAsync();
            return count > 0;
        }

        public Task<bool> EventsExistAsync(Expression<Func<Event, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                return _context.Event.AnyAsync(predicate);
            }

            return _context.Event.AnyAsync();
        }
    }
}
