using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using TrackerLibrary.Configuration;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        private const string conStringName = "Tournaments";

        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString(conStringName)))
            {
                var dp = new DynamicParameters();
                dp.Add("@FirstName", model.FirstName);
                dp.Add("@LastName", model.LastName);
                dp.Add("@EmailAddress", model.EmailAddress);
                dp.Add("@CellphoneNumber", model.CellphoneNumber);
                dp.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", dp, commandType: CommandType.StoredProcedure);

                // Get Saved Model Id
                model.Id = dp.Get<int>("@id");

                // return Saved Model
                return model;
            }
        }

        public PriceModel CreatePrice(PriceModel model)
        {
            // destroy connection object after curly bracket to prevent memory leak by using (using) 
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString(conStringName)))
            {
                var dp = new DynamicParameters();
                dp.Add("@PlaceNumber", model.PlaceNumber);
                dp.Add("@PlaceName", model.PlaceName);
                dp.Add("@PriceAmount", model.PriceAmount);
                dp.Add("@PricePercentage", model.PricePercentage);
                dp.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrices_Insert", dp, commandType: CommandType.StoredProcedure);

                // Get Saved Model Id
                model.Id = dp.Get<int>("@id");

                // return Saved Model
                return model;
            }

        }

        public TeamModel CreateTeam(TeamModel teamModel)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString(conStringName)))
            {
                var dp = new DynamicParameters();
                dp.Add("@TeamName", teamModel.TeamName);
                dp.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeam_Insert", dp, commandType: CommandType.StoredProcedure);

                // Get Saved Model Id
                teamModel.Id = dp.Get<int>("@id");

                foreach (PersonModel p in teamModel.TeamMembers)
                {
                    var dpForTeamMembers = new DynamicParameters();
                    dpForTeamMembers.Add("@PersonId", p.Id);
                    dpForTeamMembers.Add("@TeamId", teamModel.Id);
                    connection.Execute("dbo.spTeamMembers_Insert", dpForTeamMembers, commandType: CommandType.StoredProcedure);
                }

                // return Saved Model
                return teamModel;
            }
        }

        public TournamentModel CreateTournamet(TournamentModel model)
        {

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString(conStringName)))
            {
                SaveTournament(model, connection);

                SaveTournamentPrices(model, connection);

                SaveTournamentTeams(model, connection);

                SaveTournamentRounds(model, connection);

                return model;

            }
        }

        private void SaveTournamentRounds(TournamentModel model, IDbConnection connection)
        {


            model.Rounds.ForEach(matchUps =>
            {
                matchUps.ForEach(matchUp =>
                {
                    var dp = new DynamicParameters();
                    dp.Add("@TournamentId", model.Id);
                    dp.Add("@MatchUpRound", matchUp.MatchUpRound);
                    dp.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchUps_Insert", dp, commandType: CommandType.StoredProcedure);

                    matchUp.Id = dp.Get<int>("@Id");

                    matchUp.Entries.ForEach(matchUpEntry =>
                    {
                        dp = new DynamicParameters();
                        dp.Add("@MatchUpId", matchUp.Id);
                        dp.Add("@ParentMatchUpId", matchUpEntry.ParentMatchup != null ? matchUpEntry.ParentMatchup.Id : null);
                        dp.Add("@TeamCompetingId", matchUpEntry.TeamCompeting != null ? matchUpEntry.TeamCompeting.Id : null);
                        dp.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("dbo.spMatchUpEntries_Insert", dp, commandType: CommandType.StoredProcedure);
                    });
                });

            });
        }

        private void SaveTournament(TournamentModel model, IDbConnection connection)
        {
            var dp = new DynamicParameters();
            dp.Add("@TournamentName", model.TournamentName);
            dp.Add("@EntryFee", model.EntryFee);
            dp.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournaments_Insert", dp, commandType: CommandType.StoredProcedure);

            // Get Saved Model Id
            model.Id = dp.Get<int>("@id");
        }

        private void SaveTournamentPrices(TournamentModel model, IDbConnection connection)
        {
            model.Prices.ForEach(p =>
            {
                var dpForPrice = new DynamicParameters();
                dpForPrice.Add("@TournamentId", model.Id);
                dpForPrice.Add("@PriceId", p.Id);
                connection.Execute("dbo.spTournamentPrices_Insert", dpForPrice, commandType: CommandType.StoredProcedure);
            });
        }

        private void SaveTournamentTeams(TournamentModel model, IDbConnection connection)
        {
            model.EnterredTeams.ForEach(t =>
            {
                var dPForTeams = new DynamicParameters();
                dPForTeams.Add("@TournamentId", model.Id);
                dPForTeams.Add("@TeamId", t.Id);
                connection.Execute("dbo.spTournamentEntries_Insert", dPForTeams, commandType: CommandType.StoredProcedure);
            });
        }

        public List<PersonModel> GetPersonList()
        {

            List<PersonModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString(conStringName)))
            {
                output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }

            return output;


        }

        public List<TeamModel> GetTeamList()
        {

            List<TeamModel> teams;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString(conStringName)))
            {
                teams = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();

                teams.ForEach(team =>
                {
                    var dp = new DynamicParameters();
                    dp.Add("@TeamId", team.Id);
                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", dp, commandType: CommandType.StoredProcedure).ToList();
                });

            }

            return teams;

        }

        public List<TournamentModel> GetAvailableTournaments()
        {
            List<TournamentModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString(conStringName)))
            {
                output = connection.Query<TournamentModel>("dbo.spTournamemts_GetAvailableTournaments").ToList();

                output.ForEach(t =>
                {
                    var dp = new DynamicParameters();
                    dp.Add("@TournamentId", t.Id);

                    // Populate Prices
                    t.Prices = connection.Query<PriceModel>("dbo.spTournamentPrices_GetByTournament", dp, commandType: CommandType.StoredProcedure).ToList();

                    // Populate Teams
                    t.EnterredTeams = connection.Query<TeamModel>("dbo.TournamentEntries_GetByTournament", dp, commandType: CommandType.StoredProcedure).ToList();
                    t.EnterredTeams.ForEach(team =>
                    {
                        dp = new DynamicParameters();
                        dp.Add("@TeamId", team.Id);
                        team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", dp, commandType: CommandType.StoredProcedure).ToList();
                    });

                    // Populate Rounds
                    dp = new DynamicParameters();
                    dp.Add("@TournamentId", t.Id);
                    // Matchup
                    List<MatchupModel> matchUps = connection.Query<MatchupModel>("dbo.spMatchUps_GetByTournament", dp, commandType: CommandType.StoredProcedure).ToList();

                    matchUps.ForEach(matchUp =>
                    {
                        // Winner
                        if (matchUp.WinnerId > 0)
                        {
                            matchUp.Winner = GetTeamList().Where(t => t.Id == matchUp.WinnerId).First();
                        }

                        // MatchupEntries
                        dp = new DynamicParameters();
                        dp.Add("@MatchUpId", matchUp.Id);
                        matchUp.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchUpEntries_GetByMatchUp", dp, commandType: CommandType.StoredProcedure).ToList();

                        List<TeamModel> allTeams = GetTeamList();
                        matchUp.Entries.ForEach(entry =>
                        {
                            if (entry.TeamCompetingId > 0)
                            {
                                // MatchupEntries - TeamCompeting
                                entry.TeamCompeting = allTeams.Where(t => t.Id == entry.TeamCompetingId).First();
                            }
                            if (entry.ParentMatchcupId > 0)
                            {
                                // MatchupEntries - ParentMatchup
                                entry.ParentMatchup = matchUps.Where(m => m.Id == entry.ParentMatchcupId).First();
                            }
                        });
                    });

                    // Populate Round List
                    List<MatchupModel> currentRow = new List<MatchupModel>();
                    int currentRound = 1;
                    matchUps.ForEach(matchUp =>
                    {
                        if (matchUp.MatchUpRound > currentRound)
                        {
                            t.Rounds.Add(currentRow);
                            currentRow = new List<MatchupModel>();
                            currentRound++;
                        }
                        currentRow.Add(matchUp);

                    });
                    t.Rounds.Add(currentRow);
                });
            }

            return output;
        }

        public void UpdateMatchUp(MatchupModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.ConString(conStringName)))
            {

                var dp = new DynamicParameters();
                dp.Add("@Id", model.Id);
                dp.Add("@WinnerId", model.Winner.Id);
                connection.Execute("dbo.spMatchUps_Update", dp, commandType: CommandType.StoredProcedure);

                //spMatchUpEntries_Update

                model.Entries.ForEach(me =>
                {
                    dp = new DynamicParameters();
                    dp.Add("@Id", me.Id);
                    dp.Add("@TeamCompetingId", me.TeamCompeting.Id);
                    dp.Add("@Score", me.Score);
                    connection.Execute("dbo.spMatchUpEntries_Update", dp, commandType: CommandType.StoredProcedure);
                });
            }
        }
    }
}
