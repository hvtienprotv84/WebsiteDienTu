using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;

namespace WebBanDoDienTu.Areas.Admin.Controllers
{
    public class AdminChiTietDonDatHangsController : Controller
    {
        private WebBanDoDienTuEntities db = new WebBanDoDienTuEntities();

        // GET: Admin/AdminChiTietDonDatHangs
        public ActionResult Index(string IDDDH)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            else
            {
                if (IDDDH == null)
                {
                    return View(db.ChiTietDonDatHangs.Include(c => c.DonDatHang).Include(c => c.MatHang).ToList());
                }
                else if (IDDDH.Equals(""))
                {
                    return View(db.ChiTietDonDatHangs.Include(c => c.DonDatHang).Include(c => c.MatHang).ToList());
                }
                else
                {
                    int id = int.Parse(IDDDH);
                    return View(db.ChiTietDonDatHangs.Include(c => c.DonDatHang).Include(c => c.MatHang).Where(c => c.IDDDH == id).ToList());
                }
            }
        }

        // GET: Admin/AdminChiTietDonDatHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonDatHang chiTietDonDatHang = db.ChiTietDonDatHangs.Find(id);
            if (chiTietDonDatHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDonDatHang);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
