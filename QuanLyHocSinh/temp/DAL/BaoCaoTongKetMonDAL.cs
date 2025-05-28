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
                var query = from bdm in context.BANGDIEMMON
                            join hs in context.HOCSINH on bdm.MaHocSinh equals hs.MaHS
                            join lop in context.LOP on hs.MaLop equals lop.MaLop
                            where bdm.MaMH == maMH && bdm.MaHK == maHK
                            select new 
                            {
                                TenLop = lop.TenLop,
                                DiemCuoiKy = bdm.DiemCuoiKy,
                                MaHocSinh = hs.MaHS
                            };

                var allScoresForSubjectAndSemester = query.ToList();

                var reportDataByClass = allScoresForSubjectAndSemester
                    .GroupBy(r => r.TenLop)
                    .Select(g => new BaoCaoMonChiTietLopResult
                    {
                        TenLop = g.Key,
                        SiSo = g.Select(s => s.MaHocSinh).Distinct().Count(),
                        SoLuongDat = g.Count(s => s.DiemCuoiKy.HasValue)
                    }).ToList();

                int tongSoHocSinh = allScoresForSubjectAndSemester.Select(s => s.MaHocSinh).Distinct().Count();
                int tongSoDat = reportDataByClass.Sum(r => r.SoLuongDat);

                return new BaoCaoMonResult
                {
                    TongSo = tongSoHocSinh,
                    SoDat = tongSoDat,
                    ChiTietTheoLop = reportDataByClass
                };
            }
        }
    }
}