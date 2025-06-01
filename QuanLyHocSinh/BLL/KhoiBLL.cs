using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;
public class KhoiBLL
{
    private static readonly List<Khoi> DanhSachKhoi = KhoiDAL.LayDanhSachKhoi();

    public static List<Khoi> GetDanhSachKhoi() => DanhSachKhoi;

    public Khoi LayKhoiTheoMa(string maKhoi)
    {
        if (string.IsNullOrEmpty(maKhoi))
        {
            throw new ArgumentException("Mã khối không được để trống");
        }

        return KhoiDAL.LayKhoiTheoMa(maKhoi);
    }

    public static bool ThemKhoi(string maKhoi, string tenKhoi)
    {
        if (string.IsNullOrWhiteSpace(maKhoi))
        {
            throw new ArgumentException("Mã khối không được để trống");
        }

        if (string.IsNullOrWhiteSpace(tenKhoi))
        {
            throw new ArgumentException("Tên khối không được để trống");
        }

        Khoi khoi = new Khoi
        {
            MaKhoi = maKhoi,
            TenKhoi = tenKhoi
        };

        return KhoiDAL.ThemKhoi(khoi);
    }

    public static bool CapNhatKhoi(string maKhoi, string tenKhoi)
    {
        if (string.IsNullOrWhiteSpace(maKhoi))
        {
            throw new ArgumentException("Mã khối không được để trống");
        }

        if (string.IsNullOrWhiteSpace(tenKhoi))
        {
            throw new ArgumentException("Tên khối không được để trống");
        }

        Khoi khoi = new Khoi
        {
            MaKhoi = maKhoi,
            TenKhoi = tenKhoi
        };

        return KhoiDAL.CapNhatKhoi(khoi);
    }

    public bool XoaKhoi(string maKhoi)
    {
        if (string.IsNullOrEmpty(maKhoi))
        {
            throw new ArgumentException("Mã khối không được để trống");
        }

        return KhoiDAL.XoaKhoi(maKhoi);
    }
}
