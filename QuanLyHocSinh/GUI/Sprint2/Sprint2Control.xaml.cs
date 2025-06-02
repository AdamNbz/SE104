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

                            // Cập nhật danh sách học sinh có sẵn cho tất cả ComboBox khác
                            UpdateAvailableStudentsForAllComboBoxes();
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

                            // Cập nhật danh sách học sinh có sẵn cho tất cả ComboBox khác
                            UpdateAvailableStudentsForAllComboBoxes();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Phương thức cập nhật danh sách học sinh có sẵn cho tất cả ComboBox
        private void UpdateAvailableStudentsForAllComboBoxes()
        {
            try
            {
                // Lấy danh sách học sinh đã được chọn (từ cả ComboBox và TextBox)
                HashSet<string> selectedStudentIds = new HashSet<string>();

                foreach (UIElement element in sp_DanhSachHocSinh.Children)
                {
                    if (element is Grid hocSinhGrid)
                    {
                        foreach (UIElement control in hocSinhGrid.Children)
                        {
                            if (control is ComboBox comboBox && Grid.GetColumn(comboBox) == 1)
                            {
                                if (comboBox.SelectedItem is HocSinh selectedStudent)
                                {
                                    selectedStudentIds.Add(selectedStudent.MaHS);
                                }
                                // Nếu ComboBox bị readonly (đã nhận vào lớp), cũng coi như đã chọn học sinh
                                else if (comboBox.IsReadOnly && !string.IsNullOrEmpty(comboBox.Text))
                                {
                                    HocSinh hocSinhFromDisabledComboBox = danhSachHocSinh.FirstOrDefault(hs => hs.HoTen == comboBox.Text);
                                    if (hocSinhFromDisabledComboBox != null)
                                    {
                                        selectedStudentIds.Add(hocSinhFromDisabledComboBox.MaHS);
                                    }
                                }
                            }
                            else if (control is TextBox textBox && Grid.GetColumn(textBox) == 1 && !string.IsNullOrEmpty(textBox.Text))
                            {
                                // Tìm học sinh từ tên trong TextBox (đã được tiếp nhận)
                                HocSinh hocSinhFromTextBox = danhSachHocSinh.FirstOrDefault(hs => hs.HoTen == textBox.Text);
                                if (hocSinhFromTextBox != null)
                                {
                                    selectedStudentIds.Add(hocSinhFromTextBox.MaHS);
                                }
                            }
                        }
                    }
                }

                // Lấy danh sách học sinh gốc (chưa có lớp hoặc thuộc lớp hiện tại)
                List<HocSinh> baseStudentList;
                if (cbx_Lop.SelectedItem is Lop selectedClass)
                {
                    baseStudentList = danhSachHocSinh
                        .Where(hs => string.IsNullOrEmpty(hs.MaLop) || hs.MaLop == selectedClass.MaLop)
                        .ToList();
                }
                else
                {
                    baseStudentList = danhSachHocSinh.Where(hs => string.IsNullOrEmpty(hs.MaLop)).ToList();
                }

                // Cập nhật ItemsSource cho từng ComboBox (chỉ những ô enabled và chưa thành TextBox)
                foreach (UIElement element in sp_DanhSachHocSinh.Children)
                {
                    if (element is Grid hocSinhGrid)
                    {
                        foreach (UIElement control in hocSinhGrid.Children)
                        {
                            if (control is ComboBox comboBox && Grid.GetColumn(comboBox) == 1 && !comboBox.IsReadOnly)
                            {
                                // Lưu lại học sinh hiện tại được chọn
                                HocSinh currentSelected = comboBox.SelectedItem as HocSinh;

                                // Tạo danh sách học sinh có sẵn cho ComboBox này
                                List<HocSinh> availableStudents = baseStudentList
                                    .Where(hs => !selectedStudentIds.Contains(hs.MaHS) ||
                                                (currentSelected != null && hs.MaHS == currentSelected.MaHS))
                                    .ToList();

                                // Cập nhật ItemsSource
                                comboBox.ItemsSource = availableStudents;

                                // Giữ lại selection hiện tại nếu có
                                if (currentSelected != null)
                                {
                                    comboBox.SelectedItem = availableStudents.FirstOrDefault(hs => hs.MaHS == currentSelected.MaHS);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocSinh(lopDuocChon.MaLop);

                    // Nếu danh sách học sinh trong lớp là null, khởi tạo một danh sách trống
                    if (danhSachHocSinhTrongLop == null)
                    {
                        danhSachHocSinhTrongLop = new List<HocSinh>();
                    }

                    txb_SiSo.Text = danhSachHocSinhTrongLop.Count.ToString();

                    // Lấy danh sách học sinh chưa có lớp hoặc thuộc lớp hiện tại
                    List<HocSinh> danhSachHocSinhChuaCoLop = danhSachHocSinh
                        .Where(hs => string.IsNullOrEmpty(hs.MaLop) || hs.MaLop == lopDuocChon.MaLop)
                        .ToList();

                    // Reset toàn bộ danh sách học sinh - xóa và tạo lại tất cả các hàng
                    sp_DanhSachHocSinh.Children.Clear();

                    // Tạo lại danh sách với sĩ số tối đa từ tham số
                    int siSoToiDa = 40;
                    try
                    {
                        var thamSo = DAL.DataContext.Context.THAMSO.FirstOrDefault();
                        if (thamSo != null)
                        {
                            siSoToiDa = thamSo.SiSoToiDa;
                        }
                    }
                    catch { }

                    // Tạo lại toàn bộ danh sách học sinh
                    TaoDanhSachHocSinh(siSoToiDa);

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
                                    if (control is ComboBox comboBox && Grid.GetColumn(control) == 1)
                                    {
                                        // Tìm học sinh trong danh sách tổng
                                        HocSinh hocSinh = danhSachHocSinhTrongLop[index];
                                        HocSinh hocSinhTrongDS = danhSachHocSinh.FirstOrDefault(hs => hs.MaHS == hocSinh.MaHS);

                                        if (hocSinhTrongDS != null)
                                        {
                                            // Disable ComboBox thay vì thay thế bằng TextBox
                                            DisableComboBox(comboBox, hocSinhTrongDS.HoTen);
                                            UpdateHocSinhInfo(hocSinhGrid, hocSinhTrongDS);
                                        }
                                        break;
                                    }
                                }
                                index++;
                            }
                        }
                    }

                    // Cập nhật danh sách học sinh có sẵn cho tất cả ComboBox
                    UpdateAvailableStudentsForAllComboBoxes();
                }
                else
                {
                    // Xóa tất cả các lựa chọn nếu không có lớp được chọn
                    txb_Khoi.Text = "";
                    txb_SiSo.Text = "0";
                    sp_DanhSachHocSinh.Children.Clear();

                    // Tạo lại danh sách trống
                    int siSoToiDa = int.TryParse(txb_SiSoToiDa.Text, out int result) ? result : 40;
                    TaoDanhSachHocSinh(siSoToiDa);
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

                    // Lấy danh sách học sinh trong lớp sau khi lập (bao gồm cả học sinh mới và cũ)
                    List<HocSinh> danhSachHocSinhTrongLopMoi = BLL.LopBLL.LayDanhSachHocSinh(lopDuocChon.MaLop);
                    if (danhSachHocSinhTrongLopMoi == null)
                    {
                        danhSachHocSinhTrongLopMoi = new List<HocSinh>();
                    }

                    // Cập nhật sĩ số lớp
                    txb_SiSo.Text = danhSachHocSinhTrongLopMoi.Count.ToString();

                    // Cập nhật giao diện sau khi lập danh sách
                    UpdateUIAfterCreatingClassList(lopDuocChon, danhSachHocSinhTrongLopMoi);

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
                        List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocSinh(lopDuocChon.MaLop);

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
                                // Enable lại ComboBox nếu bị disabled
                                EnableComboBox(comboBox);

                                // Thiết lập lại ItemsSource như cũ
                                comboBox.ItemsSource = danhSachHocSinhChuaCoLop;
                                comboBox.DisplayMemberPath = "HoTen";
                                comboBox.SelectedValuePath = "MaHS";
                                comboBox.IsEditable = true;
                                comboBox.IsTextSearchEnabled = true;
                                TextSearch.SetTextPath(comboBox, "HoTen");
                                comboBox.StaysOpenOnEdit = true;
                                comboBox.AddHandler(TextBoxBase.TextChangedEvent,
                                                   new TextChangedEventHandler(ComboBox_TextChanged));
                            }
                            else if (control is TextBox textBox)
                            {
                                int column = Grid.GetColumn(textBox);
                                if (column == 1) // TextBox họ tên (đã thay thế ComboBox)
                                {
                                    // Thay thế TextBox trở lại thành ComboBox
                                    ReplaceTextBoxWithComboBox(hocSinhGrid);
                                }
                                else if (column > 1) // Các TextBox khác (Giới tính, Ngày sinh, Địa chỉ)
                                {
                                    // Xóa sạch các TextBox (Giới tính, Ngày sinh, Địa chỉ…)
                                    textBox.Text = string.Empty;
                                    // Nếu muốn reset cả màu nền:
                                    textBox.Background = Brushes.LightGray;
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

                                // Cập nhật danh sách học sinh có sẵn cho tất cả ComboBox khác
                                UpdateAvailableStudentsForAllComboBoxes();

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

        // Phương thức disable ComboBox sau khi học sinh được nhận vào lớp
        private void DisableComboBox(ComboBox comboBox, string hoTen)
        {
            if (comboBox != null)
            {
                // Set text và làm readonly thay vì disable
                comboBox.Text = hoTen;
                comboBox.SelectedItem = danhSachHocSinh.FirstOrDefault(hs => hs.HoTen == hoTen);
                comboBox.IsReadOnly = true; // Readonly thay vì disable
                comboBox.IsHitTestVisible = false; // Không thể click dropdown
                comboBox.Focusable = false; // Không thể focus
                comboBox.Background = Brushes.White; // Nền trắng
                comboBox.Foreground = Brushes.Black; // Chữ đen bình thường
                comboBox.BorderBrush = SystemColors.ControlDarkBrush; // Border giống các ô khác
            }
        }

        // Phương thức enable lại ComboBox
        private void EnableComboBox(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                comboBox.IsEnabled = true;
                comboBox.IsReadOnly = false;
                comboBox.IsHitTestVisible = true;
                comboBox.Focusable = true;
                comboBox.Background = Brushes.White;
                comboBox.Foreground = Brushes.Black;
                comboBox.Text = string.Empty;
                comboBox.SelectedItem = null;
            }
        }

        // Phương thức thay thế ComboBox bằng TextBox readonly (giữ lại cho tương thích)
        private void ReplaceComboBoxWithTextBox(Grid hocSinhGrid, string hoTen)
        {
            // Tìm ComboBox trong grid
            ComboBox comboBoxToRemove = null;
            foreach (UIElement control in hocSinhGrid.Children)
            {
                if (control is ComboBox comboBox && Grid.GetColumn(comboBox) == 1)
                {
                    comboBoxToRemove = comboBox;
                    break;
                }
            }

            if (comboBoxToRemove != null)
            {
                // Xóa ComboBox khỏi grid
                hocSinhGrid.Children.Remove(comboBoxToRemove);

                // Tạo TextBox readonly thay thế
                TextBox txtHoTen = new TextBox();
                txtHoTen.Text = hoTen;
                txtHoTen.Margin = new Thickness(4, 4, 4, 4);
                txtHoTen.IsReadOnly = true;
                txtHoTen.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)); // Màu nền #CCCCCC giống các TextBox khác
                txtHoTen.Style = Application.Current.Resources["MaterialDesignOutlinedTextBox"] as Style;
                Grid.SetColumn(txtHoTen, 1);
                hocSinhGrid.Children.Add(txtHoTen);
            }
        }

        // Phương thức thay thế TextBox trở lại thành ComboBox
        private void ReplaceTextBoxWithComboBox(Grid hocSinhGrid)
        {
            // Tìm TextBox họ tên trong grid
            TextBox textBoxToRemove = null;
            foreach (UIElement control in hocSinhGrid.Children)
            {
                if (control is TextBox textBox && Grid.GetColumn(textBox) == 1)
                {
                    textBoxToRemove = textBox;
                    break;
                }
            }

            if (textBoxToRemove != null)
            {
                // Xóa TextBox khỏi grid
                hocSinhGrid.Children.Remove(textBoxToRemove);

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

                // Tạo ComboBox thay thế
                ComboBox cbxHoTen = new ComboBox();
                cbxHoTen.Margin = new Thickness(4, 4, 4, 4);
                cbxHoTen.Style = Application.Current.Resources["MaterialDesignOutlinedComboBox"] as Style;
                cbxHoTen.ItemsSource = danhSachHocSinhChuaCoLop;
                cbxHoTen.DisplayMemberPath = "HoTen";
                cbxHoTen.SelectedValuePath = "MaHS";
                cbxHoTen.IsEditable = true;
                cbxHoTen.IsTextSearchEnabled = true;
                TextSearch.SetTextPath(cbxHoTen, "HoTen");
                cbxHoTen.StaysOpenOnEdit = true;
                cbxHoTen.SelectionChanged += HocSinh_ComboBox_SelectionChanged;
                cbxHoTen.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(ComboBox_TextChanged));
                Grid.SetColumn(cbxHoTen, 1);
                hocSinhGrid.Children.Add(cbxHoTen);
            }
        }

        // Phương thức cập nhật giao diện sau khi lập danh sách lớp
        private void UpdateUIAfterCreatingClassList(Lop lopDuocChon, List<HocSinh> danhSachHocSinhTrongLop)
        {
            // Lấy danh sách học sinh chưa có lớp để cập nhật cho ComboBox còn lại
            List<HocSinh> danhSachHocSinhChuaCoLop = danhSachHocSinh
                .Where(hs => string.IsNullOrEmpty(hs.MaLop) || hs.MaLop == lopDuocChon.MaLop)
                .ToList();

            // Cập nhật ItemsSource cho các ComboBox còn lại
            foreach (UIElement element in sp_DanhSachHocSinh.Children)
            {
                if (element is Grid hocSinhGrid)
                {
                    foreach (UIElement control in hocSinhGrid.Children)
                    {
                        if (control is ComboBox comboBox && Grid.GetColumn(comboBox) == 1)
                        {
                            comboBox.ItemsSource = danhSachHocSinhChuaCoLop;
                        }
                    }
                }
            }

            // Hiển thị tất cả học sinh trong lớp và chuyển thành TextBox readonly
            int index = 0;
            foreach (UIElement element in sp_DanhSachHocSinh.Children)
            {
                if (element is Grid hocSinhGrid && index < danhSachHocSinhTrongLop.Count)
                {
                    // Tìm học sinh trong danh sách tổng
                    HocSinh hocSinh = danhSachHocSinhTrongLop[index];
                    HocSinh hocSinhTrongDS = danhSachHocSinh.FirstOrDefault(hs => hs.MaHS == hocSinh.MaHS);

                    if (hocSinhTrongDS != null)
                    {
                        // Tìm ComboBox trong grid và disable nó
                        foreach (UIElement control in hocSinhGrid.Children)
                        {
                            if (control is ComboBox comboBox && Grid.GetColumn(comboBox) == 1)
                            {
                                DisableComboBox(comboBox, hocSinhTrongDS.HoTen);
                                break;
                            }
                            else if (control is TextBox textBox && Grid.GetColumn(textBox) == 1)
                            {
                                // Nếu đã là TextBox, cập nhật lại thông tin
                                textBox.Text = hocSinhTrongDS.HoTen;
                                break;
                            }
                        }

                        // Cập nhật thông tin học sinh vào các ô bên phải
                        UpdateHocSinhInfo(hocSinhGrid, hocSinhTrongDS);
                    }

                    index++;
                }
            }

            // Cập nhật danh sách học sinh có sẵn cho tất cả ComboBox còn lại
            UpdateAvailableStudentsForAllComboBoxes();
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
                        case 1: // Họ tên (khi đã thay thế ComboBox bằng TextBox)
                            textBox.Text = hocSinh.HoTen;
                            // Không cần set màu nền vì đã được set trong ReplaceComboBoxWithTextBox
                            break;
                        case 2: // Giới tính
                            textBox.Text = hocSinh.GioiTinh;
                            textBox.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)); // Màu nền #CCCCCC
                            break;
                        case 3: // Ngày sinh
                            textBox.Text = hocSinh.NgaySinh.ToShortDateString();
                            textBox.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)); // Màu nền #CCCCCC
                            break;
                        case 4: // Địa chỉ
                            textBox.Text = hocSinh.DiaChi;
                            textBox.Background = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)); // Màu nền #CCCCCC
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
                        List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocSinh(lopDuocChon.MaLop);

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

                    // Cập nhật lại danh sách học sinh tổng
                    danhSachHocSinh = BLL.HocSinhBLL.GetDanhSachHocSinh();

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
                                    // Enable lại ComboBox nếu bị disabled
                                    EnableComboBox(comboBox);
                                }
                                else if (control is TextBox textBox)
                                {
                                    int column = Grid.GetColumn(textBox);
                                    if (column == 1) // TextBox họ tên (đã thay thế ComboBox)
                                    {
                                        // Thay thế TextBox trở lại thành ComboBox
                                        ReplaceTextBoxWithComboBox(hocSinhGrid);
                                    }
                                    else if (column > 1) // Các TextBox khác (Giới tính, Ngày sinh, Địa chỉ)
                                    {
                                        // Xóa nội dung và reset màu nền
                                        textBox.Text = string.Empty;
                                        textBox.Background = Brushes.LightGray;
                                    }
                                }
                            }
                        }
                    }

                    // Cập nhật lại danh sách học sinh
                    cbx_Lop_SelectionChanged(null, null);

                    // Cập nhật danh sách học sinh có sẵn cho tất cả ComboBox
                    UpdateAvailableStudentsForAllComboBoxes();

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
