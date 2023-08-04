namespace EventRegistrationSystem.Models.Events
{
    public class EventImage
    {
        public int Id { get; set; }
        public string FileName { get; set; } = default!;
        public DateTime UploadTime { get; set; } = DateTime.Now;

        public int EventId { get; set; }
        public Event Event { get; set; } = default!;
    }
}
