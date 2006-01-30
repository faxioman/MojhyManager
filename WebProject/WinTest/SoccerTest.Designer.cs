namespace WinTest
{
    partial class SoccerTest
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
            this.btShowAreas = new System.Windows.Forms.Button();
            this.btShowDefense = new System.Windows.Forms.Button();
            this.btShowAttack = new System.Windows.Forms.Button();
            this.btSavePositions = new System.Windows.Forms.Button();
            this.btLoadPositions = new System.Windows.Forms.Button();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btShowAreas
            // 
            this.btShowAreas.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btShowAreas.Location = new System.Drawing.Point(12, 538);
            this.btShowAreas.Name = "btShowAreas";
            this.btShowAreas.Size = new System.Drawing.Size(104, 23);
            this.btShowAreas.TabIndex = 0;
            this.btShowAreas.Text = "SHOW/HIDE AREAS";
            this.btShowAreas.UseVisualStyleBackColor = true;
            this.btShowAreas.Click += new System.EventHandler(this.btShowAreas_Click);
            // 
            // btShowDefense
            // 
            this.btShowDefense.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btShowDefense.Location = new System.Drawing.Point(119, 538);
            this.btShowDefense.Name = "btShowDefense";
            this.btShowDefense.Size = new System.Drawing.Size(118, 23);
            this.btShowDefense.TabIndex = 1;
            this.btShowDefense.Text = "SHOW/HIDE DEFENSE";
            this.btShowDefense.UseVisualStyleBackColor = true;
            this.btShowDefense.Click += new System.EventHandler(this.btShowDefense_Click);
            // 
            // btShowAttack
            // 
            this.btShowAttack.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btShowAttack.Location = new System.Drawing.Point(240, 538);
            this.btShowAttack.Name = "btShowAttack";
            this.btShowAttack.Size = new System.Drawing.Size(111, 23);
            this.btShowAttack.TabIndex = 2;
            this.btShowAttack.Text = "SHOW/HIDE ATTACK";
            this.btShowAttack.UseVisualStyleBackColor = true;
            this.btShowAttack.Click += new System.EventHandler(this.btShowAttack_Click);
            // 
            // btSavePositions
            // 
            this.btSavePositions.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSavePositions.Location = new System.Drawing.Point(354, 538);
            this.btSavePositions.Name = "btSavePositions";
            this.btSavePositions.Size = new System.Drawing.Size(97, 23);
            this.btSavePositions.TabIndex = 3;
            this.btSavePositions.Text = "SAVE POSITIONS";
            this.btSavePositions.UseVisualStyleBackColor = true;
            this.btSavePositions.Click += new System.EventHandler(this.btSavePositions_Click);
            // 
            // btLoadPositions
            // 
            this.btLoadPositions.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLoadPositions.Location = new System.Drawing.Point(454, 538);
            this.btLoadPositions.Name = "btLoadPositions";
            this.btLoadPositions.Size = new System.Drawing.Size(97, 23);
            this.btLoadPositions.TabIndex = 4;
            this.btLoadPositions.Text = "LOAD POSITIONS";
            this.btLoadPositions.UseVisualStyleBackColor = true;
            this.btLoadPositions.Click += new System.EventHandler(this.btLoadPositions_Click);
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
            // SoccerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(795, 570);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.btLoadPositions);
            this.Controls.Add(this.btSavePositions);
            this.Controls.Add(this.btShowAttack);
            this.Controls.Add(this.btShowDefense);
            this.Controls.Add(this.btShowAreas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SoccerTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Soccer Test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btShowAreas;
        private System.Windows.Forms.Button btShowDefense;
        private System.Windows.Forms.Button btShowAttack;
        private System.Windows.Forms.Button btSavePositions;
        private System.Windows.Forms.Button btLoadPositions;
        private System.Windows.Forms.ComboBox cbStatus;
    }
}

