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
        //dimensioni del campo in mm
        private const float fieldWidth = 105000;
        private const float fieldHeight = 70000;
        private const float portaWidth = 7320;

        public SoccerTest()
        {
            InitializeComponent();
            this.Paint += SoccerTest_Paint;
        }

        private void SoccerTest_Paint(object sender, System.Windows.Forms.PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            // disegno il campo
            //unità di misura in millimetri
            g.PageUnit = GraphicsUnit.Millimeter;
            //imposto la scala, visto che considero il campo con le sue misure reali
            g.PageScale =(float)0.001;
            //disegno il fondo
            g.FillRectangle(Brushes.Green, 0, 0, fieldWidth, fieldHeight);
            //disegno le aree di rigore
            g.DrawLine(Pens.White, new PointF(0, (fieldHeight - portaWidth)/2), new PointF(0,1));
        }
    }
}