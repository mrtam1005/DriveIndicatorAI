//-------------------------------------------------------------------
// LicenseDialog.cs
// ライセンスのダイアログフォーム
//-------------------------------------------------------------------

namespace DriveIndicatorAI
{
    /// <summary>
    /// ライセンスダイアログフォームクラス
    /// </summary>
    public partial class LicenseDialog : Form
    {
        /// <summary>
        /// ライセンスダイアログ
        /// </summary>
        public LicenseDialog()
        {
            InitializeComponent();  // コンポーネントを初期化

            if (SettingsManager.Current.LanguageCode == "ja")
            {// 設定言語コードが "ja" の場合
                // ライセンステキストボックスに日本語テキストを設定
                textBoxLicense.Text =
                    "DriveIndicatorAI\r\n" +
                    "© 2025 mrtam1005\r\n\r\n" +
                    "本ソフトウェアは無償で提供されます。\r\n" +
                    "個人利用および非商用利用は自由に行うことができます。\r\n" +
                    "本ソフトウェアの再配布は、改変を行わない場合に限り許可します。\r\n" +
                    "商用利用を希望する場合は、開発者の許可が必要です。\r\n\r\n" +
                    "【免責事項】\r\n" +
                    "本ソフトウェアの使用または使用不能により生じたいかなる損害についても、\r\n" +
                    "開発者は一切の責任を負いません。\n" +
                    "本ソフトウェアの利用は、すべて利用者自身の責任において行ってください。\r\n\r\n" +
                    "【使用ライブラリ】\r\n" +
                    " ・Microsoft.NET Runtime\r\n" +
                    " ・System.Text.Json(MIT License)\r\n\r\n" +
                    "【Webサイト】\r\n" +
                    "  https://github.com/mrtam1005/DriveIndicatorAI\r\n"
                ;
            }
            else
            {// 設定言語コードが "ja" ではない場合
                // ライセンステキストボックスに英語テキストを設定
                textBoxLicense.Text =
                    "DriveIndicatorAI\r\n" +
                    "© 2025 mrtam1005\r\n\r\n" +
                    "This software is provided free of charge.\r\n" +
                    "Personal and non-commercial use is permitted.\r\n" +
                    "Redistribution of this software is permitted only if unmodified.\r\n" +
                    "Commercial use requires permission from the developer.\r\n\r\n" +
                    "【Disclaimer】\r\n" +
                    "The developer assumes no responsibility for any damages arising from the use of or inability to use this software.\r\n" +
                    "Use of this software is entirely at the user's own risk.\r\n\r\n" +
                    "【Libraries Used】\r\n" +
                    " ・Microsoft.NET Runtime\r\n" +
                    " ・System.Text.Json (MIT License)\r\n\r\n" +
                    "【Website】\r\n" +
                    "  https://github.com/mrtam1005/DriveIndicatorAI\r\n"
                ;
            }
        }

        /// <summary>
        /// OKボタン クリック
        /// </summary>
        /// <param name="sender">イベントを発生させたオブジェクト BtnLicenseOk.Click</param>
        /// <param name="e">イベントデータ(追加情報はなし)</param>
        private void BtnLicenseOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}