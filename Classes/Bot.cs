using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using System.Diagnostics;
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.FollowerService;
using WpfApp1.Classes;
using System.Windows;
using System.Windows.Media;
using System.Net;
using System.Threading;

namespace WpfApp1
{
    public class Bot
    {
        internal TwitchClient client;
        TwitchAPI api;
        FollowerService follower;
        bool AuthToken = false;
        TwitchLib.Api.Sections.Users.V5Api UserApi;
        Dictionary<string, Giveaway> hashtags = new Dictionary<string, Giveaway>();
        MainWindow window;

        public Bot(MainWindow window)
        {
            this.window = window;
        }

        public bool isConnected = false;

        public void BotStart()
        {
            try
            {
                ConnectionCredentials credentials = new ConnectionCredentials("JustAnyBot", Settings.AuthToken);

                client = new TwitchClient();
                client.Initialize(credentials);
                client.OnConnected += Client_OnConnected;
                client.AddChatCommandIdentifier(char.Parse("#"));
                client.OnConnectionError += Client_ConnectionError;
                client.OnUserTimedout += Client_TimedOut;
                if (Settings.AnnounceJoin)
                    client.OnUserJoined += Client_UserJoined;
                client.OnChatCommandReceived += Client_ChatCommandReceived;
                if (Settings.AnnounceLeave)
                    client.OnUserLeft += Client_UserLeft;
                if (Settings.ThankSubscribers)
                    client.OnNewSubscriber += Client_Subscribed;

                api = new TwitchAPI();
                api.Settings.ClientId = Settings.ClientId;
                api.Settings.AccessToken = Settings.AuthToken;

                GetInfo();

                if (Settings.ThankFollowers)
                {
                    follower = new FollowerService(api, 5);
                    follower.SetChannelByChannelId(Settings.ChannelId);
                    follower.OnNewFollowersDetected += Client_Followed;
                    follower.StartService();
                }

                if (Settings.AnnounceBan)
                    client.OnUserBanned += Client_Banned;
                if (Settings.AnnounceNowHosting)
                    client.OnNowHosting += Client_Hosting;

                UserApi = new TwitchLib.Api.Sections.Users.V5Api(api);

                client.Connect();


                window.Dispatcher.Invoke((Action)(() =>
                {
                    window.BotStarting();
                }));
            }
            catch(ThreadAbortException e)
            {
                MessageBox.Show("Erro: A Janela fechou antes de obter o AuthToken");
            }
            catch (Exception e)
            {
                MessageBox.Show($"An exception has occurred: \n\n{e.Message}");
            }

        }

        //private void GetAuthToken()
        //{
        //    try
        //    {
        //        try
        //        {
        //            p = System.Diagnostics.Process.Start("Chrome.exe", $"--app=https://id.twitch.tv/oauth2/authorize?client_id={Settings.ClientId}&redirect_uri=http://localhost:7425&response_type=code&scope=channel_check_subscription --window-size=600,800");
        //        }
        //        catch
        //        {

        //            p = System.Diagnostics.Process.Start("firefox.exe", $"https://id.twitch.tv/oauth2/authorize?client_id={Settings.ClientId}&redirect_uri=http://localhost:7425&response_type=code&scope=channel_check_subscription");
        //        }
        //    }
        //    catch
        //    {
        //        p = System.Diagnostics.Process.Start("MicrosoftEdge.exe", $"https://id.twitch.tv/oauth2/authorize?client_id={Settings.ClientId}&redirect_uri=http://localhost:7425&response_type=code&scope=channel_check_subscription");
        //    }
        //    p.Exited += (object sender, EventArgs e) =>
        //    {
        //        Thread.CurrentThread.Abort();
        //    };
        //    Task t = new Task(async () =>
        //    {
        //        var prefix = "http://localhost:7425/";
        //        HttpListener listener = new HttpListener();
        //        listener.Prefixes.Add(prefix);
        //        try
        //        {
        //            listener.Start();
        //        }
        //        catch (HttpListenerException hlex)
        //        {
        //        }

        //        while (listener.IsListening && !p.HasExited)
        //        {
        //            try
        //            {

        //                var context = await listener.GetContextAsync();

        //                if (!string.IsNullOrEmpty(context.Request.QueryString["code"]))
        //                {
        //                    Settings.AuthToken = "a38dd5of0jsy47mjve7f1hwinegpy7";
        //                    listener.Close();
        //                    p.Kill();
        //                }
        //                else
        //                {
        //                    listener.Close();
        //                    p.Kill();
        //                }
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    );
        //    t.Start();
        //    while(!t.IsCompleted && !p.HasExited)
        //    {

        //    }
        //}

        //public async void RunHttp(Process p)
        //{
        //    var prefix = "http://localhost:7425/";
        //    HttpListener listener = new HttpListener();
        //    listener.Prefixes.Add(prefix);
        //    try
        //    {
        //        listener.Start();
        //    }
        //    catch (HttpListenerException hlex)
        //    {
        //    }

        //    while (listener.IsListening && !p.HasExited)
        //    {
        //        try
        //        {

        //            var context = await listener.GetContextAsync();

