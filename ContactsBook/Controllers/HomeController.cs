using ContactsBook.DAL;
using ContactsBook.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ContactsBook.Controllers
{
    public class HomeController : Controller
    {
        private ContactBookContexts db = new ContactBookContexts();
        public ActionResult Index()
        {
            if (!Convert.ToBoolean(Session["IsLoggedIn"]))
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "Id,Email,Password,FirstName,LastName,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(x => x.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "User with this email is already registered!");
                    return View("Register", user);
                }
                user.RoleId = 2; //Shtojm guest user 
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View("Register", user);
        }



        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Id,Email,Password,ConfirmPassword,Name,Surname")] User user)
        {
            var loginUser = db.Users.FirstOrDefault(u => u.Email == user.Email);

            if (loginUser != null)
            {
                if (loginUser.Password == user.Password)
                {
                    Session.Add("IsLoggedIn", true);
                    Session.Add("UserName", loginUser.Email);

                    if (loginUser.RoleId == 1)
                    {
                        Session.Add("IsAdmin", true);
                    }

                    return RedirectToAction("Index", "Contacts");
                }
                else
                {
                    ModelState.AddModelError("Password", "Password Incorrect!");
                    return View("Login", user);
                }
            }
            else
            {
                ModelState.AddModelError("Email", "User not found, please register first!");
                return View("Login", user);
            }
        }

        public ActionResult LogOut()
        {
            Session.Remove("IsLoggedIn");
            Session.Remove("IsAdmin");
            return View("Login");
        }

    }
}