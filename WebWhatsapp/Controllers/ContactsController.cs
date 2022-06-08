using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebWhatsappApi;
using WebWhatsappApi.Service;
using System.Text.Json;

namespace WebWhatsapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        ContactService contactsService = new ContactService();


        [Authorize]
        [HttpGet(Name = "GetContacts")]
        public IActionResult GetAll()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst("UserId").Value;
            List<ContactsGet> c = contactsService.getAllContacts(userId);
            return Ok(contactsService.getAllContacts(userId));
        }



        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst("UserId").Value;
            ContactsGet cont = await contactsService.GetAContact(id, userId);
            if (cont == null)
            {
                return NotFound();
            }
            return Ok(cont);
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst("UserId").Value;
            Boolean deleted = await contactsService.DeleteAContact(id, userId);
            if (deleted)
            {
                return Ok();
            }
            return NotFound();
        }



        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutById([FromRoute] string id, [FromBody] Update body)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst("UserId").Value;
            Boolean changed = await contactsService.EditAContact(id, userId, body);
            if (changed)
            {
                return Ok();
            }
            return NotFound();
        }


        [Authorize]
        [HttpPost(Name = "AddContacts")]
        //[ValidateAntiForgeryToken]
        public IActionResult AddContact(ContactToAdd contact)
        {
            if (contact != null)
            {
                if (ModelState.IsValid)
                {
                    //int id = HttpContext.Current.User.Identify.
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    var userId = identity.FindFirst("UserId").Value;

                    Boolean isInDB = contactsService.AddToDB(userId, contact);
                    if (isInDB)
                    {
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }

    }
}




