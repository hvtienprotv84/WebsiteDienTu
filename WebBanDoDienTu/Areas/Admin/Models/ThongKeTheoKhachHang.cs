using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanDoDienTu.Areas.Admin.Models
{
    public class ThongKeTheoKhachHang
    {
        public string TenKhachHang { get; set; }
        public int SoLuongHangHoaDaMua { get; set; }
        public int SoTienThuVeTuKhachHang { get; set; }
        public int DoanhThuChoKhachHang { get; set; }

        public ThongKeTheoKhachHang()
        {
        }
    }
}