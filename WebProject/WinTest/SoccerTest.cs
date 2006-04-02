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
        #region Variabili Locali
        private const int MINX = 8;
        private const int MAXX = 786;
        private const int MINY = 8;
        private const int MAXY = 520;
        private Mojhy.Engine.Field l_objField;
        private SoccerGraphics l_objSoccerGraph;
        //squadre
        private Mojhy.Engine.Team l_objTeamA;
        private Mojhy.Engine.Team l_objTeamB;
        //indice del giocatore selezionato col mouse
        private int l_intSelectedDefensePlayerIndex = -1;
        private int l_intSelectedAttackPlayerIndex = -1;
        //indica se disegnare o meno le aree sensibili
        private bool blShowAreas = false;
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
        //le variabili definiscono se, in fase di selezione dei giocatori,
        //devo visualizzare le posizioni in attacco o in difesa o entrambe
        private bool l_blShowAttack = true;
        private bool l_blShowDefense = true;
        //definisco se sono in fase di drag
        private bool l_blDragging = false;
        #endregion
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
            //inizializzo le posizione dei giocatori (posizione iniziale, tutti a meta campo)
            l_objField.SetTeamA(l_objTeamA);
            foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
            {
                for (int i = 0; i < objPlayingPlayer.PositionsOnField.DefensePositions.Length; i++)
                {
                    objPlayingPlayer.PositionsOnField.DefensePositions[i] = new Point((l_objField.Width / 2)-10000, l_objField.Height / 2);
                }
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
        //eventu sul rilascio del mouse
        void SoccerTest_MouseUp(object sender, MouseEventArgs e)
        {
            l_blDragging = false;
            if (l_enState == FormStatus.SettingPlayerPosition)
            {
                //se non è selezionato nessun giocatore, allore sposto la posizione attuale del pallone
                //e calcolo la relativa area
                if ((l_intSelectedDefensePlayerIndex == -1)&&(l_intSelectedAttackPlayerIndex == -1))
                {
                    l_ptBall = l_ptMouseLoc;
                    l_blAreaSelected = true;
                    Point ptLocBallMM = l_objSoccerGraph.PixelToMM(l_ptBall);
                    Mojhy.Engine.PlayArea objPlayAreaAux = l_objField.Areas.GetAreaFromLoc(ptLocBallMM);
                    if (objPlayAreaAux != null)
                    {
                        l_intSelectedAreaIndex = objPlayAreaAux.Index;
                        Invalidate();
                    }
                }
                else
                {
                    Cursor.Show();
                }
            }
            //deseleziono qualsiasi eventuale giocatore selezionato
            l_intSelectedDefensePlayerIndex = -1;
            l_intSelectedAttackPlayerIndex = -1;
        }

        void SoccerTest_MouseDown(object sender, MouseEventArgs e)
        {
            l_blDragging = true;
            if (l_enState == FormStatus.SettingPlayerPosition)
            {
                if (l_intSelectedAreaIndex != -1)
                {
                    //verifico se sono sopra un giocatore (in difesa) e se quindi lo devo
                    //selezionare per lo spostamento
                    //calcolo il rettangolo che definisce il range di selezione del giocatore
                    Point ptMousePositionInField = l_objSoccerGraph.PixelToMM(l_ptMouseLoc);
                    Rectangle rctSelectionArea = new Rectangle(ptMousePositionInField.X - 1500, ptMousePositionInField.Y - 1500, 3000, 3000);
                    //controllo se i difensori sono visibili
                    if (l_blShowDefense)
                    {
                        for (int i = l_objTeamA.PlayingPlayers.Length - 1; i >= 0; i--)
                        {
                            if (rctSelectionArea.Contains(l_objTeamA.PlayingPlayers[i].PositionsOnField.DefensePositions[l_intSelectedAreaIndex]))
                            {
                                l_intSelectedDefensePlayerIndex = l_objTeamA.PlayingPlayers[i].Index;
                                Cursor.Hide();
                                break;
                            }
                        }
                    }
                    //verifico se sono sopra un giocatore (in attacco) e se quindi lo devo
                    //selezionare per lo spostamento
                    if ((l_intSelectedDefensePlayerIndex == -1) && (l_blShowAttack))
                    {
                        for (int i = l_objTeamA.PlayingPlayers.Length - 1; i >= 0; i--)
                        {
                            if (rctSelectionArea.Contains(l_objTeamA.PlayingPlayers[i].PositionsOnField.AttackPositions[l_intSelectedAreaIndex]))
                            {
                                l_intSelectedAttackPlayerIndex = l_objTeamA.PlayingPlayers[i].Index;
                                Cursor.Hide();
                                break;
                            }
                        }
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
                if ((l_blDragging) && ((l_intSelectedDefensePlayerIndex != -1)||(l_intSelectedAttackPlayerIndex != -1)))
                {
                    if (l_intSelectedDefensePlayerIndex != -1){
                        Point ptOldPosition = l_objSoccerGraph.MMToPixel(l_objTeamA.PlayingPlayers[l_intSelectedDefensePlayerIndex].PositionsOnField.DefensePositions[l_intSelectedAreaIndex]);
                        Point ptNewPosition = l_ptMouseLoc;
                        ptNewPosition = GetValidFieldPosition(ptNewPosition);
                        l_objTeamA.PlayingPlayers[l_intSelectedDefensePlayerIndex].PositionsOnField.DefensePositions[l_intSelectedAreaIndex] = l_objSoccerGraph.PixelToMM(ptNewPosition);
                        Invalidate(new Rectangle(ptOldPosition.X - 20, ptOldPosition.Y - 20, 40, 40));
                        Invalidate(new Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));
                    }else{
                        Point ptOldPosition = l_objSoccerGraph.MMToPixel(l_objTeamA.PlayingPlayers[l_intSelectedAttackPlayerIndex].PositionsOnField.AttackPositions[l_intSelectedAreaIndex]);
                        Point ptNewPosition = l_ptMouseLoc;
                        ptNewPosition = GetValidFieldPosition(ptNewPosition);
                        l_objTeamA.PlayingPlayers[l_intSelectedAttackPlayerIndex].PositionsOnField.AttackPositions[l_intSelectedAreaIndex] = l_objSoccerGraph.PixelToMM(ptNewPosition);
                        Invalidate(new Rectangle(ptOldPosition.X - 20, ptOldPosition.Y - 20, 40, 40));
                        Invalidate(new Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));
                    }
                }
            }
            //invalido l'area dove scrivere la posizione del mouse
            //Invalidate(new Rectangle(0,0,120,25));

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
            Point ptMouseLocTmp = l_ptMouseLoc;
            if ((ptMouseLocMM.X > l_objField.Width) || (ptMouseLocMM.Y > l_objField.Height) || (ptMouseLocMM.X < 0) || (ptMouseLocMM.Y < 0))
            {
                ptMouseLocMM.X = 0;
                ptMouseLocMM.Y = 0;
                ptMouseLocTmp.X = 0;
                ptMouseLocTmp.Y = 0;
            }
            //attivo l'area cliccata
            if ((l_blAreaSelected)&&(l_intSelectedAreaIndex!=-1)) l_objSoccerGraph.DrawSelectedArea(l_intSelectedAreaIndex, l_objField);
            //controllo se devo visualizzare i giocatori in posizione attacco, difesa, entrambi o nascosti
            if ((l_intSelectedAreaIndex != -1) && (l_blShowDefense || l_blShowAttack))
            {
                foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
                {
                    if (l_blShowDefense) l_objSoccerGraph.DrawPlayer(objPlayingPlayer, l_intSelectedAreaIndex, SoccerGraphics.PlayerPositionType.Defense);
                    if (l_blShowAttack) l_objSoccerGraph.DrawPlayer(objPlayingPlayer, l_intSelectedAreaIndex, SoccerGraphics.PlayerPositionType.Attack);
                }
            }
            //scrivo la posizione del mouse (in MM e in Pixel)
           // pe.Graphics.DrawString("X: " + ptMouseLocMM.X.ToString() + "mm  Y: " + ptMouseLocMM.Y.ToString() + "mm", new Font("Tahoma", 7), Brushes.White, new PointF(100, 100));
           // pe.Graphics.DrawString("X: " + ptMouseLocTmp.X.ToString() + "px  Y: " + ptMouseLocTmp.Y.ToString() + "px", new Font("Tahoma", 7), Brushes.White, new PointF(100, 1600));
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
            l_blShowDefense = !l_blShowDefense;
            Invalidate();
        }
        //click per visualizzare o nascondere le posizioni in attacco
        private void btShowAttack_Click(object sender, EventArgs e)
        {
            l_blShowAttack = !l_blShowAttack;
            Invalidate();
        }
        //click per il salvataggio delle posizioni correnti
        private void btSavePositions_Click(object sender, EventArgs e)
        {
            SaveFileDialog objSaveDialog = new SaveFileDialog();
            objSaveDialog.Filter = "Mojhy formation file (*.mft)|";
            DialogResult objDialogRes = objSaveDialog.ShowDialog();
            if (objDialogRes == DialogResult.OK)
            {
                String strFilename = objSaveDialog.FileName;
                if (strFilename.IndexOf(".") == -1) strFilename = String.Concat(strFilename, ".mft");
                System.IO.StreamWriter objTextFile = new System.IO.StreamWriter(strFilename, false);
                try
                {
                    //oggetto temporaneo per il salvataggio delle posizioni per oguno degli 11 giocatori in campo
                    Mojhy.Engine.PlayingPositions[] objPlayingPositionsAux = new Mojhy.Engine.PlayingPositions[l_objTeamA.PlayingPlayers.Length];
                    for (int i = 0; i < l_objTeamA.PlayingPlayers.Length; i++)
                    {
                        objPlayingPositionsAux[i] = l_objTeamA.PlayingPlayers[i].PositionsOnField;
                    }
                    objTextFile.Write(Mojhy.Utils.FrameworkUtils.SerializeObject(objPlayingPositionsAux));                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving formation: " + ex.Message);
                }
                finally
                {
                    objTextFile.Dispose();
                }
            }
            objSaveDialog.Dispose();
        }
        //click per il caricamento delle posizioni correnti
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
        //evento di modifica della compbo dello stato
        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbStatusAux = (ComboBox)sender;
            l_enState = (FormStatus)cbStatusAux.SelectedIndex;
            l_blAreaSelected = false;
            l_intSelectedAreaIndex = -1;
            //se lo stato non è di modifica dei giocatori allora nascondo
            //i button inutili
            if (l_enState != FormStatus.SettingPlayerPosition)
            {
                btLoadPositions.Hide();
                btSavePositions.Hide();
                btShowAttack.Hide();
                btShowDefense.Hide();
            }
            else
            {
                btLoadPositions.Show();
                btSavePositions.Show();
                btShowAttack.Show();
                btShowDefense.Show();
            }
            Invalidate();
        }
    }
}