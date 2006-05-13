/* PlayingPlayer.cs, FABIO MASINI
 * La classe definisce il giocatore giocante, quindi
 * con le sole proprietà utili e necessarie all'algoritmo 
 * di gioco. */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines the single playing player used by 'play algorithm'.
    /// </summary>
    public class PlayingPlayer
    {
        //posizione nel campo del giocatore
        private Point l_ptPosition;
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
        /// <summary>
        /// Gets or sets the player position on field.
        /// </summary>
        /// <value>The player position on field as Point.</value>
        public Point CurrentPositionOnField
        {
            get { return l_ptPosition; }
            set { l_ptPosition = value; }
        }
        /// <summary>
        /// Gets or sets the player's index.
        /// </summary>
        /// <value>The player's index.</value>
        public int Index
        {
            get { return l_intIndex; }
            set { l_intIndex = value; }
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
        /// Initializes a new instance of the <see cref="T:PlayingPlayer"/> class.
        /// </summary>
        /// <param name="objTeam">The referenced Team object.</param>
        /// <param name="objField">The referenced Field object.</param>
        public PlayingPlayer(Team objTeam, Field objField)
        {
            l_objPlayingPositions = new PlayingPositions();
            l_objField = objField;
            l_objTeam = objTeam;
        }
        /// <summary>
        /// Enables the artificial intelligence for the player.
        /// </summary>
        public void EnableAI(){
            //creo il nuovo thread di posizionamento del giocatore, che segue il pallone.
            if (l_objAIThread == null) {
                l_objAIThread = new Thread(this.TheBrain);
                l_objAIThread.Start();
            }
        }

        /// <summary>
        /// It's the player's brain.
        /// </summary>
        private void TheBrain(){
            //leggo la posizione del pallone
           //this.parent.parent.GetBall().PositionOnField
        }
    }
}
