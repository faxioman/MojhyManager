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
            // SoccerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(795, 570);
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
    }
}

