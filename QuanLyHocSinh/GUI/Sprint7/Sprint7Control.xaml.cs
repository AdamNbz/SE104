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
            LoadMonHoc();
        }

        private void LoadMonHoc()
        {
            try
            {
                cbx_MonHoc.Items.Clear();

                // Load from database using BLL
                cbx_MonHoc.ItemsSource = MonHocBLL.LayDanhSachMonHoc();
                cbx_MonHoc.DisplayMemberPath = "TenMH";
                cbx_MonHoc.SelectedValuePath = "MaMH";

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

        private void cbx_MonHoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbx_MonHoc.SelectedItem is MonHoc selectedMonHoc)
                {
                    txb_MaMonHoc.Text = selectedMonHoc.MaMH;
                    txb_TenMoi.Text = selectedMonHoc.TenMH;
                }
                else
                {
                    txb_MaMonHoc.Text = "";
                    txb_TenMoi.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_LuuThayDoi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbx_MonHoc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txb_TenMoi.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên mới cho môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedMonHoc = cbx_MonHoc.SelectedItem as MonHoc;
                string maMH = selectedMonHoc.MaMH;
                string tenMoi = txb_TenMoi.Text.Trim();

                // Kiểm tra tên mới có khác tên cũ không
                if (tenMoi == selectedMonHoc.TenMH)
                {
                    MessageBox.Show("Tên mới phải khác tên hiện tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Xác nhận thay đổi
                var result = MessageBox.Show($"Bạn có chắc chắn muốn thay đổi tên môn học '{selectedMonHoc.TenMH}' thành '{tenMoi}'?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Gọi BLL để cập nhật
                    var updateResult = MonHocBLL.CapNhatTenMonHoc(maMH, tenMoi);

                    if (updateResult.success)
                    {
                        MessageBox.Show("Thay đổi tên môn học thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // Reload danh sách môn học để cập nhật giao diện
                        LoadMonHoc();
                        
                        // Tìm và chọn lại môn học vừa cập nhật
                        for (int i = 0; i < cbx_MonHoc.Items.Count; i++)
                        {
                            if (cbx_MonHoc.Items[i] is MonHoc monHoc && monHoc.MaMH == maMH)
                            {
                                cbx_MonHoc.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi thay đổi tên môn học: {updateResult.message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
