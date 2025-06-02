using Microsoft.EntityFrameworkCore;
using DTO;
using System.Diagnostics;

namespace DAL;

public class DataContext : DbContext
{
    public DbSet<BangDiemMon> BANGDIEMMON { get; set; }
    public DbSet<HocSinh> HOCSINH { get; set; }
    public DbSet<ThamSo> THAMSO { get; set; }
    public DbSet<Lop> LOP { get; set; }
    public DbSet<Khoi> KHOI { get; set; }
    public DbSet<MonHoc> MONHOC { get; set; }
    public DbSet<HocKy> HOCKY { get; set; }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=QLHS.db")
            .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BangDiemMon>()
            .HasKey(b => new { b.MaHocSinh, b.MaMH, b.MaHK });

        modelBuilder.Entity<BangDiemMon>()
            .HasOne(b => b.HocSinh)
            .WithMany()
            .HasForeignKey(b => b.MaHocSinh);

        modelBuilder.Entity<BangDiemMon>()
            .HasOne(b => b.MonHoc)
            .WithMany(m => m.BangDiemMons)
            .HasForeignKey(b => b.MaMH);

        modelBuilder.Entity<BangDiemMon>()
            .HasOne(b => b.HocKy)
            .WithMany(h => h.BangDiemMons)
            .HasForeignKey(b => b.MaHK);

        modelBuilder.Entity<Khoi>()
            .HasMany(k => k.Lops)
            .WithOne(l => l.Khoi)
            .HasForeignKey(l => l.MaKhoi);

        modelBuilder.Entity<Lop>()
            .HasMany(l => l.HocSinhs)
            .WithOne(hs => hs.Lop)
            .HasForeignKey(hs => hs.MaLop);
        modelBuilder.Entity<HocKy>().ToTable("HOCKY");
        modelBuilder.Entity<MonHoc>().ToTable("MONHOC");

        SeedThamSoData(modelBuilder);
        SeedKhoiData(modelBuilder);
        SeedLopData(modelBuilder);
        SeedHocKyData(modelBuilder);
        SeedMonHocData(modelBuilder);
    }

    public DataContext()
    {
        Migrations();
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

    // Method to refresh the singleton context (clear cache)
    public static void RefreshContext()
    {
        if (_instance != null)
        {
            _instance.Dispose();
            _instance = null;
        }
        System.Diagnostics.Debug.WriteLine("DataContext refreshed - cache cleared");
    }

    private void Migrations()
    {
        var migrations = Database.GetAppliedMigrations();
        Trace.WriteLine("Applied migrations:");
        foreach (var migration in migrations)
        {
            Trace.WriteLine(migration);
        }

        migrations = Database.GetMigrations();
        Trace.WriteLine("All migrations:");
        foreach (var migration in migrations)
        {
            Trace.WriteLine(migration);
        }

        if (Database.GetPendingMigrations().Any())
        {
            Database.Migrate();
        }
        Trace.WriteLine("Database migrated!");
    }

    private void SeedThamSoData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ThamSo>()
            .HasData(
                new ThamSo
                {
                    Id = 1,
                    TuoiToiDa = 20,
                    TuoiToiThieu = 15,
                    SiSoToiDa = 40,
                    MocDiemDat = 0 // Giữ nguyên 0 để test
                }
            );
    }

    private void SeedKhoiData(ModelBuilder modelBuilder)
    {
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
    }

    private void SeedLopData(ModelBuilder modelBuilder)
    {
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

    private void SeedHocKyData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HocKy>()
            .HasData(
                new HocKy
                {
                    MaHK = "HK1",
                    TenHK = "Học kỳ 1"
                },
                new HocKy
                {
                    MaHK = "HK2",
                    TenHK = "Học kỳ 2"
                }
            );
    }

    private void SeedMonHocData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MonHoc>()
            .HasData(
                new MonHoc
                {
                    MaMH = "TOAN",
                    TenMH = "Toán học"
                },
                new MonHoc
                {
                    MaMH = "VAN",
                    TenMH = "Ngữ văn"
                },
                new MonHoc
                {
                    MaMH = "LY",
                    TenMH = "Vật lý"
                },
                new MonHoc
                {
                    MaMH = "HOA",
                    TenMH = "Hóa học"
                },
                new MonHoc
                {
                    MaMH = "ANH",
                    TenMH = "Tiếng Anh"
                }
            );
    }
}
