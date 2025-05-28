using System;
using System.ComponentModel.DataAnnotations;

namespace DTO;

public class Khoi
{
    [Key]
    [StringLength(4)]
    public required string MaKhoi { get; set; }
    public required string TenKhoi { get; set; }
    public ICollection<Lop> Lops { get; set; } = new List<Lop>();
}
