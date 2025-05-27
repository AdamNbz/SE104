using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class HocKy
{
    [Key]
    public required string MaHK { get; set; }
    public required string TenHK { get; set; }
    public ICollection<BangDiemMon> BangDiemMons { get; set; } = new List<BangDiemMon>();
}
