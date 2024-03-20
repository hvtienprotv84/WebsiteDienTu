using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;
using WebBanDoDienTu.Areas.Admin.Models;

namespace WebBanDoDienTu.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();
        // GET: Admin/AdminHome
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            Session["HoaDonCho"] = data.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
            ViewBag.SoHoaDonDangCho = data.DonDatHangs.Where(c => c.TrangThai.TenTrangThai.Equals("Chờ duyệt đơn hàng")).Count();
            ViewBag.TongSoHoaDon = data.DonDatHangs.Count();
            ViewBag.TongSoMatHang = data.MatHangs.Count();
            ViewBag.TongSoTheLoai = data.LoaiMatHangs.Count();
            ViewBag.TongSoKhachHang = data.KhachHangs.Count();
            // thongke
            List<KhachHang> khachHangList = data.KhachHangs.ToList();
            List<ThongKeTheoKhachHang> listThongKe = new List<ThongKeTheoKhachHang>();
            foreach (var item in khachHangList)
            {
                if (data.DonDatHangs.FirstOrDefault(c => c.IDKH == item.IDKH) == null) continue;
                ThongKeTheoKhachHang tk = new ThongKeTheoKhachHang();
                tk.TenKhachHang = item.TenKH;
                tk.SoLuongHangHoaDaMua = int.Parse(data.DonDatHangs.Where(c => c.IDKH == item.IDKH).Sum(c => c.TongSoluong).ToString());
                tk.SoTienThuVeTuKhachHang = int.Parse(data.DonDatHangs.Where(c => c.IDKH == item.IDKH).Sum(c => c.TongTien).ToString());
                tk.DoanhThuChoKhachHang = int.Parse(data.DonDatHangs.Where(c => c.IDKH == item.IDKH).Sum(c => c.TongTien).ToString());
                listThongKe.Add(tk);
            }
            ViewBag.TktheoKhachHang = listThongKe;


            // thong ke theo san pham    


            List<MatHang> matHangs = data.sp_ThongKeTheoDoanhThuTheoSanPham(DateTime.Today.AddDays(-7), DateTime.Today).ToList();
            List<ThongKeTheoSanPham> listThongKe1 = new List<ThongKeTheoSanPham>();
            foreach (var item in matHangs)
            {
                if (matHangs == null) continue;
                ThongKeTheoSanPham sp = new ThongKeTheoSanPham();

                sp.TenSanPham = item.TenMH;

                sp.SoLuongHangHoaBanDuoc = int.Parse(data.ChiTietDonDatHangs.Where(c => c.IDMH == item.IDMH).Sum(c => c.SoluongMH).ToString());

                sp.SoTienHangHoaThuVe = (int)(item.DonGia * sp.SoLuongHangHoaBanDuoc);

                sp.DoanhThuChoHangHoa = sp.SoTienHangHoaThuVe;

                listThongKe1.Add(sp);
            }

            ViewBag.TkTheoSanPham = listThongKe1;

            return View();
        }
    }
}