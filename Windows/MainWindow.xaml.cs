using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using System.Net.Http;
using System.Net;
using System.Diagnostics;
using WpfApp1.Classes;
using System.Data.SQLite;
using System.IO;
using System.Xml.XPath;
using System.Threading;
using System.Collections;
using WpfApp1.Windows;
using WpfApp1.Pages;
using System.Runtime.InteropServices;

namespace WpfApp1
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isMaximized = false;
        double lastWidth = 600;
        double lastHeight = 600;
        double lastX = 0;
        double lastY = 0;
        public Bot bot;
        Process p;

        SettingsPage SPage;
        Codes CPage;
        public Giveaways GPage;
        Home HPage;


        public MainWindow()
        {
            InitializeComponent();

            HPage = new Home();
            SPage = new SettingsPage();
            CPage = new Codes();
            GPage = new Giveaways();

            StartSetup();
            bot = new Bot(this);
            Width = SystemParameters.WorkArea.Width / 1.5;
            Height = SystemParameters.WorkArea.Height / 1.5;
            header.DataContext = bot;
        }






        private void StartSetup()
        {
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/pengus.db"))
            {
                SQLiteConnection.CreateFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/pengus.db");
                DAL.SetupTable();
            }

            NavigationContainer.Navigate(HPage);

            btnHome.BorderThickness = new Thickness(4, 0, 0, 0);
            btnGs.BorderThickness = new Thickness(0, 0, 0, 0);
            btnGiv.BorderThickness = new Thickness(0, 0, 0, 0);
            btnCodes.BorderThickness = new Thickness(0, 0, 0, 0);

            GPage.UpdateGiveaways();
            CPage.UpdateCodes();
            LoadSettings();
        }

        private void LoadSettings()
        {
            DAL.LoadSettings();

            SPage.LoadSettings();
        }

        private void header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        internal void BotStarting()
        {
            HPage.lblBotStats.Foreground = new SolidColorBrush(Colors.Yellow);
            HPage.lblBotStats.Content = "Bot is connecting...";
        }

        internal void BotStarted()
        {
            HPage.lblBotStats.Foreground = new SolidColorBrush(Colors.LightGreen);
            HPage.lblBotStats.Content = "Bot is connected!";
            HPage.btnStopBot.IsEnabled = true;
            HPage.imgStatus.Source = new BitmapImage(new Uri("pack://application:,,,/WpfApp1;component/Resources/Bot-Online.png"));
            HPage.lblConnectionInfo.Content = bot.client.JoinedChannels[0].Channel;
        }

        internal void BotStartFailed()
        {
            HPage.lblBotStats.Foreground = new SolidColorBrush(Colors.Red);
            HPage.lblBotStats.Content = "Bot failed to connect!";
            HPage.btnStartBot.IsEnabled = true;
        }

        internal void BotStopped()
        {
            HPage.lblBotStats.Foreground = new SolidColorBrush(Colors.Red);
            HPage.lblBotStats.Content = "Bot is disconnected!";
            HPage.btnStartBot.IsEnabled = true;
            HPage.imgStatus.Source = new BitmapImage(new Uri("pack://application:,,,/WpfApp1;component/Resources/Bot-Offline.png"));
            HPage.lblConnectionInfo.Content = "-";
        }

        #region ResizeWindows
        bool ResizeInProcess = false;
        private void Resize_Init(object sender, MouseButtonEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                Rectangle senderRect = sender as Rectangle;
                if (senderRect != null)
                {
                    ResizeInProcess = true;
                    senderRect.CaptureMouse();
                }
            }
        }

        private void Resize_End(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = false; ;
                senderRect.ReleaseMouseCapture();
            }
        }

        private void Resizeing_Form(object sender, MouseEventArgs e)
        {
            if (ResizeInProcess)
            {
                Rectangle senderRect = sender as Rectangle;
                Window mainWindow = senderRect.Tag as Window;
                if (senderRect != null)
                {
                    double width = e.GetPosition(mainWindow).X;
                    double height = e.GetPosition(mainWindow).Y;
                    senderRect.CaptureMouse();
                    if (senderRect.Name.ToLower().Contains("right"))
                    {
                        width += 5;
                        if (width > 0)
                            mainWindow.Width = width;
                    }
                    if (senderRect.Name.ToLower().Contains("left"))
                    {
                        width -= 5;
                        mainWindow.Left += width;
                        width = mainWindow.Width - width;
                        if (width > 0)
                        {
                            mainWindow.Width = width;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("bottom"))
                    {
                        height += 5;
                        if (height > 0)
                            mainWindow.Height = height;
                    }
                    if (senderRect.Name.ToLower().Contains("top"))
                    {
                        height -= 5;
                        mainWindow.Top += height;
                        height = mainWindow.Height - height;
                        if (height > 0)
                        {
                            mainWindow.Height = height;
                        }
                    }
                }
            }
        }

        #endregion

        private void SaveSettings()
        {
            SPage.SaveSettings();
            DAL.SaveSettings();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaxMinButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isMaximized)
            {
                lastX = this.Left;
                lastY = this.Top;
                lastWidth = this.Width;
                lastHeight = this.Height;
                this.Left = 0;
                this.Top = 0;
                this.Width = SystemParameters.WorkArea.Width;
                this.Height = SystemParameters.WorkArea.Height;
                isMaximized = true;
            }

            else
            {
                this.Left = lastX;
                this.Top = lastY;
                this.Width = lastWidth;
                this.Height = lastHeight;
                isMaximized = false;
            }
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void StartBot()
        {
            if (!string.IsNullOrEmpty(Settings.AuthToken))
            {
                bot = new Bot(this);
                SaveSettings();
                bot.BotStart();
            }
            else if (string.IsNullOrEmpty(Settings.AuthToken) || p.HasExited)
            {
                GetAuthToken();
            }
        }

        private async void GetAuthToken()
        {
            try
            {
                try
                {
                    p = System.Diagnostics.Process.Start("Chrome.exe", $"--app=https://id.twitch.tv/oauth2/authorize?client_id={Settings.ClientId}&redirect_uri=http://localhost:7425&response_type=code&scope=channel_check_subscription --window-size=600,800");
                }
                catch
                {

                    p = System.Diagnostics.Process.Start("firefox.exe", $"https://id.twitch.tv/oauth2/authorize?client_id={Settings.ClientId}&redirect_uri=http://localhost:7425&response_type=code&scope=channel_check_subscription");
                }
            }
            catch
            {
                p = System.Diagnostics.Process.Start("Opera.exe", $"https://id.twitch.tv/oauth2/authorize?client_id={Settings.ClientId}&redirect_uri=http://localhost:7425&response_type=code&scope=channel_check_subscription");
            }
            p.Exited += (object sender, EventArgs e) =>
            {
                Thread.CurrentThread.Abort();
            };
            var prefix = "http://localhost:7425/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            try
            {
                listener.Start();
            }
            catch (HttpListenerException hlex)
            {
            }

            while (listener.IsListening && !p.HasExited)
            {
                try
                {

                    var context = await listener.GetContextAsync();

                    if (!string.IsNullOrEmpty(context.Request.QueryString["code"]))
                    {
                        Settings.AuthToken = "a38dd5of0jsy47mjve7f1hwinegpy7";
                        listener.Close();
                        p.Kill();
                    }
                    else
                    {
                        listener.Close();
                        p.Kill();
                    }
                }
                catch
                {
                }
            }
        }

        public void StopBot()
        {
            bot.BotStop();
            bot = null;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

        private void btnCodes_Click(object sender, RoutedEventArgs e)
        {
            NavigationContainer.Navigate(CPage);

            btnHome.BorderThickness = new Thickness(0, 0, 0, 0);
            btnGs.BorderThickness = new Thickness(0, 0, 0, 0);
            btnGiv.BorderThickness = new Thickness(0, 0, 0, 0);
            btnCodes.BorderThickness = new Thickness(4, 0, 0, 0);

        }

        private void btnGs_Click(object sender, RoutedEventArgs e)
        {
            NavigationContainer.Navigate(SPage);

            btnHome.BorderThickness = new Thickness(0, 0, 0, 0);
            btnGs.BorderThickness = new Thickness(4, 0, 0, 0);
            btnGiv.BorderThickness = new Thickness(0, 0, 0, 0);
            btnCodes.BorderThickness = new Thickness(0, 0, 0, 0);


        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationContainer.Navigate(HPage);

            btnHome.BorderThickness = new Thickness(4, 0, 0, 0);
            btnGs.BorderThickness = new Thickness(0, 0, 0, 0);
            btnGiv.BorderThickness = new Thickness(0, 0, 0, 0);
            btnCodes.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void btnGiv_Click(object sender, RoutedEventArgs e)
        {
            NavigationContainer.Navigate(GPage);

            btnHome.BorderThickness = new Thickness(0, 0, 0, 0);
            btnGs.BorderThickness = new Thickness(0, 0, 0, 0);
            btnGiv.BorderThickness = new Thickness(4, 0, 0, 0);
            btnCodes.BorderThickness = new Thickness(0, 0, 0, 0);

        }

        public void UpdateGiveaways()
        {
            GPage.UpdateGiveaways();
        }

        public void UpdateCodes()
        {
            CPage.UpdateCodes();
        }

    }
}
