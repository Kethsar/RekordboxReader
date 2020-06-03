namespace RekordboxReader
{
    partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            this.PresetShareButton = new System.Windows.Forms.Button();
            this.PresetLoadButton = new System.Windows.Forms.Button();
            this.MasterDeckBox = new System.Windows.Forms.TextBox();
            this.Deck4TitleBox = new System.Windows.Forms.TextBox();
            this.Deck2TitleBox = new System.Windows.Forms.TextBox();
            this.Deck4ArtistBox = new System.Windows.Forms.TextBox();
            this.Deck2ArtistBox = new System.Windows.Forms.TextBox();
            this.Deck3TitleBox = new System.Windows.Forms.TextBox();
            this.Deck1TitleBox = new System.Windows.Forms.TextBox();
            this.Deck3ArtistBox = new System.Windows.Forms.TextBox();
            this.Deck1ArtistBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.PortBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.PresetComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PresetSaveBtn = new System.Windows.Forms.Button();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.StatusMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PresetShareButton
            // 
            this.PresetShareButton.Location = new System.Drawing.Point(292, 45);
            this.PresetShareButton.Name = "PresetShareButton";
            this.PresetShareButton.Size = new System.Drawing.Size(46, 25);
            this.PresetShareButton.TabIndex = 14;
            this.PresetShareButton.Text = "Share";
            this.ttHelp.SetToolTip(this.PresetShareButton, "Copy current settings to clipboard in shareable  form");
            this.PresetShareButton.UseVisualStyleBackColor = true;
            this.PresetShareButton.Click += new System.EventHandler(this.PresetShare);
            // 
            // PresetLoadButton
            // 
            this.PresetLoadButton.Location = new System.Drawing.Point(344, 45);
            this.PresetLoadButton.Name = "PresetLoadButton";
            this.PresetLoadButton.Size = new System.Drawing.Size(46, 25);
            this.PresetLoadButton.TabIndex = 13;
            this.PresetLoadButton.Text = "Load";
            this.ttHelp.SetToolTip(this.PresetLoadButton, "Load shareable settings in clipboard");
            this.PresetLoadButton.UseVisualStyleBackColor = true;
            this.PresetLoadButton.Click += new System.EventHandler(this.PresetLoad);
            // 
            // MasterDeckBox
            // 
            this.MasterDeckBox.Location = new System.Drawing.Point(113, 351);
            this.MasterDeckBox.Name = "MasterDeckBox";
            this.MasterDeckBox.Size = new System.Drawing.Size(277, 20);
            this.MasterDeckBox.TabIndex = 10;
            this.MasterDeckBox.Tag = "MasterDeckPointer";
            this.MasterDeckBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // Deck4TitleBox
            // 
            this.Deck4TitleBox.Location = new System.Drawing.Point(113, 322);
            this.Deck4TitleBox.Name = "Deck4TitleBox";
            this.Deck4TitleBox.Size = new System.Drawing.Size(277, 20);
            this.Deck4TitleBox.TabIndex = 9;
            this.Deck4TitleBox.Tag = "Deck4TitlePointer";
            this.Deck4TitleBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // Deck2TitleBox
            // 
            this.Deck2TitleBox.Location = new System.Drawing.Point(113, 201);
            this.Deck2TitleBox.Name = "Deck2TitleBox";
            this.Deck2TitleBox.Size = new System.Drawing.Size(277, 20);
            this.Deck2TitleBox.TabIndex = 5;
            this.Deck2TitleBox.Tag = "Deck2TitlePointer";
            this.Deck2TitleBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // Deck4ArtistBox
            // 
            this.Deck4ArtistBox.Location = new System.Drawing.Point(113, 292);
            this.Deck4ArtistBox.Name = "Deck4ArtistBox";
            this.Deck4ArtistBox.Size = new System.Drawing.Size(277, 20);
            this.Deck4ArtistBox.TabIndex = 8;
            this.Deck4ArtistBox.Tag = "Deck4ArtistPointer";
            this.Deck4ArtistBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // Deck2ArtistBox
            // 
            this.Deck2ArtistBox.Location = new System.Drawing.Point(113, 170);
            this.Deck2ArtistBox.Name = "Deck2ArtistBox";
            this.Deck2ArtistBox.Size = new System.Drawing.Size(277, 20);
            this.Deck2ArtistBox.TabIndex = 4;
            this.Deck2ArtistBox.Tag = "Deck2ArtistPointer";
            this.Deck2ArtistBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // Deck3TitleBox
            // 
            this.Deck3TitleBox.Location = new System.Drawing.Point(113, 260);
            this.Deck3TitleBox.Name = "Deck3TitleBox";
            this.Deck3TitleBox.Size = new System.Drawing.Size(277, 20);
            this.Deck3TitleBox.TabIndex = 7;
            this.Deck3TitleBox.Tag = "Deck3TitlePointer";
            this.Deck3TitleBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // Deck1TitleBox
            // 
            this.Deck1TitleBox.Location = new System.Drawing.Point(113, 139);
            this.Deck1TitleBox.Name = "Deck1TitleBox";
            this.Deck1TitleBox.Size = new System.Drawing.Size(277, 20);
            this.Deck1TitleBox.TabIndex = 3;
            this.Deck1TitleBox.Tag = "Deck1TitlePointer";
            this.Deck1TitleBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // Deck3ArtistBox
            // 
            this.Deck3ArtistBox.Location = new System.Drawing.Point(113, 230);
            this.Deck3ArtistBox.Name = "Deck3ArtistBox";
            this.Deck3ArtistBox.Size = new System.Drawing.Size(277, 20);
            this.Deck3ArtistBox.TabIndex = 6;
            this.Deck3ArtistBox.Tag = "Deck3ArtistPointer";
            this.Deck3ArtistBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // Deck1ArtistBox
            // 
            this.Deck1ArtistBox.Location = new System.Drawing.Point(113, 109);
            this.Deck1ArtistBox.Name = "Deck1ArtistBox";
            this.Deck1ArtistBox.Size = new System.Drawing.Size(277, 20);
            this.Deck1ArtistBox.TabIndex = 2;
            this.Deck1ArtistBox.Tag = "Deck1ArtistPointer";
            this.Deck1ArtistBox.TextChanged += new System.EventHandler(this.SettingChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 356);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 13);
            this.label12.TabIndex = 42;
            this.label12.Text = "Master Deck ptr";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 324);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 41;
            this.label19.Text = "Deck 4 Title ptr";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 203);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 13);
            this.label11.TabIndex = 40;
            this.label11.Text = "Deck 2 Title ptr";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 294);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(83, 13);
            this.label18.TabIndex = 39;
            this.label18.Text = "Deck 4 Artist ptr";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 173);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Deck 2 Artist ptr";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 264);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 13);
            this.label17.TabIndex = 37;
            this.label17.Text = "Deck 3 Title ptr";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Deck 1 Title ptr";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 233);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 13);
            this.label14.TabIndex = 35;
            this.label14.Text = "Deck 3 Artist ptr";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Deck 1 Artist ptr";
            // 
            // PortBox
            // 
            this.PortBox.Location = new System.Drawing.Point(113, 78);
            this.PortBox.MaxLength = 5;
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(46, 20);
            this.PortBox.TabIndex = 1;
            this.PortBox.TextChanged += new System.EventHandler(this.PortChanged);
            this.PortBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PortKeyPressed);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Port";
            // 
            // PresetComboBox
            // 
            this.PresetComboBox.FormattingEnabled = true;
            this.PresetComboBox.Location = new System.Drawing.Point(113, 47);
            this.PresetComboBox.Name = "PresetComboBox";
            this.PresetComboBox.Size = new System.Drawing.Size(121, 21);
            this.PresetComboBox.TabIndex = 11;
            this.PresetComboBox.Tag = "Name";
            this.PresetComboBox.SelectionChangeCommitted += new System.EventHandler(this.PresetSelected);
            this.PresetComboBox.TextChanged += new System.EventHandler(this.PresetNameChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 55;
            this.label1.Text = "Preset";
            // 
            // PresetSaveBtn
            // 
            this.PresetSaveBtn.Location = new System.Drawing.Point(240, 45);
            this.PresetSaveBtn.Name = "PresetSaveBtn";
            this.PresetSaveBtn.Size = new System.Drawing.Size(46, 25);
            this.PresetSaveBtn.TabIndex = 12;
            this.PresetSaveBtn.Text = "Save";
            this.ttHelp.SetToolTip(this.PresetSaveBtn, "Save current settings to this preset");
            this.PresetSaveBtn.UseVisualStyleBackColor = true;
            this.PresetSaveBtn.Click += new System.EventHandler(this.PresetSaveBtn_Click);
            // 
            // StatusMessage
            // 
            this.StatusMessage.Location = new System.Drawing.Point(15, 9);
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.Size = new System.Drawing.Size(375, 23);
            this.StatusMessage.TabIndex = 0;
            this.StatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 384);
            this.Controls.Add(this.StatusMessage);
            this.Controls.Add(this.PresetSaveBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PresetComboBox);
            this.Controls.Add(this.PresetShareButton);
            this.Controls.Add(this.PresetLoadButton);
            this.Controls.Add(this.MasterDeckBox);
            this.Controls.Add(this.Deck4TitleBox);
            this.Controls.Add(this.Deck2TitleBox);
            this.Controls.Add(this.Deck4ArtistBox);
            this.Controls.Add(this.Deck2ArtistBox);
            this.Controls.Add(this.Deck3TitleBox);
            this.Controls.Add(this.Deck1TitleBox);
            this.Controls.Add(this.Deck3ArtistBox);
            this.Controls.Add(this.Deck1ArtistBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.label7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Rekordbox Reader Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button PresetShareButton;
        private System.Windows.Forms.Button PresetLoadButton;
        private System.Windows.Forms.TextBox MasterDeckBox;
        private System.Windows.Forms.TextBox Deck4TitleBox;
        private System.Windows.Forms.TextBox Deck2TitleBox;
        private System.Windows.Forms.TextBox Deck4ArtistBox;
        private System.Windows.Forms.TextBox Deck2ArtistBox;
        private System.Windows.Forms.TextBox Deck3TitleBox;
        private System.Windows.Forms.TextBox Deck1TitleBox;
        private System.Windows.Forms.TextBox Deck3ArtistBox;
        private System.Windows.Forms.TextBox Deck1ArtistBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox PortBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox PresetComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button PresetSaveBtn;
        private System.Windows.Forms.ToolTip ttHelp;
        private System.Windows.Forms.Label StatusMessage;
    }
}