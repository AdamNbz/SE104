using BLL;
using DTO;
using DAL;

var danhSachMonHoc = BaoCaoTongKetMonBLL.LayDanhSachMonHoc();
var danhSachHocKy = BaoCaoTongKetMonBLL.LayDanhSachHocKy();

Console.WriteLine("Danh sách môn học:");
foreach (var monHoc in danhSachMonHoc)
{
    Console.WriteLine($"- {monHoc.TenMH} (Mã: {monHoc.MaMH})");
}
Console.WriteLine("\nDanh sách học kỳ:");
foreach (var hocKy in danhSachHocKy)
{
    Console.WriteLine($"- {hocKy.TenHK} (Mã: {hocKy.MaHK})");
}
