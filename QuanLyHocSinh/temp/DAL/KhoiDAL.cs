using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class KhoiDAL
    {
        public static List<Khoi> LayDanhSachKhoi()
        {
            return DataContext.Context.KHOI.ToList();
        }
    }
}
