namespace Sodoku_WilliamStory
{
    partial class PuzzleBox
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
            this.ViewPuzzle = new System.Windows.Forms.DataGridView();
            this.C1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.b_Solve = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ViewPuzzle)).BeginInit();
            this.SuspendLayout();
            // 
            // ViewPuzzle
            // 
            this.ViewPuzzle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ViewPuzzle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.C1,
            this.C2,
            this.C3,
            this.C4,
            this.C5,
            this.C6,
            this.C7,
            this.C8,
            this.C9});
            this.ViewPuzzle.Location = new System.Drawing.Point(3, 5);
            this.ViewPuzzle.Name = "ViewPuzzle";
            this.ViewPuzzle.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.ViewPuzzle.Size = new System.Drawing.Size(510, 364);
            this.ViewPuzzle.TabIndex = 0;
            this.ViewPuzzle.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ViewPuzzle_CellContentClick);
            // 
            // C1
            // 
            this.C1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C1.HeaderText = "C1";
            this.C1.Name = "C1";
            this.C1.ReadOnly = true;
            this.C1.Width = 45;
            // 
            // C2
            // 
            this.C2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C2.HeaderText = "C2";
            this.C2.Name = "C2";
            this.C2.ReadOnly = true;
            this.C2.Width = 45;
            // 
            // C3
            // 
            this.C3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C3.HeaderText = "C3";
            this.C3.Name = "C3";
            this.C3.ReadOnly = true;
            this.C3.Width = 45;
            // 
            // C4
            // 
            this.C4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C4.HeaderText = "C4";
            this.C4.Name = "C4";
            this.C4.ReadOnly = true;
            this.C4.Width = 45;
            // 
            // C5
            // 
            this.C5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C5.HeaderText = "C5";
            this.C5.Name = "C5";
            this.C5.ReadOnly = true;
            this.C5.Width = 45;
            // 
            // C6
            // 
            this.C6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C6.HeaderText = "C6";
            this.C6.Name = "C6";
            this.C6.ReadOnly = true;
            this.C6.Width = 45;
            // 
            // C7
            // 
            this.C7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C7.HeaderText = "C7";
            this.C7.Name = "C7";
            this.C7.ReadOnly = true;
            this.C7.Width = 45;
            // 
            // C8
            // 
            this.C8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C8.HeaderText = "C8";
            this.C8.Name = "C8";
            this.C8.ReadOnly = true;
            this.C8.Width = 45;
            // 
            // C9
            // 
            this.C9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.C9.HeaderText = "C9";
            this.C9.Name = "C9";
            this.C9.ReadOnly = true;
            this.C9.Width = 45;
            // 
            // b_Solve
            // 
            this.b_Solve.Location = new System.Drawing.Point(44, 407);
            this.b_Solve.Name = "b_Solve";
            this.b_Solve.Size = new System.Drawing.Size(75, 23);
            this.b_Solve.TabIndex = 1;
            this.b_Solve.Text = "Solve!";
            this.b_Solve.UseVisualStyleBackColor = true;
            this.b_Solve.Click += new System.EventHandler(this.b_Solve_Click);
            // 
            // PuzzleBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 459);
            this.Controls.Add(this.b_Solve);
            this.Controls.Add(this.ViewPuzzle);
            this.Name = "PuzzleBox";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ViewPuzzle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ViewPuzzle;
        private System.Windows.Forms.DataGridViewTextBoxColumn C1;
        private System.Windows.Forms.DataGridViewTextBoxColumn C2;
        private System.Windows.Forms.DataGridViewTextBoxColumn C3;
        private System.Windows.Forms.DataGridViewTextBoxColumn C4;
        private System.Windows.Forms.DataGridViewTextBoxColumn C5;
        private System.Windows.Forms.DataGridViewTextBoxColumn C6;
        private System.Windows.Forms.DataGridViewTextBoxColumn C7;
        private System.Windows.Forms.DataGridViewTextBoxColumn C8;
        private System.Windows.Forms.DataGridViewTextBoxColumn C9;
        private System.Windows.Forms.Button b_Solve;
    }
}

