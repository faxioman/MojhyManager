/* LeagueDB.cs, Ceccarelli Marco
 
 * La classe gestisci l'accesso ai dati per l'oggetto League
 
 */
 

using System;
using System.Data;
using System.Configuration;
using Mojhy.Leagues;
using Mojhy.Schedules;
using Npgsql;

namespace Mojhy.DataAccess
{

    //using System.Data.OleDb;


    public class LeaguesDB : CommonDB
    {

        public int InsertLeague(League objLeague)
        {
            string SQL;
            // With...
            SQL = "INSERT INTO Nazioni (Nome) VALUES ( "
              + (FormatField(objLeague.Name, true, true));
            RunCommand(SQL);

            //Ritorna l'ultimo id impostato
            //SQL = "select currval('"Calendario"."campionati_campionatoid_seq"') as ID;";
            objLeague.LeagueID = (int)GetField(SQL, "ID");

            return objLeague.LeagueID;

        }

        public int InsertDivision(Division objDiv)
        {
            string SQL;
            // With...
            SQL = ("INSERT INTO Campionati ([Name], [NazioneID]) VALUES ("
                        + (FormatField(objDiv.Name, true, true))
                        + (FormatField(objDiv.LeagueID, false, false)) + ")");
            RunCommand(SQL);

            //Ritorna l'ultimo id impostato
            //SQL = "select currval('"Calendario"."campionati_campionatoid_seq"') as ID;";
            objDiv.DivisionID = (int)GetField(SQL, "ID");
            
            return objDiv.DivisionID;

        }

        public void UpdateDivision(Division objDivision)
        {
            string SQL;
            // With...
            SQL = ("UPDATE Campionati SET Name = " + (FormatField(objDivision.Name, true, true)) + 
                " NazioneID = " + (FormatField(objDivision.LeagueID, false, false)) + 
                " WHERE CampionatoID = " + objDivision.DivisionID);
            RunCommand(SQL);
        }

        public int InsertSeason(Season objSeason)
        {
            string SQL;
            // With...
            SQL = ("INSERT INTO Stagioni ([Name], [StagioneID]) VALUES ("
                        + (FormatField(objSeason.Name, true, true))
                        + (FormatField(objSeason.SeasonID, false, false)) + ")");
            RunCommand(SQL);

            //Ritorna l'ultimo id impostato
            //SQL = "select currval('"Calendario"."campionati_campionatoid_seq"') as ID;";
            objSeason.SeasonID = (int)GetField(SQL, "ID");

            return objSeason.SeasonID;

        }

        public void UpdateSeason(Season objSeason)
        {
            string SQL;
            // With...
            SQL = ("UPDATE Stagioni SET Name = " + (FormatField(objSeason.Name, true, true)) +
                " WHERE StagioneID = " + objSeason.SeasonID);
            RunCommand(SQL);
        }
        
        public void UpdateGame(Game objGame)
        {
            string SQL;
            // With...
            SQL = ("UPDATE Partita SET " 
                + " SquadraCasaID = " + (FormatField(objGame.HomeTeamID, false, true))
                + " SquadraFuoriID = " + (FormatField(objGame.AwayTeamID, false, true))
                + " ScoreSquadraCasa = " + (FormatField(objGame.HomeScore, false, true))
                + " ScoreSquadraFuori = " + (FormatField(objGame.AwayScore, false, true))
                + " DataPartita = " + (FormatField(objGame.GameDate, false, true))
                + " StadioID = " + (FormatField(objGame.StadiumID, false, true)) 
                + " CampionatoID = " + (FormatField(objGame.DivisionID, false, true))
                + " StagioneID = " + (FormatField(objGame.SeasonID, false, false)) 
                + " WHERE PartitaID = " + objGame.GameID);
            RunCommand(SQL);
        }

