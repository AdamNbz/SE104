using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

public static class KhoiDAL
{
    public static List<Khoi> LayDanhSachKhoi()
    {
        using var context = DataContext.Context;
        return context.Set<Khoi>().Include(k => k.Lops).ToList();
    }

    public static Khoi LayKhoiTheoMa(string maKhoi)
    {
        using var context = DataContext.Context;
        return context.Set<Khoi>().Include(k => k.Lops).FirstOrDefault(k => k.MaKhoi == maKhoi);
    }

    public static bool ThemKhoi(Khoi khoi)
    {
        try
        {
            using var context = DataContext.Context;
            context.Set<Khoi>().Add(khoi);
            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool CapNhatKhoi(Khoi khoi)
    {
        try
        {
            using var context = DataContext.Context;
            var existingKhoi = context.Set<Khoi>().Find(khoi.MaKhoi);
            if (existingKhoi == null) return false;

            existingKhoi.TenKhoi = khoi.TenKhoi;
            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool XoaKhoi(string maKhoi)
    {
        try
        {
            using var context = DataContext.Context;
            var khoi = context.Set<Khoi>().Find(maKhoi);
            if (khoi == null) return false;

            context.Set<Khoi>().Remove(khoi);
            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
