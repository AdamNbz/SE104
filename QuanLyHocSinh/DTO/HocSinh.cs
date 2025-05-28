using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class HocSinh
{
    [Key]
    public required string MaHS { get; set; }
    public required string HoTen { get; set; }
    public required string GioiTinh { get; set; }
    public DateTime NgaySinh { get; set; }
    public required string DiaChi { get; set; }
    public required string Email { get; set; }
    public string? MaLop { get; set; }
    // public float? DiemTrungBinhHKI { get; set; }
    // public float? DiemTrungBinhHKII { get; set; }
    public Lop? Lop { get; set; }
}
