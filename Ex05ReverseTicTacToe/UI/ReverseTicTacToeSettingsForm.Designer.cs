
namespace Ex05.ReverseTicTacToeWindowsApp
{
    partial class ReverseTicTacToeSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Player2CheckBox = new System.Windows.Forms.CheckBox();
            this.Player1NameTextBox = new System.Windows.Forms.TextBox();
            this.Player2NameTextBox = new System.Windows.Forms.TextBox();
            this.RowsUpDown = new System.Windows.Forms.NumericUpDown();
            this.ColsUpDown = new System.Windows.Forms.NumericUpDown();
            this.StartButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.RowsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Players:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Player 1:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Board Size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(158, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Cols:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Rows:";
            // 
            // Player2CheckBox
            // 
            this.Player2CheckBox.AutoSize = true;
            this.Player2CheckBox.Location = new System.Drawing.Point(34, 66);
            this.Player2CheckBox.Name = "Player2CheckBox";
            this.Player2CheckBox.Size = new System.Drawing.Size(67, 17);
            this.Player2CheckBox.TabIndex = 6;
            this.Player2CheckBox.Text = "Player 2:";
            this.Player2CheckBox.UseVisualStyleBackColor = true;
            this.Player2CheckBox.CheckedChanged += new System.EventHandler(this.Player2CheckBox_CheckedChanged);
            // 
            // Player1NameTextBox
            // 
            this.Player1NameTextBox.Location = new System.Drawing.Point(129, 33);
            this.Player1NameTextBox.MaxLength = 9;
            this.Player1NameTextBox.Name = "Player1NameTextBox";
            this.Player1NameTextBox.Size = new System.Drawing.Size(100, 20);
            this.Player1NameTextBox.TabIndex = 7;
            // 
            // Player2NameTextBox
            // 
            this.Player2NameTextBox.Enabled = false;
            this.Player2NameTextBox.Location = new System.Drawing.Point(129, 64);
            this.Player2NameTextBox.MaxLength = 9;
            this.Player2NameTextBox.Name = "Player2NameTextBox";
            this.Player2NameTextBox.ReadOnly = true;
            this.Player2NameTextBox.Size = new System.Drawing.Size(100, 20);
            this.Player2NameTextBox.TabIndex = 8;
            this.Player2NameTextBox.Text = "[Computer]";
            // 
            // RowsUpDown
            // 
            this.RowsUpDown.Location = new System.Drawing.Point(74, 145);
            this.RowsUpDown.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.RowsUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.RowsUpDown.Name = "RowsUpDown";
            this.RowsUpDown.Size = new System.Drawing.Size(35, 20);
            this.RowsUpDown.TabIndex = 9;
            this.RowsUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.RowsUpDown.ValueChanged += new System.EventHandler(this.RowsUpDown_ValueChanged);
            // 
            // ColsUpDown
            // 
            this.ColsUpDown.Location = new System.Drawing.Point(194, 145);
            this.ColsUpDown.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.ColsUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.ColsUpDown.Name = "ColsUpDown";
            this.ColsUpDown.Size = new System.Drawing.Size(35, 20);
            this.ColsUpDown.TabIndex = 10;
            this.ColsUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.ColsUpDown.ValueChanged += new System.EventHandler(this.ColsUpDown_ValueChanged);
            // 
            // StartButton
            // 
            this.StartButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.StartButton.Location = new System.Drawing.Point(24, 177);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(205, 23);
            this.StartButton.TabIndex = 11;
            this.StartButton.Text = "Start!";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.StartButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 210);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ColsUpDown);
            this.Controls.Add(this.RowsUpDown);
            this.Controls.Add(this.Player2NameTextBox);
            this.Controls.Add(this.Player1NameTextBox);
            this.Controls.Add(this.Player2CheckBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Settings";
            ((System.ComponentModel.ISupportInitialize)(this.RowsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox Player2CheckBox;
        private System.Windows.Forms.TextBox Player1NameTextBox;
        private System.Windows.Forms.TextBox Player2NameTextBox;
        private System.Windows.Forms.NumericUpDown RowsUpDown;
        private System.Windows.Forms.NumericUpDown ColsUpDown;
        private System.Windows.Forms.Button StartButton;
    }
}

