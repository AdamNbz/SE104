using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

public class MonHocDAL
{
    public static List<MonHoc> LayDanhSachMonHoc()
    {
        var context = DataContext.Context;
        return context.Set<MonHoc>().ToList();
    }

    public static MonHoc LayMonHocTheoMa(string maMH)
    {
        var context = DataContext.Context;
        return context.Set<MonHoc>().FirstOrDefault(m => m.MaMH == maMH);
    }

    public static bool CapNhatTenMonHoc(string maMH, string tenMoi)
    {
        try
        {
            var context = DataContext.Context;
            var monHoc = context.Set<MonHoc>().FirstOrDefault(m => m.MaMH == maMH);

            if (monHoc == null) return false;

            monHoc.TenMH = tenMoi;
            context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string PhatSinhMaMonHoc()
    {
        // Use fresh context to avoid cache issues
        using (var context = new DataContext())
        {
            var lastMonHocCode = context.Set<MonHoc>().Select(m => m.MaMH).OrderByDescending(code => code).FirstOrDefault();

            int nxt = 1;

            if (lastMonHocCode != null && lastMonHocCode.StartsWith("MH"))
            {
                string numberPart = lastMonHocCode.Substring(2);
                if (int.TryParse(numberPart, out int cur))
                {
                    nxt = cur + 1;
                }
            }

            string newMaMonHoc = $"MH{nxt:D3}";
            System.Diagnostics.Debug.WriteLine($"Generated new MaMonHoc: {newMaMonHoc} (last was: {lastMonHocCode})");
            return newMaMonHoc;
        }
    }

    public static bool ThemMonHoc(MonHoc monHoc)
    {
        try
        {
            var context = DataContext.Context;

            if (context.Set<MonHoc>().Any(m => m.TenMH == monHoc.TenMH))
            {
                return false; // Subject name already exists
            }

            context.Set<MonHoc>().Add(monHoc);
            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
