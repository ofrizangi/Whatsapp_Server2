using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebWhatsappApi;
using WebWhatsappApi.Service;


namespace WebWhatsapp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {

        UsersService usersService = new UsersService();
        public class UserLogin
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }


        public IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public IEnumerable<User> Get()
        {
            var list = usersService.getAllUsers();
            return list.ToList();

        }

        //public IActionResult Get()
        //{
        //    var list = usersService.getAllUsers();
        //    Ok(list.ToList().Select(e => new { id = e.UserName, }));

        //}

        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(UserLogin user)
        {
            Boolean singed = false;
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    User myUser = usersService.checkIfInDB(user.UserName, user.Password);
                    if (myUser != null)
                    {
                        return Ok(CreateToken(user.UserName));
                    }
       
                }
            }
            return Ok(false);
        }
        
        
        private string CreateToken(string userName)
        {
            var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWTParams:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("UserId", userName),
                        };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTParams:SecretKey"]));
            var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JWTParams:Issuer"],
                _configuration["JWTParams:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(50),
                signingCredentials: mac);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //[HttpPost(Name = "LogoutUser")]
        //public void Logout()
        //{
        //    HttpContext.SignOutAsync();
        //}


        [HttpPost(Name = "RegisterUser")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            Boolean register = false;
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    Boolean isInDB = await usersService.checkIfNameExsit(user.UserName);
                    if (isInDB == false)
                    {
                        await usersService.addUser(user);
                        string token = CreateToken(user.UserName);
                        return Ok(token);
                    }

                }
            }
            return Ok(register);
        }
        
    }
}
