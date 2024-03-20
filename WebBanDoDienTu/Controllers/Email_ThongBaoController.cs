using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WebBanDoDienTu.Controllers
{
    public class Email_ThongBaoController : Controller
    {

        public ActionResult SendMail_ThongBao()
        {
            return View();
        }
        // GET: Email_ThongBao
        [HttpPost]
        public ActionResult SendMail_ThongBao(string useremail)
        {
            string subject = "ABC TEST";
            string body = "XIN CHAO QUY KHACH";
            WebMail.Send(useremail,subject,body,null,null,null,true,null,null,null, null, null, null);
            ViewBag.msg = "email sent successly";
            return View();
        }
    }
}