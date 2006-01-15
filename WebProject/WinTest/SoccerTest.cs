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
        private Mojhy.Engine.Field l_objField;
        private SoccerGraphics l_objSoccerGraph;
        //indica se disegnare o meno le aree sensibili
        private bool blShowAreas = false;
        //stato dell'applicazione
        private FormStatus l_enState = FormStatus.SettingPlayerDefensePosition;
        //posizione corrente del mouse
        private Point l_ptMouseLoc;
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
            Invalidate();
        }
        //click sul campo
        void SoccerTest_Click(object sender, EventArgs e)
        {
            switch (l_enState)
            {
                case FormStatus.SettingPlayerAttackPosition:
                    
                    break;
                case FormStatus.SettingPlayerDefensePosition:
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
            //scrivo la posizione del mouse
            pe.Graphics.DrawString("Mouse x: " + l_ptMouseLoc.X.ToString() + "  Mouse y: " + l_ptMouseLoc.Y.ToString(), new Font("Arial",12), Brushes.White,new PointF(100,100));
        }
        //click per visualizzare o nascondere le aree del campo
        private void btShowAreas_Click(object sender, EventArgs e)
        {
            blShowAreas = !(blShowAreas);
            Invalidate();
        }
    }
}