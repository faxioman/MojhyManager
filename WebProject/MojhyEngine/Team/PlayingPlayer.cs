/* PlayingPlayer.cs, FABIO MASINI
 * La classe definisce il giocatore giocante, quindi
 * con le sole proprietà utili e necessarie all'algoritmo 
 * di gioco. */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Mojhy.Utils.DrawingExt;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines the single playing player used by 'play algorithm'.
    /// </summary>
    public class PlayingPlayer
    {
        //posizione nel campo del giocatore
        private PointObject l_ptPosition = new PointObject();
        //oggetto field ove i giocatori sono posizionati
        private Field l_objField;
        //indice del giocatore
        private int l_intIndex;
        //oggetto Team padre
        private Team l_objTeam;
        //posizioni in campo
        private PlayingPositions l_objPlayingPositions;
        //thread AI
        private Thread l_objAIThread;
        //variabili locali indicanti le skill del giocatore
        private Single l_sglVelocity = 30;
        /// <summary>
        /// Gets or sets the player skill 'velocity'.
        /// </summary>
        /// <value>The skill velocity as single (from 1 to 100).</value>
        public Single SkillVelocity
        {
            get { return l_sglVelocity; }
            set { l_sglVelocity =  value; }
        }
        /// <summary>
        /// Gets or sets the player position on field.
        /// </summary>
        /// <value>The player position on field as Point.</value>
        public PointObject CurrentPositionOnField
        {
            get { return l_ptPosition; }
        }
        /// <summary>
        /// Gets the player's index.
        /// </summary>
        /// <value>The player's index.</value>
        public int Index
        {
            get { return l_intIndex; }
        }
        /// <summary>
        /// Gets the parent Team object.
        /// </summary>
        /// <value>The parent Team object.</value>
        public Team parent
        {
            get { return l_objTeam; }
        }
        /// <summary>
        /// Gets the current play area for the player.
        /// </summary>
        /// <value>The current play area.</value>
        public PlayArea CurrentPlayArea
        {
            get { return l_objField.Areas.GetAreaFromLoc(l_ptPosition); }
        }
        /// <summary>
        /// Gets or sets the positions on field for the current playing player.
        /// </summary>
        /// <value>The positions on field.</value>
        public PlayingPositions PositionsOnField
        {
            get { return l_objPlayingPositions; }
            set { l_objPlayingPositions = value; }
        }
        /// <summary>
        /// Gets the distance from the ball.
        /// </summary>
        /// <value>The distance from the ball in mm.</value>
        public int GetDistanceFromBall()
        {
            Point3D ptBallPos = this.parent.parent.GetBall().PositionOnField;
            //pitagora come se piovesse ;-)
            return (int)Math.Round(Math.Sqrt(Math.Pow(ptBallPos.X - this.CurrentPositionOnField.X, 2) + Math.Pow(ptBallPos.Y - this.CurrentPositionOnField.Y, 2)));
        }
        /// <summary>
        /// Determines whether the player is the nearest to the ball.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the player is the nearest to the ball; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNearestToBall()
        {
            int intMyDistance = this.GetDistanceFromBall();
            foreach (PlayingPlayer objPlayingPlayer in this.parent.PlayingPlayers)
            {
                if (intMyDistance > objPlayingPlayer.GetDistanceFromBall())
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PlayingPlayer"/> class.
        /// </summary>
        /// <param name="objTeam">The referenced Team object.</param>
        /// <param name="Index">The index of the player (from 0 to 10).</param>
        /// <param name="objField">The referenced Field object.</param>
        public PlayingPlayer(Team objTeam, int Index, Field objField)
        {
            if ((Index < 0) || (Index > 10)) 
                throw new Exception("PlayingPlayer Index range is from 0 to 10");
            l_objPlayingPositions = new PlayingPositions();
            l_objField = objField;
            l_objTeam = objTeam;
            l_intIndex = Index;
        }
        /// <summary>
        /// Enables the artificial intelligence for the player.
        /// </summary>
        public void EnableAI()
        {
            //creo il nuovo thread di posizionamento del giocatore, che segue il pallone.
            if (l_objAIThread == null)
            {
                l_objAIThread = new Thread(this.TheBrain);
                l_objAIThread.Start();
            }
        }
        /// <summary>
        /// Disables the artificial intelligence for the player.
        /// </summary>
        public void DisableAI()
        {
            if (l_objAIThread != null)
            {
                l_objAIThread.Abort();
                l_objAIThread = null;
            }
        }
        /// <summary>
        /// It's the player's brain.
        /// </summary>
        private void TheBrain()
        {
            double dblCos = 0;
            double dblSin = 0;
            int intMoveX = 0;
            int intMoveY = 0;
            //punto di arrivo del giocatore
            PointObject ptGoodPosition = new PointObject();
            //angolo di spostmento del giocatore
            double dblMoveAngle;
            //indice dell'area temporaneo
            int intAreaIndexTmp;
            //mantengo tre variabili 'Previous' relative allo stato della squadra, all'area corrente e per vedere se cambia lo stato di giocatore più vicino alla palla.
            //In questo modo ricalcolo la posizione del giocatore solo se queste sono cambiate.
            Mojhy.Engine.Team.PlayingStatus objStatusPrevious = Team.PlayingStatus.kickoff;
            int intAreaIndexPrevious = -1;
            bool blIsNearestPrevious = false;
            //variabile se si tratta del giocatore più vicino alla palla
            bool blIsNearestAux;
            //parte il ciclo guidato dal Thread
            while (System.Threading.Thread.CurrentThread.ThreadState == ThreadState.Running)
            {
                intAreaIndexTmp = this.parent.parent.GetCurrentArea().Index;
                blIsNearestAux = this.IsNearestToBall();
                if (blIsNearestAux || (blIsNearestPrevious != blIsNearestAux) || (intAreaIndexPrevious != intAreaIndexTmp) || (objStatusPrevious != this.parent.CurrentPlayingStatus))
                {
                    if (!blIsNearestAux)
                        //deve correre verso la sua posizione
                    {
                        //leggo la posizione del giocatore a seconda di dove si trova il pallone e lo stato della squadra
                        switch (this.parent.CurrentPlayingStatus)
                        {
                            case Team.PlayingStatus.attack:
                                ptGoodPosition = this.PositionsOnField.AttackPositions[intAreaIndexTmp];
                                break;
                            case Team.PlayingStatus.defense:
                                ptGoodPosition = this.PositionsOnField.DefensePositions[intAreaIndexTmp];
                                break;
                            default:
                                //se non dovesse avere alcuno stato (non dovrebbe succedere) considero la posizione attuale del giocatore
                                //come valida
                                ptGoodPosition = this.CurrentPositionOnField;
                                break;
                        }
                    }
                    else
                        //deve correre verso la palla
                    {
                        Point3D ptBallPosition = this.parent.parent.GetBall().PositionOnField;
                        ptGoodPosition = new PointObject(ptBallPosition.X, ptBallPosition.Y);
                    }
                    //calcolo l'angolo di spostamento del giocatore
                    dblMoveAngle = Angle(this.CurrentPositionOnField.X, this.CurrentPositionOnField.Y, ptGoodPosition.X, ptGoodPosition.Y);
                    dblCos = Math.Cos(dblMoveAngle);
                    dblSin = Math.Sin(dblMoveAngle);
                    intAreaIndexPrevious = intAreaIndexTmp;
                    objStatusPrevious = this.parent.CurrentPlayingStatus;
                    blIsNearestPrevious = blIsNearestAux;
                    intMoveX = (int)Math.Round(l_sglVelocity * dblCos);
                    intMoveY= (int)Math.Round(l_sglVelocity * dblSin);
                }
                System.Threading.Thread.Sleep(4); //TODO. SOLO PER TESTING...POI NON SARà IL CASO DI RALLENTARE NULLA!!!!
                //muovo il giocatore nella sua posizione (verifico che non sia già arrivato 'nei pressi')
                if ((Math.Abs(this.CurrentPositionOnField.X - ptGoodPosition.X) > 500) || (Math.Abs(this.CurrentPositionOnField.Y - ptGoodPosition.Y) > 500))
                {
                    this.CurrentPositionOnField.X += intMoveX;
                    this.CurrentPositionOnField.Y -= intMoveY;
                }
            }
        }
        /// <summary>
        /// Calculate the direction angle of the player towards the destination (in RAD).
        /// </summary>
        /// <param name="px1">The PX1.</param>
        /// <param name="py1">The py1.</param>
        /// <param name="px2">The PX2.</param>
        /// <param name="py2">The py2.</param>
        /// <returns></returns>
        private double Angle(double px1, double py1, double px2, double py2)
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
            //Return to RADIANT ;-) non chiedetemi il perchè 
            angle = (((double)(360 - angle)) / 180) * Math.PI;
            return angle;
        }
    }
}
