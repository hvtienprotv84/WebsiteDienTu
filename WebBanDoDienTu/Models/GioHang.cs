using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanDoDienTu.Models
{
    public class Hang
    {
        public MatHang gioHang { get; set; }
        public int _soLuongHang { get; set; }

    }
    public class GioHang
    {
        List<Hang> listHang = new List<Hang>();

        public IEnumerable<Hang> ListHang
        {
            get { return listHang; }
        }

        public void Add(MatHang _matHang, int _soLuong = 1)
        {
            var i = listHang.FirstOrDefault(s => s.gioHang.IDMH == _matHang.IDMH);
            if (i == null)
            {
                listHang.Add(new Hang
                {
                    gioHang = _matHang,
                    _soLuongHang = _soLuong
                });
            }
            else
            {
                i._soLuongHang += _soLuong;
            }
        }

        public void Update_quantity(string id, int _quatity)
        {
            var i = listHang.Find(s => s.gioHang.IDMH.ToString() == id);
            if (i != null)
            {
                i._soLuongHang = _quatity;
            }
        }

        public double sum()
        {
            var sum = listHang.Sum(s => s._soLuongHang * 1);
            return sum;
        }

        public void Remove(int id)
        {
            listHang.RemoveAll(s => s.gioHang.IDMH == id);
        }

        public int Total()
        {
            return listHang.Sum(s => s._soLuongHang);
        }

        public int TongTien()
        {
            int tongtien = 0;
            foreach (Hang item in listHang)
            {
                tongtien = tongtien + (int)(item.gioHang.DonGia * item._soLuongHang);
            }
            return tongtien;
        }

        public void clear()
        {
            listHang.Clear();
        }

        public void ssss()
        {
            DateTime aDateTime = DateTime.Now;

            Console.WriteLine("Now is " + aDateTime);

            // Một khoảng thời gian. 
            // 1 giờ + 1 phút
            TimeSpan aInterval = new System.TimeSpan(0, 1, 1, 0);

            // Thêm một khoảng thời gian.
            DateTime newTime = aDateTime.Add(aInterval);
        }
    }
}