using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public class BaoCaoTongKetMonBLL
{
    private static BaoCaoMonResult? _currentBaoCaoMon;

    public static List<MonHoc> LayDanhSachMonHoc()
    {
        return BaoCaoTongKetMonDAL.LayDanhSachMonHoc();
    }

    public static List<HocKy> LayDanhSachHocKy()
    {
        return BaoCaoTongKetMonDAL.LayDanhSachHocKy();
    }

    public static void LapBaoCaoTongKetMon(string maMH, string maHK)
    {
        int mocDiemDat = ThamSoDAL.LayMocDiemDat();
        _currentBaoCaoMon = BaoCaoTongKetMonDAL.LapBaoCaoTongKetMon(maMH, maHK, mocDiemDat);
    }

    public static BaoCaoMonResult? LayBaoCaoTongKetMon()
    {
        return _currentBaoCaoMon;
    }
}
