using BVV_Task_2;
using BVV_Task_2.Pieces;

List<(char letter, int num)> translationTuples =
[
    ('a', 1),
    ('b', 2),
    ('c', 3),
    ('d', 4),
    ('e', 5),
    ('f', 6),
    ('g', 7),
    ('h', 8)
];

ChessPiece[] pieces =
[
    new Bishop(),
    new Horse(),
    new King(),
    new Pawn(),
    new Queen(),
    new Rook()
];

var input = Console.ReadLine()!.Split();
if (input.Length != 5)
{
    Console.WriteLine("Неверный формат");
    return;
}

var targetPiece = pieces
    .First(x => string.Equals(x.Name, input[0], StringComparison.CurrentCultureIgnoreCase));
var startCoords = new Vector(translationTuples.First(x => x.letter == input[1][0]).num, int.Parse(input[1][1].ToString()));

var obstaclePiece = pieces
    .First(x => string.Equals(x.Name, input[2], StringComparison.CurrentCultureIgnoreCase));
var obstacleCoords = new Vector(translationTuples.First(x => x.letter == input[3][0]).num, int.Parse(input[3][1].ToString()));
var targetCoords = new Vector(translationTuples.First(x => x.letter == input[4][0]).num, int.Parse(input[4][1].ToString()));

Console.Clear();
DrawBoard();
DrawOnBoard('■', ConsoleColor.Green, targetCoords);
DrawOnBoard(targetPiece.Icon, ConsoleColor.White, startCoords);
DrawOnBoard(obstaclePiece.Icon, ConsoleColor.Black, obstacleCoords);

var blockedCoords = obstaclePiece.GetMoves(obstacleCoords).Append(obstacleCoords).ToArray();
var path = new Stack<Vector>();
var visited = new List<Vector>();
path.Push(startCoords);

if (FindPath())
{
    var result = path.Reverse().ToArray();
    var strResult = string
        .Join("; ", result
            .Select(x => $"{translationTuples.First(y => y.num == x.X).letter}{x.Y}"));
    foreach (var vector in result)
    {
        Console.Clear();
        DrawBoard();
        DrawOnBoard('■', ConsoleColor.Green, targetCoords);
        DrawOnBoard(targetPiece.Icon, ConsoleColor.White, vector);
        DrawOnBoard(obstaclePiece.Icon, ConsoleColor.Black, obstacleCoords);
        foreach (var coord in blockedCoords)
        {
            if (coord.Equals(obstacleCoords))
            {
                continue;
            }
            DrawOnBoard('■', ConsoleColor.Red, coord);
        }
        ColoredWrite(strResult, ConsoleColor.White, new(0, 13));
        Thread.Sleep(1000);
    }
}
else
{
    ColoredWrite("Нет пути", ConsoleColor.White, new(0, 13));
}

Console.SetCursorPosition(0, 20);
return;
void DrawBoard()
{
    var isWhite = true;
    for (var i = 0; i < 8; i++)
    {
        for (var j = 0; j < 8; j++)
        {
            var color = isWhite ? ConsoleColor.White : ConsoleColor.Black;
            ColoredWrite("■ ", color, new(j * 2, i));
            isWhite = !isWhite;
            Console.WriteLine(8 - i);
        }

        isWhite = !isWhite;
    }

    Console.WriteLine("a b c d e f g h");
}

void DrawOnBoard(char ch, ConsoleColor color, Vector position)
{
    ColoredWrite(ch.ToString(), color, position with { X = position.X * 2 - 2, Y = 8 - position.Y});
}

void ColoredWrite(string str, ConsoleColor color, Vector at)
{
    var currentColor = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.SetCursorPosition(at.X, at.Y);
    Console.Write(str);
    Console.ForegroundColor = currentColor;
}

bool FindPath()
{
    if (path.Peek().Equals(targetCoords))
    {
        return true;
    }
    var toVisit = targetPiece.GetMoves(path.Peek()).ToList();
    while (toVisit.Count > 0)
    {
        var coord = toVisit.First();
        if (blockedCoords.Contains(coord) || visited.Contains(coord))
        {
            toVisit.Remove(coord);
        }
        else
        {
            visited.Add(coord);
            path.Push(coord);
            if (FindPath())
            {
                return true;
            }

            path.Pop();
        }
    }

    return false;
}