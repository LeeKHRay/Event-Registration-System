using EventRegistrationSystem.Data;
using EventRegistrationSystem.Models.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;

namespace EventRegistrationSystem.Repositories
{
    public class EventImageRepository : IEventImageRepository
    {
        private readonly EventRegistrationSystemDbContext _context;

        public EventImageRepository(EventRegistrationSystemDbContext context)
        {
            _context = context;
        }

        public Task<List<EventImage>> GetImagesAsync(Expression<Func<EventImage, bool>> predicate) => _context.EventImage.Where(predicate).ToListAsync();
    }
}
