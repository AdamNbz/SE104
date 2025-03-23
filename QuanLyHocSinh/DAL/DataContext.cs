using Microsoft.EntityFrameworkCore;
using DTO;
using System.Diagnostics;
using System.Windows;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Cryptography.X509Certificates;

namespace DAL;

public class DataContext : DbContext
{
    public DbSet<HocSinh> HOCSINH { get; set; }
    public DbSet<ThamSo> THAMSO { get; set; }
    public DbSet<Lop> LOP { get; set; }
    public DbSet<Khoi> KHOI { get; set; }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=.\\QLHS.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ThamSo>().HasData(
            new ThamSo
            {
                Id = 1,
                TuoiToiDa = 20,
                TuoiToiThieu = 15
            }
        );
        modelBuilder.Entity<Khoi>()
            .HasMany(k => k.Lops)
            .WithOne(l => l.Khoi)
            .HasForeignKey(l => l.MaKhoi);
        modelBuilder.Entity<Khoi>()
            .HasData(
                new Khoi
                {
                    MaKhoi = "K010",
                    TenKhoi = "Khối 10"
                },
                new Khoi
                {
                    MaKhoi = "K011",
                    TenKhoi = "Khối 11"
                },
                new Khoi
                {
                    MaKhoi = "K012",
                    TenKhoi = "Khối 12"
                }
            );
        // There are these classes: 10A1, 10A2, 10A3, 10A4, 11A1, 11A2, 11A3, 12A1, 12A2
        modelBuilder.Entity<Lop>()
            .HasData(
                new Lop
                {
                    MaLop = "10A1",
                    TenLop = "10A1",
                    MaKhoi = "K010",
                },
                new Lop
                {
                    MaLop = "10A2",
                    TenLop = "10A2",
                    MaKhoi = "K010",
                },
                new Lop
                {
                    MaLop = "10A3",
                    TenLop = "10A3",
                    MaKhoi = "K010",
                },
                new Lop
                {
                    MaLop = "10A4",
                    TenLop = "10A4",
                    MaKhoi = "K010",
                },
                new Lop
                {
                    MaLop = "11A1",
                    TenLop = "11A1",
                    MaKhoi = "K011",
                },
                new Lop
                {
                    MaLop = "11A2",
                    TenLop = "11A2",
                    MaKhoi = "K011",
                },
                new Lop
                {
                    MaLop = "11A3",
                    TenLop = "11A3",
                    MaKhoi = "K011",
                },
                new Lop
                {
                    MaLop = "12A1",
                    TenLop = "12A1",
                    MaKhoi = "K012",
                },
                new Lop
                {
                    MaLop = "12A2",
                    TenLop = "12A2",
                    MaKhoi = "K012",
                }
            );
    }

    public DataContext()
    {
        Database.EnsureCreated();
    }
    private static DataContext? _instance;
    public static DataContext Context
    {
        get
        {
            _instance ??= new DataContext();
            return _instance;
        }
    }
}
