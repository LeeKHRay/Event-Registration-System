using EventRegistrationSystem.Data;
using EventRegistrationSystem.Models.Events;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationSystem.Repositories
{
    public class EventCategoryRepository : IEventCategoryRepository
    {
        private readonly EventRegistrationSystemDbContext _context;

        public EventCategoryRepository(EventRegistrationSystemDbContext context)
        {
            _context = context;
        }

        public async Task InitializeCategoriesAsync()
        {
            if (!await _context.EventCategory.AnyAsync())
            {
                await _context.EventCategory.AddRangeAsync(new EventCategory[]
                {
                    new EventCategory
                    {
                        Name = "Drama"
                    },
                    new EventCategory
                    {
                        Name = "Music"
                    },
                    new EventCategory
                    {
                        Name = "Speech"
                    },
                    new EventCategory
                    {
                        Name = "Other"
                    }
                });

                await _context.SaveChangesAsync();
            }
        }

        public Task<List<EventCategory>> GetCategoriesAsync() => _context.EventCategory.ToListAsync();
    }
}
