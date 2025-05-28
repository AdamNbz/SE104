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
    public List<BaoCaoMonChiTietLopResult> ChiTietTheoLop { get; set; } = new List<BaoCaoMonChiTietLopResult>();
    public ICollection<BangDiemMon>? ChiTietBangDiem { get; set; } = new List<BangDiemMon>();
}
