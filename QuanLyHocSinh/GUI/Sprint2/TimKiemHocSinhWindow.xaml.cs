using DTO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.Sprint2
{
    /// <summary>
    /// Interaction logic for TimKiemHocSinhWindow.xaml
    /// </summary>
    public partial class TimKiemHocSinhWindow : Window
    {
        public HocSinh HocSinhDuocChon { get; private set; }

        public TimKiemHocSinhWindow()
        {
            InitializeComponent();
        }

        private void txb_TimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TimKiemHocSinh();
            }
        }

        private void btn_TimKiem_Click(object sender, RoutedEventArgs e)
        {
            TimKiemHocSinh();
        }

        private void TimKiemHocSinh()
        {
            try
            {
                string tuKhoa = txb_TimKiem.Text.Trim();
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    MessageBox.Show("Vui lòng nhập thông tin cần tìm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Tìm kiếm học sinh
                List<HocSinh> ketQua = BLL.HocSinhBLL.TimKiemHocSinh(tuKhoa);

                // Hiển thị kết quả
                dgv_KetQuaTimKiem.ItemsSource = ketQua;

                // Thông báo kết quả
                if (ketQua.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy học sinh phù hợp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgv_KetQuaTimKiem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Không cần xử lý gì đặc biệt khi selection thay đổi
        }

        private void dgv_KetQuaTimKiem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Chọn học sinh khi double-click
            if (dgv_KetQuaTimKiem.SelectedItem != null)
            {
                HocSinhDuocChon = dgv_KetQuaTimKiem.SelectedItem as HocSinh;
                DialogResult = true;
            }
        }

        private void btn_Thoat_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ
            this.Close();
        }
    }
}
