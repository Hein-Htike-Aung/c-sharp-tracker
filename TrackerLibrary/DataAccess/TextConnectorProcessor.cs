using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Configuration;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{

    public delegate PersonModel GetPersonByIdDelegate(int id);
    public delegate TeamModel GetTeamByIdDelegate(int id);
    public delegate PriceModel GetPriceByIdDelegate(int id);

    public static class TextConnectorProcessor
    {

        public static string FullFilePath(this string filenName)
        {
            return $"{ ConfigurationManager.AppSettings["filePath"]}\\{ filenName } ";
        }

        public static List<string> LoadFiles(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            return File.ReadAllLines(file).ToList();
        }

        public static List<PriceModel> ConvertToPriceModel(this List<string> lines)
        {
            List<PriceModel> output = new List<PriceModel>();

            lines.ForEach(line =>
            {
                string[] cols = line.Split(',');

                PriceModel priceModel = new PriceModel();
                priceModel.Id = int.Parse(cols[0]);
                priceModel.PlaceNumber = int.Parse(cols[1]);
                priceModel.PlaceName = cols[2];
                priceModel.PriceAmount = decimal.Parse(cols[3]);
                priceModel.PricePercentage = double.Parse(cols[4]);

                output.Add(priceModel);
            });

            return output;
        }

        public static List<PersonModel> ConvertToPersonModel(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            lines.ForEach(line =>
            {
                string[] cols = line.Split(',');

                PersonModel personModel = new PersonModel();
                personModel.Id = int.Parse(cols[0]);
                personModel.FirstName = cols[1];
                personModel.LastName = cols[2];
                personModel.EmailAddress = cols[3];
                personModel.CellphoneNumber = cols[4];

                output.Add(personModel);
            });

            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, GetPersonByIdDelegate getPersonById)
        {

            // Id,TeamName,Person_IDs
            // 1,Ninja Team,1|3|5

            List<TeamModel> output = new List<TeamModel>();

            lines.ForEach(line =>
            {
                string[] cols = line.Split(',');

                List<PersonModel> people = new List<PersonModel>();

                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');
                foreach (string id in personIds)
                {
                    if (id != "")
                    {
                        t.TeamMembers.Add(getPersonById(int.Parse(id)));
                    }
                }

                output.Add(t);
            });

            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines, GetTeamByIdDelegate getTeamByIdCallback, GetPriceByIdDelegate getPriceByIdCallback)
        {
            List<TournamentModel> output = new List<TournamentModel>();

            // id,TournamentName,EntryFee,(id,id,id - Enterred Teams),(id,id,id - Prices),(id^id^id^|id^id^id^|id^id^id^ - Rounds)
            lines.ForEach(line =>
            {
                string[] cols = line.Split(',');

                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);

                string[] enterredTeamsId = cols[3].Substring(0, cols[3].IndexOf(" ")).Split(',');

                foreach (string id in enterredTeamsId)
                {
                    tm.EnterredTeams.Add(getTeamByIdCallback(int.Parse(id)));
                }

                string[] prices = cols[4].Substring(0, cols[4].IndexOf(" ")).Split(',');

                foreach (string id in prices)
                {
                    tm.Prices.Add(getPriceByIdCallback(int.Parse(id)));
                }

                // TODO: Capture Rounds Info

                output.Add(tm);
            });

            return output;
        }

        public static void SaveToPriceFile(this List<PriceModel> pricesModel, string fileName)
        {
            // Convert the price to a list of string<string>
            List<string> lines = new List<string>();

            foreach (PriceModel p in pricesModel)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PriceAmount },{ p.PricePercentage }");
            }

            // Save To Text File
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPeopleModel(this List<PersonModel> people, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in people)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.CellphoneNumber }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamsFile(this List<TeamModel> teamModels, string teamsFileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in teamModels)
            {
                string personIds = "";
                t.TeamMembers.ForEach(member =>
                {
                    personIds += $"{ member.Id }|";
                });
                lines.Add($"{ t.Id },{ t.TeamName },{ personIds.Substring(0, personIds.Length - 1) }");

            }

            File.WriteAllLines(teamsFileName.FullFilePath(), lines);
        }

        public static void SaveToTournamentFile(this List<TournamentModel> tournaments, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel t in tournaments)
            {
                string enterredTeams = ConvertTeamListToString(t.EnterredTeams);
                string prices = ConvertPriceListToString(t.Prices);

                lines.Add($"" +
                    $"{ t.Id }," +
                    $"{ t.TournamentName }," +
                    $"{ t.EntryFee }," +
                    $"({ enterredTeams.Substring(0, enterredTeams.Length - 1) } - Enterred Teams)," +
                    $"({ prices.Substring(0, enterredTeams.Length - 1) } - Prices)," +
                    $"({ ConvertRoundListToString(t.Rounds) } - Rounds)");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveRoundsToFile(this TournamentModel tournament, string matchUpFile, string matchupEntryFile)
        {
            tournament.Rounds.ForEach(matchUps =>
            {
                matchUps.ForEach(matchup =>
                {
                    matchup.SaveMatchUpToFile(matchUpFile, matchupEntryFile);

        
                });
            });
        }

        private static List<MatchupEntryModel> ConvertStringToMatchUpEntryModel(string input)
        {
            // TODO: 
            return null;
        }

        public static List<MatchupModel> ConvertToMatchUpModel(this List<string> lines)
        {
            List<MatchupModel> output = new List<MatchupModel>();

            lines.ForEach(line =>
            {
                string[] cols = line.Split(',');

                MatchupModel matchupModel = new MatchupModel();
                matchupModel.Id = int.Parse(cols[0]);
                matchupModel.Entries = ConvertStringToMatchUpEntryModel(cols[1]);
                //matchupModel.Winner = GetTeamById(int.Parse(cols[2]));
                matchupModel.MatchUpRound = int.Parse(cols[3]);

                output.Add(matchupModel);
            });

            return output;
        }

        public static void SaveMatchUpToFile(this MatchupModel matchup, string matchUpFile, string matchUpEntryFile)
        {
            matchup.Entries.ForEach(entry =>
            {
                entry.SaveMatchUpEntryToFile(matchUpEntryFile);
            });
        }

        public static void SaveMatchUpEntryToFile(this MatchupEntryModel entry, string matchUpEntryFile)
        {

        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";
            teams.ForEach(team =>
            {
                output += team.Id + ",";
            });

            return output;
        }

        private static string ConvertPriceListToString(List<PriceModel> priceModels)
        {
            string output = "";
            priceModels.ForEach(p =>
            {
                output += p.Id + ",";
            });

            return output;

        }

        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            string output = "";
            if(rounds.Count > 0)
            {
                rounds.ForEach(matchUpList =>
                {
                    matchUpList.ForEach(m => output += $"{ m.Id }^");

                    output += "|";
                });
            }

            return output.Substring(0, output.Length - 1);

        }

    }
}
