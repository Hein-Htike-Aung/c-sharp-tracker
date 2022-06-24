using TrackerLibrary.Configuration;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel tournament;
        List<int> rounds = new List<int>();
        List<MatchupModel> selectedMatchUps = new List<MatchupModel>();
        public TournamentViewerForm(TournamentModel tournament)
        {
            InitializeComponent();

            this.tournament = tournament;

            LoadFormData();

            LoadRounds();
        }

        private void LoadFormData()
        {
            tournamentName.Text = this.tournament.TournamentName;
        }

        private void LoadRounds()
        {
            rounds = new List<int>();
            rounds.Add(1);
            int currentRound = 1;

            this.tournament.Rounds.ForEach(matchUps =>
            {
                if (matchUps.First().MatchUpRound > currentRound)
                {
                    this.rounds.Add(matchUps.First().MatchUpRound);
                }
            });

            WireUpRoundsLists();
        }

        private void WireUpRoundsLists()
        {
            roundDropDown.DataSource = null;

            roundDropDown.DataSource = rounds;
        }

        private void WireUpMatchUpLists()
        {
            matchUpListBox.DataSource = null;

            matchUpListBox.DataSource = selectedMatchUps;
            matchUpListBox.DisplayMember = "DisplayName";
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchUps((int)roundDropDown.SelectedItem);

            WireUpMatchUpLists();
        }

        private void LoadMatchUps(int round)
        {
            this.tournament.Rounds.ForEach(matchUps =>
            {
                if (matchUps.First().MatchUpRound == round)
                {
                    selectedMatchUps.Clear();
                    matchUps.ForEach(matchUp =>
                    {
                        if (matchUp.Winner == null || !unPlayedOnlyCheckbox.Checked)
                        {
                            selectedMatchUps.Add(matchUp);
                        }
                    });

                }
            });

            DisplayMatchUpInfo();
        }

        private void DisplayMatchUpInfo()
        {
            bool isVisible = (selectedMatchUps.Count > 0);

            teamOneName.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamTwoName.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            versusLabel.Visible = isVisible;
            scoreButton.Visible = isVisible;
        }

        private void matchUpListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchUp();
        }

        private void LoadMatchUp()
        {
            MatchupModel m = (MatchupModel)matchUpListBox.SelectedItem;

            if (m != null)
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entries[0].TeamCompeting != null)
                        {
                            teamOneName.Text = m.Entries[0].TeamCompeting.TeamName;
                            teamOneScoreValue.Text = m.Entries[0].Score.ToString();

                            teamTwoName.Text = "No Team Two";
                            teamTwoScoreValue.Text = "0";
                        }
                        else
                        {
                            teamOneName.Text = "Not Yet Set";
                            teamOneScoreValue.Text = "";
                        }
                    }

                    if (i == 1)
                    {
                        if (m.Entries[1].TeamCompeting != null)
                        {
                            teamTwoName.Text = m.Entries[1].TeamCompeting.TeamName;
                            teamTwoScoreValue.Text = m.Entries[1].Score.ToString();
                        }
                        else
                        {
                            teamTwoName.Text = "Not Yet Set";
                            teamTwoScoreValue.Text = "";
                        }
                    }
                }
            }

        }

        private void unPlayedOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchUps((int)roundDropDown.SelectedItem);

            WireUpMatchUpLists();
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            MatchupModel m = (MatchupModel)matchUpListBox.SelectedItem;
            double teamOneScore = 0;
            double teamTwoScore = 0;

            for (int i = 0; i < m.Entries.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {

                        bool scoreValid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);
                        if (scoreValid)
                        {
                            m.Entries[0].Score = teamOneScore;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Score Value For Team one");
                            return;
                        }
                    }
                }

                if (i == 1)
                {
                    if (m.Entries[1].TeamCompeting != null)
                    {

                        bool scoreValid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);
                        if (scoreValid)
                        {
                            m.Entries[1].Score = teamTwoScore;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Score Value For Team Two");
                            return;
                        }

                    }
                }
            }

            if (teamOneScore > teamTwoScore)
            {
                m.Winner = m.Entries[0].TeamCompeting;
            }
            else if (teamTwoScore > teamOneScore)
            {
                m.Winner = m.Entries[1].TeamCompeting;
            }
            else
            {
                MessageBox.Show("Don't Handel Tie Game");
            }

            this.tournament.Rounds.ForEach(matchUps =>
            {
                matchUps.ForEach(matchUp =>
                {
                    matchUp.Entries.ForEach(entry =>
                    {
                        if(entry.ParentMatchup != null)
                        {
                            if (entry.ParentMatchup.Id == matchUp.Id)
                            {
                                entry.TeamCompeting = matchUp.Winner;

                                // Update MatchUps
                                GlobalConfig.connection.UpdateMatchUp(matchUp);
                            }
                        }
                    });
                });
            });

            // Refresh The List
            LoadMatchUps((int)roundDropDown.SelectedItem);
            WireUpMatchUpLists();

            // Update MatchUps & MatchUpEntries
            GlobalConfig.connection.UpdateMatchUp(m);
        }
    }
}