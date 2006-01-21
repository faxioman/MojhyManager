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
        //squadre
        private Mojhy.Engine.Team l_objTeamA;
        private Mojhy.Engine.Team l_objTeamB;
        //indice del giocatore selezionato col mouse
        private int l_intSelectedPlayerIndex = -1;
        //indica se disegnare o meno le aree sensibili
        private bool blShowAreas = false;
        //stato dell'applicazione
        private FormStatus l_enState = FormStatus.SettingPlayerPosition;
        //posizione corrente del mouse
        private Point l_ptMouseLoc;
        //posizione corrente del pallone
        private Point l_ptBall;
        //se un area è selezionata -> l_blAreaSelected = true
        private bool l_blAreaSelected = false;
        //le variabili definiscono se, in fase di selezione dei giocatori,
        //devo visualizzare le posizioni in attacco o in difesa o entrambe
        private bool l_blShowAttack = true;
        private bool l_blShowDefense = true;
        //definisco se sono in fase di drag
        private bool l_blDragging = false;
        //l'enumeratore FormStatus indica lo stato corrente dell'applicativo
        public enum FormStatus
        {
            SettingPlayerPosition,
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
            //collego l'evento di spostamento del mouse
            this.MouseMove += new MouseEventHandler(SoccerTest_MouseMove);
            //collego l'evento di mouse down
            this.MouseDown += new MouseEventHandler(SoccerTest_MouseDown);
            //collego l'evento di mouse up
            this.MouseUp += new MouseEventHandler(SoccerTest_MouseUp);
            //inizializzo la prima squadra (presente in ogni situazione)
            l_objTeamA = new Mojhy.Engine.Team();
            //inizializzo la posizione dei giocatori (posizione iniziale, tutti a meta campo)
            l_objTeamA.PutOnField(l_objField);
            foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
            {
                objPlayingPlayer.PositionOnField = new Point(l_objField.Width / 2, l_objField.Height / 2);
            }
        }

        void SoccerTest_MouseUp(object sender, MouseEventArgs e)
        {
            l_blDragging = false;
            if (l_enState == FormStatus.SettingPlayerPosition)
            {
                //se non è selezionato nessun giocatore, allore sposto la posizione attuale del pallone
                if (l_intSelectedPlayerIndex == -1)
                {
                    l_ptBall = l_ptMouseLoc;
                    l_blAreaSelected = true;
                    Invalidate();
                }
            }
            //deseleziono qualsiasi eventuale giocatore selezionato
            l_intSelectedPlayerIndex = -1;
        }

        void SoccerTest_MouseDown(object sender, MouseEventArgs e)
        {
            l_blDragging = true;
            if (l_enState == FormStatus.SettingPlayerPosition)
            {
                //verifico se sono sopra un giocatore e se quindi lo devo
                //selezionare per lo spostamento
                //calcolo il rettangolo che definisce il range di selezione del giocatore
                Point ptMousePositionInField = l_objSoccerGraph.PixelToMM(l_ptMouseLoc);
                Rectangle rctSelectionArea = new Rectangle(ptMousePositionInField.X - 1500, ptMousePositionInField.Y - 1500, 3000, 3000);
                foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
                {
                    if (rctSelectionArea.Contains(objPlayingPlayer.PositionOnField))
                    {
                        l_intSelectedPlayerIndex = objPlayingPlayer.Index;
                        break;
                    }
                }
            }
        }
        //mantiene aggiornata la posizione del mouse
        void SoccerTest_MouseMove(object sender, MouseEventArgs e)
        {
            l_ptMouseLoc.X = e.X;
            l_ptMouseLoc.Y = e.Y;
            if (l_enState == FormStatus.SettingPlayerPosition)
            {
                //verifico se devo spostare un giocatore
                if ((l_blDragging) && (l_intSelectedPlayerIndex != -1))
                {

                    Point ptOldPosition = l_objSoccerGraph.MMToPixel(l_objTeamA.PlayingPlayers[l_intSelectedPlayerIndex].PositionOnField);
                    Point ptNewPosition = l_ptMouseLoc;
                    l_objTeamA.PlayingPlayers[l_intSelectedPlayerIndex].PositionOnField = l_objSoccerGraph.PixelToMM(ptNewPosition);
                    Invalidate(new Rectangle(ptOldPosition.X - 20, ptOldPosition.Y -20, 40, 40));
                    Invalidate(new Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));
                }
            }
            //invalido l'area dove scrivere la posizione del mouse
            Invalidate(new Rectangle(0,0,120,12));

        }
        //evento di ridisegno della form
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (l_objSoccerGraph == null)
            {
                //inizializzo l'oggetto SoccerGraphics per la gestione
                //degli elementi relativi al campo di calcio
                l_objSoccerGraph = new SoccerGraphics(pe.Graphics);
            }
            else
            {
                //imposto l'oggetto Graphics per SoccerGraphics
                l_objSoccerGraph.SetGraphicsObject(pe.Graphics);
            }
            //disegno il campo
            l_objSoccerGraph.RenderField(l_objField, blShowAreas);
            //calcolo il punto del mouse in mm
            Point ptMouseLocMM = l_objSoccerGraph.PixelToMM(l_ptMouseLoc);
            if ((ptMouseLocMM.X > l_objField.Width) || (ptMouseLocMM.Y > l_objField.Height))
            {
                ptMouseLocMM.X = 0;
                ptMouseLocMM.Y = 0;
            }
            //attivo l'area cliccata
            if (l_blAreaSelected) l_objSoccerGraph.DrawSelectedArea(l_ptBall, l_objField);
            //controllo se devo visualizzare i giocatori in posizione attacco, difesa, entrambi o nascosti
            foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
            {
                l_objSoccerGraph.DrawPlayer(objPlayingPlayer, Mojhy.Engine.PlayingPlayer.PlayerPositionType.Defense);
            }
            //scrivo la posizione del mouse
            pe.Graphics.DrawString("X: " + ptMouseLocMM.X.ToString() + "mm  Y: " + ptMouseLocMM.Y.ToString() + "mm", new Font("Tahoma", 7), Brushes.White, new PointF(100, 100));
        }
        //click per visualizzare o nascondere le aree del campo
        private void btShowAreas_Click(object sender, EventArgs e)
        {
            blShowAreas = !(blShowAreas);
            Invalidate();
        }
        //click per visualizzare o nascondere le posizioni in difesa
        private void btShowDefense_Click(object sender, EventArgs e)
        {

        }
        //click per visualizzare o nascondere le posizioni in attacco
        private void btShowAttack_Click(object sender, EventArgs e)
        {

        }
    }
}