/* Team.cs, FABIO MASINI
 * La classe definisce l'oggetto 'squadra'.
 * Incapsula anche i giocatori della squadra */

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Mojhy.League;


namespace Mojhy.DataAccess
{

    //using System.Data.OleDb;


    public class LeaguesDB : CommonDB
    {
                
        int InsertLeague(League objLeague)
        {
            string SQL;
            // With...
            SQL = "INSERT INTO Nazioni (Nome) VALUES ( "
              + (FormatField(objLeague.Name, true, true));
            RunCommand(SQL);

            return objLeague.LeagueID;

        }

        int InsertDivision(Division objDiv)
        {
            string SQL;
            // With...
            SQL = ("INSERT INTO Campionati " + ("([Name], [NazioneID])" + ("VALUES ("
                        + (FormatField(objDiv.Name, true, true)
                        + (FormatField(objDiv.LeagueID, false, false) + ")")))));
            RunCommand(SQL);
            return objDiv.DivisionID;
        }

        void UpdateDivision(Division objDivision)
        {
            string SQL;
            // With...
            SQL = ("UPDATE Campionati SET " + ("Name = "
                        + (FormatField(objDivision.Name, true, true) + ("NazioneID = "
                        + (FormatField(objDivision.ConferenceID, false, false) + (" WHERE CampionatoID = " + objDivision.ID))))));
            RunCommand(SQL);
        }



        void UpdateGame(Game objGame)
        {
            string SQL;
            // With...
            SQL = ("UPDATE Schedule SET " + (" HomeScore = "
                        + (FormatField(objGame.HomeScore, false, true) + (" AwayScore = "
                        + (FormatField(objGame.AwayScore, false, true) + (" Status = "
                        + (FormatField(objGame.Status, false, true) + (" Overtime = "
                        + (FormatField(objGame.Overtime, false, true) + (" Attendance = "
                        + (FormatField(objGame.Attendance, false, true) + (" SeriesID = "
                        + (FormatField(objGame.SeriesID, false, true) + (" Season = "
                        + (FormatField(objGame.Season, false, false) + (" WHERE ScheduleID = " + objGame.GameID))))))))))))))));
            RunCommand(SQL);
        }

        int InsertGame(Game objGame)
        {
            string SQL;
            // With...
            SQL = ("INSERT INTO Schedule " + ("(ScheduleID, GameDate, AwayTeamID, HomeTeamID, AwayScore, HomeScore, " + (" Phase, Overtime, Attendance, Status, SeriesID, Season)" + ("VALUES ("
                        + (FormatField(objGame.GameID, false, true)
                        + (FormatField(objGame.GameDate, true, true)
                        + (FormatField(objGame.AwayTeamID, false, true)
                        + (FormatField(objGame.HomeTeamID, false, true)
                        + (FormatField(objGame.AwayScore, false, true)
                        + (FormatField(objGame.HomeScore, false, true)
                        + (FormatField(objGame.Phase, false, true)
                        + (FormatField(objGame.Overtime, false, true)
                        + (FormatField(objGame.Attendance, false, true)
                        + (FormatField(objGame.Status, false, true)
                        + (FormatField(objGame.SeriesID, false, true)
                        + (FormatField(objGame.Season, false, false) + ")"))))))))))))))));
            RunCommand(SQL);
            return objGame.GameID;
        }



        void UpdateDefaultTeamID(int DefaultTeamID)
        {
            string SQL;
            SQL = ("UPDATE Leagues SET DefaultTeamID = " + DefaultTeamID);
            RunCommand(SQL);
        }

        void UpdatePlayoffRound(int Round)
        {
            string SQL;
            SQL = ("UPDATE Leagues SET PlayoffRound = " + Round);
            RunCommand(SQL);
        }



