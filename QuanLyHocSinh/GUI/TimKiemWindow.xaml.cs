using DTO;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GUI
{
    public partial class TimKiemWindow : Window
    {
        public TimKiemWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Clear the search results
            dgv_KetQuaTimKiem.ItemsSource = null;
            txBx_TimKiem.Focus();
        }

        private void btn_TimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txBx_TimKiem.Text))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txBx_TimKiem.Focus();
                return;
            }

            try
            {
                string duLieuTimKiem = txBx_TimKiem.Text.Trim();
                List<HocSinh> ketQuaTimKiem = BLL.HocSinhBLL.TimKiemHocSinh(duLieuTimKiem);
                
                if (ketQuaTimKiem != null && ketQuaTimKiem.Count > 0)
                {
                    dgv_KetQuaTimKiem.ItemsSource = ketQuaTimKiem;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy học sinh phù hợp với thông tin đã nhập!", 
                        "Thông báo kết quả tìm kiếm", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information);
                    dgv_KetQuaTimKiem.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", 
                    "Lỗi", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }
    }
} 