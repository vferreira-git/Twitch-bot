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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for Giveaways.xaml
    /// </summary>
    public partial class Giveaways : Page
    {
        MainWindow MainWindow;
        public Giveaways()
        {
            InitializeComponent();
            MainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void CreateGiveaway(object sender, RoutedEventArgs e)
        {
            NewGiveaway GiveawayPage = new NewGiveaway(new Giveaway());
            MainWindow.NavigationContainer.Navigate(GiveawayPage);
        }

        private void StartGiveaway(object sender, RoutedEventArgs e)
        {
            if (MainWindow.bot != null && MainWindow.bot.isConnected)
            {

                if (((Button)sender).Content.Equals("Stop"))
                {
                    var row = dataGridGiveaways.ItemContainerGenerator.ContainerFromItem(((FrameworkElement)sender).DataContext) as DataGridRow;
                    row.Background = new SolidColorBrush(Colors.Transparent);
                    Giveaway obj = ((FrameworkElement)sender).DataContext as Giveaway;
                    obj.GiveawayStop();
                }
                else
                {
                    var row = dataGridGiveaways.ItemContainerGenerator.ContainerFromItem(((FrameworkElement)sender).DataContext) as DataGridRow;
                    row.Background = new SolidColorBrush(Colors.Green);
                    Giveaway obj = ((FrameworkElement)sender).DataContext as Giveaway;
                    obj.GiveawayStart();
                }
            }
            else
            {
                MessageBox.Show("Error: Bot is not connected!", "Bot not connected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopGiveaway(object sender, RoutedEventArgs e)
        {
            if (MainWindow.bot.isConnected)
            {
                Giveaway obj = ((FrameworkElement)sender).DataContext as Giveaway;
                obj.GiveawayStop();
            }
            else
            {
                MessageBox.Show("Error: Bot is not connected!", "Bot not connected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void EditGiveaway(object sender, RoutedEventArgs e)
        {
            NewGiveaway GiveawayPage = new NewGiveaway(((FrameworkElement)sender).DataContext as Giveaway);
            MainWindow.NavigationContainer.Navigate(GiveawayPage);
        }

        private void DeleteGiveaway(object sender, RoutedEventArgs e)
        {
            DAL.DeleteGiveaway(((FrameworkElement)sender).DataContext as Giveaway);
            UpdateGiveaways();
        }

        public void UpdateGiveaways()
        {
            List<Giveaway> giveaways = DAL.GetGiveaways();
            dataGridGiveaways.ItemsSource = giveaways;
            Settings.Giveaways = giveaways;
        }

    }
}
