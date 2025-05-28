using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL;

public static class TimKiemBLL
{
    public class ThongTinTimKiem
    {
        public string? MaHS;
        public string? HoTen;
        public string? GioiTinh;
        public string? DiaChi;
        public string? Email;
        public float? MaxDiemTrungBinhHK1;
        public float? MaxDiemTrungBinhHK2;
        public float? MinDiemTrungBinhHK1;
        public float? MinDiemTrungBinhHK2;
        public DateTime? MaxNgaySinh;
        public DateTime? MinNgaySinh;
        public int? MinSiSo;
        public int? MaxSiSo;
        public string? MaKhoi;
        public string? MaLop;
        public ThongTinTimKiem(string? maHS, string? hoTen, string? gioiTinh, string? diaChi, string? email, string? maxDiemTrungBinhHK1, string? maxDiemTrungBinhHK2, string? minDiemTrungBinhHK1, string? minDiemTrungBinhHK2, DateTime? maxNgaySinh, DateTime? minNgaySinh, string? minSiSo, string? maxSiSo, string? maKhoi, string? maLop)
        {
            MaHS = maHS;
            HoTen = hoTen;
            GioiTinh = gioiTinh;
            DiaChi = diaChi;
            Email = email;
            MaxNgaySinh = maxNgaySinh;
            MinNgaySinh = minNgaySinh;
            
            if (float.TryParse(maxDiemTrungBinhHK1, out var resultMax1))
            {
                MaxDiemTrungBinhHK1 = resultMax1;
            }
            else
            {
                MaxDiemTrungBinhHK1 = null;
            }

            if (float.TryParse(maxDiemTrungBinhHK2, out var resultMax2))
            {
                MaxDiemTrungBinhHK2 = resultMax2;
            }
            else
            {
                MaxDiemTrungBinhHK2 = null;
            }

            if (float.TryParse(minDiemTrungBinhHK1, out var resultMin1))
            {
                MinDiemTrungBinhHK1 = resultMin1;
            }
            else
            {
                MinDiemTrungBinhHK1 = null;
            }

            if (float.TryParse(minDiemTrungBinhHK2, out var resultMin2))
            {
                MinDiemTrungBinhHK2 = resultMin2;
            }
            else
            {
                MinDiemTrungBinhHK2 = null;
            }
           
            if(int.TryParse(minSiSo,out var resultmin))
            {
                MinSiSo = resultmin;
            }
            else
            {
                MinSiSo = null;
            }
            
            if (int.TryParse(minSiSo, out var resultmax))
            {
                MaxSiSo = resultmax;
            }
            else
            {
                MaxSiSo = null;
            }
            
            MaKhoi = maKhoi;
            MaLop = maLop;
        }
    }
    public static List<HocSinh> TimKiem(ThongTinTimKiem thongTin)
    {
        List<HocSinh> CacKetQuaKhaThi=HocSinhBLL.GetDanhSachHocSinh();
        if (thongTin.MaHS!=null&&thongTin.MaHS!="")
        {
            TimKiemTheoMaHS(ref CacKetQuaKhaThi, thongTin.MaHS);
        }   
        if (thongTin.HoTen!=null&&thongTin.HoTen!="")
        {
            TimKiemTheoHoTen(ref CacKetQuaKhaThi, thongTin.HoTen);
        }
        if (thongTin.GioiTinh != null && thongTin.GioiTinh != "")
        {
            TimKiemTheoGioiTinh(ref CacKetQuaKhaThi, thongTin.GioiTinh);
        }
        if (thongTin.DiaChi != null && thongTin.DiaChi != "")
        {
            TimKiemTheoDiaChi(ref CacKetQuaKhaThi, thongTin.DiaChi);
        }
        if (thongTin.Email != null && thongTin.Email != "")
        {
            TimKiemTheoEmail(ref CacKetQuaKhaThi, thongTin.Email);
        }
        if (thongTin.MaLop != null && thongTin.MaLop != "")
        {
            TimKiemTheoMaLop(ref CacKetQuaKhaThi, thongTin.MaLop);
        }
        if (thongTin.MaKhoi != null && thongTin.MaKhoi != "")
        {
            TimKiemTheoMaKhoi(ref CacKetQuaKhaThi, thongTin.MaKhoi);
        }
        //if (thongTin.MaxDiemTrungBinhHK1 != null)
        //{
        //    TimKiemTheoMaxDiemTrungBinhHocKiI(ref CacKetQuaKhaThi, (float)thongTin.MaxDiemTrungBinhHK1);
        //}
        //if(thongTin.MinDiemTrungBinhHK1!=null)
        //{
        //    TimKiemTheoMinDiemTrungBinhHocKiI(ref CacKetQuaKhaThi, (float)thongTin.MinDiemTrungBinhHK1);
        //}
        //if (thongTin.MaxDiemTrungBinhHK2 != null)
        //{
        //    TimKiemTheoMaxDiemTrungBinhHocKiII(ref CacKetQuaKhaThi, (float)thongTin.MaxDiemTrungBinhHK2);
        //}
        //if (thongTin.MinDiemTrungBinhHK2 != null)
        //{
        //    TimKiemTheoMinDiemTrungBinhHocKiII(ref CacKetQuaKhaThi, (float)thongTin.MinDiemTrungBinhHK2);
        //}
        if (thongTin.MaxSiSo != null)
        {
            TimKiemTheoMaxSiSo(ref CacKetQuaKhaThi, (int)thongTin.MaxSiSo);
        }
        if (thongTin.MinSiSo != null)
        {
            TimKiemTheoMinSiSo(ref CacKetQuaKhaThi, (int)thongTin.MinSiSo);
        }
        if(thongTin.MinNgaySinh!=null)
        {
            TimKiemTheoMinNgaySinh(ref CacKetQuaKhaThi, (DateTime)thongTin.MinNgaySinh);
        }
        if (thongTin.MaxNgaySinh != null)
        {
            TimKiemTheoMaxNgaySinh(ref CacKetQuaKhaThi, (DateTime)thongTin.MaxNgaySinh);
        }
        return CacKetQuaKhaThi;
    }
    private static void TimKiemTheoMaHS(ref List<HocSinh> hocSinhs,string maHS)
    {
        List<HocSinh> KetQua=new List<HocSinh>();
        for (int i = 0; i <hocSinhs.Count;i++)
        {
            if (hocSinhs[i].MaHS.Contains(maHS))
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoHoTen(ref List<HocSinh> hocSinhs, string HoTen)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        for (int i = 0; i < hocSinhs.Count; i++)
        {
            if (hocSinhs[i].HoTen.Contains(HoTen))
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoGioiTinh(ref List<HocSinh> hocSinhs, string GioiTinh)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        for (int i = 0; i < hocSinhs.Count; i++)
        {
            if (hocSinhs[i].GioiTinh.Contains(GioiTinh))
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoDiaChi(ref List<HocSinh> hocSinhs, string DiaChi)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        for (int i = 0; i < hocSinhs.Count; i++)
        {
            if (hocSinhs[i].DiaChi.Contains(DiaChi))
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoEmail(ref List<HocSinh> hocSinhs, string Email)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        for (int i = 0; i < hocSinhs.Count; i++)
        {
            if (hocSinhs[i].Email.Contains(Email))
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoMaLop(ref List<HocSinh> hocSinhs, string MaLop)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        for (int i = 0; i < hocSinhs.Count; i++)
        {
            if (hocSinhs[i].MaLop==MaLop)
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoMaKhoi(ref List<HocSinh> hocSinhs, string MaKhoi)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        for (int i = 0; i < hocSinhs.Count; i++)
        {
            string MaLop = hocSinhs[i].MaLop;
            if (LopBLL.GetDanhSachLop().Find(x=>x.MaLop==MaLop).MaKhoi == MaKhoi)
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    //private static void TimKiemTheoMaxDiemTrungBinhHocKiI(ref List<HocSinh> hocSinhs, float MaxDiemTrungBinhHKI)
    //{
    //    hocSinhs = hocSinhs.Where(x => x.DiemTrungBinhHKI <= MaxDiemTrungBinhHKI).ToList();
    //}
    //private static void TimKiemTheoMaxDiemTrungBinhHocKiII(ref List<HocSinh> hocSinhs, float MaxDiemTrungBinhHKII)
    //{
    //    hocSinhs = hocSinhs.Where(x => x.DiemTrungBinhHKII <= MaxDiemTrungBinhHKII).ToList();
    //}
    //private static void TimKiemTheoMinDiemTrungBinhHocKiI(ref List<HocSinh> hocSinhs, float MinDiemTrungBinhHKI)
    //{
    //    hocSinhs = hocSinhs.Where(x => x.DiemTrungBinhHKI >= MinDiemTrungBinhHKI).ToList();
    //}
    //private static void TimKiemTheoMinDiemTrungBinhHocKiII(ref List<HocSinh> hocSinhs, float MinDiemTrungBinhHKII)
    //{
    //    hocSinhs = hocSinhs.Where(x => x.DiemTrungBinhHKII >= MinDiemTrungBinhHKII).ToList();
    //}
    private static void TimKiemTheoMinSiSo(ref List<HocSinh> hocSinhs, int MinSiSo)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        List<HocSinh> HocSinhList = HocSinhBLL.GetDanhSachHocSinh();
        
        for (int i = 0; i < hocSinhs.Count; i++)
        {
            int siso=LopBLL.TinhSiSo(HocSinhList, hocSinhs[i].MaLop);
            if (siso >= MinSiSo)
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoMaxSiSo(ref List<HocSinh> hocSinhs, int MaxSiSo)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        List<HocSinh> HocSinhList = HocSinhBLL.GetDanhSachHocSinh();

        for (int i = 0; i < hocSinhs.Count; i++)
        {
            int siso = LopBLL.TinhSiSo(HocSinhList, hocSinhs[i].MaLop);
            if (siso <= MaxSiSo)
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoMinNgaySinh(ref List<HocSinh> hocSinhs, DateTime MinNgaySinh)
    {
        List<HocSinh> KetQua = new List<HocSinh>();
        

        for (int i = 0; i < hocSinhs.Count; i++)
        {
            
            if (hocSinhs[i].NgaySinh >= MinNgaySinh)
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
    private static void TimKiemTheoMaxNgaySinh(ref List<HocSinh> hocSinhs, DateTime MaxNgaySinh)
    {
        List<HocSinh> KetQua = new List<HocSinh>();

        for (int i = 0; i < hocSinhs.Count; i++)
        {
           
            if (hocSinhs[i].NgaySinh <= MaxNgaySinh)
            {
                KetQua.Add(hocSinhs[i]);
            }
        }
        hocSinhs = KetQua;
    }
}
 
