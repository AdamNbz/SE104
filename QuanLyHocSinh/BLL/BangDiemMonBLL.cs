using DAL;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL;

public class BangDiemMonBLL
{
    public string MaHocSinh { get; set; } = "";
    public string MaMH { get; set; } = "";
    public string MaHK { get; set; } = "";
    public float? Diem15P { get; set; }
    public float? Diem1T { get; set; }
    public float? DiemCuoiKy { get; set; }

    // Constructor mặc định
    public BangDiemMonBLL() { }

    // Constructor với tham số
    public BangDiemMonBLL(string maHocSinh, string maMH, string maHK)
    {
        MaHocSinh = maHocSinh;
        MaMH = maMH;
        MaHK = maHK;
    }

    public BangDiemMonBLL TaoBangDiem(string MaHocSinh, string MaMH, string MaHk, string? Diem15P, string? Diem1T, string? DiemCuoiKy)
    {
        if (MaHocSinh == null || MaHK == null || MaMH == null) { return null; }

        float? Diem15PChuyenDoi = float.TryParse(Diem15P, out float output) ? output : null;
        float? Diem1TChuyenDoi = float.TryParse(Diem1T, out float output1) ? output1 : null;
        float? DiemCuoiKyChuyenDoi = float.TryParse(DiemCuoiKy, out float output2) ? output2 : null;

        KiemTraCacDieuKien(Diem15PChuyenDoi, Diem1TChuyenDoi, DiemCuoiKyChuyenDoi);

        BangDiemMonBLL result = new BangDiemMonBLL
        {
            MaHocSinh = MaHocSinh,
            MaMH = MaMH,
            MaHK = MaHk,
            Diem15P = Diem15PChuyenDoi,
            Diem1T = Diem1TChuyenDoi,
            DiemCuoiKy = DiemCuoiKyChuyenDoi
        };

        BangDiemMon bangDiem = new BangDiemMon
        {
            MaHocSinh = MaHocSinh,
            MaMH = MaMH,
            MaHK = MaHk,
            Diem15P = result.Diem15P,
            Diem1T = result.Diem1T,
            DiemCuoiKy = result.DiemCuoiKy
        };

        BangDiemMonDAL.ThemBangDiem(bangDiem);

        return result;
    }

    private void KiemTraCacDieuKien(float? Diem15P, float? Diem1T, float? DiemCuoiKy)
    {
        if ((Diem15P.HasValue && (Diem15P < 0 || Diem15P > 10)) ||
            (Diem1T.HasValue && (Diem1T < 0 || Diem1T > 10)) ||
            (DiemCuoiKy.HasValue && (DiemCuoiKy < 0 || DiemCuoiKy > 10)))
        {
            throw new Exception("Một trong các điểm thành phần không hợp lệ (phải từ 0-10)");
        }
    }

    public void XoaBangDiem(string MaHS, string MaMonHoc, string MaHK)
    {
        // BangDiemMonDAL.XoaBangDiem(MaHS, MaMonHoc, MaHK);
    }

    public void CapNhatBangDiem(string MaHS, string MaMonHoc, string MaHK, BangDiemMon BangDiemMoi)
    {
        var existingBangDiem = BangDiemMonDAL.LayDiemTheoHocSinh(MaHS).Where(b => b.MaMH == MaMonHoc && b.MaHK == MaHK).FirstOrDefault();

        if (existingBangDiem == null) BangDiemMonDAL.ThemBangDiem(BangDiemMoi);
        else BangDiemMonDAL.CapNhatBangDiem(MaHS, MaHK, MaMonHoc, BangDiemMoi);
    }

    public void TruyXuatBangDiem()
    {
        //LaybangDiem(string MaHS, string MaMonHoc, string MaHK);
    }

    public List<BangDiemMon> LayBangDiem(string MaHS, string MaMonHoc, string MaHK)
    {
        return BangDiemMonDAL.LayDiemTheoHocSinh(MaHS).Where(b => b.MaMH == MaMonHoc && b.MaHK == MaHK).ToList();
    }

}