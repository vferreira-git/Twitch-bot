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
using WpfApp1.Classes;
using WpfApp1.Windows;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for Codes.xaml
    /// </summary>
    public partial class Codes : Page
    {
        MainWindow MainWindow;
        public Codes()
        {
            InitializeComponent();
            MainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void AddCode(object sender, RoutedEventArgs e)
        {
            CodeCreateWindow window = new CodeCreateWindow();
            window.Owner = MainWindow;
            window.Show();
        }

        private void DeleteCode(object sender, RoutedEventArgs e)
        {
            DAL.DeleteCode(((FrameworkElement)sender).DataContext as Code);
            UpdateCodes();
        }

        public void UpdateCodes()
        {
            List<Code> codes = DAL.GetCodes();
            datagridCodes.ItemsSource = codes;
            Settings.Codes = codes;
        }
    }
}
