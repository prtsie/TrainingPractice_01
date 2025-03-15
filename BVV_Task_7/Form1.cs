using Microsoft.VisualBasic.Devices;
using System;

namespace BVV_Task_7
{
    public partial class Form1 : Form
    {
        private const int squareSize = 50;
        private readonly Brush selectedBrush = Brushes.Aqua;
        private readonly Brush movesBrush = Brushes.BlanchedAlmond;
        private readonly Brush foxMovesBrush = Brushes.Maroon;
        private const int horizontalOffset = 100;
        private const int verticalOffset = 50;
        private readonly Square?[,] squares = new Square[7, 7];
        private readonly Square?[] squaresOneD = new Square[7 * 7];
        private Square? selected;
        private readonly BufferedGraphics buffer;
        private List<(Square move, Square? eaten)> foxMovesToShow = [];
        private Square? movingFox;
        private int foxMoveDelayMs = 1000;
        private Action? drawMovingFox;
        private bool? playerWon;

        public Form1()
        {
            InitializeComponent();
            Width = horizontalOffset * 2 + squareSize * 7;
            Height = verticalOffset * 2 + squareSize * 7;
            MinimumSize = MaximumSize = new(Width, Height);
            buffer = BufferedGraphicsManager.Current.Allocate(CreateGraphics(), DisplayRectangle);

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if ((i < 2 || i > 4) && (j < 2 || j > 4))
                    {
                        squares[i, j] = null;
                        squaresOneD[i * 7 + j] = null;
                        continue;
                    }

                    squares[i, j] = new Square()
                    {
                        Position = new Rectangle(horizontalOffset + squareSize * j, verticalOffset + squareSize * i, squareSize, squareSize),
                    };
                    squaresOneD[i * 7 + j] = squares[i, j];

                    if (i > 2)
                    {
                        squares[i, j]!.State = State.Chicken;
                    }
                }
            }
            squares[2, 2]!.State = State.Fox;
            squares[2, 4]!.State = State.Fox;
        }

        private void DrawField()
        {
            foreach (var square in squares)
            {
                square?.Draw(buffer);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            buffer.Graphics.Clear(Color.White);

            if (playerWon is not null)
            {
                if (playerWon.Value)
                {
                    buffer.Graphics.DrawString("Вы выиграли!!!", SystemFonts.DefaultFont, Brushes.Green, new PointF(horizontalOffset, verticalOffset));
                }
                else
                {
                    buffer.Graphics.DrawString("Вы проиграли!!!", SystemFonts.DefaultFont, Brushes.Red, new PointF(horizontalOffset, verticalOffset));
                }
            }

            if (squaresOneD.Count(x => x?.State == State.Chicken) < 9)
            {
                playerWon = false;
            }
            else
            {
                var counter = 0;
                for (var i = 0; i < 2; i++)
                {
                    for(var j = 2; j < 5; j++)
                    {
                        if (squares[i, j]?.State == State.Chicken)
                        {
                            counter++;
                        }
                    }
                }
                if (counter == 6)
                {
                    playerWon = true;
                }
            }

            FillSelected();
            drawMovingFox?.Invoke();
            DrawField();
            buffer.Render();
        }

        private void FillSelected()
        {
            if (selected != null)
            {
                buffer.Graphics.FillRectangle(selectedBrush, selected.Position);
                foreach (var square in GetChickenMoves(selected))
                {
                    buffer.Graphics.FillRectangle(movesBrush, square.Position);
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (foxMovesToShow.Count > 0 || playerWon is not null)
            {
                return;
            }

            var clicked = squaresOneD.FirstOrDefault(x => x is not null && x.Position.Contains(e.Location));

            if (selected == null)
            {
                if (clicked is { State: State.Chicken })
                {
                    selected = clicked;
                }
            }
            else if (clicked is not null)
            {
                if (clicked.State is State.Chicken)
                {
                    selected = clicked;
                    return;
                }

                var moves = GetChickenMoves(selected);
                if (moves.Contains(clicked))
                {
                    selected.State = State.Empty;
                    clicked.State = State.Chicken;
                    selected = null;
                    MoveFox();
                }
            }
            else
            {
                selected = null;
            }
        }

        private void MoveFox()
        {
            var foxes = squaresOneD.Where(x => x?.State is State.Fox).ToList();
            Square fox;
            List<(Square move, Square? eaten)> moves;
            do
            {
                fox = foxes[Random.Shared.Next(foxes.Count)]!;
                moves = GetFoxMove(fox!);
                foxes.Remove(fox);
            } while (moves.Count == 0 && foxes.Count > 0);
            if (moves.Count == 0)
            {
                var emptyNeighbours = GetNeighbours(fox!).Where(x => x.State == State.Empty).ToList();
                moves = [(emptyNeighbours[Random.Shared.Next(emptyNeighbours.Count)], null)];
            }
            foxMovesToShow.AddRange(moves);
            movingFox = fox;
            Task.Run(async () =>
            {
                while (foxMovesToShow.Count > 0)
                {
                    drawMovingFox = () =>
                    {
                        buffer.Graphics.FillRectangle(foxMovesBrush, foxMovesToShow[0].move.Position);
                        buffer.Graphics.FillRectangle(selectedBrush, movingFox!.Position);
                    };
                    await Task.Delay(foxMoveDelayMs);
                    movingFox!.State = State.Empty;
                    movingFox = foxMovesToShow[0].move;
                    movingFox!.State = State.Fox;
                    if (foxMovesToShow[0].eaten is { } eaten)
                    {
                        eaten.State = State.Empty; 
                    }
                    foxMovesToShow.RemoveAt(0);
                }
                drawMovingFox = null;
                movingFox = null;
            });
        }


        private (int row, int col)? GetIndexOf(Square square)
        {
            for (var i = 0; i < 7; i++)
            {
                for (var j = 0; j < 7; j++)
                {
                    if (squares[i, j]?.Equals(square) == true)
                    {
                        return (i, j);
                    }
                }
            }
            return null;
        }

        private List<Square> GetChickenMoves(Square square)
        {
            var result = new List<Square>();

            var indicies = GetIndexOf(square);
            if (indicies is null)
            {
                return result;
            }
            (var row, var col) = indicies.Value;

            if (row > 0 && squares[row - 1, col] is { State: State.Empty } toAddUpper)
            {
                result.Add(toAddUpper);
            }
            if (col > 0 && squares[row, col - 1] is { State: State.Empty } toAddLeft)
            {
                result.Add(toAddLeft);
            }
            if (col < 6 && squares[row, col + 1] is { State: State.Empty } toAddRight)
            {
                result.Add(toAddRight);
            }

            return result;
        }

        private List<(Square move, Square? eaten)> GetFoxMove(Square square, List<Square>? previous = null)
        {
            previous ??= [];
            var temp = GetIndexOf(square);
            if (temp is null) 
            {
                return [];
            }
            List<(Square move, Square? eaten)>? max = null;
            var foxCoords = temp.Value;
            var toVisit = GetNeighbours(square).Where(x => x.State is State.Chicken);
            foreach (var chicken in toVisit)
            {
                var indicies = GetIndexOf(chicken);
                if (indicies is null)
                {
                    continue;
                }

                var chickenCoords = indicies.Value;
                var deltaX = chickenCoords.col - foxCoords.col;
                var deltaY = chickenCoords.row - foxCoords.row;
                var toCheckRow = foxCoords.row + deltaY * 2;
                var toCheckCol = foxCoords.col+ deltaX * 2;
                if (toCheckRow >= 0 && toCheckRow < 7
                 && toCheckCol >= 0 && toCheckCol < 7
                 && squares[toCheckRow, toCheckCol] is { State: State.Empty } empty
                 && !previous.Contains(empty))
                {
                    var result = new List<(Square move, Square? eaten)> { (empty, chicken) };
                    previous.Add(empty);
                    result.AddRange(GetFoxMove(empty, previous));
                    if (max is null || max.Count < result.Count)
                    {
                        if (max is not null)
                        {
                            previous.Clear();
                            previous.AddRange(result.Take(result.Count - 1).Select(x => x.move));
                        }
                        max = result;
                    }
                }
            }

            return max ?? [];
        }

        private List<Square> GetNeighbours(Square square)
        {

            var result = new List<Square>();

            var indicies = GetIndexOf(square);
            if (indicies is null)
            {
                return result;
            }
            (var row, var col) = indicies.Value;

            if (row > 0 && squares[row - 1, col] is { } toAddUpper)
            {
                result.Add(toAddUpper);
            }
            if (row < 6 && squares[row + 1, col] is { } toAddBelow)
            {
                result.Add(toAddBelow);
            }
            if (col > 0 && squares[row, col - 1] is { } toAddLeft)
            {
                result.Add(toAddLeft);
            }
            if (col < 6 && squares[row, col + 1] is { } toAddRight)
            {
                result.Add(toAddRight);
            }

            return result;
        }
    }
}
