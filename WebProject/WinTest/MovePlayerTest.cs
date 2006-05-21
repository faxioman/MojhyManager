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
    public partial class MovePlayerTest : Form
    {
        
#region Variabili Locali

        private const int MINX = 8;
        private const int MAXX = 786;
        private const int MINY = 8;
        private const int MAXY = 520;
        private Mojhy.Engine.Field l_objField;
        private SoccerGraphics l_objSoccerGraph;
        
        //squadra A
        private Mojhy.Engine.Team l_objTeamA;
        
        //indice del giocatore selezionato col mouse
        private int l_intSelectedDefensePlayerIndex = -1;
        private int l_intSelectedAttackPlayerIndex = -1;
        
        //indica se disegnare o meno le aree sensibili
        private bool blShowAreas = true;

        //indica se visualizzare il giocatore che si deve muovere
        private bool l_blShowMovingPlayer = false;

        //indica se il pallone è stato raggiunto
        private bool l_blBallEated = false; // <--- ;-))))))

        //stato dell'applicazione
        private FormStatus l_enState = FormStatus.SettingPlayerPosition;

        //posizione corrente del mouse
        private Point l_ptMouseLoc;
        
        //posizione corrente del pallone
        private Point l_ptBall;

        //area corrente selezionata
        private int l_intSelectedAreaIndex = -1;

        //se un area è selezionata -> l_blAreaSelected = true
        private bool l_blAreaSelected = false;

        //definisco se sono in fase di drag
        private bool l_blDragging = false;

        //indica il tempo necessario per raggiungere la palla ,utilizzato in debug !!!
        private double l_dblTime;

        //indica il momento in cui il giocatore inizia a rincorrere la palla
        private System.DateTime l_dtStart;

        //indica l'angolo con cui il giocatore deve muoveris verso la palla
        private double[] l_dblPlayerAngle = new double[11];

        //indica la velocità del giocatore
        private double l_dblSpeed = 100;

        private int l_intAreaDebug;

        #endregion

        //l'enumeratore FormStatus indica lo stato corrente dell'applicativo
        public enum FormStatus
        {
            SettingPlayerPosition,
            MoveBallAndEnjoy,
            PlayingMatch
        }

        //costruttore della form
        public MovePlayerTest()
        {
            
            InitializeComponent();
            //ai miei tempi, occorrevano giorni di programmazione per fare quello che fa la riga seguente!
            //Attivo DoubleBuffer, così rimuovo il flicker
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            
            //creo l'oggetto campo
            l_objField = new Mojhy.Engine.Field();

            
            //collego l'evento di mouse up
            this.MouseUp += new MouseEventHandler(SoccerTest_MouseUp);

            //collego l'evento di spostamento del mouse
            this.MouseMove += new MouseEventHandler(SoccerTest_MouseMove);

            //inizializzo la prima squadra (presente in ogni situazione)
            l_objTeamA = new Mojhy.Engine.Team();

            //inizializzo le posizione dei giocatori (posizione iniziale, tutti a meta campo)
            //viene inizializzata anche la posizione corrente del giocatore !!!
            l_objTeamA.PutOnField(l_objField);
                        
            foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
            {
                for (int i = 0; i < objPlayingPlayer.PositionsOnField.AttackPositions.Length; i++)
                {
                    objPlayingPlayer.PositionsOnField.AttackPositions[i] = new Point((l_objField.Width / 2)+10000, l_objField.Height / 2);                    
                }

            }
                        

            //inizializzo la combo dello stato allo stato attuale
            cbStatus.SelectedIndex = (int)this.l_enState;
        }

        //restituisce un punto valido del campo in pixel
        private Point GetValidFieldPosition(Point ptRawPoint)
        {
            Point ptValidPosition = new Point(ptRawPoint.X,ptRawPoint.Y);
            if (ptValidPosition.X < MINX) ptValidPosition.X = MINX;
            if (ptValidPosition.X > MAXX) ptValidPosition.X = MAXX;
            if (ptValidPosition.Y < MINY) ptValidPosition.Y = MINY;
            if (ptValidPosition.Y > MAXY) ptValidPosition.Y = MAXY;
            return ptValidPosition;
        }

        //restituisce un punto valido del campo in pixel
        private Point GetMMValidFieldPosition(Point ptMMPoint)
        {
            
            Point ptValidPosition = new Point(ptMMPoint.X, ptMMPoint.Y);
            Point ptMinValidPosition = l_objSoccerGraph.PixelToMM(new Point(MINX,MINY));
            Point ptMaxValidPosition = l_objSoccerGraph.PixelToMM(new Point(MAXX, MAXY));

            if (ptValidPosition.X < ptMinValidPosition.X) ptValidPosition.X = ptMinValidPosition.X;
            if (ptValidPosition.X > ptMaxValidPosition.X) ptValidPosition.X = ptMaxValidPosition.X;
            if (ptValidPosition.Y < ptMinValidPosition.Y) ptValidPosition.Y = ptMinValidPosition.Y;
            if (ptValidPosition.Y > ptMaxValidPosition.Y) ptValidPosition.Y = ptMaxValidPosition.Y;
            return ptValidPosition;
        }

        //eventu sul rilascio del mouse
        void SoccerTest_MouseUp(object sender, MouseEventArgs e)
        {

            l_ptBall = l_ptMouseLoc;            
            
            l_blDragging = false;
            
            if (l_enState == FormStatus.SettingPlayerPosition)
            {
                //se non è selezionato nessun giocatore, allore sposto la posizione attuale del pallone
                //e calcolo la relativa area
                if ((l_intSelectedDefensePlayerIndex == -1)&&(l_intSelectedAttackPlayerIndex == -1))
                {
                   
                    l_blAreaSelected = true;
                    
                    Point ptLocBallMM = l_objSoccerGraph.PixelToMM(l_ptBall);

                    Mojhy.Engine.PlayArea objPlayAreaAux = l_objField.Areas.GetAreaFromLoc(ptLocBallMM);
                    
                    if (objPlayAreaAux != null)
                    {
                        l_intSelectedAreaIndex = objPlayAreaAux.Index;

                        foreach (Mojhy.Engine.PlayingPlayer objPlayer in l_objTeamA.PlayingPlayers)
                        {
                            objPlayer.CurrentPositionOnField = objPlayer.PositionsOnField.AttackPositions[l_intSelectedAreaIndex];
                        }  

                        Invalidate();
                    }
                                        
                }
                else
                {
                    Cursor.Show();
                }
            }

            if (l_enState == FormStatus.PlayingMatch)
            {

                l_blAreaSelected = true;

                Point ptLocBallMM = l_objSoccerGraph.PixelToMM(l_ptBall);

                Mojhy.Engine.PlayArea objPlayAreaAux = l_objField.Areas.GetAreaFromLoc(ptLocBallMM);

                if (objPlayAreaAux != null)
                {
                    l_intSelectedAreaIndex = objPlayAreaAux.Index;                    
                }

                int i= 0;
                foreach (Mojhy.Engine.PlayingPlayer objPlayer in l_objTeamA.PlayingPlayers)
                {

                    //imposta la pozione di partenza del giocatore e la posizione del pallone
                    // calcola quindi l'angolo con cui il giocatore si deve muovere
                    Point ptStartPosition = l_objSoccerGraph.MMToPixel(objPlayer.CurrentPositionOnField);

                    l_blBallEated = false;

                    Point ptBallMM = l_objSoccerGraph.PixelToMM(l_ptBall);

                    Point ptDefaultPlayerPosition = objPlayer.PositionsOnField.AttackPositions[l_intSelectedAreaIndex];

                    if (objPlayer.CurrentPlayArea.Index == l_intSelectedAreaIndex)
                    {
                        l_dblPlayerAngle[i] = Angle(objPlayer.CurrentPositionOnField.X, objPlayer.CurrentPositionOnField.Y, ptBallMM.X, ptBallMM.Y);
                    }
                    else
                    {
                        l_dblPlayerAngle[i] = Angle(objPlayer.CurrentPositionOnField.X, objPlayer.CurrentPositionOnField.Y, ptDefaultPlayerPosition.X, ptDefaultPlayerPosition.Y);
                    }                                      

                    i++;
                }
                               
                
                Invalidate();

              }

            //deseleziono qualsiasi eventuale giocatore selezionato
            l_intSelectedDefensePlayerIndex = -1;
            l_intSelectedAttackPlayerIndex = -1;
        }


        //mantiene aggiornata la posizione del mouse
        void SoccerTest_MouseMove(object sender, MouseEventArgs e)
        {
            l_ptMouseLoc.X = e.X;
            l_ptMouseLoc.Y = e.Y;
            
            if (l_enState == FormStatus.SettingPlayerPosition)
            {
                
                //verifico se devo spostare un giocatore
                if ((l_blDragging) && ((l_intSelectedDefensePlayerIndex != -1) || (l_intSelectedAttackPlayerIndex != -1)))
                {
                    if (l_intSelectedDefensePlayerIndex != -1)
                    {
                        Point ptOldPosition = l_objSoccerGraph.MMToPixel(l_objTeamA.PlayingPlayers[l_intSelectedDefensePlayerIndex].PositionsOnField.DefensePositions[l_intSelectedAreaIndex]);
                        Point ptNewPosition = l_ptMouseLoc;
                        ptNewPosition = GetValidFieldPosition(ptNewPosition);
                        l_objTeamA.PlayingPlayers[l_intSelectedDefensePlayerIndex].PositionsOnField.DefensePositions[l_intSelectedAreaIndex] = l_objSoccerGraph.PixelToMM(ptNewPosition);
                        //l_objTeamA.PlayingPlayers[l_intSelectedDefensePlayerIndex].CurrentPositionOnField = l_objSoccerGraph.PixelToMM(ptNewPosition);
                        Invalidate(new Rectangle(ptOldPosition.X - 20, ptOldPosition.Y - 20, 40, 40));
                        Invalidate(new Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));
                    }
                    else
                    {
                        Point ptOldPosition = l_objSoccerGraph.MMToPixel(l_objTeamA.PlayingPlayers[l_intSelectedAttackPlayerIndex].PositionsOnField.AttackPositions[l_intSelectedAreaIndex]);
                        Point ptNewPosition = l_ptMouseLoc;
                        ptNewPosition = GetValidFieldPosition(ptNewPosition);
                        l_objTeamA.PlayingPlayers[l_intSelectedAttackPlayerIndex].PositionsOnField.AttackPositions[l_intSelectedAreaIndex] = l_objSoccerGraph.PixelToMM(ptNewPosition);
                        l_objTeamA.PlayingPlayers[l_intSelectedAttackPlayerIndex].CurrentPositionOnField = l_objSoccerGraph.PixelToMM(ptNewPosition);
                        Invalidate(new Rectangle(ptOldPosition.X - 20, ptOldPosition.Y - 20, 40, 40));
                        Invalidate(new Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));
                    }
                }


            }
            
        }

        //evento di modifica della compbo dello stato
        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ComboBox cbStatusAux = (ComboBox)sender;
            l_enState = (FormStatus)cbStatusAux.SelectedIndex;
            Invalidate();
                        
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

            //disegno il pallone
            l_objSoccerGraph.DrawBall(l_objSoccerGraph.PixelToMM(l_ptBall));

            //calcolo il punto del mouse in mm
            Point ptMouseLocMM = l_objSoccerGraph.PixelToMM(l_ptMouseLoc);
            Point ptMouseLocTmp = l_ptMouseLoc;
            if ((ptMouseLocMM.X > l_objField.Width) || (ptMouseLocMM.Y > l_objField.Height) || (ptMouseLocMM.X < 0) || (ptMouseLocMM.Y < 0))
            {
                ptMouseLocMM.X = 0;
                ptMouseLocMM.Y = 0;
                ptMouseLocTmp.X = 0;
                ptMouseLocTmp.Y = 0;
            }

            //attivo l'area cliccata
            if ((l_blAreaSelected) && (l_intSelectedAreaIndex != -1)) l_objSoccerGraph.DrawSelectedAreaNoBall(l_intSelectedAreaIndex, l_objField);


            if (l_enState == FormStatus.SettingPlayerPosition)
            {

                //controllo se devo visualizzare i giocatori in posizione attacco, difesa, entrambi o nascosti
                if ((l_intSelectedAreaIndex != -1))
                {

                    foreach (Mojhy.Engine.PlayingPlayer objPlayer in l_objTeamA.PlayingPlayers)
                    {
                        l_objSoccerGraph.DrawPlayer(objPlayer, l_intSelectedAreaIndex, SoccerGraphics.PlayerPositionType.Attack);
                    }
                                        
                }
                
            }

            Mojhy.Engine.PlayingPlayer objPlayingPlayer = l_objTeamA.PlayingPlayers[0];
            
            if (l_enState == FormStatus.PlayingMatch)
            {

                //controllo se devo visualizzare i giocatori in posizione attacco, difesa, entrambi o nascosti
                if ((l_intSelectedAreaIndex != -1))
                {

                    foreach (Mojhy.Engine.PlayingPlayer objPlayer in l_objTeamA.PlayingPlayers)
                    {
                        l_objSoccerGraph.DrawMovingPlayer(objPlayer);//, SoccerGraphics.PlayerPositionType.Attack);
                    }

                }

            }

            Point ptBallMM = l_objSoccerGraph.PixelToMM(l_ptBall);
            Point ptPlayer = l_objSoccerGraph.MMToPixel(objPlayingPlayer.CurrentPositionOnField);

            //scrivo la posizione del mouse (in MM e in Pixel)
            pe.Graphics.DrawString("Ball X     : " + ptBallMM.X.ToString() + "mm  Y: " + ptBallMM.Y.ToString() + "mm", new Font("Tahoma", 7), Brushes.White, new PointF(100, 100));            
            pe.Graphics.DrawString("Player X : " + objPlayingPlayer.CurrentPositionOnField.X.ToString() + "mm  Y: " + objPlayingPlayer.CurrentPositionOnField.Y.ToString() + "mm", new Font("Tahoma", 7), Brushes.White, new PointF(100, 2000));
            
            pe.Graphics.DrawString("Player X : " + ptPlayer.X.ToString() + "px  Y: " + ptPlayer.Y.ToString() + "px", new Font("Tahoma", 7), Brushes.White, new PointF(100, 4000));
            pe.Graphics.DrawString("Ball X     : " + l_ptBall.X.ToString() + "px  Y: " + l_ptBall.Y.ToString() + "px", new Font("Tahoma", 7), Brushes.White, new PointF(100, 6000));

            pe.Graphics.DrawString("Angle     : " + l_dblPlayerAngle[0] , new Font("Tahoma", 7), Brushes.White, new PointF(100, 8000));
            pe.Graphics.DrawString("Time      : " + l_dblTime.ToString(), new Font("Tahoma", 7), Brushes.White, new PointF(100, 10000));

        }
   

        //click per visualizzare o nascondere il giocatore in movimento
        private void btShowMovingPlayer_Click(object sender, EventArgs e)
        {
            
            l_blShowMovingPlayer = !(l_blShowMovingPlayer);            
            Invalidate();

        }

        //click per far partire la corsa del giocatore che si muoverà cercando di 
        //correre verso il pallone

        //DEBUG per ora corre solo il giocatore 0
        private void btnStartMove_Click(object sender, EventArgs e)
        {                        

            //indica se la palla è stata raggiunta
            l_blBallEated = false;
            
            //ora esatta di partenza del giocatore 
            l_dtStart = System.DateTime.Now;
             
            Timer1.Enabled = !(Timer1.Enabled);

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
            if (l_blBallEated == false)
            {
                //Aggiorna la posizione del giocatore portandolo verso il pallone
                MovePlayer();                
            }
            else
            {
                 //Pallone Raggiunto , Fermati !!!!
                 Timer1.Enabled = false;
            }
            
        }

        //controlla se il giocatore ha raggiunto il pallone
        private Boolean CheckBall(Point playerPos, Point ballPos)
        {
            if (playerPos.X < (ballPos.X + l_dblSpeed) && playerPos.X > (ballPos.X - l_dblSpeed)
                && playerPos.Y < (ballPos.Y + l_dblSpeed) && playerPos.Y > (ballPos.Y - l_dblSpeed))
                return true;
            else
                return false;
        }

        
        // la procedura effetua lo spostamento del giocare , in base alla velocità e alla direzione
        // calcolata tramite la formula che utilizza sin e cos ...
        // N.B.: i calcoli sono sempre sulle posizioni in mm
        private void MovePlayer()
        {
            int i = 0;

            Point ptBallMM = l_objSoccerGraph.PixelToMM(l_ptBall);

            foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
            {

                if (objPlayingPlayer.CurrentPlayArea.Index == l_intSelectedAreaIndex)
                {

                   if (CheckBall(objPlayingPlayer.CurrentPositionOnField, ptBallMM))
                    {
                        //l_blBallEated = true;

                        TimeSpan mySpan = DateTime.Now.Subtract(l_dtStart);
                        l_dblTime = mySpan.TotalSeconds;
                        Invalidate();

                    }
                    else
                    {

                        //imposta la posizione attuale del giocatore e calcola la nuova posizione
                        Point ptOldPosition = objPlayingPlayer.CurrentPositionOnField;

                        Point ptNewPosition = ptOldPosition;

                        double myCos = Math.Cos(l_dblPlayerAngle[i]);
                        double mySin = Math.Sin(l_dblPlayerAngle[i]);

                        ptNewPosition.X += (int)Math.Round((l_dblSpeed * myCos));
                        ptNewPosition.Y -= (int)Math.Round((l_dblSpeed * mySin));

                        //objPlayingPlayer.CurrentPositionOnField = ptNewPosition;
                        objPlayingPlayer.CurrentPositionOnField = GetMMValidFieldPosition(ptNewPosition);

                        ptNewPosition = l_objSoccerGraph.MMToPixel(ptNewPosition);
                        ptNewPosition = GetValidFieldPosition(ptNewPosition);

                        Invalidate(new Rectangle(ptOldPosition.X - 20, ptOldPosition.Y - 20, 40, 40));
                        Invalidate(new Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));

                    }

                    
                }
                else
                {
                    int intArea = 0;

                    Point ptLocBallMM = l_objSoccerGraph.PixelToMM(l_ptBall);

                    Mojhy.Engine.PlayArea objPlayAreaAux = l_objField.Areas.GetAreaFromLoc(ptLocBallMM);

                    l_intAreaDebug = l_objTeamA.PlayingPlayers[2].CurrentPlayArea.Index;

                    if (objPlayAreaAux != null)
                    {
                        intArea = objPlayAreaAux.Index;                        
                    }

                    Point ptPlayerPosition = objPlayingPlayer.PositionsOnField.AttackPositions[intArea];
                                        
                    if (CheckBall(objPlayingPlayer.CurrentPositionOnField, ptPlayerPosition))
                    {

                        //l_blBallEated = true;

                        TimeSpan mySpan = DateTime.Now.Subtract(l_dtStart);
                        l_dblTime = mySpan.TotalSeconds;
                        Invalidate();

                    }
                    else
                    {

                        //imposta la posizione attuale del giocatore e calcola la nuova posizione
                        Point ptOldPosition = objPlayingPlayer.CurrentPositionOnField;
                                                                       
                        Point ptNewPosition = ptOldPosition;

                        double myCos = Math.Cos(l_dblPlayerAngle[i]);
                        double mySin = Math.Sin(l_dblPlayerAngle[i]);

                        ptNewPosition.X += (int)Math.Round((l_dblSpeed * myCos));
                        ptNewPosition.Y -= (int)Math.Round((l_dblSpeed * mySin));

                        //objPlayingPlayer.CurrentPositionOnField = ptNewPosition;
                        objPlayingPlayer.CurrentPositionOnField = GetMMValidFieldPosition(ptNewPosition);

                        if (objPlayingPlayer.CurrentPlayArea.Index == l_intSelectedAreaIndex)
                        {
                            l_dblPlayerAngle[i] = Angle(objPlayingPlayer.CurrentPositionOnField.X, objPlayingPlayer.CurrentPositionOnField.Y, ptBallMM.X, ptBallMM.Y);
                        }

                        ptNewPosition = l_objSoccerGraph.MMToPixel(ptNewPosition);
                        ptNewPosition = GetValidFieldPosition(ptNewPosition);

                        Invalidate(new Rectangle(ptOldPosition.X - 20, ptOldPosition.Y - 20, 40, 40));
                        Invalidate(new Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));

                    }

                }

                i++;                              

            }

            
           Invalidate();
            
        }
        
        //Calcola l'angolo della direzione del giocatore verso il pallone
        //l'angolo che viene ritornato è in RADIANTI
        public double Angle(double px1, double py1, double px2, double py2)
        {
            // Negate X and Y value
            double pxRes = px2 - px1;
            double pyRes = py2 - py1;
            double angle = 0.0;
            // Calculate the angle
            if (pxRes == 0.0)
            {
                if (pxRes == 0.0)
                    angle = 0.0;
                else if (pyRes > 0.0)
                    angle = System.Math.PI / 2.0;
                else
                    angle = System.Math.PI * 3.0 / 2.0;
            }
            else if (pyRes == 0.0)
            {
                if (pxRes > 0.0)
                    angle = 0.0;
                else
                    angle = System.Math.PI;
            }
            else
            {
                if (pxRes < 0.0)
                    angle = System.Math.Atan(pyRes / pxRes) + System.Math.PI;
                else if (pyRes < 0.0)
                    angle = System.Math.Atan(pyRes / pxRes) + (2 * System.Math.PI);
                else
                    angle = System.Math.Atan(pyRes / pxRes);
            }
            // Convert to degrees
            angle = angle * 180 / System.Math.PI;
            //Return to RADIANT ;-) no nchiedetemi il perchè            
            angle = (((double)(360 - angle)) / 180) * Math.PI;
            return angle;
        }

        private void btLoadPositions_Click(object sender, EventArgs e)
        {

            OpenFileDialog objFileDialog = new OpenFileDialog();
            objFileDialog.Filter = "Mojhy formation file (*.mft)|";
            objFileDialog.Multiselect = false;
            DialogResult objDialogRes = objFileDialog.ShowDialog();
            if (objDialogRes == DialogResult.OK)
            {
                System.IO.StreamReader objTextFile = new System.IO.StreamReader(objFileDialog.FileName);
                try
                {
                    //oggetto temporaneo per il salvataggio delle posizioni per oguno degli 11 giocatori in campo
                    Mojhy.Engine.PlayingPositions[] objPlayingPositionsAux;
                    objPlayingPositionsAux = (Mojhy.Engine.PlayingPositions[])Mojhy.Utils.FrameworkUtils.DeserializeObject(objTextFile.ReadToEnd(), System.Type.GetType("Mojhy.Engine.PlayingPositions[]"));
                    
                    //assegno alla squadra A le posizioni lette
                    for (int i = 0; i < objPlayingPositionsAux.Length; i++)
                    {
                        l_objTeamA.PlayingPlayers[i].PositionsOnField = objPlayingPositionsAux[i];
                        l_objTeamA.PlayingPlayers[i].CurrentPositionOnField = objPlayingPositionsAux[i].AttackPositions[0];

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading formation: " + ex.Message);
                }
                finally
                {
                    objTextFile.Dispose();
                }
                Invalidate();
            }
            objFileDialog.Dispose();

        }

    }
}