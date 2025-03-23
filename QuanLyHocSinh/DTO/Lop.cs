using System;
using System.ComponentModel.DataAnnotations;

namespace DTO;

public class Lop
{
    [Key]
    [StringLength(4)]
    public required string MaLop { get; set; }
    public required string TenLop { get; set; }
    public required string MaKhoi { get; set; }
    public Khoi? Khoi { get; set; }

    public ICollection<HocSinh>? HocSinhs { get; } = null!;
}
