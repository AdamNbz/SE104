using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class MonHoc
{
    [Key]
    public required string MaMH { get; set; }
    public required string TenMH { get; set; }
    public ICollection<BangDiemMon>? BangDiemMons { get; } = null!;
}
