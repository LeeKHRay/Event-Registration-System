using EventRegistrationSystem.Models.Events;
using EventRegistrationSystem.Repositories;
using EventRegistrationSystem.ViewModels.Events;
using LinqKit;

namespace EventRegistrationSystem.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        private Func<IQueryable<Event>, IOrderedQueryable<Event>>? GetOrderBy(string? sortBy) =>
            sortBy switch
            {
                "+CreateTime" => query => query.OrderBy(e => e.CreateTime),
                "-CreateTime" => query => query.OrderByDescending(e => e.CreateTime),
                "+Name" => query => query.OrderBy(e => e.Name),
                "-Name" => query => query.OrderByDescending(e => e.Name),
                _ => null
            };

        private Task<List<Event>> GetEventsAsync(
            EventSearchCriteria searchCriteria,
            string? userId, 
            string? organizationId,
            params string[] includeProperties)
        {
            var predicate = PredicateBuilder.New<Event>(true);

            if (!string.IsNullOrEmpty(searchCriteria.Name))
            {
                predicate.And(e => e.Name.ToLower().Contains(searchCriteria.Name.ToLower()));
            }

            if (searchCriteria.CategoryId > 0)
            {
                predicate.And(e => e.EventCategoryId == searchCriteria.CategoryId);
            }

            if (userId != null)
            {
                predicate.And(e => e.EventEnrollments.Any(ee => ee.UserId == userId));
            }

            if (organizationId != null)
            {
                predicate.And(e => e.CreatorId == organizationId);
            }

            var orderBy = GetOrderBy(searchCriteria.SortBy);

            return _eventRepository.GetEventsAsync(predicate, orderBy, includeProperties: includeProperties);
        }

        public Task<List<Event>> GetEventsAsync(EventSearchCriteria searchCriteria, params string[] includeProperties) => 
            GetEventsAsync(searchCriteria, null, null, includeProperties);

        public Task<List<Event>> GetEnrolledEventsAsync(EventSearchCriteria searchCriteria, string? userId, params string[] includeProperties) =>
            GetEventsAsync(searchCriteria, userId, null, includeProperties);

        public Task<List<Event>> GetCreatedEventsAsync(EventSearchCriteria searchCriteria, string? organizationId, params string[] includeProperties) =>
            GetEventsAsync(searchCriteria, null, organizationId, includeProperties);

        public Task<List<Event>> GetNewestNEventsAsync(int n, params string[] includeProperties) =>
            _eventRepository.GetEventsAsync(orderBy: GetOrderBy("-CreateTime"), count: n, includeProperties: includeProperties);

        public Task<Event?> GetEventByIdAsync(int id, params string[] includeProperties) => _eventRepository.GetEventByIdAsync(id, includeProperties);

        public Task<bool> AddEventAsync(Event @event) => _eventRepository.AddEventAsync(@event);

        public Task<bool> UpdateEventAsync(Event @event) => _eventRepository.UpdateEventAsync(@event);

        public Task<bool> DeleteEventByIdAsync(int id) => _eventRepository.DeleteEventByIdAsync(id);

        public Task<bool> EventExistsAsync(int id, string? organizationId = null)
        {
            ExpressionStarter<Event> predicate = PredicateBuilder.New<Event>(e => e.Id == id);
            
            if (organizationId != null)
            {
                predicate.And(e => e.CreatorId == organizationId);
            }

            return _eventRepository.EventsExistAsync(predicate);
        }

        public Task<bool> EventsExistAsync() => _eventRepository.EventsExistAsync();

        public Task<bool> EnrolledEventsExistAsync(string? userId) => _eventRepository.EventsExistAsync(e => e.EventEnrollments.Any(ee => ee.UserId == userId));

        public Task<bool> CreatedEventsExistAsync(string? organizationId) => _eventRepository.EventsExistAsync(e => e.CreatorId == organizationId);

        public async Task<Dictionary<string, string>> ValidateEvent(Event @event)
        {
            DateTime[] newDateTimes = { @event.EnrollStartTime, @event.EnrollEndTime, @event.EventStartTime, @event.EventEndTime };
            string[] propertyNames = { nameof(@event.EnrollStartTime), nameof(@event.EnrollEndTime), nameof(@event.EventStartTime), nameof(@event.EventEndTime) };
            Dictionary<string, string> errors = new();            

            var originalEvent = await GetEventByIdAsync(@event.Id);

            DateTime now = DateTime.Now;
            now = now.AddSeconds(-now.Second);

            for (int i = 0; i < propertyNames.Length; i++)
            {
                DateTime dateTime = (DateTime)typeof(Event).GetProperty(propertyNames[i])!.GetValue(originalEvent)!;
                if (dateTime > now && newDateTimes[i] <= now)
                {
                    errors[propertyNames[i]] = "The date and time should not be at the past";
                }
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            var error = ValidateEventDateTimePeriod(propertyNames, newDateTimes);
            if (error.HasValue)
            {
                var (errorMessage, memberNames) = error.Value;
                foreach (string memberName in memberNames)
                {
                    errors[memberName] = errorMessage;
                }
            }

            return errors;
        }

        public (string, List<string>)? ValidateEventDateTimePeriod(string[] propertyNames, DateTime[] dateTimes)
        {
            string enrollStartPropName = propertyNames[0];
            string enrollEndPropName = propertyNames[1];
            string eventStartPropName = propertyNames[2];
            string eventEndPropName = propertyNames[3];

            DateTime enrollStartTime = dateTimes[0];
            DateTime enrollEndTime = dateTimes[1];
            DateTime eventStartTime = dateTimes[2];
            DateTime eventEndTime = dateTimes[3];

            List<string> memberNames = new();

            if (enrollStartTime >= enrollEndTime)
            {
                memberNames.Add(enrollStartPropName);
                memberNames.Add(enrollEndPropName);
            }
            if (eventStartTime >= eventEndTime)
            {
                memberNames.Add(eventStartPropName);
                memberNames.Add(eventEndPropName);
            }

            if (memberNames.Count > 0)
            {
                return ("End time must be later than start time", memberNames);
            }

            if (eventStartTime <= enrollStartTime)
            {
                memberNames.Add(enrollStartPropName);
                memberNames.Add(enrollEndPropName);
                memberNames.Add(eventStartPropName);

                if (enrollEndTime >= eventEndTime)
                {
                    memberNames.Add(eventEndPropName);
                }
            }
            else if (eventStartTime <= enrollEndTime)
            {
                memberNames.Add(enrollEndPropName);
                memberNames.Add(eventStartPropName);

                if (enrollEndTime >= eventEndTime)
                {
                    memberNames.Add(eventEndPropName);
                }
            }

            if (memberNames.Count > 0)
            {
                return ("Event must start after the enrollment period ends", memberNames);
            }

            return null;
        }
    }
}
