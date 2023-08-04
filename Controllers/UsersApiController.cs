using EventRegistrationSystem.Extensions;
using EventRegistrationSystem.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationSystem.Controllers
{
    [Route("api/users/[action]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        public UsersApiController()
        {

        }

        // POST: users/preference
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Preference([FromBody] UserPreferences pref)
        {
            HttpContext.Session.SetObjectAsJson("preferences", pref);

            return Ok(new { pref.Color });
        }
    }
}
