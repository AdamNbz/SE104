using DAL;
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
    public static KetQuaBaoCaoHocKy LapBaoCaoTongKetHocKy(string maHK, int mocDiemDat)
    {
        var context = DataContext.Context;

        var semesterScores = context.BANGDIEMMON.Include(b => b.HocSinh).ThenInclude(hs => hs.Lop).Where(b => b.MaHK == maHK && b.HocSinh != null && b.HocSinh.Lop != null).ToList();

        var reportByClass = semesterScores.GroupBy(b => b.HocSinh.Lop).Select(classGroup => {
                var lop = classGroup.Key;
                var studentsInClass = classGroup.Select(b => b.HocSinh).Distinct().ToList();
                int siSo = studentsInClass.Count;
                int soLuongDat = 0;

                foreach (var student in studentsInClass)
                {
                    var studentSubjects = classGroup.Where(b => b.MaHocSinh == student.MaHS).Select(b => b.MaMH).Distinct().ToList();

                    if (!studentSubjects.Any())
                    {
                        continue;
                    }

                    double totalSubjectAverage = 0;
                    double validSubjectCount = 0;

                    foreach (var subject in studentSubjects)
                    {
                        double? subjectAverage = BaoCaoTongKetMonDAL.TinhDiemTrungBinhMon(student.MaHS, subject, maHK);

                        if (subjectAverage.HasValue)
                        {
                            totalSubjectAverage += subjectAverage.Value;
                            validSubjectCount++;
                        }
                    }

                    double overallAverage = (validSubjectCount > 0) ?
                        totalSubjectAverage / validSubjectCount : 0;

                    if (overallAverage >= mocDiemDat)
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

        return new KetQuaBaoCaoHocKy
        {
            DanhSachThongKeLop = reportByClass
        };
    }
}
