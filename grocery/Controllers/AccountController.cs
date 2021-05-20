using grocery.Models;
using grocery.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace grocery.Controllers
{
    public class AccountController : Controller
    {
        
        // GET: Account
        KantipurDBEntities db = new KantipurDBEntities();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(LoginViewModel l, string ReturnUrl = "")
        {
            using (KantipurDBEntities db = new KantipurDBEntities())
            {
                var users = db.tblusers.Where(a => a.Username == l.Username && a.Password == l.Password).FirstOrDefault();
                if (users != null)
                {
                    Session.Add("Username", users.Username);
                    FormsAuthentication.SetAuthCookie(l.Username, l.RememberMe);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    

                    else
                    {
                         return RedirectToAction("Index", "Home");
                        
                    }


                }
                
                else
                {
                    ModelState.AddModelError("", "Invalid User");
                }
            }
            return View();

        }
      
        //Logout 
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ChangePassword()
        {



            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel ch)
        {
            string username = Session["Username"].ToString();

            tbluser us = db.tblusers.Where(u => u.Username == username && u.Password == ch.OldPassword).FirstOrDefault();
            if (us != null)
            {
                us.Password = ch.NewPassword;
                db.SaveChanges();

            }
            else
            {
                return Json(new { success = false, message = "You Enter Wrong Password" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Password Changed Successfully" }, JsonRequestBehavior.AllowGet);
        }

             //forget password
        public ActionResult ForgetPassword()
        {
            return View();


        }
        [ValidateOnlyIncomingValuesAttribute]
        [HttpPost]

        public ActionResult ForgetPassword(UserViewModel uv)
        {

            if (ModelState.IsValid)
            {
                //https://www.google.com/settings/security/lesssecureapps
                //Make Access for less secure apps=true

                string from = "wildxxgaming@gmail.com";
                using (MailMessage mail = new MailMessage(from, uv.Email))
                {
                    try
                    {
                        tbluser tb = db.tblusers.Where(u => u.Email == uv.Email).FirstOrDefault();
                        if (tb != null)
                        {
                            mail.Subject = "Password Recovery";
                            mail.Body = "Your Password is:" + tb.Password;

                            mail.IsBodyHtml = false;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential networkCredential = new NetworkCredential(from, "basubinod");
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkCredential;
                            smtp.Port = 587;
                            smtp.Send(mail);
                            ViewBag.Message = "Your Password Is Sent to your email";
                        }
                        else
                        {
                            ViewBag.Message = "email Doesnot Exist in Database";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }

                }

            }
            return View();


            //return RedirectToAction("Index", "Home");
        }


    }
}
