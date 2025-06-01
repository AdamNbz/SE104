using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public class QuyDinhBLL
{
    public bool CapNhatQuyDinhTuoi(int tuoiToiThieu, int tuoiToiDa)
    {
        // Validation
        if (tuoiToiThieu <= 0 || tuoiToiDa <= 0)
        {
            throw new ArgumentException("Tuổi phải lớn hơn 0");
        }

        if (tuoiToiThieu >= tuoiToiDa)
        {
            throw new ArgumentException("Tuổi tối thiểu phải nhỏ hơn tuổi tối đa");
        }

        // Call DAL to update
        return QuyDinhDAL.CapNhatQuyDinhTuoi(tuoiToiThieu, tuoiToiDa);
    }

    // QD2: Thay đổi sĩ số tối đa của các lớp
    public bool CapNhatSiSoToiDa(int siSoToiDa)
    {
        // Validation
        if (siSoToiDa <= 0)
        {
            throw new ArgumentException("Sĩ số tối đa phải lớn hơn 0");
        }

        // Call DAL to update
        return QuyDinhDAL.CapNhatSiSoToiDa(siSoToiDa);
    }

    // QD4: Thay đổi số lượng và tên các môn học
    public bool CapNhatMonHoc(MonHoc monHoc)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(monHoc.MaMH))
        {
            throw new ArgumentException("Mã môn học không được để trống");
        }

        if (string.IsNullOrWhiteSpace(monHoc.TenMH))
        {
            throw new ArgumentException("Tên môn học không được để trống");
        }

        // Call DAL to update
        return QuyDinhDAL.CapNhatMonHoc(monHoc);
    }

    public bool ThemMonHoc(MonHoc monHoc)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(monHoc.MaMH))
        {
            throw new ArgumentException("Mã môn học không được để trống");
        }

        if (string.IsNullOrWhiteSpace(monHoc.TenMH))
        {
            throw new ArgumentException("Tên môn học không được để trống");
        }

        // Check if subject already exists
        var existingMonHoc = MonHocDAL.LayMonHocTheoMa(monHoc.MaMH);
        if (existingMonHoc != null)
        {
            throw new ArgumentException("Môn học với mã này đã tồn tại");
        }

        // Call DAL to add
        return QuyDinhDAL.ThemMonHoc(monHoc);
    }

    public bool XoaMonHoc(string maMH)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(maMH))
        {
            throw new ArgumentException("Mã môn học không được để trống");
        }

        // Check if subject exists
        var existingMonHoc = MonHocDAL.LayMonHocTheoMa(maMH);
        if (existingMonHoc == null)
        {
            throw new ArgumentException("Môn học với mã này không tồn tại");
        }

        // Check if subject has associated scores
        if (BangDiemMonDAL.KiemTraMonHocCoDiem(maMH))
        {
            throw new ArgumentException("Không thể xóa môn học đã có điểm số");
        }

        // Call DAL to delete
        return QuyDinhDAL.XoaMonHoc(maMH);
    }

    // QD5: Thay đổi điểm chuẩn đạt môn
    public bool CapNhatDiemChuanDatMon(int diemChuanDatMon)
    {
        // Validation
        if (diemChuanDatMon < 0 || diemChuanDatMon > 10)
        {
            throw new ArgumentException("Điểm chuẩn đạt môn phải từ 0 đến 10");
        }

        // Call DAL to update
        return QuyDinhDAL.CapNhatDiemChuanDatMon(diemChuanDatMon);
    }

    // Lấy các quy định hiện tại
    public ThamSo LayQuyDinh()
    {
        return QuyDinhDAL.LayQuyDinh();
    }

    // Lấy danh sách môn học
    public List<MonHoc> LayDanhSachMonHoc()
    {
        return MonHocDAL.LayDanhSachMonHoc();
    }
}
