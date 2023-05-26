using TournamentTrackerLibrary;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerUI
{
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableMembers = GlobalConfig.Connector.GetPerson_All();
        private List<PersonModel> selectedMembers = new();

        public CreateTeamForm()
        {
            InitializeComponent();

            // set up
            WireUpLists();

            // event handlers
            addMemberButton.Click += AddMemberButton_Click;
            createMemberButton.Click += CreateMemberButton_Click;
            removeSelectedMemberButton.Click += RemoveSelectedMemberButton_Click;
        }

        private void AddMemberButton_Click(object? sender, EventArgs e)
        {
            var selectedMember = (PersonModel)selectTeamMemberComboBox.SelectedItem;

            if (selectedMember != null)
            {
                selectedMembers.Add(selectedMember);
                availableMembers.Remove(selectedMember);

                WireUpLists();
            }
        }

        private void CreateMemberButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateMemberForm())
            {
                MessageBox.Show("You need to fill in all the fields to add a new member."
                    , "Invalid member data");

                return;
            }

            var person = new PersonModel
            {
                FirstName = firstNameTextBox.Text,
                LastName = lastNameTextBox.Text,
                EmailAddress = emailTextBox.Text,
                PhoneNumber = phoneNumberTextBox.Text,
            };

            GlobalConfig.Connector.CreatePerson(person);

            selectedMembers.Add(person);

            WireUpLists();
            ResetMemberForm();
        }

        private void RemoveSelectedMemberButton_Click(object? sender, EventArgs e)
        {
            var selectedMember = (PersonModel)teamMembersListBox.SelectedItem;

            if (selectedMember != null)
            {
                selectedMembers.Remove(selectedMember);
                availableMembers.Add(selectedMember);

                WireUpLists();
            }
        }

        private void WireUpLists()
        {
            selectTeamMemberComboBox.DataSource = null;
            selectTeamMemberComboBox.DataSource = availableMembers;
            selectTeamMemberComboBox.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private bool ValidateMemberForm()
        {
            // TODO - Enhance member form validation

            // Do not use if statements on the same line in other cases, to facilitate debugging
            if (firstNameTextBox.Text.Length < 1) { return false; }

            if (lastNameTextBox.Text.Length < 1) { return false; }

            if (emailTextBox.Text.Length < 1) { return false; }

            if (phoneNumberTextBox.Text.Length < 1) { return false; }

            return true;
        }

        private void ResetMemberForm()
        {
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            emailTextBox.Text = "";
            phoneNumberTextBox.Text = "";
        }
    }
}
