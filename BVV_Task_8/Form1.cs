namespace BVV_Task_8;

public sealed partial class Form1 : Form
{
    private readonly Square[,] squares = new Square[Program.Settings.Size, Program.Settings.Size];
    private readonly Square[] squaresOneD = new Square[Program.Settings.Size * Program.Settings.Size];
    private const int Offset = 50;
    private const int WindowSize = 1000;
    private readonly BufferedGraphics buffer;

    public Form1()
    {
        InitializeComponent();
        MinimumSize = MaximumSize = Size = new(WindowSize, WindowSize);
        buffer = BufferedGraphicsManager.Current.Allocate(CreateGraphics(), DisplayRectangle);
        timer.Interval = Program.Settings.TimeInterval;
        var squareSize = (WindowSize - Offset * 2) / Program.Settings.Size;
        for (var i = 0; i < Program.Settings.Size; i++)
        {
            for (var j = 0; j < Program.Settings.Size; j++)
            {
                squares[i, j] = new()
                {
                    Position = new(Offset + i * squareSize, Offset + j * squareSize, squareSize, squareSize)
                };
                squaresOneD[i * Program.Settings.Size + j] = squares[i, j];
            }
        }

        squares[Program.Settings.Size / 2, Program.Settings.Size / 2].State = State.Infected;
        Draw();
    }

    private void timer_Tick(object sender, EventArgs e)
    {
        foreach (var square in squaresOneD.Where(x => x.State is State.Infected).ToArray())
        {
            if (++square.Count == 6)
            {
                square.State = State.Immunity;
                continue;
            }
            if (TossCoin() || square.Count == 1)
            {
                continue;
            }

            var canInfect = GetNeighbours(square).Where(x => x.State is State.Normal).ToArray();
            if (canInfect.Length > 0)
            {
                canInfect[Random.Shared.Next(canInfect.Length)].State = State.Infected;
            }
        }

        foreach (var square in squaresOneD.Where(x => x.State == State.Immunity).ToArray())
        {
            if (++square.Count == 4)
            {
                square.State = State.Normal;
            }

            if (square.Count == 1)
            {
                continue;
            }
            
            var infectedNeighbours = GetNeighbours(square).Where(x => x.State is State.Infected && x.State != 0).ToArray();
            if (infectedNeighbours.Length > 0 && TossCoin(Program.Settings.ImmunityIgnoreChance))
            {
                square.State = State.Infected;
            }
        }
        
        Draw();
    }

    private void Draw()
    {
        buffer.Graphics.Clear(Color.White);
        foreach (var square in squaresOneD)
        {
            square.Draw(buffer);
        }

        buffer.Render();
    }


    private (int row, int col)? GetIndexOf(Square square)
    {
        for (var i = 0; i < Program.Settings.Size; i++)
        {
            for (var j = 0; j < Program.Settings.Size; j++)
            {
                if (squares[i, j].Equals(square))
                {
                    return (i, j);
                }
            }
        }

        return null;
    }

    private List<Square> GetNeighbours(Square square)
    {
        var result = new List<Square>();

        var indicies = GetIndexOf(square);
        if (indicies is null)
        {
            return result;
        }

        var (row, col) = indicies.Value;

        if (row > 0 && squares[row - 1, col] is { } toAddUpper)
        {
            result.Add(toAddUpper);
        }

        if (row < Program.Settings.Size - 1 && squares[row + 1, col] is { } toAddBelow)
        {
            result.Add(toAddBelow);
        }

        if (col > 0 && squares[row, col - 1] is { } toAddLeft)
        {
            result.Add(toAddLeft);
        }

        if (col < Program.Settings.Size - 1 && squares[row, col + 1] is { } toAddRight)
        {
            result.Add(toAddRight);
        }

        return result;
    }

    private static bool TossCoin(decimal probability = 0.5m) =>
        Convert.ToDecimal(Random.Shared.NextDouble()) < probability;
}