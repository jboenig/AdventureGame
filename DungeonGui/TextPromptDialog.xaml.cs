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
    /// Interaction logic for TextPromptDialog.xaml
    /// </summary>
    public partial class TextPromptDialog : Window
    {
        public TextPromptDialog(string promptText)
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

        public string Response
        {
            get;
            set;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
