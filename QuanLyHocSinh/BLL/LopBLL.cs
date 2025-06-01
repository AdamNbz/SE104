using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public static class LopBLL
{
    private static readonly List<Lop> DanhSachLop = LopDAL.LayDanhSachLop();
    private static readonly List<Khoi> DanhSachKhoiLop = KhoiDAL.LayDanhSachKhoi();

    public static List<Lop> GetDanhSachLop() => DanhSachLop;

    public static List<Khoi> GetDanhSachKhoiLop() => DanhSachKhoiLop;

    public static int TinhSiSo(List<HocSinh> HocSinhList, string MaLop)
    {
        return DataContext.Context.HOCSINH.Count(h => h.MaLop == MaLop);
    }

    public static List<HocSinh> LayDanhSachHocSinh(string MaLop)
    {
        return DataContext.Context.HOCSINH.Where(h => h.MaLop == MaLop).ToList();
    }

    public static List<Lop> LayDanhSachLopTheoKhoi(string maKhoi)
    {
        if (string.IsNullOrWhiteSpace(maKhoi))
        {
            throw new ArgumentException("Mã khối không được để trống");
        }

        return LopDAL.LayDanhSachLopTheoKhoi(maKhoi);
    }

    public static Lop LayLopTheoMa(string maLop)
    {
        if (string.IsNullOrWhiteSpace(maLop))
        {
            throw new ArgumentException("Mã lớp không được để trống");
        }

        return LopDAL.LayLopTheoMa(maLop);
    }

    public static string PhatSinhMaLop()
    {
        return LopDAL.PhatSinhMaLop();
    }

    public static bool ThemLopMoi(string tenLop, string maKhoi)
    {
        if (string.IsNullOrWhiteSpace(tenLop))
        {
            throw new ArgumentException("Tên lớp không được để trống");
        }

        if (string.IsNullOrWhiteSpace(maKhoi))
        {
            throw new ArgumentException("Mã khối không được để trống");
        }

        var khoi = KhoiDAL.LayKhoiTheoMa(maKhoi);
        if (khoi == null)
        {
            throw new ArgumentException("Khối học không tồn tại");
        }

        var thamSo = QuyDinhDAL.LayQuyDinh();
        int siSoToiDa = thamSo?.SiSoToiDa ?? 40;

        string maLop = PhatSinhMaLop();

        Lop lop = new Lop
        {
            MaLop = maLop,
            TenLop = tenLop,
            MaKhoi = maKhoi
        };

        return LopDAL.ThemLop(lop);
    }

    public static bool CapNhatLop(string maLop, string tenLop, string maKhoi)
    {
        if (string.IsNullOrEmpty(maLop))
        {
            throw new ArgumentException("Mã lớp không được để trống");
        }

        if (string.IsNullOrWhiteSpace(tenLop))
        {
            throw new ArgumentException("Tên lớp không được để trống");
        }

        if (string.IsNullOrWhiteSpace(maKhoi))
        {
            throw new ArgumentException("Mã khối không được để trống");
        }

        Lop lop = new Lop
        {
            MaLop = maLop,
            TenLop = tenLop,
            MaKhoi = maKhoi
        };

        return LopDAL.CapNhatLop(lop);
    }

    public static bool CapNhatTenLop(string maLop, string tenLopMoi)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(maLop))
        {
            throw new ArgumentException("Mã lớp không được để trống");
        }

        if (string.IsNullOrWhiteSpace(tenLopMoi))
        {
            throw new ArgumentException("Tên lớp mới không được để trống");
        }

        var lopHienTai = DanhSachLop.FirstOrDefault(l => l.MaLop == maLop);
        if (lopHienTai == null)
        {
            throw new ArgumentException("Không tìm thấy lớp cần cập nhật");
        }

        if (DanhSachLop.Any(l => l.MaLop != maLop && l.TenLop.Equals(tenLopMoi, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException("Tên lớp đã tồn tại");
        }

        bool result = LopDAL.CapNhatTenLop(maLop, tenLopMoi);

        if (result)
        {
            lopHienTai.TenLop = tenLopMoi;
        }

        return result;
    }

    public static bool XoaLop(string maLop)
    {
        if (string.IsNullOrEmpty(maLop))
        {
            throw new ArgumentException("Mã lớp không được để trống");
        }

        return LopDAL.XoaLop(maLop);
    }

    public static void LamMoiDanhSachLop()
    {
        DanhSachLop.Clear();
        DanhSachLop.AddRange(LopDAL.LayDanhSachLop());
    }
}
