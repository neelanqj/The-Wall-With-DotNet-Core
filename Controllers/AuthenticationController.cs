using System.Linq;
using Extensions;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistance;
using The_Wall_With_DotNet_Core.Extensions;

namespace Extensions.Authentication
{
    public class AuthenticationController : Controller
    {
        private MyDbContext _dbContext;
        private UserSessionWrapper _USW;
        
        public AuthenticationController(MyDbContext context){
            _dbContext=context;
            _USW = new UserSessionWrapper(HttpContext);
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = _dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                if(userInDb == null || userSubmission.Password == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View("Index");
                }
                
                HttpContext.Session.SetInt32("User", userInDb.UserId);
                return Redirect("/dashboard");
            }

            return View("Index");
        }

        [Route("/logout")]
        [ServiceFilter(typeof(LoggedInAttribute))]
        public IActionResult Logout(){
            HttpContext.Session.Remove("User");
            return Redirect("/login");
        }
        
    }
}