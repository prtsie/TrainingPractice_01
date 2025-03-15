using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BVV_Task_7
{
    internal class Square
    {
        public Rectangle Position { get; set; }

        public State State { get; set; }

        public void Draw(BufferedGraphics graphics)
        {
            graphics.Graphics.DrawRectangle(Program.borderPen, Position);
            var center = new Point(Position.Left + Position.Width / 2, Position.Top + Position.Height / 2);
            var str = State switch
            {
                State.Chicken => "к",
                State.Fox => "л",
                _ => string.Empty
            };
            var strSize = graphics.Graphics.MeasureString(str, SystemFonts.DefaultFont);
            var fontPos = new Point(center.X - (int)(strSize.Width / 2), center.Y - (int)(strSize.Height / 2));
            graphics.Graphics.DrawString(str, SystemFonts.DefaultFont, Program.textBrush, fontPos);
        }
    }
}
