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
    private static KetQuaBaoCaoHocKy? _currentBaoCaoHocKy;

    public static List<HocKy> LayDanhSachHocKy()
    {
        return BaoCaoTongKetMonDAL.LayDanhSachHocKy();
    }

    public static void LapBaoCaoTongKetHocKy(string maHK)
    {
        int mocDiemDat = ThamSoDAL.LayMocDiemDat();
        _currentBaoCaoHocKy = BaoCaoTongKetHocKyDAL.LapBaoCaoTongKetHocKy(maHK, mocDiemDat);
    }

    public static KetQuaBaoCaoHocKy? LayBaoCaoTongKetHocKy()
    {
        return _currentBaoCaoHocKy;
    }
}
