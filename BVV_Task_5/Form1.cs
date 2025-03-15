namespace BVV_Task_5
{
    public partial class Form1 : Form
    {
        private const int squareSize = 50;
        private readonly Pen boldBorderPen;
        private readonly Brush selectedBrush = Brushes.Aqua;
        private readonly Brush rightBrush = Brushes.DarkGreen;
        private const int horizontalOffset = 100;
        private const int verticalOffset = 50;
        private readonly BufferedGraphics buffer;
        private readonly Square[,] squares = new Square[9, 9];
        private readonly Square[] squaresOneD = new Square[9 * 9];
        private Square? selected;
        private float difficulty = 0.2f;
        private string? filePath;
        private readonly Dictionary<string, float> difficulties = new()
        {
            {"Простой", 0.15f },
            {"Средний", 0.3f },
            {"Сложный", 0.6f },
        };

        public Form1()
        {
            InitializeComponent();
            boldBorderPen = (Pen)Program.borderPen.Clone();
            boldBorderPen.Width *= 3;
            Width = horizontalOffset * 2 + squareSize * 9;
            Height = verticalOffset * 2 + squareSize * 9;
            MinimumSize = MaximumSize = new(Width, Height);
            buffer = BufferedGraphicsManager.Current.Allocate(CreateGraphics(), DisplayRectangle);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    squares[i, j] = new Square()
                    {
                        Position = new Rectangle(horizontalOffset + squareSize * j, verticalOffset + squareSize * i, squareSize, squareSize),
                    };
                    squaresOneD[i * 9 + j] = squares[i, j];
                }
            }
            if (File.Exists("data.txt"))
            {
                var fileName = File.ReadAllText("data.txt");
                if (File.Exists(fileName))
                {
                    filePath = fileName;
                    LoadFromFile();
                }
            }
            else
            {
                difficultyComboBox.Items.AddRange(difficulties.Keys.ToArray());
                difficultyComboBox.SelectedIndex = 0;
            }
        }

        private void ApplyDifficulty()
        {
            for (int i = 0; i < 9 * 9 * difficulty; i++)
            {
                var canSetNull = squaresOneD.Where(x => x.Value != 0).ToArray();
                var square = canSetNull[Random.Shared.Next(canSetNull.Length)];
                square.ValueReadonly = false;
                square.Value = 0;
            }
        }

        private bool FillSudoku()
        {
            Array.ForEach(squaresOneD, x =>
            {
                x.Value = 0;
                x.ValueReadonly = true;
            });
            return SolveSudoku(0, 0);
        }

        private bool SolveSudoku(int row, int col)
        {
            if (row == 9)
                return true;

            if (col == 9)
                return SolveSudoku(row + 1, 0);

            if (squares[row, col].Value != 0)
                return SolveSudoku(row, col + 1);

            var allowedValues = GetAllowedValues(row, col);
            Shuffle(allowedValues);

            foreach (var num in allowedValues)
            {
                squares[row, col].Value = num;
                if (SolveSudoku(row, col + 1))
                    return true;
                squares[row, col].Value = 0;
            }

            return false;
        }

        private List<int> GetAllowedValues(int row, int col)
        {
            var allowedValues = Enumerable.Range(1, 9).ToList();

            for (int i = 0; i < 9; i++)
            {
                allowedValues.Remove(squares[row, i].Value);
                allowedValues.Remove(squares[i, col].Value);
            }

            int startRow = row / 3 * 3;
            int startCol = col / 3 * 3;
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    allowedValues.Remove(squares[i, j].Value);
                }
            }

            return allowedValues;
        }

        private static void Shuffle(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Shared.Next(i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

        private void DrawField()
        {
            foreach (var square in squares)
            {
                square.Draw(buffer);
            }
            DrawSeparators();
        }

        private void DrawSeparators()
        {
            for (int i = 2; i < 8; i += 3)
            {
                for (int j = 3; j < 9; j += 3)
                {
                    buffer.Graphics.DrawLine(boldBorderPen, new(horizontalOffset + squareSize * j, verticalOffset), new(horizontalOffset + squareSize * j, verticalOffset + squareSize * 9));
                }
                buffer.Graphics.DrawLine(boldBorderPen, new(horizontalOffset, verticalOffset + squareSize * (i + 1)), new(horizontalOffset + squareSize * 9, verticalOffset + squareSize * (i + 1)));
            }
        }

        private void FillSelected()
        {
            if (selected != null)
            {
                buffer.Graphics.FillRectangle(selectedBrush, selected.Position);
            }
        }

        private void FillRightFields()
        {
            var valid = true;
            for (int i = 0; i < 9; i++)
            {
                var vals = Enumerable.Range(1, 9).ToList();
                for (int j = 0; j < 9; j++)
                {
                    vals.Remove(squares[i, j].Value);
                }
                if (vals.Count == 0)
                {
                    buffer.Graphics.FillRectangle(rightBrush, new(horizontalOffset, verticalOffset + squareSize * i, squareSize * 9, squareSize));
                }
                else
                {
                    valid = false;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                var vals = Enumerable.Range(1, 9).ToList();
                for (int j = 0; j < 9; j++)
                {
                    vals.Remove(squares[j, i].Value);
                }
                if (vals.Count == 0)
                {
                    buffer.Graphics.FillRectangle(rightBrush, new(horizontalOffset + squareSize * i, verticalOffset, squareSize, squareSize * 9));
                }
                else
                {
                    valid = false;
                }
            }

            for (int startRow = 0; startRow < 9; startRow += 3)
            {
                for (int startCol = 0; startCol < 9; startCol += 3)
                {
                    var vals = Enumerable.Range(1, 9).ToList();
                    for (int i = startRow; i < startRow + 3; i++)
                    {
                        for (int j = startCol; j < startCol + 3; j++)
                        {
                            vals.Remove(squares[i, j].Value);
                        }
                    }
                    if (vals.Count == 0)
                    {
                        buffer.Graphics.FillRectangle(rightBrush, new(horizontalOffset + squareSize * startCol, verticalOffset + squareSize * startRow, squareSize * 3, squareSize * 3));
                    }
                    else
                    {
                        valid = false;
                    }
                }
            }
            if (valid)
            {
                timer.Stop();
                MessageBox.Show("ПОООБЕЕЕЕДААА!!!!!!!");
                FillSudoku();
                ApplyDifficulty();
                timer.Start();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (squaresOneD.FirstOrDefault(x => x.Position.Contains(e.Location)) is { } square)
            {
                selected = square;
            }
            else
            {
                selected = null;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            buffer.Graphics.Clear(Color.White);
            FillRightFields();
            FillSelected();
            DrawField();
            buffer.Render();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            var ch = e.KeyChar.ToString();
            if (selected is { ValueReadonly: false } && int.TryParse(ch, out var val))
            {
                selected.Value = val;
            }
        }

        private void difficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (difficultyComboBox.SelectedItem is null)
            {
                difficultyComboBox.SelectedIndex = 0;
            }

            difficulty = difficulties[difficultyComboBox.SelectedItem!.ToString()!];
            FillSudoku();
            ApplyDifficulty();
            Focus();
        }

        private void difficultyComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Form1_KeyPress(sender, e);
            e.Handled = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveTimer_Tick(sender, e);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                LoadFromFile();
            }
        }

        private void LoadFromFile()
        {
            if (filePath is null)
            {
                return;
            }
            var content = File.ReadAllText(filePath);
            if (content.Length != 81)
            {
                MessageBox.Show("Файл неверного формата", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < 81; i++)
            {
                var ch = content[i].ToString();
                var square = squaresOneD[i];
                if (int.TryParse(ch, out var val))
                {
                    square.Value = val;
                    square.ValueReadonly = val != 0;
                }
                else
                {
                    MessageBox.Show("Файл неверного формата", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            EnableAutoSave();
            difficultyComboBox.Text = "Custom";
        }

        private void saveTimer_Tick(object sender, EventArgs e)
        {
            if (filePath is null)
            {
                MessageBox.Show("Ошибка сохранения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var content = new string(squaresOneD.Select(x => x.Value.ToString()[0]).ToArray());
            File.WriteAllText(filePath, content);
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var content = new string(squaresOneD.Select(x => x.Value.ToString()[0]).ToArray());
                File.WriteAllText(saveFileDialog.FileName, content);
                filePath = saveFileDialog.FileName;
                EnableAutoSave();
            }
        }

        private void EnableAutoSave()
        {
            saveButton.Enabled = true;
            File.WriteAllText("data.txt", filePath);
            saveTimer.Start();
        }
    }
}
