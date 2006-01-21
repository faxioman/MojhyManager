/* PlayingPlayer.cs, FABIO MASINI
 * La classe definisce il giocatore giocante, quindi
 * con le sole proprietÓ utili e necessarie all'algoritmo 
 * di gioco. */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines the single playing player used by 'play algorithm'.
    /// </summary>
    class PlayingPlayer
    {
        //posizione nel campo del giocatore
        private Point l_ptPosition;
        //oggetto field ove i giocatori sono posizionati
        private Field l_objField;
        //indice del giocatore
        private int l_intIndex;
        /// <summary>
        /// It's the player position type (attack or defense)
        /// </summary>
        public enum PlayerPositionType
        {
            Attack, Defense
        }
        /// <summary>
        /// Gets or sets the player position on field.
        /// </summary>
        /// <value>The player position on field as Point.</value>
        public Point PositionOnField
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
        /// Gets the current play area for the player.
        /// </summary>
        /// <value>The current play area.</value>
        public PlayArea CurrentPlayArea
        {
            get { return l_objField.Areas.GetAreaFromLoc(l_ptPosition); }
        }
	
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PlayingPlayer"/> class.
        /// </summary>
        /// <param name="objField">The referenced field object.</param>
        public PlayingPlayer(Field objField)
        {
            l_objField = objField;
        }
    }
}
