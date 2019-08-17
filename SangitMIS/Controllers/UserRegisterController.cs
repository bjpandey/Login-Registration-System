using RegistrationAndLogin;
using SangitMIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SangitMIS.Controllers
{
    public class UserRegisterController : Controller
    {
        // Register Action
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "IsEmailVerified, ActivationCode")]UserRegister register)
        {
            bool Status = false;
            string Message = "";

            if (ModelState.IsValid)
            {
                #region Email Already Exist
                var isExist = IsEmailExist(register.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exist");
                    return View(register);
                }
                #endregion

                #region Generate Activation Code
                register.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                register.Password = Crypto.Hash(register.Password);
                register.ConfirmPassword = Crypto.Hash(register.ConfirmPassword);
                #endregion

                #region Save to database
                using (SangitMISEntities db = new SangitMISEntities())
                {
                    db.UserRegisters.Add(register);
                    db.SaveChanges();
                }
                #endregion

                #region Send Verification Email Link
                sendVerificationEmailLink(register.EmailID, register.ActivationCode.ToString());
                Message = " Registration succesfully done" + " Email Activation Link has been sent to your email Id "
                    + register.EmailID;
                Status = true;
                #endregion

            }
            else
            {
                Message = "Invalid Request";
            }
            ViewBag.message = Message;
            ViewBag.status = Status;
            return View(register);

        }
        [NonAction]
        public bool IsEmailExist(string EmailId)
        {
            using (SangitMISEntities db = new SangitMISEntities())
            {
                var v = db.UserRegisters.Where(a => a.EmailID == EmailId).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void sendVerificationEmailLink(string EmailID, string ActivationCode)
        {
            var verifyUrl = "/UserRegister/VerifyAccount/" + ActivationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("creatorbzay1111@gmail.com", "www.bijayapandey.com.np");
            var toEmail = new MailAddress(EmailID);
            var fromEmailPassword = "20182rdimr7x4s6gaic";
            string subject = "Account created successfully";
            string body = "<br/><br/>We are excited to tell you that your account is" +
                " successfully created. Please, click on the link below to verify your account" +
                "<br/><br/><a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
                smtp.Send(message);

        }


    }
}