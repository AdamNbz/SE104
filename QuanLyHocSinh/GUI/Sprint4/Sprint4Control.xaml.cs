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

namespace GUI.Sprint4
{
    /// <summary>
    /// Interaction logic for Sprint4Control.xaml
    /// </summary>
    public partial class Sprint4Control : UserControl
    {
        public Sprint4Control()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLop();
            LoadMonHoc();
            LoadHocKy();
            LoadStudentsForClass();
        }

        private void LoadLop()
        {
            try
            {
                cbx_Lop.Items.Clear();

                var danhSachLop = LopBLL.GetDanhSachLop();
                foreach (var lop in danhSachLop)
                {
                    cbx_Lop.Items.Add(lop.TenLop);
                }

                if (cbx_Lop.Items.Count > 0)
                {
                    cbx_Lop.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMonHoc()
        {
            try
            {
                cbx_MonHoc.Items.Clear();

                // TODO: Load from database - tạm thời thêm dữ liệu mẫu
                cbx_MonHoc.Items.Add("Toán");
                cbx_MonHoc.Items.Add("Văn");
                cbx_MonHoc.Items.Add("Anh");
                cbx_MonHoc.Items.Add("Lý");
                cbx_MonHoc.Items.Add("Hóa");
                cbx_MonHoc.Items.Add("Sinh");
                cbx_MonHoc.Items.Add("Sử");
                cbx_MonHoc.Items.Add("Địa");

                if (cbx_MonHoc.Items.Count > 0)
                {
                    cbx_MonHoc.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadHocKy()
        {
            try
            {
                cbx_HocKy.Items.Clear();

                // TODO: Load from database - tạm thời thêm dữ liệu mẫu
                cbx_HocKy.Items.Add("Học kỳ 1");
                cbx_HocKy.Items.Add("Học kỳ 2");

                if (cbx_HocKy.Items.Count > 0)
                {
                    cbx_HocKy.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học kỳ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbx_Lop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // TODO: Load students for selected class
                LoadStudentsForClass();
                LoadScoresForSubject(); // Load điểm theo môn học hiện tại
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbx_MonHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // TODO: Load scores for selected subject
                LoadScoresForSubject();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbx_HocKy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // TODO: Load scores for selected semester
                LoadScoresForSemester();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi học kỳ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadStudentsForClass()
        {
            try
            {
                sp_DanhSachHocSinh.Children.Clear();

                if (cbx_Lop.SelectedItem == null)
                {
                    AddSampleStudentRows();
                    return;
                }

                // Lấy mã lớp từ tên lớp được chọn
                string tenLop = cbx_Lop.SelectedItem.ToString();
                var danhSachLop = BLL.LopBLL.GetDanhSachLop();
                var lopDuocChon = danhSachLop.FirstOrDefault(l => l.TenLop == tenLop);

                if (lopDuocChon == null)
                {
                    AddSampleStudentRows();
                    return;
                }

                // Lấy danh sách học sinh trong lớp
                var danhSachHocSinh = BLL.LopBLL.LayDanhSachHocSinh(lopDuocChon.MaLop);

                if (danhSachHocSinh.Count == 0)
                {
                    // Nếu không có học sinh, thêm 3 dòng trống
                    for (int i = 1; i <= 3; i++)
                    {
                        AddStudentRow(i, null, "", "", "");
                    }
                }
                else
                {
                    // Hiển thị học sinh thực tế
                    for (int i = 0; i < danhSachHocSinh.Count; i++)
                    {
                        AddStudentRow(i + 1, danhSachHocSinh[i], "", "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                AddSampleStudentRows();
            }
        }

        private void LoadScoresForSubject()
        {
            // TODO: Implement loading scores for selected subject
        }

        private void LoadScoresForSemester()
        {
            // TODO: Implement loading scores for selected semester
        }

        private void AddSampleStudentRows()
        {
            // Add 3 sample student rows as shown in the image
            for (int i = 1; i <= 3; i++)
            {
                AddStudentRow(i, null, "", "", "");
            }
        }

        private void AddStudentRow(int stt, DTO.HocSinh hocSinh, string diem15Phut, string diem1Tiet, string diemCuoiHK)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                BorderThickness = new Thickness(0, 0, 0, 1)
            };

            Grid grid = new Grid
            {
                Margin = new Thickness(8),
                MinHeight = 40
            };

            // Define columns
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

            // STT
            TextBlock sttTextBlock = new TextBlock
            {
                Text = stt.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(sttTextBlock, 0);
            grid.Children.Add(sttTextBlock);

            // Học Sinh (TextBlock hoặc ComboBox)
            if (hocSinh != null)
            {
                // Hiển thị học sinh cố định
                TextBlock tbHocSinh = new TextBlock
                {
                    Text = $"{hocSinh.MaHS} - {hocSinh.HoTen}",
                    Margin = new Thickness(4),
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = hocSinh.MaHS // Lưu mã học sinh để dùng sau
                };
                Grid.SetColumn(tbHocSinh, 1);
                grid.Children.Add(tbHocSinh);
            }
            else
            {
                // ComboBox để chọn học sinh (cho dòng trống)
                ComboBox cbxHocSinh = new ComboBox
                {
                    Name = $"cbx_HocSinh_{stt}",
                    Margin = new Thickness(4),
                    VerticalAlignment = VerticalAlignment.Center,
                    Background = new SolidColorBrush(Color.FromRgb(211, 211, 211)) // Light gray
                };

                // Load tất cả học sinh để chọn
                var tatCaHocSinh = BLL.HocSinhBLL.GetDanhSachHocSinh();
                cbxHocSinh.Items.Add("-- Chọn học sinh --");
                foreach (var hs in tatCaHocSinh)
                {
                    cbxHocSinh.Items.Add($"{hs.MaHS} - {hs.HoTen}");
                }
                cbxHocSinh.SelectedIndex = 0;

                Grid.SetColumn(cbxHocSinh, 1);
                grid.Children.Add(cbxHocSinh);
            }

            // Điểm 15 phút
            TextBox txtDiem15Phut = new TextBox
            {
                Name = $"txt_Diem15Phut_{stt}",
                Text = diem15Phut,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(txtDiem15Phut, 2);
            grid.Children.Add(txtDiem15Phut);

            // Điểm 1 tiết
            TextBox txtDiem1Tiet = new TextBox
            {
                Name = $"txt_Diem1Tiet_{stt}",
                Text = diem1Tiet,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(txtDiem1Tiet, 3);
            grid.Children.Add(txtDiem1Tiet);

            // Điểm cuối HK
            TextBox txtDiemCuoiHK = new TextBox
            {
                Name = $"txt_DiemCuoiHK_{stt}",
                Text = diemCuoiHK,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(txtDiemCuoiHK, 4);
            grid.Children.Add(txtDiemCuoiHK);

            border.Child = grid;
            sp_DanhSachHocSinh.Children.Add(border);
        }

        private void btn_NhapDiem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra các lựa chọn
                if (cbx_Lop.SelectedItem == null || cbx_MonHoc.SelectedItem == null || cbx_HocKy.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ lớp, môn học và học kỳ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string monHoc = cbx_MonHoc.SelectedItem.ToString();
                string hocKy = cbx_HocKy.SelectedItem.ToString();

                int soLuongDiemDaNhap = 0;
                List<string> errors = new List<string>();

                // Duyệt qua tất cả các dòng học sinh
                foreach (Border border in sp_DanhSachHocSinh.Children)
                {
                    if (border.Child is Grid grid)
                    {
                        string maHocSinh = null;
                        string diem15P = "";
                        string diem1T = "";
                        string diemCuoiKy = "";

                        // Lấy thông tin từ các control trong grid
                        foreach (UIElement child in grid.Children)
                        {
                            int column = Grid.GetColumn(child);

                            if (column == 1) // Cột học sinh
                            {
                                if (child is TextBlock tb && tb.Tag != null)
                                {
                                    maHocSinh = tb.Tag.ToString();
                                }
                                else if (child is ComboBox cbx && cbx.SelectedIndex > 0)
                                {
                                    string selected = cbx.SelectedItem.ToString();
                                    maHocSinh = selected.Split('-')[0].Trim();
                                }
                            }
                            else if (column == 2 && child is TextBox txt15P) // Điểm 15 phút
                            {
                                diem15P = txt15P.Text.Trim();
                            }
                            else if (column == 3 && child is TextBox txt1T) // Điểm 1 tiết
                            {
                                diem1T = txt1T.Text.Trim();
                            }
                            else if (column == 4 && child is TextBox txtCuoiKy) // Điểm cuối kỳ
                            {
                                diemCuoiKy = txtCuoiKy.Text.Trim();
                            }
                        }

                        // Nếu có mã học sinh và ít nhất một điểm được nhập
                        if (!string.IsNullOrEmpty(maHocSinh) &&
                            (!string.IsNullOrEmpty(diem15P) || !string.IsNullOrEmpty(diem1T) || !string.IsNullOrEmpty(diemCuoiKy)))
                        {
                            try
                            {
                                // Tạo bảng điểm mới
                                var bangDiem = new BLL.BangDiemMonBLL
                                {
                                    MaHocSinh = maHocSinh,
                                    MaMH = monHoc,
                                    MaHK = hocKy
                                };

                                var result = bangDiem.TaoBangDiem(maHocSinh, monHoc, hocKy, diem15P, diem1T, diemCuoiKy);
                                if (result != null)
                                {
                                    soLuongDiemDaNhap++;
                                }
                            }
                            catch (Exception ex)
                            {
                                errors.Add($"Học sinh {maHocSinh}: {ex.Message}");
                            }
                        }
                    }
                }

                // Hiển thị kết quả
                if (errors.Count > 0)
                {
                    MessageBox.Show($"Có lỗi khi nhập điểm:\n{string.Join("\n", errors)}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (soLuongDiemDaNhap > 0)
                {
                    MessageBox.Show($"Đã nhập điểm thành công cho {soLuongDiemDaNhap} học sinh!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Không có điểm nào được nhập. Vui lòng nhập ít nhất một điểm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nhập điểm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_TimHocSinh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mở window tìm kiếm học sinh (tương tự Sprint 2)
                var timKiemWindow = new GUI.Sprint2.TimKiemHocSinhWindow();
                timKiemWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở tìm kiếm học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_Thoat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Close the current window or navigate back
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thoát: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
