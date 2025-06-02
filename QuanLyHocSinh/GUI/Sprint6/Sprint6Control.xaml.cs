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
using DAL;

namespace GUI.Sprint6
{
    /// <summary>
    /// Interaction logic for Sprint6Control.xaml
    /// </summary>
    public partial class Sprint6Control : UserControl
    {
        public Sprint6Control()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHocKy();
            // Không hiển thị dòng trống mặc định - chỉ hiển thị khi lập báo cáo
            sp_DanhSachLop.Children.Clear();
        }

        private void LoadHocKy()
        {
            try
            {
                cbx_HocKy.Items.Clear();

                // Load from database using BLL
                cbx_HocKy.ItemsSource = BaoCaoTongKetHocKyBLL.LayDanhSachHocKy();
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
                if (cbx_HocKy.SelectedItem == null)
                    return;

                // Clear existing data
                sp_DanhSachLop.Children.Clear();

                // Generate report automatically when semester changes
                GenerateReport();
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
                if (cbx_HocKy.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn học kỳ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // TODO: Generate actual report
                GenerateReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lập báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateReport()
        {
            try
            {
                // Clear existing data
                sp_DanhSachLop.Children.Clear();

                if (cbx_HocKy.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn học kỳ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Lấy mã học kỳ
                var hocKy = cbx_HocKy.SelectedItem as HocKy;
                if (hocKy == null)
                {
                    MessageBox.Show("Không thể lấy thông tin học kỳ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Lập báo cáo tổng kết học kỳ
                BaoCaoTongKetHocKyBLL.LapBaoCaoTongKetHocKy(hocKy.MaHK);
                var baoCao = BaoCaoTongKetHocKyBLL.LayBaoCaoTongKetHocKy();

                if (baoCao == null || baoCao.DanhSachThongKeLop.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu báo cáo cho học kỳ đã chọn", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Hiển thị báo cáo theo từng lớp
                int stt = 1;
                foreach (var lopThongKe in baoCao.DanhSachThongKeLop)
                {
                    AddClassRow(stt, lopThongKe.TenLop ?? "",
                               lopThongKe.SiSo.ToString(),
                               lopThongKe.SoLuongDat.ToString(),
                               $"{lopThongKe.TyLeDat:F1}%");
                    stt++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



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
                MinHeight = 40
                // Background removed - no gray background
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
            TextBox txtLop = new TextBox
            {
                Text = tenLop,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                IsReadOnly = true
            };
            Grid.SetColumn(txtLop, 1);
            grid.Children.Add(txtLop);

            // Sĩ Số
            TextBox txtSiSo = new TextBox
            {
                Text = siSo,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                IsReadOnly = true
            };
            Grid.SetColumn(txtSiSo, 2);
            grid.Children.Add(txtSiSo);

            // Số Lượng Đạt
            TextBox txtSoLuongDat = new TextBox
            {
                Text = soLuongDat,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                IsReadOnly = true
            };
            Grid.SetColumn(txtSoLuongDat, 3);
            grid.Children.Add(txtSoLuongDat);

            // Tỷ Lệ
            TextBox txtTyLe = new TextBox
            {
                Text = tyLe,
                Margin = new Thickness(4),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                IsReadOnly = true
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