using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;

namespace WebBanDoDienTu.Controllers
{
    public class LoginController : Controller
    {
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(KhachHang user)
        {
            KhachHang check = data.KhachHangs.Where(s => s.UserName == user.UserName && s.Password == user.Password).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai thông tin tài khoản";
                return View("Index");
            }
            else
            {
                data.Configuration.ValidateOnSaveEnabled = false;

                Session["UserName"] = check.UserName;
                Session["Password"] = check.Password;
                Session["KhachHang"] = check;


                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(KhachHang khachHang)
        {                      
            if (ModelState.IsValid)
            {                
                data.KhachHangs.Add(khachHang);
                data.SaveChanges();                
                return RedirectToAction("Index", "Login");
            }
            ViewBag.ErrorInfo = "Vui lòng nhập đúng thông tin";
            return View();
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }
    }
}