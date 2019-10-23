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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        MainWindow window;
        public Home()
        {
            InitializeComponent();
            window = (MainWindow)Application.Current.MainWindow;
        }

        private void StartBotClick(object sender, RoutedEventArgs e)
        {
            btnStartBot.IsEnabled = false;
            window.StartBot();
        }

        private void StopBotClick(object sender, RoutedEventArgs e)
        {
            btnStopBot.IsEnabled = false;
            window.StopBot();
        }

    }
}
