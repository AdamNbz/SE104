using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public static class LopBLL
{
    private static readonly List<Lop> DanhSachLop = LopDAL.LayDanhSachLop();
    private static readonly List<Khoi> DanhSachKhoiLop = KhoiDAL.LayDanhSachKhoi();
    public static List<Lop> GetDanhSachLop() => DanhSachLop;
    public static List<Khoi> GetDanhSachKhoiLop() => DanhSachKhoiLop;
    public static int TinhSiSo(List<HocSinh> HocSinhList, string MaLop)
    {
        //int siso = 0;
        //for (int i = 0; i < HocSinhList.Count; i++)
        //{
        //    // Sau Khi Co Lay Lop Theo Ma Lop Co The Thay Bang Viec Dung Count Cua HocSinhs
        //    if (HocSinhList[i].MaLop == MaLop)
        //    {
        //        siso++;
        //    }
        //}
        //return siso;
        return DataContext.Context.HOCSINH.Count(h => h.MaLop == MaLop);
    }
    public static List<HocSinh> LayDanhSachHocSinh(string MaLop)
    {
        //List<HocSinh> DanhSachHocSinhTrongLop = new List<HocSinh>();
        //List<HocSinh> DanhSachHocSinh = HocSinhBLL.GetDanhSachHocSinh();
        //for (int i = 0; i < DanhSachHocSinh.Count; i++)
        //{
        //    // Sau Khi Co Lay Lop Theo Ma Lop Co The Thay Bang Viec Dung Count Cua HocSinhs
        //    if (DanhSachHocSinh[i].MaLop == MaLop)
        //    {
        //        DanhSachHocSinhTrongLop.Add(DanhSachHocSinh[i]);
        //    }
        //}
        //return DanhSachHocSinhTrongLop;
        return DataContext.Context.HOCSINH.Where(h => h.MaLop == MaLop).ToList();
    }
}
