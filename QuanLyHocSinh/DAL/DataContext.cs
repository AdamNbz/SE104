using Microsoft.EntityFrameworkCore;
using DTO;

namespace DAL;

public class DataContext : DbContext
{
    public DbSet<HocSinh> HOCSINH { get; set; }
    public DbSet<ThamSo> THAMSO { get; set; }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=hocsinh.db");
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
    }

    private DataContext()
    {
        Database.EnsureCreated();
    }

    private static DataContext _instance = new();
    public static DataContext Context => _instance;

}
