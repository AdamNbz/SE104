using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DAL;
using DTO;

namespace BLL
{
    public static class PhanLopBLL
    {
        static HocSinh HocSinhCanPhanLop;
        static Lop LopDuocPhanVao;
        static List<Lop> DanhSachLop = LopBLL.GetDanhSachLop();
        
        public static void PhanLopChoTungHocSinh(string MaHS,string MaLop)
        {
            try
            {

                if (LopBLL.TinhSiSo(HocSinhBLL.GetDanhSachHocSinh(),MaLop) >= 40)
                {
                    throw new Exception("SoLuongHocSinhVuotGioiHan");
                }
                HocSinhCanPhanLop = HocSinhDAL.LayHocSinh(MaHS);
                LopDuocPhanVao = DanhSachLop.Find(x => x.MaLop == MaLop);
                HocSinhCanPhanLop.MaLop = LopDuocPhanVao.MaLop;
                HocSinhDAL.CapNhatHocSinh(HocSinhCanPhanLop);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"Loi Lap Danh Sach Lop",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        public static void PhanLopChoMotDanhSachHocSinh(string MaLop,List<string> DanhSachMaHS)
        {
            try
            {
                if (DanhSachLop.Find(x => x.MaLop == MaLop) == null)
                {
                    throw new Exception("LopKhongNamTrongDanhSachLop");
                }
                for (int i = 0; i < DanhSachMaHS.Count; i++)
                {
                    PhanLopChoTungHocSinh(DanhSachMaHS[i], MaLop);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Loi Lap Danh Sach Lop", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
