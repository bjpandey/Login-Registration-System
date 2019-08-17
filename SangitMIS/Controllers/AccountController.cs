using SangitMIS.Models;
using SangitMIS.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace SangitMIS.Controllers
{
    public class AccountController : Controller
    {

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login l, bool RememberMe, string ReturnUrl = "")
        {
            using (SangitMISEntities db = new SangitMISEntities())
            {
                var users = db.Users.Where(a => a.EmailID == l.EmailID && a.Password == l.Password).FirstOrDefault();
                if (users != null)
                {
                  FormsAuthentication.SetAuthCookie(l.EmailID, RememberMe);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("MyProfile", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User");
                }
            }
            return View();

        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


    }
}