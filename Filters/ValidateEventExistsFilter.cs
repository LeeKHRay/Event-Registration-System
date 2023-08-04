using EventRegistrationSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventRegistrationSystem.Filters
{
    public class ValidateEventExistsFilter : IAsyncActionFilter
    {
        private readonly IEventService _eventService;

        public ValidateEventExistsFilter(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.ContainsKey("id"))
            {
                context.Result = new NotFoundObjectResult("Missing event ID"); // status code 404 with data/message
                return;
            }

            int? id = (int?)context.ActionArguments["id"];
            if (id == null)
            {
                context.Result = new NotFoundObjectResult("Missing event ID");
                return;
            }

            if (id <= 0)
            {
                context.Result = new NotFoundObjectResult("Event does not exist");
                return;
            }

            if (!await _eventService.EventExistsAsync(id.Value))
            {
                context.Result = new NotFoundObjectResult("Event does not exist");
                return;
            }

            await next();
        }
    }
}
