using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public static class HocSinhBLL
{
    public static void GetAllHocSinh()
    {
        using (var context = new DAL.DataContext())
        {
            var hocSinhs = context.HocSinhs.ToList();
            foreach (var hocSinh in hocSinhs)
            {
                Trace.WriteLine(hocSinh.HoTen);
            }
        }   
    }
}
