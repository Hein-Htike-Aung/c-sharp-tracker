using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.Common;
using TrackerLibrary.Configuration;
using TrackerLibrary.Interfaces;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPriceRequester, ITeamRequester
    {

        private List<TeamModel> availableTeams = GlobalConfig.connection.GetTeamList();
        private List<TeamModel> selectedTeams = new List<TeamModel>();
        private List<PriceModel> selectedPrices = new List<PriceModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

            InitializeLists();
        }

        private void InitializeLists()
        {
            selectTeamDropDown.DataSource = null;
            tournamentPlayersListbox.DataSource = null;
            priceListbox.DataSource = null;

            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentPlayersListbox.DataSource = selectedTeams;
            tournamentPlayersListbox.DisplayMember = "TeamName";

            priceListbox.DataSource = selectedPrices;
            priceListbox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel teamModel = (TeamModel)selectTeamDropDown.SelectedItem;

            if (teamModel != null)
            {
                availableTeams.Remove(teamModel);
                selectedTeams.Add(teamModel);

                InitializeLists();
            }

        }

        private void deleteSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            TeamModel teamModel = (TeamModel)tournamentPlayersListbox.SelectedItem;

            if (teamModel != null)
            {
                availableTeams.Add(teamModel);
                selectedTeams.Remove(teamModel);

                InitializeLists();
            }

        }

        private void createPriceButton_Click(object sender, EventArgs e)
        {
            CreatePriceForm frm = new CreatePriceForm(this);
            frm.Show();

        }

        public void PriceComplete(PriceModel model)
        {

            if (model != null)
            {
                this.selectedPrices.Add(model);

                InitializeLists();
            }
        }

        private void createNewLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        public void Request(TeamModel teamModel)
        {
            if(teamModel != null)
            {
                this.selectedTeams.Add(teamModel);

                InitializeLists();
            }
        }

        private void deleteSelectPriceButton_Click(object sender, EventArgs e)
        {
            PriceModel price =  (PriceModel) priceListbox.SelectedItem;

            if(price != null)
            {
                this.selectedPrices.Remove(price);

                this.InitializeLists();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {

            if(ValidateForm())
            {
                TournamentModel tm = new TournamentModel();

                tm.TournamentName = tournamentNameValue.Text;
                tm.EntryFee = decimal.Parse(entryFeeValue.Text);
                tm.Prices = selectedPrices;
                tm.EnterredTeams = selectedTeams;

                // Wire Matchup
                TeamMatchUpInTournament.CreateRounds(tm);

                GlobalConfig.connection.CreateTournamet(tm);

                tournamentNameValue.Text = "";
                entryFeeValue.Text = "";
                selectedPrices.Clear();
                selectedTeams.Clear();
                availableTeams = GlobalConfig.connection.GetTeamList();
                InitializeLists();
            }
            else
            {
                MessageBox.Show("Fill All Values!!!", "Invalild Form", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool ValidateForm()
        {

            decimal fee = 0;
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out fee);
            if(!feeAcceptable || fee <= 0)
            {
                return false;
            }

            if(tournamentNameValue.Text.Length == 0)
            {
                return false;
            }

            if(selectedPrices.Count == 0 || selectedTeams.Count == 0)
            {
                return false;
            }

            return true;
        }
    }

}
