using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using ContactsBook.DAL;
using ContactsBook.Models;

namespace ContactsBook.Controllers
{
    public class ContactsController : Controller
    {
        private ContactBookContexts db = new ContactBookContexts();

        //Lista e kontakteve
        [HttpGet]
        public ActionResult Index(string searchString)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            List<Contact> contacts = db.Contacts.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(
                cont => cont.FirstName.Contains(searchString)
                       || cont.LastName.Contains(searchString)
                       || cont.Emails.Any(e => e.EmailAddress.Contains(searchString))
                       || cont.PhoneNumbers.Any(p => p.Number.Contains(searchString))
                   ).ToList();
            }

            return View(contacts);
        }

        // GET: Contacts/Details/5
        [HttpGet]
        public ActionResult ContactDetails(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contact contact = db.Contacts.Find(id);

            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }


        // GET: Kthen  view per krijimin e kontaktit
        public ActionResult CreateContact()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            return View("CreateContact");
        }



        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateContact([Bind(Include = "Id,FirstName,LastName")] Contact contact)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        //Kthen view per te edituar kontaktin
        public ActionResult EditContact(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //Ruajm kontaktin ne database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContact([Bind(Include = "Id,FirstName,LastName")] Contact contact)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult DeleteContact(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);

            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }



        // POST: Contacts/Delete/5
        [HttpPost, ActionName("DeleteContact")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        // GET: Emails
        public ActionResult GetContactEmails(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            return View("Emails/Index", contact.Emails.ToList());
        }



        // GET: Addresses/Create
        public ActionResult CreateAddress()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            return View("Addresses/CreateAddress");
        }

        //post Address

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddress([Bind(Include = "Id,Country,City,StreetAddress,ContactId")] Address address)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                db.SaveChanges();
                return RedirectToAction("ContactDetails", new { id = address.ContactId });
            }

            return View("Addresses/CreateAddress", address);

        }


        //GET: Address/Edit/5  krijon view-n per editim
        public ActionResult EditAddress(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View("Addresses/EditAddress", address);
        }

        //POST : Address/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAddress([Bind(Include = "Id,Country,City,StreetAddress,ContactId")] Address address)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ContactDetails", new { id = address.ContactId });
            }
            return View("Addresses/EditAddress", address);
        }


        public ActionResult DeleteAddress(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);

            if (address == null)
            {
                return HttpNotFound();
            }
            return View("Addresses/DeleteAddress", address);
        }



        [HttpPost, ActionName("DeleteAddress")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAddress(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            Address address = db.Addresses.Find(id);
            var contactId = address.ContactId;
            db.Addresses.Remove(address);
            db.SaveChanges();
            return RedirectToAction("ContactDetails", new { id = contactId });
        }



        public ActionResult AddEmail(int? contactId)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (contactId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Email model = new Email();
            model.ContactId = Convert.ToInt32(contactId);

            return View("Emails/CreateEmail", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmail([Bind(Include = "Id,EmailAddress,ContactId")]Email email)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Emails.Add(email);
                db.SaveChanges();
                return RedirectToAction("ContactDetails", new { id = email.ContactId });
            }

            return RedirectToAction("Index");
        }

        //delete email
        [HttpGet, ActionName("DeleteEmail")]
        public ActionResult DeleteEmail(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Email email = db.Emails.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }

            return View("Emails/DeleteEmail", email);
        }

        [HttpPost, ActionName("DeleteEmail")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmailConfirmed(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            Email email = db.Emails.Find(id);
            var contactId = email.ContactId;
            db.Emails.Remove(email);
            db.SaveChanges();

            return RedirectToAction("ContactDetails", new { id = contactId });
        }
        //Add phone number 

        public ActionResult AddPhoneNumber(int? contactId)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (contactId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PhoneNumber model = new PhoneNumber();
            model.ContactId = Convert.ToInt32(contactId);
            return View("PhoneNumbers/AddPhoneNumber", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPhoneNumber([Bind(Include = "Id,Number,ContactId")]PhoneNumber phoneNumber)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.PhoneNumbers.Add(phoneNumber);
                db.SaveChanges();
                return RedirectToAction("ContactDetails", new { id = phoneNumber.ContactId });
            }
            //kthen view me error validations nqs nuk eshte modeli i marre nga forma me post valid
            return View("PhoneNumbers/AddPhoneNumber", phoneNumber);
        }

        //delete number


        [HttpGet, ActionName("DeleteNumber")]
        public ActionResult DeleteNumber(int? id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            PhoneNumber phoneNumber = db.PhoneNumbers.Find(id);
            if (phoneNumber == null)
            {
                return HttpNotFound();
            }

            return View("PhoneNumbers/DeleteNumber", phoneNumber);
        }

        [HttpPost, ActionName("DeleteNumber")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNumberConfirmed(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            PhoneNumber number = db.PhoneNumbers.Find(id);
            var contactId = number.ContactId;
            db.PhoneNumbers.Remove(number);
            db.SaveChanges();

            return RedirectToAction("ContactDetails", new { id = contactId });
        }

        public ActionResult EmailDetails(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var email = db.Emails.Find(id);
            return View("Emails/EmailDetails", email);
        }



        public ActionResult EditEmail(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var email = db.Emails.Find(id);
            return View("Emails/EditEmail", email);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmail([Bind(Include = "Id,EmailAddress,ContactId")]Email email)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(email).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ContactDetails", new { id = email.ContactId });
            }

            return RedirectToAction("Index");
        }

        //phone number

        public ActionResult EditPhoneNumber(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            var number = db.PhoneNumbers.Find(id);
            return View("PhoneNumbers/EditPhoneNumber", number);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhoneNumber([Bind(Include = "Id,Number,ContactId")]PhoneNumber phoneNumber)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(phoneNumber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ContactDetails", new { id = phoneNumber.ContactId });
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public FileResult Export()
        {
            var workbook = new XLWorkbook();
            workbook.AddWorksheet("Contacts");
            var ws = workbook.Worksheet("Contacts");

            var contacts = db.Contacts.ToList();

            //insert columns
            ws.Cell("A1").Value = "First Name";
            ws.Cell("B1").Value = "Last Name";
            ws.Cell("C1").Value = "Phone Number";
            ws.Cell("D1").Value = "Email";
            ws.Cell("E1").Value = "Address";


            int row = 2;
            foreach (var contact in contacts)
            {
                ws.Cell("A" + row.ToString()).Value = contact.FirstName;
                ws.Cell("B" + row.ToString()).Value = contact.LastName;
                ws.Cell("C" + row.ToString()).Value = contact.PhoneNumbers.FirstOrDefault() != null ? contact.PhoneNumbers.FirstOrDefault().Number : "";

                ws.Cell("D" + row.ToString()).Value = contact.Emails.FirstOrDefault() != null ? contact.Emails.FirstOrDefault().EmailAddress : "";
                ws.Cell("E" + row.ToString()).Value = contact.Addresses.FirstOrDefault() != null ? contact.Addresses.FirstOrDefault().Country
                    + " " + contact.Addresses.FirstOrDefault().City + " "
                    + contact.Addresses.FirstOrDefault().StreetAddress : "";
                row++;
            }

            //konverton ne byte excelin per ta shkarkuar me metoden File
            var workbookBytes = new byte[0];
            using (var ms = new MemoryStream())
            {
                workbook.SaveAs(ms);
                workbookBytes = ms.ToArray();
            }

            return File(workbookBytes, "application/vnd.ms-excel", "Grid.xls");
        }



        public bool IsLoggedIn()
        {
            if (Convert.ToBoolean(Session["IsLoggedIn"]))
            {
                return true;
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
