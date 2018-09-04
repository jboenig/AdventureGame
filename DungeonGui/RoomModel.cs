using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Windows.Forms.Diagram;
using AdventureGameEngine;
using Dungeon;

namespace AdventureGameGui
{
    public class RoomModel : Model
    {
        private readonly Room room;

        public RoomModel(Room room)
        {
            this.room = room;
            this.Init();
        }

        private void Init()
        {
            var backgroundImg = this.LoadGraphic("Cave");
            this.BackgroundStyle.Texture = backgroundImg.Image;

            if (this.room != null)
            {
                var curX = 0.2f;

                foreach (var curCharacter in this.room.GetEnemies())
                {
                    var bmpNode = this.LoadGraphic(curCharacter.Name);
                    bmpNode.Bounds = new System.Drawing.RectangleF(curX, 0.2f, 60.0f, 70.0f);
                    this.AppendChild(bmpNode);
                    curX += bmpNode.Width + 10.0f;
                }

                foreach (var curCharacter in this.room.GetPlayerFollowers())
                {
                    var bmpNode = this.LoadGraphic(curCharacter.Name);
                    bmpNode.Bounds = new System.Drawing.RectangleF(curX, 2.0f, 60.0f, 70.0f);
                    this.AppendChild(bmpNode);
                    curX += bmpNode.Width + 10.0f;
                }
            }
        }

        private BitmapNode LoadGraphic(string imgName)
        {
            var assembly = this.GetType().Assembly;
            var resourceName = string.Format("AdventureGameGui.Resources.{0}.png", imgName);

            Stream stream = null;

            try
            {
                stream = assembly.GetManifestResourceStream(resourceName);
            }
            catch
            {
                stream = null;
            }

            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("AdventureGameGui.Resources.DefaultCharacter.png");
            }

            using (stream)
            {
                return new BitmapNode(stream);
            }
        }
    }
}
