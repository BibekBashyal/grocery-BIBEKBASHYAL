using grocery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using grocery.Models.ViewModel;
using System.Web.Security;

namespace grocery.Controllers
{
    public class HomeController : Controller
    {

        KantipurDBEntities db = new KantipurDBEntities();
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult ProductList(string search, int? page, int id = 0)
        {

            if (id != 0)
            {

                return View(db.tblProducts.Where(p => p.CategoryId == id).ToList().ToPagedList(page ?? 1, 4));
            }
            else
            {
                if (search != "")
                {
                    return View(db.tblProducts.Where(x => x.Description.Contains(search) || x.ProductName.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, 4));
                }
                else
                {
                    return View(db.tblProducts.ToList().ToPagedList(page ?? 1, 4));
                }

            }

        }
       
        [Authorize]
        public ActionResult Dogout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult ViewItem(int id)
        {
            return PartialView("_ViewItem", db.tblItems.Find(id));
        }

        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(UserViewModel uv)
        {
            tbluser tbl = db.tblusers.Where(u => u.Username == uv.Username).FirstOrDefault();
            if (tbl != null)
            {
                return Json(new { success = false, message = "User Already Register" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                tbluser tb = new tbluser();
                tb.Username = uv.Username;
                tb.Password = uv.Password;
                tb.FullName = uv.FullName;
                tb.Email = uv.Email;
                db.tblusers.Add(tb);
                db.SaveChanges();

                tblUserRole ud = new tblUserRole();
                ud.UserId = tb.UserId;
                ud.RoleId = 2;
                db.tblUserRoles.Add(ud);
                db.SaveChanges();
                return Json(new { success = true, message = "User Register Successfully" }, JsonRequestBehavior.AllowGet);
            }



        }
    }
}