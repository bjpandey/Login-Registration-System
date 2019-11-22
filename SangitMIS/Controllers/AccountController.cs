using SangitMIS.Models;
using SangitMIS.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using SangitMIS;

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
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login l, string ReturnUrl)
        {
            
            using (SangitMISEntities db = new SangitMISEntities())
            {
              
                var v = db.UserRegisters.Where(a => a.EmailID == l.EmailID).FirstOrDefault();
                if ( ModelState.IsValid && v != null)
                {
                    if (string.Compare(Crypto.Hash(l.Password), v.Password) == 0)
                    {
                        int timeout = l.RememberMe ? 525600 : 20;
                        var ticket = new FormsAuthenticationTicket(l.EmailID, l.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

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
                        ModelState.AddModelError("", "Invalid Credentials Provided");
                    }
          
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