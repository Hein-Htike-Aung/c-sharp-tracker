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
using TrackerLibrary.Models;
using TrackerLibrary.Interfaces;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {

        private List<PersonModel> availableTeamMembers = GlobalConfig.connection.GetPersonList();
        private List<PersonModel> selectedMembers = new List<PersonModel>();
        private ITeamRequester callingForm;

        public object TeamMembersModel { get; private set; }

        public CreateTeamForm(ITeamRequester teamRequester)
        {
            InitializeComponent();

            this.callingForm = teamRequester;

            WireUpLists();

        }

        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;
            teamMemberListBox.DataSource = null;

            // Add Data into Drop Down
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            // Add Data into List Box
            teamMemberListBox.DataSource = selectedMembers;
            teamMemberListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateMemberForm())
            {
                PersonModel personModel = new PersonModel();
                personModel.FirstName = firstNameValue.Text;
                personModel.LastName = lastNameValue.Text;
                personModel.EmailAddress = emailValue.Text;
                personModel.CellphoneNumber = cellPhoneValue.Text;

                personModel = GlobalConfig.connection.CreatePerson(personModel);

                availableTeamMembers.Add(personModel);
                this.WireUpLists();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellPhoneValue.Text = "";

            }
            else
            {
                MessageBox.Show("Invalid Members Fields. Try Again");
            }
        }

        private bool ValidateMemberForm()
        {

            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }
            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }
            if (emailValue.Text.Length == 0)
            {
                return false;
            }
            if (cellPhoneValue.Text.Length == 0)
            {
                return false;
            }

            return true;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel selectedPerson = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            if (selectedPerson != null)
            {
                availableTeamMembers.Remove(selectedPerson);

                selectedMembers.Add(selectedPerson);

                // Refresh Data
                this.WireUpLists();
            }

        }

        private void removeTeamMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel personModel = (PersonModel)teamMemberListBox.SelectedItem;

            if (personModel != null)
            {
                selectedMembers.Remove(personModel);
                availableTeamMembers.Add(personModel);

                this.WireUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if (ValidTeamMemberForm())
            {
                TeamModel teamModel = new TeamModel();
                teamModel.TeamName = teamNameValue.Text;
                teamModel.TeamMembers = selectedMembers;

                GlobalConfig.connection.CreateTeam(teamModel);


                this.callingForm.Request(teamModel);
                this.Close();

                //teamNameValue.Text = "";
                //teamMemberListBox.DataSource = null;
                //availableTeamMembers = GlobalConfig.connection.GetPersonList();
                //selectedMembers.Clear();

                //WireUpLists();

            }
            else
            {
                MessageBox.Show("Fail to Create Team.");
            }
        }

        private bool ValidTeamMemberForm()
        {
            if (teamNameValue.Text.Length == 0)
            {
                return false;
            }
            if (selectedMembers.Count == 0)
            {
                return false;
            }


            return true;
        }


    }
}
