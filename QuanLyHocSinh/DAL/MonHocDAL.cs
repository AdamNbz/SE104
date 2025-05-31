using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

public static class MonHocDAL
{
    public static List<MonHoc> LayDanhSachMonHoc()
    {
        return DataContext.Context.MONHOC.ToList();
    }

    public static MonHoc? LayMonHocTheoMa(string maMH)
    {
        return DataContext.Context.MONHOC.FirstOrDefault(mh => mh.MaMH == maMH);
    }

    public static bool CapNhatTenMonHoc(string maMH, string tenMoi)
    {
        try
        {
            var monHoc = DataContext.Context.MONHOC.FirstOrDefault(mh => mh.MaMH == maMH);
            if (monHoc == null)
                return false;

            monHoc.TenMH = tenMoi;
            DataContext.Context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool KiemTraTenMonHocTonTai(string tenMH, string? maMHLoaiTru = null)
    {
        var query = DataContext.Context.MONHOC.Where(mh => mh.TenMH.ToLower() == tenMH.ToLower());
        
        if (!string.IsNullOrEmpty(maMHLoaiTru))
        {
            query = query.Where(mh => mh.MaMH != maMHLoaiTru);
        }
        
        return query.Any();
    }
}