        public int InsertGame(Game objGame)
        {
            string SQL;
            // With...
            SQL = ("INSERT INTO Partita ( SquadraCasaID, SquadraFuoriID, ScoreSquadraCasa, ScoreSquadraFuori, DataPartita, StadioID, CampionatoID, StagioneID ) VALUES ("
                        + (FormatField(objGame.HomeTeamID, true, true))
                        + (FormatField(objGame.AwayTeamID, true, true))
                        + (FormatField(objGame.HomeScore, false, true))
                        + (FormatField(objGame.AwayScore, false, true))
                        + (FormatField(objGame.GameDate, false, true))
                        + (FormatField(objGame.StadiumID, false, true))
                        + (FormatField(objGame.DivisionID, false, true))
                        + (FormatField(objGame.SeasonID, false, true)) + ")");
            RunCommand(SQL);
                       
            //Ritorna l'ultimo id impostato
            //SQL = "select currval('"Calendario"."campionati_campionatoid_seq"') as ID;";

            objGame.GameID = (int)GetField(SQL,"ID");
            
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
            
            NpgsqlDataReader dr = GetDataReader(SQL);
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
            NpgsqlDataReader dr = GetDataReader(SQL);
            while (dr.Read())
            {
                Result = (int)dr["Level"];
                dr.Close();
                return Result;
            }
            return 0;
        }

        NpgsqlDataReader GetTeamsRemainingInPlayoffs()
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
            NpgsqlDataReader dr = GetDataReader(SQL);
            while (dr.Read())
            {
                if (((int)dr["Status"] == 2))
                {
                    Result = (int)dr["ScheduleID"];
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
            NpgsqlDataReader dr = GetDataReader(SQL);
            while (dr.Read())
            {
                Result = (int)dr["WinnerID"];
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
            NpgsqlDataReader dr = GetDataReader(SQL);
            while (dr.Read())
            {
                Result = (int)dr["WinnerID"];
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
            NpgsqlDataReader dr = GetDataReader(SQL);
            while (dr.Read())
            {
                Result = (int)dr["Level"];
                dr.Close();
                return Result;
            }
            return 0;
        }

        bool GetPlayoffsComplete()
        {
            string SQL = ("SELECT Count(ps.PlayoffSeriesID) AS Winners, ps.Level " + ("FROM PlayoffSeries AS ps " + ("WHERE(((ps.WinnerID) > 0) And ((ps.LoserID) > 0)) " + ("GROUP BY ps.Level " + ("HAVING(((Count(ps.PlayoffSeriesID)) = 1)) " + "ORDER BY ps.Level DESC;")))));
            
            NpgsqlDataReader dr = GetDataReader(SQL);
            while (dr.Read())
            {
                if (((int)dr["Winners"] == 1))
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

        NpgsqlDataReader GetSeries(int SeriesID)
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

        NpgsqlDataReader GetLeague()
        {
            string SQL = "SELECT * FROM Leagues Where LeagueID = 1";
            return GetDataReader(SQL);
        }

        NpgsqlDataReader GetConference(int ConferenceID)
        {
            string SQL = ("SELECT * FROM Conferences WHERE ConferenceID = " + ConferenceID);
            return GetDataReader(SQL);
        }

        NpgsqlDataReader GetConferences()
        {
            string SQL = "SELECT * FROM Conferences ORDER BY ConferenceID";
            return GetDataReader(SQL);
        }

        NpgsqlDataReader GetDivisions()
        {
            string SQL = "SELECT * FROM Divisions ORDER BY DivisionID";
            return GetDataReader(SQL);
        }

        NpgsqlDataReader GetSchedule()
        {
            string SQL = "SELECT * FROM Schedule ORDER BY GameDate, ScheduleID";
            return GetDataReader(SQL);
        }

        int GetLastSeriesID()
        {
            string SQL = "SELECT TOP 1 SeriesID FROM Schedule ORDER BY SeriesID DESC";
            int Result=-1;
            NpgsqlDataReader dr = GetDataReader(SQL);
            while (dr.Read())
            {
                Result = (int)dr["SeriesID"];
                break; //Warning!!! Review that break works as 'Exit Do' as it could be in a nested instruction like switch
            }
            dr.Close();
            return Result;
        }

        NpgsqlDataReader GetConferenceDivisionNames()
        {
            string SQL = ("SELECT c.Name AS ConferenceName, d.Name as DivisionName, d.DivisionID " + (" FROM Conferences c INNER JOIN Divisions d ON c.ConferenceID = d.ConferenceID " + " ORDER BY d.ConferenceID, d.DivisionID"));
            return GetDataReader(SQL);
        }
    }
}

