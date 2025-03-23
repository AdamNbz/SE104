using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

public static class HocSinhDAL
{
    public static int LaySoLuongHocSinh() => DataContext.Context.HOCSINH.Count();
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
