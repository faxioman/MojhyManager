/* Division.cs, MARCO CECCARELLI
 * La classe definisce le stagioni dei vari campionati */

using System;

namespace Mojhy.Leagues
{
        
    /// <summary>
    /// Definisce le divisioni
    /// </summary>
    public class Season
    {
        private string l_strName;
        private int l_intSeasonID;

        /// <summary>
        /// Gets / Sets the Name of the Season.
        /// </summary>        
        public string Name
        {
            get { return l_strName; }
            set { l_strName = value; }
        }


        /// <summary>
        /// Gets / Sets the SeasonID.
        /// </summary>
        /// <value>The Season ID.</value>
        public int SeasonID
        {
            get { return l_intSeasonID; }
            set { l_intSeasonID = value; }
        }

        void Save()
        {
            LeaguesDB Data = new LeaguesDB();
            Data.Insert(this);
            Data.Close();
        }

        void Update()
        {
            LeaguesDB Data = new LeaguesDB();
            Data.UpdateDivision(this);
            Data.Close();
        }

        /// <summary>
        /// Gets the teams.
        /// </summary>
        public void GetTeams()
        {

        }
    }
}
