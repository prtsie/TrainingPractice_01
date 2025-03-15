namespace BVV_Task_8
{
    public class Square
    {
        private State state;
        public Rectangle Position { get; set; }

        public State State
        {
            get => state;
            set
            {
                if (value == state)
                {
                    return;
                }

                Count = 0;
                state = value;
            }
        }

        public int Count { get; set; }

        public void Draw(BufferedGraphics graphics)
        {
            var brush = State switch
            {
                State.Infected => Brushes.DarkRed,
                State.Immunity => Brushes.LightBlue,
                _ => Brushes.White
            };
            graphics.Graphics.FillRectangle(brush, Position);
            graphics.Graphics.DrawRectangle(Pens.Black, Position);
        }
    }
}
