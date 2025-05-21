using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

                    // Kiểm tra danh sách học sinh
                    if (danhSachHocSinh == null)
                    {
                        danhSachHocSinh = new List<HocSinh>();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    int siSoToiDa = 40; // Giá trị mặc định

                    if (thamSo != null)
                    {
                        siSoToiDa = thamSo.SiSoToiDa;
                        txb_SiSoToiDa.Text = siSoToiDa.ToString();
                    }
                    else
                    {
                        txb_SiSoToiDa.Text = siSoToiDa.ToString();
                    }

                    // Tạo động số lượng dòng học sinh dựa trên sĩ số tối đa
                    TaoDanhSachHocSinh(siSoToiDa);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy sĩ số tối đa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    txb_SiSoToiDa.Text = "40"; // Giá trị mặc định

                    // Tạo với giá trị mặc định
                    TaoDanhSachHocSinh(40);
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
                    HocSinh hocSinh = comboBox.SelectedItem as HocSinh;
                    if (hocSinh != null)
                    {
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
                }
                else if (sender is ComboBox editableComboBox && editableComboBox.IsEditable && !string.IsNullOrEmpty(editableComboBox.Text))
                {
                    // Xử lý trường hợp người dùng nhập text
                    string searchText = editableComboBox.Text.ToLower();

                    // Tìm học sinh phù hợp với text đã nhập
                    HocSinh hocSinhTimThay = danhSachHocSinh.FirstOrDefault(hs =>
                        hs.HoTen.ToLower().Contains(searchText));

                    if (hocSinhTimThay != null)
                    {
                        // Chọn học sinh tìm thấy
                        editableComboBox.SelectedItem = hocSinhTimThay;

                        // Tìm Grid cha của ComboBox
                        FrameworkElement parent = editableComboBox.Parent as FrameworkElement;
                        while (parent != null && !(parent is Grid))
                        {
                            parent = parent.Parent as FrameworkElement;
                        }

                        if (parent is Grid grid)
                        {
                            // Cập nhật thông tin học sinh
                            UpdateHocSinhInfo(grid, hocSinhTimThay);
                        }
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

                    // Lấy danh sách học sinh chưa có lớp
                    List<HocSinh> danhSachHocSinhChuaCoLop = danhSachHocSinh
                        .Where(hs => string.IsNullOrEmpty(hs.MaLop) || hs.MaLop == lopDuocChon.MaLop)
                        .ToList();

                    // Xóa tất cả các lựa chọn hiện tại
                    foreach (UIElement element in sp_DanhSachHocSinh.Children)
                    {
                        if (element is Grid hocSinhGrid)
                        {
                            foreach (UIElement control in hocSinhGrid.Children)
                            {
                                if (control is ComboBox comboBox)
                                {
                                    comboBox.SelectedItem = null;
                                    comboBox.ItemsSource = danhSachHocSinhChuaCoLop;
                                    comboBox.DisplayMemberPath = "HoTen";
                                    comboBox.SelectedValuePath = "MaHS";
                                    comboBox.IsEditable = true; // Cho phép nhập text
                                    comboBox.IsTextSearchEnabled = true; // Cho phép tìm kiếm theo text
                                    TextSearch.SetTextPath(comboBox, "HoTen"); // Tìm kiếm theo thuộc tính HoTen
                                    comboBox.StaysOpenOnEdit = true; // Giữ dropdown mở khi đang nhập
                                    comboBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(ComboBox_TextChanged));
                                }
                                else if (control is TextBox textBox && Grid.GetColumn(control) > 1)
                                {
                                    textBox.Text = string.Empty;
                                }
                            }
                        }
                    }

                    // Nếu có học sinh trong lớp, hiển thị theo thứ tự
                    if (danhSachHocSinhTrongLop.Count > 0)
                    {
                        int index = 0;
                        foreach (UIElement element in sp_DanhSachHocSinh.Children)
                        {
                            if (element is Grid hocSinhGrid && index < danhSachHocSinhTrongLop.Count)
                            {
                                foreach (UIElement control in hocSinhGrid.Children)
                                {
                                    if (control is ComboBox comboBox)
                                    {
                                        // Tìm học sinh trong danh sách tổng
                                        HocSinh hocSinh = danhSachHocSinhTrongLop[index];
                                        HocSinh hocSinhTrongDS = danhSachHocSinh.FirstOrDefault(hs => hs.MaHS == hocSinh.MaHS);

                                        if (hocSinhTrongDS != null)
                                        {
                                            comboBox.SelectedItem = hocSinhTrongDS;
                                            UpdateHocSinhInfo(hocSinhGrid, hocSinhTrongDS);
                                        }

                                        break;
                                    }
                                }
                                index++;
                            }
                        }
                    }
                }
                else
                {
                    // Xóa tất cả các lựa chọn nếu không có lớp được chọn
                    foreach (UIElement element in sp_DanhSachHocSinh.Children)
                    {
                        if (element is Grid hocSinhGrid)
                        {
                            foreach (UIElement control in hocSinhGrid.Children)
                            {
                                if (control is ComboBox comboBox)
                                {
                                    comboBox.SelectedItem = null;
                                }
                                else if (control is TextBox textBox && Grid.GetColumn(control) > 1)
                                {
                                    textBox.Text = string.Empty;
                                }
                            }
                        }
                    }
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
                HashSet<string> danhSachMaHSKiemTra = new HashSet<string>();
                bool coHocSinhTrung = false;
                string thongBaoTrung = "Có học sinh bị trùng lặp trong danh sách:\n";

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
                                    // Kiểm tra trùng lặp
                                    if (danhSachMaHSKiemTra.Contains(hocSinh.MaHS))
                                    {
                                        coHocSinhTrung = true;
                                        thongBaoTrung += $"- {hocSinh.HoTen} (Mã: {hocSinh.MaHS})\n";
                                    }
                                    else
                                    {
                                        danhSachMaHSKiemTra.Add(hocSinh.MaHS);
                                        danhSachHocSinhDaChon.Add(hocSinh);
                                        danhSachMaHS.Add(hocSinh.MaHS);
                                    }
                                }
                            }
                        }
                    }
                }

                // Kiểm tra nếu có học sinh trùng
                if (coHocSinhTrung)
                {
                    MessageBox.Show(thongBaoTrung, "Lỗi - Học sinh trùng lặp", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
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

                    // Cập nhật lại danh sách học sinh tổng
                    danhSachHocSinh = BLL.HocSinhBLL.GetDanhSachHocSinh();

                    // Cập nhật sĩ số lớp
                    txb_SiSo.Text = danhSachHocSinhDaChon.Count.ToString();

                    // Lấy danh sách học sinh chưa có lớp
                    List<HocSinh> danhSachHocSinhChuaCoLop = danhSachHocSinh
                        .Where(hs => string.IsNullOrEmpty(hs.MaLop) || hs.MaLop == lopDuocChon.MaLop)
                        .ToList();

                    // Hiển thị học sinh đã chọn trong danh sách
                    foreach (UIElement element in sp_DanhSachHocSinh.Children)
                    {
                        if (element is Grid hocSinhGrid)
                        {
                            // Xóa lựa chọn hiện tại
                            foreach (UIElement control in hocSinhGrid.Children)
                            {
                                if (control is ComboBox comboBox)
                                {
                                    comboBox.SelectedItem = null;
                                    comboBox.ItemsSource = danhSachHocSinhChuaCoLop;
                                    comboBox.DisplayMemberPath = "HoTen";
                                    comboBox.SelectedValuePath = "MaHS";
                                    comboBox.IsEditable = true; // Cho phép nhập text
                                    comboBox.IsTextSearchEnabled = true; // Cho phép tìm kiếm theo text
                                    TextSearch.SetTextPath(comboBox, "HoTen"); // Tìm kiếm theo thuộc tính HoTen
                                    comboBox.StaysOpenOnEdit = true; // Giữ dropdown mở khi đang nhập
                                    comboBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(ComboBox_TextChanged));
                                }
                                else if (control is TextBox textBox && Grid.GetColumn(control) > 1)
                                {
                                    textBox.Text = string.Empty;
                                }
                            }
                        }
                    }

                    // Hiển thị học sinh đã chọn
                    int index = 0;
                    foreach (UIElement element in sp_DanhSachHocSinh.Children)
                    {
                        if (element is Grid hocSinhGrid && index < danhSachHocSinhDaChon.Count)
                        {
                            foreach (UIElement control in hocSinhGrid.Children)
                            {
                                if (control is ComboBox comboBox)
                                {
                                    // Tìm học sinh trong danh sách tổng
                                    HocSinh hocSinh = danhSachHocSinhDaChon[index];
                                    HocSinh hocSinhTrongDS = danhSachHocSinh.FirstOrDefault(hs => hs.MaHS == hocSinh.MaHS);

                                    if (hocSinhTrongDS != null)
                                    {
                                        comboBox.SelectedItem = hocSinhTrongDS;
                                        UpdateHocSinhInfo(hocSinhGrid, hocSinhTrongDS);
                                    }

                                    break;
                                }
                            }
                            index++;
                        }
                    }

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
                // Cập nhật lại danh sách học sinh
                danhSachHocSinh = BLL.HocSinhBLL.GetDanhSachHocSinh();

                if (cbx_Lop.SelectedItem != null)
                {
                    Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                    if (lopDuocChon != null)
                    {
                        // Kiểm tra xem lớp đã có học sinh chưa
                        List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocsinh(lopDuocChon.MaLop);

                        if (danhSachHocSinhTrongLop != null && danhSachHocSinhTrongLop.Count > 0)
                        {
                            // Nếu lớp đã có học sinh, hiển thị lại danh sách học sinh của lớp
                            cbx_Lop_SelectionChanged(null, null);
                            MessageBox.Show("Đã làm mới dữ liệu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                }

                // Cập nhật lại danh sách lớp
                danhSachLop = BLL.LopBLL.GetDanhSachLop();
                cbx_Lop.ItemsSource = danhSachLop;

                // Lấy danh sách học sinh chưa có lớp
                List<HocSinh> danhSachHocSinhChuaCoLop = danhSachHocSinh;
                if (cbx_Lop.SelectedItem != null)
                {
                    Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                    if (lopDuocChon != null)
                    {
                        danhSachHocSinhChuaCoLop = danhSachHocSinh
                            .Where(hs => string.IsNullOrEmpty(hs.MaLop) || hs.MaLop == lopDuocChon.MaLop)
                            .ToList();
                    }
                }

                // Cập nhật lại ItemsSource của các ComboBox
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
                                comboBox.ItemsSource = danhSachHocSinhChuaCoLop;
                                comboBox.DisplayMemberPath = "HoTen";
                                comboBox.SelectedValuePath = "MaHS";
                                comboBox.IsEditable = true; // Cho phép nhập text
                                comboBox.IsTextSearchEnabled = true; // Cho phép tìm kiếm theo text
                                TextSearch.SetTextPath(comboBox, "HoTen"); // Tìm kiếm theo thuộc tính HoTen
                                comboBox.StaysOpenOnEdit = true; // Giữ dropdown mở khi đang nhập
                                comboBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(ComboBox_TextChanged));

                                // Khôi phục lại item đã chọn
                                if (selectedItem != null)
                                {
                                    HocSinh hocSinh = selectedItem as HocSinh;
                                    if (hocSinh != null)
                                    {
                                        // Tìm học sinh trong danh sách mới
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

                // Cập nhật lại thông tin lớp
                if (cbx_Lop.SelectedItem != null)
                {
                    cbx_Lop_SelectionChanged(null, null);
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
                                    comboBox.IsEditable = true; // Cho phép nhập text
                                    comboBox.IsTextSearchEnabled = true; // Cho phép tìm kiếm theo text
                                    TextSearch.SetTextPath(comboBox, "HoTen"); // Tìm kiếm theo thuộc tính HoTen
                                    comboBox.StaysOpenOnEdit = true; // Giữ dropdown mở khi đang nhập
                                    comboBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(ComboBox_TextChanged));
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

                    // Đặt màu nền cho TextBox
                    textBox.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)); // Màu nền #CCCCCC

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
                    // Lấy danh sách học sinh của lớp
                    Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                    if (lopDuocChon != null)
                    {
                        List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocsinh(lopDuocChon.MaLop);

                        // Xóa học sinh khỏi lớp trong cơ sở dữ liệu
                        foreach (var hocSinh in danhSachHocSinhTrongLop)
                        {
                            // Tạo bản sao của học sinh để cập nhật
                            HocSinh hocSinhCapNhat = new HocSinh
                            {
                                MaHS = hocSinh.MaHS,
                                HoTen = hocSinh.HoTen,
                                GioiTinh = hocSinh.GioiTinh,
                                NgaySinh = hocSinh.NgaySinh,
                                DiaChi = hocSinh.DiaChi,
                                Email = hocSinh.Email,
                                MaLop = null // Xóa mã lớp
                            };

                            // Cập nhật học sinh
                            BLL.HocSinhBLL.SuaHocSinh(hocSinhCapNhat);
                        }

                        // Cập nhật sĩ số lớp
                        txb_SiSo.Text = "0";
                    }

                    // Xóa danh sách học sinh trên giao diện
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

                    // Cập nhật lại danh sách học sinh
                    cbx_Lop_SelectionChanged(null, null);

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

        // Phương thức tạo động danh sách học sinh dựa trên sĩ số tối đa
        private void TaoDanhSachHocSinh(int siSoToiDa)
        {
            // Xóa tất cả các dòng hiện tại
            sp_DanhSachHocSinh.Children.Clear();

            // Tạo mới các dòng dựa trên sĩ số tối đa
            for (int i = 0; i < siSoToiDa; i++)
            {
                // Tạo Grid cho mỗi dòng
                Grid hocSinhGrid = new Grid();
                hocSinhGrid.Margin = new Thickness(0, 4, 0, 4);

                // Tạo các cột
                hocSinhGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                hocSinhGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                hocSinhGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
                hocSinhGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                hocSinhGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                // Tạo TextBlock STT
                TextBlock txtSTT = new TextBlock();
                txtSTT.Text = (i + 1).ToString();
                txtSTT.Margin = new Thickness(8, 8, 8, 8);
                txtSTT.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(txtSTT, 0);
                hocSinhGrid.Children.Add(txtSTT);

                // Lấy danh sách học sinh chưa có lớp
                List<HocSinh> danhSachHocSinhChuaCoLop = danhSachHocSinh;
                if (cbx_Lop.SelectedItem != null)
                {
                    Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                    if (lopDuocChon != null)
                    {
                        danhSachHocSinhChuaCoLop = danhSachHocSinh
                            .Where(hs => string.IsNullOrEmpty(hs.MaLop) || hs.MaLop == lopDuocChon.MaLop)
                            .ToList();
                    }
                }

                // Tạo ComboBox Họ tên
                ComboBox cbxHoTen = new ComboBox();
                cbxHoTen.Margin = new Thickness(4, 4, 4, 4);
                cbxHoTen.Style = Application.Current.Resources["MaterialDesignOutlinedComboBox"] as Style;
                cbxHoTen.ItemsSource = danhSachHocSinhChuaCoLop;
                cbxHoTen.DisplayMemberPath = "HoTen";
                cbxHoTen.SelectedValuePath = "MaHS";
                cbxHoTen.IsEditable = true; // Cho phép nhập text
                cbxHoTen.IsTextSearchEnabled = true; // Cho phép tìm kiếm theo text
                TextSearch.SetTextPath(cbxHoTen, "HoTen"); // Tìm kiếm theo thuộc tính HoTen
                cbxHoTen.StaysOpenOnEdit = true; // Giữ dropdown mở khi đang nhập
                cbxHoTen.SelectionChanged += HocSinh_ComboBox_SelectionChanged;
                cbxHoTen.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(ComboBox_TextChanged));
                Grid.SetColumn(cbxHoTen, 1);
                hocSinhGrid.Children.Add(cbxHoTen);

                // Tạo TextBox Giới tính
                TextBox txtGioiTinh = new TextBox();
                txtGioiTinh.Margin = new Thickness(4, 4, 4, 4);
                txtGioiTinh.IsReadOnly = true;
                txtGioiTinh.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)); // Màu nền #CCCCCC
                txtGioiTinh.Style = Application.Current.Resources["MaterialDesignOutlinedTextBox"] as Style;
                Grid.SetColumn(txtGioiTinh, 2);
                hocSinhGrid.Children.Add(txtGioiTinh);

                // Tạo TextBox Ngày sinh
                TextBox txtNgaySinh = new TextBox();
                txtNgaySinh.Margin = new Thickness(4, 4, 4, 4);
                txtNgaySinh.IsReadOnly = true;
                txtNgaySinh.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)); // Màu nền #CCCCCC
                txtNgaySinh.Style = Application.Current.Resources["MaterialDesignOutlinedTextBox"] as Style;
                Grid.SetColumn(txtNgaySinh, 3);
                hocSinhGrid.Children.Add(txtNgaySinh);

                // Tạo TextBox Địa chỉ
                TextBox txtDiaChi = new TextBox();
                txtDiaChi.Margin = new Thickness(4, 4, 4, 4);
                txtDiaChi.IsReadOnly = true;
                txtDiaChi.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)); // Màu nền #CCCCCC
                txtDiaChi.Style = Application.Current.Resources["MaterialDesignOutlinedTextBox"] as Style;
                Grid.SetColumn(txtDiaChi, 4);
                hocSinhGrid.Children.Add(txtDiaChi);

                // Thêm Grid vào StackPanel
                sp_DanhSachHocSinh.Children.Add(hocSinhGrid);
            }
        }

        // Phương thức xử lý sự kiện khi người dùng nhập text vào ComboBox
        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ComboBox comboBox = sender as ComboBox;
                if (comboBox != null && comboBox.IsEditable && comboBox.IsDropDownOpen)
                {
                    // Lấy TextBox bên trong ComboBox
                    TextBox textBox = e.OriginalSource as TextBox;
                    if (textBox != null)
                    {
                        string searchText = textBox.Text.ToLower();

                        // Lấy danh sách học sinh hiện tại của ComboBox
                        var currentList = comboBox.ItemsSource as List<HocSinh>;
                        if (currentList == null)
                        {
                            // Nếu không có danh sách, sử dụng danh sách học sinh chưa có lớp
                            if (cbx_Lop.SelectedItem != null)
                            {
                                Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                                if (lopDuocChon != null)
                                {
                                    currentList = danhSachHocSinh
                                        .Where(hs => string.IsNullOrEmpty(hs.MaLop) || hs.MaLop == lopDuocChon.MaLop)
                                        .ToList();
                                }
                                else
                                {
                                    currentList = danhSachHocSinh;
                                }
                            }
                            else
                            {
                                currentList = danhSachHocSinh;
                            }
                        }

                        if (!string.IsNullOrEmpty(searchText))
                        {
                            // Lọc danh sách học sinh dựa trên text đã nhập
                            List<HocSinh> filteredList = currentList
                                .Where(hs => hs.HoTen.ToLower().Contains(searchText))
                                .ToList();

                            // Cập nhật ItemsSource của ComboBox
                            comboBox.ItemsSource = filteredList;
                            comboBox.IsDropDownOpen = true;
                        }
                        else
                        {
                            // Nếu không có text, hiển thị lại toàn bộ danh sách
                            comboBox.ItemsSource = currentList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
