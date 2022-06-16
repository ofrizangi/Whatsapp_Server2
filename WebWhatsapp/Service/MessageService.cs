
using WebWhatsappApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebWhatsappApi.Service
{

    public class MessagePost
    {
        public string Content { get; set; }
    }

    public class MessagesGet
    {
        public int Id { get; set; }
        public string Content { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        public Boolean Sent { get; set; }
    }
    public class PostModel
    {
        public int ID { get; set; }
        public MessagePost message { get; set; }
    }

    public class MessageService
    {
        public List<MessagesGet> getAllMessages(string userId, string contactName)
        {
            using (var db = new WhatsappContext())
            {
                var q = db.Contacts.
                    Where(x => x.User.UserName == userId && x.ContactUserName == contactName).
                    Select(u => u.Messages.
                        Select(v => new MessagesGet
                        {
                            Id = v.Id,
                            Content = v.Content,
                            Created = v.Time,
                            Sent = v.Sent
                        }).ToList()
                    );
                ;
                ;

                var items = q.ToList()[0];
                return items;
            }
        }


        public MessagesGet SpecificMessage(string userId, string contactName, int idMessage)
        {
            List<MessagesGet> list = getAllMessages(userId, contactName);
            MessagesGet m = list.Find(x => x.Id == idMessage);
            return m;
        }

        public async Task<Contact> FindContact(string userId, string id)
        {
            using (var db = new WhatsappContext())
            {
                var q = await db.Users.Where(u => u.UserName == userId).
                Select(user => user.Contacts.ToList()).ToListAsync();
                if (q.Count == 0)
                {
                    return null;
                }

                return q[0].Find(contact => contact.ContactUserName == id);
            }

        }



        public void AddToDB(string userId, MessagePost message, string contactName)
        {
            using (var db = new WhatsappContext())
            {
                var a = db.Contacts.FirstOrDefault(x => x.User.UserName == userId && x.ContactUserName == contactName);
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
                newMessage.Sent = true;
                newMessage.Content = message.Content;
                newMessage.Contact = db.Contacts.FirstOrDefault(x => x.User.UserName == userId && x.ContactUserName == contactName);

                db.Messages.Add(newMessage);
                db.SaveChanges();
                
            }
        }



        public void DeleteMessage(int idMessage)
        {
            using (var db = new WhatsappContext())
            {

                Messages massage = db.Messages.Find(idMessage);
                if (massage != null)
                {
                    db.Messages.Remove(massage);
                    db.SaveChanges();
                }

            }

        }


        public void UpdateMessage(int idMessage, MessagePost Updatemessage)
        {
            using (var db = new WhatsappContext())
            {

                Messages? massage = db.Messages.Find(idMessage);
                if (massage != null)
                {
                    massage.Content = Updatemessage.Content;
                    db.SaveChanges();
                }

            }

        }
        
        
    }
}


