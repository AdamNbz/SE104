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
}
