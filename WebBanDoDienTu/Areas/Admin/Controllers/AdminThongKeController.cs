using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;
using WebBanDoDienTu.Areas.Admin.Models;
namespace WebBanDoDienTu.Areas.Admin.Controllers
{
    public class AdminThongKeController : Controller
    {
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();
        // GET: Admin/AdminThongKe
        public ActionResult Index()
        {
            return View();
        }

        // thống kê doanh thu theo khách hàng
        public ActionResult ThongKeDoanhThuTheoKhachHang(DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {

            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }

            List<KhachHang> khachHangList = data.sp_ThongKeDoanhThuTheoKhachHang(NgayBatDau, NgayKetThuc).ToList();
            List<ThongKeTheoKhachHang> listThongKe = new List<ThongKeTheoKhachHang>();
            int TongKhachhang = 0;
            int TongHangHoa = 0;
            int TongThuNhap = 0;
            foreach (var item in khachHangList)
            {
                if (data.DonDatHangs.FirstOrDefault(c => c.IDKH == item.IDKH) == null) continue;
                ThongKeTheoKhachHang tk = new ThongKeTheoKhachHang();
                tk.TenKhachHang = item.TenKH;
                tk.SoLuongHangHoaDaMua = int.Parse(data.DonDatHangs.Where(c => c.IDKH == item.IDKH).Sum(c => c.TongSoluong).ToString());
                tk.SoTienThuVeTuKhachHang = int.Parse(data.DonDatHangs.Where(c => c.IDKH == item.IDKH).Sum(c => c.TongTien).ToString());
                tk.DoanhThuChoKhachHang = int.Parse(data.DonDatHangs.Where(c => c.IDKH == item.IDKH).Sum(c => c.TongTien).ToString());
                listThongKe.Add(tk);
                TongKhachhang += 1;
                TongHangHoa += tk.SoLuongHangHoaDaMua;
                TongThuNhap += tk.SoTienThuVeTuKhachHang;
            }
            ViewBag.TongThuNhap = TongThuNhap;
            ViewBag.TongHangHoa = TongHangHoa;
            ViewBag.TongKhachHang = TongKhachhang;
            return View(listThongKe);
        }

        // Thống kê doang thu theo sản phẩm
        public ActionResult ThongKeDoanhThuTheoSanPham(DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            List<MatHang> matHangs = new List<MatHang>();
            matHangs = data.sp_ThongKeTheoDoanhThuTheoSanPham(NgayBatDau, NgayKetThuc).ToList();

            List<ThongKeTheoSanPham> listThongKe = new List<ThongKeTheoSanPham>();
            int TongHangHoa = 0;
            int TongThuNhap = 0;
            foreach (var item in matHangs)
            {
                if (matHangs == null) continue;

                ThongKeTheoSanPham sp = new ThongKeTheoSanPham();

                sp.TenSanPham = item.TenMH;

                sp.SoLuongHangHoaBanDuoc = int.Parse(data.ChiTietDonDatHangs.Where(c => c.IDMH == item.IDMH).Sum(c => c.SoluongMH).ToString());

                TongHangHoa += sp.SoLuongHangHoaBanDuoc;

                sp.SoTienHangHoaThuVe = (int)(item.DonGia * sp.SoLuongHangHoaBanDuoc);

                TongThuNhap += sp.SoTienHangHoaThuVe;

                sp.DoanhThuChoHangHoa = sp.SoTienHangHoaThuVe;

                listThongKe.Add(sp);
            }

            ViewBag.TongThuNhap = TongThuNhap;
            ViewBag.TongHangHoa = TongHangHoa;

            return View(listThongKe);
            /*return View();*/
        }
        public ActionResult ThongKeDoanhThu()
        {
            ViewBag.DonDatHangDangChoDuyet = data.DonDatHangs.Where(c => c.IDTrangThai == 1).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongKeDoanhThu(FormCollection form)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            /*if (form["NgayBatDau"] == null || form["NgayKetThuc"] == null || form["PhuongThucTT"] == null)
                return View();*/
            string NgayBatDau1 = form["NgayBatDau"];
            string NgayKetThuc1 = form["NgayKetThuc"];
            int PhuongThucThanhToan = int.Parse(form["PhuongThucTT"]);


            DateTime NgayBatDau = new DateTime(int.Parse(NgayBatDau1.Split('-')[0]), int.Parse(NgayBatDau1.Split('-')[1]), int.Parse(NgayBatDau1.Split('-')[2]));
            DateTime NgayKetThuc = new DateTime(int.Parse(NgayKetThuc1.Split('-')[0]), int.Parse(NgayKetThuc1.Split('-')[1]), int.Parse(NgayKetThuc1.Split('-')[2]));

            List<DonDatHang> donDatHangs = data.sp_LietKeDonDatHangTheoNgay(NgayBatDau, NgayKetThuc, PhuongThucThanhToan).ToList();

            int TongMatHang = 0;
            int TongThuNhap = 0;

            foreach (DonDatHang item in donDatHangs)
            {
                TongMatHang += item.TongSoluong.Value;
                TongThuNhap += item.TongTien.Value;
            }
            ViewBag.NgayBatDau = NgayBatDau1;

            ViewBag.NgayKetThuc = NgayKetThuc1;

            ViewBag.TongMatHang = TongMatHang;
            
            ViewBag.TongThuNhap = TongThuNhap;

            

            return View();
        }

        // thống kê doanh thu theo khách hàng
        public ActionResult ThongKeDoanhThuTheoKhachHangLe(DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {

            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "AdminLogin");
            }

            List<DonDatHang> DonDatHangList = data.sp_ThongKeTheoDoanhThuTheoKhachHangLe(NgayBatDau, NgayKetThuc).ToList();
            List<ThongKeTheoKhachHang> listThongKe = new List<ThongKeTheoKhachHang>();
            int TongKhachhang = 0;
            int TongHangHoa = 0;
            int TongThuNhap = 0;
            foreach (var item in DonDatHangList)
            {
                if (data.ChiTietDonDatHangs.Where(c => c.IDDDH == item.IDDDH).Count() == 0) continue;
                ThongKeTheoKhachHang tk = new ThongKeTheoKhachHang();
                tk.TenKhachHang = item.TenKHKhongAccount;
                tk.SoLuongHangHoaDaMua = int.Parse(data.DonDatHangs.Where(c => c.DienThoaiKhongAccount == item.DienThoaiKhongAccount).Sum(c => c.TongSoluong).ToString());
                tk.SoTienThuVeTuKhachHang = int.Parse(data.DonDatHangs.Where(c => c.DienThoaiKhongAccount == item.DienThoaiKhongAccount).Sum(c => c.TongTien).ToString());
                tk.DoanhThuChoKhachHang = int.Parse(data.DonDatHangs.Where(c => c.DienThoaiKhongAccount == item.DienThoaiKhongAccount).Sum(c => c.TongTien).ToString());
                listThongKe.Add(tk);
                TongKhachhang += 1;
                TongHangHoa += tk.SoLuongHangHoaDaMua;
                TongThuNhap += tk.SoTienThuVeTuKhachHang;
            }
            ViewBag.TongThuNhap = TongThuNhap;
            ViewBag.TongHangHoa = TongHangHoa;
            ViewBag.TongKhachHang = TongKhachhang;
            return View(listThongKe);
        }
    }
}