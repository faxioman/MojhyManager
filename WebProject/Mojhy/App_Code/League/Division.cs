/* Division.cs, MARCO CECCARELLI
 * La classe definisce le divisioni (le serie) presenti all'interno di ogni lega */

namespace Mojhy.League
{
    /// <summary>
    /// Definisce le divisioni
    /// </summary>
    class Division
    {
        private string l_strName;
        private int l_intLeagueID;
        private int l_intDivisionID;
        
        /// <summary>
        /// Gets / Sets the Name of the Divisions.
        /// </summary>        
        public string Name
        {
            get { return l_strName; }
            set { l_strName = value; }
        }

        /// <summary>
        /// Gets / Sets the LeagueID.
        /// </summary>
        /// <value>The league ID.</value>
        public int LeagueID
        {
            get { return l_intLeagueID; }
            set { l_intLeagueID = value; }
        }

        /// <summary>
        /// Gets / Sets the DivisionID.
        /// </summary>
        /// <value>The Division ID.</value>
        public int DivisionID
        {
            get { return l_intDivisionID; }
            set { l_intDivisionID = value; }
        }


        /// <summary>
        /// Gets the teams.
        /// </summary>
        public void GetTeams()
        {

        }
    }
}
