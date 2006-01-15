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
        //l'enumeratore FormStatus indica lo stato corrente dell'applicativo
        public enum FormStatus
        {
            SettingPlayerAttackPosition,
            SettingPlayerDefensePosition,
            MoveBallAndEnjoy,
            PlayingMatch
        }
        public SoccerTest()
        {
            InitializeComponent();
            //creo l'oggetto campo
            l_objField = new Mojhy.Engine.Field();
            //inizializzo l'oggetto SoccerGraphics per la gestione
            //degli elementi relativi al campo di calcio
            l_objSoccerGraph = new SoccerGraphics(l_objField);
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //disegno il campo
            l_objSoccerGraph.RenderField(pe.Graphics, blShowAreas);
        }

        private void btShowAreas_Click(object sender, EventArgs e)
        {
            blShowAreas = !(blShowAreas);
            this.Refresh();
        }
    }
}