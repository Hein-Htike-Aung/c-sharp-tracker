using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.Common
{
    public static class TeamMatchUpInTournament
    {
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizeTeams = RandomizeTeamOrder(model.EnterredTeams);

            int rounds = FindNumberOfRounds(randomizeTeams.Count);
            int byes = NumberOfByes(rounds, randomizeTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizeTeams));

            CreateOtherRounds(model, rounds);
        }

        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currentRound = new List<MatchupModel>();
            MatchupModel currentMatchUp = new MatchupModel();

            while (round <= rounds)
            {
                previousRound.ForEach(match =>
                {
                    currentMatchUp.Entries.Add(new MatchupEntryModel { ParentMatchup = match });

                    if (currentMatchUp.Entries.Count > 1)
                    {
                        currentMatchUp.MatchUpRound = round;
                        currentRound.Add(currentMatchUp);
                        currentMatchUp = new MatchupModel();
                    }
                });

                model.Rounds.Add(currentRound);
                previousRound = currentRound;

                currentRound = new List<MatchupModel>();
                round += 1;
            }
        }

        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();

            MatchupModel matchup = new MatchupModel();

            teams.ForEach(team =>
            {
                matchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                if (byes > 0 || matchup.Entries.Count > 1)
                {
                    matchup.MatchUpRound = 1;
                    output.Add(matchup);
                    matchup = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            });

            return output;
        }

        private static int NumberOfByes(int rounds, int teamCount)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i < rounds; i++)
            {
                totalTeams *= 2;
            }

            output = teamCount - totalTeams;

            return output;
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output++;
                teamCount -= 2;
                // val *= 2;
            }

            return output;
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(t => Guid.NewGuid()).ToList();
        }
    }
}
