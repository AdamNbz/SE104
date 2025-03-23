using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL;

public static class HocSinhDAL
{
    public static int LaySoLuongHocSinh()
    {
        var ds = LayDanhSachHocSinh();
        foreach (var hs in ds)
        {
            Debug.WriteLine($"Hoc sinh: {hs.HoTen}, Lop: {hs.Lop?.MaLop}");
        }
        return DataContext.Context.HOCSINH.Count();
    }
    public static int TiepNhanHocSinh(HocSinh hs)
    {
        try
        {
            DataContext.Context.HOCSINH.Add(hs);
            DataContext.Context.SaveChanges();
            return 1;
        }
        catch
        {
            return 0;
        }
    }

    public static ICollection<HocSinh> LayDanhSachHocSinh()
    {
        return DataContext.Context.HOCSINH.Include(hs => hs.Lop).ToList();
    }
}
