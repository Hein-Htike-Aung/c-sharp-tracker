using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        PriceModel CreatePrice(PriceModel model);
        PersonModel CreatePerson(PersonModel model);
        List<PersonModel> GetPersonList();
        TeamModel CreateTeam(TeamModel teamModel);
        List<TeamModel> GetTeamList();
        List<TournamentModel> GetAvailableTournaments();
        TournamentModel CreateTournamet(TournamentModel model);
        void UpdateMatchUp(MatchupModel model);
    }
}
