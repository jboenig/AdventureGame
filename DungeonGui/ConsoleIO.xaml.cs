using System;
using System.Windows.Controls;
using System.Windows.Input;
using AdventureGameEngine;

namespace AdventureGameGui
{
    /// <summary>
    /// Interaction logic for ConsoleIO.xaml
    /// </summary>
    public partial class ConsoleIO : UserControl, IConsoleOutputService
    {
        private ICommandProcessor commandProcessor;
        private int historyIdx;

        public ConsoleIO()
        {
            InitializeComponent();
        }

        public void Init(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        public void Write(string msg)
        {
            this.outputTextBlock.Text += msg;
        }

        public void Write(char c)
        {
            this.outputTextBlock.Text += c;
        }

        public void WriteLine()
        {
            this.outputTextBlock.Text += "\r";
        }

        public void WriteLine(string msg)
        {
            this.outputTextBlock.Text += (msg + "\r");
        }

        public void Clear()
        {
            this.outputTextBlock.Text = string.Empty;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var cmdHistory = this.commandProcessor.CommandHistory;

            if (e.Key == Key.Return)
            {
                var cmdLine = this.cmdTextBox.Text;
                this.outputTextBlock.Text += "\r> ";
                this.outputTextBlock.Text += cmdLine;
                this.outputTextBlock.Text += "\r";
                this.cmdTextBox.Text = string.Empty;
                if (!this.commandProcessor.ExecuteCommand(cmdLine))
                {
                    // Command line is invalid
                    this.WriteLine("That is not a valid command - type help for a list of commands");
                }
                this.outputTextScrollViewer.ScrollToBottom();
                this.historyIdx = cmdHistory.Count;
            }
            else if (e.Key == Key.Up)
            {
                if (this.historyIdx > 0)
                {
                    this.historyIdx = this.historyIdx - 1;
                    this.cmdTextBox.Clear();
                    this.cmdTextBox.Text = cmdHistory[this.historyIdx];
                    this.cmdTextBox.CaretIndex = this.cmdTextBox.Text.Length;
                }
            }
            else if (e.Key == Key.Down)
            {
                if (this.historyIdx < cmdHistory.Count - 1)
                {
                    this.historyIdx = this.historyIdx + 1;
                    this.cmdTextBox.Clear();
                    this.cmdTextBox.Text = cmdHistory[this.historyIdx];
                    this.cmdTextBox.CaretIndex = this.cmdTextBox.Text.Length;
                }
                else
                {
                    this.historyIdx = cmdHistory.Count;
                    this.cmdTextBox.Clear();
                }
            }
        }
    }
}
