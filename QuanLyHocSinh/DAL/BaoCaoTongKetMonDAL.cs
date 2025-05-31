using DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL;

public static class BaoCaoTongKetMonDAL
{

    public static List<MonHoc> LayDanhSachMonHoc()
    {
        return DataContext.Context.Set<MonHoc>().ToList();
    }

    public static List<HocKy> LayDanhSachHocKy()
    {
        return DataContext.Context.Set<HocKy>().ToList();
    }

    public static double? TinhDiemTrungBinhMon(string maHocSinh, string maMH, string maHK)
    {
        var context = DataContext.Context;

        var bangDiem = context.BANGDIEMMON.FirstOrDefault(b => b.MaHocSinh == maHocSinh && b.MaMH == maMH && b.MaHK == maHK);

        if (bangDiem == null)
        {
            return null;
        }

        int heSoDiem15P = 1;
        int heSoDiem1T = 2;
        int heSoDiemCuoiKy = 3;

        double tongDiem = 0;
        int tongHeSo = 0;

        if (bangDiem.Diem15P.HasValue)
        {
            tongDiem += bangDiem.Diem15P.Value * heSoDiem15P;
            tongHeSo += heSoDiem15P;
        }

        if (bangDiem.Diem1T.HasValue)
        {
            tongDiem += bangDiem.Diem1T.Value * heSoDiem1T;
            tongHeSo += heSoDiem1T;
        }

        if (bangDiem.DiemCuoiKy.HasValue)
        {
            tongDiem += bangDiem.DiemCuoiKy.Value * heSoDiemCuoiKy;
            tongHeSo += heSoDiemCuoiKy;
        }

        if (tongHeSo == 0)
        {
            return null;
        }

        return tongDiem / tongHeSo;
    }

    public static BaoCaoMonResult LapBaoCaoTongKetMon(string maMH, string maHK, int mocDiemDat)
    {
        var context = DataContext.Context;

        var danhSach = context.BANGDIEMMON.Include(b => b.HocSinh).Where(b => b.MaMH == maMH && b.MaHK == maHK).ToList();

        int tongSo = danhSach.Count;
        int soDat = 0;

        foreach (var bangDiem in danhSach)
        {
            var diemTrungBinh = TinhDiemTrungBinhMon(bangDiem.MaHocSinh, maMH, maHK);
            if (diemTrungBinh.HasValue && diemTrungBinh >= mocDiemDat)
            {
                soDat++;
            }
        }

        int soKhongDat = tongSo - soDat;
        double tyLeDat = tongSo > 0 ? (soDat * 100.0 / tongSo) : 0;

        return new BaoCaoMonResult
        {
            TongSo = tongSo,
            SoLuongDat = soDat,
            SoLuongKhongDat = soKhongDat,
            TyLeDat = tyLeDat,
            ChiTietBangDiem = danhSach
        };
    }
}