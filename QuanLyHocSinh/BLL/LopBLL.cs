using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    internal static class LopBLL
    {
        static List<Lop> DanhSachLop;
        public static List<Lop> GetDanhSachLop()
        {
            return DanhSachLop;
        }
        public static int TinhSiSo(List<HocSinh> HocSinhList, string MaLop)
        {
            int siso = 0;
            for (int i = 0; i < HocSinhList.Count; i++)//Sau Khi Co Lay Lop Theo Ma Lop Co The Thay Bang Viec Dung Count Cua HocSinhs
            {

                if (HocSinhList[i].MaLop == MaLop)
                {
                    siso++;
                }
            }
            return siso;
        }
    }
}
