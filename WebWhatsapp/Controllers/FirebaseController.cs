using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebWhatsappApi.Firebase;

namespace WebWhatsappApi.Controllers
{

    public class TokenUser
    {
        public string token { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class FireBaseController : Controller
    {
 


        [Authorize]
        [HttpPost]
        public IActionResult getToken(TokenUser tokenAppliction)
        {

            if (tokenAppliction != null && ModelState.IsValid)
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var userId = identity.FindFirst("UserId").Value;
                ClientFirebase.addUserWithToken(userId, tokenAppliction.token);
                //ClientFirebase.fireBase.Add(userId, tokenAppliction.token);
                return Ok();
            }
            return BadRequest();
        }
    }
}

