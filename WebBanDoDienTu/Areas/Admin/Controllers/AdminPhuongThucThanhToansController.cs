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
    public class AdminPhuongThucThanhToansController : Controller
    {
        private WebBanDoDienTuEntities db = new WebBanDoDienTuEntities();

        // GET: Admin/AdminPhuongThucThanhToans
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            return View(db.PhuongThucThanhToans.ToList());
        }

        // GET: Admin/AdminPhuongThucThanhToans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuongThucThanhToan phuongThucThanhToan = db.PhuongThucThanhToans.Find(id);
            if (phuongThucThanhToan == null)
            {
                return HttpNotFound();
            }
            return View(phuongThucThanhToan);
        }

        // GET: Admin/AdminPhuongThucThanhToans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminPhuongThucThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDPT,TenPT")] PhuongThucThanhToan phuongThucThanhToan)
        {
            if (ModelState.IsValid)
            {
                db.PhuongThucThanhToans.Add(phuongThucThanhToan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phuongThucThanhToan);
        }

        // GET: Admin/AdminPhuongThucThanhToans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuongThucThanhToan phuongThucThanhToan = db.PhuongThucThanhToans.Find(id);
            if (phuongThucThanhToan == null)
            {
                return HttpNotFound();
            }
            return View(phuongThucThanhToan);
        }

        // POST: Admin/AdminPhuongThucThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDPT,TenPT")] PhuongThucThanhToan phuongThucThanhToan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phuongThucThanhToan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phuongThucThanhToan);
        }

        // GET: Admin/AdminPhuongThucThanhToans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuongThucThanhToan phuongThucThanhToan = db.PhuongThucThanhToans.Find(id);
            if (phuongThucThanhToan == null)
            {
                return HttpNotFound();
            }
            return View(phuongThucThanhToan);
        }

        // POST: Admin/AdminPhuongThucThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhuongThucThanhToan phuongThucThanhToan = db.PhuongThucThanhToans.Find(id);
            db.PhuongThucThanhToans.Remove(phuongThucThanhToan);
            db.SaveChanges();
            return RedirectToAction("Index");
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
