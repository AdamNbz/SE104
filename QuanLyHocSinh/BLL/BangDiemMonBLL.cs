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
    public required string MaHocSinh { get; set; }
    public required string MaMH { get; set; }
    public required string MaHK { get; set; }
    private float? Diem15P;
    private float? Diem1T;
    private float? DiemCuoiKy;


    public BangDiemMonBLL TaoBangDiem(string MaHocSinh, string MaMH, string MaHk, string Diem15P = "", string Diem1T = "", string DiemCuoiKy = "")
    {
        if (MaHocSinh == null) { return null; }
        if (MaHK == null) { return null; }
        if (MaMH == null) { return null; }
        float? Diem15PChuyenDoi = float.TryParse(Diem15P, out float output) ? output : null;
        float? Diem1TChuyenDoi = float.TryParse(Diem1T, out float output1) ? output1 : null;
        float? DiemCuoiKyChuyenDoi = float.TryParse(Diem1T, out float output2) ? output2 : null;
        KiemTraCacDieuKien(Diem15PChuyenDoi, Diem1TChuyenDoi, DiemCuoiKyChuyenDoi);
        BangDiemMonBLL result = new BangDiemMonBLL { MaHocSinh = MaHocSinh, MaMH = MaMH, MaHK = MaHk };
        if (Diem1T != null)
        {
            result.Diem1T = Diem1TChuyenDoi;
        }
        if (Diem15P != null)
        {
            result.Diem15P = Diem15PChuyenDoi;
        }
        if (DiemCuoiKy != null)
        {
            result.DiemCuoiKy = DiemCuoiKyChuyenDoi;
        }
        return result;
    }
    private void KiemTraCacDieuKien(float? Diem15P, float? Diem1T, float? DiemCuoiKy)
    {
        if (MaHK != "HK01" && MaHK != "HK02")
        {
            throw new Exception("Không tồn tại học kỳ này.");
        }
        if (Diem15P < 0 || Diem15P > 10 || Diem1T < 0 || Diem1T > 10 || DiemCuoiKy > 10 || DiemCuoiKy < 0)
        {
            throw new Exception("Mot Trong Cac Diem Thanh Phan Khong Hop Le");
        }
        //if(MaMH khong ton tai trong danh sach)
        //{
        //    throw new Exception("Khong Ton Tai Mon Hoc Nay");
        //}
    }

    public void XoaBangDiem(string MaHS, string MaMonHoc, string MaHK)
    {
        // BangDiemDAL.XoaBangDiem()
    }
    public void CapNhatBangDiem(string MaHS,string MaMonHoc,string MaHK, BangDiemMon BangDiemMoi)
    {
        BangDiemMonDAL.CapNhatBangDiem(MaHS, MaHK, MaMonHoc, BangDiemMoi);
    }
    public void TruyXuatBangDiem()
    {
        //LaybangDiem(string MaHS, string MaMonHoc, string MaHK);
    }
    public void LayBangDiem(string MaHS, string MaMonHoc, string MaHK)
    {
        BangDiemMonDAL.LayDiemTheoHocSinh(MaHS)
            .Where(b => b.MaMH == MaMonHoc && b.MaHK == MaHK)
            .ToList();
    }

}

