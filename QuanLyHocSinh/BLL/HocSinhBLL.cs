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
    public static void GetAllHocSinh()
    {
        DataContext.Context.HOCSINH.ToList().ForEach(hs => {
            Debug.WriteLine($"{hs.HoTen} {hs.MaHS}");
        });
    }

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

        HocSinhDAL.TiepNhanHocSinh(hs);
        return true;
    }
    private static bool KiemTraHopLeVoiHocSinh(HocSinh hs)
    {
        int tuoiToiThieu = DAL.ThamSoDAL.LayTuoiToiThieu();
        int tuoiToiDa = DAL.ThamSoDAL.LayTuoiToiDa();

        DateTime ngayToiDa = DateTime.Now.AddYears(-tuoiToiThieu);
        DateTime ngayToiThieu = DateTime.Now.AddYears(-tuoiToiDa);
        return hs.NgaySinh >= ngayToiThieu && hs.NgaySinh <= ngayToiDa;
    }
}
