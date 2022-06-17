
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebWhatsappApi;
using WebWhatsappApi.Service;
using WebWhatsappApi.Firebase;



namespace WebWhatsappApi.Controllers
{


    [ApiController]
    [Route("api/contacts/{id}/[controller]")]


    public class MessagesController : Controller
    {
        MessageService messageService = new MessageService();

        [Authorize]
        private string getUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.FindFirst("UserId").Value;
        }


        [Authorize]
        [HttpGet(Name = "GetMessages")]
        //public IEnumerable<MessagesGet> Get()
        public async Task<IActionResult> Get(string id)
        {
            var userId = getUserId();
            Contact c = await messageService.FindContact(userId,id);
            if (c == null) return NotFound();
            return Ok(messageService.getAllMessages(userId, id));
        }


        [Authorize]
        [HttpGet("{id2}")]
        public IActionResult GetSpecificMessage(string id, int id2)
        {
            var userId = getUserId();
            MessagesGet m = messageService.SpecificMessage(userId, id, id2);
            if(m == null)
            {
                return NotFound();
            }
            return Ok(m);
        }
    

        [Authorize]
        [HttpPost(Name = "NewMessages")]
        //[ValidateAntiForgeryToken]
        public IActionResult New(string id, MessagePost message)

        {
            if (message != null && ModelState.IsValid)
            {
                var userId = getUserId();
                //id is name of contact
                messageService.AddToDB(userId, message, id);
                //ClientFirebase.SendMessage();
                return Ok();
            }
            return BadRequest();
        }


        [Authorize]
        [HttpDelete("{id2}")]
        public void DeleteMessage(int id2)
        {       
            messageService.DeleteMessage(id2);
        }


        [Authorize]
        [HttpPut("{id2}")]
        public void UpdateMessage([FromRoute]int id2, [FromBody]MessagePost message)
        {
            messageService.UpdateMessage(id2, message);
        }


    }
}




