using System;
using System.Windows;
using System.Windows.Controls;
using GUI.Sprint1;
using GUI.Sprint2;
using GUI.Sprint3;
using GUI.Sprint4;
using GUI.Sprint5;
using GUI.Sprint6;

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
            // Select the first item by default
            if (NavList.Items.Count > 0)
            {
                NavList.SelectedIndex = 0;
            }
        }

        private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (NavList.SelectedItem == null) return;

                // Get the selected ListBoxItem
                ListBoxItem selectedListBoxItem = NavList.SelectedItem as ListBoxItem;
                if (selectedListBoxItem == null) return;

                // Find the TextBlock inside the StackPanel
                StackPanel stackPanel = selectedListBoxItem.Content as StackPanel;
                if (stackPanel == null || stackPanel.Children.Count < 2) return;

                TextBlock textBlock = stackPanel.Children[1] as TextBlock;
                if (textBlock == null) return;

                // Get the text content
                string selectedItem = textBlock.Text;
                UserControl selectedControl = null;

                switch (selectedItem)
                {
                    case "Sprint 1":
                        selectedControl = new Sprint1Control();
                        break;
                    case "Sprint 2":
                        try
                        {
                            selectedControl = new Sprint2Control();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi tạo SimpleSprint2Control: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    case "Sprint 3":
                        try
                        {
                            selectedControl = new Sprint3Control();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi tạo Sprint3Control: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    case "Sprint 4":
                        try
                        {
                            selectedControl = new Sprint4Control();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi tạo Sprint4Control: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    case "Sprint 5":
                        try
                        {
                            selectedControl = new Sprint5Control();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi tạo Sprint5Control: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    case "Sprint 6":
                        try
                        {
                            selectedControl = new Sprint6Control();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi tạo Sprint6Control: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                }

                // Hiển thị nội dung trong ContentControl
                if (selectedControl != null)
                {
                    MainContent.Content = selectedControl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chuyển đổi tab: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
