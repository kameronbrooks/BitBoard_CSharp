namespace BitBoard_CSharp
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
            doubleBufferedPanel1 = new DoubleBufferedPanel();
            winnerLabel = new Label();
            gameOverLabel = new Label();
            panel1 = new Panel();
            label3 = new Label();
            hoverBitMaskDataLabel = new Label();
            TurnLabel = new Label();
            turnDataLabel = new Label();
            player1ScoreLabel = new Label();
            player2ScoreLabel = new Label();
            panel2 = new Panel();
            label9 = new Label();
            player2HexDataLabel = new Label();
            player2BinaryDataLabel = new Label();
            label8 = new Label();
            player1HexDataLabel = new Label();
            player1BinaryDataLabel = new Label();
            label4 = new Label();
            restartButton = new Button();
            asciiDisplay = new Label();
            player2ScoreValue = new Label();
            player1ScoreValue = new Label();
            label5 = new Label();
            doubleBufferedPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // doubleBufferedPanel1
            // 
            doubleBufferedPanel1.Controls.Add(winnerLabel);
            doubleBufferedPanel1.Controls.Add(gameOverLabel);
            doubleBufferedPanel1.Location = new Point(0, 0);
            doubleBufferedPanel1.Name = "doubleBufferedPanel1";
            doubleBufferedPanel1.Size = new Size(480, 480);
            doubleBufferedPanel1.TabIndex = 1;
            doubleBufferedPanel1.Paint += doubleBufferedPanel1_Paint;
            doubleBufferedPanel1.MouseClick += doubleBufferedPanel1_MouseClick;
            doubleBufferedPanel1.MouseMove += doubleBufferedPanel1_MouseMove;
            // 
            // winnerLabel
            // 
            winnerLabel.AutoSize = true;
            winnerLabel.BackColor = Color.Transparent;
            winnerLabel.Font = new Font("Stencil", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            winnerLabel.ForeColor = SystemColors.ControlLightLight;
            winnerLabel.Location = new Point(111, 261);
            winnerLabel.Name = "winnerLabel";
            winnerLabel.Size = new Size(236, 44);
            winnerLabel.TabIndex = 9;
            winnerLabel.Text = "Black Wins";
            winnerLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gameOverLabel
            // 
            gameOverLabel.AutoSize = true;
            gameOverLabel.BackColor = Color.Transparent;
            gameOverLabel.Font = new Font("Stencil", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            gameOverLabel.ForeColor = SystemColors.ControlLightLight;
            gameOverLabel.Location = new Point(91, 214);
            gameOverLabel.Name = "gameOverLabel";
            gameOverLabel.Size = new Size(284, 57);
            gameOverLabel.TabIndex = 8;
            gameOverLabel.Text = "Game Over";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label3);
            panel1.Controls.Add(hoverBitMaskDataLabel);
            panel1.Location = new Point(5, 481);
            panel1.Name = "panel1";
            panel1.Size = new Size(789, 34);
            panel1.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Lime;
            label3.Location = new Point(3, 10);
            label3.Name = "label3";
            label3.Size = new Size(119, 14);
            label3.TabIndex = 9;
            label3.Text = "Cursor Bit Mask:";
            // 
            // hoverBitMaskDataLabel
            // 
            hoverBitMaskDataLabel.AutoSize = true;
            hoverBitMaskDataLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            hoverBitMaskDataLabel.ForeColor = Color.Lime;
            hoverBitMaskDataLabel.Location = new Point(120, 10);
            hoverBitMaskDataLabel.Name = "hoverBitMaskDataLabel";
            hoverBitMaskDataLabel.Size = new Size(231, 14);
            hoverBitMaskDataLabel.TabIndex = 8;
            hoverBitMaskDataLabel.Text = "00000000000000000000000000000000";
            // 
            // TurnLabel
            // 
            TurnLabel.AutoSize = true;
            TurnLabel.Location = new Point(486, 9);
            TurnLabel.Name = "TurnLabel";
            TurnLabel.Size = new Size(34, 15);
            TurnLabel.TabIndex = 3;
            TurnLabel.Text = "Turn:";
            // 
            // turnDataLabel
            // 
            turnDataLabel.AutoSize = true;
            turnDataLabel.Location = new Point(526, 9);
            turnDataLabel.Name = "turnDataLabel";
            turnDataLabel.Size = new Size(35, 15);
            turnDataLabel.TabIndex = 4;
            turnDataLabel.Text = "Black";
            // 
            // player1ScoreLabel
            // 
            player1ScoreLabel.AutoSize = true;
            player1ScoreLabel.Location = new Point(486, 35);
            player1ScoreLabel.Name = "player1ScoreLabel";
            player1ScoreLabel.Size = new Size(83, 15);
            player1ScoreLabel.TabIndex = 5;
            player1ScoreLabel.Text = "Player 1 Score:";
            // 
            // player2ScoreLabel
            // 
            player2ScoreLabel.AutoSize = true;
            player2ScoreLabel.Location = new Point(486, 62);
            player2ScoreLabel.Name = "player2ScoreLabel";
            player2ScoreLabel.Size = new Size(83, 15);
            player2ScoreLabel.TabIndex = 6;
            player2ScoreLabel.Text = "Player 2 Score:";
            // 
            // panel2
            // 
            panel2.BackColor = Color.Black;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label9);
            panel2.Controls.Add(player2HexDataLabel);
            panel2.Controls.Add(player2BinaryDataLabel);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(player1HexDataLabel);
            panel2.Controls.Add(player1BinaryDataLabel);
            panel2.Controls.Add(label4);
            panel2.Location = new Point(486, 240);
            panel2.Name = "panel2";
            panel2.Size = new Size(308, 235);
            panel2.TabIndex = 7;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.Lime;
            label9.Location = new Point(6, 103);
            label9.Name = "label9";
            label9.Size = new Size(287, 14);
            label9.TabIndex = 7;
            label9.Text = "----------------------------------------";
            // 
            // player2HexDataLabel
            // 
            player2HexDataLabel.AutoSize = true;
            player2HexDataLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            player2HexDataLabel.ForeColor = Color.Lime;
            player2HexDataLabel.Location = new Point(6, 187);
            player2HexDataLabel.Name = "player2HexDataLabel";
            player2HexDataLabel.Size = new Size(84, 14);
            player2HexDataLabel.TabIndex = 6;
            player2HexDataLabel.Text = "FF FF FF FF";
            // 
            // player2BinaryDataLabel
            // 
            player2BinaryDataLabel.AutoSize = true;
            player2BinaryDataLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            player2BinaryDataLabel.ForeColor = Color.Lime;
            player2BinaryDataLabel.Location = new Point(6, 161);
            player2BinaryDataLabel.Name = "player2BinaryDataLabel";
            player2BinaryDataLabel.Size = new Size(231, 14);
            player2BinaryDataLabel.TabIndex = 5;
            player2BinaryDataLabel.Text = "00000000000000000000000000000000";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.Lime;
            label8.Location = new Point(6, 134);
            label8.Name = "label8";
            label8.Size = new Size(126, 14);
            label8.TabIndex = 4;
            label8.Text = "Player 2 (Binary)";
            // 
            // player1HexDataLabel
            // 
            player1HexDataLabel.AutoSize = true;
            player1HexDataLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            player1HexDataLabel.ForeColor = Color.Lime;
            player1HexDataLabel.Location = new Point(3, 69);
            player1HexDataLabel.Name = "player1HexDataLabel";
            player1HexDataLabel.Size = new Size(84, 14);
            player1HexDataLabel.TabIndex = 3;
            player1HexDataLabel.Text = "FF FF FF FF";
            // 
            // player1BinaryDataLabel
            // 
            player1BinaryDataLabel.AutoSize = true;
            player1BinaryDataLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            player1BinaryDataLabel.ForeColor = Color.Lime;
            player1BinaryDataLabel.Location = new Point(3, 43);
            player1BinaryDataLabel.Name = "player1BinaryDataLabel";
            player1BinaryDataLabel.Size = new Size(231, 14);
            player1BinaryDataLabel.TabIndex = 2;
            player1BinaryDataLabel.Text = "00000000000000000000000000000000";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Lime;
            label4.Location = new Point(3, 16);
            label4.Name = "label4";
            label4.Size = new Size(126, 14);
            label4.TabIndex = 1;
            label4.Text = "Player 1 (Binary)";
            // 
            // restartButton
            // 
            restartButton.Location = new Point(671, 5);
            restartButton.Name = "restartButton";
            restartButton.Size = new Size(123, 23);
            restartButton.TabIndex = 8;
            restartButton.Text = "Restart Game";
            restartButton.UseVisualStyleBackColor = true;
            restartButton.Click += restartButton_Click;
            // 
            // asciiDisplay
            // 
            asciiDisplay.AutoSize = true;
            asciiDisplay.Font = new Font("Cascadia Mono", 7F);
            asciiDisplay.Location = new Point(485, 120);
            asciiDisplay.Name = "asciiDisplay";
            asciiDisplay.Size = new Size(103, 117);
            asciiDisplay.TabIndex = 9;
            asciiDisplay.Text = "░░▉▉░░▉▉░░▉▉░░▉▉\r\n▉▉░░▉▉░░▉▉░░▉▉░░\r\n░░▉▉░░▉▉░░▉▉░░▉▉\r\n▉▉░░▉▉░░▉▉░░▉▉░░\r\n░░▉▉░░▉▉░░▉▉░░▉▉\r\n▉▉░░▉▉░░▉▉░░▉▉░░\r\n░░▉▉░░▉▉░░▉▉░░▉▉\r\n▉▉░░▉▉░░▉▉░░▉▉░░\r\n\r\n";
            // 
            // player2ScoreValue
            // 
            player2ScoreValue.AutoSize = true;
            player2ScoreValue.Location = new Point(575, 62);
            player2ScoreValue.Name = "player2ScoreValue";
            player2ScoreValue.Size = new Size(13, 15);
            player2ScoreValue.TabIndex = 10;
            player2ScoreValue.Text = "0";
            // 
            // player1ScoreValue
            // 
            player1ScoreValue.AutoSize = true;
            player1ScoreValue.Location = new Point(575, 35);
            player1ScoreValue.Name = "player1ScoreValue";
            player1ScoreValue.Size = new Size(13, 15);
            player1ScoreValue.TabIndex = 11;
            player1ScoreValue.Text = "0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(486, 96);
            label5.Name = "label5";
            label5.Size = new Size(35, 15);
            label5.TabIndex = 12;
            label5.Text = "Ascii:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 519);
            Controls.Add(label5);
            Controls.Add(player1ScoreValue);
            Controls.Add(player2ScoreValue);
            Controls.Add(asciiDisplay);
            Controls.Add(restartButton);
            Controls.Add(panel2);
            Controls.Add(player2ScoreLabel);
            Controls.Add(player1ScoreLabel);
            Controls.Add(turnDataLabel);
            Controls.Add(TurnLabel);
            Controls.Add(panel1);
            Controls.Add(doubleBufferedPanel1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "BitBoard";
            Load += Form1_Load;
            Paint += Form1_Paint;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            doubleBufferedPanel1.ResumeLayout(false);
            doubleBufferedPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DoubleBufferedPanel doubleBufferedPanel1;
        private Panel panel1;
        private Label TurnLabel;
        private Label turnDataLabel;
        private Label player1ScoreLabel;
        private Label player2ScoreLabel;
        private Panel panel2;
        private Label player2HexDataLabel;
        private Label player2BinaryDataLabel;
        private Label label8;
        private Label player1HexDataLabel;
        private Label player1BinaryDataLabel;
        private Label label4;
        private Label label9;
        private Label hoverBitMaskDataLabel;
        private Label label3;
        private Label gameOverLabel;
        private Label winnerLabel;
        private Button restartButton;
        private Label asciiDisplay;
        private Label player2ScoreValue;
        private Label player1ScoreValue;
        private Label label5;
    }
}