        void InsertSeries(int SeriesID, int Season, int TopSeedID, int BottomSeedID, int Level, int TopSeed, int BottomSeed)
        {
            string SQL;
            SQL = ("INSERT INTO PlayoffSeries ([Level], TopSeed, BottomSeed, SeriesID, Season, TopSeedID, BottomSeedID) V" +
            "ALUES ("
                        + (FormatField(Level, false, true)
                        + (FormatField(TopSeed, false, true)
                        + (FormatField(BottomSeed, false, true)
                        + (FormatField(SeriesID, false, true)
                        + (FormatField(Season, false, true)
                        + (FormatField(TopSeedID, false, true)
                        + (FormatField(BottomSeedID, false, false) + ")"))))))));
            RunCommand(SQL, false);
        }

        bool GetPlayoffLevelComplete(int SeriesID)
        {
            string SQL = ("SELECT ps2.*, ps.SeriesID " + ("FROM PlayoffSeries AS ps INNER JOIN PlayoffSeries AS ps2 ON ps.[Level] = ps2.[Level] " + ("WHERE (((ps2.WinnerID)=0) AND ((ps.SeriesID)="
                        + (SeriesID + "));"))));
            bool Result;
            OleDbDataReader dr = GetDataReader(SQL);
            if (dr.HasRows)
            {
                dr.Close();
                return false;
            }
            else
            {
                dr.Close();
                return true;
            }
        }

        int GetLastPlayoffLevel()
        {
            string SQL = ("SELECT Top 1 ps.[Level]  " + ("FROM PlayoffSeries ps  " + ("WHERE ps.WinnerID > 0 " + "ORDER BY SeriesID DESC;")));
            int Result;
            OleDbDataReader dr = GetDataReader(SQL);
            while (dr.Read)
            {
                Result = dr.Item["Level"];
                dr.Close();
                return Result;
            }
            return 0;
        }

        OleDbDataReader GetTeamsRemainingInPlayoffs()
        {
            int Level;
            Level = this.GetLastPlayoffLevel();
            string SQL = (" SELECT PlayoffSeries.Level, PlayoffSeries.WinnerID, PlayOffSeries.WinnerSeed" + ("            FROM(PlayoffSeries)" + (" WHERE PlayoffSeries.Level = "
                        + (Level + (" GROUP BY PlayoffSeries.Level, PlayOffSeries.WinnerSeed, PlayoffSeries.WinnerID" + " ORDER BY PlayoffSeries.Level DESC , PlayoffSeries.WinnerSeed, PlayoffSeries.WinnerID;")))));
            return GetDataReader(SQL);
        }

        int GetNextTentativePlayoffGame(int SeriesID)
        {
            string SQL;
            SQL = ("SELECT TOP 1 Schedule.SeriesID, Schedule.Status, Schedule.GameDate, Schedule.ScheduleID " + ("FROM Schedule " + ("WHERE (((Schedule.SeriesID) = "
                        + (SeriesID + (")) AND Schedule.Status = 2  " + "ORDER BY Schedule.GameDate ASC; ")))));
            int Result;
            OleDbDataReader dr = GetDataReader(SQL);
            while (dr.Read)
            {
                if ((dr.Item["Status"] == 2))
                {
                    Result = dr.Item["ScheduleID"];
                }
                else
                {
                    Result = 0;
                }
                dr.Close();
                return Result;
            }
            return 0;
        }

        int GetLowSeedFromSeries(int Round)
        {
            string SQL;
            SQL = ("SELECT TOP 1 ps.WinnerID, t.PlayoffSeed " + ("FROM PlayoffSeries AS ps LEFT JOIN Teams AS t ON ps.WinnerID = t.TeamID " + ("WHERE(((ps.Level) = "
                        + (Round + (")) " + "ORDER BY t.PlayoffSeed DESC; ")))));
            int Result;
            OleDbDataReader dr = GetDataReader(SQL);
            while (dr.Read)
            {
                Result = dr.Item["WinnerID"];
                dr.Close();
                return Result;
            }
            return 0;
        }

        int GetHighSeedFromSeries(int Round)
        {
            string SQL;
            SQL = ("SELECT TOP 1 ps.WinnerID, t.PlayoffSeed " + ("FROM PlayoffSeries AS ps LEFT JOIN Teams AS t ON ps.WinnerID = t.TeamID " + ("WHERE(((ps.Level) = "
                        + (Round + (")) " + "ORDER BY t.PlayoffSeed ASC; ")))));
            int Result;
            OleDbDataReader dr = GetDataReader(SQL);
            while (dr.Read)
            {
                Result = dr.Item["WinnerID"];
                dr.Close();
                return Result;
            }
            return 0;
        }

