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
                    MessageBox.Show("Tiếp nhận hoc sinh thành công", "Thông báo: TÌNH TRẠNG TIẾP NHẬN", MessageBoxButton.OK, MessageBoxImage.Information);
                    btn_TiepNhan.IsEnabled = false;
                    stPn_ThongTin.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Thong Bao Loi Tiep Nhan",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_LamMoi_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
        }
    }
}