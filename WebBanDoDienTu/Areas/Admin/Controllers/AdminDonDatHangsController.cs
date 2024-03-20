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
    public class AdminDonDatHangsController : Controller
    {
        private WebBanDoDienTuEntities db = new WebBanDoDienTuEntities();

        // GET: Admin/AdminDonDatHangs
        public ActionResult Index(string tenKH, string SDT, DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {

            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            if (tenKH != null && tenKH != "" && SDT != null && SDT != "")
            {
                var ketqua = db.DonDatHangs.Where(s => s.KhachHang.SDT.Contains(SDT)).Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower())).ToList();
                var ketqua1 = db.DonDatHangs.Where(s => s.DienThoaiKhongAccount.Contains(SDT)).Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower())).ToList();

                return View(ketqua.Concat(ketqua1));
            }
            if (tenKH != null && tenKH != "")
            {
                Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
                var donDatHangs = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower())).ToList();
                var donDatHangs1 = db.DonDatHangs.Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower())).ToList();
                return View(donDatHangs.Concat(donDatHangs1));
            }
            if (SDT != null && SDT != "")
            {
                var donDatHangs = db.DonDatHangs.Where(s => s.DienThoaiKhongAccount == SDT).ToList();
                var donDatHangs1 = db.DonDatHangs.Where(s => s.KhachHang.SDT == SDT).ToList();
                return View(donDatHangs.Concat(donDatHangs1));
            }
            else if (NgayBatDau != null && NgayKetThuc != null)
            {
                return View(db.sp_TimKiemDonDatHang(NgayBatDau, NgayKetThuc).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc == null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayBatDau).ToList());
            }
            else if (NgayBatDau == null && NgayKetThuc != null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayKetThuc).ToList());
            }
            return View(db.DonDatHangs.ToList());
        }

        // GET: Admin/AdminDonDatHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            return View(donDatHang);
        }


        // GET: Admin/AdminDonDatHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
            ViewBag.IDKH = new SelectList(db.KhachHangs, "IDKH", "TenKH", donDatHang.IDKH);
            ViewBag.IDPT = new SelectList(db.PhuongThucThanhToans, "IDPT", "TenPT", donDatHang.IDPT);
            ViewBag.IDTrangThai = new SelectList(db.TrangThais, "IDTrangThai", "TenTrangThai", donDatHang.IDTrangThai);
            ViewBag.IDNhanVien = new SelectList(db.NhanViens, "IDNhanVien", "TenNhanVien", donDatHang.IDTrangThai);
            Session["TrangThai"] = donDatHang.IDTrangThai;
            return View(donDatHang);
        }

        // POST: Admin/AdminDonDatHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DonDatHang donDatHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donDatHang).State = EntityState.Modified;
                db.SaveChanges();
                int temp = (int)Session["TrangThai"];
                if ((temp == 1 || temp == 2 || temp == 3 || temp == 4 || temp == 5) && (donDatHang.IDTrangThai == 6))
                {
                    List<ChiTietDonDatHang> cthd = db.ChiTietDonDatHangs.Where(c => c.IDDDH == donDatHang.IDDDH).ToList();
                    foreach (ChiTietDonDatHang item in cthd)
                    {
                        MatHang mh = db.MatHangs.FirstOrDefault(c => c.IDMH == item.IDMH);
                        mh.SoLuong = mh.SoLuong + item.SoluongMH;
                        db.Entry(mh).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    if ((int)Session["TrangThai"] == 6 && donDatHang.IDTrangThai != 6)
                    {
                        List<ChiTietDonDatHang> cthd = db.ChiTietDonDatHangs.Where(c => c.IDDDH == donDatHang.IDDDH).ToList();
                        foreach (ChiTietDonDatHang item in cthd)
                        {
                            MatHang mh = db.MatHangs.FirstOrDefault(c => c.IDMH == item.IDMH);
                            mh.SoLuong = mh.SoLuong - item.SoluongMH;
                            db.Entry(mh).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
            ViewBag.IDKH = new SelectList(db.KhachHangs, "IDKH", "TenKH", donDatHang.IDKH);
            ViewBag.IDPT = new SelectList(db.PhuongThucThanhToans, "IDPT", "TenPT", donDatHang.IDPT);
            ViewBag.IDTrangThai = new SelectList(db.TrangThais, "IDTrangThai", "TenTrangThai", donDatHang.IDTrangThai);
            ViewBag.IDNhanVien = new SelectList(db.NhanViens, "IDNhanVien", "TenNhanVien", donDatHang.IDTrangThai);
            return View(donDatHang);
        }

        // GET: Admin/AdminDonDatHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
            return View(donDatHang);
        }

        // POST: Admin/AdminDonDatHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.sp_XoaDonDatHangADMIN(id);

            /* DonDatHang donDatHang = db.DonDatHangs.Find(id);
             db.DonDatHangs.Remove(donDatHang);
             db.SaveChanges();*/

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


        public ActionResult DuyetDonDatHang(string IDDDH, string tenKH, string SDT, DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            if (tenKH != null && tenKH != "" && SDT != null && SDT != "")
            {
                var ketqua = db.DonDatHangs.Where(s => s.KhachHang.SDT.Contains(SDT)).Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower())).ToList();
                var ketqua1 = db.DonDatHangs.Where(s => s.DienThoaiKhongAccount.Contains(SDT)).Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower())).ToList();

                return View(ketqua.Concat(ketqua1));
            }
            if (tenKH != null && tenKH != "")
            {
                Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
                var donDatHangs = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower())).ToList();
                var donDatHangs1 = db.DonDatHangs.Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower())).ToList();
                return View(donDatHangs.Concat(donDatHangs1));
            }
            if (SDT != null && SDT != "")
            {
                return View(db.DonDatHangs.Where(s => s.KhachHang.SDT == SDT).Include(s => s.DienThoaiKhongAccount == SDT).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc != null)
            {
                return View(db.sp_TimKiemDonDatHang(NgayBatDau, NgayKetThuc).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc == null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayBatDau).ToList());
            }
            else if (NgayBatDau == null && NgayKetThuc != null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayKetThuc).ToList());
            }
            return View(db.DonDatHangs.ToList());
        }

        public ActionResult GiaoHangKhongThanhCong(string IDDDH, string tenKH, string SDT, DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            if (tenKH != null && tenKH != "" && SDT != null && SDT != "")
            {
                var ketqua = db.DonDatHangs.Where(s => s.KhachHang.SDT.Contains(SDT)).Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 5).ToList();
                var ketqua1 = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 5 && s.DienThoaiKhongAccount.Contains(tenKH.ToLower())).ToList();

                return View(ketqua.Concat(ketqua1));
            }
            if (tenKH != null && tenKH != "")
            {
                Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
                var donDatHangs = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 5).ToList();
                var donDatHangs1 = db.DonDatHangs.Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 5).ToList();
                return View(donDatHangs.Concat(donDatHangs1));
            }
            if (SDT != null && SDT != "")
            {
                return View(db.DonDatHangs.Where(s => s.KhachHang.SDT == SDT && s.IDTrangThai == 5).Include(s => s.DienThoaiKhongAccount == SDT).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc != null)
            {
                return View(db.sp_TimKiemDonDatHang(NgayBatDau, NgayKetThuc).Where(s => s.IDTrangThai == 5).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc == null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayBatDau && x.IDTrangThai == 5).ToList());
            }
            else if (NgayBatDau == null && NgayKetThuc != null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayKetThuc).Where(s => s.IDTrangThai == 5).ToList());
            }
            return View(db.DonDatHangs.Where(x => x.IDTrangThai == 5).ToList());
        }

        public ActionResult GiaoHangThanhCong(string IDDDH, string tenKH, string SDT, DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            if (tenKH != null && tenKH != "" && SDT != null && SDT != "")
            {
                var ketqua = db.DonDatHangs.Where(s => s.KhachHang.SDT.Contains(SDT) && s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 4).ToList();
                var ketqua1 = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 4 && s.DienThoaiKhongAccount.Contains(tenKH.ToLower())).ToList();

                return View(ketqua.Concat(ketqua1));
            }
            if (tenKH != null && tenKH != "")
            {
                Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
                var donDatHangs = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 4).ToList();
                var donDatHangs1 = db.DonDatHangs.Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 4).ToList();
                return View(donDatHangs.Concat(donDatHangs1));
            }
            if (SDT != null && SDT != "")
            {
                return View(db.DonDatHangs.Where(s => s.KhachHang.SDT == SDT && s.IDTrangThai == 4).Include(s => s.DienThoaiKhongAccount == SDT).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc != null)
            {
                return View(db.sp_TimKiemDonDatHang(NgayBatDau, NgayKetThuc).Where(s => s.IDTrangThai == 4).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc == null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayBatDau && x.IDTrangThai == 4).ToList());
            }
            else if (NgayBatDau == null && NgayKetThuc != null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayKetThuc && x.IDTrangThai == 4).ToList());
            }
            return View(db.DonDatHangs.Where(x => x.IDTrangThai == 4).ToList());
        }

        public ActionResult TatCaDonHangDaHuy(string IDDDH, string tenKH, string SDT, DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            if (tenKH != null && tenKH != "" && SDT != null && SDT != "")
            {
                var ketqua = db.DonDatHangs.Where(s => s.KhachHang.SDT.Contains(SDT) && s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 6).ToList();
                var ketqua1 = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 6 && s.DienThoaiKhongAccount.Contains(tenKH.ToLower())).ToList();

                return View(ketqua.Concat(ketqua1));
            }
            if (tenKH != null && tenKH != "")
            {
                Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
                var donDatHangs = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 6).ToList();
                var donDatHangs1 = db.DonDatHangs.Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 6).ToList();
                return View(donDatHangs.Concat(donDatHangs1));
            }
            if (SDT != null && SDT != "")
            {
                return View(db.DonDatHangs.Where(s => s.KhachHang.SDT == SDT && s.IDTrangThai == 6).Include(s => s.DienThoaiKhongAccount == SDT).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc != null)
            {
                return View(db.sp_TimKiemDonDatHang(NgayBatDau, NgayKetThuc).Where(s => s.IDTrangThai == 6).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc == null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayBatDau && x.IDTrangThai == 6).ToList());
            }
            else if (NgayBatDau == null && NgayKetThuc != null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayKetThuc && x.IDTrangThai == 6).ToList());
            }
            return View(db.DonDatHangs.Where(x => x.IDTrangThai == 6).ToList());
        }

        public ActionResult DonHangCanXoa(string IDDDH, string tenKH, string SDT, DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            if (tenKH != null && tenKH != "" && SDT != null && SDT != "")
            {
                var ketqua = db.DonDatHangs.Where(s => s.KhachHang.SDT.Contains(SDT) && s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 5).ToList();
                var ketqua1 = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 5 && s.DienThoaiKhongAccount.Contains(tenKH.ToLower())).ToList();

                return View(ketqua.Concat(ketqua1));
            }
            if (tenKH != null && tenKH != "")
            {
                Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
                var donDatHangs = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 5).ToList();
                var donDatHangs1 = db.DonDatHangs.Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 5).ToList();
                return View(donDatHangs.Concat(donDatHangs1));
            }
            if (SDT != null && SDT != "")
            {
                return View(db.DonDatHangs.Where(s => s.KhachHang.SDT == SDT && s.IDTrangThai == 5).Include(s => s.DienThoaiKhongAccount == SDT).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc != null)
            {
                return View(db.sp_TimKiemDonDatHang(NgayBatDau, NgayKetThuc).Where(s => s.IDTrangThai == 5).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc == null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayBatDau && x.IDTrangThai == 5).ToList());
            }
            else if (NgayBatDau == null && NgayKetThuc != null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayKetThuc && x.IDTrangThai == 5).ToList());
            }
            return View(db.DonDatHangs.Where(x => x.IDTrangThai == 5).ToList());
        }

        public ActionResult PhanCongGiaoHang(string IDDDH, string tenKH, string SDT, DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            if (tenKH != null && tenKH != "" && SDT != null && SDT != "")
            {
                var ketqua = db.DonDatHangs.Where(s => s.KhachHang.SDT.Contains(SDT) && s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 3).ToList();
                var ketqua1 = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 3 && s.DienThoaiKhongAccount.Contains(tenKH.ToLower())).ToList();

                return View(ketqua.Concat(ketqua1));
            }
            if (tenKH != null && tenKH != "")
            {
                Session["DonDatHangCho"] = db.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
                var donDatHangs = db.DonDatHangs.Where(s => s.TenKHKhongAccount.Contains(tenKH.ToLower()) && s.IDTrangThai == 3).ToList();
                var donDatHangs1 = db.DonDatHangs.Where(s => s.KhachHang.TenKH.Contains(tenKH.ToLower()) && s.IDTrangThai == 3).ToList();
                return View(donDatHangs.Concat(donDatHangs1));
            }
            if (SDT != null && SDT != "")
            {
                return View(db.DonDatHangs.Where(s => s.KhachHang.SDT == SDT && s.IDTrangThai == 3).Include(s => s.DienThoaiKhongAccount == SDT).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc != null)
            {
                return View(db.sp_TimKiemDonDatHang(NgayBatDau, NgayKetThuc).Where(s => s.IDTrangThai == 3).ToList());
            }
            else if (NgayBatDau != null && NgayKetThuc == null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayBatDau && x.IDTrangThai == 3).ToList());
            }
            else if (NgayBatDau == null && NgayKetThuc != null)
            {
                return View(db.DonDatHangs.Where(x => x.NgayMua == NgayKetThuc && x.IDTrangThai == 3).ToList());
            }
            return View(db.DonDatHangs.Where(x => x.IDTrangThai == 3).ToList());
        }
    }
}
