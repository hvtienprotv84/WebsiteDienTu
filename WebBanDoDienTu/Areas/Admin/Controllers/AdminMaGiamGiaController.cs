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
    public class AdminMaGiamGiaController : Controller
    {
        private WebBanDoDienTuEntities db = new WebBanDoDienTuEntities();

        // GET: Admin/AdminMaGiamGia
        public ActionResult Index()
        {
            return View(db.MaGiamGias.ToList());
        }

        // GET: Admin/AdminMaGiamGia/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaGiamGia maGiamGia = db.MaGiamGias.Find(id);
            if (maGiamGia == null)
            {
                return HttpNotFound();
            }
            return View(maGiamGia);
        }

        // GET: Admin/AdminMaGiamGia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminMaGiamGia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( MaGiamGia maGiamGia)
        {
            if (ModelState.IsValid)
            {
                db.MaGiamGias.Add(maGiamGia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maGiamGia);
        }

        // GET: Admin/AdminMaGiamGia/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaGiamGia maGiamGia = db.MaGiamGias.Find(id);
            if (maGiamGia == null)
            {
                return HttpNotFound();
            }
            return View(maGiamGia);
        }

        // POST: Admin/AdminMaGiamGia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDMaGiamGia,TenMaGiamGia,NgayBatDauGiamGia,NgayKetThucGiamGia,SoTienGiam")] MaGiamGia maGiamGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maGiamGia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maGiamGia);
        }

        // GET: Admin/AdminMaGiamGia/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaGiamGia maGiamGia = db.MaGiamGias.Find(id);
            if (maGiamGia == null)
            {
                return HttpNotFound();
            }
            return View(maGiamGia);
        }

        // POST: Admin/AdminMaGiamGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MaGiamGia maGiamGia = db.MaGiamGias.Find(id);
            db.MaGiamGias.Remove(maGiamGia);
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
