using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class BaoCaoTongKetMonBLL
    {
        public static List<MonHoc> LayDanhSachMonHoc()
        {
            return BaoCaoTongKetMonDAL.LayDanhSachMonHoc();
        }

        public static List<HocKy> LayDanhSachHocKy()
        {
            return BaoCaoTongKetMonDAL.LayDanhSachHocKy();
        }

        public static BaoCaoMonResult LapBaoCaoTongKetMon(string maMH, string maHK)
        {
            // BaoCaoDTO...
        }
    }
}
