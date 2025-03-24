using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public static class HocSinhBLL
{
    public static string LayMaHocSinhTuDong()
    {
        string maHS = "HS";
        int soLuongHSHienTai = HocSinhDAL.LaySoLuongHocSinh();
        return maHS + string.Format("{0:D4}", soLuongHSHienTai + 1);
    }

    public static bool TiepNhanHocSinh(HocSinh hs)
    {
        if (!KiemTraHopLeVoiHocSinh(hs))
            return false;

        if (HocSinhDAL.TiepNhanHocSinh(hs) == 1)
            return true;
        return false;
    }
    private static bool KiemTraHopLeVoiHocSinh(HocSinh hs)
    {
        int tuoiToiThieu = ThamSoDAL.LayTuoiToiThieu();
        int tuoiToiDa = ThamSoDAL.LayTuoiToiDa();

        DateTime ngayToiDa = DateTime.Now.AddYears(-tuoiToiThieu);
        DateTime ngayToiThieu = DateTime.Now.AddYears(-tuoiToiDa);
        return hs.NgaySinh >= ngayToiThieu && hs.NgaySinh <= ngayToiDa;
    }
}
