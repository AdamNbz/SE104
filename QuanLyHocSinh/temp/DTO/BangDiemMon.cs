using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class BangDiemMon
{
    public required string MaHocSinh { get; set; }
    public required string MaMH { get; set; }
    public required string MaHK { get; set; }
    public float? Diem15P { get; set; }
    public float? Diem1T { get; set; }
    public float? DiemCuoiKy { get; set; }
    public HocSinh? HocSinh { get; set; }
    public MonHoc? MonHoc { get; set; }
    public HocKy? HocKy { get; set; }

}
