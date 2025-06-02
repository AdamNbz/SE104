using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public static class LopDAL
{
    public static bool XepLopHocSinh(string maHS, string maLop)
    {
        try
        {
            var hocSinh = DataContext.Context.HOCSINH.FirstOrDefault(h => h.MaHS == maHS);
            var lop = DataContext.Context.LOP.FirstOrDefault(l => l.MaLop == maLop);
            if (hocSinh != null && lop != null)
            {
                hocSinh.MaLop = maLop;
                lop.HocSinhs?.Add(hocSinh);
                DataContext.Context.SaveChanges();
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public static List<Lop> LayDanhSachLop()
    {
        var context = DataContext.Context;
        return context.Set<Lop>().Include(l => l.Khoi).ToList();
    }

    public static List<Lop> LayDanhSachLopTheoKhoi(string maKhoi)
    {
        var context = DataContext.Context;
        return context.Set<Lop>().Include(l => l.Khoi).Where(l => l.MaKhoi == maKhoi).ToList();
    }

    public static Lop LayLopTheoMa(string maLop)
    {
        var context = DataContext.Context;
        return context.Set<Lop>().Include(l => l.Khoi).FirstOrDefault(l => l.MaLop == maLop);
    }

    public static string PhatSinhMaLop()
    {
        // Use fresh context to avoid cache issues
        using (var context = new DataContext())
        {
            var lastLopCode = context.Set<Lop>().Select(l => l.MaLop).OrderByDescending(code => code).FirstOrDefault();

            int nxt = 1;

            if (lastLopCode != null && lastLopCode.StartsWith("L"))
            {
                string numberPart = lastLopCode.Substring(1);
                if (int.TryParse(numberPart, out int cur))
                {
                    nxt = cur + 1;
                }
            }

            string newMaLop = $"L{nxt:D3}";
            System.Diagnostics.Debug.WriteLine($"Generated new MaLop: {newMaLop} (last was: {lastLopCode})");
            return newMaLop;
        }
    }

    public static bool ThemLop(Lop lop)
    {
        try
        {
            var context = DataContext.Context;

            if (context.Set<Lop>().Any(l => l.TenLop == lop.TenLop))
            {
                return false;
            }

            context.Set<Lop>().Add(lop);
            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool CapNhatLop(Lop lop)
    {
        try
        {
            var context = DataContext.Context;
            var existingLop = context.Set<Lop>().Find(lop.MaLop);
            if (existingLop == null) return false;

            if (context.Set<Lop>().Any(l => l.TenLop == lop.TenLop && l.MaLop != lop.MaLop))
            {
                return false;
            }

            existingLop.TenLop = lop.TenLop;
            existingLop.MaKhoi = lop.MaKhoi;

            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool CapNhatTenLop(string maLop, string tenLopMoi)
    {
        try
        {
            var context = DataContext.Context;
            var existingLop = context.Set<Lop>().Find(maLop);
            if (existingLop == null) return false;

            // Check if the updated class name already exists on another class
            if (context.Set<Lop>().Any(l => l.TenLop == tenLopMoi && l.MaLop != maLop))
            {
                return false;
            }

            existingLop.TenLop = tenLopMoi;

            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool XoaLop(string maLop)
    {
        try
        {
            var context = DataContext.Context;
            var lop = context.Set<Lop>().Find(maLop);
            if (lop == null) return false;

            if (context.Set<HocSinh>().Any(hs => hs.MaLop == maLop))
            {
                return false;
            }

            context.Set<Lop>().Remove(lop);
            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

