namespace WinTest
{
    partial class MovePlayerTest
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
            this.components = new System.ComponentModel.Container();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.btnStartMove = new System.Windows.Forms.Button();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.btLoadPositions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "Players positions",
            "Move ball",
            "Play match"});
            this.cbStatus.Location = new System.Drawing.Point(662, 539);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(121, 21);
            this.cbStatus.TabIndex = 5;
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.cbStatus_SelectedIndexChanged);
            // 
            // btnStartMove
            // 
            this.btnStartMove.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartMove.Location = new System.Drawing.Point(2, 549);
            this.btnStartMove.Name = "btnStartMove";
            this.btnStartMove.Size = new System.Drawing.Size(119, 18);
            this.btnStartMove.TabIndex = 7;
            this.btnStartMove.Text = "START/STOP MOVING PLAYER";
            this.btnStartMove.UseVisualStyleBackColor = true;
            this.btnStartMove.Click += new System.EventHandler(this.btnStartMove_Click);
            // 
            // Timer1
            // 
            this.Timer1.Interval = 1;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // btLoadPositions
            // 
            this.btLoadPositions.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLoadPositions.Location = new System.Drawing.Point(127, 549);
            this.btLoadPositions.Name = "btLoadPositions";
            this.btLoadPositions.Size = new System.Drawing.Size(97, 20);
            this.btLoadPositions.TabIndex = 8;
            this.btLoadPositions.Text = "LOAD POSITIONS";
            this.btLoadPositions.UseVisualStyleBackColor = true;
            this.btLoadPositions.Click += new System.EventHandler(this.btLoadPositions_Click);
            // 
            // MovePlayerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(795, 574);
            this.Controls.Add(this.btLoadPositions);
            this.Controls.Add(this.btnStartMove);
            this.Controls.Add(this.cbStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MovePlayerTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Soccer Test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Button btnStartMove;
        internal System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.Button btLoadPositions;
    }
}

