using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL;

public static class BangDiemMonDAL
{
    public static void ThemBangDiem(BangDiemMon bangDiem)
    {
        var context = new DataContext();
        context.BANGDIEMMON.Add(bangDiem);
        context.SaveChanges();
    }

    public static void LuuDiem(BangDiemMon diem)
    {
        var context = DataContext.Context;
        var diemCu = context.BANGDIEMMON.FirstOrDefault(b => b.MaHocSinh == diem.MaHocSinh && b.MaMH == diem.MaMH && b.MaHK == diem.MaHK);

        if (diemCu != null)
        {
            diemCu.Diem15P = diem.Diem15P;
            diemCu.Diem1T = diem.Diem1T;
            diemCu.DiemCuoiKy = diem.DiemCuoiKy;
        }
        else
        {
            context.BANGDIEMMON.Add(diem);
        }

        context.SaveChanges();
    }

    public static List<BangDiemMon> LayDiemTheoHocSinh(string maHocSinh)
    {
        return DataContext.Context.BANGDIEMMON.Where(b => b.MaHocSinh == maHocSinh).Include(b => b.MonHoc).Include(b => b.HocKy).ToList();
    }

    public static List<BangDiemMon> LayDiem(string maHS, string maMH, string maHK)
    {
        var context = DataContext.Context;
        return context.BANGDIEMMON.Where(b => b.MaHocSinh == maHS && b.MaMH == maMH && b.MaHK == maHK).Include(b => b.MonHoc).Include(b => b.HocKy).ToList();
    }

    public static bool XoaDiem(string maHS, string maMH, string maHK)
    {
        var context = DataContext.Context;
        var diem = context.BANGDIEMMON.FirstOrDefault(b => b.MaHocSinh == maHS && b.MaMH == maMH && b.MaHK == maHK);

        if (diem != null)
        {
            context.BANGDIEMMON.Remove(diem);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    public static bool CapNhatBangDiem(string maHocSinh, string maHK, string maMonHoc, BangDiemMon bangDiem)
    {
        var context = DataContext.Context;
        var diem = context.BANGDIEMMON.FirstOrDefault(b => b.MaHocSinh == maHocSinh && b.MaMH == maMonHoc && b.MaHK == maHK);
        if (diem != null)
        {
            diem.Diem15P = bangDiem.Diem15P;
            diem.Diem1T = bangDiem.Diem1T;
            diem.DiemCuoiKy = bangDiem.DiemCuoiKy;
            context.SaveChanges();
            return true;
        }
        return false;
    }

    public static bool KiemTraMonHocCoDiem(string maMH)
    {
        var context = DataContext.Context;
        return context.BANGDIEMMON.Any(b => b.MaMH == maMH);
    }
}