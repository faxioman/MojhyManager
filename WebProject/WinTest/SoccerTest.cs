using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Mojhy.Utils.DrawingExt;

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
        private PointObject l_ptMouseLoc = new PointObject();
        //area corrente selezionata
        private int l_intSelectedAreaIndex = -1;
        //se un area � selezionata -> l_blAreaSelected = true
        private bool l_blAreaSelected = false;
        //le variabili definiscono se, in fase di selezione dei giocatori,
        //devo visualizzare le posizioni in attacco o in difesa o entrambe
        private bool l_blShowAttack = true;
        private bool l_blShowDefense = true;
        //definisco se sono in fase di drag
        private bool l_blDragging = false;
        //timer per il ridisegno
        private Timer l_objTimer = new System.Windows.Forms.Timer();
        //posizioni in memoria per raffinare il ridisegno
        private System.Drawing.Point[] l_arrPosPoints;
        private System.Drawing.Point l_ptBallPos;
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
            //Attivo DoubleBuffer, cos� rimuovo il flicker
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            //creo l'oggetto campo
            l_objField = new Mojhy.Engine.Field();
            //collego l'evento di spostamento del mouse
            this.MouseMove += new MouseEventHandler(SoccerTest_MouseMove);
            //collego l'evento di mouse down
            this.MouseDown += new MouseEventHandler(SoccerTest_MouseDown);
            //collego l'evento di mouse up
            this.MouseUp += new MouseEventHandler(SoccerTest_MouseUp);
            //collego l'evento di chiusura della form
            this.FormClosing += new FormClosingEventHandler(SoccerTest_FormClosing);
            //inizializzo la prima squadra (presente in ogni situazione)
            l_objTeamA = new Mojhy.Engine.Team();
            //inizializzo le posizione dei giocatori (posizione iniziale, tutti a meta campo)
            l_objField.SetTeamA(l_objTeamA);
            foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
            {
                for (int i = 0; i < objPlayingPlayer.PositionsOnField.DefensePositions.Length; i++)
                {
                    objPlayingPlayer.PositionsOnField.DefensePositions[i].X = (l_objField.Width / 2) - 10000;
                    objPlayingPlayer.PositionsOnField.DefensePositions[i].Y = l_objField.Height / 2;
                }
                for (int i = 0; i < objPlayingPlayer.PositionsOnField.AttackPositions.Length; i++)
                {
                    objPlayingPlayer.PositionsOnField.AttackPositions[i].X = (l_objField.Width / 2) + 10000;
                    objPlayingPlayer.PositionsOnField.AttackPositions[i].Y = l_objField.Height / 2;
                }

            }
            //inserisco il pallone nel campo
            l_objField.SetBall(new Mojhy.Engine.Ball());
            //inizializzo la combo dello stato allo stato attuale
            cbStatus.SelectedIndex = (int)this.l_enState;
            //inizializzo la combo dello stato della squadra ad 'attacco'
            cbTeamStatus.SelectedIndex = 0;
            //inizializzo il timer per il ridisegno
            l_objTimer.Interval = 40;
            l_objTimer.Tick += new System.EventHandler(this.RedrawTimer);
        }
        //distruzione della form
        private void SoccerTest_FormClosing(object sender,  FormClosingEventArgs e)
        {
            l_objTeamA.DisableAI();
        }
        //restituisce un punto valido del campo in pixel
        private PointObject GetValidFieldPosition(PointObject ptRawPoint)
        {
            PointObject ptValidPosition = new PointObject(ptRawPoint.X, ptRawPoint.Y);
            if (ptValidPosition.X < MINX) ptValidPosition.X = MINX;
            if (ptValidPosition.X > MAXX) ptValidPosition.X = MAXX;
            if (ptValidPosition.Y < MINY) ptValidPosition.Y = MINY;
            if (ptValidPosition.Y > MAXY) ptValidPosition.Y = MAXY;
            return ptValidPosition;
        }
        //eventu sul rilascio del mouse
        void SoccerTest_MouseUp(object sender, MouseEventArgs e)
        {
            PointObject ptLocBallMM;
            switch (l_enState)
            {
                case FormStatus.SettingPlayerPosition:
                    l_blDragging = false;
                    //se non � selezionato nessun giocatore, allore sposto la posizione attuale del pallone
                    //e calcolo la relativa area
                    if ((l_intSelectedDefensePlayerIndex == -1) && (l_intSelectedAttackPlayerIndex == -1))
                    {
                        l_blAreaSelected = true;
                        ptLocBallMM = l_objSoccerGraph.PixelToMM(new PointObject(l_ptMouseLoc.X, l_ptMouseLoc.Y));
                        l_objField.GetBall().PositionOnField.X = ptLocBallMM.X;
                        l_objField.GetBall().PositionOnField.Y = ptLocBallMM.Y;
                        Mojhy.Engine.PlayArea objPlayAreaAux = l_objField.Areas.GetAreaFromLoc(l_objField.GetBall().PositionOnField);
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
                    //deseleziono qualsiasi eventuale giocatore selezionato
                    l_intSelectedDefensePlayerIndex = -1;
                    l_intSelectedAttackPlayerIndex = -1;
                    break;
                case FormStatus.MoveBallAndEnjoy:
                    l_blDragging = false;
                    Cursor.Show();
                    break;
                case FormStatus.PlayingMatch:
                    break;
            }
        }

        void SoccerTest_MouseDown(object sender, MouseEventArgs e)
        {
            PointObject ptLocBallMM;
            switch (l_enState)
            {
                case FormStatus.SettingPlayerPosition:
                    l_blDragging = true;
                    if (l_intSelectedAreaIndex != -1)
                    {
                        //verifico se sono sopra un giocatore (in difesa) e se quindi lo devo
                        //selezionare per lo spostamento
                        //calcolo il rettangolo che definisce il range di selezione del giocatore
                        PointObject ptMousePositionInField = l_objSoccerGraph.PixelToMM(l_ptMouseLoc);
                        RectangleObject rctSelectionArea = new RectangleObject(ptMousePositionInField.X - 1500, ptMousePositionInField.Y - 1500, 3000, 3000);
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
                    break;
                case FormStatus.MoveBallAndEnjoy:
                    l_blDragging = true;
                    Cursor.Hide();
                    ptLocBallMM = GetValidFieldPosition(new PointObject(l_ptMouseLoc.X, l_ptMouseLoc.Y));
                    ptLocBallMM = l_objSoccerGraph.PixelToMM(ptLocBallMM);
                    l_objField.GetBall().PositionOnField.X = ptLocBallMM.X;
                    l_objField.GetBall().PositionOnField.Y = ptLocBallMM.Y;
                    break;
                case FormStatus.PlayingMatch:
                    break;
            }
        }
        //mantiene aggiornata la posizione del mouse
        void SoccerTest_MouseMove(object sender, MouseEventArgs e)
        {
            PointObject ptLocBallMM;
            l_ptMouseLoc.X = e.X;
            l_ptMouseLoc.Y = e.Y;
            switch (l_enState)
            {
                case FormStatus.SettingPlayerPosition:
                    //verifico se devo spostare un giocatore
                    if ((l_blDragging) && ((l_intSelectedDefensePlayerIndex != -1) || (l_intSelectedAttackPlayerIndex != -1)))
                    {
                        if (l_intSelectedDefensePlayerIndex != -1)
                        {
                            PointObject ptOldPosition = l_objSoccerGraph.MMToPixel(l_objTeamA.PlayingPlayers[l_intSelectedDefensePlayerIndex].PositionsOnField.DefensePositions[l_intSelectedAreaIndex]);
                            PointObject ptNewPosition = l_ptMouseLoc;
                            ptNewPosition = GetValidFieldPosition(ptNewPosition);
                            l_objTeamA.PlayingPlayers[l_intSelectedDefensePlayerIndex].PositionsOnField.DefensePositions[l_intSelectedAreaIndex] = l_objSoccerGraph.PixelToMM(ptNewPosition);
                            Invalidate(new System.Drawing.Rectangle(ptOldPosition.X - 20, ptOldPosition.Y - 20, 40, 40));
                            Invalidate(new System.Drawing.Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));
                        }
                        else
                        {
                            PointObject ptOldPosition = l_objSoccerGraph.MMToPixel(l_objTeamA.PlayingPlayers[l_intSelectedAttackPlayerIndex].PositionsOnField.AttackPositions[l_intSelectedAreaIndex]);
                            PointObject ptNewPosition = l_ptMouseLoc;
                            ptNewPosition = GetValidFieldPosition(ptNewPosition);
                            l_objTeamA.PlayingPlayers[l_intSelectedAttackPlayerIndex].PositionsOnField.AttackPositions[l_intSelectedAreaIndex] = l_objSoccerGraph.PixelToMM(ptNewPosition);
                            Invalidate(new System.Drawing.Rectangle(ptOldPosition.X - 20, ptOldPosition.Y - 20, 40, 40));
                            Invalidate(new System.Drawing.Rectangle(ptNewPosition.X - 20, ptNewPosition.Y - 20, 40, 40));
                        }
                    }
                    break;
                case FormStatus.MoveBallAndEnjoy:
                    if (l_blDragging)
                    {
                        ptLocBallMM = GetValidFieldPosition(new PointObject(l_ptMouseLoc.X, l_ptMouseLoc.Y));
                        ptLocBallMM = l_objSoccerGraph.PixelToMM(ptLocBallMM);
                        l_objField.GetBall().PositionOnField.X = ptLocBallMM.X;
                        l_objField.GetBall().PositionOnField.Y = ptLocBallMM.Y;
                    }
                    break;
                case FormStatus.PlayingMatch:
                    break;
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
            PointObject ptMouseLocMM = l_objSoccerGraph.PixelToMM(l_ptMouseLoc);
            PointObject ptMouseLocTmp = l_ptMouseLoc;
            if ((ptMouseLocMM.X > l_objField.Width) || (ptMouseLocMM.Y > l_objField.Height) || (ptMouseLocMM.X < 0) || (ptMouseLocMM.Y < 0))
            {
                ptMouseLocMM.X = 0;
                ptMouseLocMM.Y = 0;
                ptMouseLocTmp.X = 0;
                ptMouseLocTmp.Y = 0;
            }
            //attivo l'area cliccata
            if ((l_blAreaSelected) && (l_intSelectedAreaIndex != -1)) l_objSoccerGraph.DrawSelectedArea(l_intSelectedAreaIndex, l_objField);
            //controllo se devo visualizzare i giocatori in posizione attacco, difesa, entrambi o nascosti
            if ((l_intSelectedAreaIndex != -1) && (l_blShowDefense || l_blShowAttack))
            {
                foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
                {
                    if (l_blShowDefense) l_objSoccerGraph.DrawPlayer(objPlayingPlayer, l_intSelectedAreaIndex, SoccerGraphics.PlayerPositionType.Defense);
                    if (l_blShowAttack) l_objSoccerGraph.DrawPlayer(objPlayingPlayer, l_intSelectedAreaIndex, SoccerGraphics.PlayerPositionType.Attack);
                }
            }
            if (l_enState == FormStatus.MoveBallAndEnjoy)
            {
                foreach (Mojhy.Engine.PlayingPlayer objPlayingPlayer in l_objTeamA.PlayingPlayers)
                {
                    l_objSoccerGraph.DrawPlayer(objPlayingPlayer, -1, SoccerGraphics.PlayerPositionType.Current);
                }
            }
            if (l_enState != FormStatus.SettingPlayerPosition) l_objSoccerGraph.DrawBall(l_objField.GetBall());
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
                    objTextFile.Write(Mojhy.Utils.FrameworkUtils.SerializeObject(objPlayingPositionsAux));
                }
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
                    Mojhy.Engine.PlayingPositions[] objPlayingPositionsAux = new Mojhy.Engine.PlayingPositions[0];
                    objPlayingPositionsAux = (Mojhy.Engine.PlayingPositions[])Mojhy.Utils.FrameworkUtils.DeserializeObject(objTextFile.ReadToEnd(), objPlayingPositionsAux.GetType());
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
            //se lo stato non � di modifica dei giocatori allora nascondo
            //i button inutili
            if (l_enState != FormStatus.SettingPlayerPosition)
            {
                btLoadPositions.Hide();
                btSavePositions.Hide();
                btShowAttack.Hide();
                btShowDefense.Hide();
                cbTeamStatus.Show();
                l_objTimer.Start();
                if (l_enState == FormStatus.MoveBallAndEnjoy)
                {
                    l_objTeamA.CurrentPlayingStatus = Mojhy.Engine.Team.PlayingStatus.attack;
                    l_objTeamA.EnableAI();
                    //l_objField.GetBall().EnableAI();
                }
            }
            else
            {
                l_objTimer.Stop();
                cbTeamStatus.Hide();
                btLoadPositions.Show();
                btSavePositions.Show();
                btShowAttack.Show();
                btShowDefense.Show();
                l_objTeamA.DisableAI();
                //l_objField.GetBall().DisableAI();
            }
            Invalidate();
        }
        //il metodo viene chiamato dal timer che si occupa del ridisegno
        private void RedrawTimer(object sender, EventArgs e)
        {
            if (l_arrPosPoints == null)
            {
                l_arrPosPoints = new System.Drawing.Point[11];
            }
            for (int i = 0; i < 11; i++)
            {
                PointObject objCurrentPosAux = l_objSoccerGraph.MMToPixel(l_objTeamA.PlayingPlayers[i].CurrentPositionOnField);
                Invalidate(new System.Drawing.Rectangle(l_arrPosPoints[i].X - 20, l_arrPosPoints[i].Y - 20, 40, 40));
                Invalidate(new System.Drawing.Rectangle(objCurrentPosAux.X - 20, objCurrentPosAux.Y - 20, 40, 40));
                l_arrPosPoints[i].X = objCurrentPosAux.X;
                l_arrPosPoints[i].Y = objCurrentPosAux.Y;
            }
            PointObject objCurrentBallPosAux = l_objSoccerGraph.MMToPixel(l_objField.GetBall().PositionOnField);
            Invalidate(new System.Drawing.Rectangle(l_ptBallPos.X - 20, l_ptBallPos.Y - 20, 40, 40));
            Invalidate(new System.Drawing.Rectangle(objCurrentBallPosAux.X - 20, objCurrentBallPosAux.Y - 20, 40, 40));
            l_ptBallPos.X = objCurrentBallPosAux.X;
            l_ptBallPos.Y = objCurrentBallPosAux.Y;
        }
        //combo dello stato attuale della squadra
        private void cbTeamStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((ComboBox)sender).SelectedItem.ToString())
            {
                case "Attack":
                    l_objTeamA.CurrentPlayingStatus = Mojhy.Engine.Team.PlayingStatus.attack;
                    break;
                case "Defense":
                    l_objTeamA.CurrentPlayingStatus = Mojhy.Engine.Team.PlayingStatus.defense;
                break;
            }
        }
    }
}