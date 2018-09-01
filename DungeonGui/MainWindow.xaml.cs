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

using AdventureGameEngine;

namespace AdventureGameGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IGameHost, IUserPromptService
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize Ninject kernel and bindings
            NInjectHelper.Initialize(this);

            // Initialize Syncfusion Diagram
            Syncfusion.Windows.Forms.Diagram.Global.Initialize();

            // Get the game host and run the game
            var gameHost = NInjectHelper.Get<IGameHost>();
            gameHost.Run(NInjectHelper.Get<Game>());
        }

        private void Game_CommandExecuted(object sender, EventArgs e)
        {
            if (this.game.PlayerPosition.IsUndefined)
            {
                var playAgain = this.PromptBool("You've won", "Would you like to play again?");
                if (playAgain)
                {
                    this.Reset();
                }
                else
                {
                    this.Exit();
                }
            }
            else if (this.game.Player.IsDead())
            {
                var playAgain = this.PromptBool("You're dead", "Would you like to play again?");
                if (playAgain)
                {
                    this.Reset();
                }
                else
                {
                    this.Exit();
                }
            }
            else
            {
                this.mapControl.UpdateMap();
            }
        }

        #region IGameHost Interface

        public void Run(Game game)
        {
            this.game = game;
            this.game.CreateGameBoard("CrossPattern");
            this.game.CommandExecuted += Game_CommandExecuted;
            this.consoleIO.Init(this.game);

            // Bind player inventory to inventory control
            this.inventoryCtl.DataContext = this.game.Player.Inventory;
            this.healthCtl.DataContext = this.game.Player;
            this.mapControl.DataContext = this.game;

            // Display game intro
            this.game.DisplayGameIntro();
            this.game.DisplayCurrentRoomDescription();
        }

        public void Reset()
        {
            this.game.Reset();

            // Bind player inventory to inventory control
            this.inventoryCtl.DataContext = this.game.Player.Inventory;
            this.healthCtl.DataContext = this.game.Player;
            this.mapControl.DataContext = this.game;

            // Display game intro
            this.game.DisplayGameIntro();
            this.game.DisplayCurrentRoomDescription();

            this.consoleIO.Clear();
            this.mapControl.UpdateMap();
        }

        public void Exit()
        {
            this.Close();
        }

        #endregion

        #region IUserPromptService Implementation

        public bool PromptBool(string dlgTitle, string displayText)
        {
            var dlg = new BoolPromptDialog(displayText);
            dlg.Title = dlgTitle;
            var res = dlg.ShowDialog();
            if (res == true)
            {
                return dlg.Response;
            }
            return false;
        }

        public string PromptText(string dlgTitle, string displayText)
        {
            var dlg = new TextPromptDialog(displayText);
            dlg.Title = dlgTitle;
            var res = dlg.ShowDialog();
            if (res == true)
            {
                return dlg.Response;
            }
            return string.Empty;
        }

        #endregion
    }
}

