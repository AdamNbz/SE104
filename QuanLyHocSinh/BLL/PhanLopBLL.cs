using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    internal class PhanLopBLL
    {
        HocSinh HocSinhCanPhanLop;
        Lop LopDuocPhanVao;
        List<Lop> DanhSachLop;
        public void PhanLopChoTungHocSinh(string MaHS,string MaLop)
        {
            HocSinhCanPhanLop = HocSinhDAL.LayHocSinh(MaHS);
            LopDuocPhanVao = DanhSachLop.Find(x => x.MaLop == MaLop);
            HocSinhCanPhanLop.MaLop = LopDuocPhanVao.MaLop;
            HocSinhDAL.CapNhatHocSinh(HocSinhCanPhanLop);
        }
        public void PhanLopChoMotDanhSachHocSinh(string MaLop,List<string> DanhSachMaHS)
        {
            for (int i = 0;i<DanhSachMaHS.Count;i++)
            {
                PhanLopChoTungHocSinh(DanhSachMaHS[i], MaLop);
            }
        }
    }
}
