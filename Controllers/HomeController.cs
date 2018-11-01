using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Extensions;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;
using The_Wall_With_DotNet_Core.Extensions;
using The_Wall_With_DotNet_Core.Models;

namespace The_Wall_With_DotNet_Core.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext _dbContext;
        private User _user;
        private UserSessionWrapper _USW;
        public HomeController(MyDbContext context){
            _dbContext = context;
            _USW = new UserSessionWrapper(HttpContext);
        }

        [Route("/wall")]
        
        [ServiceFilter(typeof(LoggedInAttribute))]
        public IActionResult Index()
        {
            List<Message> vm = new List<Message>();

            vm = _dbContext.Messages
                .Include(m=>m.Comments)
                .Include(u=>u.User)
                .ToList();

            return View(vm);
        }

        [Route("/createmessage")]
        [ServiceFilter(typeof(LoggedInAttribute))]
        public IActionResult CreateMessage(Message message){
            message.UserId =  _USW.GetSessionUser();
            _dbContext.Add(message);
            _dbContext.SaveChanges();

            return View();
        }

        [Route("/createcomment")]
        [ServiceFilter(typeof(LoggedInAttribute))]
        public IActionResult CreateComment(Comment comment) {
            comment.UserId = _USW.GetSessionUser();
            _dbContext.Add(comment);
            _dbContext.SaveChanges();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
