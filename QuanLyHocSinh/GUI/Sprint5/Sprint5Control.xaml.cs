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
            AddSampleClassRows();
        }

        private void LoadMon()
        {
            try
            {
                cbx_Mon.Items.Clear();
                
                // TODO: Load from database - tạm thời thêm dữ liệu mẫu
                cbx_Mon.Items.Add("Toán");
                cbx_Mon.Items.Add("Văn");
                cbx_Mon.Items.Add("Anh");
                cbx_Mon.Items.Add("Lý");
                cbx_Mon.Items.Add("Hóa");
                cbx_Mon.Items.Add("Sinh");
                cbx_Mon.Items.Add("Sử");
                cbx_Mon.Items.Add("Địa");
                
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
            // TODO: Implement updating report data based on selected subject and semester
            // For now, just refresh the sample data
            sp_DanhSachLop.Children.Clear();
            AddSampleClassRows();
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

                // TODO: Generate actual report
                GenerateReport();
                
                MessageBox.Show($"Đã lập báo cáo tổng kết môn {cbx_Mon.SelectedItem} - {cbx_HocKy.SelectedItem}", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lập báo cáo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateReport()
        {
            // Clear existing data
            sp_DanhSachLop.Children.Clear();

            // TODO: Get actual data from database
            // For now, add sample data with calculated statistics
            var sampleData = new[]
            {
                new { STT = 1, Lop = "10A1", SiSo = 40, SoLuongDat = 35, TyLe = "87.5%" },
                new { STT = 2, Lop = "10A2", SiSo = 38, SoLuongDat = 32, TyLe = "84.2%" },
                new { STT = 3, Lop = "10A3", SiSo = 42, SoLuongDat = 38, TyLe = "90.5%" }
            };

            foreach (var item in sampleData)
            {
                AddClassRow(item.STT, item.Lop, item.SiSo.ToString(), item.SoLuongDat.ToString(), item.TyLe);
            }
        }

        private void AddSampleClassRows()
        {
            // Add 3 empty sample rows as shown in the image
            for (int i = 1; i <= 3; i++)
            {
                AddClassRow(i, "", "", "", "");
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
                MinHeight = 40,
                Background = new SolidColorBrush(Color.FromRgb(211, 211, 211)) // Light gray
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
                Background = new SolidColorBrush(Color.FromRgb(211, 211, 211)),
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
                Background = new SolidColorBrush(Color.FromRgb(211, 211, 211)),
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
                Background = new SolidColorBrush(Color.FromRgb(211, 211, 211)),
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
                Background = new SolidColorBrush(Color.FromRgb(211, 211, 211)),
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
