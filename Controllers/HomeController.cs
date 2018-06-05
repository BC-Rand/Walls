using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WallProj.Models;

namespace WallProj.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;
        public HomeController(UserContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            if (SessionId != null)
            {
                ViewBag.LoggedIn = true;
                ViewBag.Name = HttpContext.Session.GetString("name");
                ViewBag.Id = SessionId;
                ViewBag.Permission = HttpContext.Session.GetInt32("permission");
            }
            else
            {
                ViewBag.LoggedIn = false;
            }

            return View();
        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            if (SessionId != null)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.LoggedIn = false;
            }
            
            return View();
        }
        [HttpPost]
        [Route("login/process")]
        public IActionResult ProcessLogin(UserLogModel LoginAttempt)
        {
            
            return View("Login", LoginAttempt);
        }
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            if (SessionId != null)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.LoggedIn = false;
            }

            return View();
        }
        [HttpPost]
        [Route("register/process")]
        public IActionResult ProcessRegister(UserRegModel RegisterAttempt)
        {
            if (ModelState.IsValid)
            {
                User EmailCheck = _context.users.SingleOrDefault(user => user.Email == RegisterAttempt.Email);
                if (EmailCheck == null)
                {
                    User NewUser = new User{
                        FirstName = RegisterAttempt.FirstName,
                        LastName = RegisterAttempt.LastName,
                        Email = RegisterAttempt.Email,
                        Description = RegisterAttempt.Description,
                        Password = RegisterAttempt.Password,
                        PermissionLevel = 0
                    };
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                    _context.users.Add(NewUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("id", NewUser.idUser);
                    HttpContext.Session.SetString("name", NewUser.FirstName+' '+NewUser.LastName);
                    HttpContext.Session.SetInt32("permission", NewUser.PermissionLevel);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("Email", "Please try another email address");
                }
            }
            return View("Register", RegisterAttempt);
        }
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            if (SessionId != null)
            {
                ViewBag.LoggedIn = true;
                ViewBag.Name = HttpContext.Session.GetString("name");
                ViewBag.Id = SessionId;
                ViewBag.Permission = HttpContext.Session.GetInt32("permission");
            }
            else
            {
                ViewBag.LoggedIn = false;
            }

            return View();
        }
        [HttpGet]
        [Route("users/show/{id}")]
        public IActionResult UserPage(int? UserId)
        {
            return View();
        }
        [HttpPost]
        [Route("users/comment/process")]
        public IActionResult ProcessComment()
        {
            return View();
        }
        [HttpPost]
        [Route("users/reply/process")]
        public IActionResult ProcessReply()
        {
            return View();
        }
        [HttpGet]
        [Route("users/edit")]
        public IActionResult EditCurrentUser()
        {
            return View();
        }
        [HttpPost]
        [Route("users/edit/process")]
        public IActionResult ProcessEditCurrentUser()
        {
            return View();
        }
        // ADMIN ROUTES
        [HttpGet]
        [Route("admin/edit/{id}")]
        public IActionResult AdminEditUser(int? UserId)
        {
            return View();
        }
        [HttpPost]
        [Route("admin/edit/process")]
        public IActionResult AdminProcessEditUser()
        {
            return View();
        }




        // public IActionResult About()
        // {
        //     ViewData["Message"] = "Your application description page.";

        //     return View();
        // }

        // public IActionResult Contact()
        // {
        //     ViewData["Message"] = "Your contact page.";

        //     return View();
        // }

        public IActionResult Error()
        {
            return View();
        }
    }
}
