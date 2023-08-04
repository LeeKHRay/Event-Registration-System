using EventRegistrationSystem.Models.Events;
using System.Linq.Expressions;

namespace EventRegistrationSystem.Repositories
{
    public interface IEventImageRepository
    {
        Task<List<EventImage>> GetImagesAsync(Expression<Func<EventImage, bool>> predicate);
    }
}
