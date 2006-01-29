/* League.cs, MARCO CECCARELLI
 * La classe definisce le leghe in cui sono presenti i diversi campionati */

namespace Mojhy.League
{
    /// <summary>
    /// Defines all the twenty areas of the field
    /// </summary>
    class League
    {
        private string l_strName;
        private int l_intLeagueID;
        
        /// <summary>
        /// Gets the Name of the League.
        /// </summary>        
        public string Name
        {
            get { return l_strName; }
            set { l_strName = value; }
        }

        /// <summary>
        /// Gets or sets the league ID.
        /// </summary>
        /// <value>The league ID.</value>
        public int LeagueID
        {
            get { return l_intLeagueID; }
            set { l_intLeagueID = value; }
        }
        
    }
}
