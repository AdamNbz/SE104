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

namespace GUI.Sprint7
{
    /// <summary>
    /// Interaction logic for Sprint7Control.xaml
    /// </summary>
    public partial class Sprint7Control : UserControl
    {
        public Sprint7Control()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLop();
        }

        private void LoadLop()
        {
            try
            {
                // Clear ItemsSource instead of Items to avoid binding conflict
                cbx_MonHoc.ItemsSource = null;

                // Use BLL to get real data from database
                var danhSachLop = LopBLL.GetDanhSachLop();
                cbx_MonHoc.ItemsSource = danhSachLop;
                cbx_MonHoc.DisplayMemberPath = "TenLop";
                cbx_MonHoc.SelectedValuePath = "MaLop";

                if (danhSachLop != null && danhSachLop.Count > 0)
                {
                    cbx_MonHoc.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbx_MonHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbx_MonHoc.SelectedItem is Lop selectedLop)
                {
                    // Sử dụng hàm BLL mới để lấy mã lớp theo tên lớp
                    string maLop = LopBLL.LayMaTheoLop(selectedLop.TenLop);
                    txb_MaMonHoc.Text = maLop ?? selectedLop.MaLop; // Fallback nếu không tìm thấy
                    txb_TenMoi.Text = selectedLop.TenLop;
                }
                else
                {
                    txb_MaMonHoc.Text = "";
                    txb_TenMoi.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_LuuThayDoi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbx_MonHoc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txb_TenMoi.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên mới cho lớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedLop = cbx_MonHoc.SelectedItem as Lop;
                string maLop = selectedLop.MaLop;
                string tenMoi = txb_TenMoi.Text.Trim();

                // Kiểm tra tên mới có khác tên cũ không
                if (tenMoi == selectedLop.TenLop)
                {
                    MessageBox.Show("Tên mới phải khác tên hiện tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Xác nhận thay đổi
                var result = MessageBox.Show($"Bạn có chắc chắn muốn thay đổi tên lớp '{selectedLop.TenLop}' thành '{tenMoi}'?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Use BLL to update the class name
                    try
                    {
                        // Call BLL to update in database using the new method
                        bool ketQua = LopBLL.CapNhatTenLop(maLop, tenMoi);

                        if (ketQua)
                        {
                            MessageBox.Show("Thay đổi tên lớp thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Reload danh sách lớp để cập nhật giao diện
                            LoadLop();

                            // Tìm và chọn lại lớp vừa cập nhật bằng cách sử dụng ItemsSource
                            if (cbx_MonHoc.ItemsSource is List<Lop> danhSachLop)
                            {
                                for (int i = 0; i < danhSachLop.Count; i++)
                                {
                                    if (danhSachLop[i].MaLop == maLop)
                                    {
                                        cbx_MonHoc.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật tên lớp. Tên lớp có thể đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception updateEx)
                    {
                        MessageBox.Show($"Lỗi khi cập nhật lớp: {updateEx.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thay đổi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
