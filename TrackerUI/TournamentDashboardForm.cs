using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.Configuration;
using TrackerLibrary.Interfaces;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentDashboardForm : Form
    {
        private List<TournamentModel> availableTournaments = GlobalConfig.connection.GetAvailableTournaments();

        public TournamentDashboardForm()
        {
            InitializeComponent();

            InitializedList();
        }

        void InitializedList()
        {

            loadExistingTrounamentDropDown.DataSource = null;

            loadExistingTrounamentDropDown.DataSource = availableTournaments;
            loadExistingTrounamentDropDown.DisplayMember = "TournamentName";

        }

        private void createTrounamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm frm = new CreateTournamentForm();
            frm.Show();
        }

        private void loadTrounamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel t = (TournamentModel)loadExistingTrounamentDropDown.SelectedItem;
            TournamentViewerForm frm = new TournamentViewerForm(t);
            frm.Show();
        }
       
    }
}
