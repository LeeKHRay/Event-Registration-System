using EventRegistrationSystem.Models.Events;
using EventRegistrationSystem.Repositories;
using LinqKit;

namespace EventRegistrationSystem.Services
{
    public class EventImageService : IEventImageService
    {
        private readonly IEventImageRepository _eventImageRepository;
        private readonly string uploadFolder;
        private readonly HashSet<string> validImageTypes;

        public EventImageService(IWebHostEnvironment env, IEventImageRepository eventImageRepository)
        {
            _eventImageRepository = eventImageRepository;
            uploadFolder = Path.Combine(env.WebRootPath, @"images\upload\events");
            validImageTypes = new() { ".jpg", ".jpeg", ".png" };
        }

        public Task<List<EventImage>> GetImagesAsync(int eventId, List<int>? ids)
        {
            var predicate = PredicateBuilder.New<EventImage>(img => img.EventId == eventId);

            if (ids != null)
            {
                predicate.And(e => ids.Contains(e.Id));
            } 

            return _eventImageRepository.GetImagesAsync(predicate);
        }

        public async Task<List<EventImage>> UploadImagesAsync(List<IFormFile>? formFiles)
        {
            List<EventImage> eventImages = new();

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            if (formFiles != null)
            {
                foreach (var formFile in formFiles)
                {
                    if (formFile.Length > 0 && validImageTypes.Contains(Path.GetExtension(formFile.FileName)))
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                        string filePath = Path.Combine(uploadFolder, fileName);

                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await formFile.CopyToAsync(fileStream);

                        eventImages.Add(new()
                        {
                            FileName = fileName
                        });
                    }
                }
            }

            return eventImages;
        }

        public async Task DeleteImagesAsync(int eventId, List<int>? ids)
        {
            var predicate = PredicateBuilder.New<EventImage>(img => img.EventId == eventId);

            if (ids != null)
            {
                predicate.And(img => !ids.Contains(img.Id));
            }

            var images = await _eventImageRepository.GetImagesAsync(predicate);

            foreach (var image in images)
            {
                string filePath = Path.Combine(uploadFolder, image.FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
