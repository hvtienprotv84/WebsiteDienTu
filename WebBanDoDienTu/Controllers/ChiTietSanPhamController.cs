using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;

namespace WebBanDoDienTu.Controllers
{
    public class ChiTietSanPhamController : Controller
    {
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();
        // GET: ChiTietSanPham
        public ActionResult Index(int id)
        {
            ViewBag.BinhLuan = data.ChiTietDonDatHangs.Where(c => c.IDMH == id).ToList();
            return View(data.MatHangs.FirstOrDefault(c => c.IDMH == id));

        }
    }
}