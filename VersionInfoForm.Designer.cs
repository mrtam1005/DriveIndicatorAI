namespace DriveIndicatorAI
{
    partial class VersionInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VersionInfoForm));
            labelAppName = new Label();
            labelVersion = new Label();
            labelVersionNo = new Label();
            labelCopyright = new Label();
            labelDevelopedBy = new Label();
            labelSupport = new Label();
            LinkLabelGithubLink = new LinkLabel();
            pictureBoxAppIcon256 = new PictureBox();
            BtnLicense = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAppIcon256).BeginInit();
            SuspendLayout();
            // 
            // labelAppName
            // 
            labelAppName.AutoSize = true;
            labelAppName.Location = new Point(294, 24);
            labelAppName.Name = "labelAppName";
            labelAppName.Size = new Size(301, 25);
            labelAppName.TabIndex = 0;
            labelAppName.Text = "Application Name: Drive Indicator AI";
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Location = new Point(336, 72);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(74, 25);
            labelVersion.TabIndex = 1;
            labelVersion.Text = "Version:";
            // 
            // labelVersionNo
            // 
            labelVersionNo.AutoSize = true;
            labelVersionNo.Location = new Point(414, 72);
            labelVersionNo.Name = "labelVersionNo";
            labelVersionNo.Size = new Size(135, 25);
            labelVersionNo.TabIndex = 2;
            labelVersionNo.Text = "VersionNumber";
            // 
            // labelCopyright
            // 
            labelCopyright.AutoSize = true;
            labelCopyright.Location = new Point(356, 126);
            labelCopyright.Name = "labelCopyright";
            labelCopyright.Size = new Size(166, 25);
            labelCopyright.TabIndex = 3;
            labelCopyright.Text = "©2025 mrtam1005";
            // 
            // labelDevelopedBy
            // 
            labelDevelopedBy.AutoSize = true;
            labelDevelopedBy.Location = new Point(333, 168);
            labelDevelopedBy.Name = "labelDevelopedBy";
            labelDevelopedBy.Size = new Size(221, 25);
            labelDevelopedBy.TabIndex = 4;
            labelDevelopedBy.Text = "Developed by mrtam1005";
            // 
            // labelSupport
            // 
            labelSupport.AutoSize = true;
            labelSupport.Location = new Point(36, 306);
            labelSupport.Name = "labelSupport";
            labelSupport.Size = new Size(147, 25);
            labelSupport.TabIndex = 6;
            labelSupport.Text = "Support  GitHub:";
            // 
            // LinkLabelGithubLink
            // 
            LinkLabelGithubLink.AutoSize = true;
            LinkLabelGithubLink.Location = new Point(186, 306);
            LinkLabelGithubLink.Name = "LinkLabelGithubLink";
            LinkLabelGithubLink.Size = new Size(397, 25);
            LinkLabelGithubLink.TabIndex = 7;
            LinkLabelGithubLink.TabStop = true;
            LinkLabelGithubLink.Text = "https://github.com/mrtam1005/DriveIndicatorAI";
            LinkLabelGithubLink.Click += LinkLabelGithubLink_Click;
            // 
            // pictureBoxAppIcon256
            // 
            pictureBoxAppIcon256.Image = Properties.Resources.AppIcon;
            pictureBoxAppIcon256.Location = new Point(24, 24);
            pictureBoxAppIcon256.Name = "pictureBoxAppIcon256";
            pictureBoxAppIcon256.Size = new Size(256, 256);
            pictureBoxAppIcon256.TabIndex = 8;
            pictureBoxAppIcon256.TabStop = false;
            // 
            // BtnLicense
            // 
            BtnLicense.Location = new Point(348, 222);
            BtnLicense.Name = "BtnLicense";
            BtnLicense.Size = new Size(180, 50);
            BtnLicense.TabIndex = 9;
            BtnLicense.Text = "License";
            BtnLicense.UseVisualStyleBackColor = true;
            BtnLicense.Click += BtnLicense_Click;
            // 
            // VersionInfoForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(608, 364);
            Controls.Add(BtnLicense);
            Controls.Add(pictureBoxAppIcon256);
            Controls.Add(LinkLabelGithubLink);
            Controls.Add(labelSupport);
            Controls.Add(labelDevelopedBy);
            Controls.Add(labelCopyright);
            Controls.Add(labelVersionNo);
            Controls.Add(labelVersion);
            Controls.Add(labelAppName);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "VersionInfoForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Version Infomation";
            ((System.ComponentModel.ISupportInitialize)pictureBoxAppIcon256).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelAppName;
        private Label labelVersion;
        private Label labelVersionNo;
        private Label labelCopyright;
        private Label labelDevelopedBy;
        private Label labelSupport;
        private LinkLabel LinkLabelGithubLink;
        private PictureBox pictureBoxAppIcon256;
        private Button BtnLicense;
    }
}