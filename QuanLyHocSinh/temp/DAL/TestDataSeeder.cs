using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL;

internal static class TestDataSeeder
{
    internal static void SeedStudentData(ModelBuilder modelBuilder)
    {
#if DEBUG
        modelBuilder.Entity<HocSinh>()
            .HasData(
                new HocSinh
                {
                    MaHS = "HS0001",
                    HoTen = "Dương Quốc Thuận",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2009, 1, 1),
                    DiaChi = "Ở đây",
                    Email = "thuandq@uit.edu.vn"
                },
                new HocSinh
                {
                    MaHS = "HS0002",
                    HoTen = "Tiền Minh Dương",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2009, 1, 1),
                    DiaChi = "Ở đây",
                    Email = "1234@lmao.com"
                }
            );
#endif
    }
}
