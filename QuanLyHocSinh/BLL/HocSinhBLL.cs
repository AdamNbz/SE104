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
        if (hs.Email == "")
        {
            MessageBox.Show("EMAIL KHÔNG HỢP LỆ", "TIẾP NHẬN THẤT BẠI", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        else if (hs.GioiTinh == "")
        {
            MessageBox.Show("GIOITINH KHÔNG HỢP LỆ", "TIẾP NHẬN THẤT BẠI", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        else if (hs.DiaChi == "")
        {
            MessageBox.Show("DIACHI KHÔNG HỢP LỆ", "TIẾP NHẬN THẤT BẠI", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        else if (hs.HoTen == "")
        {
            MessageBox.Show("HOTEN KHÔNG HỢP LỆ", "TIẾP NHẬN THẤT BẠI", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        else if (hs.MaHS == "")
        {
            MessageBox.Show("MAHS KHÔNG HỢP LỆ", "TIẾP NHẬN THẤT BẠI", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        else if (!KiemTraTuoiHopLeVoiHocSinh(hs))
        {
            MessageBox.Show("Tuoi KHÔNG HỢP LỆ", "TIẾP NHẬN THẤT BẠI", MessageBoxButton.OK, MessageBoxImage.Error);
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
