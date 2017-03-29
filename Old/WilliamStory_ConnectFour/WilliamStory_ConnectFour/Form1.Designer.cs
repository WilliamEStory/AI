namespace WilliamStory_ConnectFour
{
    partial class Form1
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
            this.dgv_Board = new System.Windows.Forms.DataGridView();
            this.tb_Column = new System.Windows.Forms.TextBox();
            this.b_Play = new System.Windows.Forms.Button();
            this.tb_Rows = new System.Windows.Forms.TextBox();
            this.tb_Columns = new System.Windows.Forms.TextBox();
            this.tb_ConnectR = new System.Windows.Forms.TextBox();
            this.b_Generate = new System.Windows.Forms.Button();
            this.cb_AIFirst = new System.Windows.Forms.CheckBox();
            this.cb_PlayerVsAI = new System.Windows.Forms.CheckBox();
            this.tb_Depth = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Board)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Board
            // 
            this.dgv_Board.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Board.Location = new System.Drawing.Point(13, 13);
            this.dgv_Board.Name = "dgv_Board";
            this.dgv_Board.Size = new System.Drawing.Size(792, 400);
            this.dgv_Board.TabIndex = 0;
            // 
            // tb_Column
            // 
            this.tb_Column.Location = new System.Drawing.Point(13, 435);
            this.tb_Column.Name = "tb_Column";
            this.tb_Column.Size = new System.Drawing.Size(100, 20);
            this.tb_Column.TabIndex = 1;
            this.tb_Column.Text = "Column";
            // 
            // b_Play
            // 
            this.b_Play.Location = new System.Drawing.Point(13, 462);
            this.b_Play.Name = "b_Play";
            this.b_Play.Size = new System.Drawing.Size(75, 23);
            this.b_Play.TabIndex = 2;
            this.b_Play.Text = "Add Play";
            this.b_Play.UseVisualStyleBackColor = true;
            this.b_Play.Click += new System.EventHandler(this.b_AddColumn_Click);
            // 
            // tb_Rows
            // 
            this.tb_Rows.Location = new System.Drawing.Point(243, 434);
            this.tb_Rows.Name = "tb_Rows";
            this.tb_Rows.Size = new System.Drawing.Size(100, 20);
            this.tb_Rows.TabIndex = 3;
            this.tb_Rows.Text = "Rows";
            // 
            // tb_Columns
            // 
            this.tb_Columns.Location = new System.Drawing.Point(349, 435);
            this.tb_Columns.Name = "tb_Columns";
            this.tb_Columns.Size = new System.Drawing.Size(100, 20);
            this.tb_Columns.TabIndex = 4;
            this.tb_Columns.Text = "Columns";
            // 
            // tb_ConnectR
            // 
            this.tb_ConnectR.Location = new System.Drawing.Point(455, 435);
            this.tb_ConnectR.Name = "tb_ConnectR";
            this.tb_ConnectR.Size = new System.Drawing.Size(100, 20);
            this.tb_ConnectR.TabIndex = 5;
            this.tb_ConnectR.Text = "Connect R";
            // 
            // b_Generate
            // 
            this.b_Generate.Location = new System.Drawing.Point(243, 462);
            this.b_Generate.Name = "b_Generate";
            this.b_Generate.Size = new System.Drawing.Size(75, 23);
            this.b_Generate.TabIndex = 6;
            this.b_Generate.Text = "Generate";
            this.b_Generate.UseVisualStyleBackColor = true;
            this.b_Generate.Click += new System.EventHandler(this.b_Generate_Click);
            // 
            // cb_AIFirst
            // 
            this.cb_AIFirst.AutoSize = true;
            this.cb_AIFirst.Enabled = false;
            this.cb_AIFirst.Location = new System.Drawing.Point(455, 468);
            this.cb_AIFirst.Name = "cb_AIFirst";
            this.cb_AIFirst.Size = new System.Drawing.Size(92, 17);
            this.cb_AIFirst.TabIndex = 7;
            this.cb_AIFirst.Text = "AI Goes First?";
            this.cb_AIFirst.UseVisualStyleBackColor = true;
            this.cb_AIFirst.CheckedChanged += new System.EventHandler(this.cb_AIFirst_CheckedChanged);
            // 
            // cb_PlayerVsAI
            // 
            this.cb_PlayerVsAI.AutoSize = true;
            this.cb_PlayerVsAI.Location = new System.Drawing.Point(349, 468);
            this.cb_PlayerVsAI.Name = "cb_PlayerVsAI";
            this.cb_PlayerVsAI.Size = new System.Drawing.Size(86, 17);
            this.cb_PlayerVsAI.TabIndex = 8;
            this.cb_PlayerVsAI.Text = "Player Vs. AI";
            this.cb_PlayerVsAI.UseVisualStyleBackColor = true;
            this.cb_PlayerVsAI.CheckedChanged += new System.EventHandler(this.cb_PlayerVsAI_CheckedChanged);
            // 
            // tb_Depth
            // 
            this.tb_Depth.Location = new System.Drawing.Point(562, 434);
            this.tb_Depth.Name = "tb_Depth";
            this.tb_Depth.Size = new System.Drawing.Size(100, 20);
            this.tb_Depth.TabIndex = 9;
            this.tb_Depth.Text = "Depth";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 579);
            this.Controls.Add(this.tb_Depth);
            this.Controls.Add(this.cb_PlayerVsAI);
            this.Controls.Add(this.cb_AIFirst);
            this.Controls.Add(this.b_Generate);
            this.Controls.Add(this.tb_ConnectR);
            this.Controls.Add(this.tb_Columns);
            this.Controls.Add(this.tb_Rows);
            this.Controls.Add(this.b_Play);
            this.Controls.Add(this.tb_Column);
            this.Controls.Add(this.dgv_Board);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Board)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Board;
        private System.Windows.Forms.TextBox tb_Column;
        private System.Windows.Forms.Button b_Play;
        private System.Windows.Forms.TextBox tb_Rows;
        private System.Windows.Forms.TextBox tb_Columns;
        private System.Windows.Forms.TextBox tb_ConnectR;
        private System.Windows.Forms.Button b_Generate;
        private System.Windows.Forms.CheckBox cb_AIFirst;
        private System.Windows.Forms.CheckBox cb_PlayerVsAI;
        private System.Windows.Forms.TextBox tb_Depth;
    }
}

