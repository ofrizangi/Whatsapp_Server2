
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



        public async Task<Boolean> checkIfNameExsit(string UserName)
            {
            using (var db = new WhatsappContext())
            {
                var q = await db.Users.Where(u => u.UserName == UserName).ToListAsync();
                if (q.Any())
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<Boolean> addUser(User user)
        {
            using (var db = new WhatsappContext())
            {
                var q = await db.Users.Where(u => u.UserName == user.UserName).ToListAsync();
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
