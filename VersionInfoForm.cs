//-------------------------------------------------------------------
// VersionInfoForm.cs
// バージョン情報フォーム
//-------------------------------------------------------------------

using System.Diagnostics;

namespace DriveIndicatorAI
{
    /// <summary>
    /// バージョン情報フォームクラス
    /// </summary>
    public partial class VersionInfoForm : Form
    {
        /// <summary>
        /// バージョン情報フォーム
        /// </summary>
        public VersionInfoForm()
        {
            InitializeComponent();
            // バージョンナンバーを設定
            string? version = FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
            labelVersionNo.Text = $"{version}";
        }

        /// <summary>
        /// ライセンスダイアログの状態
        /// </summary>
        private LicenseDialog? _licenseDialog;

        /// <summary>
        /// ライセンスボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnLicense.Click</param>
        /// <param name="e">イベントデータ(追加情報はなし)</param>
        private void BtnLicense_Click(object sender, EventArgs e)
        {
            if (_licenseDialog == null || _licenseDialog.IsDisposed)
            {// ライセンスダイアログが非表示である場合
                // ライセンスダイアログを取得
                _licenseDialog = new LicenseDialog();
            }
            // ライセンスダイアログを非モーダルで表示
            _licenseDialog.Show();
            // ライセンスダイアログを最前面で表示
            _licenseDialog.BringToFront();
        }

        /// <summary>
        /// Github ラベルリンク クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト LinkLabelGithubLink.Click</param>
        /// <param name="e">イベントデータ(追加情報はなし)</param>
        private void LinkLabelGithubLink_Click(object sender, EventArgs e)
        {
            // 外部アプリケーションを起動
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/mrtam1005/DriveIndicatorAI", // URLを指定
                UseShellExecute = true  // Windowsのシェルに処理を任せる
            });
        }
    }
}