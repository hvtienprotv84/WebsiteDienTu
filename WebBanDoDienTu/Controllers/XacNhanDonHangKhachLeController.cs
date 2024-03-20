using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;
using System.Net;


namespace WebBanDoDienTu.Controllers
{
    public class XacNhanDonHangKhachLeController : Controller
    {
        // GET: XacNhanDonHangKhachLe   
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();
        public ActionResult XacNhan()
        {

            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = data.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GioHang gioHang = (GioHang)Session["GioHang"];

            ViewBag.DonDatHang = donDatHang;
            ViewBag.GioHang = gioHang;

            ViewData["IDPT"] = new SelectList(data.PhuongThucThanhToans, "IDPT", "TenPT", donDatHang.IDPT);

            return View(donDatHang);*/

            GioHang gio = (GioHang)Session["GioHang"];
            if (gio == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.IDPT = new SelectList(data.PhuongThucThanhToans, "IDPT", "TenPT", 1);

            return View();
        }

        // POST: XanNhanDonHangKhachLe/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanDonHang()
        {
            /*try
            {*/
                if (Session["GioHang"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                DonDatHang donDatHang = new DonDatHang();

                donDatHang.DiaChiNhanHang = Request.Form.Get("DiaChiNhanHang");
                donDatHang.TenKHKhongAccount = Request.Form.Get("TenKHKhongAccount");
                donDatHang.DienThoaiKhongAccount = Request.Form.Get("DienThoaiKhongAccount");

                if (donDatHang.TenKHKhongAccount == "" || donDatHang.DienThoaiKhongAccount == "" || donDatHang.DienThoaiKhongAccount == "")
                {
                    return RedirectToAction("XacNhan", "XacNhanDonHangKhachLe");
                }

                var checkbox = Request.Form.Get("TrangThaiThanhToan");
                if (checkbox != null)
                {
                    donDatHang.IDPT = int.Parse(Request.Form.Get("IDPT").ToString());
                    donDatHang.TrangThaiThanhToan = true;
                    donDatHang.NgayThanhToan = DateTime.Now;
                }
                else
                {
                    donDatHang.TrangThaiThanhToan = false;
                    donDatHang.IDPT = 8;
                }

                donDatHang.IDTrangThai = 1;
                donDatHang.NgayMua = DateTime.Now;
                data.DonDatHangs.Add(donDatHang);
                data.SaveChanges();

                int tongtien = 0;
                int _tongHang = 0;
                try
                {
                    // Lấy tưng sản phẩm
                    GioHang gio = (GioHang)Session["GioHang"];

                    foreach (var item in gio.ListHang)
                    {
                        ChiTietDonDatHang detail = new ChiTietDonDatHang();
                        detail.IDDDH = donDatHang.IDDDH;
                        detail.IDMH = item.gioHang.IDMH;
                        detail.SoluongMH = item._soLuongHang;

                        tongtien += (int)(item.gioHang.DonGia * item._soLuongHang);
                        _tongHang += item._soLuongHang;
                        data.ChiTietDonDatHangs.Add(detail);
                        data.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    data.DonDatHangs.Remove(donDatHang);
                }
                donDatHang.TongSoluong = _tongHang;
                donDatHang.TongTien = tongtien;

                data.SaveChanges();                
                Session.Remove("GioHang");
                Session.Remove("SoLuongHangTrongGioHang");
                return RedirectToAction("MuaThanhCong", "ThongBao");
            /*}
            catch
            {
                Session.Remove("DonDatHang");
                Session.Remove("GioHang");
                Session.Remove("SoLuongHangTrongGioHang");
                return Content("<script language='javascript' type='text/javascript'>alert ('Vui lòng đặt hàng lại từ đầu!');</script>");

            }*/

        }
    }
}