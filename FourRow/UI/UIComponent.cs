using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FourRow.UI
{
    public class UIComponent
    {
        public Point Position { get; set;}
        public Size Size { get; set; }

        public int Width { get { return this.Size.Width; } }
        public int Height { get { return this.Size.Height; } }

        public Rectangle Bounds { get { return new Rectangle(this.Position, this.Size); } }

        public bool ContainsPoint(Point p) {
            return Bounds.Contains(p);
        }
    }
}
