using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebWhatsappApi.Service;
using WebWhatsappApi.Models;
using WebWhatsappApi.Firebase;

namespace WebWhatsappApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class TransferController : Controller
    {
        TransferService transferService = new TransferService();
        ClientFirebase firebase;

        public TransferController()
        {
            firebase = ClientFirebase.GetFirebase();
        }

        [HttpPost(Name = "AddMessageTransfer")]
        public IActionResult AddMessage(Transfer transfer)
        {
            if (transfer != null && ModelState.IsValid)
            {
                transferService.AddToDB(transfer);
                MessagePost m = new MessagePost();
                m.Content = transfer.Content;
                firebase.SendMessage(transfer.From, m, transfer.To);
                return Ok();
            }
            return BadRequest();
        }
    }
}




