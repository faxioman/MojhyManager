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
            this.SuspendLayout();
            // 
            // btShowAreas
            // 
            this.btShowAreas.Location = new System.Drawing.Point(12, 538);
            this.btShowAreas.Name = "btShowAreas";
            this.btShowAreas.Size = new System.Drawing.Size(118, 23);
            this.btShowAreas.TabIndex = 0;
            this.btShowAreas.Text = "Show/Hide Areas";
            this.btShowAreas.UseVisualStyleBackColor = true;
            this.btShowAreas.Click += new System.EventHandler(this.btShowAreas_Click);
            // 
            // btShowDefense
            // 
            this.btShowDefense.Location = new System.Drawing.Point(136, 538);
            this.btShowDefense.Name = "btShowDefense";
            this.btShowDefense.Size = new System.Drawing.Size(118, 23);
            this.btShowDefense.TabIndex = 1;
            this.btShowDefense.Text = "Show/Hide Defense";
            this.btShowDefense.UseVisualStyleBackColor = true;
            this.btShowDefense.Click += new System.EventHandler(this.btShowDefense_Click);
            // 
            // btShowAttack
            // 
            this.btShowAttack.Location = new System.Drawing.Point(260, 538);
            this.btShowAttack.Name = "btShowAttack";
            this.btShowAttack.Size = new System.Drawing.Size(118, 23);
            this.btShowAttack.TabIndex = 2;
            this.btShowAttack.Text = "Show/Hide Attack";
            this.btShowAttack.UseVisualStyleBackColor = true;
            this.btShowAttack.Click += new System.EventHandler(this.btShowAttack_Click);
            // 
            // SoccerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(795, 570);
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
    }
}

