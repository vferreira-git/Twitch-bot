using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Classes
{
    public static class Settings
    {


        public static bool AnnounceJoin { get; set; }
        public static bool AnnounceLeave { get; set; }
        public static bool ThankFollowers { get; set; }
        public static bool ThankSubscribers { get; set; }
        public static bool WhisperWinners { get; set; }
        public static bool AnnounceBan { get; set; }
        public static bool AnnounceNowHosting { get; set; }
        public static bool WhisperCode { get; set; }
        public static string AuthToken { get; set; }
        public static string ChannelName { get; set; }
        public static string SLAuthToken { get; set; }
        public const string ClientId = "k2ot1ymfb6pas2ep9uyahtzju64t09";
        
        public static class Messages
        {
            public static string mUserJoins { get; set; }
            public static string mUserLeaves { get; set; }
            public static string mUserFollows { get; set; }
            public static string mUserSubscribes { get; set; }
            public static string mUserBanned { get; set; }
            public static string mNowHosting { get; set; }
        }

        public static List<Giveaway> Giveaways { get; set; }
        public static List<Code> Codes { get; set; }

        public static string ChannelId { get; set; }

    }
}
