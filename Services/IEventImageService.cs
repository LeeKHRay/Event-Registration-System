using EventRegistrationSystem.Models.Events;

namespace EventRegistrationSystem.Services
{
    public interface IEventImageService
    {
        Task<List<EventImage>> GetImagesAsync(int eventId, List<int>? ids = null);
        Task<List<EventImage>> UploadImagesAsync(List<IFormFile>? formFiles);
        Task DeleteImagesAsync(int eventId, List<int>? ids = null); // delete images in hard disk
    }
}
