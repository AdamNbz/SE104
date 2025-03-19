using DTO;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string maHS = BLL.HocSinhBLL.LayMaHocSinhTuDong();
            txBx_MaHS.Text = maHS;
            btn_TiepNhan.IsEnabled = true;
            stPn_ThongTin.IsEnabled = true;
            txBx_DiaChi.Clear();
            txBx_Email.Clear();
            txBx_GioiTinh.Clear();
            txBx_HoTen.Clear();
            dtPk_NgaySinh.SelectedDate = null;
        }

        private void btn_TiepNhan_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txBx_HoTen.Text))
            {
                MessageBox.Show("Không được bỏ trống tên", "Lỗi: NHẬP LIỆU", MessageBoxButton.OK, MessageBoxImage.Error);
                txBx_HoTen.Focus();
            }
            else if (string.IsNullOrEmpty(txBx_GioiTinh.Text))
            {
                MessageBox.Show("Không được bỏ trống giới tính", "Lỗi: NHẬP LIỆU", MessageBoxButton.OK, MessageBoxImage.Error);
                txBx_GioiTinh.Focus();
            }
            else if (string.IsNullOrEmpty(txBx_Email.Text))
            {
                MessageBox.Show("Không được bỏ trống email", "Lỗi: NHẬP LIỆU", MessageBoxButton.OK, MessageBoxImage.Error);
                txBx_Email.Focus();
            }
            else if (dtPk_NgaySinh.SelectedDate == null)
            {
                MessageBox.Show("Không được bỏ trống ngày sinh", "Lỗi: NHẬP LIỆU", MessageBoxButton.OK, MessageBoxImage.Error);
                dtPk_NgaySinh.Focus();
            }
            else if (string.IsNullOrEmpty(txBx_DiaChi.Text))
            {
                MessageBox.Show("Không được bỏ trống địa chỉ", "Lỗi: NHẬP LIỆU", MessageBoxButton.OK, MessageBoxImage.Error);
                txBx_DiaChi.Focus();
            }
            else
            {
                HocSinh hs = new HocSinh(txBx_MaHS.Text, txBx_HoTen.Text, dtPk_NgaySinh.SelectedDate.Value, txBx_GioiTinh.Text, txBx_Email.Text, txBx_DiaChi.Text);

                try
                {
                    if (BLL.HocSinhBLL.TiepNhanHocSinh(hs))
                    {
                        MessageBox.Show("Tiếp nhận hoc sinh thành công", "Thông báo: TÌNH TRẠNG TIẾP NHẬN", MessageBoxButton.OK, MessageBoxImage.Information);
                        btn_TiepNhan.IsEnabled = false;
                        stPn_ThongTin.IsEnabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Tuổi của học sinh không hợp lệ", "Lỗi - Thất bại: TÌNH TRẠNG TIẾP NHẬN", MessageBoxButton.OK, MessageBoxImage.Error);
                    }    

                }
                catch(Exception)
                {
                    MessageBox.Show("Lỗi tiếp nhận", "Lỗi - Thất bại: TÌNH TRẠNG TIẾP NHẬN", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btn_LamMoi_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
        }
    }
}
