using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class BaoCaoMonResult
{
    public int TongSo { get; set; }
    public int SoDat { get; set; }
    public int SoKhongDat { get; set; }
    public double TyLeDat { get; set; }

    public ICollection<BangDiemMon>? ChiTietBangDiem { get; } = null!;
}
