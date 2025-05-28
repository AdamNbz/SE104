using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class BaoCaoMonResult
{
    public int TongSo { get; set; }
    public int SoLuongDat { get; set; }
    public int SoLuongKhongDat { get; set; }
    public double TyLeDat { get; set; }

    public ICollection<BangDiemMon>? ChiTietBangDiem { get; set; } = null!;
}
