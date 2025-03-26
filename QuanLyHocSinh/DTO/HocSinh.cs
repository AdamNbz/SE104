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
    public string? HoTen { get; set; }
    public string? GioiTinh { get; set; }
    public DateTime? NgaySinh { get; set; }
    public string? DiaChi { get; set; }
    public string? Email { get; set; }
    public string? MaLop { get; set; }
    public Lop? Lop { get; set; }
}
