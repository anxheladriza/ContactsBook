using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactsBook.Models
{
    public class IsAuthorizedAttribute : AuthorizeAttribute
    {

        public IsAuthorizedAttribute()
        {
        }

        public IsAuthorizedAttribute(string role)
        {
            Role = role;
        }

        public string Role { get; set; }

        protected override bool AuthorizeCore(HttpContextBase htppContext)
        {
            if (Convert.ToBoolean(htppContext.Session["IsLoggedIn"]) == false)
            {
                return false;
            }

            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (AuthorizeCore(filterContext.HttpContext))
            {
                if (!string.IsNullOrEmpty(Role) && Role == "Admin")
                {
                    if (Convert.ToBoolean(filterContext.HttpContext.Session["IsAdmin"]) == false)
                    {
                        filterContext.Result = new HttpUnauthorizedResult("You do not have access to this page!");
                    }
                }
            }
            else
            {
                // Otherwise redirect to your specific authorized area
                filterContext.Result = new RedirectResult("~/Home/Login");
            }
        }

    }
}