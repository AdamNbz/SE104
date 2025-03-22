using Microsoft.EntityFrameworkCore;
using DTO;

namespace DAL;

public class DataContext : DbContext
{
    public DbSet<HocSinh> HocSinhs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=../hocsinh.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<HocSinh>().HasData(
            new HocSinh
            {
                MaHS = "HS001",
                HoTen = "Nguyen Van A",
                GioiTinh = "Nam",
                NgaySinh = new DateTime(2000, 1, 1),
                DiaChi = "Ha Noi",
                Email = "hello"
            },
            new HocSinh
            {
                MaHS = "HS002",
                HoTen = "Nguyen Van B",
                GioiTinh = "Nam",
                NgaySinh = new DateTime(2000, 1, 1),
                DiaChi = "Ha Noi",
                Email = "hello"
            },
            new HocSinh
            {
                MaHS = "HS003",
                HoTen = "Nguyen Van C",
                GioiTinh = "Nam",
                NgaySinh = new DateTime(2000, 1, 1),
                DiaChi = "Ha Noi",
                Email = "hello"
            }
        );
    }
}
