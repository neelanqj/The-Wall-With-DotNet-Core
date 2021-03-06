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
        private const string mainPortal = "/wall";
        private const string loginPage ="/";
        private const string loginView ="Index";
        private MyDbContext _dbContext;
        private UserSessionWrapper _USW;

        
        public AuthenticationController(MyDbContext context){
            _dbContext=context;
            _USW = new UserSessionWrapper();
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index(){
            return View();
        }


        [HttpPost]
        [Route("/register")]
        public IActionResult Register(User user)
        {
            // Check initial ModelState
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(_dbContext.Users.Any(u => u.Email == user.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    
                    // You may consider returning to the View at this point
                    return View(loginView);
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                User newUser =_dbContext.Add(user).Entity;
                _dbContext.SaveChanges();

                _USW.SetSessionUser(newUser.UserId, HttpContext);
                
                return Redirect(mainPortal);
            }

            // other code
            return View(loginView);
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
                    return View(loginView);
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return View(loginView);
                }
                
                HttpContext.Session.SetInt32("User", userInDb.UserId);
                return Redirect(mainPortal);
            }

            return View(loginView);
        }

        [Route("/logout")]
        [ServiceFilter(typeof(LoggedInAttribute))]
        public IActionResult Logout(){
            HttpContext.Session.Remove("User");
            return Redirect(loginPage);
        }
        
    }
}