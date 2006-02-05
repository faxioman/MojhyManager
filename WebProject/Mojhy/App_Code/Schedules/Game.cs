
using System;
using Mojhy.DataAccess;

namespace Mojhy.Schedules
{

    public class Game : IComparable
    {

        private int l_intGameID;
        private int l_intHomeTeamID;
        private int l_intAwayTeamID;
        private int l_intHomeScore;
        private int l_intAwayScore;
        private int l_intSeasonID;
        private int l_intDivisionID;
        private int l_intStadiumID;
        private DateTime l_dtGameDate;                     

        /// <summary>
        /// Gets / Sets the GameID.
        /// </summary>
        /// <value>The GameID.</value>
        public int GameID
        {
            get { return l_intGameID; }
            set { l_intGameID = value; }
        }

        /// <summary>
        /// Gets / Sets the HomeTeamID.
        /// </summary>
        /// <value>The HomeTeamID</value>
        public int HomeTeamID
        {
            get { return l_intHomeTeamID; }
            set { l_intHomeTeamID = value; }
        }

        /// <summary>
        /// Gets / Sets the AwayTeamID.
        /// </summary>
        /// <value>The AwayTeamID</value>
        public int AwayTeamID
        {
            get { return l_intAwayTeamID; }
            set { l_intAwayTeamID = value; }
        }

        /// <summary>
        /// Gets / Sets the StadiumID.
        /// </summary>
        /// <value>The StadiumID</value>
        public int StadiumID
        {
            get { return l_intStadiumID; }
            set { l_intStadiumID = value; }
        }

        /// <summary>
        /// Gets / Sets the DivisionID.
        /// </summary>
        /// <value>The DivisionID</value>
        public int DivisionID
        {
            get { return l_intDivisionID; }
            set { l_intDivisionID = value; }
        }

        /// <summary>
        /// Gets / Sets the SeasonID.
        /// </summary>
        /// <value>The SeasonID</value>
        public int SeasonID
        {
            get { return l_intSeasonID; }
            set { l_intSeasonID = value; }
        }

        /// <summary>
        /// Gets / Sets the Home Score.
        /// </summary>
        /// <value>The Home Score</value>
        public int HomeScore
        {
            get { return l_intHomeScore; }
            set { l_intHomeScore = value; }
        }

        /// <summary>
        /// Gets / Sets the AwayScore.
        /// </summary>
        /// <value>The AwayScore</value>
        public int AwayScore
        {
            get { return l_intAwayScore; }
            set { l_intAwayScore = value; }
        }

        /// <summary>
        /// Gets / Sets the Game Date.
        /// </summary>
        /// <value>The Game Date</value>
        public DateTime GameDate
        {
            get { return l_dtGameDate; }
            set { l_dtGameDate = value; }
        }

        void Save()
        {
            LeaguesDB Data = new LeaguesDB();
            Data.InsertGame(this);
            Data.Close();
        }

        void UpdateResult()
        {
            LeaguesDB Data = new LeaguesDB();
            Data.UpdateGame(this);
            Data.Close();
        }

        public int CompareTo(object obj)
        {
            if ((obj == null))
            {
                return 1;
            }
            Game other = ((Game)(obj));
            if ((this.GameDate > other.GameDate))
            {
                return 1;
            }
            else if ((this.GameDate < other.GameDate))
            {
                return -1;
            }
            else if ((this.GameID > other.GameID))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }


        //string GetScheduleText()
        //{
        //    Team HomeTeam = Sim.League.GetTeamByID(this.HomeTeamID);
        //    Team AwayTeam = Sim.League.GetTeamByID(this.AwayTeamID);
        //    string Result;
        //    if ((this.Status == ISMGameScheduleStatus.NotPlayed))
        //    {
        //        Result = (AwayTeam.Name + (" at " + HomeTeam.Name));
        //    }
        //    else if ((this.Status == ISMGameScheduleStatus.Played))
        //    {
        //        Result = (AwayTeam.Name + (" "
        //                    + (this.AwayScore + (", "
        //                    + (HomeTeam.Name + (" " + this.HomeScore))))));
        //    }
        //    else
        //    {
        //        Result = (AwayTeam.Name + (" at "
        //                    + (HomeTeam.Name + " (if needed)")));
        //    }
        //    return Result;
        //}
    }


}



