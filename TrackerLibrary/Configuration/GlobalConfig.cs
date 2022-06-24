using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Enumeration;

namespace TrackerLibrary.Configuration
{
    public static class GlobalConfig
    {
        public const string PriceFileName = "PriceModels.csv";
        public const string PeopleFileName = "PeopleModels.csv";
        public const string TeamFileName = "TeamModels.csv";
        public const string TournamentFileName = "TournamentModels.csv";
        public const string MatchUpFile = "MatchUpModels.csv";
        public const string MatchUpEntryFile = "MatchUpEntryModels.csv";

        public static IDataConnection connection { get; private set; }

        public static void InitializeConnections(DataConnectionType type)
        {
            switch (type)
            {
                case DataConnectionType.SQL:
                    SqlConnector sql = new SqlConnector();
                    connection = sql;
                    break;
                case DataConnectionType.TEXTFILE:
                    TextConnector text = new TextConnector();
                    connection = text;
                    break;
            }
        }

        /// <summary>
        /// Give ConnectionString from TrackerUI.App.config file
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ConString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
