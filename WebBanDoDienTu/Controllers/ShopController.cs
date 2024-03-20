using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDoDienTu.Models;

namespace WebBanDoDienTu.Controllers
{
    public class ShopController : Controller
    {
        WebBanDoDienTuEntities data = new WebBanDoDienTuEntities();
        // GET: Shop

        public ActionResult Index(string idLoaiMH, string TenMatHang, int page = 1, int pagelist = 6)
        {
            ViewBag.Category = data.LoaiMatHangs.ToList();
            if (idLoaiMH != null)
            {
                int a = int.Parse(idLoaiMH.ToString());
                /*ViewBag.MatHangTheoTheLoai = data.MatHangs.Where(c => c.IDLoaiMH == a).ToList();*/
                return View(data.MatHangs.Where(c => c.IDLoaiMH == a && c.SoLuong > 0).ToList().OrderByDescending(c => c.IDMH).ToPagedList(page, pagelist));
            }
            else
            if (TenMatHang != null)
            {
                return View(data.MatHangs.Where(c => c.TenMH.ToLower().Contains(TenMatHang.ToLower()) && c.SoLuong > 0).OrderByDescending(c => c.IDMH).ToPagedList(page, pagelist));
            }
            else
            {
                return View(data.MatHangs.Where(c => c.SoLuong > 0).OrderByDescending(x => x.IDLoaiMH).ToPagedList(page, pagelist));
            }
            return View(data.MatHangs.Where(c => c.SoLuong > 0).OrderByDescending(x => x.IDLoaiMH).ToPagedList(page, pagelist));

        }
    }
}