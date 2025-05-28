using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

public class BaoCaoTongKetHocKyDAL
{
    public static BaoCaoHocKyResult LapBaoCaoTongKetHocKy(string maHK, int mocDiemDat)
    {
        var context = DataContext.Context;

        var semesterScores = context.BANGDIEMMON.Include(b => b.HocSinh).ThenInclude(hs => hs.Lop).Where(b => b.MaHK == maHK && b.HocSinh != null && b.HocSinh.Lop != null).ToList();

        var reportByClass = semesterScores.GroupBy(b => b.HocSinh.Lop).Select(classGroup => {
                var lop = classGroup.Key;
                var studentsInClassWithScores = classGroup.Select(b => b.HocSinh).Distinct().ToList();
                int siSo = studentsInClassWithScores.Count;
                int soLuongDat = 0;

                foreach (var student in studentsInClassWithScores)
                {
                    var studentScoresInSemester = classGroup
                        .Where(b => b.MaHocSinh == student.MaHS)
                        .ToList();
                    if (!studentScoresInSemester.Any())
                    {
                        continue;
                    }
                    bool studentPassedSemester = studentScoresInSemester
                        .All(b => b.DiemCuoiKy.HasValue && b.DiemCuoiKy >= mocDiemDat);

                    if (studentPassedSemester)
                    {
                        soLuongDat++;
                    }
                }

                double tyLe = (siSo > 0) ? (double)soLuongDat * 100.0 / siSo : 0;

                return new ChiTietBaoCaoHocKyLop
                {
                    TenLop = lop.TenLop,
                    SiSo = siSo,
                    SoLuongDat = soLuongDat,
                    TyLeDat = Math.Round(tyLe, 2)
                };
            }).OrderBy(r => r.TenLop).ToList();

        return new BaoCaoHocKyResult
        {
            DanhSachThongKeLop = reportByClass
        };
    }
}
