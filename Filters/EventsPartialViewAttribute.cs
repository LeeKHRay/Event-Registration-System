using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventRegistrationSystem.Filters
{
    public class EventsPartialViewAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("displayType"))
            {
                context.Result = new BadRequestObjectResult("Missing displayType");
                return;
            }

            string? displayType = (string?)context.ActionArguments["displayType"];
            if (displayType != "Grid" && displayType != "List")
            {
                context.Result = new BadRequestObjectResult("Invalid displayType");
                return;
            }

            context.HttpContext.Response.Cookies.Append("DisplayType", displayType);
        }
    }
}
