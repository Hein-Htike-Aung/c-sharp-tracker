using TrackerLibrary.DataAccess.TextHelpers;
using TrackerLibrary.Models;



namespace TrackerLibrary.DataAccess
{

    public class TextConnector : IDataConnection
    {
        private const string PriceFileName = "PriceModels.csv";
        private const string PeopleFileName = "PeopleModels.csv";
        private const string TeamFileName = "TeamModels.csv";
        private const string TournamentFileName = "TournamentModels.csv";
        private const string MatchUpFile = "MatchUpModels.csv";
        private const string MatchUpEntryFile = "MatchUpEntryModels.csv";


        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> storedPeople = PeopleFileName.FullFilePath().LoadFiles().ConvertToPersonModel();

            int currentId = 1;

            if (storedPeople.Count > 0)
            {
                currentId = storedPeople
                        .OrderByDescending(p => p.Id)
                        .First().Id + 1;
            }

            model.Id = currentId;

            storedPeople.Add(model);

            storedPeople.SaveToPeopleModel(PeopleFileName);

            return model;
        }

        public PriceModel CreatePrice(PriceModel model)
        {
            List<PriceModel> storedPrices = PriceFileName.FullFilePath().LoadFiles().ConvertToPriceModel();

            int currentId = 1;

            if (storedPrices.Count > 0)
            {
                currentId = storedPrices
               .OrderByDescending(p => p.Id)
               .First().Id + 1;
            }

            model.Id = currentId;

            storedPrices.Add(model);

            storedPrices.SaveToPriceFile(PriceFileName);

            return model;
        }

        public TeamModel CreateTeam(TeamModel teamModel)
        {

            GetPersonByIdDelegate callback = GetPersonById;
            List<TeamModel> teams = TeamFileName.FullFilePath().LoadFiles().ConvertToTeamModels(callback);

            int currentId = 1;

            if(teams.Count > 0)
            {
                currentId = teams.OrderByDescending(t => t.Id).First().Id + 1;
            }

            teamModel.Id = currentId;

            teams.Add(teamModel);

            teams.SaveToTeamsFile(TeamFileName);

            return teamModel;
        }

        public List<PersonModel> GetPersonList()
        {
            return PeopleFileName.FullFilePath().LoadFiles().ConvertToPersonModel();
        }

        public PersonModel GetPersonById(int id)
        {
            List<PersonModel> storedPeople = PeopleFileName.FullFilePath().LoadFiles().ConvertToPersonModel();

            return storedPeople.Find(p => p.Id == id);
        }

        public TeamModel GetTeamById(int id)
        {
            GetPersonByIdDelegate callback = GetPersonById;
            List<TeamModel> storedTeams = TeamFileName.FullFilePath().LoadFiles().ConvertToTeamModels(callback);

            return storedTeams.Find(t => t.Id == id);
        }

        public PriceModel GetPriceById(int id)
        {
            List<PriceModel> storedPeople = PeopleFileName.FullFilePath().LoadFiles().ConvertToPriceModel();

            return storedPeople.Find(p => p.Id == id);
        }

        public List<TeamModel> GetTeamList()
        {

            GetPersonByIdDelegate callback = GetPersonById;

            return TeamFileName.FullFilePath().LoadFiles().ConvertToTeamModels(callback);

        }

        public TournamentModel CreateTournamet(TournamentModel model)
        {
            // Save To Tournamet, TournamentPrices & TournamentTeams File
            GetTeamByIdDelegate getTeamByIdCallback = GetTeamById;
            GetPriceByIdDelegate getPriceByIdCallback = GetPriceById;
            
            List<TournamentModel> tournaments = TournamentFileName.FullFilePath().LoadFiles().ConvertToTournamentModels(getTeamByIdCallback, getPriceByIdCallback);

            int currentId = 1;

            if(tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(t => t.Id).First().Id + 1;
            }

            model.Id = currentId;

            // Save To MatchUp & MatchUpEntries File
            model.SaveRoundsToFile(MatchUpFile, MatchUpEntryFile);

            tournaments.Add(model);

            tournaments.SaveToTournamentFile(TournamentFileName);

            return model;
        }

        public List<TournamentModel> GetAvailableTournaments()
        {
            throw new NotImplementedException();
        }

        public void UpdateMatchUp(MatchupModel model)
        {
            throw new NotImplementedException();
        }
    }
}
