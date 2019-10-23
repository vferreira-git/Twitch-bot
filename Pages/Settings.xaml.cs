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
    /// Interaction logic for GeneralSettings.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            txtAuthToken.Text = Settings.AuthToken;
            txtChannel.Text = Settings.ChannelName;
            txtSLAuthToken.Text = Settings.SLAuthToken;
            checkJoining.IsChecked = Settings.AnnounceJoin;
            checkLeaving.IsChecked = Settings.AnnounceLeave;
            checkFollow.IsChecked = Settings.ThankFollowers;
            checkFollow.IsChecked = Settings.ThankSubscribers;
            checkWhisperGiveaway.IsChecked = Settings.WhisperWinners;
            checkSub.IsChecked = Settings.ThankSubscribers;
            checkBan.IsChecked = Settings.AnnounceBan;
            checkHostStart.IsChecked = Settings.AnnounceNowHosting;
            checkWhisperCode.IsChecked = Settings.WhisperCode;

            txtMessageJoin.Text = Settings.Messages.mUserJoins;
            txtMessageLeave.Text = Settings.Messages.mUserLeaves;
            txtMessageFollow.Text = Settings.Messages.mUserFollows;
            txtMessageSub.Text = Settings.Messages.mUserSubscribes;
            txtMessageBan.Text = Settings.Messages.mUserBanned;
            txtMessageChannelHost.Text = Settings.Messages.mNowHosting;
        }

        public void SaveSettings()
        {
            Settings.ChannelName = txtChannel.Text;
            Settings.SLAuthToken = txtSLAuthToken.Text;
            Settings.AnnounceJoin = checkJoining.IsChecked.Value;
            Settings.AnnounceLeave = checkLeaving.IsChecked.Value;
            Settings.ThankFollowers = checkFollow.IsChecked.Value;
            Settings.ThankSubscribers = checkSub.IsChecked.Value;
            Settings.WhisperWinners = checkWhisperGiveaway.IsChecked.Value;
            Settings.AnnounceBan = checkBan.IsChecked.Value;
            Settings.AnnounceNowHosting = checkHostStart.IsChecked.Value;
            Settings.WhisperCode = checkWhisperCode.IsChecked.Value;

            Settings.Messages.mUserJoins = txtMessageJoin.Text;
            Settings.Messages.mUserLeaves = txtMessageLeave.Text;
            Settings.Messages.mUserFollows = txtMessageFollow.Text;
            Settings.Messages.mUserSubscribes = txtMessageSub.Text;
            Settings.Messages.mUserBanned = txtMessageBan.Text;
            Settings.Messages.mNowHosting = txtMessageChannelHost.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings.AuthToken = "";
            txtAuthToken.Text = "";
        }

        private void CheckStringArguments(object sender, TextCompositionEventArgs e)
        {
            TextBox txtBox = sender as TextBox;

            if (txtBox.Equals(txtMessageJoin))
            {
                if (!txtMessageJoin.Text.Contains("{username}"))
                {
                    lblUserJoins.Foreground = new SolidColorBrush(Colors.Red);
                    lblUserJoins.Content = "Error: Argument {username} not defined in message!";
                }
                else
                {
                    lblUserJoins.Foreground = new SolidColorBrush(Colors.LightYellow);
                    Binding b = new Binding();
                    b.Path = new PropertyPath(TextBox.TextProperty);
                    b.ElementName = "txtMessageJoin";
                    lblUserJoins.SetBinding(ContentProperty, b);
                }
            }

        }

        private void CheckStringArguments2(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;

            if (txtBox.Equals(txtMessageJoin))
            {
                if (!txtMessageJoin.Text.Contains("{username}") && !string.IsNullOrEmpty(txtMessageJoin.Text))
                {
                    lblUserJoins.Foreground = new SolidColorBrush(Colors.Red);
                    lblUserJoins.Content = "Error: Argument {username} not defined in message!";
                }
                else
                {
                    lblUserJoins.Foreground = new SolidColorBrush(Colors.LightYellow);
                    lblUserJoins.Content = txtMessageJoin.Text.Replace("{username}", "StreamerBanana");
                }
            }

            if (txtBox.Equals(txtMessageLeave) && !string.IsNullOrEmpty(txtMessageLeave.Text))
            {
                if (!txtMessageLeave.Text.Contains("{username}"))
                {
                    lblUserLeaves.Foreground = new SolidColorBrush(Colors.Red);
                    lblUserLeaves.Content = "Error: Argument {username} not defined in message!";
                }
                else
                {
                    lblUserLeaves.Foreground = new SolidColorBrush(Colors.LightYellow);
                    lblUserLeaves.Content = txtMessageLeave.Text.Replace("{username}", "StreamerBanana");
                }
            }

            if (txtBox.Equals(txtMessageFollow) && !string.IsNullOrEmpty(txtMessageFollow.Text))
            {
                if (!txtMessageFollow.Text.Contains("{username}"))
                {
                    lblUserFollows.Foreground = new SolidColorBrush(Colors.Red);
                    lblUserFollows.Content = "Error: Argument {username} not defined in message!";
                }
                else
                {
                    lblUserFollows.Foreground = new SolidColorBrush(Colors.LightYellow);
                    lblUserFollows.Content = txtMessageFollow.Text.Replace("{username}", "StreamerBanana");
                }
            }

            if (txtBox.Equals(txtMessageSub) && !string.IsNullOrEmpty(txtMessageSub.Text))
            {
                if (!txtMessageSub.Text.Contains("{username}"))
                {
                    lblUserSubscribes.Foreground = new SolidColorBrush(Colors.Red);
                    lblUserSubscribes.Content = "Error: Argument {username} not defined in message!";
                }
                else
                {
                    lblUserSubscribes.Foreground = new SolidColorBrush(Colors.LightYellow);
                    lblUserSubscribes.Content = txtMessageSub.Text.Replace("{username}", "StreamerBanana");
                }
            }

            if (txtBox.Equals(txtMessageBan))
            {
                if (!txtMessageBan.Text.Contains("{username}") && !string.IsNullOrEmpty(txtMessageBan.Text))
                {
                    lblUserBanned.Foreground = new SolidColorBrush(Colors.Red);
                    lblUserBanned.Content = "Error: Argument {username} not defined in message!";
                }
                else
                {
                    lblUserBanned.Foreground = new SolidColorBrush(Colors.LightYellow);
                    if (txtMessageBan.Text.Contains("{banreason}"))
                        lblUserBanned.Content = txtMessageBan.Text.Replace("{username}", "StreamerBanana").Replace("{banreason}", "Spamming");

                    lblUserBanned.Content = txtMessageBan.Text.Replace("{username}", "StreamerBanana");
                }
            }
        }
    }
}
