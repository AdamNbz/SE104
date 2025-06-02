using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public class MonHocBLL
{
    public static List<MonHoc> LayDanhSachMonHoc()
    {
        return MonHocDAL.LayDanhSachMonHoc();
    }

    public static MonHoc LayMonHocTheoMa(string maMH)
    {
        return MonHocDAL.LayMonHocTheoMa(maMH);
    }

    public static bool ThayDoiTenMonHoc(string maMH, string tenMoi)
    {
        if (string.IsNullOrWhiteSpace(maMH))
            throw new ArgumentException("Mã môn học không được để trống");

        if (string.IsNullOrWhiteSpace(tenMoi))
            throw new ArgumentException("Tên môn học không được để trống");

        var monHoc = MonHocDAL.LayMonHocTheoMa(maMH);
        if (monHoc == null) throw new Exception($"Không tìm thấy môn học với mã {maMH}");

        return MonHocDAL.CapNhatTenMonHoc(maMH, tenMoi);
    }

    public static string PhatSinhMaMonHoc()
    {
        return MonHocDAL.PhatSinhMaMonHoc();
    }

    public static bool ThemMonHocMoi(string tenMonHoc)
    {
        if (string.IsNullOrWhiteSpace(tenMonHoc))
        {
            throw new ArgumentException("Tên môn học không được để trống");
        }

        // Check if subject name already exists
        var existingMonHoc = MonHocDAL.LayDanhSachMonHoc()
            .FirstOrDefault(m => m.TenMH.Equals(tenMonHoc, StringComparison.OrdinalIgnoreCase));

        if (existingMonHoc != null)
        {
            return false; // Subject name already exists
        }

        string maMonHoc = PhatSinhMaMonHoc();

        MonHoc monHoc = new MonHoc
        {
            MaMH = maMonHoc,
            TenMH = tenMonHoc
        };

        return MonHocDAL.ThemMonHoc(monHoc);
    }
}
