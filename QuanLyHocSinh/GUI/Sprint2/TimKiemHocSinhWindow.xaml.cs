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
            // Kích hoạt nút Chọn nếu có học sinh được chọn
            btn_Chon.IsEnabled = dgv_KetQuaTimKiem.SelectedItem != null;
        }

        private void btn_Chon_Click(object sender, RoutedEventArgs e)
        {
            // Lấy học sinh được chọn
            HocSinhDuocChon = dgv_KetQuaTimKiem.SelectedItem as HocSinh;
            
            // Đóng cửa sổ với kết quả thành công
            DialogResult = true;
        }

        private void btn_Huy_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ với kết quả hủy
            DialogResult = false;
        }
    }
}
