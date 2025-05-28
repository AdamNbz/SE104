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

    public static BaoCaoMonResult LapBaoCaoTongKetMon(string maMH, string maHK, int mocDiemDat)
    {
        var context = DataContext.Context;

        var danhSach = context.BANGDIEMMON.Include(b => b.HocSinh).Where(b => b.MaMH == maMH && b.MaHK == maHK).ToList();

        int tongSo = danhSach.Count;
        int soDat = danhSach.Count(b => b.DiemCuoiKy.HasValue && b.DiemCuoiKy >= mocDiemDat);
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