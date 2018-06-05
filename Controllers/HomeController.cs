using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WallProj.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("login")]
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        [Route("login/process")]
        public IActionResult ProcessLogin()
        {
            return View();
        }
        [HttpGet]
        [Route("register")]
        public IActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]
        [Route("register/process")]
        public IActionResult ProcessRegister()
        {
            return View();
        }
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
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
