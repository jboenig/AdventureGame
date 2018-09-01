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
    /// Interaction logic for MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        private Grid playerIcon;

        public MapControl()
        {
            InitializeComponent();
            this.InitPlayerIcon();
        }

        public Game Game
        {
            get { return this.DataContext as Game; }
        }

        public void UpdateMap()
        {
            this.playerIcon.SetValue(Grid.RowProperty, this.Game.Player.Position.Row);
            this.playerIcon.SetValue(Grid.ColumnProperty, this.Game.Player.Position.Column);

            // Add a grid cell for each entry in the map
            this.Game.GameBoard.ForeachPosition(p =>
            {
                var gridMapEntry = this.mapGrid.Children.Cast<Grid>().First(e => Grid.GetRow(e) == p.Row && Grid.GetColumn(e) == p.Column);
                var mapEntry = this.Game.GameBoard.GetEntry(p);
                this.UpdateGridMapEntry(mapEntry, gridMapEntry);
            });
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "DataContext")
            {
                this.InitMapGrid();
            }
            base.OnPropertyChanged(e);
        }

        private void InitMapGrid()
        {
            var game = this.Game;
            if (game == null)
            {
                throw new InvalidOperationException("DataContext is not set to a Game object");
            }

            this.mapGrid.RowDefinitions.Clear();
            this.mapGrid.ColumnDefinitions.Clear();
            this.mapGrid.Children.Clear();

            var gameMap = game.GameBoard;

            // Add a row to the grid for each row in the map
            for (int row = 0; row < this.Game.GameBoard.RowCount; row++)
            {
                var rowDef = new RowDefinition();
                this.mapGrid.RowDefinitions.Add(rowDef);
            }

            // Add a column to the grid for each column in the map
            for (int col = 0; col < this.Game.GameBoard.ColumnCount; col++)
            {
                var colDef = new ColumnDefinition();
                this.mapGrid.ColumnDefinitions.Add(colDef);
            }

            // Add a grid cell for each entry in the map
            gameMap.ForeachPosition(p =>
            {
                var gridMapEntry = new Grid();
                gridMapEntry.ShowGridLines = false;
                gridMapEntry.Margin = new Thickness(-1);
                gridMapEntry.SetValue(Grid.RowProperty, p.Row);
                gridMapEntry.SetValue(Grid.ColumnProperty, p.Column);

                var mapEntry = gameMap.GetEntry(p);
                this.UpdateGridMapEntry(mapEntry, gridMapEntry);
                this.mapGrid.Children.Add(gridMapEntry);
            });

            // Add the player icon to the grid
            this.mapGrid.Children.Add(this.playerIcon);
            this.playerIcon.SetValue(Grid.RowProperty, this.Game.Player.Position.Row);
            this.playerIcon.SetValue(Grid.ColumnProperty, this.Game.Player.Position.Column);
        }

        private void InitPlayerIcon()
        {
            var tbPlayerIcon = new TextBlock();
            tbPlayerIcon.Text = "@";
            tbPlayerIcon.TextAlignment = TextAlignment.Center;
            tbPlayerIcon.Foreground = Brushes.White;
            this.playerIcon = new Grid();
            this.playerIcon.Children.Add(tbPlayerIcon);
        }

        private void UpdateGridMapEntry(Tile mapEntry, Grid gridMapEntry)
        {
            Brush cellBackgroundBrush = Brushes.Blue;

            if (mapEntry.IsAccessible(this.Game.Player))
            {
                if (mapEntry.HasVisited(this.Game.Player))
                {
                    cellBackgroundBrush = Brushes.Black;

                    var room = mapEntry as Room;
                    if (room != null)
                    {
                        var hasPortals = room.GetFeatures<Portal>().Count() > 0;
                        if (hasPortals)
                        {
                            cellBackgroundBrush = Brushes.Green;
                        }
                    }
                }
                else
                {
                    cellBackgroundBrush = Brushes.DarkGray;
                }
            }

            gridMapEntry.Background = cellBackgroundBrush;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                this.Game.ExecuteCommand("move north");
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                this.Game.ExecuteCommand("move south");
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                this.Game.ExecuteCommand("move west");
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                this.Game.ExecuteCommand("move east");
                e.Handled = true;
            }
        }
    }
}
