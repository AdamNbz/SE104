using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class DiemBLL
    {
        // Class để chứa điểm TB theo học kỳ
        public class DiemTrungBinhHocKy
        {
            public string MaHK { get; set; }
            public double? DiemTB { get; set; }
        }

        // Method để lấy điểm trung bình của học sinh theo từng học kỳ
        public static List<DiemTrungBinhHocKy> LayDiemTrungBinhHocSinh(string maHocSinh)
        {
            var result = new List<DiemTrungBinhHocKy>();

            try
            {
                // Lấy tất cả điểm của học sinh
                var bangDiemList = DAL.BangDiemMonDAL.LayDiemTheoHocSinh(maHocSinh);
                if (bangDiemList == null || bangDiemList.Count == 0)
                {
                    return result;
                }

                // Lấy danh sách học kỳ
                var danhSachHocKy = bangDiemList.Select(d => d.MaHK).Distinct().ToList();

                foreach (var maHK in danhSachHocKy)
                {
                    // Lấy điểm của học kỳ này
                    var diemHocKy = bangDiemList.Where(d => d.MaHK == maHK).ToList();
                    
                    double tongDiem = 0;
                    int soMon = 0;

                    foreach (var diem in diemHocKy)
                    {
                        // Tính điểm TB môn
                        var diemTBMon = DAL.BaoCaoTongKetMonDAL.TinhDiemTrungBinhMon(maHocSinh, diem.MaMH, maHK);
                        if (diemTBMon.HasValue)
                        {
                            tongDiem += diemTBMon.Value;
                            soMon++;
                        }
                    }

                    // Tính điểm TB học kỳ
                    double? diemTBHocKy = null;
                    if (soMon > 0)
                    {
                        diemTBHocKy = tongDiem / soMon;
                    }

                    result.Add(new DiemTrungBinhHocKy
                    {
                        MaHK = maHK,
                        DiemTB = diemTBHocKy
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi trong LayDiemTrungBinhHocSinh: {ex.Message}");
            }

            return result;
        }

        // Method để lấy điểm TB của một học sinh trong một học kỳ cụ thể
        public static double? LayDiemTrungBinhHocSinhTheoHocKy(string maHocSinh, string maHocKy)
        {
            try
            {
                var danhSachDiem = LayDiemTrungBinhHocSinh(maHocSinh);
                var diemHocKy = danhSachDiem.FirstOrDefault(d => d.MaHK == maHocKy);
                return diemHocKy?.DiemTB;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi trong LayDiemTrungBinhHocSinhTheoHocKy: {ex.Message}");
                return null;
            }
        }
    }
}
