namespace BVV_Task_5
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            toolStrip1 = new ToolStrip();
            difficultyComboBox = new ToolStripComboBox();
            saveAsButton = new ToolStripButton();
            saveButton = new ToolStripButton();
            openButton = new ToolStripButton();
            timer = new System.Windows.Forms.Timer(components);
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            saveTimer = new System.Windows.Forms.Timer(components);
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { difficultyComboBox, saveAsButton, saveButton, openButton });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // difficultyComboBox
            // 
            difficultyComboBox.Name = "difficultyComboBox";
            difficultyComboBox.Size = new Size(121, 25);
            difficultyComboBox.SelectedIndexChanged += difficultyComboBox_SelectedIndexChanged;
            difficultyComboBox.KeyPress += difficultyComboBox_KeyPress;
            // 
            // saveAsButton
            // 
            saveAsButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            saveAsButton.Image = (Image)resources.GetObject("saveAsButton.Image");
            saveAsButton.ImageTransparentColor = Color.Magenta;
            saveAsButton.Name = "saveAsButton";
            saveAsButton.Size = new Size(91, 22);
            saveAsButton.Text = "Сохранить как";
            saveAsButton.Click += saveAsButton_Click;
            // 
            // saveButton
            // 
            saveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            saveButton.Enabled = false;
            saveButton.Image = (Image)resources.GetObject("saveButton.Image");
            saveButton.ImageTransparentColor = Color.Magenta;
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(70, 22);
            saveButton.Text = "Сохранить";
            saveButton.Click += saveButton_Click;
            // 
            // openButton
            // 
            openButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            openButton.Image = (Image)resources.GetObject("openButton.Image");
            openButton.ImageTransparentColor = Color.Magenta;
            openButton.Name = "openButton";
            openButton.Size = new Size(58, 22);
            openButton.Text = "Открыть";
            openButton.Click += openButton_Click;
            // 
            // timer
            // 
            timer.Enabled = true;
            timer.Interval = 30;
            timer.Tick += timer_Tick;
            // 
            // openFileDialog
            // 
            openFileDialog.DefaultExt = "sudoku";
            openFileDialog.FileName = "openFileDialog1";
            openFileDialog.Filter = "Судоку|*.sudoku";
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "sudoku";
            saveFileDialog.Filter = "Судоку|*.sudoku";
            // 
            // saveTimer
            // 
            saveTimer.Interval = 60000;
            saveTimer.Tick += saveTimer_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(toolStrip1);
            Name = "Form1";
            Text = "Form1";
            KeyPress += Form1_KeyPress;
            MouseDown += Form1_MouseDown;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private System.Windows.Forms.Timer timer;
        private ToolStripComboBox difficultyComboBox;
        private ToolStripButton saveButton;
        private ToolStripButton openButton;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Timer saveTimer;
        private ToolStripButton saveAsButton;
    }
}
