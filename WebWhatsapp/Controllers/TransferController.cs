using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebWhatsappApi.Service;
using WebWhatsappApi.Models;


namespace WebWhatsappApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class TransferController : Controller
    {
        TransferService transferService = new TransferService();

        [HttpPost(Name = "AddMessageTransfer")]
        public IActionResult AddMessage(Transfer transfer)
        {
            if (transfer != null && ModelState.IsValid)
            {
                transferService.AddToDB(transfer);
                return Ok();
            }
            return BadRequest();
        }
    }
}




