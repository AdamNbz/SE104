using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL;

public static class HocSinhDAL
{
    public static int LaySoLuongHocSinh()
    {
        return DataContext.Context.HOCSINH.Count();
    }
    public static int TiepNhanHocSinh(HocSinh hs)
    {
        try
        {
            DataContext.Context.HOCSINH.Add(hs);
            DataContext.Context.SaveChanges();
            return 1;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return 0;
        }
    }

    public static int CapNhatHocSinh(HocSinh hs)
    {
        var hocSinh = DataContext.Context.HOCSINH.FirstOrDefault(HocSinh => HocSinh.MaHS == hs.MaHS);
        if (hocSinh == null)
            return 0;

        hocSinh.HoTen = hs.HoTen;
        hocSinh.GioiTinh = hs.GioiTinh;
        hocSinh.NgaySinh = hs.NgaySinh;
        hocSinh.DiaChi = hs.DiaChi;
        hocSinh.Email = hs.Email;
        hocSinh.MaLop = hs.MaLop;

        try
        {
            DataContext.Context.SaveChanges();
            return 1;
        }
        catch
        {
            return 0;
        }
    }

    public static int XoaHocSinh(string maHS)
    {
        var hocSinh = DataContext.Context.HOCSINH.FirstOrDefault(HocSinh => HocSinh.MaHS == maHS);
        if (hocSinh == null)
            return 0;
        try
        {
            DataContext.Context.HOCSINH.Remove(hocSinh);
            DataContext.Context.SaveChanges();
            return 1;
        }
        catch
        {
            return 0;
        }
    }
    public static List<HocSinh> LayTatCaHocSinh()
    {
        return DataContext.Context.HOCSINH.Include(hs => hs.Lop).ToList();
    }
    public static HocSinh? LayHocSinh(string MaHS)
    {
        return DataContext.Context.HOCSINH.Include(hs => hs.Lop).FirstOrDefault(hs => hs.MaHS == MaHS);
    }
}
