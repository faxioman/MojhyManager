/* PlayingPlayer.cs, FABIO MASINI
 * La classe definisce il giocatore giocante, quindi
 * con le sole proprietà utili e necessarie all'algoritmo 
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
        /// <summary>
        /// Gets or sets the player position on field.
        /// </summary>
        /// <value>The player position on field as Point.</value>
        public Point PositionOnField
        {
            get { return l_ptPosition; }
            set { l_ptPosition = value; }
        }
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
