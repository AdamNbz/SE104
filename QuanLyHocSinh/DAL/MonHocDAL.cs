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
}
