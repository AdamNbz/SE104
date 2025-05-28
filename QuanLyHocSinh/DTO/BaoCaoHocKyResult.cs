using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class BaoCaoHocKyResult
{
    public List<ChiTietBaoCaoHocKyLop> DanhSachThongKeLop { get; set; } = new List<ChiTietBaoCaoHocKyLop>();
}

public class ChiTietBaoCaoHocKyLop
{
    public string? TenLop { get; set; }
    public int SiSo { get; set; }
    public int SoLuongDat { get; set; }
    public double TyLeDat { get; set; }
}