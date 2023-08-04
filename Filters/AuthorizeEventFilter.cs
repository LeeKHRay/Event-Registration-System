using EventRegistrationSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EventRegistrationSystem.Filters
{
    public class AuthorizeEventFilter : IAsyncActionFilter
    {
        private readonly IEventService _eventService;

        public AuthorizeEventFilter(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = (int)context.ActionArguments["id"]!;
            string organizationId = context.HttpContext.User!.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (!await _eventService.EventExistsAsync(id, organizationId))
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
