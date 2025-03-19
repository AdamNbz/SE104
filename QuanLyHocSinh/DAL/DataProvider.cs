using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal static class DataProvider
    {
        private static SqlConnection GetConnection()
        {
            return new SqlConnection("Server=LAPTOP-6F5A88TT;Database=QuanLyHocSinh;Integrated Security=True");
        }
        public static DataTable ExecuteQuery(string StrCmd, SqlParameter[] Params = null)
        {
            var Conn = GetConnection();
            Conn.Open();
            try
            {
                SqlCommand Cmd = new SqlCommand(StrCmd, Conn);
                if (Params != null)
                    Cmd.Parameters.AddRange(Params);
                DataTable dt = new DataTable();
                SqlDataAdapter adt = new SqlDataAdapter(Cmd);
                adt.Fill(dt);
                return dt;
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }
        public static object ExecuteScalar(string StrCmd, SqlParameter[] Params = null)
        {
            var Conn = GetConnection();
            Conn.Open();
            try
            {
                SqlCommand Cmd = new SqlCommand(StrCmd, Conn);
                if (Params != null)
                    Cmd.Parameters.AddRange(Params);
                object o = Cmd.ExecuteScalar();
                if (o == DBNull.Value)
                    return null;
                return o;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }
        public static int ExecuteNonQuery(string StrCmd, SqlParameter[] Params = null)
        {
            var Conn = GetConnection();
            Conn.Open();
            try
            {
                SqlCommand Cmd = new SqlCommand(StrCmd, Conn);
                if (Params != null)
                    Cmd.Parameters.AddRange(Params);
                return Cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }
    }
}
