
using WebWhatsappApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebWhatsappApi.Service
{




    public class TransferService
    {
        public void AddToDB(Transfer transfer)
        {
            using (var db = new WhatsappContext())
            {

                Messages newMessage = new Messages();
                try
                {
                    newMessage.Id = db.Messages.Max(x => x.Id) + 1;

                }
                catch (Exception ex)
                {
                    newMessage.Id = 0;

                }
                newMessage.Time = DateTime.Now;
                newMessage.Sent = false;
                newMessage.Content = transfer.Content;
                newMessage.Contact = db.Contacts.FirstOrDefault(x => x.User.UserName == transfer.To && x.ContactUserName == transfer.From);

                db.Messages.Add(newMessage);
                db.SaveChanges();

            }
        }



   


    }
}


