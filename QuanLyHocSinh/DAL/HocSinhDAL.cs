using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class HocSinhDAL
    {
        public static int LaySoLuongHocSinh()
        {
            string strCmd = "Select Count(*) From HocSinh";
            return (int)DataProvider.ExecuteScalar(strCmd);
        }

        public static int TiepNhanHocSinh(HocSinh hs)
        {
            string strCmd = "Insert into HocSinh(MaHS, HoTen, NgaySinh, GioiTinh, Email, DiaChi) Values (@mahs, @hoten, @ngsinh, @gioitinh, @email, @diachi)";
            SqlParameter[] Params = new SqlParameter[]
            {
                new SqlParameter("@mahs", hs.MaHS),
                new SqlParameter("@hoten", hs.HoTen),
                new SqlParameter("@ngsinh", hs.NgaySinh),
                new SqlParameter("@gioitinh", hs.GioiTinh),
                new SqlParameter("@email", hs.Email),
                new SqlParameter("@diachi", hs.DiaChi)
            };

            return DataProvider.ExecuteNonQuery(strCmd, Params);
        }
    }
}
