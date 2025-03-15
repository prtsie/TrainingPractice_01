using System.ComponentModel;

namespace BVV_Task_8;

sealed partial class SettingsPrompt
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        label1 = new System.Windows.Forms.Label();
        sizeInput = new System.Windows.Forms.NumericUpDown();
        immunityIgnoreChanceInput = new System.Windows.Forms.NumericUpDown();
        label2 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        label4 = new System.Windows.Forms.Label();
        intervalInput = new System.Windows.Forms.NumericUpDown();
        label5 = new System.Windows.Forms.Label();
        button1 = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)sizeInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)immunityIgnoreChanceInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)intervalInput).BeginInit();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Location = new System.Drawing.Point(185, 10);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(55, 21);
        label1.TabIndex = 0;
        label1.Text = "Размер";
        label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // sizeInput
        // 
        sizeInput.Increment = new decimal(new int[] { 2, 0, 0, 0 });
        sizeInput.Location = new System.Drawing.Point(246, 11);
        sizeInput.Maximum = new decimal(new int[] { 51, 0, 0, 0 });
        sizeInput.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
        sizeInput.Name = "sizeInput";
        sizeInput.Size = new System.Drawing.Size(120, 23);
        sizeInput.TabIndex = 1;
        sizeInput.Value = new decimal(new int[] { 5, 0, 0, 0 });
        // 
        // immunityIgnoreChanceInput
        // 
        immunityIgnoreChanceInput.Location = new System.Drawing.Point(246, 40);
        immunityIgnoreChanceInput.Name = "immunityIgnoreChanceInput";
        immunityIgnoreChanceInput.Size = new System.Drawing.Size(120, 23);
        immunityIgnoreChanceInput.TabIndex = 3;
        // 
        // label2
        // 
        label2.Location = new System.Drawing.Point(6, 39);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(234, 21);
        label2.TabIndex = 2;
        label2.Text = "Шанс заражения клетки с иммунитетом";
        label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // label3
        // 
        label3.Location = new System.Drawing.Point(372, 40);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(55, 21);
        label3.TabIndex = 4;
        label3.Text = "%";
        label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // label4
        // 
        label4.Location = new System.Drawing.Point(372, 69);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(55, 21);
        label4.TabIndex = 7;
        label4.Text = "мс";
        label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // intervalInput
        // 
        intervalInput.Increment = new decimal(new int[] { 100, 0, 0, 0 });
        intervalInput.Location = new System.Drawing.Point(246, 69);
        intervalInput.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        intervalInput.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
        intervalInput.Name = "intervalInput";
        intervalInput.Size = new System.Drawing.Size(120, 23);
        intervalInput.TabIndex = 6;
        intervalInput.Value = new decimal(new int[] { 1000, 0, 0, 0 });
        // 
        // label5
        // 
        label5.Location = new System.Drawing.Point(6, 68);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(234, 21);
        label5.TabIndex = 5;
        label5.Text = "Интервал времени";
        label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // button1
        // 
        button1.Location = new System.Drawing.Point(291, 98);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(75, 23);
        button1.TabIndex = 8;
        button1.Text = "ОК";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // SettingsPrompt
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(405, 130);
        Controls.Add(button1);
        Controls.Add(label4);
        Controls.Add(intervalInput);
        Controls.Add(label5);
        Controls.Add(label3);
        Controls.Add(immunityIgnoreChanceInput);
        Controls.Add(label2);
        Controls.Add(sizeInput);
        Controls.Add(label1);
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Настройки";
        ((System.ComponentModel.ISupportInitialize)sizeInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)immunityIgnoreChanceInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)intervalInput).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button button1;

    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.NumericUpDown intervalInput;
    private System.Windows.Forms.Label label5;

    private System.Windows.Forms.NumericUpDown immunityIgnoreChanceInput;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown sizeInput;

    #endregion
}