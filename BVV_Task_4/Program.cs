using System.Text;

namespace BVV_Task_4
{
    enum State : byte
    {
        NotVisited,
        Visited,
        Enemy
    }
    [Flags]
    enum SurroundingWalls
    {
        None = 0,
        Up = 0x0001,
        Down = 0x0002,
        Left = 0x0004,
        Right = 0x0008,
        Enemy = 16
    }

    struct CellCoords(int x, int y)
    {
        public int x = x;
        public int y = y;

        public readonly bool Equals(CellCoords cell)
        {
            return x == cell.x && y == cell.y;
        }
    }

    internal class Program
    {
        static int wallsThickness = 2;
        static int generatorStep = wallsThickness + 1;

        //Size must be at least 2x2
        static int stepsHeight = 8;
        static int stepsWidth = 30;

        private static int hp = 10;

        static int rows;
        static int columns;

        const ConsoleColor backgroundColor = ConsoleColor.Gray;
        const ConsoleColor foregroundColor = ConsoleColor.Red;
        const ConsoleColor finishColor = ConsoleColor.Green;

        static CellCoords finishCell;
        static State[,] maze = null!;
        static readonly Random random = new();
        static readonly Dictionary<SurroundingWalls, char> chars = new()
        {
            {SurroundingWalls.None, ' ' },
            {SurroundingWalls.Up,  '║'},
            {SurroundingWalls.Down, '║' },
            {SurroundingWalls.Left, '═' },
            {SurroundingWalls.Right, '═' },
            {SurroundingWalls.Left | SurroundingWalls.Up, '╝' },
            {SurroundingWalls.Left | SurroundingWalls.Right, '═' },
            {SurroundingWalls.Left | SurroundingWalls.Down, '╗' },
            {SurroundingWalls.Up | SurroundingWalls.Right, '╚' },
            {SurroundingWalls.Up | SurroundingWalls.Down, '║' },
            {SurroundingWalls.Right | SurroundingWalls.Down, '╔' },
            {SurroundingWalls.Left | SurroundingWalls.Up | SurroundingWalls.Right, '╩' },
            {SurroundingWalls.Left| SurroundingWalls.Up | SurroundingWalls.Down, '╣' },
            {SurroundingWalls.Left | SurroundingWalls.Down | SurroundingWalls.Right, '╦' },
            {SurroundingWalls.Up | SurroundingWalls.Right | SurroundingWalls.Down, '╠' },
            {SurroundingWalls.Up | SurroundingWalls.Down | SurroundingWalls.Left | SurroundingWalls.Right, '╬' },
            { SurroundingWalls.Enemy , '\u263a'}
        };
        const char finishChar = '█';
        const string code = "hesoyam";
        private const int enemiesCount = 5;
        private static CellCoords prevPlayerPos;

