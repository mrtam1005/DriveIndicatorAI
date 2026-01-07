//-------------------------------------------------------------------
// SettingsForm.cs
// 設定フォーム
//-------------------------------------------------------------------

using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace DriveIndicatorAI
{
    /// <summary>
    /// 設定フォーム クラス
    /// </summary>
    public partial class SettingsForm : Form
    {
        /// <summary>
        /// A〜Z の CheckBox をまとめて扱うための配列 
        /// </summary>
        /// <return>CheckBox の配列 true:チェックあり、false:チェックなし</return> 
        private CheckBox[] driveBoxes = [];

        /// <summary>
        /// SettingsForm Class
        /// </summary>
        public SettingsForm()
        {
            LogHelper.LogWrite("SettingsForm-> SettingsForm() Start");
            // コンポーネントの初期化
            InitializeComponent();
            LogHelper.LogWrite("SettingsForm-> Component initialized.");

            // チェックボックス配列の初期化
            InitializeDriveBoxArray();
            LogHelper.LogWrite("SettingsForm-> Checkbox array initialized.");

            // 言語名リストの読み込み
            LoadLanguageList();
            LogHelper.LogWrite("SettingsForm-> The language list (languages.json) has been loaded.");

            // 言語選択(Formの言語切り替え)
            SelectFormLanguage(SettingsManager.Current.LanguageCode);
            LogHelper.LogWrite($"SettingsForm-> The language selected is \"{SettingsManager.Current.LanguageCode}\".");

            // 設定フォームに設定を読み出し
            this.Load += LoadSettingsToUI;
            LogHelper.LogWrite("SettingsForm-> SettingsForm() The SettingsForm has been loaded.");
            LogHelper.LogWrite("SettingsForm-> SettingsForm() End");
        }

        /// <summary>
        /// 表示ドライブ選択 CheckBox の配列を初期化 
        /// </summary>
        private void InitializeDriveBoxArray()
        {
            // ドライブ選択チェックボックス配列の初期化
            driveBoxes = [
                chkDriveA, chkDriveB, chkDriveC, chkDriveD, chkDriveE, chkDriveF,
                chkDriveG, chkDriveH, chkDriveI, chkDriveJ, chkDriveK, chkDriveL,
                chkDriveM, chkDriveN, chkDriveO, chkDriveP, chkDriveQ, chkDriveR,
                chkDriveS, chkDriveT, chkDriveU, chkDriveV, chkDriveW, chkDriveX,
                chkDriveY, chkDriveZ
            ];
        }

        /// <summary>
        /// UI(Form)に設定を読み出す 
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト Form</param>
        /// <param name="e">イベントデータ(追加情報はなし)</param>
        private void LoadSettingsToUI(object? sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> LoadSettingsToUI() Start");
            // 設定データを読み出し
            SettingsManager.Current = SettingsManager.Load();
            // 表示ドライブ
            var monitored = SettingsManager.Current.MonitoredDrives // 監視対象ドライブ
                .Select(c => char.ToUpperInvariant(c))              // ドライブ文字を大文字に変換
                .ToHashSet();                                       // HashSet に変換

            foreach (var box in driveBoxes)
            {// chkDriveA～chkDriveZ 全てに対し
                // 表示ドライブチェックボックスの末尾1文字を取得 (chkDrive* → '*')
                char letter = box.Name[^1];
                // ドライブのルートディレクトリ
                string root = letter + @":\";
                // ドライブ情報を取得
                var info = DriveInfo.GetDrives().FirstOrDefault(d => d.Name.Equals(root, StringComparison.OrdinalIgnoreCase));
                // 各ドライブのチェック状態を設定
                box.Checked = monitored.Contains(letter);

                if (info != null                            // ドライブ情報が null ではない
                    && info.IsReady                         // 未準備ドライブではない
                    && info.DriveType != DriveType.CDRom    // CD-ROMドライブではない
                    && info.DriveType != DriveType.Network) // ネットワークドライブではない
                {// 上記条件をすべて満たす場合
                    // ドライブ選択を有効化
                    box.Enabled = true;
                    LogHelper.LogWrite($"SettingsForm.cs-> LoadSettingsToUI() Drive letter \"{letter}\" enabled. ");
                    LogHelper.LogWrite($"SettingsForm.cs-> LoadSettingsToUI() Drive letter \"{letter}\" monitor \"{box.Checked}\".");
                }
                else
                {// ドライブ情報が null、未準備ドライブ、CD-ROMドライブ、ネットワークドライブ のいずれかである場合
                    // ドライブ選択を無効化 (チェックボックスをグレーアウト)
                    box.Enabled = false;
                    // 表示ドライブ選択を解除
                    box.Checked = false;
                }
            }

            // 表示サイクル
            numDisplayInterval.Value = SettingsManager.Current.DisplayInterval;

            // ドライブ文字色
            panelColorPreview.BackColor = ColorTranslator.FromHtml(SettingsManager.Current.DriveLetterColor);

            // アイコン画像フォルダー
            txtFolderPath.Text = SettingsManager.Current.IconFolderPath;

            // Windows起動時に自動実行 
            chkStartup.Checked = SettingsManager.Current.RunOnStartup;
            // 自動実行の設定をWindowsに反映
            ReflectingAutorunSettings(SettingsManager.Current.RunOnStartup);

            // アイコンサンプル
            UpdateSampleIcons();

            // logを記録する
            chkEnableLog.Checked = SettingsManager.Current.LogEnabled;

            // Language (言語)
            // 選択された言語コードから言語名を取得して言語名選択コンボボックスに反映
            ComboLanguage.SelectedItem = LangManager.GetName(SettingsManager.Current.LanguageCode);
            // 選択された言語コードから言語名(英語)を取得して言語名(英語)選択コンボボックスに反映
            ComboLanguageEn.SelectedItem = LangManager.GetEname(SettingsManager.Current.LanguageCode);
            LogHelper.LogWrite("SettingsForm-> LoadSettingsToUI() End");
        }


        /// <summary>
        /// SettingsForm の設定を SettingsManager に保存する
        /// </summary>
        private void SaveSettings()
        {
            LogHelper.LogWrite("SettingsForm-> SaveSettings() Start");
            // 表示ドライブリストを作成
            List<char> drives = [];

            foreach (var box in driveBoxes)
            {// chkDriveA～chkDriveZ 全てに対し
                if (box.Checked)
                {// 表示ドライブがチェックされている場合
                    // 表示ドライブチェックボックスの末尾1文字を取得 (chkDrive* → '*')
                    char letter = box.Name[^1];
                    // 表示ドライブリストに追加
                    drives.Add(letter);
                }
            }

            if (drives.Count == 0)
            {// 表示ドライブが1つも選択されていない場合
                // ドライブリストにCドライブを追加
                drives.Add('C');
            }

            // 表示ドライブリスト 設定に反映
            SettingsManager.Current.MonitoredDrives = drives;

            // 表示間隔 [msec] 設定に反映
            SettingsManager.Current.DisplayInterval = (int)numDisplayInterval.Value;

            // ドライブ文字色 設定に反映
            SettingsManager.Current.DriveLetterColor =
                ColorTranslator.ToHtml(panelColorPreview.BackColor);

            // アイコン画像フォルダー
            if (Directory.Exists(txtFolderPath.Text))
            {// 指定されたフォルダーが存在する場合
                // アイコン画像フォルダー 設定に反映
                SettingsManager.Current.IconFolderPath = txtFolderPath.Text;
            }

            // logを記録する 設定に反映
            SettingsManager.Current.LogEnabled = chkEnableLog.Checked;

            // Language (言語)
            LogHelper.LogWrite($"SettingsForm-> SaveSettings() ComboLanguage.SelectedItem = \"{ComboLanguage.SelectedItem}\"");
            LogHelper.LogWrite($"SettingsForm-> SaveSettings() ComboLanguageEn.SelectedItem = \"{ComboLanguageEn.SelectedItem}\"");
            if (ComboLanguage.SelectedItem is string name && !string.IsNullOrEmpty(name))
            {// 言語選択コンボボックスの言語名が存在する場合
                // 選択された言語名から言語コードを取得して設定に反映
                SettingsManager.Current.LanguageCode = LangManager.GetNameToCode(name);
                // 言語コードから言語名(英語)を取得してフォームに反映(念の為)
                ComboLanguageEn.SelectedItem = LangManager.GetEname(SettingsManager.Current.LanguageCode);
            }
            else
            {// 言語選択コンボボックスの言語名が存在しない場合
                // 言語コードを デフォルト値 に設定
                SettingsManager.Current.LanguageCode = SettingsManager.DefLangCode;
                // 言語選択コンボボックス の言語名を デフォルト値 に設定
                ComboLanguage.SelectedItem = SettingsManager.DefLangName;
                // 言語(英語)選択コンボボックス の言語名(英語)を デフォルト値 に設定
                ComboLanguageEn.SelectedItem = SettingsManager.DefLangEname;
            }
            LogHelper.LogWrite($"SettingsForm-> SaveSettings() SettingsManager.Current.LanguageCode = \"{SettingsManager.Current.LanguageCode}\"");

            // Windows起動時に自動実行
            SettingsManager.Current.RunOnStartup = chkStartup.Checked;
            // 自動実行の設定をWindowsに反映
            ReflectingAutorunSettings(SettingsManager.Current.RunOnStartup);

            // 設定を設定ファイルに保存
            SettingsManager.Current.Save();
            LogHelper.LogWrite("SettingsForm-> SaveSettings() End");
        }

        /// <summary>
        /// Windows起動時に自動実行 の設定をWindowsに反映する(タスクスケジューラー方式) 
        /// </summary>
        /// <param name="flag">true:タスク登録、false:タスク登録解除</param>
        private static void ReflectingAutorunSettings(bool flag)
        {
            LogHelper.LogWrite($"SettingsForm-> ReflectAutorunSetting({flag}) Start");
            if (flag)
            {// チェックされている場合
                StartupHelper.Register();   // スタートアップ登録
                //StartupHelper.EnableStartup();  // スタートアップ有効化
                LogHelper.LogWrite($"SettingsForm-> ReflectAutorunSetting({flag}) Registaer");
            }
            else
            {// チェックされていない場合
                StartupHelper.Unregister(); // スタートアップ登録解除
                //StartupHelper.DisableStartup(); // スタートアップ無効化                   
                LogHelper.LogWrite($"SettingsForm-> ReflectAutorunSetting({flag}) Unregistaer");
            }
            LogHelper.LogWrite($"SettingsForm-> ReflectAutorunSetting({flag}) End");
        }

        /// <summary>
        /// ドライブステータスに応じたサンプルアイコンを描画する
        /// </summary>
        /// <param name="status">"none":読み書き無し、"write":書き込み、"read":読み取り、"both":読み書き両方</param>
        /// <returns>Bitmap DPI>=144:SizeW16xH32、DPI<144:SizeW8xH16</returns>
        private Bitmap CreateSampleIcon(string status)
        {
            // DPI値 を取得
            int dpi = this.DeviceDpi;

            // 1ドライブ分のドライブ画像サイズ
            int driveSizeW = dpi >= SettingsManager.SizeChangeDpi ? 16 : 8;     // 幅   W 解像度閾値以上なら16、未満なら 8
            int driveSizeH = dpi >= SettingsManager.SizeChangeDpi ? 32 : 16;    // 高さ H 解像度閾値以上なら32、未満なら16
            // 1ドライブ分のドライブ文字サイズ
            int letterSizeW = dpi >= SettingsManager.SizeChangeDpi ? 16 : 8;    // 幅   W 解像度閾値以上なら16、未満なら 8
            int letterSizeH = dpi >= SettingsManager.SizeChangeDpi ? 16 : 8;    // 高さ H 解像度閾値以上なら16、未満なら 8

            // アイコン画像フォルダーの解像度別サブフォルダー名を取得
            string iconSizeFolderName = dpi >= SettingsManager.SizeChangeDpi ? "32" : "16";

            string fileName = status switch
            {// ドライブのステータスに応じてアイコン画像ファイル名を取得
                "none" => "write_off_read_off.png",     // 読み取り・書き込み 両方オフ画像
                "write" => "write_on__read_off.png",    // 読み取りオフ、書き込みオン画像
                "read" => "write_off_read_on_.png",     // 読み取りオン、書き込みオフ画像
                "both" => "write_on__read_on_.png",     // 読み取り・書き込み 両方オン画像
                _ => "write_off_read_off.png"           // 不明時は 読み取り・書き込み 両方オフ
            };

            // アイコン画像ファイルパスを取得
            string iconImgFilePath = Path.Combine(txtFolderPath.Text, iconSizeFolderName, fileName);

            // 受け渡し用ビットマップを作成
            Bitmap result = new(driveSizeW, driveSizeH);

            try
            {
                using (Graphics g = Graphics.FromImage(result))
                {
                    // 描画品質設定
                    g.CompositingMode = CompositingMode.SourceOver;             // 元の画像を保持しつつ描画
                    g.CompositingQuality = CompositingQuality.HighQuality;      // 高品質合成
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic; // 高品質補間
                    g.SmoothingMode = SmoothingMode.AntiAlias;                  // アンチエイリアス
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;   // クリアタイプテキスト

                    // 透明背景でクリア
                    g.Clear(Color.Transparent);

                    if (File.Exists(iconImgFilePath))
                    {// アイコン画像のファイルパスが存在する場合
                        // アイコン画像を読み込む
                        using Bitmap baseImg = new(iconImgFilePath);

                        // アイコン画像を描画
                        g.DrawImage(baseImg, 0, 0, baseImg.Width, baseImg.Height);
                    }
                    else
                    {// アイコン画像のファイルパスが存在しない場合
                        LogHelper.LogWrite($"SettingsForm-> Icon file not found: {iconImgFilePath}");
                        // 何もせず result は透明のまま（フォールバック）
                    }

                    // ドライブ文字フォントを取得
                    using var font = FontHelper.GetDriveLetterFont((float)dpi);
                    // ドライブ文字の色を取得
                    using var brush = new SolidBrush(panelColorPreview.BackColor);
                    // ドライブ文字のフォーマットを中央揃えに設定
                    using var format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    // ドライブ文字描画用の矩形サイズを取得
                    var rect = new Rectangle(0, 0, letterSizeW, letterSizeH);

                    // ドライブ文字を描画
                    g.DrawString("C", font, brush, rect, format);
                }

                //ビットマップを返す
                return result;
            }
            catch (Exception ex)
            {// 不正だった場合
                // サンプルアイコン描画エラーとログに書き込む
                LogHelper.LogWrite($"SettingsForm-> CreateSampleIcon error: {ex.Message}");
                // フォールバック画像を返す
                return result;
            }
        }

        /// <summary>
        /// アイコンサンプルを更新する
        /// </summary>
        private void UpdateSampleIcons()
        {
            LogHelper.LogWrite("SettingsForm-> UpdateSampleIcons() Start");
            try
            {
                // アイコンサンプル画像の置換
                ReplaceImage(picSampleNone, CreateSampleIcon("none"));  // 読み取りオフ、書き込みオフサンプル
                ReplaceImage(picSampleWrite, CreateSampleIcon("write"));// 読み取りオフ、書き込みオンサンプル
                ReplaceImage(picSampleRead, CreateSampleIcon("read"));  // 読み取りオン、書き込みオフサンプル
                ReplaceImage(picSampleBoth, CreateSampleIcon("both"));  // 読み取りオン、書き込みオンサンプル
                LogHelper.LogWrite("SettingsForm-> UpdateSampleIcons() Replaced the icon sample image.");
            }
            catch (Exception ex)
            {// 不正だった場合
                // アイコンサンプル更新エラーとログに書き込む
                LogHelper.LogWrite($"SettingsForm-> UpdateSampleIcons error: {ex.Message}");
            }
            LogHelper.LogWrite("SettingsForm-> UpdateSampleIcons() End");
        }

        /// <summary>
        /// PictureBox の画像を置き換える
        /// </summary>
        /// <param name="box">PictureBox(描画先)</param>
        /// <param name="newImg">Image(新しい画像)</param>
        // PictureBox の画像置換
        private static void ReplaceImage(PictureBox box, Image newImg)
        {
            var old = box.Image;    // 現在のPictureBoxの画像を退避
            box.Image = newImg;     // 新しい画像をPictureBoxに描画
            old?.Dispose();         // 古い画像を破棄
        }


        /// <summary>
        /// ドライブ文字色の 色選択ボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnColor.Click</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        // ドライブ文字色の色選択ボタン クリック
        private void BtnColor_Click(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> BtnColor_Click() Start");
            // カラーダイアログを表示
            using var dlg = new ColorDialog();
            // 現在の色を設定
            dlg.Color = panelColorPreview.BackColor;

            if (dlg.ShowDialog() == DialogResult.OK)
            {// 色が選択された場合
                // ドライブ文字色を選択色に変更
                panelColorPreview.BackColor = dlg.Color;
                // アイコンサンプルを更新
                UpdateSampleIcons();
            }
            LogHelper.LogWrite("SettingsForm-> BtnColor_Click() End");
        }


        /// <summary>
        /// アイコン画像フォルダー選択ボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnBrowse.Click</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> BtnBrowse_Click() Start");
            // フォルダーブラウザーダイアログを表示
            using var dlg = new FolderBrowserDialog();
            // 説明を設定
            dlg.Description = LangManager.Lang.T("BrowsWindowMessage") + "　";

            if (dlg.ShowDialog() == DialogResult.OK)
            {// フォルダーが選択された場合
                // テキストボックスに選択フォルダーのパスを設定
                txtFolderPath.Text = dlg.SelectedPath;
                // アイコンサンプルを更新
                UpdateSampleIcons();
            }
            LogHelper.LogWrite($"SettingsForm-> BtnBrowse_Click() FolderPath=\"{txtFolderPath.Text}\"");
            LogHelper.LogWrite("SettingsForm-> BtnBrowse_Click() End");
        }


        /// <summary>
        /// logフォルダーを開くボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnOpenLogFolder.Click</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        private void BtnOpenLogFolder_Click(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> BtnOpenLogFolder_Click() Start");
            try
            {
                // logフォルダーのパスを取得
                string path = LogHelper.LogFolderPath;

                if (!Directory.Exists(path))
                {// フォルダーが存在しない場合
                    // logフォルダーを作成
                    Directory.CreateDirectory(path);
                }

                // logフォルダーをエクスプローラーで開く
                Process.Start("explorer.exe", path);
            }
            catch (Exception ex)
            {// logフォルダーを開けなかった場合
                // エラーメッセージボックスを表示
                MessageBox.Show(
                    LangManager.Lang.T("LogFolderOpenErrorMessage") + $": {ex.Message}",    // メッセージ
                    LangManager.Lang.T("LogFolderOpenErrorTitle"),                          // タイトル
                    MessageBoxButtons.OK,                                                   // ボタン
                    MessageBoxIcon.Error                                                    // アイコン
                );
            }
            LogHelper.LogWrite("SettingsForm-> BtnOpenLogFolder_Click() End");
        }

        /// <summary>
        /// logクリアボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnLogCleare.Click</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        private void BtnLogClear_Click(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> BtnLogClear_Click() Start");
            // 確認メッセージボックスを表示
            var result = MessageBox.Show(
                LangManager.Lang.T("LogClearQuestionMessage"),  // メッセージ
                LangManager.Lang.T("LogClearQuestionTitle"),    // タイトル
                MessageBoxButtons.OKCancel,                     // ボタン
                MessageBoxIcon.Question                         // アイコン
            );

            if (result == DialogResult.OK)
            {// OKが押された場合
                try
                {
                    // logをクリア
                    LogHelper.ClearLog();
                }
                catch (Exception ex)
                {// logクリアに失敗した場合
                    // エラーメッセージボックスを表示
                    MessageBox.Show(
                        LangManager.Lang.T("LogClearErrorMessage") + $": {ex.Message}", // メッセージ
                        LangManager.Lang.T("LogClearErrorTitle"),                       // タイトル
                        MessageBoxButtons.OK,                                           // ボタン
                        MessageBoxIcon.Error                                            // アイコン
                    );
                }
            }
            else
            {// Cancelが押された場合
                // 何もしない
            }
            LogHelper.LogWrite("SettingsForm-> BtnLogClear_Click() End");
        }

        /// <summary>
        /// 言語名リスト(languages.json)を読み込み
        /// ComboLanguage.Items、ComboLanguageEn.Items に反映
        /// </summary>
        private void LoadLanguageList()
        {
            LogHelper.LogWrite("SettingsForm-> LoadLanguageList() Start");
            try
            {
                // 言語情報リストを取得 (LangManager.LanguagesInfoList作成)
                LangManager.LoadLanguagesJson(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,  // アプリケーションルートパス
                    "Resources\\Language\\languages.json"   // Resources\Language\languages.json
                    )
                );
                // コンボボックスをクリア
                ComboLanguage.Items.Clear();
                LogHelper.LogWrite("SettingsForm-> LoadLanguageList() \"ComboLanguage.Items\" has been cleared.");
                ComboLanguageEn.Items.Clear();
                LogHelper.LogWrite("SettingsForm-> LoadLanguageList() \"ComboLanguageEn.Items\" has been cleared.");

                if (LangManager.LanguagesInfoList != null)
                { // 言語情報リストが null ではない(作成済みの)場合
                    foreach (var lang in LangManager.LanguagesInfoList)
                    {// 各言語情報ごとに
                        // 言語情報の言語名のみ(lang.ToString())をComboLanguage.Itemsに追加
                        ComboLanguage.Items.Add(lang?.ToString() ?? "");
                        // 言語情報の言語名(英語)のみ(LangManager.GetNameToEname(lang.ToString()))をComboLanguageEn.Itemsに追加
                        ComboLanguageEn.Items.Add(LangManager.GetNameToEname(lang?.ToString() ?? ""));
                    }
                    LogHelper.LogWrite("SettingsForm-> LoadLanguageList() \"ComboLanguage.Items\" now reflects \"languages.json\".");
                    LogHelper.LogWrite("SettingsForm-> LoadLanguageList() \"ComboLanguageEn.Items\" now reflects \"languages.json\".");
                }
            }
            catch (Exception ex)
            {// 不正だった場合
                // LoadLanguageListエラーとログに書き込む
                LogHelper.LogWrite($"SettingsForm-> LoadLanguageList() failed: {ex.Message}");
            }
            LogHelper.LogWrite("SettingsForm-> LoadLanguageList() End");
        }

        /// <summary>
        /// 言語コンボボックス 選択変更
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト ComboLanguage.SelectedIndexChanged</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        private void ComboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> ComboLanguiage_SelectedIndexChanged() Start");
            if (ComboLanguage.SelectedItem is string name && !string.IsNullOrEmpty(name))
            {// 言語名が存在する場合
                // コンボボックスで選択された言語の言語コードを設定に反映
                SettingsManager.Current.LanguageCode = LangManager.GetNameToCode(name);
                // 設定言語コードの言語名(英語)を言語(英語)選択コンボボックスに反映
                ComboLanguageEn.SelectedItem = LangManager.GetEname(SettingsManager.Current.LanguageCode);
            }
            else
            {// 言語名が存在しない場合
                // 言語コードを デフォルト値 に設定
                SettingsManager.Current.LanguageCode = SettingsManager.DefLangCode;
                // 言語名(英語)を  に設定
                ComboLanguageEn.SelectedItem = SettingsManager.DefLangEname;
            }
            // 言語ファイルを読み込む
            LangManager.Lang.Load(SettingsManager.Current.LanguageCode);
            // フォームに言語を適用
            ApplyLanguage();
            LogHelper.LogWrite("SettingsForm-> ComboLanguiage_SelectedIndexChanged() End");
        }

        /// <summary>
        /// 言語(英語)コンボボックス 選択変更
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト ComboLanguageEn.SelectedIndexChanged</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        // 言語(英語)コンボボックス 選択変更
        private void ComboLanguageEn_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> ComboLanguiageEn_SelectedIndexChanged() Start");
            if (ComboLanguageEn.SelectedItem is string name && !string.IsNullOrEmpty(name))
            {// 言語名が存在する場合
                // コンボボックスで選択された言語の言語コードを設定に反映
                SettingsManager.Current.LanguageCode = LangManager.GetEnameToCode(name);
                // 設定言語コードの言語名を言語選択コンボボックスに反映
                ComboLanguage.SelectedItem = LangManager.GetName(SettingsManager.Current.LanguageCode);
            }
            else
            {// 言語名が存在しない場合
                // 言語コードを デフォルト値 に設定
                SettingsManager.Current.LanguageCode = SettingsManager.DefLangCode;
                // 言語名を デフォルト値 に設定
                ComboLanguage.SelectedItem = SettingsManager.DefLangName;
            }
            // 言語ファイルを読み込む
            LangManager.Lang.Load(SettingsManager.Current.LanguageCode);
            // フォームに言語を適用
            ApplyLanguage();
            LogHelper.LogWrite("SettingsForm-> ComboLanguiageEn_SelectedIndexChanged() End");
        }

        /// <summary>
        /// 言語コードから該当する言語を選択
        /// </summary>
        /// <param name="languageCode">言語コード</param>
        public void SelectFormLanguage(string languageCode)
        {
            LogHelper.LogWrite("SettingsForm-> SelectFormLanguage() Start");
            if (!string.IsNullOrEmpty(languageCode))
            {// 言語が存在する場合
                // 言語ファイルを読み込む
                LangManager.Lang.Load(languageCode);
                // フォームに言語を適用
                ApplyLanguage();
                // ログに言語名から言語コードを適用と書き込み
                LogHelper.LogWrite($"Set the language to \"{languageCode}\".");
            }
            LogHelper.LogWrite("SettingsForm-> SelectFormLanguage() End");
        }

        /// <summary>
        /// チェックボックスの幅をテキストに合わせて調整する
        /// (SettingsForm を表示した際、chkEnableLog のテキストが欠ける不具合の対策)
        /// </summary>
        /// <param name="chk">幅を変更するCheckBox</param>
        private static void FixCheckboxWidth(CheckBox chk)
        {
            using var g = chk.CreateGraphics();             // チェックボックスのGraphicsオブジェクトを作成
            var size = g.MeasureString(chk.Text, chk.Font); // テキストのサイズを測定
            chk.Width = (int)Math.Ceiling(size.Width) + 30; // テキスト幅に余白30を加えて幅を設定
        }

        /// <summary>
        /// SettingsFormに表示言語を適用する
        /// </summary>
        private void ApplyLanguage()
        {
            LogHelper.LogWrite("SettingsForm-> ApplyLanguage() Start");
            // Form内の各オブジェクトのテキストにjsonファイルのテキストを適用
            labelSettings.Text = LangManager.Lang.T("labelSettingsText");                       // ラベル "設定"
            chkStartup.Text = LangManager.Lang.T("chkStartupText");                             // チェックボックス "Windows起動時に自動実行"
            gbDisplayDriveSelection.Text = LangManager.Lang.T("gbDisplayDriveSelectionText");   // グループボックス "表示ドライブ選択"
            gbDriveLettersColor.Text = LangManager.Lang.T("gbDriveLettersColorText");           // グループボックス "ドライブ文字色"
            BtnColor.Text = LangManager.Lang.T("BtnColorText");                                 // ボタン "色選択"
            gbDisplayInterval.Text = LangManager.Lang.T("gbDisplayIntervalText");               // グループボックス "表示サイクル [msec]"
            gbIconImageFolder.Text = LangManager.Lang.T("gbIconImageFolderText");               // グループボックス "アイコン画像フォルダー"
            BtnBrowse.Text = LangManager.Lang.T("BtnBrowseText");                               // ボタン "フォルダー選択"
            gbLog.Text = LangManager.Lang.T("gbLogText");                                       // グループボックス "Log"
            chkEnableLog.Text = LangManager.Lang.T("chkEnableLogText");                         // チェックボックス "logを記録する"
            BtnOpenLogFolder.Text = LangManager.Lang.T("BtnOpenLogFolderText");                 // ボタン "logフォルダーを開く"
            BtnLogClear.Text = LangManager.Lang.T("BtnLogClearText");                           // ボタン "logクリア"
            gbIconSample.Text = LangManager.Lang.T("gbIconSampleText");                         // グループボックス "アイコンサンプル"
            BtnVersionInfo.Text = LangManager.Lang.T("BtnVersionInfoText");                     // ボタン "バージョン情報"
            BtnCancel.Text = LangManager.Lang.T("BtnCancelText");                               // ボタン "キャンセル"
            BtnSave.Text = LangManager.Lang.T("BtnSaveText");                                   // ボタン "保存"

            // chkStartup の幅をテキストに合わせて調整(テキストの文字欠け防止)
            FixCheckboxWidth(chkStartup);
            // chkEnableLog の幅をテキストに合わせて調整(テキストの文字欠け防止)
            FixCheckboxWidth(chkEnableLog);
            // 再描画
            this.PerformLayout();
            LogHelper.LogWrite("SettingsForm-> ApplyLanguage() End");
        }

        /// <summary>
        /// キャンセルボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnCancel.Click</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> BtnCancel_Click() Start");
            // フォームを閉じる
            Close();
            LogHelper.LogWrite("SettingsForm-> BtnCancel_Click() SettingsForm has been closed.");

            // 保存している設定データを読み出し
            SettingsManager.Current = SettingsManager.Load();
            // 言語コードから言語名を取得しComboLanguage.SettingItemに設定
            ComboLanguage.SelectedItem = LangManager.GetName(SettingsManager.Current.LanguageCode);
            LogHelper.LogWrite($"SettingsForm-> BtnCancel_Click() ComboLanguage.SelectedItem = \"{ComboLanguage.SelectedItem}\"");
            // 言語コードから言語名(英語)を取得しComboLanguageEn.SettingItemに設定
            ComboLanguageEn.SelectedItem = LangManager.GetEname(SettingsManager.Current.LanguageCode);
            LogHelper.LogWrite($"SettingsForm-> BtnCancel_Click() ComboLanguageEn.SelectedItem = \"{ComboLanguageEn.SelectedItem}\"");
            LogHelper.LogWrite("SettingsForm-> BtnCancel_Click() End");
        }

        /// <summary>
        /// 保存ボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnSave.Click</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> BtnSave_Click() Start");
            // 設定保存
            SaveSettings();
            // 言語選択(Formの言語切り替え)
            SelectFormLanguage(SettingsManager.Current.LanguageCode);
            // TrayIconManager に設定変更を反映
            TrayIconManager.Instance.ReloadMonitor();
            // フォームを閉じる
            Close();
            LogHelper.LogWrite("SettingsForm-> BtnSave_Click() End");
        }

        /// <summary>
        /// バージョン情報フォームの状態
        /// </summary>
        private VersionInfoForm? _versionInfoForm;

        /// <summary>
        /// バージョン情報ボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnVersionInfo.Click</param>
        /// <param name="e">イベントデータ(追加情報なし)</param>
        private void BtnVersionInfo_Click(object sender, EventArgs e)
        {
            LogHelper.LogWrite("SettingsForm-> BtnVersionInfo_Click() Start");
            if (_versionInfoForm == null || _versionInfoForm.IsDisposed)
            {// バージョン情報フォームが非表示である場合
                // バージョン情報フォームを読み込み
                _versionInfoForm = new VersionInfoForm();
            }

            // バージョン情報フォームを非モーダルで開く
            _versionInfoForm.Show();
            // バージョン情報フォームを最前面に表示
            _versionInfoForm.BringToFront();
            LogHelper.LogWrite("SettingsForm-> BtnVersionInfo_Click() End");
        }
    }
}