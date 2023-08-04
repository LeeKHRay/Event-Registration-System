using EventRegistrationSystem.Models.Events;

namespace EventRegistrationSystem.Repositories
{
    public interface IEventEnrollmentRepository
    {
        Task<bool> AddEventEnrollmentAsync(EventEnrollment eventEnrollment);
        Task<bool> DeleteEventEnrollmentAsync(int eventId, string userId);
        Task<bool> EventEnrollmentExistsAsync(int eventId, string userId);
        Task<int> CountEventEnrollmentAsync(int eventId);
    }
}