        //            if (!string.IsNullOrEmpty(context.Request.QueryString["code"]))
        //            {
        //                Settings.AuthToken = "a38dd5of0jsy47mjve7f1hwinegpy7";
        //                listener.Close();
        //                p.Kill();
        //            }
        //            else
        //            {
        //                listener.Close();
        //                p.Kill();
        //            }
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}

        private void Client_TimedOut(object sender, OnUserTimedoutArgs e)
        {
            window.Dispatcher.Invoke((Action)(() =>
            {
                window.BotStopped();

            }));
        }

        public void BotStop()
        {
            client.Disconnect();
            isConnected = false;
            window.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                window.BotStopped();
            }));
        }



        public async Task<bool> UserFollowsChannel(string userId)
        {
            return await UserApi.UserFollowsChannelAsync(userId, Settings.ChannelId);
        }

        public async Task<TwitchLib.Api.Models.v5.Users.Users> GetUserByName(string name)
        {
            return await UserApi.GetUserByNameAsync(name);
        }

        public async Task<List<TwitchLib.Api.Models.Undocumented.Chatters.ChatterFormatted>> GetChatters()
        {
            return await api.Undocumented.GetChattersAsync(Settings.ChannelName);
        }

        public void SendWhisper(string username, string message)
        {
            client.SendWhisper(username, message);
        }

        public void SendMessage(string message)
        {
            try
            {
                client.SendMessage(client.JoinedChannels[0].Channel, message);
            }
            catch (Exception e)
            {
                MessageBox.Show($"An exception has occurred: \n\n{e.Message}");
            }
        }



        internal void StopGiveaway(Giveaway giveaway)
        {
            hashtags.Remove(giveaway.Hashtag);
        }

        private async void GetInfo()
        {
            try
            {
                TwitchLib.Api.Models.v5.Channels.ChannelAuthed channel = await new TwitchLib.Api.Sections.Channels.V5Api(api).GetChannelAsync("5e0n8srzhaukogwvjjzwvy8xao8zoy");
                Settings.ChannelId = channel.Id.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show($"An exception has occurred: \n\n{e.Message}");
            }
        }

        internal void StartingGiveaway(Giveaway giveaway)
        {
            if (giveaway.Type == 1 || giveaway.Type == 2)
            {
                hashtags.Add(giveaway.Hashtag, giveaway);

                if (giveaway.AnnounceStart)
                    client.SendMessage(client.JoinedChannels[0].Channel, giveaway.StartMessage);
            }
            else
            {
                if (giveaway.AnnounceStart)
                    client.SendMessage(client.JoinedChannels[0].Channel, giveaway.StartMessage);
            }
        }



        private void Client_Hosting(object sender, OnNowHostingArgs e)
        {
            if (Settings.AnnounceNowHosting)
                client.SendMessage(e.Channel, $"{e.HostedChannel} {Settings.Messages.mNowHosting}");
        }

        private void Client_Banned(object sender, OnUserBannedArgs e)
        {
            if (Settings.AnnounceBan)
                client.SendMessage(e.UserBan.Channel, $"{e.UserBan.Username} {Settings.Messages.mUserBanned}");
        }

        private void Client_Subscribed(object sender, OnNewSubscriberArgs e)
        {
            if (Settings.ThankSubscribers)
                client.SendMessage(e.Channel, $"{e.Subscriber.DisplayName} {Settings.Messages.mUserSubscribes}");
        }

        private void Client_Followed(object sender, OnNewFollowersDetectedArgs e)
        {
            if (Settings.ThankFollowers)
                client.SendMessage(client.JoinedChannels[0], $"{e.NewFollowers[0].User.Name} {Settings.Messages.mUserFollows}");
        }

        private void Client_UserLeft(object sender, OnUserLeftArgs e)
        {
            if (Settings.AnnounceLeave)
                client.SendMessage(e.Channel, $"{e.Username} {Settings.Messages.mUserLeaves}");
        }

        public Bot()
        {

        }

        private async void Client_ChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (hashtags.ContainsKey(e.Command.ChatMessage.Message))
            {
                if (hashtags[e.Command.ChatMessage.Message].Type == 2)
                {

                    bool uf = await new TwitchLib.Api.Sections.Users.V5Api(api).UserFollowsChannelAsync(e.Command.ChatMessage.UserId, Settings.ChannelId.ToString());
                    if (uf)
                        hashtags[e.Command.ChatMessage.Message].hashtagUsers.Add(e.Command.ChatMessage.Username);
                }
                else if (hashtags[e.Command.ChatMessage.Message].Type == 1)
                    hashtags[e.Command.ChatMessage.Message].hashtagUsers.Add(e.Command.ChatMessage.Username);


            }
        }

        private void Client_UserJoined(object sender, OnUserJoinedArgs e)
        {
            if (Settings.AnnounceJoin)
                client.SendMessage(e.Channel, $"{e.Username} {Settings.Messages.mUserJoins}");
            Console.WriteLine("User joined");
        }

        private void Client_ConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine(e.Error);
            isConnected = false;
            window.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                window.BotStartFailed();
            }));

        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {

            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
            client.JoinChannel(Settings.ChannelName);
            isConnected = true;
            window.Dispatcher.Invoke((Action)(() =>
            {//this refer to form in WPF application 
                window.BotStarted();
            }));
        }
    }

}
