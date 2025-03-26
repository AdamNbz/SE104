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
        KiemTraHopLeVoiHocSinh(hs);

        if (HocSinhDAL.TiepNhanHocSinh(hs) == 1)
            return true;
        return false;
    }
    private static void KiemTraHopLeVoiHocSinh(HocSinh hs)
    {
        if (string.IsNullOrEmpty(hs.HoTen))
            throw new ArgumentException("Không được bỏ trống họ tên");
        if (string.IsNullOrEmpty(hs.GioiTinh))
            throw new ArgumentException("Không được bỏ trống giới tính");
        if (string.IsNullOrEmpty(hs.Email))
            throw new ArgumentException("Không được bỏ trống email");
        if (string.IsNullOrEmpty(hs.DiaChi))
            throw new ArgumentException("Không được bỏ trống địa chỉ");

        KiemTraNgaySinh(hs.NgaySinh);
    }

    private static void KiemTraNgaySinh(DateTime? ngaySinh)
    {
        if (ngaySinh == null)
            throw new ArgumentException("Không được bỏ trống ngày sinh");
        int tuoiToiThieu = ThamSoDAL.LayTuoiToiThieu();
        int tuoiToiDa = ThamSoDAL.LayTuoiToiDa();

        DateTime ngayToiDa = DateTime.Now.AddYears(-tuoiToiThieu);
        DateTime ngayToiThieu = DateTime.Now.AddYears(-tuoiToiDa);

        if (ngaySinh < ngayToiThieu || ngaySinh > ngayToiDa)
            throw new ArgumentException($"Tuổi phải từ {tuoiToiThieu} đến {tuoiToiDa}");
    }
}
