/* Team.cs, FABIO MASINI
 * La classe definisce l'oggetto 'squadra'.
 * Incapsula anche i giocatori della squadra */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mojhy.Engine
{
    /// <summary>
    /// Defines the team.
    /// </summary>
    public class Team
    {
        //il nome della squadra
        private string l_strTeamName;
        //i giocatori in campo con le rispettive propriet�
        private PlayingPlayer[] l_objPlayingPlayers;
        //oggetto campo ove sono collocati i giocatori
        private Field l_objField;
        //schema corrente (4-4-2, 4-3-3, 5-4-1, ecc...)
        private string l_strCurrentFormation = "4-4-2";
        //stato di gioco (attacco, difesa, angolo...)
        private PlayingStatus l_enPlayingStatus;
        //flag che definisce se l'Intelligienza Artificiale � abilitata (okkio! Che non vogliano diventare giocatori veri...)
        private Boolean l_blAIenabled = false;
        //enumeratore che definisce gli stati possibili della squadra
        public enum PlayingStatus
        {
            attack,
            defense
        }
        /// <summary>
        /// Gets or sets the team's name.
        /// </summary>
        /// <value>The team's name as string.</value>
	    public string Name
	    {
            get { return l_strTeamName; }
            set { l_strTeamName = value; }
	    }
        /// <summary>
        /// Gets the playing players.
        /// </summary>
        /// <value>The playing players array.</value>
        public PlayingPlayer[] PlayingPlayers
        {
            get { return l_objPlayingPlayers; }
        }
        /// <summary>
        /// Gets or sets the current playing status (attack, defense, kick-off, ecc...).
        /// </summary>
        /// <value>The current playing status.</value>
        public PlayingStatus CurrentPlayingStatus
        {
            get { return l_enPlayingStatus; }
            set { l_enPlayingStatus = value; }
        }
        /// <summary>
        /// Enable or disable AI.
        /// </summary>
        /// <value><c>true</c> if AI enabled; otherwise, <c>false</c>.</value>
        public Boolean EnableAI
        {
            get { return l_blAIenabled; }
            set
            {
                if (value == true)
                {
                    //devo abilitare l'intelligienza artificiale
                    //verifico se i giocatori sono stati inseriti in campo
                    if (l_objPlayingPlayers != null)
                    {
                        //attivo l'algoritmo di posizionamento per ogni giocatore

                    }
                    else
                    {
                        l_blAIenabled = false;
                    }
                }
                else
                {
                    //devo disabilitare l'intelligienza artificiale
                }
            }
        }
        /// <summary>
        /// Puts eleven empty players on field.
        /// </summary>
        /// <param name="objField">The referenced field object.</param>
        public void PutOnField(Field objField)
        {
            //inizializzo l'array dei giocatori che scendono in campo
            l_objPlayingPlayers = new PlayingPlayer[11];
            //creo i giocatori sul campo
            for (int i = 0; i < l_objPlayingPlayers.Length; i++)
            {
                l_objPlayingPlayers[i] = new PlayingPlayer(objField);
                l_objPlayingPlayers[i].Index = i;
            }
            //imposto il riferimento all'oggetto campo  
            l_objField = objField;
        }
    }
}
