using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;

namespace WebBanDoDienTu.Areas.Admin.Controllers
{
    public class AdminMatHangsController : Controller
    {
        private WebBanDoDienTuEntities db = new WebBanDoDienTuEntities();

        // GET: Admin/AdminMatHangs
        public ActionResult Index(string TenMH)
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "AdminLogin", new { Areas = "Admin" });

            }
            else
            {
                var matHangs = db.MatHangs.Include(m => m.LoaiMatHang);
                if (TenMH == null)
                {
                    return View(matHangs.ToList());
                }
                else if (TenMH.Equals(""))
                {
                    return View(matHangs.ToList());
                }
                else
                {
                    List<MatHang> mb = db.MatHangs.Where(c => c.LoaiMatHang.TenLoaiMH.ToLower().Contains(TenMH.ToLower())).ToList();
                    if (mb.Count > 0)
                    {
                        return View(mb);
                    }
                    return View(db.MatHangs.Where(s => s.TenMH.ToLower().Contains(TenMH.ToLower())).ToList());
                }
            }
        }

        // GET: Admin/AdminMatHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatHang matHang = db.MatHangs.Find(id);
            if (matHang == null)
            {
                return HttpNotFound();
            }
            return View(matHang);
        }

        // GET: Admin/AdminMatHangs/Create
        public ActionResult Create()
        {
            ViewBag.IDLoaiMH = new SelectList(db.LoaiMatHangs, "IDLoaiMH", "TenLoaiMH");
            return View();
        }

        // POST: Admin/AdminMatHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MatHang matHang)
        {
            if (matHang.UploadImage1 != null)
            {
                using (var binaryReader = new BinaryReader(matHang.UploadImage1.InputStream))
                    matHang.HinhAnh1 = binaryReader.ReadBytes(matHang.UploadImage1.ContentLength);
            }
            if (matHang.UploadImage2 != null)
            {
                using (var binaryReader = new BinaryReader(matHang.UploadImage2.InputStream))
                    matHang.HinhAnh2 = binaryReader.ReadBytes(matHang.UploadImage2.ContentLength);
            }
            if (matHang.UploadImage3 != null)
            {
                using (var binaryReader = new BinaryReader(matHang.UploadImage3.InputStream))
                    matHang.HinhAnh3 = binaryReader.ReadBytes(matHang.UploadImage3.ContentLength);
            }
            if (matHang.UploadImage4 != null)
            {
                using (var binaryReader = new BinaryReader(matHang.UploadImage4.InputStream))
                    matHang.HinhAnh4 = binaryReader.ReadBytes(matHang.UploadImage4.ContentLength);
            }
            if (ModelState.IsValid)
            {
                db.MatHangs.Add(matHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDLoaiMH = new SelectList(db.LoaiMatHangs, "IDLoaiMH", "TenLoaiMH", matHang.IDLoaiMH);
            return View(matHang);
        }

        // GET: Admin/AdminMatHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatHang matHang = db.MatHangs.Find(id);

            if (matHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDLoaiMH = new SelectList(db.LoaiMatHangs, "IDLoaiMH", "TenLoaiMH", matHang.IDLoaiMH);
            return View(matHang);
        }

        // POST: Admin/AdminMatHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MatHang matHang)
        {
            if (ModelState.IsValid)
            {
                if (matHang.UploadImage1 != null)
                {
                    using (var binaryReader = new BinaryReader(matHang.UploadImage1.InputStream))
                        matHang.HinhAnh1 = binaryReader.ReadBytes(matHang.UploadImage1.ContentLength);
                }
                if (matHang.UploadImage2 != null)
                {
                    using (var binaryReader = new BinaryReader(matHang.UploadImage2.InputStream))
                        matHang.HinhAnh2 = binaryReader.ReadBytes(matHang.UploadImage2.ContentLength);
                }
                if (matHang.UploadImage3 != null)
                {
                    using (var binaryReader = new BinaryReader(matHang.UploadImage3.InputStream))
                        matHang.HinhAnh3 = binaryReader.ReadBytes(matHang.UploadImage3.ContentLength);
                }
                if (matHang.UploadImage4 != null)
                {
                    using (var binaryReader = new BinaryReader(matHang.UploadImage4.InputStream))
                        matHang.HinhAnh4 = binaryReader.ReadBytes(matHang.UploadImage4.ContentLength);
                }
                db.Entry(matHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLoaiMH = new SelectList(db.LoaiMatHangs, "IDLoaiMH", "TenLoaiMH", matHang.IDLoaiMH);
            return View(matHang);
        }

        // GET: Admin/AdminMatHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatHang matHang = db.MatHangs.Find(id);
            if (matHang == null)
            {
                return HttpNotFound();
            }
            return View(matHang);
        }

        // POST: Admin/AdminMatHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MatHang matHang = db.MatHangs.Find(id);
            db.MatHangs.Remove(matHang);
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