        static void Main()
        {
            int startConsoleWidth = Console.WindowWidth;
            int startConsoleHeight = Console.WindowHeight;
            var startBackgroundColor = Console.BackgroundColor;
            var startForegroundColor = Console.ForegroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;

            //Maze generation cycle
            bool escapePressed = false;
            while (!escapePressed)
            {
                generatorStep = wallsThickness + 1;
                int cursorStep = generatorStep;
                rows = stepsHeight * generatorStep + 2 * wallsThickness + 1;
                columns = stepsWidth * generatorStep + 2 * wallsThickness + 1;
                maze = new State[rows, columns];

                int rightWindowOffset = 60;
                Console.SetWindowSize(columns + rightWindowOffset, rows + 1);

                GenerateMaze();
                Display();
                DisplayHealthBar();
                Console.SetCursorPosition(wallsThickness, wallsThickness);
                bool mazeExitRequested = false;
                string inputCode = "";
                var showPath = false;

                //User-input request cycle
                while (!mazeExitRequested)
                {
                    if (hp <= 0)
                    {
                        break;
                    }
                    var (Left, Top) = Console.GetCursorPosition();
                    var key = Console.ReadKey(true);
                    if (showPath)
                    {
                        DisplayWithPath(new PathFinder(maze, new CellCoords(Left, Top), finishCell));
                    }
                    else
                    {
                        Display();
                    }
                    if (code.Contains(inputCode + key.KeyChar))
                    {
                        inputCode += key.KeyChar;
                        if (inputCode.Length == code.Length)
                        {
                            showPath = true;
                            DisplayWithPath(new PathFinder(maze, new CellCoords(Left, Top), finishCell));
                            inputCode = "";
                        }
                    }
                    else
                    {
                        inputCode = "" + (code[0] == key.KeyChar ? key.KeyChar : "");
                    }
                    DisplayHealthBar();
                    Console.SetCursorPosition(Left, Top);
                    prevPlayerPos = new(Left, Top);

                    bool WayBlocked = false;
                    switch (key.Key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            if (Top >= cursorStep)
                            {
                                for (var i = Top; i > Top - cursorStep; i--)
                                    if (maze[i, Left] == State.NotVisited)
                                    {
                                        WayBlocked = true;
                                        break;
                                    }
                                if (!WayBlocked)
                                {
                                    Console.SetCursorPosition(Left, Top - cursorStep);
                                    MoveEnemies();
                                }
                            }
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            for (var i = Top; i < Top + cursorStep; i++)
                                if (maze[i, Left] == State.NotVisited)
                                {
                                    WayBlocked = true;
                                    break;
                                }
                            if (!WayBlocked)
                            {
                                
                                Console.SetCursorPosition(Left, Top + cursorStep);
                                MoveEnemies();
                            }

                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            if (Left >= cursorStep)
                            {
                                for (var i = Left; i > Left - cursorStep; i--)
                                    if (maze[Top, i] == State.NotVisited)
                                    {
                                        WayBlocked = true;
                                        break;
                                    }
                                if (!WayBlocked)
                                {
                                    Console.SetCursorPosition(Left - cursorStep, Top);
                                    MoveEnemies();
                                }
                            }
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            if (Left + cursorStep >= columns - 1 && Top == finishCell.y)
                                mazeExitRequested = true;
                            else
                            {

                                for (var i = Left; i < Left + cursorStep; i++)
                                    if (maze[Top, i] == State.NotVisited)
                                    {
                                        WayBlocked = true;
                                        break;
                                    }
                                if (!WayBlocked)
                                {
                                    Console.SetCursorPosition(Left + cursorStep, Top);
                                    MoveEnemies();
                                }
                            }
                            break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.Escape:
                            mazeExitRequested = true;
                            escapePressed = true;
                            break;
                    }
                }

                if (hp <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Хаха умер");
                    Console.ReadKey();
                    hp = 10;
                }
            }
            Console.SetWindowSize(startConsoleWidth, startConsoleHeight);
            Console.BackgroundColor = startBackgroundColor;
            Console.ForegroundColor = startForegroundColor;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }

        public static void DamagePlayer()
        {
            hp--;
        }

