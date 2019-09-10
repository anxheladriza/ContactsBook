using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ContactsBook.DAL;
using ContactsBook.Models;

namespace ContactsBook.Controllers
{
    [IsAuthorized("Admin")]
    public class UsersController : Controller
    {
        private ContactBookContexts db = new ContactBookContexts();
        // GET: Users
        public ActionResult Users()
        {           
            List<User> user = db.Users.Include("Role").ToList();
            return View(user);
        }

        

        // GET: User/Delete/5
        public ActionResult DeleteUSer(int? id)
        {
           
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
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Users");
        }

    }

}
  

    

