using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public static class MonHocBLL
{
    public static List<MonHoc> LayDanhSachMonHoc()
    {
        return MonHocDAL.LayDanhSachMonHoc();
    }

    public static MonHoc? LayMonHocTheoMa(string maMH)
    {
        if (string.IsNullOrWhiteSpace(maMH))
            return null;
            
        return MonHocDAL.LayMonHocTheoMa(maMH);
    }

    public static (bool success, string message) CapNhatTenMonHoc(string maMH, string tenMoi)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(maMH))
            return (false, "Mã môn học không được để trống");

        if (string.IsNullOrWhiteSpace(tenMoi))
            return (false, "Tên môn học không được để trống");

        tenMoi = tenMoi.Trim();
        
        if (tenMoi.Length > 100)
            return (false, "Tên môn học không được vượt quá 100 ký tự");

        // Kiểm tra môn học có tồn tại không
        var monHoc = MonHocDAL.LayMonHocTheoMa(maMH);
        if (monHoc == null)
            return (false, "Không tìm thấy môn học");

        // Kiểm tra tên mới có trùng với môn học khác không
        if (MonHocDAL.KiemTraTenMonHocTonTai(tenMoi, maMH))
            return (false, "Tên môn học đã tồn tại");

        // Cập nhật
        bool result = MonHocDAL.CapNhatTenMonHoc(maMH, tenMoi);
        
        if (result)
            return (true, "Cập nhật tên môn học thành công");
        else
            return (false, "Có lỗi xảy ra khi cập nhật tên môn học");
    }
}
