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
using System.Windows.Shapes;
using WpfApp1.Classes;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Lógica interna para CodeCreateWindow.xaml
    /// </summary>
    public partial class CodeCreateWindow : Window
    {
        public CodeCreateWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text) || !string.IsNullOrEmpty(txtName.Text))
            {
                Code code = new Code();
                code.Name = txtName.Text;
                code.CodeString = txtCode.Text;
                DAL.AddCode(code);
                (this.Owner as MainWindow).UpdateCodes();
            }
            else
            {
                MessageBox.Show("Error: You must fill all parameters.");
                txtCode.Clear();
                txtName.Clear();
            }
        }
    }
}
