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
}
