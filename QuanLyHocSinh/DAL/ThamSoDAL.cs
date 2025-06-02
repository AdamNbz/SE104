using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

public static class ThamSoDAL
{
    // Use fresh context to avoid cache issues
    public static int LayTuoiToiDa()
    {
        using (var context = new DataContext())
        {
            return context.THAMSO.First().TuoiToiDa;
        }
    }

    public static int LayTuoiToiThieu()
    {
        using (var context = new DataContext())
        {
            return context.THAMSO.First().TuoiToiThieu;
        }
    }

    public static int LayMocDiemDat()
    {
        using (var context = new DataContext())
        {
            var thamSo = context.Set<ThamSo>().FirstOrDefault();

            if (thamSo != null && thamSo.MocDiemDat == 0)
            {
                thamSo.MocDiemDat = 5;
                context.SaveChanges();
            }
            return thamSo?.MocDiemDat ?? 5;
        }
    }
}
