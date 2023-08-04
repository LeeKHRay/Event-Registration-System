using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventRegistrationSystem.Filters
{
    public class SetEventsDisplayTypeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string? displayType = context.HttpContext.Request.Cookies["DisplayType"];
            if (displayType != "Grid" && displayType != "List")
            {
                displayType = "Grid";
                context.HttpContext.Response.Cookies.Append("DisplayType", displayType); // session cookie
            }

            ((Controller)context.Controller).ViewBag.DisplayType = displayType;
        }
    }
}
