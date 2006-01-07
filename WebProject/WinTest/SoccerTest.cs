using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinTest
{
    public partial class SoccerTest : Form
    {
        public SoccerTest()
        {
            InitializeComponent();
            this.Paint += SoccerTest_Paint;
        }

        private void SoccerTest_Paint(object sender, System.Windows.Forms.PaintEventArgs pe)
        {
            //attivo l'antialias
            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //disegno il campo
            SoccerGraphics objSoccerGraph = new SoccerGraphics();
            objSoccerGraph.RenderField(pe.Graphics);
        }
    }
}