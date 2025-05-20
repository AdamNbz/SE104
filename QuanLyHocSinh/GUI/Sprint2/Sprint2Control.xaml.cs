using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DAL;

namespace GUI.Sprint2
{
    /// <summary>
    /// Interaction logic for Sprint2Control.xaml
    /// </summary>
    public partial class Sprint2Control : UserControl
    {
        // Khai báo các biến cần thiết
        private ObservableCollection<HocSinhLopViewModel> danhSachHocSinhLop;
        private List<Lop> danhSachLop;
        private List<HocSinh> danhSachHocSinh;

        public Sprint2Control()
        {
            InitializeComponent();
            danhSachHocSinhLop = new ObservableCollection<HocSinhLopViewModel>(); 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Khởi tạo dữ liệu khi control được load
            LoadDanhSachLop();
            LoadDanhSachHocSinh();
        }

        private void LoadDanhSachLop()
        {
            try
            {
                // Lấy danh sách lớp từ BLL
                danhSachLop = BLL.LopBLL.GetDanhSachLop();

                // Cập nhật ComboBox
                cbx_Lop.ItemsSource = danhSachLop;
                cbx_Lop.DisplayMemberPath = "TenLop";
                cbx_Lop.SelectedValuePath = "MaLop";

                // Chọn lớp đầu tiên nếu có
                if (danhSachLop.Count > 0)
                {
                    cbx_Lop.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDanhSachHocSinh()
        {
            try
            {
                // Lấy danh sách học sinh chưa có lớp từ BLL
                danhSachHocSinh = BLL.HocSinhBLL.LayDanhSachHocSinhChuaPhanLop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    // Cập nhật thông tin lớp
                    txb_Khoi.Text = lopDuocChon.MaKhoi ?? "";

                    // Tính sĩ số lớp
                    List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocsinh(lopDuocChon.MaLop);

                    // Nếu danh sách học sinh trong lớp là null, khởi tạo một danh sách trống
                    if (danhSachHocSinhTrongLop == null)
                    {
                        danhSachHocSinhTrongLop = new List<HocSinh>();
                    }

                    txb_SiSo.Text = danhSachHocSinhTrongLop.Count.ToString();

                    // Lấy sĩ số tối đa từ ThamSo
                    txb_SiSoToiDa.Text = LaySiSoToiDa().ToString();

                    // Tạo danh sách học sinh cho lớp
                    TaoDanhSachHocSinhLop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TaoDanhSachHocSinhLop()
        {
            try
            {
                danhSachHocSinhLop.Clear();

                // Đảm bảo danh sách học sinh không null
                if (danhSachHocSinh == null)
                {
                    danhSachHocSinh = new List<HocSinh>();
                }

                // Lấy sĩ số tối đa
                int siSoToiDa = LaySiSoToiDa();

                // Tạo danh sách học sinh cho lớp
                for (int i = 0; i < siSoToiDa; i++)
                {
                    danhSachHocSinhLop.Add(new HocSinhLopViewModel
                    {
                        STT = i + 1,
                        DanhSachHocSinh = danhSachHocSinh
                    });
                }

                lv_DanhSachHocSinh.ItemsSource = danhSachHocSinhLop;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int LaySiSoToiDa()
        {
            try
            {
                // Lấy sĩ số tối đa từ bảng ThamSo trong database
                var thamSo = DAL.DataContext.Context.THAMSO.FirstOrDefault();
                if (thamSo != null)
                {
                    return thamSo.SiSoToiDa;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy sĩ số tối đa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Giá trị mặc định nếu không lấy được từ database
            return 40;
        }

        private bool KiemTraDanhSachHopLe()
        {
            if (cbx_Lop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn lớp trước khi lập danh sách", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Kiểm tra xem có học sinh nào được chọn không
            bool coHocSinhDuocChon = false;
            foreach (var item in danhSachHocSinhLop)
            {
                if (item.HocSinhDuocChon != null)
                {
                    coHocSinhDuocChon = true;
                    break;
                }
            }

            if (!coHocSinhDuocChon)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một học sinh cho lớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Kiểm tra xem có học sinh nào bị trùng không
            var danhSachMaHS = new List<string>();
            foreach (var item in danhSachHocSinhLop)
            {
                if (item.HocSinhDuocChon != null)
                {
                    if (danhSachMaHS.Contains(item.HocSinhDuocChon.MaHS))
                    {
                        MessageBox.Show($"Học sinh {item.HocSinhDuocChon.HoTen} được chọn nhiều lần", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    danhSachMaHS.Add(item.HocSinhDuocChon.MaHS);
                }
            }

            return true;
        }

        private void CapNhatDanhSachLop()
        {
            if (cbx_Lop.SelectedItem == null) return;

            Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
            List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocsinh(lopDuocChon.MaLop);
            txb_SiSo.Text = danhSachHocSinhTrongLop.Count.ToString();

            // Cập nhật lại danh sách học sinh chưa phân lớp
            LoadDanhSachHocSinh();

            // Cập nhật lại danh sách học sinh trong lớp
            TaoDanhSachHocSinhLop();
        }

        private void btn_LapDanhSachLop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!KiemTraDanhSachHopLe()) return;

                Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                var danhSachMaHS = new List<string>();

                // Lấy danh sách mã học sinh được chọn
                foreach (var item in danhSachHocSinhLop)
                {
                    if (item.HocSinhDuocChon != null)
                    {
                        danhSachMaHS.Add(item.HocSinhDuocChon.MaHS);
                    }
                }

                // Kiểm tra sĩ số lớp sau khi thêm học sinh
                int siSoHienTai = int.Parse(txb_SiSo.Text);
                int siSoToiDa = LaySiSoToiDa();

                if (siSoHienTai + danhSachMaHS.Count > siSoToiDa)
                {
                    MessageBox.Show($"Sĩ số lớp vượt quá giới hạn cho phép ({siSoToiDa})", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Phân lớp cho các học sinh được chọn
                foreach (string maHS in danhSachMaHS)
                {
                    HocSinh hocSinh = BLL.HocSinhBLL.LayThongTinHocSinh(maHS);
                    if (hocSinh != null)
                    {
                        hocSinh.MaLop = lopDuocChon.MaLop;
                        BLL.HocSinhBLL.SuaHocSinh(hocSinh);
                    }
                }

                MessageBox.Show("Lập danh sách lớp thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Cập nhật lại giao diện
                CapNhatDanhSachLop();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lập danh sách lớp: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_LamMoi_Click(object sender, RoutedEventArgs e)
        {
            // Làm mới danh sách học sinh và thông tin lớp
            LoadDanhSachHocSinh();
            if (cbx_Lop.SelectedItem != null)
            {
                cbx_Lop_SelectionChanged(null, null);
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

                // Nếu người dùng chọn một học sinh, thêm vào danh sách
                if (result == true && timKiemWindow.HocSinhDuocChon != null)
                {
                    // Tìm vị trí trống đầu tiên trong danh sách
                    for (int i = 0; i < danhSachHocSinhLop.Count; i++)
                    {
                        if (danhSachHocSinhLop[i].HocSinhDuocChon == null)
                        {
                            // Kiểm tra xem học sinh đã có trong danh sách chưa
                            bool daTonTai = false;
                            foreach (var item in danhSachHocSinhLop)
                            {
                                if (item.HocSinhDuocChon?.MaHS == timKiemWindow.HocSinhDuocChon.MaHS)
                                {
                                    daTonTai = true;
                                    break;
                                }
                            }

                            if (daTonTai)
                            {
                                MessageBox.Show("Học sinh này đã được chọn trong danh sách", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }

                            // Thêm học sinh vào vị trí trống
                            danhSachHocSinhLop[i].HocSinhDuocChon = timKiemWindow.HocSinhDuocChon;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả học sinh khỏi lớp này?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Lop lopDuocChon = cbx_Lop.SelectedItem as Lop;
                    List<HocSinh> danhSachHocSinhTrongLop = BLL.LopBLL.LayDanhSachHocsinh(lopDuocChon.MaLop);

                    // Xóa tất cả học sinh khỏi lớp
                    foreach (HocSinh hocSinh in danhSachHocSinhTrongLop)
                    {
                        hocSinh.MaLop = null;
                        BLL.HocSinhBLL.SuaHocSinh(hocSinh);
                    }

                    MessageBox.Show("Đã xóa tất cả học sinh khỏi lớp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Cập nhật lại giao diện
                    CapNhatDanhSachLop();
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

    // ViewModel cho danh sách học sinh trong lớp
    public class HocSinhLopViewModel
    {
        public int STT { get; set; }
        public List<HocSinh> DanhSachHocSinh { get; set; }
        public HocSinh HocSinhDuocChon { get; set; }

        // Các thuộc tính để hiển thị thông tin học sinh
        public string GioiTinh => HocSinhDuocChon?.GioiTinh ?? "";
        public DateTime NgaySinh => HocSinhDuocChon?.NgaySinh ?? DateTime.Now;
        public string DiaChi => HocSinhDuocChon?.DiaChi ?? "";
    }
}
