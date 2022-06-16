using Microsoft.AspNetCore.Mvc;
using WebWhatsappApi.Service;
using WebWhatsappApi.Models;
using FirebaseAdmin;

namespace WebWhatsappApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : Controller
    {

        ContactService contactsService = new ContactService();
        
        [HttpPost]
        public IActionResult Invitations(Invitation invitation)
        {
            ContactToAdd contactToAdd = new ContactToAdd();
            contactToAdd.Id = invitation.from;
            contactToAdd.Server = invitation.server;

            contactToAdd.Name = invitation.from;

            string userId = invitation.to;

            Boolean added = contactsService.AddToDB(userId, contactToAdd);

            if (added)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
