using DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public static class BaoCaoTongKetMonDAL
    {

        public static List<MonHoc> LayDanhSachMonHoc()
        {
            return DataContext.Context.Set<MonHoc>().ToList();
        }

        public static List<HocKy> LayDanhSachHocKy()
        {
            return DataContext.Context.Set<HocKy>().ToList();
        }

        public static BaoCaoMonResult LapBaoCaoTongKetMon(string maMH, string maHK)
        {
            using (var context = new DataContext())
            {
                // It's better to get LOP and HOCSINH info for the report grid
                var query = from bdm in context.BANGDIEMMON
                            join hs in context.HOCSINH on bdm.MaHocSinh equals hs.MaHS
                            join lop in context.LOP on hs.MaLop equals lop.MaLop
                            where bdm.MaMH == maMH && bdm.MaHK == maHK
                            select new // Project to an anonymous type or a dedicated DTO for the grid row
                            {
                                TenLop = lop.TenLop,
                                DiemCuoiKy = bdm.DiemCuoiKy,
                                MaHocSinh = hs.MaHS // Needed for counting students per class
                            };

                var allScoresForSubjectAndSemester = query.ToList();

                // Group by class to get per-class statistics
                var reportDataByClass = allScoresForSubjectAndSemester
                    .GroupBy(r => r.TenLop)
                    .Select(g => new BaoCaoMonChiTietLopResult // You'll need to create this DTO
                    {
                        TenLop = g.Key,
                        SiSo = g.Select(s => s.MaHocSinh).Distinct().Count(), // Count distinct students in the class for this subject/semester
                        SoLuongDat = g.Count(s => s.DiemCuoiKy.HasValue)
                        // TyLe will be calculated in BLL or GUI from SiSo and SoLuongDat
                    }).ToList();


                // Overall statistics (can also be derived from reportDataByClass in BLL/GUI)
                int tongSoHocSinh = allScoresForSubjectAndSemester.Select(s => s.MaHocSinh).Distinct().Count();
                int tongSoDat = reportDataByClass.Sum(r => r.SoLuongDat);

                return new BaoCaoMonResult // This DTO might need adjustment
                {
                    // These overall numbers might be less relevant if you're showing per-class breakdown
                    TongSo = tongSoHocSinh,
                    SoDat = tongSoDat,
                    // SoKhongDat and TyLeDat can be calculated from the above

                    // You need a new property in BaoCaoMonResult to hold List<BaoCaoMonChiTietLopResult>
                    ChiTietTheoLop = reportDataByClass
                };
            }
        }
    }
}