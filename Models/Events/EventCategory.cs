using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.Models.Events
{
    public class EventCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public ICollection<Event> Events { get; set; } = default!;
    }
}
