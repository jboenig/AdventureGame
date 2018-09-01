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
using Syncfusion.Windows.Forms.Diagram;
using AdventureGameEngine;

namespace AdventureGameGui
{
    /// <summary>
    /// Interaction logic for MapControl2.xaml
    /// </summary>
    public partial class MapControl2 : UserControl
    {
        private DiagramController minimapController;
        private Syncfusion.Windows.Forms.Diagram.View minimapView;
        private Syncfusion.Windows.Forms.Diagram.Model minimapModel;

        public MapControl2()
        {
            InitializeComponent();

            var panelMinimap = new System.Windows.Forms.Panel();
            panelMinimap.BackColor = System.Drawing.Color.Aqua;
            this.miniMapWinFormHost.Child = panelMinimap;
            this.minimapView = new View(panelMinimap);
            this.minimapController = new DiagramController();
            this.minimapController.Initialize(this.minimapView);
            this.minimapModel = new Syncfusion.Windows.Forms.Diagram.Model();
            minimapModel.BackgroundStyle.Color = System.Drawing.Color.Green;
            this.minimapView.Model = this.minimapModel;
        }

        public Game Game
        {
            get { return this.DataContext as Game; }
        }

        public void UpdateMap()
        {
        }
    }
}
