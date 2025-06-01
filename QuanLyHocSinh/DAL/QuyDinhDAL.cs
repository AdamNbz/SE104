using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

public class QuyDinhDAL
{
    public static ThamSo LayQuyDinh()
    {
        using var context = DataContext.Context;
        var thamSo = context.THAMSO.FirstOrDefault();

        if (thamSo == null)
        {
            thamSo = new ThamSo
            {
                TuoiToiThieu = 15,
                TuoiToiDa = 20,
                SiSoToiDa = 40,
                MocDiemDat = 5
            };
            context.THAMSO.Add(thamSo);
            context.SaveChanges();
        }

        return thamSo;
    }

    public static bool CapNhatQuyDinhTuoi(int tuoiToiThieu, int tuoiToiDa)
    {
        try
        {
            using var context = DataContext.Context;
            var thamSo = context.THAMSO.FirstOrDefault();

            if (thamSo == null)
            {
                thamSo = new ThamSo();
                context.THAMSO.Add(thamSo);
            }

            thamSo.TuoiToiThieu = tuoiToiThieu;
            thamSo.TuoiToiDa = tuoiToiDa;

            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool CapNhatSiSoToiDa(int siSoToiDa)
    {
        try
        {
            using var context = DataContext.Context;
            var thamSo = context.THAMSO.FirstOrDefault();

            if (thamSo == null)
            {
                thamSo = new ThamSo();
                context.THAMSO.Add(thamSo);
            }

            thamSo.SiSoToiDa = siSoToiDa;

            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool CapNhatDiemChuanDatMon(int diemChuanDatMon)
    {
        try
        {
            using var context = DataContext.Context;
            var thamSo = context.THAMSO.FirstOrDefault();

            if (thamSo == null)
            {
                thamSo = new ThamSo();
                context.THAMSO.Add(thamSo);
            }

            thamSo.MocDiemDat = diemChuanDatMon;

            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool CapNhatMonHoc(MonHoc monHoc)
    {
        try
        {
            using var context = DataContext.Context;
            var existingMonHoc = context.Set<MonHoc>()
                .FirstOrDefault(m => m.MaMH == monHoc.MaMH);

            if (existingMonHoc == null)
            {
                return false;
            }

            existingMonHoc.TenMH = monHoc.TenMH;

            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool ThemMonHoc(MonHoc monHoc)
    {
        try
        {
            using var context = DataContext.Context;
            context.Set<MonHoc>().Add(monHoc);
            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool XoaMonHoc(string maMH)
    {
        try
        {
            using var context = DataContext.Context;
            var monHoc = context.Set<MonHoc>()
                .FirstOrDefault(m => m.MaMH == maMH);

            if (monHoc == null)
            {
                return false;
            }

            context.Set<MonHoc>().Remove(monHoc);
            return context.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
