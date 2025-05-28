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
                    throw new Exception("Số lượng học sinh vượt giới hạn!");
                }
                HocSinhCanPhanLop = HocSinhDAL.LayHocSinh(MaHS);
                LopDuocPhanVao = DanhSachLop.Find(x => x.MaLop == MaLop);
                HocSinhCanPhanLop.MaLop = LopDuocPhanVao.MaLop;
                HocSinhDAL.CapNhatHocSinh(HocSinhCanPhanLop);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"Đã xảy ra lỗi khi lập danh sách lớp!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        public static void PhanLopChoMotDanhSachHocSinh(string MaLop,List<string> DanhSachMaHS)
        {
            try
            {
                if (DanhSachLop.Find(x => x.MaLop == MaLop) == null)
                {
                    throw new Exception("Lớp không nằm trong danh sách lớp!");
                }
                for (int i = 0; i < DanhSachMaHS.Count; i++)
                {
                    PhanLopChoTungHocSinh(DanhSachMaHS[i], MaLop);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Đã xảy ra lỗi khi lập danh sách lớp!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
