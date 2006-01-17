using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WinTest
{
    public partial class SoccerTest : Form
    {
        private Mojhy.Engine.Field l_objField;
        private SoccerGraphics l_objSoccerGraph;
        //indica se disegnare o meno le aree sensibili
        private bool blShowAreas = false;
        //stato dell'applicazione
        private FormStatus l_enState = FormStatus.SettingPlayerDefensePosition;
        //posizione corrente del mouse
        private Point l_ptMouseLoc;
        //posizione corrente del pallone
        private Point l_ptBall;
        //se un area è selezionata -> l_blAreaSelected = true
        private bool l_blAreaSelected = false;
        //l'enumeratore FormStatus indica lo stato corrente dell'applicativo
        public enum FormStatus
        {
            SettingPlayerAttackPosition,
            SettingPlayerDefensePosition,
            MoveBallAndEnjoy,
            PlayingMatch
        }
        //costruttore della form
        public SoccerTest()
        {
            InitializeComponent();
            //ai miei tempi, occorrevano giorni di programmazione per fare quello che fa la riga seguente!
            //Attivo DoubleBuffer, così rimuovo il flicker
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            //creo l'oggetto campo
            l_objField = new Mojhy.Engine.Field();
            //inizializzo l'oggetto SoccerGraphics per la gestione
            //degli elementi relativi al campo di calcio
            l_objSoccerGraph = new SoccerGraphics(l_objField);
            //collego l'evento sul click in form
            this.Click += new EventHandler(SoccerTest_Click);
            //collego l'evento di spostamento del mouse
            this.MouseMove += new MouseEventHandler(SoccerTest_MouseMove);
        }
        //mantiene aggiornata la posizione del mouse
        void SoccerTest_MouseMove(object sender, MouseEventArgs e)
        {
            l_ptMouseLoc.X = e.X;
            l_ptMouseLoc.Y = e.Y;
            Invalidate(new Rectangle(0,0,120,12));
        }
        //click sul campo
        void SoccerTest_Click(object sender, EventArgs e)
        {
            //calcolo il punto del campo dove sta puntando il mouse
            switch (l_enState)
            {
                case FormStatus.SettingPlayerAttackPosition:
                    //imposto la posizione del pallone (dove ho cliccato il mouse)
                    l_ptBall = l_ptMouseLoc;
                    l_blAreaSelected = true;
                    Invalidate();
                    break;
                case FormStatus.SettingPlayerDefensePosition:
                    //imposto la posizione del pallone (dove ho cliccato il mouse)
                    l_ptBall = l_ptMouseLoc;
                    l_blAreaSelected = true;
                    Invalidate();
                    break;
                case FormStatus.MoveBallAndEnjoy:
                    break;
                case FormStatus.PlayingMatch:
                    break;
            }
        }
        //evento di ridisegno della form
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //disegno il campo
            l_objSoccerGraph.RenderField(pe.Graphics, blShowAreas);
            //calcolo il punto del mouse in mm
            Point ptMouseLocMM = l_objSoccerGraph.PixelToMM(pe.Graphics, l_ptMouseLoc);
            if ((ptMouseLocMM.X > l_objField.Width) || (ptMouseLocMM.Y > l_objField.Height))
            {
                ptMouseLocMM.X = 0;
                ptMouseLocMM.Y = 0;
            }
            //attivo l'area cliccata
            if (l_blAreaSelected) l_objSoccerGraph.DrawSelectedArea(pe.Graphics, l_ptBall);
            //scrivo la posizione del mouse
            pe.Graphics.DrawString("X: " + ptMouseLocMM.X.ToString() + "mm  Y: " + ptMouseLocMM.Y.ToString() + "mm", new Font("Tahoma", 7), Brushes.White, new PointF(100, 100));
        }
        //click per visualizzare o nascondere le aree del campo
        private void btShowAreas_Click(object sender, EventArgs e)
        {
            blShowAreas = !(blShowAreas);
            Invalidate();
        }
    }
}