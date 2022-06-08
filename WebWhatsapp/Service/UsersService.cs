
using WebWhatsappApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;


namespace WebWhatsappApi.Service
{
    public class UsersService
    {

        public User checkIfInDB(string UserName, string password)
        {
            using(var db = new WhatsappContext())
            {
                var q = db.Users.Where(u => u.UserName == UserName && u.Password == password);
                if(q.Any())
                {
                    return q.First();
                }
                return null;
            }
        }



        public Boolean checkIfNameExsit(string UserName)
            {
            using (var db = new WhatsappContext())
            {
                var q = db.Users.Where(u => u.UserName == UserName);
                if (q.Any())
                {
                    return true;
                }
                return false;
            }
        }

        public Boolean addUser(User user)
        {
            using (var db = new WhatsappContext())
            {
                var q = db.Users.Where(u => u.UserName == user.UserName);
                if (!q.Any())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }


        public List<User> getAllUsers()
        {
            using (var db = new WhatsappContext())
            {
                var items = db.Users.ToList();
                return items;
            }
        }






    }
}
