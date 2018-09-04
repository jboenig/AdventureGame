using System;
using System.Windows.Controls;
using Syncfusion.Windows.Forms.Diagram;
using AdventureGameEngine;

namespace AdventureGameGui
{
    /// <summary>
    /// Interaction logic for ActionControl.xaml
    /// </summary>
    public partial class RoomControl : UserControl
    {
        private RoomModel model;
        private RoomView view;
        private RoomController controller;

        public RoomControl()
        {
            InitializeComponent();

            this.roomCanvas.SizeChanged += RoomCanvas_SizeChanged;
            this.roomCanvas.Paint += RoomCanvas_Paint;
        }

        public void LoadRoom(Room room)
        {
            this.model = new RoomModel(room);
            this.view = new RoomView();
            this.controller = new RoomController();

            if (this.model != null)
            {
                this.model.MeasurementUnits = System.Drawing.GraphicsUnit.Inch;

                this.model.Bounds = new System.Drawing.RectangleF(0, 0, 5, 5);
                this.model.BackgroundStyle.Color = System.Drawing.Color.Black;

                //this.model.PageSettings = this.pageSettings;
                //this.model.BoundsChanged += new BoundsEventHandler(OnModelBoundsChange);
                //this.model.ChildrenChanging += new NodeCollection.EventHandler(OnModelChildrenChanging);
                //this.model.ChildrenChangeComplete += new NodeCollection.EventHandler(OnModelChildrenChangeComplete);
                //this.model.PropertyChanged += new PropertyEventHandler(OnModelPropertyChanged);
            }

            if (this.view != null)
            {
                this.view.Initialize(this.roomCanvas);
                this.view.Location = new System.Drawing.Point(0, 0);
                this.view.Size = new System.Drawing.Size(this.roomCanvas.Width, this.roomCanvas.Height);
                this.view.Model = this.model;
                this.view.BackgroundColor = System.Drawing.Color.Black;
                //this.view.OriginChanged += new ViewOriginEventHandler(OnViewOriginChanged);
            }

            if (this.controller != null)
            {
                this.controller.Initialize(this.view);

                // Wire up event handler to listen for changes in the selection list
                //NodeCollection selectionList = this.controller.SelectionList;
                //if (selectionList != null)
                //{
                //    selectionList.ChangeComplete += new NodeCollection.EventHandler(OnSelectionListChanged);
                //}

                //this.controller.ToolActivate += new Controller.ToolEventHandler(this.OnToolActivate);
                //this.controller.ToolDeactivate += new Controller.ToolEventHandler(this.OnToolDeactivate);
            }
        }

        private void RoomCanvas_SizeChanged(object sender, EventArgs e)
        {
            if (this.view != null)
            {
                this.view.Size = new System.Drawing.Size(this.roomCanvas.Width, this.roomCanvas.Height);
            }
        }

        private void RoomCanvas_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (this.view != null)
            {
                this.view.Draw();
            }
        }
    }
}
