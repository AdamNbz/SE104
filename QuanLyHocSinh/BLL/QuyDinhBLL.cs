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
        if (tuoiToiThieu <= 0 || tuoiToiDa <= 0)
        {
            throw new ArgumentException("Tuổi phải lớn hơn 0");
        }

        if (tuoiToiThieu >= tuoiToiDa)
        {
            throw new ArgumentException("Tuổi tối thiểu phải nhỏ hơn tuổi tối đa");
        }

        return QuyDinhDAL.CapNhatQuyDinhTuoi(tuoiToiThieu, tuoiToiDa);
    }

    public bool CapNhatSiSoToiDa(int siSoToiDa)
    {
        if (siSoToiDa <= 0)
        {
            throw new ArgumentException("Sĩ số tối đa phải lớn hơn 0");
        }

        return QuyDinhDAL.CapNhatSiSoToiDa(siSoToiDa);
    }

    public bool CapNhatMonHoc(MonHoc monHoc)
    {
        if (string.IsNullOrWhiteSpace(monHoc.MaMH))
        {
            throw new ArgumentException("Mã môn học không được để trống");
        }

        if (string.IsNullOrWhiteSpace(monHoc.TenMH))
        {
            throw new ArgumentException("Tên môn học không được để trống");
        }

        return QuyDinhDAL.CapNhatMonHoc(monHoc);
    }

    public bool ThemMonHoc(MonHoc monHoc)
    {
        if (string.IsNullOrWhiteSpace(monHoc.MaMH))
        {
            throw new ArgumentException("Mã môn học không được để trống");
        }

        if (string.IsNullOrWhiteSpace(monHoc.TenMH))
        {
            throw new ArgumentException("Tên môn học không được để trống");
        }

        var existingMonHoc = MonHocDAL.LayMonHocTheoMa(monHoc.MaMH);
        if (existingMonHoc != null)
        {
            throw new ArgumentException("Môn học với mã này đã tồn tại");
        }

        return QuyDinhDAL.ThemMonHoc(monHoc);
    }

    public bool XoaMonHoc(string maMH)
    {
        if (string.IsNullOrWhiteSpace(maMH))
        {
            throw new ArgumentException("Mã môn học không được để trống");
        }

        var existingMonHoc = MonHocDAL.LayMonHocTheoMa(maMH);
        if (existingMonHoc == null)
        {
            throw new ArgumentException("Môn học với mã này không tồn tại");
        }

        if (BangDiemMonDAL.KiemTraMonHocCoDiem(maMH))
        {
            throw new ArgumentException("Không thể xóa môn học đã có điểm số");
        }

        return QuyDinhDAL.XoaMonHoc(maMH);
    }

    public bool CapNhatDiemChuanDatMon(int diemChuanDatMon)
    {
        if (diemChuanDatMon < 0 || diemChuanDatMon > 10)
        {
            throw new ArgumentException("Điểm chuẩn đạt môn phải từ 0 đến 10");
        }

        return QuyDinhDAL.CapNhatDiemChuanDatMon(diemChuanDatMon);
    }

    public ThamSo LayQuyDinh()
    {
        return QuyDinhDAL.LayQuyDinh();
    }

    public List<MonHoc> LayDanhSachMonHoc()
    {
        return MonHocDAL.LayDanhSachMonHoc();
    }
}
