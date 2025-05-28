using BLL;
using DTO;
using DAL;

var list = LopDAL.LayDanhSachLop();
foreach (var lop in list)
{
    Console.WriteLine($"Lop: {lop.MaLop}");
}

var list1 = KhoiDAL.LayDanhSachKhoi();
foreach (var khoi in list1)
{
    Console.WriteLine($"Khoi: {khoi.MaKhoi}");
}
var ThongTinTimKiem = new TimKiemBLL.ThongTinTimKiem("1", "Nguyen", "", "", "", "", "", "", "", null, null, "", "", "", "");
var result = TimKiemBLL.TimKiem(ThongTinTimKiem);
if (result.Count != 0)
{
    Console.WriteLine("Tim Thay Ket Qua");
}