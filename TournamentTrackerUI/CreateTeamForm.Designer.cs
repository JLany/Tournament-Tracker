namespace TournamentTrackerUI
{
    partial class CreateTeamForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            teamNameTextBox = new TextBox();
            teamNameLabel = new Label();
            headerLabel = new Label();
            addMemberButton = new Button();
            selectTeamMemberComboBox = new ComboBox();
            selectTeamMemberLabel = new Label();
            createNewMemberGroupBox = new GroupBox();
            createMemberButton = new Button();
            phoneNumberTextBox = new TextBox();
            phoneNumberLabel = new Label();
            emailTextBox = new TextBox();
            emailLabel = new Label();
            lastNameTextBox = new TextBox();
            lastNameLabel = new Label();
            firstNameTextBox = new TextBox();
            firstNameLabel = new Label();
            teamMembersListBox = new ListBox();
            teamMembersLabel = new Label();
            removeSelectedMemberButton = new Button();
            createTeamButton = new Button();
            createNewMemberGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // teamNameTextBox
            // 
            teamNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            teamNameTextBox.Location = new Point(34, 134);
            teamNameTextBox.Name = "teamNameTextBox";
            teamNameTextBox.Size = new Size(396, 35);
            teamNameTextBox.TabIndex = 13;
            // 
            // teamNameLabel
            // 
            teamNameLabel.AutoSize = true;
            teamNameLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            teamNameLabel.ForeColor = SystemColors.MenuHighlight;
            teamNameLabel.Location = new Point(25, 94);
            teamNameLabel.Name = "teamNameLabel";
            teamNameLabel.Size = new Size(157, 37);
            teamNameLabel.TabIndex = 12;
            teamNameLabel.Text = "Team Name";
            // 
            // headerLabel
            // 
            headerLabel.AutoSize = true;
            headerLabel.Font = new Font("Segoe UI Light", 27.75F, FontStyle.Regular, GraphicsUnit.Point);
            headerLabel.ForeColor = SystemColors.MenuHighlight;
            headerLabel.Location = new Point(25, 32);
            headerLabel.Name = "headerLabel";
            headerLabel.Size = new Size(213, 50);
            headerLabel.TabIndex = 11;
            headerLabel.Text = "Create Team";
            // 
            // addMemberButton
            // 
            addMemberButton.FlatAppearance.BorderColor = Color.Silver;
            addMemberButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(102, 102, 102);
            addMemberButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 242, 242);
            addMemberButton.FlatStyle = FlatStyle.Flat;
            addMemberButton.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            addMemberButton.ForeColor = SystemColors.MenuHighlight;
            addMemberButton.Location = new Point(131, 305);
            addMemberButton.Name = "addMemberButton";
            addMemberButton.Size = new Size(243, 38);
            addMemberButton.TabIndex = 19;
            addMemberButton.Text = "Add Member";
            addMemberButton.UseVisualStyleBackColor = true;
            // 
            // selectTeamMemberComboBox
            // 
            selectTeamMemberComboBox.FormattingEnabled = true;
            selectTeamMemberComboBox.Location = new Point(34, 239);
            selectTeamMemberComboBox.Name = "selectTeamMemberComboBox";
            selectTeamMemberComboBox.Size = new Size(396, 38);
            selectTeamMemberComboBox.TabIndex = 18;
            // 
            // selectTeamMemberLabel
            // 
            selectTeamMemberLabel.AutoSize = true;
            selectTeamMemberLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            selectTeamMemberLabel.ForeColor = SystemColors.MenuHighlight;
            selectTeamMemberLabel.Location = new Point(25, 199);
            selectTeamMemberLabel.Name = "selectTeamMemberLabel";
            selectTeamMemberLabel.Size = new Size(263, 37);
            selectTeamMemberLabel.TabIndex = 17;
            selectTeamMemberLabel.Text = "Select Team Member";
            // 
            // createNewMemberGroupBox
            // 
            createNewMemberGroupBox.Controls.Add(createMemberButton);
            createNewMemberGroupBox.Controls.Add(phoneNumberTextBox);
            createNewMemberGroupBox.Controls.Add(phoneNumberLabel);
            createNewMemberGroupBox.Controls.Add(emailTextBox);
            createNewMemberGroupBox.Controls.Add(emailLabel);
            createNewMemberGroupBox.Controls.Add(lastNameTextBox);
            createNewMemberGroupBox.Controls.Add(lastNameLabel);
            createNewMemberGroupBox.Controls.Add(firstNameTextBox);
            createNewMemberGroupBox.Controls.Add(firstNameLabel);
            createNewMemberGroupBox.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            createNewMemberGroupBox.ForeColor = SystemColors.HotTrack;
            createNewMemberGroupBox.Location = new Point(34, 382);
            createNewMemberGroupBox.Name = "createNewMemberGroupBox";
            createNewMemberGroupBox.Size = new Size(419, 296);
            createNewMemberGroupBox.TabIndex = 20;
            createNewMemberGroupBox.TabStop = false;
            createNewMemberGroupBox.Text = "Create New Member";
            // 
            // createMemberButton
            // 
            createMemberButton.FlatAppearance.BorderColor = Color.Silver;
            createMemberButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(102, 102, 102);
            createMemberButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 242, 242);
            createMemberButton.FlatStyle = FlatStyle.Flat;
            createMemberButton.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            createMemberButton.ForeColor = SystemColors.MenuHighlight;
            createMemberButton.Location = new Point(121, 231);
            createMemberButton.Name = "createMemberButton";
            createMemberButton.Size = new Size(187, 48);
            createMemberButton.TabIndex = 21;
            createMemberButton.Text = "Create Member";
            createMemberButton.UseVisualStyleBackColor = true;
            // 
            // phoneNumberTextBox
            // 
            phoneNumberTextBox.BorderStyle = BorderStyle.FixedSingle;
            phoneNumberTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            phoneNumberTextBox.Location = new Point(179, 168);
            phoneNumberTextBox.Name = "phoneNumberTextBox";
            phoneNumberTextBox.Size = new Size(217, 35);
            phoneNumberTextBox.TabIndex = 28;
            // 
            // phoneNumberLabel
            // 
            phoneNumberLabel.AutoSize = true;
            phoneNumberLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            phoneNumberLabel.ForeColor = SystemColors.MenuHighlight;
            phoneNumberLabel.Location = new Point(19, 168);
            phoneNumberLabel.Name = "phoneNumberLabel";
            phoneNumberLabel.Size = new Size(154, 30);
            phoneNumberLabel.TabIndex = 27;
            phoneNumberLabel.Text = "Phone Number";
            // 
            // emailTextBox
            // 
            emailTextBox.BorderStyle = BorderStyle.FixedSingle;
            emailTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            emailTextBox.Location = new Point(179, 127);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new Size(217, 35);
            emailTextBox.TabIndex = 26;
            // 
            // emailLabel
            // 
            emailLabel.AutoSize = true;
            emailLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            emailLabel.ForeColor = SystemColors.MenuHighlight;
            emailLabel.Location = new Point(19, 127);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(63, 30);
            emailLabel.TabIndex = 25;
            emailLabel.Text = "Email";
            // 
            // lastNameTextBox
            // 
            lastNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            lastNameTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lastNameTextBox.Location = new Point(179, 86);
            lastNameTextBox.Name = "lastNameTextBox";
            lastNameTextBox.Size = new Size(217, 35);
            lastNameTextBox.TabIndex = 24;
            // 
            // lastNameLabel
            // 
            lastNameLabel.AutoSize = true;
            lastNameLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lastNameLabel.ForeColor = SystemColors.MenuHighlight;
            lastNameLabel.Location = new Point(19, 88);
            lastNameLabel.Name = "lastNameLabel";
            lastNameLabel.Size = new Size(112, 30);
            lastNameLabel.TabIndex = 23;
            lastNameLabel.Text = "Last Name";
            // 
            // firstNameTextBox
            // 
            firstNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            firstNameTextBox.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            firstNameTextBox.Location = new Point(179, 45);
            firstNameTextBox.Name = "firstNameTextBox";
            firstNameTextBox.Size = new Size(217, 35);
            firstNameTextBox.TabIndex = 22;
            // 
            // firstNameLabel
            // 
            firstNameLabel.AutoSize = true;
            firstNameLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            firstNameLabel.ForeColor = SystemColors.MenuHighlight;
            firstNameLabel.Location = new Point(19, 47);
            firstNameLabel.Name = "firstNameLabel";
            firstNameLabel.Size = new Size(113, 30);
            firstNameLabel.TabIndex = 21;
            firstNameLabel.Text = "First Name";
            // 
            // teamMembersListBox
            // 
            teamMembersListBox.BorderStyle = BorderStyle.FixedSingle;
            teamMembersListBox.FormattingEnabled = true;
            teamMembersListBox.ItemHeight = 30;
            teamMembersListBox.Location = new Point(495, 134);
            teamMembersListBox.Name = "teamMembersListBox";
            teamMembersListBox.Size = new Size(299, 542);
            teamMembersListBox.TabIndex = 21;
            // 
            // teamMembersLabel
            // 
            teamMembersLabel.AutoSize = true;
            teamMembersLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            teamMembersLabel.ForeColor = SystemColors.MenuHighlight;
            teamMembersLabel.Location = new Point(490, 94);
            teamMembersLabel.Name = "teamMembersLabel";
            teamMembersLabel.Size = new Size(197, 37);
            teamMembersLabel.TabIndex = 22;
            teamMembersLabel.Text = "Team Members";
            // 
            // removeSelectedMemberButton
            // 
            removeSelectedMemberButton.FlatAppearance.BorderColor = Color.Silver;
            removeSelectedMemberButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(102, 102, 102);
            removeSelectedMemberButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 242, 242);
            removeSelectedMemberButton.FlatStyle = FlatStyle.Flat;
            removeSelectedMemberButton.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            removeSelectedMemberButton.ForeColor = Color.IndianRed;
            removeSelectedMemberButton.Location = new Point(803, 305);
            removeSelectedMemberButton.Name = "removeSelectedMemberButton";
            removeSelectedMemberButton.Size = new Size(113, 78);
            removeSelectedMemberButton.TabIndex = 23;
            removeSelectedMemberButton.Text = "Remove Selected";
            removeSelectedMemberButton.UseVisualStyleBackColor = true;
            // 
            // createTeamButton
            // 
            createTeamButton.FlatAppearance.BorderColor = Color.Silver;
            createTeamButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(102, 102, 102);
            createTeamButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 242, 242);
            createTeamButton.FlatStyle = FlatStyle.Flat;
            createTeamButton.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            createTeamButton.ForeColor = SystemColors.MenuHighlight;
            createTeamButton.Location = new Point(325, 706);
            createTeamButton.Name = "createTeamButton";
            createTeamButton.Size = new Size(266, 68);
            createTeamButton.TabIndex = 25;
            createTeamButton.Text = "Create Team";
            createTeamButton.UseVisualStyleBackColor = true;
            // 
            // CreateTeamForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(936, 786);
            Controls.Add(createTeamButton);
            Controls.Add(removeSelectedMemberButton);
            Controls.Add(teamMembersLabel);
            Controls.Add(teamMembersListBox);
            Controls.Add(createNewMemberGroupBox);
            Controls.Add(addMemberButton);
            Controls.Add(selectTeamMemberComboBox);
            Controls.Add(selectTeamMemberLabel);
            Controls.Add(teamNameTextBox);
            Controls.Add(teamNameLabel);
            Controls.Add(headerLabel);
            Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(5, 6, 5, 6);
            Name = "CreateTeamForm";
            Text = "Create Team";
            createNewMemberGroupBox.ResumeLayout(false);
            createNewMemberGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox teamNameTextBox;
        private Label teamNameLabel;
        private Label headerLabel;
        private Button addMemberButton;
        private ComboBox selectTeamMemberComboBox;
        private Label selectTeamMemberLabel;
        private GroupBox createNewMemberGroupBox;
        private TextBox phoneNumberTextBox;
        private Label phoneNumberLabel;
        private TextBox emailTextBox;
        private Label emailLabel;
        private TextBox lastNameTextBox;
        private Label lastNameLabel;
        private TextBox firstNameTextBox;
        private Label firstNameLabel;
        private Button createMemberButton;
        private ListBox teamMembersListBox;
        private Label teamMembersLabel;
        private Button removeSelectedMemberButton;
        private Button createTeamButton;
    }
}