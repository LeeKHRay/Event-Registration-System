using EventRegistrationSystem.Data;
using EventRegistrationSystem.Models.Events;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationSystem.Repositories
{
    public class EventEnrollmentRepository : IEventEnrollmentRepository
    {
        private readonly EventRegistrationSystemDbContext _context;

        public EventEnrollmentRepository(EventRegistrationSystemDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEventEnrollmentAsync(EventEnrollment eventEnrollment)
        {
            _context.EventEnrollment.Add(eventEnrollment);
            int count = await _context.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> DeleteEventEnrollmentAsync(int eventId, string userId)
        {
            // if no entities satisfy the condition, no entities will be deleted
            int count = await _context.EventEnrollment.Where(ee => ee.EventId == eventId && ee.UserId == userId).ExecuteDeleteAsync();
            return count > 0;
        }

        public Task<bool> EventEnrollmentExistsAsync(int eventId, string userId) =>
            _context.EventEnrollment.AnyAsync(ee => ee.EventId == eventId && ee.UserId == userId);

        public Task<int> CountEventEnrollmentAsync(int eventId) =>
            _context.EventEnrollment.CountAsync(ee => ee.EventId == eventId);
    }
}
