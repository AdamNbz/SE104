using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HocSinh
    {
        string maHS;
        string hoTen;
        DateTime ngaySinh;
        string gioiTinh;
        string email;
        string diaChi;

        public HocSinh(string maHS, string hoTen, DateTime ngaySinh, string gioiTinh, string email, string diaChi)
        {
            this.maHS = maHS;
            this.hoTen = hoTen;
            this.ngaySinh = ngaySinh;
            this.gioiTinh = gioiTinh;
            this.email = email;
            this.diaChi = diaChi;
        }

        public string MaHS 
        { 
            get => maHS; 
            set => maHS = value;
        }
        public string HoTen
        { 
            get => hoTen;
            set => hoTen = value; 
        }
        public DateTime NgaySinh 
        { 
            get => ngaySinh;
            set => ngaySinh = value; 
        }
        public string GioiTinh 
        { 
            get => gioiTinh;
            set => gioiTinh = value;
        }
        public string Email 
        { 
            get => email; 
            set => email = value; 
        }
        public string DiaChi 
        { 
            get => diaChi; 
            set => diaChi = value; 
        }
    }
}
