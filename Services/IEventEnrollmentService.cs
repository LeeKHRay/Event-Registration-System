using EventRegistrationSystem.Models.Events;

namespace EventRegistrationSystem.Services
{
    public interface IEventEnrollmentService
    {
        Task<bool> EnrollEventAsync(int eventId, string userId);
        Task<bool> DeleteEventEnrollmentAsync(int eventId, string userId);
        Task<bool> UserEnrolledInEvent(int eventId, string userId);
        Task<bool> UserEnrolledInEvent(Event @event, string userId);
        Task<bool> EventAvailable(Event @event);
    }
}
