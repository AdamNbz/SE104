using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public class BaoCaoTongKetHocKyBLL
{
    public static List<HocKy> LayDanhSachHocKy()
    {
        return BaoCaoTongKetMonDAL.LayDanhSachHocKy();
    }

    public static BaoCaoHocKyResult LapBaoCaoTongKetHocKy(string maHK)
    {
        int mocDiemDat = ThamSoDAL.LayMocDiemDat();
        return BaoCaoTongKetHocKyDAL.LapBaoCaoTongKetHocKy(maHK, mocDiemDat);
    }
}
