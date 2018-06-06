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
            if (ModelState.IsValid)
            {
                User EmailCheck = _context.users.SingleOrDefault(user => user.Email == LoginAttempt.Email);
                if (EmailCheck == null)
                {
                    ModelState.AddModelError("Email", "Invalid login attempt");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    
                    if (0 != Hasher.VerifyHashedPassword(EmailCheck, EmailCheck.Password, LoginAttempt.Password))
                    {
                        HttpContext.Session.SetInt32("id", EmailCheck.UserId);
                        HttpContext.Session.SetString("name", EmailCheck.FirstName+' '+EmailCheck.LastName);
                        HttpContext.Session.SetInt32("permission", EmailCheck.PermissionLevel);
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Invalid login attempt");
                    }
                }
            }

            ViewBag.LoggedIn = false;
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
                    HttpContext.Session.SetInt32("id", NewUser.UserId);
                    HttpContext.Session.SetString("name", NewUser.FirstName+' '+NewUser.LastName);
                    HttpContext.Session.SetInt32("permission", NewUser.PermissionLevel);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("Email", "Please try another email address");
                }
            }

            ViewBag.LoggedIn = false;
            return View("Register", RegisterAttempt);
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
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

            List<User> AllUsers = _context.users.Where(user => user.UserId > 0).ToList();
            ViewBag.AllUsers = AllUsers;
            
            return View(AllUsers);
        }
        [HttpGet]
        [Route("users/show/{UserId}")]
        public IActionResult UserPage(int? UserId)
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
                return RedirectToAction("Login");
            }

            
            if (UserId != null)
            {
                User UserCheck = _context.users
                    .Include(user => user.ReceivedComments).ThenInclude(comment => comment.Commenter)
                    .Include(user => user.ReceivedComments).ThenInclude(comment => comment.Replies).ThenInclude(reply => reply.Poster)
                    .SingleOrDefault(user => user.UserId == UserId);
                if (UserCheck != null)
                {
                    ViewBag.UserFirstName = UserCheck.FirstName;
                    ViewBag.UserLastName = UserCheck.LastName;
                    ViewBag.UserDescription = UserCheck.Description;
                    ViewBag.UserEmail = UserCheck.Email;
                    ViewBag.UserCreatedAt = UserCheck.CreatedAt;

                    UserCheck.ReceivedComments.Sort( (Comment1, Comment2) => -1 * Comment1.CreatedAt.CompareTo(Comment2.CreatedAt) );
                    foreach (Comment comment in UserCheck.ReceivedComments)
                    {
                        comment.Replies.Sort( (Reply1, Reply2) => -1 * Reply1.CreatedAt.CompareTo(Reply2.CreatedAt) );
                    }

                    return View(UserCheck);
                }
            }
            
            return View("UserPageError");
        }
        [HttpPost]
        [Route("users/comment/process")]
        public IActionResult ProcessComment(string Comment, string Recipient)
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
                return RedirectToAction("Login");
            }
            
            int RecipientId;
            if (Int32.TryParse(Recipient, out RecipientId) && Comment != null && Comment.Length <= 500)
            {
                Comment NewComment = new Comment{
                    CommenterUserId = SessionId.Value,
                    RecipientUserId = RecipientId,
                    Text = Comment
                };
                _context.comments.Add(NewComment);
                _context.SaveChanges();

                return RedirectToAction("UserPage", new { UserId = RecipientId });
            }
            else
            {
                return RedirectToAction("Dashboard");
            }

        }
        [HttpPost]
        [Route("users/reply/process")]
        public IActionResult ProcessReply(string Reply, string Parent, string Recipient)
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
                return RedirectToAction("Login");
            }

            int ParentId;
            if (Int32.TryParse(Parent, out ParentId) && Reply.Length >= 3 && Reply.Length <= 500)
            {
                Reply NewReply = new Reply{
                    PosterUserId = SessionId.Value,
                    ParentCommentId = ParentId,
                    Text = Reply
                };
                _context.replies.Add(NewReply);
                _context.SaveChanges();
            }

            int RecipientId;
            if(Int32.TryParse(Recipient, out RecipientId))
            {
                return RedirectToAction("UserPage", new { UserId = RecipientId });
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }
        [HttpGet]
        [Route("users/edit")]
        public IActionResult EditCurrentUser()
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
                return RedirectToAction("Login");
            }

            User CurrentUser = _context.users.SingleOrDefault(user => user.UserId == SessionId);

            ViewBag.EditFirstName = CurrentUser.FirstName;
            ViewBag.EditLastName = CurrentUser.LastName;
            ViewBag.EditEmail = CurrentUser.Email;
            ViewBag.EditDescription = CurrentUser.Description;


            return View();
        }
        [HttpPost]
        [Route("users/edit/process")]
        public IActionResult ProcessEditCurrentUser(UserEditModel EditAttempt)
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            if (SessionId != null)
            {
                User LoggedUser = _context.users.SingleOrDefault(user => user.UserId == SessionId);

                if (ModelState.IsValid)
                {
                    LoggedUser.Email = EditAttempt.Email;
                    LoggedUser.FirstName = EditAttempt.FirstName;
                    LoggedUser.LastName = EditAttempt.LastName;
                    LoggedUser.Description = EditAttempt.Description;
                    
                    _context.SaveChanges();
                    
                    return RedirectToAction("UserPage", new { UserId = SessionId });
                }
                else
                {
                    ViewBag.LoggedIn = true;
                    ViewBag.Name = HttpContext.Session.GetString("name");
                    ViewBag.Id = SessionId;
                    ViewBag.Permission = HttpContext.Session.GetInt32("permission");

                    ViewBag.EditFirstName = LoggedUser.FirstName;
                    ViewBag.EditLastName = LoggedUser.LastName;
                    ViewBag.EditEmail = LoggedUser.Email;
                    ViewBag.EditDescription = LoggedUser.Description;
                    return View("EditCurrentUser", EditAttempt);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }
        [HttpPost]
        [Route("users/edit/password")]
        public IActionResult ProcessPasswordCurrentUser(string Password, string PasswordConfirm)
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            if (SessionId != null)
            {
                if (Password == PasswordConfirm && Password.Length >= 8)
                {
                    User LoggedUser = _context.users.SingleOrDefault(user => user.UserId == SessionId);
                    
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    LoggedUser.Password = Hasher.HashPassword(LoggedUser, Password);

                    _context.SaveChanges();

                    return RedirectToAction("UserPage", new { UserId = SessionId });
                }
                else
                {
                    // Add error messages for password not changing
                    return View("EditCurrentUser");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        // ADMIN ROUTES
        [HttpGet]
        [Route("admin/edit/{UserId}")]
        public IActionResult AdminEditUser(int? UserId)
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            int? Permission = HttpContext.Session.GetInt32("permission");
            if (SessionId != null && Permission != null && Permission == 9)
            {
                ViewBag.LoggedIn = true;
                ViewBag.Name = HttpContext.Session.GetString("name");
                ViewBag.Id = SessionId;
                ViewBag.Permission = Permission;
            }
            else
            {
                return RedirectToAction("Dashboard");
            }

            User UserLookup = _context.users.SingleOrDefault(user => user.UserId == UserId);

            if (UserLookup != null)
            {
                ViewBag.EditFirstName = UserLookup.FirstName;
                ViewBag.EditLastName = UserLookup.LastName;
                ViewBag.EditEmail = UserLookup.Email;
                ViewBag.EditDescription = UserLookup.Description;
                ViewBag.EditPermissionLevel = UserLookup.PermissionLevel;
                ViewBag.EditUserId = UserId;

                return View();
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
        }
        [HttpPost]
        [Route("admin/edit/process")]
        public IActionResult AdminProcessEditUser(AdminEditModel AdminEditAttempt)
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            int? Permission = HttpContext.Session.GetInt32("permission");
            if (SessionId != null && Permission != null && Permission == 9)
            {
                ViewBag.LoggedIn = true;
                ViewBag.Name = HttpContext.Session.GetString("name");
                ViewBag.Id = SessionId;
                ViewBag.Permission = Permission;
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
                        
            if (ModelState.IsValid && (AdminEditAttempt.PermissionLevel == 0 || AdminEditAttempt.PermissionLevel == 9))
            {
                User EditUser = _context.users.SingleOrDefault(user => user.UserId == AdminEditAttempt.UserId);
                
                EditUser.FirstName = AdminEditAttempt.FirstName;
                EditUser.LastName = AdminEditAttempt.LastName;
                EditUser.Email = AdminEditAttempt.Email;
                EditUser.Description = AdminEditAttempt.Description;
                EditUser.PermissionLevel = AdminEditAttempt.PermissionLevel;

                return RedirectToAction("Dashboard");
            }


            return View("AdminEditUser", AdminEditAttempt);
        }
        
        [HttpPost]
        [Route("admin/edit/password")]
        public IActionResult AdminProcessPassword(string Password, string PasswordConfirm, int UserId)
        {
            int? SessionId = HttpContext.Session.GetInt32("id");
            int? Permission = HttpContext.Session.GetInt32("permission");
            if (SessionId != null && Permission != null && Permission == 9)
            {
                ViewBag.LoggedIn = true;
                ViewBag.Name = HttpContext.Session.GetString("name");
                ViewBag.Id = SessionId;
                ViewBag.Permission = Permission;
            }
            else
            {
                return RedirectToAction("Dashboard");
            }

            User PasswordChangeAttempt = _context.users.SingleOrDefault(user => user.UserId == UserId);
            if (Password.Length >= 8 && Password == PasswordConfirm && PasswordChangeAttempt != null)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                PasswordChangeAttempt.Password = Hasher.HashPassword(PasswordChangeAttempt, Password);

                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            else
            {
                //Add error messages for password not changing
                return View("AdminEditUser");
            }
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