        int GetLastPlayoffRound()
        {
            string SQL;
            SQL = "SELECT TOP 1 ps.Level FROM PlayoffSeries AS ps ORDER BY ps.Level DESC;";
            int Result;
            OleDbDataReader dr = GetDataReader(SQL);
            while (dr.Read)
            {
                Result = dr.Item["Level"];
                dr.Close();
                return Result;
            }
            return 0;
        }

        bool GetPlayoffsComplete()
        {
            string SQL = ("SELECT Count(ps.PlayoffSeriesID) AS Winners, ps.Level " + ("FROM PlayoffSeries AS ps " + ("WHERE(((ps.WinnerID) > 0) And ((ps.LoserID) > 0)) " + ("GROUP BY ps.Level " + ("HAVING(((Count(ps.PlayoffSeriesID)) = 1)) " + "ORDER BY ps.Level DESC;")))));
            bool Result;
            OleDbDataReader dr = GetDataReader(SQL);
            while (dr.Read)
            {
                if ((dr.Item["Winners"] == 1))
                {
                    dr.Close();
                    return true;
                }
                else
                {
                    dr.Close();
                    return false;
                }
            }
            dr.Close();
            return false;
        }

        void UpdateSeries(int SeriesID, int WinnerID, int LoserID, int WinnerSeed)
        {
            string SQL;
            SQL = ("UPDATE PlayoffSeries SET WinnerID = "
                        + (WinnerID + (", LoserID = "
                        + (LoserID + (", WinnerSeed = "
                        + (WinnerSeed + (" WHERE SeriesID = " + SeriesID)))))));
            RunCommand(SQL);
        }

        OleDbDataReader GetSeries(int SeriesID)
        {
            string SQL;
            SQL = ("SELECT * FROM PlayoffSeries WHERE SeriesID = " + SeriesID);
            return GetDataReader(SQL);
        }

        void DeleteUnusedGamesFromSeries(int SeriesID)
        {
            string SQL;
            SQL = ("DELETE FROM Schedule WHERE SeriesID = "
                        + (SeriesID + " AND Status = 2"));
            RunCommand(SQL);
        }

        OleDbDataReader GetLeague()
        {
            string SQL = "SELECT * FROM Leagues Where LeagueID = 1";
            return GetDataReader(SQL);
        }

        OleDbDataReader GetConference(int ConferenceID)
        {
            string SQL = ("SELECT * FROM Conferences WHERE ConferenceID = " + ConferenceID);
            return GetDataReader(SQL);
        }

        OleDbDataReader GetConferences()
        {
            string SQL = "SELECT * FROM Conferences ORDER BY ConferenceID";
            return GetDataReader(SQL);
        }

        OleDbDataReader GetDivisions()
        {
            string SQL = "SELECT * FROM Divisions ORDER BY DivisionID";
            return GetDataReader(SQL);
        }

        void GetSchedule()
        {
            string SQL = "SELECT * FROM Schedule ORDER BY GameDate, ScheduleID";
            return GetDataReader(SQL);
        }

        int GetLastSeriesID()
        {
            string SQL = "SELECT TOP 1 SeriesID FROM Schedule ORDER BY SeriesID DESC";
            int Result;
            OleDbDataReader dr = GetDataReader(SQL);
            while (dr.Read())
            {
                Result = dr.Item["SeriesID"];
                break; //Warning!!! Review that break works as 'Exit Do' as it could be in a nested instruction like switch
            }
            dr.Close();
            return Result;
        }

        void GetConferenceDivisionNames()
        {
            string SQL = ("SELECT c.Name AS ConferenceName, d.Name as DivisionName, d.DivisionID " + (" FROM Conferences c INNER JOIN Divisions d ON c.ConferenceID = d.ConferenceID " + " ORDER BY d.ConferenceID, d.DivisionID"));
            return GetDataReader(SQL);
        }
    }
}

