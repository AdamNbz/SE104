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
    public static int LayTuoiToiDa() => DataContext.Context.THAMSO.First().TuoiToiDa;
    public static int LayTuoiToiThieu() => DataContext.Context.THAMSO.First().TuoiToiThieu;
    public static int LayMocDiemDat()
    {
        var thamSo = DataContext.Context.Set<ThamSo>().FirstOrDefault();

        if (thamSo.MocDiemDat == 0)
        {
            thamSo.MocDiemDat = 5;
            DataContext.Context.SaveChanges();
        }
        return thamSo.MocDiemDat;
    }
}
