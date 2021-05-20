using grocery.Models;
using grocery.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace grocery.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        // GET: AdminUser
        KantipurDBEntities _db = new KantipurDBEntities();
        public ActionResult ManageUser()
        {
            return View();
        }
        public JsonResult GetData()
        {
            using (KantipurDBEntities db = new KantipurDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<UserViewModel> lstitem = new List<UserViewModel>();
                var lst = db.tblusers.ToList();
                foreach (var item in lst)
                {
                    lstitem.Add(new UserViewModel() { UserId = item.UserId, Username = item.Username, Photo = item.Photo });
                }
                return Json(new { data = lstitem }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddOrEdit()
        {
            return View();
        }
        [HttpPost]

        public ActionResult AddOrEdit(UserViewModel uv)
        {
            tbluser tb = new tbluser();
            tb.Username = uv.Username;
            tb.Email = uv.Email;
            tb.Password = uv.Password;

            HttpPostedFileBase fup = Request.Files["Photo"];
            if (fup != null)
            {
                if (fup.FileName != "")
                {
                    tb.Photo = fup.FileName;
                    fup.SaveAs(Server.MapPath("~/images/" + fup.FileName));
                }
            }
            _db.tblusers.Add(tb);
            _db.SaveChanges();

            tblUserRole ud = new tblUserRole();
            ud.UserId = tb.UserId;
            ud.UserRoleId = 1;
            _db.tblUserRoles.Add(ud);
            _db.SaveChanges();
            ViewBag.Message = "User Created Successfully";


            return View();
        }
        [HttpPost]

        public ActionResult Delete(int id)
        {
            using (KantipurDBEntities db = new KantipurDBEntities())
            {
                tbluser sm = db.tblusers.Where(x => x.UserId == id).FirstOrDefault();
                db.tblusers.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}