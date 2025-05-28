using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public static class ThamSoDAL
{
    public static int LayTuoiToiDa() => DataContext.Context.THAMSO.First().TuoiToiDa;
    public static int LayTuoiToiThieu() => DataContext.Context.THAMSO.First().TuoiToiThieu;
    public static int LaySiSoToiDa() => DataContext.Context.THAMSO.First().SiSoToiDa;
    //public static int LayMocDiemDat() => DataContext.Context.THAMSO.First().MocDiemDat;
}
