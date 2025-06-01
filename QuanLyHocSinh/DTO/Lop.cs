using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO;

public class Lop
{
    [Key]
    [StringLength(4)]
    public required string MaLop { get; set; }
    public required string TenLop { get; set; }
    public string? MaKhoi { get; set; }
    [ForeignKey("MaKhoi")]
    public Khoi? Khoi { get; set; }
    public ICollection<HocSinh> HocSinhs { get; set; } = new List<HocSinh>();
}