        public static void MoveEnemies()
        {
            var enemyCoords = new List<CellCoords>();
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == State.Enemy)
                    {
                        enemyCoords.Add(new(j, i));
                    }
                }
            }

            var pos = Console.GetCursorPosition();
            var curPlayerPos = new CellCoords(pos.Left, pos.Top);
            foreach (var coord in enemyCoords)
            {
                var freeCells = new List<CellCoords>();
                if (coord.y >= wallsThickness + generatorStep
                    && maze[coord.y - 1, coord.x] == State.Visited
                    && maze[coord.y - 2, coord.x] == State.Visited
                    && maze[coord.y - 3, coord.x] == State.Visited)
                {
                    freeCells.Add(new(coord.x, coord.y - generatorStep));
                }

                if (coord.x <= columns - wallsThickness - generatorStep
                    && maze[coord.y, coord.x + 1] == State.Visited
                    && maze[coord.y, coord.x + 2] == State.Visited
                    && maze[coord.y, coord.x + 3] == State.Visited)
                {
                    freeCells.Add(new(coord.x + generatorStep, coord.y));
                }

                if (coord.y <= rows - wallsThickness - generatorStep
                    && maze[coord.y + 1, coord.x] == State.Visited
                    && maze[coord.y + 2, coord.x] == State.Visited
                    && maze[coord.y + 3, coord.x] == State.Visited)
                {
                    freeCells.Add(new(coord.x, coord.y + generatorStep));
                }

                if (coord.x >= wallsThickness + generatorStep
                    && maze[coord.y, coord.x - 1] == State.Visited
                    && maze[coord.y, coord.x - 2] == State.Visited
                    && maze[coord.y, coord.x - 3] == State.Visited)
                {
                    freeCells.Add(new(coord.x - generatorStep, coord.y));
                }
                
                if (freeCells.Count != 0)
                {
                    var toMove = freeCells[random.Next(freeCells.Count)];
                    maze[toMove.y, toMove.x] = State.Enemy;
                    maze[coord.y, coord.x] = State.Visited;
                    if (curPlayerPos.Equals(toMove) || prevPlayerPos.Equals(toMove))
                    {
                        DamagePlayer();
                    }
                }
            }
        }

        static public List<CellCoords> GetNeighbours(CellCoords cell)
        {
            var list = new List<CellCoords>();

            if (cell.y >= wallsThickness + generatorStep)
            {
                list.Add(new CellCoords(cell.x, cell.y - generatorStep));
            }

            if (cell.x <= columns - wallsThickness - generatorStep)
            {
                list.Add(new CellCoords(cell.x + generatorStep, cell.y));
            }

            if (cell.y <= rows - wallsThickness - generatorStep)
            {
                list.Add(new CellCoords(cell.x, cell.y + generatorStep));
            }

            if (cell.x >= wallsThickness + generatorStep)
            {
                list.Add(new CellCoords(cell.x - generatorStep, cell.y));
            }
            return list;
        }

        static void GenerateMaze()
        {
            int generationStartX = wallsThickness;
            int generationStartY = wallsThickness;

            var generationStartCell = new CellCoords(generationStartX, generationStartY);
            maze[generationStartY, generationStartX] = State.Visited;
            //List with visited cells and surrounding unvisited cells to visit
            List<(List<CellCoords> cells, CellCoords from)> routeTuples = [(GetNeighbours(generationStartCell), generationStartCell)];
            while (routeTuples.Count > 0)
            {
                var randomTuple = routeTuples[random.Next(routeTuples.Count)];
                var unvisitedCells = randomTuple.cells.Where(cell => maze[cell.y, cell.x] == State.NotVisited).ToList();
                if (unvisitedCells.Count > 0)
                {
                    var randomDestination = unvisitedCells[random.Next(unvisitedCells.Count)];
                    MakePath(randomTuple.from, randomDestination);
                    routeTuples.Add((GetNeighbours(randomDestination), randomDestination));
                    randomTuple.cells.Remove(randomDestination);
                }
                else
                {
                    routeTuples.Remove(randomTuple);
                }
            }
            finishCell = GenerateFinish();
            var counter = 0;
            while (counter < enemiesCount)
            {
                var x = wallsThickness + generatorStep * random.Next(stepsWidth);
                var y = wallsThickness + generatorStep * random.Next(stepsHeight);
                if (maze[y, x] != State.Visited)
                {
                    continue;
                }

                maze[y, x] = State.Enemy;
                counter++;
            }
        }


        static void MakePath(CellCoords first, CellCoords second)
        {
            int deltaX = first.x - second.x;
            CellCoords min;
            CellCoords max;
            if (deltaX == 0)
            {
                if (first.y < second.y)
                {
                    min = first;
                    max = second;
                }
                else
                {
                    min = second;
                    max = first;
                }
                for (int i = min.y; i <= max.y; i++)
                {
                    maze[i, first.x] = State.Visited;
                }
            }
            else
            {
                if (first.x < second.x)
                {
                    min = first;
                    max = second;
                }
                else
                {
                    min = second;
                    max = first;
                }
                for (int i = min.x; i <= max.x; i++)
                {
                    maze[first.y, i] = State.Visited;
                }
            }
        }

        static CellCoords GenerateFinish()
        {
            var cell = new CellCoords();
            int y = random.Next(wallsThickness - 1, rows - wallsThickness);
            y -= (y - wallsThickness) % generatorStep;
            cell.x = columns - 1 - wallsThickness;
            cell.y = y;
            MakePath(cell, new CellCoords(columns - 1, cell.y));
            return cell;
        }

        static void Display()
        {
            Console.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    SurroundingWalls walls = SurroundingWalls.None;
                    if (i == finishCell.y && j > finishCell.x)
                    {
                        var consoleColor = Console.ForegroundColor;
                        Console.ForegroundColor = finishColor;
                        Console.Write(finishChar);
                        Console.ForegroundColor = consoleColor;
                    }
                    else
                    {
                        if (maze[i, j] != State.Visited && maze[i, j] != State.Enemy)
                        {
                            if (i > 0 && maze[i - 1, j] == State.NotVisited) walls |= SurroundingWalls.Up;

                            if (i < rows - 1 && maze[i + 1, j] == State.NotVisited) walls |= SurroundingWalls.Down;

                            if (j > 0 && maze[i, j - 1] == State.NotVisited) walls |= SurroundingWalls.Left;

                            if (j < columns - 1 && maze[i, j + 1] == State.NotVisited) walls |= SurroundingWalls.Right;
                        }
                        else if (maze[i, j] == State.Enemy)
                        {
                            walls = SurroundingWalls.Enemy;
                        }
                        Console.Write(chars[walls]);
                    }
                }
                Console.WriteLine();
            }
        }

        static void DisplayHealthBar()
        {
            Console.SetCursorPosition(columns + 3, 3);
            var stringBuilder = new StringBuilder("Здоровье [");
            for (int i = 1; i <= 10; i++)
            {
                if (i <= hp)
                {
                    stringBuilder.Append('#');
                }
                else
                {
                    stringBuilder.Append(' ');
                }
            }

            stringBuilder.Append(']');
            Console.Write(stringBuilder.ToString());
        }
        static void DisplayWithPath(PathFinder pathFinder)
        {
            Console.Clear();
            var pathColor = ConsoleColor.DarkGreen;
            var pathChar = finishChar;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    SurroundingWalls walls = SurroundingWalls.None;
                    if (i == finishCell.y && j > finishCell.x)
                    {
                        var consoleColor = Console.ForegroundColor;
                        Console.ForegroundColor = finishColor;
                        Console.Write(finishChar);
                        Console.ForegroundColor = consoleColor;
                    }
                    else if (pathFinder.Way.Contains(new CellCoords(j, i)) && maze[i, j] != State.Enemy && !(new CellCoords(j, i).Equals(prevPlayerPos)))
                    {
                        var consoleColor = Console.ForegroundColor;
                        Console.ForegroundColor = pathColor;
                        Console.Write(pathChar);
                        Console.ForegroundColor = consoleColor;
                    }
                    else
                    {
                        if (maze[i, j] != State.Visited)
                        {
                            if (maze[i, j] != State.Visited && maze[i, j] != State.Enemy)
                        {
                            if (i > 0 && maze[i - 1, j] == State.NotVisited) walls |= SurroundingWalls.Up;

                            if (i < rows - 1 && maze[i + 1, j] == State.NotVisited) walls |= SurroundingWalls.Down;

                            if (j > 0 && maze[i, j - 1] == State.NotVisited) walls |= SurroundingWalls.Left;

                            if (j < columns - 1 && maze[i, j + 1] == State.NotVisited) walls |= SurroundingWalls.Right;
                        }
                        else if (maze[i, j] == State.Enemy)
                        {
                            walls = SurroundingWalls.Enemy;
                        }
                        }
                        Console.Write(chars[walls]);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}