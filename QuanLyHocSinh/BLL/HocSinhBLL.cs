using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        string ErrorMessage = "";
        if (string.IsNullOrEmpty(hs.HoTen))
        {
            ErrorMessage += "Họ tên không được để trống\n";
        }
        if (string.IsNullOrEmpty(hs.GioiTinh))
        {
            ErrorMessage += "Giới tính không được để trống\n";
        }
        if (string.IsNullOrEmpty(hs.Email))
        {
            ErrorMessage += "Email không được để trống\n";
        }
        if (!KiemTraTuoiHopLeVoiHocSinh(hs))
        {
            ErrorMessage += "Ngày sinh không hợp lệ\n";
        }
        if (string.IsNullOrEmpty(hs.DiaChi))
        {
            ErrorMessage += "Địa chỉ không được để trống\n";
        }
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            MessageBox.Show(ErrorMessage, "Tiếp nhận thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (HocSinhDAL.TiepNhanHocSinh(hs) == 1)
            return true;
        return false;
    }
    private static bool KiemTraTuoiHopLeVoiHocSinh(HocSinh hs)
    {
        int tuoiToiThieu = ThamSoDAL.LayTuoiToiThieu();
        int tuoiToiDa = ThamSoDAL.LayTuoiToiDa();

        DateTime ngayToiDa = DateTime.Now.AddYears(-tuoiToiThieu);
        DateTime ngayToiThieu = DateTime.Now.AddYears(-tuoiToiDa);
        return hs.NgaySinh >= ngayToiThieu && hs.NgaySinh <= ngayToiDa;
    }
}
