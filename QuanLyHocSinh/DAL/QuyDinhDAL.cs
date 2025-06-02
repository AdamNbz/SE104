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
        // Always use fresh context to avoid cache issues
        using (var context = new DataContext())
        {
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
                System.Diagnostics.Debug.WriteLine("Created default ThamSo values");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Loaded ThamSo from DB: {thamSo.TuoiToiThieu}-{thamSo.TuoiToiDa}, {thamSo.SiSoToiDa}, {thamSo.MocDiemDat}");
            }

            return thamSo;
        }
    }

    public static bool CapNhatQuyDinhTuoi(int tuoiToiThieu, int tuoiToiDa)
    {
        try
        {
            using (var context = new DataContext())
            {
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
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in CapNhatQuyDinhTuoi: {ex.Message}");
            return false;
        }
    }

    public static bool CapNhatSiSoToiDa(int siSoToiDa)
    {
        try
        {
            using (var context = new DataContext())
            {
                var thamSo = context.THAMSO.FirstOrDefault();

                if (thamSo == null)
                {
                    thamSo = new ThamSo();
                    context.THAMSO.Add(thamSo);
                }

                thamSo.SiSoToiDa = siSoToiDa;

                return context.SaveChanges() > 0;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in CapNhatSiSoToiDa: {ex.Message}");
            return false;
        }
    }

    public static bool CapNhatDiemChuanDatMon(int diemChuanDatMon)
    {
        try
        {
            using (var context = new DataContext())
            {
                var thamSo = context.THAMSO.FirstOrDefault();

                if (thamSo == null)
                {
                    thamSo = new ThamSo();
                    context.THAMSO.Add(thamSo);
                }

                thamSo.MocDiemDat = diemChuanDatMon;

                return context.SaveChanges() > 0;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in CapNhatDiemChuanDatMon: {ex.Message}");
            return false;
        }
    }

    // MAIN METHOD: Update all parameters at once to avoid context issues
    public static bool CapNhatTatCaQuyDinh(int tuoiToiThieu, int tuoiToiDa, int siSoToiDa, int diemChuanDatMon)
    {
        try
        {
            using (var context = new DataContext())
            {
                var thamSo = context.THAMSO.FirstOrDefault();

                if (thamSo == null)
                {
                    // Create new record with all values
                    thamSo = new ThamSo
                    {
                        TuoiToiThieu = tuoiToiThieu,
                        TuoiToiDa = tuoiToiDa,
                        SiSoToiDa = siSoToiDa,
                        MocDiemDat = diemChuanDatMon
                    };
                    context.THAMSO.Add(thamSo);
                    System.Diagnostics.Debug.WriteLine("Created new ThamSo record");
                }
                else
                {
                    // Update existing record with all values
                    thamSo.TuoiToiThieu = tuoiToiThieu;
                    thamSo.TuoiToiDa = tuoiToiDa;
                    thamSo.SiSoToiDa = siSoToiDa;
                    thamSo.MocDiemDat = diemChuanDatMon;
                    System.Diagnostics.Debug.WriteLine($"Updated existing ThamSo: {tuoiToiThieu}-{tuoiToiDa}, {siSoToiDa}, {diemChuanDatMon}");
                }

                int changes = context.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"SaveChanges returned: {changes}");

                if (changes > 0)
                {
                    // Refresh singleton context to clear cache
                    DataContext.RefreshContext();
                    System.Diagnostics.Debug.WriteLine("Context refreshed after successful update");
                }

                return changes > 0;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in CapNhatTatCaQuyDinh: {ex.Message}");
            if (ex.InnerException != null)
            {
                System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            return false;
        }
    }

    public static bool CapNhatMonHoc(MonHoc monHoc)
    {
        try
        {
            var context = DataContext.Context;
            var existingMonHoc = context.Set<MonHoc>().FirstOrDefault(m => m.MaMH == monHoc.MaMH);

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
            var context = DataContext.Context;
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
            var context = DataContext.Context;
            var monHoc = context.Set<MonHoc>().FirstOrDefault(m => m.MaMH == maMH);

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
