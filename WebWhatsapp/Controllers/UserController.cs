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

        [Authorize]
        [HttpGet(Name = "GetUser")]
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

        
        [HttpPost(Name = "LoginUser")]
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
        public IActionResult Register(User user)
        {
            Boolean register = false;
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    Boolean isInDB = usersService.checkIfNameExsit(user.UserName);
                    if (isInDB == false)
                    {
                        usersService.addUser(user);
                        return Ok(CreateToken(user.UserName));
                    }

                }
            }
            return Ok(register);
        }
        

        //private async void SignIn(User user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName)
        //    };
        //    var claimsIdentity = new ClaimsIdentity(
        //        claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    var authProperties = new AuthenticationProperties { };


        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        new ClaimsPrincipal(claimsIdentity),
        //        authProperties);
        //}



        //private readonly  _context;

        //public UsersController(WebWhatsappApiContext context)
        //{
        //    _context = context;
        //}

        // GET: Users
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Movie != null ? 
        //                  View(await _context.Movie.ToListAsync()) :
        //                  Problem("Entity set 'WebWhatsappApiContext.User'  is null.");
        //}

        // GET: Users/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null || _context.User == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.UserName == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// GET: Users/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Users/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserName,Password,NickName,Image")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null || _context.User == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("UserName,Password,NickName,Image")] User user)
        //{
        //    if (id != user.UserName)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.UserName))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.User == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.UserName == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_context.User == null)
        //    {
        //        return Problem("Entity set 'WebWhatsappApiContext.User'  is null.");
        //    }
        //    var user = await _context.User.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.User.Remove(user);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(string id)
        //{
        //    return (_context.User?.Any(e => e.UserName == id)).GetValueOrDefault();
        //}
    }
}
