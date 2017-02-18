using System.Linq;
using Microsoft.AspNetCore.Mvc;
using blackBelt.Models;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

namespace blackBelt.Controllers
{
    public class HomeController : Controller
    {
         public PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
         private MainContext _context;
         public HomeController(MainContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {

            return View();
        }
        //Register a User
        [HttpPost]
        [Route("/Register")]
        public IActionResult Register(RegisterViewModel model)
        {
              if(ModelState.IsValid)
            {
                User ifExists = _context.Users.Where(user => user.FirstName == model.FirstName).FirstOrDefault();
                if(ifExists == null){
                User userInstance = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Password = Hasher.HashPassword(model, model.Password),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Wallet = 1000
                };
                _context.Users.Add(userInstance);
                _context.SaveChanges();
                ViewData["Success"] = "Thanks for Registering!! Please Login!";
                return View("Index");
            }
            ViewData["Success"] = "Hm, a user is already registered at this name.";
            return View("Index");
            }
            return View("Index", model);
        }
        //Login
        [HttpPost]
        [Route("/Login")]
        public IActionResult login(string UserName, string password){
        
           PasswordHasher<User> userHasher = new PasswordHasher<User>();
           User userLoggingInstance = _context.Users.SingleOrDefault(user => user.UserName == UserName );
           if(userLoggingInstance != null && password != null)
           {

            if(0 != userHasher.VerifyHashedPassword(userLoggingInstance, userLoggingInstance.Password, password))
            {
               HttpContext.Session.SetString("userFirstName", userLoggingInstance.FirstName);
               HttpContext.Session.SetInt32("userID", userLoggingInstance.Id);
               return Redirect("/Main"); 
            }
            }
            ViewData["Success"] = "Something failed on login";
            return View("Index");
        }
         [HttpGet]
        [Route("/logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

    }
}
