﻿using System;
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
// using GUI.Sprint7; // Not needed since we're in the same namespace

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

        // Method public để reload danh sách lớp từ bên ngoài
        public void ReloadDanhSachLop()
        {
            try
            {
                LoadLop(); // Reload tab 3 (Thay đổi tên lớp)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi reload danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method để reload các controls khác (Sprint 2, etc.)
        private void ReloadOtherControls()
        {
            try
            {
                // Tìm MainWindow và reload Sprint 2 nếu có
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    // Tìm Sprint2Control trong MainWindow
                    var sprint2Control = FindVisualChild<Sprint2.Sprint2Control>(mainWindow);
                    if (sprint2Control != null)
                    {
                        sprint2Control.ReloadDanhSachLop();
                        System.Diagnostics.Debug.WriteLine("Reloaded Sprint 2 danh sách lớp");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reloading other controls: {ex.Message}");
                // Không hiển thị MessageBox để tránh làm phiền user
            }
        }

        // Helper method để tìm child control
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                    return (T)child;

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLop();
            LoadQuyDinh();
            LoadTab2Data(); // Load for tab 2
            LoadTab4Data(); // Load for tab 4
            LoadTab5Data(); // Load for tab 5
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

                if (selectedLop == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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

        // Methods for "Thay đổi quy định" tab
        // Lưu giá trị cũ để reset khi có lỗi
        private int oldTuoiToiThieu, oldTuoiToiDa, oldSiSoToiDa, oldDiemChuanDatMon;

        private void LoadQuyDinh()
        {
            try
            {
                var quyDinhBLL = new QuyDinhBLL();
                var thamSo = quyDinhBLL.LayQuyDinh();

                if (thamSo != null)
                {
                    // Lưu giá trị cũ
                    oldTuoiToiThieu = thamSo.TuoiToiThieu;
                    oldTuoiToiDa = thamSo.TuoiToiDa;
                    oldSiSoToiDa = thamSo.SiSoToiDa;
                    oldDiemChuanDatMon = thamSo.MocDiemDat;

                    // Ensure TextBoxes are properly set with current values
                    txb_TuoiToiThieu.Text = thamSo.TuoiToiThieu.ToString();
                    txb_TuoiToiDa.Text = thamSo.TuoiToiDa.ToString();
                    txb_SiSoToiDa.Text = thamSo.SiSoToiDa.ToString();
                    txb_DiemChuanDatMon.Text = thamSo.MocDiemDat.ToString();

                    // Debug: Show loaded values
                    System.Diagnostics.Debug.WriteLine($"Loaded values: Tuoi {thamSo.TuoiToiThieu}-{thamSo.TuoiToiDa}, SiSo {thamSo.SiSoToiDa}, Diem {thamSo.MocDiemDat}");
                }
                else
                {
                    // Set default values if no data found
                    oldTuoiToiThieu = 15;
                    oldTuoiToiDa = 20;
                    oldSiSoToiDa = 40;
                    oldDiemChuanDatMon = 5;

                    txb_TuoiToiThieu.Text = "15";
                    txb_TuoiToiDa.Text = "20";
                    txb_SiSoToiDa.Text = "40";
                    txb_DiemChuanDatMon.Text = "5";

                    System.Diagnostics.Debug.WriteLine("No ThamSo found, using default values");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải quy định: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in LoadQuyDinh: {ex.Message}");
            }
        }

        private void btn_LuuQuyDinh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Debug: Show TextBox values before parsing
                System.Diagnostics.Debug.WriteLine("=== LƯU QUY ĐỊNH ===");
                System.Diagnostics.Debug.WriteLine($"Before parsing - TextBox values:");
                System.Diagnostics.Debug.WriteLine($"- TuoiToiThieu: '{txb_TuoiToiThieu.Text}'");
                System.Diagnostics.Debug.WriteLine($"- TuoiToiDa: '{txb_TuoiToiDa.Text}'");
                System.Diagnostics.Debug.WriteLine($"- SiSoToiDa: '{txb_SiSoToiDa.Text}'");
                System.Diagnostics.Debug.WriteLine($"- DiemChuanDatMon: '{txb_DiemChuanDatMon.Text}'");

                // Validate input và reset về giá trị cũ nếu có ô trống
                bool hasEmptyField = false;
                int tuoiToiThieu, tuoiToiDa, siSoToiDa, diemChuanDatMon;

                if (string.IsNullOrWhiteSpace(txb_TuoiToiThieu.Text) || !int.TryParse(txb_TuoiToiThieu.Text, out tuoiToiThieu))
                {
                    txb_TuoiToiThieu.Text = oldTuoiToiThieu.ToString();
                    tuoiToiThieu = oldTuoiToiThieu;
                    hasEmptyField = true;
                }

                if (string.IsNullOrWhiteSpace(txb_TuoiToiDa.Text) || !int.TryParse(txb_TuoiToiDa.Text, out tuoiToiDa))
                {
                    txb_TuoiToiDa.Text = oldTuoiToiDa.ToString();
                    tuoiToiDa = oldTuoiToiDa;
                    hasEmptyField = true;
                }

                if (string.IsNullOrWhiteSpace(txb_SiSoToiDa.Text) || !int.TryParse(txb_SiSoToiDa.Text, out siSoToiDa))
                {
                    txb_SiSoToiDa.Text = oldSiSoToiDa.ToString();
                    siSoToiDa = oldSiSoToiDa;
                    hasEmptyField = true;
                }

                if (string.IsNullOrWhiteSpace(txb_DiemChuanDatMon.Text) || !int.TryParse(txb_DiemChuanDatMon.Text, out diemChuanDatMon))
                {
                    txb_DiemChuanDatMon.Text = oldDiemChuanDatMon.ToString();
                    diemChuanDatMon = oldDiemChuanDatMon;
                    hasEmptyField = true;
                }

                // Thông báo nếu có ô trống đã được reset
                if (hasEmptyField)
                {
                    MessageBox.Show("Hãy nhập giá trị mới.",
                                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Additional validation
                if (tuoiToiThieu <= 0)
                {
                    MessageBox.Show("Tuổi tối thiểu phải lớn hơn 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    txb_TuoiToiThieu.Focus();
                    return;
                }

                if (tuoiToiDa <= 0)
                {
                    MessageBox.Show("Tuổi tối đa phải lớn hơn 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    txb_TuoiToiDa.Focus();
                    return;
                }

                if (tuoiToiThieu >= tuoiToiDa)
                {
                    MessageBox.Show("Tuổi tối thiểu phải nhỏ hơn tuổi tối đa", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    txb_TuoiToiThieu.Focus();
                    return;
                }

                if (siSoToiDa <= 0)
                {
                    MessageBox.Show("Sĩ số tối đa phải lớn hơn 0", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    txb_SiSoToiDa.Focus();
                    return;
                }

                if (diemChuanDatMon < 0 || diemChuanDatMon > 10)
                {
                    MessageBox.Show("Điểm chuẩn đạt môn phải từ 0 đến 10", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    txb_DiemChuanDatMon.Focus();
                    return;
                }

                // Debug: Show values being saved
                var debugMessage = $"Giá trị sẽ được lưu:\n" +
                                 $"- Tuổi tối thiểu: {tuoiToiThieu}\n" +
                                 $"- Tuổi tối đa: {tuoiToiDa}\n" +
                                 $"- Sĩ số tối đa: {siSoToiDa}\n" +
                                 $"- Điểm chuẩn: {diemChuanDatMon}\n\n" +
                                 $"Bạn có chắc chắn muốn lưu?";

                // Confirm changes
                var result = MessageBox.Show(debugMessage, "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var quyDinhBLL = new QuyDinhBLL();

                    // Update all parameters at once using the new method
                    try
                    {
                        bool success = quyDinhBLL.CapNhatTatCaQuyDinh(tuoiToiThieu, tuoiToiDa, siSoToiDa, diemChuanDatMon);

                        if (success)
                        {
                            MessageBox.Show("Cập nhật quy định thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadQuyDinh(); // Reload to show updated values
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật quy định. Có thể do lỗi database.", "Lỗi cập nhật", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi cập nhật quy định: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi cập nhật", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu quy định: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_TaiLaiQuyDinh_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("=== TẢI LẠI QUY ĐỊNH ===");
            LoadQuyDinh();

            // Debug: Show current TextBox values after reload
            System.Diagnostics.Debug.WriteLine($"After reload - TextBox values:");
            System.Diagnostics.Debug.WriteLine($"- TuoiToiThieu: '{txb_TuoiToiThieu.Text}'");
            System.Diagnostics.Debug.WriteLine($"- TuoiToiDa: '{txb_TuoiToiDa.Text}'");
            System.Diagnostics.Debug.WriteLine($"- SiSoToiDa: '{txb_SiSoToiDa.Text}'");
            System.Diagnostics.Debug.WriteLine($"- DiemChuanDatMon: '{txb_DiemChuanDatMon.Text}'");
        }

        // Methods for Tab 2: Thêm một lớp mới
        private string maLopPhatSinh = "";

        private void LoadTab2Data()
        {
            try
            {
                // Load danh sách khối
                LoadDanhSachKhoi();

                // Generate mã lớp mới
                GenerateMaLop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu tab 2: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in LoadTab2Data: {ex.Message}");
            }
        }

        private void LoadDanhSachKhoi()
        {
            try
            {
                var danhSachKhoi = KhoiBLL.GetDanhSachKhoi();
                cbx_LopThuocKhoi.ItemsSource = danhSachKhoi;

                if (danhSachKhoi.Count > 0)
                {
                    cbx_LopThuocKhoi.SelectedIndex = 0;
                }

                System.Diagnostics.Debug.WriteLine($"Loaded {danhSachKhoi.Count} khối for ComboBox");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khối: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in LoadDanhSachKhoi: {ex.Message}");
            }
        }

        private void GenerateMaLop()
        {
            try
            {
                maLopPhatSinh = LopBLL.PhatSinhMaLop();
                txb_MaLopHoc.Text = maLopPhatSinh;
                txb_MaLopHoc.Foreground = System.Windows.Media.Brushes.Black;

                System.Diagnostics.Debug.WriteLine($"Generated MaLop: {maLopPhatSinh}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi phát sinh mã lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in GenerateMaLop: {ex.Message}");
            }
        }

        private void txb_TenLopMoi_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Enable/disable button based on input
            btn_ThemLop.IsEnabled = !string.IsNullOrWhiteSpace(txb_TenLopMoi.Text) &&
                                   cbx_LopThuocKhoi.SelectedItem != null;
        }

        private void cbx_LopThuocKhoi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Enable/disable button based on selection
            btn_ThemLop.IsEnabled = !string.IsNullOrWhiteSpace(txb_TenLopMoi.Text) &&
                                   cbx_LopThuocKhoi.SelectedItem != null;
        }

        private void btn_ThemLop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txb_TenLopMoi.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên lớp mới", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txb_TenLopMoi.Focus();
                    return;
                }

                if (cbx_LopThuocKhoi.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn khối cho lớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cbx_LopThuocKhoi.Focus();
                    return;
                }

                string tenLop = txb_TenLopMoi.Text.Trim();
                var selectedKhoi = cbx_LopThuocKhoi.SelectedItem as Khoi;

                if (selectedKhoi == null)
                {
                    MessageBox.Show("Vui lòng chọn khối cho lớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    cbx_LopThuocKhoi.Focus();
                    return;
                }

                string maKhoi = selectedKhoi.MaKhoi;

                // Confirm action
                var result = MessageBox.Show($"Bạn có chắc chắn muốn thêm lớp mới?\n\n" +
                                           $"Tên lớp: {tenLop}\n" +
                                           $"Mã lớp: {maLopPhatSinh}\n" +
                                           $"Khối: {selectedKhoi.TenKhoi}",
                                           "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Add new class
                    bool success = LopBLL.ThemLopMoi(tenLop, maKhoi);

                    if (success)
                    {
                        MessageBox.Show("Thêm lớp mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Reset form
                        txb_TenLopMoi.Text = "";
                        GenerateMaLop(); // Generate new code
                        cbx_LopThuocKhoi.SelectedIndex = 0;

                        // Reload danh sách lớp trong tab 3 (Thay đổi tên lớp)
                        LoadLop();

                        // Reload danh sách lớp trong Sprint 2 nếu có
                        ReloadOtherControls();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm lớp mới. Tên lớp có thể đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm lớp mới: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in btn_ThemLop_Click: {ex.Message}");
            }
        }

        private void btn_ThoatThemLop_Click(object sender, RoutedEventArgs e)
        {
            // Thoát khỏi ứng dụng
            Application.Current.Shutdown();
        }

        // Methods for Tab 4: Thêm môn học mới
        private string maMonHocPhatSinh = "";

        private void LoadTab4Data()
        {
            try
            {
                // Generate mã môn học mới
                GenerateMaMonHoc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu tab 4: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in LoadTab4Data: {ex.Message}");
            }
        }

        private void GenerateMaMonHoc()
        {
            try
            {
                maMonHocPhatSinh = MonHocBLL.PhatSinhMaMonHoc();
                txb_MaMonHocMoi.Text = maMonHocPhatSinh;
                txb_MaMonHocMoi.Foreground = System.Windows.Media.Brushes.Black;

                System.Diagnostics.Debug.WriteLine($"Generated MaMonHoc: {maMonHocPhatSinh}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi phát sinh mã môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in GenerateMaMonHoc: {ex.Message}");
            }
        }

        private void txb_TenMonHoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Enable/disable button based on input
            btn_ThemMonHoc.IsEnabled = !string.IsNullOrWhiteSpace(txb_TenMonHoc.Text);
        }

        private void btn_ThemMonHoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txb_TenMonHoc.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txb_TenMonHoc.Focus();
                    return;
                }

                string tenMonHoc = txb_TenMonHoc.Text.Trim();

                // Confirm action
                var result = MessageBox.Show($"Bạn có chắc chắn muốn thêm môn học mới?\n\n" +
                                           $"Tên môn học: {tenMonHoc}\n" +
                                           $"Mã môn học: {maMonHocPhatSinh}",
                                           "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Add new subject
                    bool success = MonHocBLL.ThemMonHocMoi(tenMonHoc);

                    if (success)
                    {
                        MessageBox.Show("Thêm môn học mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Reset form
                        txb_TenMonHoc.Text = "";
                        GenerateMaMonHoc(); // Generate new code
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm môn học mới. Tên môn học có thể đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm môn học mới: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in btn_ThemMonHoc_Click: {ex.Message}");
            }
        }

        private void btn_ThoatThemMonHoc_Click(object sender, RoutedEventArgs e)
        {
            // Thoát khỏi ứng dụng
            Application.Current.Shutdown();
        }

        // Methods for Tab 5: Thay đổi tên môn học
        private void LoadTab5Data()
        {
            try
            {
                LoadDanhSachMonHoc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu tab 5: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in LoadTab5Data: {ex.Message}");
            }
        }

        private void LoadDanhSachMonHoc()
        {
            try
            {
                var danhSachMonHoc = MonHocBLL.LayDanhSachMonHoc();
                cbx_MonHocHienTai.ItemsSource = danhSachMonHoc;
                cbx_MonHocHienTai.DisplayMemberPath = "TenMH";
                cbx_MonHocHienTai.SelectedValuePath = "MaMH";

                if (danhSachMonHoc.Count > 0)
                {
                    cbx_MonHocHienTai.SelectedIndex = 0;
                }

                System.Diagnostics.Debug.WriteLine($"Loaded {danhSachMonHoc.Count} môn học for ComboBox");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in LoadDanhSachMonHoc: {ex.Message}");
            }
        }

        private void cbx_MonHocHienTai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbx_MonHocHienTai.SelectedItem is MonHoc selectedMonHoc)
                {
                    txb_MaMonHocHienTai.Text = selectedMonHoc.MaMH;
                    txb_TenMonHocMoi.Text = selectedMonHoc.TenMH;
                }
                else
                {
                    txb_MaMonHocHienTai.Text = "";
                    txb_TenMonHocMoi.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in cbx_MonHocHienTai_SelectionChanged: {ex.Message}");
            }
        }

        private void btn_ThayDoiTenMonHoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbx_MonHocHienTai.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txb_TenMonHocMoi.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên mới cho môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedMonHoc = cbx_MonHocHienTai.SelectedItem as MonHoc;

                if (selectedMonHoc == null)
                {
                    MessageBox.Show("Vui lòng chọn môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string maMonHoc = selectedMonHoc.MaMH;
                string tenMoi = txb_TenMonHocMoi.Text.Trim();

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
                    try
                    {
                        // Call BLL to update in database
                        bool ketQua = MonHocBLL.ThayDoiTenMonHoc(maMonHoc, tenMoi);

                        if (ketQua)
                        {
                            MessageBox.Show("Thay đổi tên môn học thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Reload danh sách môn học để cập nhật giao diện
                            LoadDanhSachMonHoc();

                            // Tìm và chọn lại môn học vừa cập nhật
                            if (cbx_MonHocHienTai.ItemsSource is List<MonHoc> danhSachMonHoc)
                            {
                                for (int i = 0; i < danhSachMonHoc.Count; i++)
                                {
                                    if (danhSachMonHoc[i].MaMH == maMonHoc)
                                    {
                                        cbx_MonHocHienTai.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật tên môn học. Tên môn học có thể đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception updateEx)
                    {
                        MessageBox.Show($"Lỗi khi cập nhật môn học: {updateEx.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi tên môn học: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in btn_ThayDoiTenMonHoc_Click: {ex.Message}");
            }
        }

        private void btn_ThoatThayDoiMonHoc_Click(object sender, RoutedEventArgs e)
        {
            // Thoát khỏi ứng dụng
            Application.Current.Shutdown();
        }
    }
}
