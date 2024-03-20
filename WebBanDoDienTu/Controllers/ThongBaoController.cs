using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;

namespace WebBanDoDienTu.Controllers
{
    public class ThongBaoController : Controller
    {
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();
        // GET: ThongBao
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MuaThanhCong()
        {
            return View();
        }

        public ActionResult CacMatHangDaMua()
        {
            if (Session["KhachHang"] != null)
            {
                KhachHang khachhang = (KhachHang)Session["KhachHang"];
                List<DonDatHang> listdondathang = data.DonDatHangs.Where(c => c.IDKH == khachhang.IDKH).ToList();
                List<ChiTietDonDatHang> listchitietdondathang = new List<ChiTietDonDatHang>();
                foreach (DonDatHang item in listdondathang)
                {
                    List<ChiTietDonDatHang> temp = data.ChiTietDonDatHangs.Where(c => c.IDDDH == item.IDDDH).ToList();
                    listchitietdondathang = listchitietdondathang.Concat(temp).ToList();
                }
                listchitietdondathang = listchitietdondathang.Distinct().ToList();
                ViewBag.chitietdonhang = listchitietdondathang;
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult DanhGia(FormCollection form)
        {
            string danhgia = form["rate"];
            string IDChiTietDonDatHang = form["IDChiTietDonDatHang"];
            string BinhLuan = form["BinhLuan"];
            if (BinhLuan == null)
                BinhLuan = null;
            else BinhLuan = BinhLuan;
            data.sp_DanhGiaMatHangDaMua(int.Parse(IDChiTietDonDatHang), int.Parse(danhgia), BinhLuan);
            return RedirectToAction("CacMatHangDaMua", "ThongBao");
        }

        public ActionResult CacDonHangDangCho()
        {
            if (Session["KhachHang"] != null)
            {
                KhachHang khachhang = (KhachHang)Session["KhachHang"];
                List<DonDatHang> listdondathang = data.DonDatHangs.Where(c => c.IDKH == khachhang.IDKH && c.IDTrangThai == 1).ToList();
                ViewBag.cacdondathangdangcho = listdondathang;
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult HuyDonHang(FormCollection form)
        {

            int id = int.Parse(form["ID DonDatHang"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = data.DonDatHangs.FirstOrDefault(c => c.IDDDH == id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }

            data.sp_XoaDonDatHangChuaDuyet(id);

            return RedirectToAction("CacDonHangDangCho", "ThongBao");

        }

        public ActionResult TatCaCacDonHang()
        {

            if (Session["KhachHang"] != null)
            {
                KhachHang khachhang = (KhachHang)Session["KhachHang"];
                List<DonDatHang> listdondathang = data.DonDatHangs.Where(c => c.IDKH == khachhang.IDKH).ToList();
                ViewBag.tatcacacdonhang = listdondathang;
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult TimKiemDonDatHang(string SoDienThoai)
        {
            if (SoDienThoai == null)
                return View();
            List<DonDatHang> listdondathang = data.DonDatHangs.Where(c => c.DienThoaiKhongAccount == SoDienThoai).ToList();
            return View(listdondathang);
        }

        public ActionResult DanhGiaKhachLe(FormCollection form)
        {
            string danhgia = form["rate"];
            string IDChiTietDonDatHang = form["IDChiTietDonDatHang"];
            string BinhLuan = form["BinhLuan"];
            if (danhgia == null || IDChiTietDonDatHang == null || BinhLuan == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert ('Vui đánh giá và bình luận!');</script>");
            }

            if (BinhLuan == null)
                BinhLuan = null;
            else BinhLuan = BinhLuan;
            data.sp_DanhGiaMatHangDaMua(int.Parse(IDChiTietDonDatHang), int.Parse(danhgia), BinhLuan);
            return RedirectToAction("TimKiemDonDatHang", "ThongBao");
        }

        public ActionResult HuyDonHangKhachLe(FormCollection form)
        {

            int id = int.Parse(form["ID DonDatHang"].ToString());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = data.DonDatHangs.FirstOrDefault(c => c.IDDDH == id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }

            data.sp_XoaDonDatHangChuaDuyet(id);

            return RedirectToAction("TimKiemDonDatHang", "ThongBao");

        }
    }
}