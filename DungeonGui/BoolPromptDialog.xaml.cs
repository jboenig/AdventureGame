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

namespace AdventureGameGui
{
    /// <summary>
    /// Interaction logic for BoolPromptDialog.xaml
    /// </summary>
    public partial class BoolPromptDialog : Window
    {
        public BoolPromptDialog(string promptText)
        {
            this.DataContext = this;
            this.Prompt = promptText;
            InitializeComponent();
        }

        public string Prompt
        {
            get;
            set;
        }

        public bool Response
        {
            get;
            set;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.Response = true;
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Response = false;
            this.DialogResult = true;
            this.Close();
        }
    }
}
