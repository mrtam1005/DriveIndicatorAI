namespace DriveIndicatorAI
{
    partial class LicenseDialog
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
            textBoxLicense = new TextBox();
            BtnLicenseOk = new Button();
            SuspendLayout();
            // 
            // textBoxLicense
            // 
            textBoxLicense.Dock = DockStyle.Top;
            textBoxLicense.Location = new Point(0, 0);
            textBoxLicense.Multiline = true;
            textBoxLicense.Name = "textBoxLicense";
            textBoxLicense.ReadOnly = true;
            textBoxLicense.ScrollBars = ScrollBars.Vertical;
            textBoxLicense.Size = new Size(800, 378);
            textBoxLicense.TabIndex = 0;
            textBoxLicense.TabStop = false;
            // 
            // BtnLicenseOk
            // 
            BtnLicenseOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnLicenseOk.Location = new Point(624, 396);
            BtnLicenseOk.Name = "BtnLicenseOk";
            BtnLicenseOk.Size = new Size(160, 40);
            BtnLicenseOk.TabIndex = 1;
            BtnLicenseOk.Text = "OK";
            BtnLicenseOk.UseVisualStyleBackColor = true;
            BtnLicenseOk.Click += BtnLicenseOk_Click;
            // 
            // LicenseDialog
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(BtnLicenseOk);
            Controls.Add(textBoxLicense);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LicenseDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "License";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxLicense;

        private Button BtnLicenseOk;
    }
}