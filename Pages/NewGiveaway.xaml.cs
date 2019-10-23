using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for NewGiveaway.xaml
    /// </summary>
    public partial class NewGiveaway : Page
    {
        Giveaway g;
        bool editing = false;

        public NewGiveaway(Giveaway g)
        {
            InitializeComponent();
            this.g = g;
            editing = true;
            FillControlls();
        }

        private void FillControlls()
        {
            txtBoxName.Text = g.Name;
            comboBoxReward.SelectedIndex = g.Reward - 1;
            txtBoxReward.Text = g.RewardString;
            comboBoxGiveawayType.SelectedIndex = g.Type - 1;
            txtBoxRepeat.Text = g.Repeat.ToString();
            txtBoxDelay.Text = g.Delay.ToString();
            txtBoxHashtag.Text = g.Hashtag;
            richBoxGiveawayStart.Document.Blocks.Clear();
            richBoxGiveawayStart.Document.Blocks.Add(new Paragraph(new Run(g.StartMessage)));
            richBoxGiveawayWinner.Document.Blocks.Clear();
            richBoxGiveawayWinner.Document.Blocks.Add(new Paragraph(new Run(g.WinnerMessage)));
            richBoxWinnerWhisper.Document.Blocks.Clear();
            richBoxWinnerWhisper.Document.Blocks.Add(new Paragraph(new Run(g.WinnerWhisper)));
            richBoxCodeWhisper.Document.Blocks.Clear();
            richBoxCodeWhisper.Document.Blocks.Add(new Paragraph(new Run(g.CodeWhisper)));
            checkBoxAnnounceStart.IsChecked = g.AnnounceStart;
            checkBoxPickCode.IsChecked = g.AutoPickCode;
            //btnCreateUpdate.Content = "Update";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (editing)
            //    UpdateDb();
            //else
            //{
            //    int rewardtype = 0;
            //    int giveawaytype = 0;
            //    bool autoroll = checkBoxAutoRoll.IsChecked.Value;
            //    if (rbCode.IsChecked.Value)
            //        rewardtype = 1;
            //    else if (rbOther.IsChecked.Value)
            //        rewardtype = 2;
            //    if (rbHashtag.IsChecked.Value)
            //        giveawaytype = 1;
            //    else if (rbHashtagFollowers.IsChecked.Value)
            //        giveawaytype = 2;
            //    else if (rbFollowers.IsChecked.Value)
            //        giveawaytype = 3;
            //    else if (rbAllViewers.IsChecked.Value)
            //        giveawaytype = 4;
            //    Giveaway giveaway = new Giveaway();
            //    giveaway.Name = txtBoxName.Text;
            //    giveaway.Reward = rewardtype;
            //    giveaway.RewardString = txtBoxReward.Text;
            //    giveaway.Type = giveawaytype;
            //    giveaway.Hashtag = txtBoxHashtag.Text;
            //    giveaway.AutoRoll = autoroll;
            //    giveaway.Repeat = int.Parse(txtBoxRepeat.Text);
            //    giveaway.Delay = int.Parse(txtBoxDelay.Text);
            //    giveaway.StartMessage = new TextRange(richBoxGiveawayStart.Document.ContentStart, richBoxGiveawayStart.Document.ContentEnd).Text;
            //    giveaway.WinnerMessage = new TextRange(richBoxGiveawayWinner.Document.ContentStart, richBoxGiveawayWinner.Document.ContentEnd).Text;
            //    giveaway.WinnerWhisper = new TextRange(richBoxWinnerWhisper.Document.ContentStart, richBoxWinnerWhisper.Document.ContentEnd).Text;
            //    giveaway.CodeWhisper = new TextRange(richBoxCodeWhisper.Document.ContentStart, richBoxCodeWhisper.Document.ContentEnd).Text;
            //    giveaway.AnnounceStart = checkBoxAnnounceStart.IsChecked.Value;
            //    giveaway.AutoPickCode = checkBoxPickCode.IsChecked.Value;
            //    DAL.CreateGiveaway(giveaway);
            //    (Owner as MainWindow).UpdateGiveaways();
            //    this.Close();
            //}
        }

        private void UpdateDb()
        {
            //int rewardtype = 0;
            //int giveawaytype = 0;
            //bool autoroll = checkBoxAutoRoll.IsChecked.Value;
            //if (rbCode.IsChecked.Value)
            //    rewardtype = 1;
            //else if (rbOther.IsChecked.Value)
            //    rewardtype = 2;
            //if (rbHashtag.IsChecked.Value)
            //    giveawaytype = 1;
            //else if (rbHashtagFollowers.IsChecked.Value)
            //    giveawaytype = 2;
            //else if (rbFollowers.IsChecked.Value)
            //    giveawaytype = 3;
            //else if (rbAllViewers.IsChecked.Value)
            //    giveawaytype = 4;
            //g.Name = txtBoxName.Text;
            //g.Reward = rewardtype;
            //g.RewardString = txtBoxReward.Text;
            //g.Type = giveawaytype;
            //g.Hashtag = txtBoxHashtag.Text;
            //g.AutoRoll = autoroll;
            //g.Repeat = int.Parse(txtBoxRepeat.Text);
            //g.Delay = int.Parse(txtBoxDelay.Text);
            //g.StartMessage = new TextRange(richBoxGiveawayStart.Document.ContentStart, richBoxGiveawayStart.Document.ContentEnd).Text;
            //g.WinnerMessage = new TextRange(richBoxGiveawayWinner.Document.ContentStart, richBoxGiveawayWinner.Document.ContentEnd).Text;
            //g.WinnerWhisper = new TextRange(richBoxWinnerWhisper.Document.ContentStart, richBoxWinnerWhisper.Document.ContentEnd).Text;
            //g.CodeWhisper = new TextRange(richBoxCodeWhisper.Document.ContentStart, richBoxCodeWhisper.Document.ContentEnd).Text;
            //g.AnnounceStart = checkBoxAnnounceStart.IsChecked.Value;
            //g.AutoPickCode = checkBoxPickCode.IsChecked.Value;
            //DAL.UpdateGiveaway(g);
            //(Owner as MainWindow).UpdateGiveaways();
            //this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            if (!int.TryParse(e.Text, out int i))
                (sender as TextBox).Text = "0";
        }
        private void NumberValidationTextBox(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse((sender as TextBox).Text, out int i))
                (sender as TextBox).Text = "0";
        }
    }
}
