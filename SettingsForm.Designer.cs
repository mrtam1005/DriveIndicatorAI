namespace DriveIndicatorAI
{
    partial class SettingsForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            labelSettings = new Label();
            chkStartup = new CheckBox();
            gbDisplayDriveSelection = new GroupBox();
            chkDriveZ = new CheckBox();
            chkDriveY = new CheckBox();
            chkDriveX = new CheckBox();
            chkDriveW = new CheckBox();
            chkDriveV = new CheckBox();
            chkDriveU = new CheckBox();
            chkDriveT = new CheckBox();
            chkDriveS = new CheckBox();
            chkDriveR = new CheckBox();
            chkDriveQ = new CheckBox();
            chkDriveP = new CheckBox();
            chkDriveO = new CheckBox();
            chkDriveN = new CheckBox();
            chkDriveM = new CheckBox();
            chkDriveL = new CheckBox();
            chkDriveK = new CheckBox();
            chkDriveJ = new CheckBox();
            chkDriveI = new CheckBox();
            chkDriveH = new CheckBox();
            chkDriveG = new CheckBox();
            chkDriveF = new CheckBox();
            chkDriveE = new CheckBox();
            chkDriveD = new CheckBox();
            chkDriveC = new CheckBox();
            chkDriveB = new CheckBox();
            chkDriveA = new CheckBox();
            gbDriveLettersColor = new GroupBox();
            BtnColor = new Button();
            panelColorPreview = new Panel();
            gbDisplayInterval = new GroupBox();
            labelIntervalLimit = new Label();
            numDisplayInterval = new NumericUpDown();
            gbIconSample = new GroupBox();
            picSampleBoth = new PictureBox();
            picSampleWrite = new PictureBox();
            picSampleRead = new PictureBox();
            picSampleNone = new PictureBox();
            gbIconImageFolder = new GroupBox();
            BtnBrowse = new Button();
            txtFolderPath = new TextBox();
            gbLog = new GroupBox();
            BtnLogClear = new Button();
            BtnOpenLogFolder = new Button();
            chkEnableLog = new CheckBox();
            gbLanguage = new GroupBox();
            ComboLanguageEn = new ComboBox();
            labelEnglishName = new Label();
            ComboLanguage = new ComboBox();
            BtnCancel = new Button();
            BtnSave = new Button();
            BtnVersionInfo = new Button();
            gbDisplayDriveSelection.SuspendLayout();
            gbDriveLettersColor.SuspendLayout();
            gbDisplayInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDisplayInterval).BeginInit();
            gbIconSample.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSampleBoth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSampleWrite).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSampleRead).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSampleNone).BeginInit();
            gbIconImageFolder.SuspendLayout();
            gbLog.SuspendLayout();
            gbLanguage.SuspendLayout();
            SuspendLayout();
            // 
            // labelSettings
            // 
            labelSettings.AutoSize = true;
            labelSettings.Font = new Font("Yu Gothic UI", 15F);
            labelSettings.Location = new Point(18, 18);
            labelSettings.Name = "labelSettings";
            labelSettings.Size = new Size(78, 41);
            labelSettings.TabIndex = 0;
            labelSettings.Text = "設定";
            // 
            // chkStartup
            // 
            chkStartup.AutoSize = true;
            chkStartup.Font = new Font("Yu Gothic UI", 9F);
            chkStartup.Location = new Point(24, 72);
            chkStartup.Name = "chkStartup";
            chkStartup.Size = new Size(252, 29);
            chkStartup.TabIndex = 1;
            chkStartup.Text = "Windows起動時に自動実行";
            chkStartup.UseVisualStyleBackColor = true;
            // 
            // gbDisplayDriveSelection
            // 
            gbDisplayDriveSelection.Controls.Add(chkDriveX);
            gbDisplayDriveSelection.Controls.Add(chkDriveW);
            gbDisplayDriveSelection.Controls.Add(chkDriveV);
            gbDisplayDriveSelection.Controls.Add(chkDriveU);
            gbDisplayDriveSelection.Controls.Add(chkDriveZ);
            gbDisplayDriveSelection.Controls.Add(chkDriveT);
            gbDisplayDriveSelection.Controls.Add(chkDriveY);
            gbDisplayDriveSelection.Controls.Add(chkDriveS);
            gbDisplayDriveSelection.Controls.Add(chkDriveR);
            gbDisplayDriveSelection.Controls.Add(chkDriveQ);
            gbDisplayDriveSelection.Controls.Add(chkDriveP);
            gbDisplayDriveSelection.Controls.Add(chkDriveO);
            gbDisplayDriveSelection.Controls.Add(chkDriveN);
            gbDisplayDriveSelection.Controls.Add(chkDriveM);
            gbDisplayDriveSelection.Controls.Add(chkDriveL);
            gbDisplayDriveSelection.Controls.Add(chkDriveK);
            gbDisplayDriveSelection.Controls.Add(chkDriveJ);
            gbDisplayDriveSelection.Controls.Add(chkDriveI);
            gbDisplayDriveSelection.Controls.Add(chkDriveH);
            gbDisplayDriveSelection.Controls.Add(chkDriveG);
            gbDisplayDriveSelection.Controls.Add(chkDriveF);
            gbDisplayDriveSelection.Controls.Add(chkDriveE);
            gbDisplayDriveSelection.Controls.Add(chkDriveD);
            gbDisplayDriveSelection.Controls.Add(chkDriveC);
            gbDisplayDriveSelection.Controls.Add(chkDriveB);
            gbDisplayDriveSelection.Controls.Add(chkDriveA);
            gbDisplayDriveSelection.Location = new Point(23, 108);
            gbDisplayDriveSelection.Name = "gbDisplayDriveSelection";
            gbDisplayDriveSelection.Size = new Size(444, 342);
            gbDisplayDriveSelection.TabIndex = 2;
            gbDisplayDriveSelection.TabStop = false;
            gbDisplayDriveSelection.Text = "表示ドライブ選択";
            // 
            // chkDriveZ
            // 
            chkDriveZ.AutoSize = true;
            chkDriveZ.Location = new Point(90, 288);
            chkDriveZ.Name = "chkDriveZ";
            chkDriveZ.Size = new Size(48, 29);
            chkDriveZ.TabIndex = 25;
            chkDriveZ.Text = "Z";
            chkDriveZ.UseVisualStyleBackColor = true;
            // 
            // chkDriveY
            // 
            chkDriveY.AutoSize = true;
            chkDriveY.Location = new Point(24, 288);
            chkDriveY.Name = "chkDriveY";
            chkDriveY.Size = new Size(48, 29);
            chkDriveY.TabIndex = 24;
            chkDriveY.Text = "Y";
            chkDriveY.UseVisualStyleBackColor = true;
            // 
            // chkDriveX
            // 
            chkDriveX.AutoSize = true;
            chkDriveX.Location = new Point(354, 222);
            chkDriveX.Name = "chkDriveX";
            chkDriveX.Size = new Size(49, 29);
            chkDriveX.TabIndex = 23;
            chkDriveX.Text = "X";
            chkDriveX.UseVisualStyleBackColor = true;
            // 
            // chkDriveW
            // 
            chkDriveW.AutoSize = true;
            chkDriveW.Location = new Point(288, 222);
            chkDriveW.Name = "chkDriveW";
            chkDriveW.Size = new Size(55, 29);
            chkDriveW.TabIndex = 22;
            chkDriveW.Text = "W";
            chkDriveW.UseVisualStyleBackColor = true;
            // 
            // chkDriveV
            // 
            chkDriveV.AutoSize = true;
            chkDriveV.Location = new Point(222, 222);
            chkDriveV.Name = "chkDriveV";
            chkDriveV.Size = new Size(49, 29);
            chkDriveV.TabIndex = 21;
            chkDriveV.Text = "V";
            chkDriveV.UseVisualStyleBackColor = true;
            // 
            // chkDriveU
            // 
            chkDriveU.AutoSize = true;
            chkDriveU.Location = new Point(156, 222);
            chkDriveU.Name = "chkDriveU";
            chkDriveU.Size = new Size(50, 29);
            chkDriveU.TabIndex = 20;
            chkDriveU.Text = "U";
            chkDriveU.UseVisualStyleBackColor = true;
            // 
            // chkDriveT
            // 
            chkDriveT.AutoSize = true;
            chkDriveT.Location = new Point(90, 222);
            chkDriveT.Name = "chkDriveT";
            chkDriveT.Size = new Size(47, 29);
            chkDriveT.TabIndex = 19;
            chkDriveT.Text = "T";
            chkDriveT.UseVisualStyleBackColor = true;
            // 
            // chkDriveS
            // 
            chkDriveS.AutoSize = true;
            chkDriveS.Location = new Point(24, 222);
            chkDriveS.Name = "chkDriveS";
            chkDriveS.Size = new Size(48, 29);
            chkDriveS.TabIndex = 18;
            chkDriveS.Text = "S";
            chkDriveS.UseVisualStyleBackColor = true;
            // 
            // chkDriveR
            // 
            chkDriveR.AutoSize = true;
            chkDriveR.Location = new Point(354, 162);
            chkDriveR.Name = "chkDriveR";
            chkDriveR.Size = new Size(49, 29);
            chkDriveR.TabIndex = 17;
            chkDriveR.Text = "R";
            chkDriveR.UseVisualStyleBackColor = true;
            // 
            // chkDriveQ
            // 
            chkDriveQ.AutoSize = true;
            chkDriveQ.Location = new Point(288, 162);
            chkDriveQ.Name = "chkDriveQ";
            chkDriveQ.Size = new Size(52, 29);
            chkDriveQ.TabIndex = 16;
            chkDriveQ.Text = "Q";
            chkDriveQ.UseVisualStyleBackColor = true;
            // 
            // chkDriveP
            // 
            chkDriveP.AutoSize = true;
            chkDriveP.Location = new Point(222, 162);
            chkDriveP.Name = "chkDriveP";
            chkDriveP.Size = new Size(48, 29);
            chkDriveP.TabIndex = 15;
            chkDriveP.Text = "P";
            chkDriveP.UseVisualStyleBackColor = true;
            // 
            // chkDriveO
            // 
            chkDriveO.AutoSize = true;
            chkDriveO.Location = new Point(156, 162);
            chkDriveO.Name = "chkDriveO";
            chkDriveO.Size = new Size(52, 29);
            chkDriveO.TabIndex = 14;
            chkDriveO.Text = "O";
            chkDriveO.UseVisualStyleBackColor = true;
            // 
            // chkDriveN
            // 
            chkDriveN.AutoSize = true;
            chkDriveN.Location = new Point(90, 162);
            chkDriveN.Name = "chkDriveN";
            chkDriveN.Size = new Size(51, 29);
            chkDriveN.TabIndex = 13;
            chkDriveN.Text = "N";
            chkDriveN.UseVisualStyleBackColor = true;
            // 
            // chkDriveM
            // 
            chkDriveM.AutoSize = true;
            chkDriveM.Location = new Point(24, 162);
            chkDriveM.Name = "chkDriveM";
            chkDriveM.Size = new Size(54, 29);
            chkDriveM.TabIndex = 12;
            chkDriveM.Text = "M";
            chkDriveM.UseVisualStyleBackColor = true;
            // 
            // chkDriveL
            // 
            chkDriveL.AutoSize = true;
            chkDriveL.Location = new Point(354, 102);
            chkDriveL.Name = "chkDriveL";
            chkDriveL.Size = new Size(46, 29);
            chkDriveL.TabIndex = 11;
            chkDriveL.Text = "L";
            chkDriveL.UseVisualStyleBackColor = true;
            // 
            // chkDriveK
            // 
            chkDriveK.AutoSize = true;
            chkDriveK.Location = new Point(288, 102);
            chkDriveK.Name = "chkDriveK";
            chkDriveK.Size = new Size(48, 29);
            chkDriveK.TabIndex = 10;
            chkDriveK.Text = "K";
            chkDriveK.UseVisualStyleBackColor = true;
            // 
            // chkDriveJ
            // 
            chkDriveJ.AutoSize = true;
            chkDriveJ.Location = new Point(222, 102);
            chkDriveJ.Name = "chkDriveJ";
            chkDriveJ.Size = new Size(44, 29);
            chkDriveJ.TabIndex = 9;
            chkDriveJ.Text = "J";
            chkDriveJ.UseVisualStyleBackColor = true;
            // 
            // chkDriveI
            // 
            chkDriveI.AutoSize = true;
            chkDriveI.Location = new Point(156, 102);
            chkDriveI.Name = "chkDriveI";
            chkDriveI.Size = new Size(43, 29);
            chkDriveI.TabIndex = 8;
            chkDriveI.Text = "I";
            chkDriveI.UseVisualStyleBackColor = true;
            // 
            // chkDriveH
            // 
            chkDriveH.AutoSize = true;
            chkDriveH.Location = new Point(90, 102);
            chkDriveH.Name = "chkDriveH";
            chkDriveH.Size = new Size(51, 29);
            chkDriveH.TabIndex = 7;
            chkDriveH.Text = "H";
            chkDriveH.UseVisualStyleBackColor = true;
            // 
            // chkDriveG
            // 
            chkDriveG.AutoSize = true;
            chkDriveG.Location = new Point(24, 102);
            chkDriveG.Name = "chkDriveG";
            chkDriveG.Size = new Size(50, 29);
            chkDriveG.TabIndex = 6;
            chkDriveG.Text = "G";
            chkDriveG.UseVisualStyleBackColor = true;
            // 
            // chkDriveF
            // 
            chkDriveF.AutoSize = true;
            chkDriveF.Location = new Point(354, 42);
            chkDriveF.Name = "chkDriveF";
            chkDriveF.Size = new Size(47, 29);
            chkDriveF.TabIndex = 5;
            chkDriveF.Text = "F";
            chkDriveF.UseVisualStyleBackColor = true;
            // 
            // chkDriveE
            // 
            chkDriveE.AutoSize = true;
            chkDriveE.Location = new Point(288, 42);
            chkDriveE.Name = "chkDriveE";
            chkDriveE.Size = new Size(47, 29);
            chkDriveE.TabIndex = 4;
            chkDriveE.Text = "E";
            chkDriveE.UseVisualStyleBackColor = true;
            // 
            // chkDriveD
            // 
            chkDriveD.AutoSize = true;
            chkDriveD.Location = new Point(222, 42);
            chkDriveD.Name = "chkDriveD";
            chkDriveD.Size = new Size(51, 29);
            chkDriveD.TabIndex = 3;
            chkDriveD.Text = "D";
            chkDriveD.UseVisualStyleBackColor = true;
            // 
            // chkDriveC
            // 
            chkDriveC.AutoSize = true;
            chkDriveC.Location = new Point(156, 42);
            chkDriveC.Name = "chkDriveC";
            chkDriveC.Size = new Size(49, 29);
            chkDriveC.TabIndex = 2;
            chkDriveC.Text = "C";
            chkDriveC.UseVisualStyleBackColor = true;
            // 
            // chkDriveB
            // 
            chkDriveB.AutoSize = true;
            chkDriveB.Location = new Point(90, 42);
            chkDriveB.Name = "chkDriveB";
            chkDriveB.Size = new Size(48, 29);
            chkDriveB.TabIndex = 1;
            chkDriveB.Text = "B";
            chkDriveB.UseVisualStyleBackColor = true;
            // 
            // chkDriveA
            // 
            chkDriveA.AutoSize = true;
            chkDriveA.Location = new Point(24, 42);
            chkDriveA.Name = "chkDriveA";
            chkDriveA.Size = new Size(50, 29);
            chkDriveA.TabIndex = 0;
            chkDriveA.Text = "A";
            chkDriveA.UseVisualStyleBackColor = true;
            // 
            // gbDriveLettersColor
            // 
            gbDriveLettersColor.Controls.Add(BtnColor);
            gbDriveLettersColor.Controls.Add(panelColorPreview);
            gbDriveLettersColor.Location = new Point(480, 228);
            gbDriveLettersColor.Name = "gbDriveLettersColor";
            gbDriveLettersColor.Size = new Size(390, 102);
            gbDriveLettersColor.TabIndex = 3;
            gbDriveLettersColor.TabStop = false;
            gbDriveLettersColor.Text = "ドライブ文字色";
            // 
            // BtnColor
            // 
            BtnColor.Location = new Point(186, 36);
            BtnColor.Name = "BtnColor";
            BtnColor.Size = new Size(180, 50);
            BtnColor.TabIndex = 1;
            BtnColor.Text = "色選択";
            BtnColor.UseVisualStyleBackColor = true;
            BtnColor.Click += BtnColor_Click;
            // 
            // panelColorPreview
            // 
            panelColorPreview.BackColor = Color.Silver;
            panelColorPreview.Location = new Point(24, 36);
            panelColorPreview.Name = "panelColorPreview";
            panelColorPreview.Size = new Size(150, 48);
            panelColorPreview.TabIndex = 0;
            // 
            // gbDisplayInterval
            // 
            gbDisplayInterval.Controls.Add(labelIntervalLimit);
            gbDisplayInterval.Controls.Add(numDisplayInterval);
            gbDisplayInterval.Location = new Point(480, 348);
            gbDisplayInterval.Name = "gbDisplayInterval";
            gbDisplayInterval.Size = new Size(390, 102);
            gbDisplayInterval.TabIndex = 4;
            gbDisplayInterval.TabStop = false;
            gbDisplayInterval.Text = "表示間隔";
            // 
            // labelIntervalLimit
            // 
            labelIntervalLimit.AutoSize = true;
            labelIntervalLimit.Location = new Point(138, 46);
            labelIntervalLimit.Name = "labelIntervalLimit";
            labelIntervalLimit.Size = new Size(106, 25);
            labelIntervalLimit.TabIndex = 1;
            labelIntervalLimit.Text = "（1～250）";
            // 
            // numDisplayInterval
            // 
            numDisplayInterval.Location = new Point(24, 42);
            numDisplayInterval.Maximum = new decimal(new int[] { 250, 0, 0, 0 });
            numDisplayInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDisplayInterval.Name = "numDisplayInterval";
            numDisplayInterval.Size = new Size(102, 31);
            numDisplayInterval.TabIndex = 0;
            numDisplayInterval.TextAlign = HorizontalAlignment.Right;
            numDisplayInterval.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // gbIconSample
            // 
            gbIconSample.Controls.Add(picSampleBoth);
            gbIconSample.Controls.Add(picSampleWrite);
            gbIconSample.Controls.Add(picSampleRead);
            gbIconSample.Controls.Add(picSampleNone);
            gbIconSample.Location = new Point(480, 108);
            gbIconSample.Name = "gbIconSample";
            gbIconSample.Size = new Size(390, 102);
            gbIconSample.TabIndex = 5;
            gbIconSample.TabStop = false;
            gbIconSample.Text = "アイコンサンプル";
            // 
            // picSampleBoth
            // 
            picSampleBoth.Location = new Point(342, 42);
            picSampleBoth.Name = "picSampleBoth";
            picSampleBoth.Size = new Size(16, 32);
            picSampleBoth.TabIndex = 3;
            picSampleBoth.TabStop = false;
            // 
            // picSampleWrite
            // 
            picSampleWrite.Location = new Point(240, 42);
            picSampleWrite.Name = "picSampleWrite";
            picSampleWrite.Size = new Size(16, 32);
            picSampleWrite.TabIndex = 2;
            picSampleWrite.TabStop = false;
            // 
            // picSampleRead
            // 
            picSampleRead.Location = new Point(138, 42);
            picSampleRead.Name = "picSampleRead";
            picSampleRead.Size = new Size(16, 32);
            picSampleRead.TabIndex = 1;
            picSampleRead.TabStop = false;
            // 
            // picSampleNone
            // 
            picSampleNone.Location = new Point(36, 42);
            picSampleNone.Name = "picSampleNone";
            picSampleNone.Size = new Size(16, 32);
            picSampleNone.TabIndex = 0;
            picSampleNone.TabStop = false;
            // 
            // gbIconImageFolder
            // 
            gbIconImageFolder.Controls.Add(BtnBrowse);
            gbIconImageFolder.Controls.Add(txtFolderPath);
            gbIconImageFolder.Location = new Point(24, 462);
            gbIconImageFolder.Name = "gbIconImageFolder";
            gbIconImageFolder.Size = new Size(846, 102);
            gbIconImageFolder.TabIndex = 6;
            gbIconImageFolder.TabStop = false;
            gbIconImageFolder.Text = "アイコン画像フォルダー";
            // 
            // BtnBrowse
            // 
            BtnBrowse.Location = new Point(640, 36);
            BtnBrowse.Name = "BtnBrowse";
            BtnBrowse.Size = new Size(180, 50);
            BtnBrowse.TabIndex = 1;
            BtnBrowse.Text = "フォルダー選択";
            BtnBrowse.UseVisualStyleBackColor = true;
            BtnBrowse.Click += BtnBrowse_Click;
            // 
            // txtFolderPath
            // 
            txtFolderPath.Location = new Point(24, 48);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.Size = new Size(604, 31);
            txtFolderPath.TabIndex = 0;
            // 
            // gbLog
            // 
            gbLog.Controls.Add(BtnLogClear);
            gbLog.Controls.Add(BtnOpenLogFolder);
            gbLog.Controls.Add(chkEnableLog);
            gbLog.Location = new Point(24, 574);
            gbLog.Name = "gbLog";
            gbLog.Size = new Size(552, 209);
            gbLog.TabIndex = 7;
            gbLog.TabStop = false;
            gbLog.Text = "ログ";
            // 
            // BtnLogClear
            // 
            BtnLogClear.Location = new Point(24, 140);
            BtnLogClear.Name = "BtnLogClear";
            BtnLogClear.Size = new Size(390, 50);
            BtnLogClear.TabIndex = 2;
            BtnLogClear.Text = "logクリア";
            BtnLogClear.UseVisualStyleBackColor = true;
            BtnLogClear.Click += BtnLogClear_Click;
            // 
            // BtnOpenLogFolder
            // 
            BtnOpenLogFolder.Location = new Point(24, 78);
            BtnOpenLogFolder.Name = "BtnOpenLogFolder";
            BtnOpenLogFolder.Size = new Size(390, 50);
            BtnOpenLogFolder.TabIndex = 1;
            BtnOpenLogFolder.Text = "log保存フォルダーを開く";
            BtnOpenLogFolder.UseVisualStyleBackColor = true;
            BtnOpenLogFolder.Click += BtnOpenLogFolder_Click;
            // 
            // chkEnableLog
            // 
            chkEnableLog.AutoSize = true;
            chkEnableLog.Location = new Point(24, 36);
            chkEnableLog.Name = "chkEnableLog";
            chkEnableLog.Size = new Size(143, 29);
            chkEnableLog.TabIndex = 0;
            chkEnableLog.Text = "logを記録する";
            chkEnableLog.UseVisualStyleBackColor = true;
            // 
            // gbLanguage
            // 
            gbLanguage.Controls.Add(ComboLanguageEn);
            gbLanguage.Controls.Add(labelEnglishName);
            gbLanguage.Controls.Add(ComboLanguage);
            gbLanguage.Location = new Point(588, 574);
            gbLanguage.Name = "gbLanguage";
            gbLanguage.Size = new Size(282, 209);
            gbLanguage.TabIndex = 8;
            gbLanguage.TabStop = false;
            gbLanguage.Text = "Language";
            // 
            // ComboLanguageEn
            // 
            ComboLanguageEn.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboLanguageEn.FormattingEnabled = true;
            ComboLanguageEn.Location = new Point(24, 120);
            ComboLanguageEn.Name = "ComboLanguageEn";
            ComboLanguageEn.Size = new Size(228, 33);
            ComboLanguageEn.TabIndex = 2;
            ComboLanguageEn.SelectedIndexChanged += ComboLanguageEn_SelectedIndexChanged;
            // 
            // labelEnglishName
            // 
            labelEnglishName.AutoSize = true;
            labelEnglishName.Location = new Point(6, 90);
            labelEnglishName.Name = "labelEnglishName";
            labelEnglishName.Size = new Size(117, 25);
            labelEnglishName.TabIndex = 1;
            labelEnglishName.Text = "English name";
            // 
            // ComboLanguage
            // 
            ComboLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboLanguage.FormattingEnabled = true;
            ComboLanguage.Location = new Point(24, 32);
            ComboLanguage.Name = "ComboLanguage";
            ComboLanguage.Size = new Size(228, 33);
            ComboLanguage.TabIndex = 0;
            ComboLanguage.SelectedIndexChanged += ComboLanguage_SelectedIndexChanged;
            // 
            // BtnVersionInfo
            // 
            BtnVersionInfo.Location = new Point(606, 18);
            BtnVersionInfo.Name = "BtnVersionInfo";
            BtnVersionInfo.Size = new Size(264, 50);
            BtnVersionInfo.TabIndex = 11;
            BtnVersionInfo.Text = "バージョン情報";
            BtnVersionInfo.UseVisualStyleBackColor = true;
            BtnVersionInfo.Click += BtnVersionInfo_Click;
            // 
            // BtnSave
            // 
            BtnSave.Location = new Point(690, 804);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(180, 50);
            BtnSave.TabIndex = 10;
            BtnSave.Text = "保存";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(500, 804);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(180, 50);
            BtnCancel.TabIndex = 9;
            BtnCancel.Text = "キャンセル";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(896, 874);
            Controls.Add(BtnVersionInfo);
            Controls.Add(BtnSave);
            Controls.Add(BtnCancel);
            Controls.Add(gbLanguage);
            Controls.Add(gbLog);
            Controls.Add(gbIconImageFolder);
            Controls.Add(gbIconSample);
            Controls.Add(gbDisplayInterval);
            Controls.Add(gbDriveLettersColor);
            Controls.Add(gbDisplayDriveSelection);
            Controls.Add(chkStartup);
            Controls.Add(labelSettings);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Drive Indicator AI";
            gbDisplayDriveSelection.ResumeLayout(false);
            gbDisplayDriveSelection.PerformLayout();
            gbDriveLettersColor.ResumeLayout(false);
            gbDisplayInterval.ResumeLayout(false);
            gbDisplayInterval.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDisplayInterval).EndInit();
            gbIconSample.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picSampleBoth).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSampleWrite).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSampleRead).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSampleNone).EndInit();
            gbIconImageFolder.ResumeLayout(false);
            gbIconImageFolder.PerformLayout();
            gbLog.ResumeLayout(false);
            gbLog.PerformLayout();
            gbLanguage.ResumeLayout(false);
            gbLanguage.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelSettings;
        private CheckBox chkStartup;
        private GroupBox gbDisplayDriveSelection;
        private CheckBox chkDriveZ;
        private CheckBox chkDriveY;
        private CheckBox chkDriveX;
        private CheckBox chkDriveW;
        private CheckBox chkDriveV;
        private CheckBox chkDriveU;
        private CheckBox chkDriveT;
        private CheckBox chkDriveS;
        private CheckBox chkDriveR;
        private CheckBox chkDriveQ;
        private CheckBox chkDriveP;
        private CheckBox chkDriveO;
        private CheckBox chkDriveN;
        private CheckBox chkDriveM;
        private CheckBox chkDriveL;
        private CheckBox chkDriveK;
        private CheckBox chkDriveJ;
        private CheckBox chkDriveI;
        private CheckBox chkDriveH;
        private CheckBox chkDriveG;
        private CheckBox chkDriveF;
        private CheckBox chkDriveE;
        private CheckBox chkDriveD;
        private CheckBox chkDriveC;
        private CheckBox chkDriveB;
        private CheckBox chkDriveA;
        private GroupBox gbDriveLettersColor;
        private Button BtnColor;
        private Panel panelColorPreview;
        private GroupBox gbDisplayInterval;
        private Label labelIntervalLimit;
        private NumericUpDown numDisplayInterval;
        private GroupBox gbIconSample;
        private PictureBox picSampleBoth;
        private PictureBox picSampleWrite;
        private PictureBox picSampleRead;
        private PictureBox picSampleNone;
        private GroupBox gbIconImageFolder;
        private Button BtnBrowse;
        private TextBox txtFolderPath;
        private GroupBox gbLog;
        private Button BtnLogClear;
        private Button BtnOpenLogFolder;
        private CheckBox chkEnableLog;
        private GroupBox gbLanguage;
        private ComboBox ComboLanguageEn;
        private Label labelEnglishName;
        private ComboBox ComboLanguage;
        private Button BtnSave;
        private Button BtnCancel;
        private Button BtnVersionInfo;
    }
}
