using Mojhy.DataAccess;

namespace Schedules
{

    public class Game : IComparable
    {

        private int GameID;

        private int HomeTeamID;

        private int AwayTeamID;

        private int HomeScore;

        private int AwayScore;

        private bool Overtime;

        private ISMGameScheduleStatus Status;

        private ISMPhase Phase;

        private int Attendance;

        private bool Used;

        private int Season;

        private DateTime GameDate;

        private int WinnerID;

        private int LoserID;

        private int WinnerScore;

        private int LoserScore;

        private int SeriesID;


        void Save()
        {
            LeaguesDB Data = new LeaguesDB();
            //Data.InsertGame(this);
            Data.Close();
        }

        void UpdateResult()
        {
            LeaguesDB Data = new LeaguesDB();
            //Data.UpdateGame(this);
            Data.Close();
        }

        int CompareTo(object obj)
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

        string GetScheduleText()
        {
            Team HomeTeam = Sim.League.GetTeamByID(this.HomeTeamID);
            Team AwayTeam = Sim.League.GetTeamByID(this.AwayTeamID);
            string Result;
            if ((this.Status == ISMGameScheduleStatus.NotPlayed))
            {
                Result = (AwayTeam.Name + (" at " + HomeTeam.Name));
            }
            else if ((this.Status == ISMGameScheduleStatus.Played))
            {
                Result = (AwayTeam.Name + (" "
                            + (this.AwayScore + (", "
                            + (HomeTeam.Name + (" " + this.HomeScore))))));
            }
            else
            {
                Result = (AwayTeam.Name + (" at "
                            + (HomeTeam.Name + " (if needed)")));
            }
            return Result;
        }
    }


}



