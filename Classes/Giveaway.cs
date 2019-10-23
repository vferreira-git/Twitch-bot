using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.Classes
{
    public class Giveaway : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reward { get; set; }
        public string RewardString { get; set; }
        public int Type { get; set; }
        public string Hashtag { get; set; }
        public int Repeat { get; set; }
        public int Delay { get; set; }
        public string StartMessage { get; set; }
        public string WinnerMessage { get; set; }
        public string WinnerWhisper { get; set; }
        public string CodeWhisper { get; set; }
        public bool AnnounceStart { get; set; }
        public bool AutoPickCode { get; set; }
        private bool _IsRunning = false;
        private string _ButtonString = "";

        public bool IsRunning
        {
            get { return _IsRunning; }
            set
            {
                _IsRunning = value;
                ButtonString = IsRunning ? "Stop" : "Start";
            }
        }

        public string ButtonString { get { return _ButtonString; }
            set
            {
                _ButtonString = value;
                OnPropertyChanged("ButtonString");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public List<string> hashtagUsers { get; set; }

        MainWindow window;

        Bot bot;

        Timer timer;

        public int RanTimes = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public Giveaway()
        {
            Id = -1;
            window = Application.Current.MainWindow as MainWindow;
            IsRunning = false;
        }

        public void GiveawayStart()
        {
            bot = window.bot;
            IsRunning = true;
            RanTimes = 0;
            timer = new Timer(Delay * 1000);
            if (Repeat > 0)
                timer.AutoReset = true;
            timer.Elapsed += RollGiveaway;
            timer.Start();

            if (Type == 1 || Type == 2)
                hashtagUsers = new List<string>();
            bot.StartingGiveaway(this);

        }

        public void GiveawayStop()
        {
            IsRunning = false;
            timer.Stop();
            timer.Close();
            if (Type == 1 || Type == 2)
            {
                hashtagUsers.Clear();
                bot.StopGiveaway(this);
            }
            window.Dispatcher.Invoke((Action)(() =>
            {
                var row = window.GPage.dataGridGiveaways.ItemContainerGenerator.ContainerFromItem(this) as DataGridRow;
                row.Background = new SolidColorBrush(Colors.Transparent);
            }));
        }

        private void RollGiveaway(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            RollGiveaway();
        }

        private async void RollGiveaway()
        {
            string winner = "";

            if (this.AutoPickCode && Settings.Codes.Count() == 0)
            {
                GiveawayStop();
                bot.SendMessage("Giveaway ended. No codes left to give.");
                MessageBox.Show("Error: No codes left to give. Giveaway stopped.");
                return;
            }

            switch (this.Type)
            {
                case 1:
                case 2:
                    try
                    {

                        if (this.hashtagUsers.Count() > 0)
                        {
                            winner = this.hashtagUsers[new Random().Next(this.hashtagUsers.Count())];
                            bot.SendMessage($"{winner} {this.WinnerMessage}");
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"An exception has occurred: \n\n{e.Message}");
                        this.GiveawayStop();
                    }
                    break;

                case 3:

                    try
                    {

                        List<TwitchLib.Api.Models.Undocumented.Chatters.ChatterFormatted> chatters = await bot.GetChatters();
                        List<string> users = new List<string>();
                        if (chatters.Count() <= 0)
                        {
                            MessageBox.Show("Error: No viewers to pick a giveaway winner from.");
                            GiveawayStop();
                            return;
                        }
                        foreach (TwitchLib.Api.Models.Undocumented.Chatters.ChatterFormatted cf in chatters)
                        {
                            TwitchLib.Api.Models.v5.Users.Users user = await bot.GetUserByName(cf.Username);
                            bool uf = await bot.UserFollowsChannel(user.Matches[0].Id);
                            if (uf)
                                users.Add(user.Matches[0].DisplayName);
                        }
                        winner = users[new Random().Next(users.Count)];
                        bot.SendMessage($"{winner}  {this.WinnerMessage}");

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"An exception has occurred: \n\n{e.Message}");
                        GiveawayStop();
                    }
                    break;

                case 4:
                    try
                    {

                        List<TwitchLib.Api.Models.Undocumented.Chatters.ChatterFormatted> chatters = await bot.GetChatters();
                        if (chatters.Count() <= 0)
                        {
                            MessageBox.Show("Error: No viewers to pick a giveaway winner from.");
                            GiveawayStop();
                            return;
                        }
                        winner = chatters[new Random().Next(chatters.Count)].Username;
                        bot.SendMessage($"{winner} {this.WinnerMessage}");

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"An exception has occurred: \n\n{e.Message}");
                        this.GiveawayStop();
                    }
                    break;

            }

            MessageBoxResult result = MessageBox.Show($"{winner} has won \n\n Would you wish to Re-Roll the winner?", this.Name, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                RollGiveaway();
            }
            else
            {

                if (this.Reward == 1)
                {

                    if (Settings.WhisperCode)
                    {
                        Code code = Settings.Codes[new Random().Next(Settings.Codes.Count())];
                        if (!string.IsNullOrEmpty(this.CodeWhisper))
                        {
                            if (this.AutoPickCode)
                            {
                                bot.SendWhisper(winner, $"{this.CodeWhisper} {code.CodeString}");
                            }
                            else
                            {
                                bot.SendWhisper(winner, $"{this.CodeWhisper} {this.RewardString}");
                            }
                        }
                        else
                        {
                            if (this.AutoPickCode)
                            {
                                bot.SendWhisper(winner, $"Congratulations! You have won the giveaway: {this.Name}. This is a redeem code giveaway, so the code is: {code.CodeString}");
                            }
                            else
                            {
                                bot.SendWhisper(winner, $"Congratulations! You have won the giveaway: {this.Name}. This is a redeem code giveaway, so the code is: {this.Reward}");
                            }
                        }
                        Settings.Codes.Remove(code);
                        DAL.DeleteCode(code);
                        window.Dispatcher.Invoke((Action)(() =>
                        {//this refer to form in WPF application 
                            window.UpdateCodes();
                        }));

                    }
                    else
                    {
                        Code code = Settings.Codes[new Random().Next(Settings.Codes.Count())];
                        if (this.AutoPickCode)
                        {
                            MessageBox.Show($"{winner} won the code: {code.CodeString}");
                        }
                        else
                        {
                            MessageBox.Show($"{winner} won the code: {this.RewardString}");
                        }
                        Settings.Codes.Remove(code);
                        DAL.DeleteCode(code);
                        window.Dispatcher.Invoke((Action)(() =>
                        {//this refer to form in WPF application 
                            window.UpdateCodes();
                        }));
                    }
                }


                if (this.RanTimes < this.Repeat)
                {
                    this.RanTimes++;
                    timer.Start();
                }
                else
                {
                    this.GiveawayStop();
                }
            }
        }
    }
}
