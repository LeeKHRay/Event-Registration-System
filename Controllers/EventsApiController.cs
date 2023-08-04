using EventRegistrationSystem.Filters;
using EventRegistrationSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationSystem.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsApiController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventImageService _eventImageService;

        public EventsApiController(IEventService eventService, IEventImageService eventImageService)
        {
            _eventService = eventService;
            _eventImageService = eventImageService;
        }

        // DELETE: Events/Delete/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "OrganizationUser")]
        [ServiceFilter(typeof(ValidateEventExistsFilter), Order = 1)]
        [ServiceFilter(typeof(AuthorizeEventFilter), Order = 2)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _eventImageService.DeleteImagesAsync(id);
            await _eventService.DeleteEventByIdAsync(id);
            return Ok();
        }
    }
}
