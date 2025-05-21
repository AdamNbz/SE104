using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;

namespace GUI.Sprint2
{
    /// <summary>
    /// Interaction logic for Sprint2Control.xaml
    /// </summary>
    public partial class Sprint2Control : UserControl
    {
        // Khai báo các biến cần thiết
        private List<Lop> danhSachLop;
        private List<HocSinh> danhSachHocSinh;

        public Sprint2Control()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy danh sách lớp từ BLL
                danhSachLop = BLL.LopBLL.GetDanhSachLop();

                // Cập nhật ComboBox
                cbx_Lop.ItemsSource = danhSachLop;
                cbx_Lop.DisplayMemberPath = "TenLop";
                cbx_Lop.SelectedValuePath = "MaLop";

                // Lấy danh sách học sinh
                try
                {
                    danhSachHocSinh = BLL.HocSinhBLL.GetDanhSachHocSinh();

                    // Hiển thị thông báo debug
                    string message = "Thông tin danh sách học sinh:\n";

                    // Kiểm tra danh sách học sinh
                    if (danhSachHocSinh == null)
                    {
                        message += "Danh sách học sinh là NULL";
                        MessageBox.Show(message, "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        danhSachHocSinh = new List<HocSinh>();
                    }
                    else if (danhSachHocSinh.Count == 0)
                    {
                        message += "Danh sách học sinh trống (Count = 0)";
                        MessageBox.Show(message, "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        message += $"Số lượng học sinh: {danhSachHocSinh.Count}\n";
                        foreach (var hs in danhSachHocSinh)
                        {
                            message += $"Học sinh: {hs.MaHS} - {hs.HoTen}\n";
                        }
                        MessageBox.Show(message, "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy danh sách học sinh: {ex.Message}\n{ex.StackTrace}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    danhSachHocSinh = new List<HocSinh>();
                }

                // Khởi tạo ComboBox cho các dòng học sinh
                foreach (UIElement element in sp_DanhSachHocSinh.Children)
                {
                    if (element is Grid hocSinhGrid)
                    {
                        foreach (UIElement control in hocSinhGrid.Children)
                        {
                            if (control is ComboBox comboBox)
                            {
                                comboBox.ItemsSource = danhSachHocSinh;
                                comboBox.DisplayMemberPath = "HoTen";
                                comboBox.SelectedValuePath = "MaHS";

                                // Thêm sự kiện SelectionChanged
                                comboBox.SelectionChanged += HocSinh_ComboBox_SelectionChanged;
                            }
                        }
                    }
                }

                // Lấy sĩ số tối đa từ ThamSo
                try
                {
                    var thamSo = DAL.DataContext.Context.THAMSO.FirstOrDefault();
                    if (thamSo != null)
                    {
                        txb_SiSoToiDa.Text = thamSo.SiSoToiDa.ToString();
                    }
                    else
                    {
                        txb_SiSoToiDa.Text = "40"; // Giá trị mặc định
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy sĩ số tối đa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    txb_SiSoToiDa.Text = "40"; // Giá trị mặc định
                }

                // Chọn lớp đầu tiên nếu có
                if (danhSachLop.Count > 0)
                {
                    cbx_Lop.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sự kiện khi chọn học sinh trong ComboBox
        private void HocSinh_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
                {
                    // Debug: Hiển thị loại đối tượng được chọn
                    MessageBox.Show($"Loại đối tượng được chọn: {comboBox.SelectedItem.GetType().FullName}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    HocSinh hocSinh = comboBox.SelectedItem as HocSinh;
                    if (hocSinh != null)
                    {
                        // Debug: Hiển thị thông tin học sinh
                        MessageBox.Show($"Học sinh được chọn: {hocSinh.MaHS} - {hocSinh.HoTen}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Tìm Grid cha của ComboBox
                        FrameworkElement parent = comboBox.Parent as FrameworkElement;
                        while (parent != null && !(parent is Grid))
                        {
                            parent = parent.Parent as FrameworkElement;
                        }

                        if (parent is Grid grid)
                        {
                            // Cập nhật thông tin học sinh
                            UpdateHocSinhInfo(grid, hocSinh);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể chuyển đổi đối tượng được chọn thành HocSinh", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void cbx_Lop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbx_Lop.SelectedItem != null)
                {
                    Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                    if (lopDuocChon == null) return;

                    // Cập nhật thông tin khối
                    txb_Khoi.Text = lopDuocChon.MaKhoi ?? "";

                    // Tính sĩ số lớp
                    List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocsinh(lopDuocChon.MaLop);

                    // Nếu danh sách học sinh trong lớp là null, khởi tạo một danh sách trống
                    if (danhSachHocSinhTrongLop == null)
                    {
                        danhSachHocSinhTrongLop = new List<HocSinh>();
                    }

                    txb_SiSo.Text = danhSachHocSinhTrongLop.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        private void btn_LapDanhSachLop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbx_Lop.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp trước khi lập danh sách", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                if (lopDuocChon == null) return;

                // Lấy danh sách học sinh đã chọn
                List<HocSinh> danhSachHocSinhDaChon = new List<HocSinh>();
                List<string> danhSachMaHS = new List<string>();

                foreach (UIElement element in sp_DanhSachHocSinh.Children)
                {
                    if (element is Grid hocSinhGrid)
                    {
                        // Tìm ComboBox trong grid
                        foreach (UIElement control in hocSinhGrid.Children)
                        {
                            if (control is ComboBox comboBox && comboBox.SelectedItem != null)
                            {
                                HocSinh hocSinh = comboBox.SelectedItem as HocSinh;
                                if (hocSinh != null)
                                {
                                    danhSachHocSinhDaChon.Add(hocSinh);
                                    danhSachMaHS.Add(hocSinh.MaHS);
                                }
                            }
                        }
                    }
                }

                if (danhSachHocSinhDaChon.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một học sinh để lập danh sách", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Xác nhận lập danh sách
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn lập danh sách {danhSachHocSinhDaChon.Count} học sinh cho lớp {lopDuocChon.TenLop}?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Lập danh sách học sinh
                    BLL.PhanLopBLL.PhanLopChoMotDanhSachHocSinh(lopDuocChon.MaLop, danhSachMaHS);

                    // Cập nhật sĩ số lớp
                    txb_SiSo.Text = BLL.LopBLL.TinhSiSo(BLL.HocSinhBLL.GetDanhSachHocSinh(), lopDuocChon.MaLop).ToString();

                    MessageBox.Show($"Đã lập danh sách {danhSachHocSinhDaChon.Count} học sinh cho lớp {lopDuocChon.TenLop}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lập danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_LamMoi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Cập nhật lại danh sách lớp
                danhSachLop = BLL.LopBLL.GetDanhSachLop();
                cbx_Lop.ItemsSource = danhSachLop;

                // Cập nhật lại thông tin lớp
                if (cbx_Lop.SelectedItem != null)
                {
                    cbx_Lop_SelectionChanged(null, null);
                }

                // Cập nhật lại danh sách học sinh trong các ComboBox
                foreach (UIElement element in sp_DanhSachHocSinh.Children)
                {
                    if (element is Grid hocSinhGrid)
                    {
                        foreach (UIElement control in hocSinhGrid.Children)
                        {
                            if (control is ComboBox comboBox)
                            {
                                // Lưu lại item đã chọn
                                var selectedItem = comboBox.SelectedItem;

                                // Cập nhật lại danh sách
                                comboBox.ItemsSource = BLL.HocSinhBLL.GetDanhSachHocSinh();
                                comboBox.DisplayMemberPath = "HoTen";
                                comboBox.SelectedValuePath = "MaHS";

                                // Khôi phục lại item đã chọn
                                if (selectedItem != null)
                                {
                                    HocSinh hocSinh = selectedItem as HocSinh;
                                    if (hocSinh != null)
                                    {
                                        // Tìm học sinh trong danh sách mới
                                        var danhSachHocSinh = BLL.HocSinhBLL.GetDanhSachHocSinh();
                                        var hocSinhMoi = danhSachHocSinh.FirstOrDefault(hs => hs.MaHS == hocSinh.MaHS);
                                        if (hocSinhMoi != null)
                                        {
                                            comboBox.SelectedItem = hocSinhMoi;

                                            // Cập nhật thông tin học sinh
                                            UpdateHocSinhInfo(hocSinhGrid, hocSinhMoi);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Đã làm mới dữ liệu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_TimKiemHocSinh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mở cửa sổ tìm kiếm học sinh
                TimKiemHocSinhWindow timKiemWindow = new TimKiemHocSinhWindow();
                timKiemWindow.Owner = Window.GetWindow(this);

                // Hiển thị cửa sổ tìm kiếm
                bool? result = timKiemWindow.ShowDialog();
                if (result == true && timKiemWindow.HocSinhDuocChon != null)
                {
                    // Debug: Hiển thị thông tin học sinh được chọn
                    MessageBox.Show($"Đã chọn học sinh: {timKiemWindow.HocSinhDuocChon.MaHS} - {timKiemWindow.HocSinhDuocChon.HoTen}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Tìm ComboBox trống đầu tiên để thêm học sinh
                    bool daThemHocSinh = false;

                    foreach (UIElement element in sp_DanhSachHocSinh.Children)
                    {
                        if (element is Grid hocSinhGrid)
                        {
                            // Tìm ComboBox trong grid
                            ComboBox comboBox = null;
                            foreach (UIElement control in hocSinhGrid.Children)
                            {
                                if (control is ComboBox cb)
                                {
                                    comboBox = cb;
                                    break;
                                }
                            }

                            if (comboBox != null && comboBox.SelectedItem == null)
                            {
                                // Tạo danh sách học sinh nếu chưa có
                                if (comboBox.ItemsSource == null)
                                {
                                    comboBox.ItemsSource = BLL.HocSinhBLL.GetDanhSachHocSinh();
                                    comboBox.DisplayMemberPath = "HoTen";
                                    comboBox.SelectedValuePath = "MaHS";
                                }

                                // Chọn học sinh
                                comboBox.SelectedItem = timKiemWindow.HocSinhDuocChon;

                                // Cập nhật các TextBox thông tin học sinh
                                UpdateHocSinhInfo(hocSinhGrid, timKiemWindow.HocSinhDuocChon);

                                daThemHocSinh = true;
                                break;
                            }
                        }
                    }

                    if (daThemHocSinh)
                    {
                        MessageBox.Show($"Đã thêm học sinh: {timKiemWindow.HocSinhDuocChon.HoTen} vào danh sách", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Danh sách đã đầy, không thể thêm học sinh mới", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Phương thức cập nhật thông tin học sinh vào các TextBox
        private void UpdateHocSinhInfo(Grid hocSinhGrid, HocSinh hocSinh)
        {
            foreach (UIElement control in hocSinhGrid.Children)
            {
                if (control is TextBox textBox)
                {
                    // Xác định vị trí cột của TextBox
                    int column = Grid.GetColumn(textBox);

                    // Cập nhật thông tin tương ứng
                    switch (column)
                    {
                        case 2: // Giới tính
                            textBox.Text = hocSinh.GioiTinh;
                            break;
                        case 3: // Ngày sinh
                            textBox.Text = hocSinh.NgaySinh.ToShortDateString();
                            break;
                        case 4: // Địa chỉ
                            textBox.Text = hocSinh.DiaChi;
                            break;
                    }
                }
            }
        }

        private void btn_XoaDanhSachLop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbx_Lop.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp trước khi xóa danh sách", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Xác nhận xóa
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa danh sách học sinh của lớp này?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Xóa danh sách học sinh
                    foreach (UIElement element in sp_DanhSachHocSinh.Children)
                    {
                        if (element is Grid hocSinhGrid)
                        {
                            // Tìm ComboBox và TextBox trong grid
                            foreach (UIElement control in hocSinhGrid.Children)
                            {
                                if (control is ComboBox comboBox)
                                {
                                    // Xóa lựa chọn
                                    comboBox.SelectedItem = null;
                                }
                                else if (control is TextBox textBox && Grid.GetColumn(control) > 1) // Bỏ qua TextBlock STT
                                {
                                    // Xóa nội dung
                                    textBox.Text = string.Empty;
                                }
                            }
                        }
                    }

                    MessageBox.Show("Đã xóa danh sách học sinh của lớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_Thoat_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ hiện tại
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }


}
