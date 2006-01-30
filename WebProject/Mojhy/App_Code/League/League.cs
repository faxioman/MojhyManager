/* League.cs, MARCO CECCARELLI
 * La classe definisce le nazioni che conterranno i campionati */

namespace Mojhy.Leagues
{
    /// <summary>
    /// Definisce le nazioni
    /// </summary>
    public class League
    {
        private string l_strName;
        private int l_intLeagueID;        

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
        /// Gets the Divisions.
        /// </summary>
        public void GetDivisions()
        {

        }
    }
}
