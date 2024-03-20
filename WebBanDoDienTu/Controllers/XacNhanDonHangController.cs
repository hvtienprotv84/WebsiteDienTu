using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;

namespace WebBanDoDienTu.Controllers
{
    public class XacNhanDonHangController : Controller
    {
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();

        public List<string> maGiamGia()
        {
            List<string> maGiamGiaList = new List<string>();
            List<MaGiamGia> temp = data.MaGiamGias.Where(c => c.NgayBatDauGiamGia < DateTime.Today).Where(c => c.NgayKetThucGiamGia > DateTime.Today).ToList();
            foreach (MaGiamGia item in temp)
            {
                maGiamGiaList.Add(item.IDMaGiamGia.ToString());
            }
            return maGiamGiaList;
        }

        public List<int> soTienTuongUnMaGiamGia()
        {
            List<int> tien = new List<int>();
            List<MaGiamGia> temp = data.MaGiamGias.ToList();
            foreach (MaGiamGia item in temp)
            {
                tien.Add(int.Parse(item.SoTienGiam.ToString()));
            }
            return tien;
        }


        // GET: Admin/AdminDonDatHangs/Edit/5
        public ActionResult XacNhan()
        {
            GioHang gio = (GioHang)Session["GioHang"];
            if (gio == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Session["KhachHang"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            KhachHang kh = (KhachHang)Session["KhachHang"];
            ViewBag.IDPT = new SelectList(data.PhuongThucThanhToans, "IDPT", "TenPT", 1);
            ViewBag.DiemTichLuyCuaKhachHang = kh.DiemTichLuyConLai;
            ViewBag.MaGiamGia = maGiamGia();
            ViewBag.SoTienTuongUngMaGiamGia = soTienTuongUnMaGiamGia();
            ViewBag.HangCuaKhachHang = kh.LoaiKhachHang;
            return View();
        }

        // POST: Admin/AdminDonDatHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanDonHang()
        {
            /*try
            {*/
                if (Session["KhachHang"] == null || Session["GioHang"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                KhachHang khachHang = (KhachHang)Session["KhachHang"];

                DonDatHang donDatHang = new DonDatHang();

                donDatHang.IDKH = khachHang.IDKH;

                if (Request.Form.Get("DiaChiNhanHang") != null && Request.Form.Get("DiaChiNhanHang") != "")
                {
                    donDatHang.DiaChiNhanHang = Request.Form.Get("DiaChiNhanHang");
                }
                else
                {
                    donDatHang.DiaChiNhanHang = khachHang.DiaChiGiaoHang1;
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
                    
                donDatHang.TongSoluong = _tongHang;
                donDatHang.TongTien = tongtien;


                var checkmagiamgia = Request.Form.Get("DungMaGiamGia");
                if (checkbox == "on")
                {
                    string magiamgia = Request.Form.Get("magiamgia");
                    if (magiamgia != "")
                    {
                        MaGiamGia mgg = data.MaGiamGias.Where(c => c.IDMaGiamGia == magiamgia).FirstOrDefault();
                        if (mgg != null)
                        {
                            donDatHang.TongTien = donDatHang.TongTien - mgg.SoTienGiam;
                            if (donDatHang.TongTien < 0) donDatHang.TongTien = 0;
                        }
                    }
                }


                var checkboxDiemTichLuy = Request.Form.Get("DungDiemTichLuy");
                if (checkboxDiemTichLuy == "on")
                {
                    var diemdung = Request.Form.Get("SoDiemDung");
                int intDienDung = int.Parse(diemdung.ToString());

                    donDatHang.TongTien = donDatHang.TongTien - intDienDung;
                    if (donDatHang.TongTien < 0) donDatHang.TongTien = 0;   
                   
                    data.sp_GiamDiemKhachHangKhiMuaHangSuDungDiem(donDatHang.IDKH, intDienDung);
                }


                int diem_tang = (int)donDatHang.TongTien / 10000;
                data.sp_ThemDiemKhachHangKhiMuaHang(donDatHang.IDKH, diem_tang);

            /*data.Entry(donDatHang).State = EntityState.Modified;       */
            Session["KhachHang"] = data.KhachHangs.FirstOrDefault(x => x.IDKH == khachHang.IDKH);
                data.SaveChanges();                
                Session.Remove("GioHang");
                Session.Remove("SoLuongHangTrongGioHang");
                return RedirectToAction("MuaThanhCong", "ThongBao");
            /*}
            catch
            {
                DonDatHang donDatHang = (DonDatHang)Session["DonDatHang"];
                data.DonDatHangs.Remove(donDatHang);
                Session.Remove("DonDatHang");
                Session.Remove("GioHang");
                Session.Remove("SoLuongHangTrongGioHang");
                return Content("<script language='javascript' type='text/javascript'>alert ('Vui lòng kiểm tra lại thông tin!');</script>");

            }         */
        }
    }
}