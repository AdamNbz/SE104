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

namespace GUI.Sprint5
{
    /// <summary>
    /// Interaction logic for Sprint5Control.xaml
    /// </summary>
    public partial class Sprint5Control : UserControl
    {
        public Sprint5Control()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMon();
            LoadHocKy();
            // Không hiển thị dòng trống mặc định - chỉ hiển thị khi lập báo cáo
            sp_DanhSachLop.Children.Clear();
        }

        private void LoadMon()
        {
            try
            {
                cbx_Mon.Items.Clear();

                // Load from database
                cbx_Mon.ItemsSource = BaoCaoTongKetMonBLL.LayDanhSachMonHoc();
                cbx_Mon.DisplayMemberPath = "TenMH";
                cbx_Mon.SelectedValuePath = "MaMH";

                if (cbx_Mon.Items.Count > 0)
                {
                    cbx_Mon.SelectedIndex = 0;
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
                //cbx_HocKy.Items.Clear();

                //// TODO: Load from database - tạm thời thêm dữ liệu mẫu
                //cbx_HocKy.Items.Add("Học kỳ 1");
                //cbx_HocKy.Items.Add("Học kỳ 2");
                cbx_HocKy.ItemsSource = BaoCaoTongKetMonBLL.LayDanhSachHocKy();
                cbx_HocKy.DisplayMemberPath = "TenHK";
                cbx_HocKy.SelectedValuePath = "MaHK";
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

        private void cbx_Mon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // TODO: Update report data when subject changes
                UpdateReportData();
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
                // TODO: Update report data when semester changes
                UpdateReportData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi học kỳ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateReportData()
        {
            try
            {
                if (cbx_Mon.SelectedItem == null || cbx_HocKy.SelectedItem == null)
                    return;

                // Clear existing data
                sp_DanhSachLop.Children.Clear();

                // Sử dụng dữ liệu từ LopBLL để tạo báo cáo
                GenerateReportFromBLL();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_LapBaoCao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbx_Mon.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbx_HocKy.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn học kỳ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Generate report từ BLL
                GenerateReportFromBLL();

                MessageBox.Show($"Đã lập báo cáo tổng kết môn {cbx_Mon.SelectedItem} - {cbx_HocKy.SelectedItem}",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lập báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateReportFromBLL()
        {
            try
            {
                // Clear existing data
                sp_DanhSachLop.Children.Clear();

                if (cbx_Mon.SelectedItem == null || cbx_HocKy.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn môn học và học kỳ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Lấy mã môn học và mã học kỳ
                var monHoc = cbx_Mon.SelectedItem as MonHoc;
                var hocKy = cbx_HocKy.SelectedItem as HocKy;

                if (monHoc == null || hocKy == null)
                {
                    MessageBox.Show("Không thể lấy thông tin môn học hoặc học kỳ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Lập báo cáo tổng kết môn học
                BaoCaoTongKetMonBLL.LapBaoCaoTongKetMon(monHoc.MaMH, hocKy.MaHK);
                var baoCao = BaoCaoTongKetMonBLL.LayBaoCaoTongKetMon();

                if (baoCao == null)
                {
                    MessageBox.Show("Không có dữ liệu báo cáo", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Hiển thị báo cáo tổng kết - sử dụng dữ liệu đã được tính toán từ BLL
                // BLL đã tính toán sẵn tổng số và số lượng đạt, chỉ cần hiển thị theo từng lớp

                // Log mốc điểm đạt để debug
                int mocDiemDatCheck = BaoCaoTongKetMonBLL.LayMocDiemDat();
                MessageBox.Show($"Mốc điểm đạt hiện tại: {mocDiemDatCheck}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                var danhSachLop = LopBLL.GetDanhSachLop();
                int stt = 1;

                foreach (var lop in danhSachLop)
                {
                    // Lấy học sinh có điểm môn này trong lớp
                    var hocSinhTrongLop = baoCao.ChiTietBangDiem?
                        .Where(bd => bd.HocSinh?.MaLop == lop.MaLop)
                        .ToList() ?? new List<BangDiemMon>();

                    int siSo = hocSinhTrongLop.Count;

                    if (siSo > 0) // Chỉ hiển thị lớp có học sinh có điểm
                    {
                        // Tính số lượng đạt thực tế trong lớp này
                        // Lấy mốc điểm đạt từ ThamSo (giống như BLL đã làm)
                        int mocDiemDat = BaoCaoTongKetMonBLL.LayMocDiemDat();

                        // Log để debug
                        System.Diagnostics.Debug.WriteLine($"[Sprint5] MocDiemDat từ ThamSo: {mocDiemDat}");

                        // Đếm số học sinh đạt trong lớp dựa trên điểm thực tế
                        int soLuongDat = 0;
                        foreach (var bangDiem in hocSinhTrongLop)
                        {
                            // Kiểm tra điểm có đạt không (sử dụng cùng logic như BLL)
                            // Sử dụng mốc điểm đạt thật từ ThamSo thay vì hardcode
                            if (bangDiem.DiemCuoiKy.HasValue && bangDiem.DiemCuoiKy >= mocDiemDat)
                            {
                                soLuongDat++;
                            }
                        }

                        double tyLeDat = siSo > 0 ? (soLuongDat * 100.0 / siSo) : 0;

                        AddClassRow(stt, lop.TenLop, siSo.ToString(),
                                   soLuongDat.ToString(), $"{tyLeDat:F1}%");
                        stt++;
                    }
                }

                if (stt == 1) // Không có lớp nào có dữ liệu
                {
                    MessageBox.Show("Không có dữ liệu điểm cho môn học và học kỳ đã chọn", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Đã xóa AddSampleClassRows - không cần dòng trống mặc định

        private void AddClassRow(int stt, string tenLop, string siSo, string soLuongDat, string tyLe)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                BorderThickness = new Thickness(0, 0, 0, 1)
            };

            Grid grid = new Grid
            {
                Margin = new Thickness(8),
                MinHeight = 40,
                Background = Brushes.White // Nền trắng thay vì xám
            };

            // Define columns
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

            // STT
            TextBlock sttTextBlock = new TextBlock
            {
                Text = stt.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(sttTextBlock, 0);
            grid.Children.Add(sttTextBlock);

            // Lớp
            TextBlock txtLop = new TextBlock
            {
                Text = tenLop,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 12,
                Foreground = string.IsNullOrEmpty(tenLop) ? Brushes.LightGray : Brushes.Black
            };
            Grid.SetColumn(txtLop, 1);
            grid.Children.Add(txtLop);

            // Sĩ Số
            TextBlock txtSiSo = new TextBlock
            {
                Text = siSo,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 12,
                Foreground = string.IsNullOrEmpty(siSo) ? Brushes.LightGray : Brushes.Black
            };
            Grid.SetColumn(txtSiSo, 2);
            grid.Children.Add(txtSiSo);

            // Số Lượng Đạt
            TextBlock txtSoLuongDat = new TextBlock
            {
                Text = soLuongDat,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 12,
                Foreground = string.IsNullOrEmpty(soLuongDat) ? Brushes.LightGray : Brushes.Black
            };
            Grid.SetColumn(txtSoLuongDat, 3);
            grid.Children.Add(txtSoLuongDat);

            // Tỷ Lệ
            TextBlock txtTyLe = new TextBlock
            {
                Text = tyLe,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 12,
                Foreground = string.IsNullOrEmpty(tyLe) ? Brushes.LightGray : Brushes.Black
            };
            Grid.SetColumn(txtTyLe, 4);
            grid.Children.Add(txtTyLe);

            border.Child = grid;
            sp_DanhSachLop.Children.Add(border);
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