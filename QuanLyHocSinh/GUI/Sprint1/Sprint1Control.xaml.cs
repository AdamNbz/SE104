using DTO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Sprint1
{
    /// <summary>
    /// Interaction logic for Sprint1Control.xaml
    /// </summary>
    public partial class Sprint1Control : UserControl
    {
        public Sprint1Control()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize control data when loaded
            string maHS = BLL.HocSinhBLL.LayMaHocSinhTuDong();
            txBx_MaHS.Text = maHS;
            btn_TiepNhan.IsEnabled = true;
            stPn_ThongTin.IsEnabled = true;
            txBx_DiaChi.Clear();
            txBx_Email.Clear();
            txBx_GioiTinh.Clear();
            txBx_HoTen.Clear();
            dtPk_NgaySinh.SelectedDate = null;

            // Load tuổi từ quy định
            LoadTuoiToiThieu();
            LoadTuoiToiDa();

            // Clear tuổi học sinh
            txBx_TuoiHocSinh.Clear();
        }

        private void btn_TiepNhan_Click(object sender, RoutedEventArgs e)
        {
            HocSinh hs = new HocSinh
            {
                MaHS = txBx_MaHS.Text ?? "",
                HoTen = txBx_HoTen.Text ?? "",
                GioiTinh = txBx_GioiTinh.Text ?? "",
                NgaySinh = dtPk_NgaySinh.SelectedDate.GetValueOrDefault(DateTime.Now),
                DiaChi = txBx_DiaChi.Text ?? "",
                Email = txBx_Email.Text ?? ""
            };
            try
            {
                if (BLL.HocSinhBLL.TiepNhanHocSinh(hs))
                {
                    MessageBox.Show("Tiếp nhận học sinh thành công", "Thông báo: TÌNH TRẠNG TIẾP NHẬN", MessageBoxButton.OK, MessageBoxImage.Information);
                    btn_TiepNhan.IsEnabled = false;
                    stPn_ThongTin.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi tiếp nhận", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_LamMoi_Click(object sender, RoutedEventArgs e)
        {
            UserControl_Loaded(sender, e);
        }

        private void btn_TimKiem_Click(object sender, RoutedEventArgs e)
        {
            TimKiemWindow timKiemWindow = new TimKiemWindow();
            timKiemWindow.Owner = Window.GetWindow(this); // Get the parent window
            timKiemWindow.ShowDialog();
        }

        private void dtPk_NgaySinh_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CapNhatTuoiHocSinh();
        }

        private void LoadTuoiToiThieu()
        {
            try
            {
                int tuoiToiThieu = DAL.ThamSoDAL.LayTuoiToiThieu();
                txBx_TuoiToiThieu.Text = tuoiToiThieu.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tuổi tối thiểu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txBx_TuoiToiThieu.Text = "15"; // Giá trị mặc định
            }
        }

        private void LoadTuoiToiDa()
        {
            try
            {
                int tuoiToiDa = DAL.ThamSoDAL.LayTuoiToiDa();
                txBx_TuoiToiDa.Text = tuoiToiDa.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tuổi tối đa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txBx_TuoiToiDa.Text = "20"; // Giá trị mặc định
            }
        }

        private void CapNhatTuoiHocSinh()
        {
            if (dtPk_NgaySinh.SelectedDate.HasValue)
            {
                DateTime ngaySinh = dtPk_NgaySinh.SelectedDate.Value;
                int tuoi = TinhTuoi(ngaySinh);
                txBx_TuoiHocSinh.Text = tuoi.ToString();
            }
            else
            {
                txBx_TuoiHocSinh.Clear();
            }
        }

        private int TinhTuoi(DateTime ngaySinh)
        {
            DateTime today = DateTime.Today;
            int tuoi = today.Year - ngaySinh.Year;

            // Nếu chưa đến sinh nhật trong năm nay thì trừ 1
            if (ngaySinh.Date > today.AddYears(-tuoi))
                tuoi--;

            return tuoi;
        }
    }
}
