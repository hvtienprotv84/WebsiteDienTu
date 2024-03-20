using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;

namespace WebBanDoDienTu.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAdmin(WebBanDoDienTu.Models.Admin admin)
        {
            WebBanDoDienTu.Models.Admin check = data.Admins.Where(s => s.UserName == admin.UserName && s.Password == admin.Password).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai thông tin tài khoản";
                return View("Index");
            }
            else
            {
                data.Configuration.ValidateOnSaveEnabled = false;
                Session["Username"] = admin.UserName;
                Session["Password"] = admin.Password;
                Session["TenAdmin"] = check.TenAdmin;
                Session["Admin"] = check;
                return RedirectToAction("Index", "AdminHome", new { Areas = "Admin" });
            }
        }
    }
}