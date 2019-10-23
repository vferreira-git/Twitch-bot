using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections;
using System.Windows;

namespace WpfApp1.Classes
{

    public static class DAL
    {

        private static string connectionString = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\pengus.db; Version=3;";


        public static bool AddCode(Code code)
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Codes (Name,CodeString) VALUES (@p1,@p2)", _conn))
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@p1", code.Name);
                        cmd.Parameters.AddWithValue("@p2", code.CodeString);

                        if (cmd.ExecuteNonQuery() != 0)
                        {
                            _conn.Close();
                            return true;
                        }
                        else
                        {
                            _conn.Close();
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error adding code to DB: \n\n {e.Message}");
                return false;
            }
        }

        public static List<Code> GetCodes()
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Codes", _conn))
                    {
                        cmd.Connection.Open();
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            List<Code> codes = new List<Code>();
                            while (dr.Read())
                            {
                                Code code = new Code();
                                code.Id = dr.GetInt32(0);
                                code.Name = dr.GetString(1);
                                code.CodeString = dr.GetString(2);
                                codes.Add(code);
                            }
                            dr.Close();
                            _conn.Close();
                            return codes;
                        }
                        else
                        {
                            dr.Close();
                            _conn.Close();
                            return new List<Code>();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error retrieving codes: \n\n" + e.Message);
                return null;
            }
        }

        public static bool DeleteCode(Code code)
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Codes WHERE Id = @p1", _conn))
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@p1", code.Id);
                        if (cmd.ExecuteNonQuery() != 0)
                        {
                            _conn.Close();
                            return true;
                        }
                        else
                        {
                            _conn.Close();
                            return false;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error deleting code: \n\n" + e.Message);
                return false;
            }
        }

        public static bool LoadSettings()
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Settings LIMIT 1", _conn))
                    {
                        cmd.Connection.Open();
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            Settings.AnnounceJoin = dr.GetBoolean(1);
                            Settings.AnnounceLeave = dr.GetBoolean(2);
                            Settings.ThankFollowers = dr.GetBoolean(3);
                            Settings.ThankSubscribers = dr.GetBoolean(4);
                            Settings.WhisperWinners = dr.GetBoolean(5);
                            Settings.AnnounceBan = dr.GetBoolean(6);
                            Settings.AnnounceNowHosting = dr.GetBoolean(7);
                            Settings.WhisperCode = dr.GetBoolean(8);
                            Settings.AuthToken = dr.GetString(9);
                            Settings.ChannelName = dr.GetString(10);
                            Settings.SLAuthToken = dr.GetString(11);
                            Settings.Messages.mUserJoins = dr.GetString(12);
                            Settings.Messages.mUserLeaves = dr.GetString(13);
                            Settings.Messages.mUserFollows = dr.GetString(14);
                            Settings.Messages.mUserSubscribes = dr.GetString(15);
                            Settings.Messages.mUserBanned = dr.GetString(16);
                            Settings.Messages.mNowHosting = dr.GetString(17);
                            dr.Close();
                            _conn.Close();
                            return true;
                        }
                        else
                        {
                            dr.Close();
                            _conn.Close();
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Loading Settings: \n\n" + e.Message);
                return false;
            }
        }

        public static bool SaveSettings()
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("UPDATE Settings SET AnnounceJoin = @p1,AnnounceLeave = @p2,ThankFollowers = @p3,ThankSubscribers = @p4,WhisperWinners = @p5,AnnounceBan = @p6, AnnounceNowHosting = @p7,WhisperCode = @p8,AuthToken = @p9,ChannelName = @p10,SLAuthToken = @p11,mUserJoins = @p12,mUserLeaves = @p13,mUserFollows = @p14,mUserSubscribes = @p15,mUserBanned = @p16,mNowHosting = @p17 WHERE Id=1", _conn))
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@p1", Settings.AnnounceJoin);
                        cmd.Parameters.AddWithValue("@p2", Settings.AnnounceLeave);
                        cmd.Parameters.AddWithValue("@p3", Settings.ThankFollowers);
                        cmd.Parameters.AddWithValue("@p4", Settings.ThankSubscribers);
                        cmd.Parameters.AddWithValue("@p5", Settings.WhisperWinners);
                        cmd.Parameters.AddWithValue("@p6", Settings.AnnounceBan);
                        cmd.Parameters.AddWithValue("@p7", Settings.AnnounceNowHosting);
                        cmd.Parameters.AddWithValue("@p8", Settings.WhisperCode);
                        cmd.Parameters.AddWithValue("@p9", Settings.AuthToken);
                        cmd.Parameters.AddWithValue("@p10", Settings.ChannelName);
                        cmd.Parameters.AddWithValue("@p11", Settings.SLAuthToken);
                        cmd.Parameters.AddWithValue("@p12", Settings.Messages.mUserJoins);
                        cmd.Parameters.AddWithValue("@p13", Settings.Messages.mUserLeaves);
                        cmd.Parameters.AddWithValue("@p14", Settings.Messages.mUserFollows);
                        cmd.Parameters.AddWithValue("@p15", Settings.Messages.mUserSubscribes);
                        cmd.Parameters.AddWithValue("@p16", Settings.Messages.mUserBanned);
                        cmd.Parameters.AddWithValue("@p17", Settings.Messages.mNowHosting);
                        int dr = cmd.ExecuteNonQuery();
                        _conn.Close();
                        if (dr > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error saving settings: \n\n" + e.Message);
                return false;
            }
        }


        public static List<Giveaway> GetGiveaways()
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Giveaways", _conn))
                    {
                        cmd.Connection.Open();
                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            List<Giveaway> giveaways = new List<Giveaway>();
                            while (dr.Read())
                            {
                                Giveaway giveaway = new Giveaway();
                                giveaway.Id = dr.GetInt32(0);
                                giveaway.Name = dr.GetString(1);
                                giveaway.Reward = dr.GetInt32(2);
                                giveaway.RewardString = dr.GetString(3);
                                giveaway.Type = dr.GetInt32(4);
                                giveaway.Hashtag = dr.GetString(5);
                                giveaway.Repeat = dr.GetInt32(6);
                                giveaway.Delay = dr.GetInt32(7);
                                giveaway.StartMessage = dr.GetString(8);
                                giveaway.WinnerMessage = dr.GetString(9);
                                giveaway.WinnerWhisper = dr.GetString(10);
                                giveaway.CodeWhisper = dr.GetString(11);
                                giveaway.AnnounceStart = dr.GetBoolean(12);
                                giveaway.AutoPickCode = dr.GetBoolean(13);
                                giveaways.Add(giveaway);
                            }
                            dr.Close();
                            _conn.Close();
                            return giveaways;
                        }
                        else
                        {
                            dr.Close();
                            _conn.Close();
                            return new List<Giveaway>();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error retrieving giveaways: \n\n" + e.Message);
                return null;
            }
        }

        internal static void SetupTable()
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS 'Settings'('AnnounceJoin'  INTEGER,'AnnounceLeave' INTEGER,'ThankFollowers'    INTEGER,'ThankSubscribers'  INTEGER,'WhisperWinners'    INTEGER,'AnnounceBan'   INTEGER,'AnnounceNowHosting'    INTEGER,'WhisperCode'   INTEGER,'AuthToken' TEXT,'ChannelName'   TEXT,'SLAuthToken'   TEXT,'mUserJoins'    TEXT,'mUserLeaves'   TEXT,'mUserFollows'  TEXT,'mUserSubscribes'   TEXT,'mUserBanned'   TEXT,'mNowHosting'   TEXT,'Id'    INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE);CREATE TABLE IF NOT EXISTS 'Codes'('Id'    INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,'Name'  TEXT,'CodeString'    TEXT);CREATE TABLE IF NOT EXISTS 'Giveaways'('Id'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,'Name'  TEXT NOT NULL,'Reward'    INTEGER NOT NULL,'RewardString'  TEXT NOT NULL,'Type'  INTEGER NOT NULL,'Hashtag'   TEXT,'AutoRoll'  INTEGER NOT NULL,'Repeat'    INTEGER NOT NULL,'Delay' INTEGER NOT NULL,'StartMessage'  TEXT,'WinnerMessage' TEXT,'WinnerWhisper' TEXT,'CodeWhisper'   TEXT,'AnnounceStart' INTEGER);", _conn))
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        _conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error on table setup: \n\n" + e.Message);
                throw;
            }
        }

        public static Giveaway GetGiveawayByName(string name)
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Giveaways WHERE Name = @p1", _conn))
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@p1", name);

                        SQLiteDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            Giveaway giveaway = new Giveaway();
                            giveaway.Id = dr.GetInt32(0);
                            giveaway.Name = dr.GetString(1);
                            giveaway.Reward = dr.GetInt32(2);
                            giveaway.RewardString = dr.GetString(3);
                            giveaway.Type = dr.GetInt32(4);
                            giveaway.Hashtag = dr.GetString(5);
                            giveaway.Repeat = dr.GetInt32(6);
                            giveaway.Delay = dr.GetInt32(7);
                            giveaway.StartMessage = dr.GetString(8);
                            giveaway.WinnerMessage = dr.GetString(9);
                            giveaway.WinnerWhisper = dr.GetString(10);
                            giveaway.CodeWhisper = dr.GetString(11);
                            dr.Close();
                            _conn.Close();
                            return giveaway;
                        }
                        else
                        {
                            dr.Close();
                            _conn.Close();
                            return new Giveaway();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error retrieving giveaway: \n\n" + e.Message);
                return null;
            }
        }

        public static bool CreateGiveaway(Giveaway g)
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Giveaways (Name,Reward,RewardString,Type,Hashtag,Repeat,Delay,StartMessage,WinnerMessage,WinnerWhisper,CodeWhisper,AnnounceStart,AutoPickCode) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13)", _conn))
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@p1", g.Name);
                        cmd.Parameters.AddWithValue("@p2", g.Reward);
                        cmd.Parameters.AddWithValue("@p3", g.RewardString);
                        cmd.Parameters.AddWithValue("@p4", g.Type);
                        cmd.Parameters.AddWithValue("@p5", g.Hashtag);
                        cmd.Parameters.AddWithValue("@p6", g.Repeat);
                        cmd.Parameters.AddWithValue("@p7", g.Delay);
                        cmd.Parameters.AddWithValue("@p8", g.StartMessage);
                        cmd.Parameters.AddWithValue("@p9", g.WinnerMessage);
                        cmd.Parameters.AddWithValue("@p10", g.WinnerWhisper);
                        cmd.Parameters.AddWithValue("@p11", g.CodeWhisper);
                        cmd.Parameters.AddWithValue("@p12", g.AnnounceStart);
                        cmd.Parameters.AddWithValue("@p13", g.AutoPickCode);

                        if (cmd.ExecuteNonQuery() != 0)
                        {
                            _conn.Close();
                            return true;
                        }
                        else
                        {
                            _conn.Close();
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error creating giveaway: \n\n" + e.Message);
                return false;
            }
        }

        public static bool DeleteGiveaway(Giveaway g)
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Giveaways WHERE Id = @p1", _conn))
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@p1", g.Id);

                        if (cmd.ExecuteNonQuery() != 0)
                        {
                            _conn.Close();
                            return true;
                        }
                        else
                        {
                            _conn.Close();
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error deleting giveaway: \n\n" + e.Message);
                return false;
            }
        }

        public static bool UpdateGiveaway(Giveaway g)
        {
            try
            {
                using (SQLiteConnection _conn = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand cmd = new SQLiteCommand("UPDATE Giveaways SET Name = @p1,Reward = @p2,RewardString = @p3,Type = @p4,Hashtag = @p5,Repeat = @p6,Delay = @p7,StartMessage = @p8,WinnerMessage = @p9,WinnerWhisper = @p10,CodeWhisper = @p11,AnnounceStart = @p12,AutoPickCode = @p13 WHERE Id = @p15", _conn))
                    {
                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@p1", g.Name);
                        cmd.Parameters.AddWithValue("@p2", g.Reward);
                        cmd.Parameters.AddWithValue("@p3", g.RewardString);
                        cmd.Parameters.AddWithValue("@p4", g.Type);
                        cmd.Parameters.AddWithValue("@p5", g.Hashtag);
                        cmd.Parameters.AddWithValue("@p6", g.Repeat);
                        cmd.Parameters.AddWithValue("@p7", g.Delay);
                        cmd.Parameters.AddWithValue("@p8", g.StartMessage);
                        cmd.Parameters.AddWithValue("@p9", g.WinnerMessage);
                        cmd.Parameters.AddWithValue("@p10", g.WinnerWhisper);
                        cmd.Parameters.AddWithValue("@p11", g.CodeWhisper);
                        cmd.Parameters.AddWithValue("@p12", g.AnnounceStart);
                        cmd.Parameters.AddWithValue("@p13", g.AutoPickCode);
                        cmd.Parameters.AddWithValue("@p14", g.Id);

                        if (cmd.ExecuteNonQuery() != 0)
                        {
                            _conn.Close();
                            return true;
                        }
                        else
                        {
                            _conn.Close();
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error updating giveaway: \n\n" + e.Message);
                return false;
            }
        }


    }


}
