using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTO;
using BLL;

namespace GUI.Sprint3
{
    /// <summary>
    /// Interaction logic for Sprint3Control.xaml
    /// </summary>
    public partial class Sprint3Control : UserControl
    {
        public Sprint3Control()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadKhoiLop();
            LoadTenLop();
            // Không hiển thị dòng trống mặc định - chỉ hiển thị khi có kết quả tìm kiếm
        }

        private void LoadKhoiLop()
        {
            try
            {
                cbx_TenKhoiLop.Items.Clear();
                cbx_TenKhoiLop.Items.Add("Tất Cả");

                var danhSachKhoi = LopBLL.GetDanhSachKhoiLop();
                foreach (var khoi in danhSachKhoi)
                {
                    cbx_TenKhoiLop.Items.Add(khoi.TenKhoi);
                }
                cbx_TenKhoiLop.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khối lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTenLop()
        {
            try
            {
                cbx_TenLop.Items.Clear();
                cbx_TenLop.Items.Add("Tất Cả");

                var danhSachLop = LopBLL.GetDanhSachLop();
                foreach (var lop in danhSachLop)
                {
                    cbx_TenLop.Items.Add(lop.TenLop);
                }
                cbx_TenLop.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbx_TenKhoiLop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Lọc danh sách lớp theo khối được chọn
                LoadTenLopTheoKhoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi khối lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTenLopTheoKhoi()
        {
            try
            {
                cbx_TenLop.Items.Clear();
                cbx_TenLop.Items.Add("Tất Cả");

                var selectedKhoi = cbx_TenKhoiLop.SelectedItem?.ToString();
                var danhSachLop = LopBLL.GetDanhSachLop();

                if (selectedKhoi == "Tất Cả" || string.IsNullOrEmpty(selectedKhoi))
                {
                    // Hiển thị tất cả lớp
                    foreach (var lop in danhSachLop)
                    {
                        cbx_TenLop.Items.Add(lop.TenLop);
                    }
                }
                else
                {
                    // Lọc lớp theo khối được chọn
                    var danhSachKhoi = LopBLL.GetDanhSachKhoiLop();
                    var khoiDuocChon = danhSachKhoi.FirstOrDefault(k => k.TenKhoi == selectedKhoi);

                    if (khoiDuocChon != null)
                    {
                        var lopTheoKhoi = danhSachLop.Where(l => l.MaKhoi == khoiDuocChon.MaKhoi);
                        foreach (var lop in lopTheoKhoi)
                        {
                            cbx_TenLop.Items.Add(lop.TenLop);
                        }
                    }
                }

                cbx_TenLop.SelectedIndex = 0; // Mặc định chọn "Tất Cả"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách lớp theo khối: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_TimKiem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Collect search criteria from UI
                var thongTinTimKiem = new TimKiemBLL.ThongTinTimKiem(
                    string.IsNullOrWhiteSpace(txb_MaHocSinh.Text) ? null : txb_MaHocSinh.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_HoTen.Text) ? null : txb_HoTen.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_GioiTinh.Text) ? null : txb_GioiTinh.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_DiaChi.Text) ? null : txb_DiaChi.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_Email.Text) ? null : txb_Email.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_DiemTBHK1_Den.Text) ? null : txb_DiemTBHK1_Den.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_DiemTBHK2_Den.Text) ? null : txb_DiemTBHK2_Den.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_DiemTBHK1_Tu.Text) ? null : txb_DiemTBHK1_Tu.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_DiemTBHK2_Tu.Text) ? null : txb_DiemTBHK2_Tu.Text.Trim(),
                    dp_NgaySinh_Den.SelectedDate,
                    dp_NgaySinh_Tu.SelectedDate,
                    string.IsNullOrWhiteSpace(txb_SiSo_Tu.Text) ? null : txb_SiSo_Tu.Text.Trim(),
                    string.IsNullOrWhiteSpace(txb_SiSo_Den.Text) ? null : txb_SiSo_Den.Text.Trim(),
                    GetSelectedMaKhoi(),
                    GetSelectedMaLop()
                );

                // Perform search using BLL
                var ketQuaTimKiem = TimKiemBLL.TimKiem(thongTinTimKiem);

                // Clear existing results
                sp_KetQuaTimKiem.Children.Clear();

                // Display search results
                DisplaySearchResults(ketQuaTimKiem);

                // Show result count
                MessageBox.Show($"Tìm thấy {ketQuaTimKiem.Count} học sinh phù hợp", "Kết quả tìm kiếm",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear all input fields
                txb_MaHocSinh.Text = "";
                txb_HoTen.Text = "";
                txb_GioiTinh.Text = "";
                txb_DiaChi.Text = "";
                txb_Email.Text = "";
                txb_DiemTBHK1_Tu.Text = "";
                txb_DiemTBHK1_Den.Text = "";
                txb_DiemTBHK2_Tu.Text = "";
                txb_DiemTBHK2_Den.Text = "";
                dp_NgaySinh_Tu.SelectedDate = null;
                dp_NgaySinh_Den.SelectedDate = null;
                txb_SiSo_Tu.Text = "";
                txb_SiSo_Den.Text = "";

                cbx_TenKhoiLop.SelectedIndex = 0;
                cbx_TenLop.SelectedIndex = 0;

                // Clear results only - don't show empty rows
                sp_KetQuaTimKiem.Children.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi reset form: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddSampleResultRows()
        {
            // Add sample data for demonstration
            var sampleData = new[]
            {
                new { STT = "1", MaHS = "HS001", HoTen = "Nguyễn Văn A", GioiTinh = "Nam", DiaChi = "Hà Nội", Email = "nva@email.com", Lop = "10A1", DiemHK1 = "8.5", DiemHK2 = "8.7" },
                new { STT = "2", MaHS = "HS002", HoTen = "Trần Thị B", GioiTinh = "Nữ", DiaChi = "TP.HCM", Email = "ttb@email.com", Lop = "10A2", DiemHK1 = "9.0", DiemHK2 = "8.8" }
            };

            foreach (var item in sampleData)
            {
                AddResultRow(item.STT, item.MaHS, item.HoTen, item.GioiTinh, item.DiaChi, item.Email, item.Lop, item.DiemHK1, item.DiemHK2);
            }
        }

        private void AddEmptyResultRows()
        {
            // Add 2 empty rows as shown in the image
            for (int i = 1; i <= 2; i++)
            {
                AddResultRow(i.ToString(), "", "", "", "", "", "", "", "");
            }
        }

        private void AddResultRow(string stt, string maHS, string hoTen, string gioiTinh, string diaChi, string email, string lop, string diemHK1, string diemHK2)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                BorderThickness = new Thickness(0, 0, 0, 1) // Chỉ có border dưới
            };

            Grid grid = new Grid
            {
                Margin = new Thickness(8),
                Background = Brushes.White, // Nền trắng thay vì xám
                MinHeight = 40
            };

            // Define columns - phải khớp với header trong XAML
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });   // STT
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });  // Mã Học Sinh
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Họ Tên
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });   // Giới Tính
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });  // Địa Chỉ
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });  // Email
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });   // Lớp
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(130) });  // Điểm TB HK1
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(130) });  // Điểm TB HK2

            // Add text blocks - thứ tự phải khớp với header
            // Header: STT | Mã Học Sinh | Họ Tên | Giới Tính | Địa Chỉ | Email | Lớp | Điểm TB HK1 | Điểm TB HK2
            string[] values = { stt, maHS, hoTen, gioiTinh, diaChi, email, lop, diemHK1, diemHK2 };


            for (int i = 0; i < values.Length; i++)
            {
                if (i == 2) // Họ Tên column
                {
                    TextBox txtHoTen = new TextBox
                    {
                        Text = values[i],
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(4),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Background = Brushes.White,
                        BorderThickness = new Thickness(0),
                        BorderBrush = Brushes.Transparent,
                        IsReadOnly = true,
                        Focusable = false,
                        FontSize = 12,
                        Foreground = string.IsNullOrEmpty(values[i]) ? Brushes.LightGray : Brushes.Black
                    };
                    Grid.SetColumn(txtHoTen, i);
                    grid.Children.Add(txtHoTen);
                }
                // Use TextBox for Email and Địa Chỉ columns as well for better text display
                else if (i == 4 || i == 5) // Địa Chỉ and Email columns
                {
                    TextBox txtContent = new TextBox
                    {
                        Text = values[i],
                        TextAlignment = TextAlignment.Center,
                        Margin = new Thickness(4),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Background = Brushes.White,
                        BorderThickness = new Thickness(0),
                        BorderBrush = Brushes.Transparent,
                        IsReadOnly = true,
                        Focusable = false,
                        FontSize = 12,
                        Foreground = string.IsNullOrEmpty(values[i]) ? Brushes.LightGray : Brushes.Black
                    };
                    Grid.SetColumn(txtContent, i);
                    grid.Children.Add(txtContent);
                }
                else // Use TextBlock for other columns
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Text = values[i],
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 12,
                        Foreground = string.IsNullOrEmpty(values[i]) ? Brushes.LightGray : Brushes.Black,
                        Padding = new Thickness(4)
                    };
                    Grid.SetColumn(textBlock, i);
                    grid.Children.Add(textBlock);
                }
            }

            border.Child = grid;
            sp_KetQuaTimKiem.Children.Add(border);
        }

        private string GetSelectedMaKhoi()
        {
            try
            {
                if (cbx_TenKhoiLop.SelectedItem == null || cbx_TenKhoiLop.SelectedItem.ToString() == "Tất Cả")
                    return null;

                var tenKhoi = cbx_TenKhoiLop.SelectedItem.ToString();
                var danhSachKhoi = LopBLL.GetDanhSachKhoiLop();
                var khoi = danhSachKhoi.FirstOrDefault(k => k.TenKhoi == tenKhoi);
                return khoi?.MaKhoi;
            }
            catch
            {
                return null;
            }
        }

        private string GetSelectedMaLop()
        {
            try
            {
                if (cbx_TenLop.SelectedItem == null || cbx_TenLop.SelectedItem.ToString() == "Tất Cả")
                    return null;

                var tenLop = cbx_TenLop.SelectedItem.ToString();
                var danhSachLop = LopBLL.GetDanhSachLop();
                var lop = danhSachLop.FirstOrDefault(l => l.TenLop == tenLop);
                return lop?.MaLop;
            }
            catch
            {
                return null;
            }
        }

        private void DisplaySearchResults(List<HocSinh> ketQuaTimKiem)
        {
            try
            {
                if (ketQuaTimKiem == null || ketQuaTimKiem.Count == 0)
                {
                    // Không hiển thị gì khi không có kết quả - để trống
                    return;
                }

                // Display actual results
                for (int i = 0; i < ketQuaTimKiem.Count; i++)
                {
                    var hocSinh = ketQuaTimKiem[i];

                    // Get class name
                    var tenLop = "";
                    try
                    {
                        var lop = LopBLL.GetDanhSachLop().FirstOrDefault(l => l.MaLop == hocSinh.MaLop);
                        tenLop = lop?.TenLop ?? "";
                    }
                    catch { }



                    // Lấy điểm TB học kỳ
                    string diemTBHK1 = "";
                    string diemTBHK2 = "";
                    try
                    {
                        // Lấy điểm TB từ DiemBLL
                        var diemTB = BLL.DiemBLL.LayDiemTrungBinhHocSinh(hocSinh.MaHS);
                        if (diemTB != null && diemTB.Count > 0)
                        {
                            // Tìm điểm HK1 và HK2
                            var diemHK1 = diemTB.FirstOrDefault(d => d.MaHK == "HK1");
                            var diemHK2 = diemTB.FirstOrDefault(d => d.MaHK == "HK2");

                            diemTBHK1 = diemHK1?.DiemTB?.ToString("F1") ?? "";
                            diemTBHK2 = diemHK2?.DiemTB?.ToString("F1") ?? "";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ignore errors when getting scores
                    }

                    AddResultRow(
                        (i + 1).ToString(),     // STT
                        hocSinh.MaHS ?? "",     // Mã Học Sinh
                        hocSinh.HoTen ?? "",    // Họ Tên
                        hocSinh.GioiTinh ?? "", // Giới Tính
                        hocSinh.DiaChi ?? "",   // Địa Chỉ
                        hocSinh.Email ?? "",    // Email
                        tenLop,                 // Lớp
                        diemTBHK1,              // Điểm TB Học Kỳ 1
                        diemTBHK2               // Điểm TB Học Kỳ 2
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị kết quả: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
