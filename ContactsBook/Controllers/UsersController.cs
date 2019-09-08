using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContactsBook;
using ContactsBook.DAL;
using ContactsBook.Models;

namespace ContactsBook.Controllers
{

    public class UsersController : Controller
    {
        private ContactBookContexts db = new ContactBookContexts();
        // GET: Users
        public ActionResult Users()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            List<User> user = db.Users.Include("Role").ToList();

            return View(user);
        }

        public bool IsLoggedIn()
        {
            if (Convert.ToBoolean(Session["IsLoggedIn"]))
            {
                return true;
            }

            return false;
        }


      

        // GET: User/Delete/5
        public ActionResult DeleteUSer(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }



        // POST: Contacts/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Users");
        }
    }
}
  

    

