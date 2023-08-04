using System.ComponentModel.DataAnnotations;

namespace EventRegistrationSystem.DataAnnotations
{
    public class EventTimeDisplayFormatAttribute : DisplayFormatAttribute
    {
        public EventTimeDisplayFormatAttribute()
        {
            DataFormatString = "{0:yyyy-MM-dd(dddd) hh:mm}";
        }
    }
}
