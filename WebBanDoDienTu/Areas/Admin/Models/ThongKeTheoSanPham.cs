using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanDoDienTu.Areas.Admin.Models
{
    public class ThongKeTheoSanPham
    {
        public string TenSanPham { get; set; }
        public int SoLuongHangHoaBanDuoc { get; set; }
        public int SoTienHangHoaThuVe { get; set; }
        public int DoanhThuChoHangHoa { get; set; }

        public ThongKeTheoSanPham()
        {
        }
    }
}