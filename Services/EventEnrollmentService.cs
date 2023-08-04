using EventRegistrationSystem.Models.Events;
using EventRegistrationSystem.Repositories;

namespace EventRegistrationSystem.Services
{
    public class EventEnrollmentService : IEventEnrollmentService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventEnrollmentRepository _eventEnrollmentRepository;

        public EventEnrollmentService(IEventRepository eventService, IEventEnrollmentRepository eventEnrollmentRepository)
        {
            _eventRepository = eventService;
            _eventEnrollmentRepository = eventEnrollmentRepository;
        }

        public async Task<bool> EnrollEventAsync(int eventId, string userId)
        {
            if (await UserEnrolledInEvent(eventId, userId))
            {
                return false;
            }

            var @event = await _eventRepository.GetEventByIdAsync(eventId);
            if (@event == null || !await EventAvailable(@event))
            {
                return false;
            }

            EventEnrollment eventEnrollment = new()
            {
                EventId = eventId,
                UserId = userId
            };

            return await _eventEnrollmentRepository.AddEventEnrollmentAsync(eventEnrollment);
        }

        public Task<bool> DeleteEventEnrollmentAsync(int eventId, string userId) => 
            _eventEnrollmentRepository.DeleteEventEnrollmentAsync(eventId, userId);

        public Task<bool> UserEnrolledInEvent(int eventId, string userId) =>
            _eventEnrollmentRepository.EventEnrollmentExistsAsync(eventId, userId);

        public async Task<bool> UserEnrolledInEvent(Event @event, string userId)
        {
            if (@event.EventEnrollments == null) { 
                return await UserEnrolledInEvent(@event.Id, userId);
            }
            return @event.EventEnrollments.Any(ee => ee.UserId == userId);
        }

        public async Task<bool> EventAvailable(Event @event)
        {
            if (DateTime.Now < @event.EnrollStartTime || DateTime.Now > @event.EnrollEndTime)
            {
                return false;
            }

            int applicantNum = @event.EventEnrollments?.Count ?? await _eventEnrollmentRepository.CountEventEnrollmentAsync(@event.Id);

            return applicantNum < @event.Quota;
        }
    }
}
