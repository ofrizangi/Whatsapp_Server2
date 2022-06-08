
using WebWhatsappApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebWhatsappApi.Service
{

    public class ContactToAdd
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
    }


    public class ContactsGet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string Last { get; set; }
        public string Lastdate { get; set; }
    }

    public class Update
    {
        public string Name { get; set; }
        public string Server { get; set; }
    }

    public class LastMessage
    {
        public string Last { get; set; }
        public string LastTime { get; set; }
    }


    public class ContactService
    {
        public List<ContactsGet> getAllContacts(string userId)
        {
            using (var db = new WhatsappContext())
            {
                var q = db.Users.
                    Where(u => u.UserName == userId).    // only if you don't want all elements of Table1 
                    Select(u => u.Contacts.ToList()).ToList();

                var items = q[0];
                var newItems = new List<ContactsGet>();
                foreach (Contact i in items)
                {
                    LastMessage message = GetLastMessage(i, userId);
                    newItems.Add(new ContactsGet
                    {
                        Id = i.ContactUserName,
                        Name = i.ContactNickName,
                        Server = i.Server,
                        Last = message.Last,
                        Lastdate = message.LastTime
                    });
                  }
                return newItems;
            }
        }



        public Boolean AddToDB(string userId, ContactToAdd contact)
        {
            using (var db = new WhatsappContext())
            {
                var q = db.Users.
                    Where(u => u.UserName == userId).
                    Select(u => new
                    {
                        //UserName = u.UserName,
                        Contact = u.Contacts.Where(v => v.ContactUserName == contact.Id).
                        Select(v => new
                        {
                            ContactUserName = v.ContactUserName,
                            ContactNickName = v.ContactNickName
                        }).ToList(),

                    });

                var b = q.ToList()[0].Contact;


                if (b.Count == 0)
                {
                    Contact cont = new Contact();
                    try
                    {
                        cont.Id = db.Contacts.Max(x => x.Id) + 1;

                    } catch (Exception ex)
                    {
                        cont.Id = 0;

                    }
                    cont.ContactUserName = contact.Id;
                    if(contact.Name == "")
                    {
                        cont.ContactNickName = contact.Id;
                    }
                    else
                    {
                        cont.ContactNickName = contact.Name;
                    }
                    cont.Server = contact.Server;
                    cont.User = db.Users.FirstOrDefault(x => x.UserName == userId);

                    db.Contacts.Add(cont);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }


        private async Task<Contact> FindContact(string userId, string id)
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

        public async Task<ContactsGet> GetAContact(string id, string userId)
        {
            Contact c= await FindContact(userId, id);
            LastMessage m =  GetLastMessage(c, userId);

            return new ContactsGet
            {
                Id = c.ContactUserName,
                Name = c.ContactNickName,
                Server = c.Server,
                Last = m.Last,
                Lastdate = m.LastTime
            };
        }


        private LastMessage GetLastMessage(Contact c, string userId)
        {
            using (var db = new WhatsappContext())
            {
                LastMessage m = new LastMessage();
                // the messages can be sent by me r by my friend
                var q =  db.Contacts.Where(u => (u.ContactUserName == c.ContactUserName && u.User.UserName == userId)).
                Select(contact => contact.Messages.ToList()).ToList();
                if(q.Count == 0)
                {
                    m.Last = null;
                    m.LastTime = null;
                    return m;
                }
                DateTime last;
                try
                {
                    last = q[0].Max(x => x.Time);
                }
                catch (Exception ex)
                {
                    m.Last = null;
                    m.LastTime = null;
                    return m;

                }
                m.LastTime = last.ToString();

                m.Last = q[0].Find(a => a.Time == last).Content;
                return m;
            }

        }


        public async Task<Boolean> DeleteAContact(string id, string userId)
        {
            Contact item = await FindContact(userId, id);

            using (var db = new WhatsappContext())
            {
                if(item!= null)
                {
                    db.Contacts.Remove(item);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public async Task<Boolean> EditAContact(string id, string userId, Update body)
        {
            using (var db = new WhatsappContext())
            {
                var q = await db.Users.Where(u => u.UserName == userId).
                Select(user => user.Contacts.ToList()).ToListAsync();
                if (q.Count == 0)
                {
                    return false;
                }

                Contact item=  q[0].Find(contact => contact.ContactUserName == id);

                if (item != null)
                {
                    item.Server = body.Server;
                    item.ContactNickName = body.Name;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
