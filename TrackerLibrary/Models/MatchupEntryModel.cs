using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupEntryModel
    {
        public int Id { get; set; }

        public int TeamCompetingId { get; set; }
        /// <summary>
        /// Represents one team in the matchup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Represent the score for this particular team
        /// </summary>
        public double Score { get; set; }

        public int ParentMatchcupId { get; set; }
        /// <summary>
        /// Respresents the matchup that this team came from as the winner
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }


    }
}
