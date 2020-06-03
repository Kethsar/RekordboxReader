namespace RekordboxReader
{
    partial class MainForm
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
            this.Deck2TitleLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Deck2ArtistLabel = new System.Windows.Forms.Label();
            this.MasterDeckInfo = new System.Windows.Forms.Label();
            this.Deck1ArtistLabel = new System.Windows.Forms.Label();
            this.Deck1TitleLabel = new System.Windows.Forms.Label();
            this.StatusMessage = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.settingsBtn = new System.Windows.Forms.Button();
            this.Deck3ArtistLabel = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.Deck4ArtistLabel = new System.Windows.Forms.Label();
            this.Deck3TitleLabel = new System.Windows.Forms.Label();
            this.Deck4TitleLabel = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Deck2TitleLabel
            // 
            this.Deck2TitleLabel.AutoSize = true;
            this.Deck2TitleLabel.Location = new System.Drawing.Point(89, 93);
            this.Deck2TitleLabel.Name = "Deck2TitleLabel";
            this.Deck2TitleLabel.Size = new System.Drawing.Size(100, 13);
            this.Deck2TitleLabel.TabIndex = 11;
            this.Deck2TitleLabel.Text = "Nothing read so far.";
            this.Deck2TitleLabel.Resize += new System.EventHandler(this.Label_Resize);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Deck 1 Artist:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Deck 2 Artist:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Master Deck:";
            // 
            // Deck2ArtistLabel
            // 
            this.Deck2ArtistLabel.AutoSize = true;
            this.Deck2ArtistLabel.Location = new System.Drawing.Point(89, 74);
            this.Deck2ArtistLabel.Name = "Deck2ArtistLabel";
            this.Deck2ArtistLabel.Size = new System.Drawing.Size(100, 13);
            this.Deck2ArtistLabel.TabIndex = 5;
            this.Deck2ArtistLabel.Text = "Nothing read so far.";
            this.Deck2ArtistLabel.Resize += new System.EventHandler(this.Label_Resize);
            // 
            // MasterDeckInfo
            // 
            this.MasterDeckInfo.AutoSize = true;
            this.MasterDeckInfo.Location = new System.Drawing.Point(89, 204);
            this.MasterDeckInfo.Name = "MasterDeckInfo";
            this.MasterDeckInfo.Size = new System.Drawing.Size(0, 13);
            this.MasterDeckInfo.TabIndex = 6;
            // 
            // Deck1ArtistLabel
            // 
            this.Deck1ArtistLabel.AutoSize = true;
            this.Deck1ArtistLabel.Location = new System.Drawing.Point(89, 30);
            this.Deck1ArtistLabel.Name = "Deck1ArtistLabel";
            this.Deck1ArtistLabel.Size = new System.Drawing.Size(100, 13);
            this.Deck1ArtistLabel.TabIndex = 1;
            this.Deck1ArtistLabel.Text = "Nothing read so far.";
            this.Deck1ArtistLabel.Resize += new System.EventHandler(this.Label_Resize);
            // 
            // Deck1TitleLabel
            // 
            this.Deck1TitleLabel.AutoSize = true;
            this.Deck1TitleLabel.Location = new System.Drawing.Point(89, 49);
            this.Deck1TitleLabel.Name = "Deck1TitleLabel";
            this.Deck1TitleLabel.Size = new System.Drawing.Size(100, 13);
            this.Deck1TitleLabel.TabIndex = 10;
            this.Deck1TitleLabel.Text = "Nothing read so far.";
            this.Deck1TitleLabel.Resize += new System.EventHandler(this.Label_Resize);
            // 
            // StatusMessage
            // 
            this.StatusMessage.AutoSize = true;
            this.StatusMessage.Location = new System.Drawing.Point(12, 9);
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.Size = new System.Drawing.Size(79, 13);
            this.StatusMessage.TabIndex = 13;
            this.StatusMessage.Text = "Status: Starting";
            this.StatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Deck 1 Title:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Deck 2 Title:";
            // 
            // settingsBtn
            // 
            this.settingsBtn.Location = new System.Drawing.Point(80, 228);
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.Size = new System.Drawing.Size(93, 23);
            this.settingsBtn.TabIndex = 28;
            this.settingsBtn.Text = "Show Settings";
            this.settingsBtn.UseVisualStyleBackColor = true;
            this.settingsBtn.Click += new System.EventHandler(this.SettingsBtn_Click);
            // 
            // Deck3ArtistLabel
            // 
            this.Deck3ArtistLabel.AutoSize = true;
            this.Deck3ArtistLabel.Location = new System.Drawing.Point(89, 118);
            this.Deck3ArtistLabel.Name = "Deck3ArtistLabel";
            this.Deck3ArtistLabel.Size = new System.Drawing.Size(100, 13);
            this.Deck3ArtistLabel.TabIndex = 1;
            this.Deck3ArtistLabel.Text = "Nothing read so far.";
            this.Deck3ArtistLabel.Resize += new System.EventHandler(this.Label_Resize);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 118);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Deck 3 Artist:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 161);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(71, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "Deck 4 Artist:";
            // 
            // Deck4ArtistLabel
            // 
            this.Deck4ArtistLabel.AutoSize = true;
            this.Deck4ArtistLabel.Location = new System.Drawing.Point(89, 161);
            this.Deck4ArtistLabel.Name = "Deck4ArtistLabel";
            this.Deck4ArtistLabel.Size = new System.Drawing.Size(100, 13);
            this.Deck4ArtistLabel.TabIndex = 5;
            this.Deck4ArtistLabel.Text = "Nothing read so far.";
            this.Deck4ArtistLabel.Resize += new System.EventHandler(this.Label_Resize);
            // 
            // Deck3TitleLabel
            // 
            this.Deck3TitleLabel.AutoSize = true;
            this.Deck3TitleLabel.Location = new System.Drawing.Point(89, 137);
            this.Deck3TitleLabel.Name = "Deck3TitleLabel";
            this.Deck3TitleLabel.Size = new System.Drawing.Size(100, 13);
            this.Deck3TitleLabel.TabIndex = 10;
            this.Deck3TitleLabel.Text = "Nothing read so far.";
            this.Deck3TitleLabel.Resize += new System.EventHandler(this.Label_Resize);
            // 
            // Deck4TitleLabel
            // 
            this.Deck4TitleLabel.AutoSize = true;
            this.Deck4TitleLabel.Location = new System.Drawing.Point(89, 181);
            this.Deck4TitleLabel.Name = "Deck4TitleLabel";
            this.Deck4TitleLabel.Size = new System.Drawing.Size(100, 13);
            this.Deck4TitleLabel.TabIndex = 11;
            this.Deck4TitleLabel.Text = "Nothing read so far.";
            this.Deck4TitleLabel.Resize += new System.EventHandler(this.Label_Resize);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(12, 137);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(68, 13);
            this.label20.TabIndex = 14;
            this.label20.Text = "Deck 3 Title:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(12, 181);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(68, 13);
            this.label21.TabIndex = 15;
            this.label21.Text = "Deck 4 Title:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 260);
            this.Controls.Add(this.settingsBtn);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.StatusMessage);
            this.Controls.Add(this.Deck4TitleLabel);
            this.Controls.Add(this.Deck2TitleLabel);
            this.Controls.Add(this.Deck3TitleLabel);
            this.Controls.Add(this.Deck1TitleLabel);
            this.Controls.Add(this.MasterDeckInfo);
            this.Controls.Add(this.Deck4ArtistLabel);
            this.Controls.Add(this.Deck2ArtistLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.Deck3ArtistLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Deck1ArtistLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion
        private System.Windows.Forms.Label Deck2TitleLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Deck2ArtistLabel;
        private System.Windows.Forms.Label MasterDeckInfo;
        private System.Windows.Forms.Label Deck1ArtistLabel;
        private System.Windows.Forms.Label Deck1TitleLabel;
        public System.Windows.Forms.Label StatusMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button settingsBtn;
        private System.Windows.Forms.Label Deck3ArtistLabel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label Deck4ArtistLabel;
        private System.Windows.Forms.Label Deck3TitleLabel;
        private System.Windows.Forms.Label Deck4TitleLabel;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
    }
}

