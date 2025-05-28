using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
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
            return DataContext.Context.LOP.ToList();
        }
    }
}
