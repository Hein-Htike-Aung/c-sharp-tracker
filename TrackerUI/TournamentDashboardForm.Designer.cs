namespace TrackerUI
{
    partial class TournamentDashboardForm
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
            this.loadExistingTournamentLabel = new System.Windows.Forms.Label();
            this.tournamentDashboardLabel = new System.Windows.Forms.Label();
            this.loadTrounamentButton = new System.Windows.Forms.Button();
            this.createTrounamentButton = new System.Windows.Forms.Button();
            this.loadExistingTrounamentDropDown = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // loadExistingTournamentLabel
            // 
            this.loadExistingTournamentLabel.AutoSize = true;
            this.loadExistingTournamentLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.loadExistingTournamentLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.loadExistingTournamentLabel.Location = new System.Drawing.Point(205, 128);
            this.loadExistingTournamentLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.loadExistingTournamentLabel.Name = "loadExistingTournamentLabel";
            this.loadExistingTournamentLabel.Size = new System.Drawing.Size(289, 32);
            this.loadExistingTournamentLabel.TabIndex = 27;
            this.loadExistingTournamentLabel.Text = "Load Existing Tournament";
            // 
            // tournamentDashboardLabel
            // 
            this.tournamentDashboardLabel.AutoSize = true;
            this.tournamentDashboardLabel.Font = new System.Drawing.Font("Segoe UI Light", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tournamentDashboardLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.tournamentDashboardLabel.Location = new System.Drawing.Point(105, 38);
            this.tournamentDashboardLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tournamentDashboardLabel.Name = "tournamentDashboardLabel";
            this.tournamentDashboardLabel.Size = new System.Drawing.Size(488, 62);
            this.tournamentDashboardLabel.TabIndex = 26;
            this.tournamentDashboardLabel.Text = "Tournament Dashboard";
            // 
            // loadTrounamentButton
            // 
            this.loadTrounamentButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.loadTrounamentButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loadTrounamentButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.loadTrounamentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.loadTrounamentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.loadTrounamentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadTrounamentButton.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.loadTrounamentButton.ForeColor = System.Drawing.Color.White;
            this.loadTrounamentButton.Location = new System.Drawing.Point(223, 247);
            this.loadTrounamentButton.Name = "loadTrounamentButton";
            this.loadTrounamentButton.Size = new System.Drawing.Size(253, 51);
            this.loadTrounamentButton.TabIndex = 38;
            this.loadTrounamentButton.Text = "Load Tournament";
            this.loadTrounamentButton.UseVisualStyleBackColor = false;
            this.loadTrounamentButton.Click += new System.EventHandler(this.loadTrounamentButton_Click);
            // 
            // createTrounamentButton
            // 
            this.createTrounamentButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.createTrounamentButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.createTrounamentButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.createTrounamentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.createTrounamentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
            this.createTrounamentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createTrounamentButton.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.createTrounamentButton.ForeColor = System.Drawing.Color.White;
            this.createTrounamentButton.Location = new System.Drawing.Point(223, 326);
            this.createTrounamentButton.Name = "createTrounamentButton";
            this.createTrounamentButton.Size = new System.Drawing.Size(253, 51);
            this.createTrounamentButton.TabIndex = 39;
            this.createTrounamentButton.Text = "Create Trounament";
            this.createTrounamentButton.UseVisualStyleBackColor = false;
            this.createTrounamentButton.Click += new System.EventHandler(this.createTrounamentButton_Click);
            // 
            // loadExistingTrounamentDropDown
            // 
            this.loadExistingTrounamentDropDown.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.loadExistingTrounamentDropDown.FormattingEnabled = true;
            this.loadExistingTrounamentDropDown.Location = new System.Drawing.Point(167, 188);
            this.loadExistingTrounamentDropDown.Name = "loadExistingTrounamentDropDown";
            this.loadExistingTrounamentDropDown.Size = new System.Drawing.Size(364, 38);
            this.loadExistingTrounamentDropDown.TabIndex = 40;
            // 
            // TournamentDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(732, 450);
            this.Controls.Add(this.loadExistingTrounamentDropDown);
            this.Controls.Add(this.createTrounamentButton);
            this.Controls.Add(this.loadTrounamentButton);
            this.Controls.Add(this.loadExistingTournamentLabel);
            this.Controls.Add(this.tournamentDashboardLabel);
            this.Name = "TournamentDashboardForm";
            this.Text = "Tournament Dashboard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label loadExistingTournamentLabel;
        private Label tournamentDashboardLabel;
        private Button loadTrounamentButton;
        private Button createTrounamentButton;
        private ComboBox loadExistingTrounamentDropDown;
    }
}