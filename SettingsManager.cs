//-------------------------------------------------------------------
// SettingsManager.cs
// 設定の読み書きと保持を管理する
//-------------------------------------------------------------------

using System.Text.Json;

namespace DriveIndicatorAI
{
    /// <summary>
    /// 設定の読み書きと保持を管理するクラス
    /// </summary>
    public class SettingsManager
    {
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        private const string SettingsFileName = "settings.json";

        /// <summary>
        /// アイコンサイズを切り替えるDPI値: デフォルト: 144 DPI (150%拡大表示時) 固定値
        /// </summary>
        public const int SizeChangeDpi = 144;

        /// <summary>
        /// デフォルト言語コード "ja"
        /// </summary>
        public const string DefLangCode = "ja";
        /// <summary>
        /// デフォルト言語名 "日本語"
        /// </summary>
        public const string DefLangName = "日本語";
        /// <summary>
        /// デフォルト言語名(英語) "Japanese"
        /// </summary>
        public const string DefLangEname = "Japanese";

        /// <summary>
        /// 現在設定インスタンス 初回読み込みフラグ (初回読み込みはログの書き込みをパス)
        /// </summary>
        private static bool firstLoadFlag = false;

        /// <summary>
        /// 現在設定インスタンス
        /// </summary>
        public static SettingsManager Current { get; set; } = Load();

        //-----------------------------------------------------------
        // 設定ファイル (settings.json) に保存するデータ ここから

        /// <summary>
        /// Windows起動時に自動実行 デフォルト: false(しない)
        /// </summary>
        public bool RunOnStartup { get; set; } = false;

        /// <summary>
        /// 表示ドライブ設定のリスト デフォルト: Cドライブのみ
        /// </summary>
        public List<char> MonitoredDrives { get; set; } = ['C'];

        /// <summary>
        /// ドライブ文字色 (HTMLカラーコード形式) デフォルト: #BFBFBF (R=191,G=191,B=191 薄いグレー)
        /// </summary>
        public string DriveLetterColor { get; set; } = "#BFBFBF";

        /// <summary>
        /// 表示間隔 [msec] (1～250msec) デフォルト: 50ms
        /// </summary>
        public int DisplayInterval { get; set; } = 50;

        /// <summary>
        /// アイコン画像のフォルダーパス デフォルト: アプリケーションフォルダー直下の "Resources\Icons\Default"
        /// </summary>
        public string IconFolderPath { get; set; } = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Resources\\Icons\\Default"
            );

        /// <summary>
        /// logを記録する デフォルト：false(しない)
        /// </summary>
        public bool LogEnabled { get; set; } = false;

        /// <summary>
        /// 言語コード デフォルト：DefLangCode
        /// 対応言語はアプリケーションフォルダー直下の "Resources\Language\languages.json"を参照 
        /// </summary>
        public string LanguageCode { get; set; } = DefLangCode;

        // 設定ファイル (settings.json) に保存するデータ ここまで
        //-----------------------------------------------------------

        /// <summary>
        /// アプリケーションフォルダー内の設定ファイルパスを取得
        /// </summary>
        /// <returns>アプリケーションフォルダー内の Settings.json ファイルのパス</returns>
        private static string GetAppFolderSettingsFilePath()
        {
            // アプリケーションフォルダーパスを取得
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            // アプリケーションフォルダー内の設定ファイルパスを返す
            return Path.Combine(exeDir, SettingsFileName);
        }

        /// <summary>
        /// 一時フォルダー内の設定ファイルパスを取得
        /// </summary>
        /// <returns>一時フォルダー内の Settings.json ファイルのパス</returns>
        private static string GetTempSettingsFilePath()
        {
            // 一時フォルダーパスを取得
            string tempDir = Path.Combine(Path.GetTempPath(), "DriveIndicatorAI");
            // 一時フォルダーが存在しない場合は作成
            Directory.CreateDirectory(tempDir);
            // 一時フォルダー内の設定ファイルパスを返す
            return Path.Combine(tempDir, SettingsFileName);
        }

        /// <summary>
        /// JSONシリアライズオプション 大文字･小文字区別なし
        /// </summary>
        private static readonly JsonSerializerOptions _jsonOptionsCaseInsensitive =
            new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// settings.jsonファイルを読み込みインスタンスに反映
        /// </summary>
        /// <returns>SettingsManager インスタンス</returns>
        public static SettingsManager Load()
        {
            if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Load() Start");
            try
            {
                // アプリケーションフォルダー内の設定ファイルパスを取得
                string appSettingsFilePath = GetAppFolderSettingsFilePath();
                // 一時フォルダー内の設定ファイルパスを取得
                string tempSettingsFilePath = GetTempSettingsFilePath();

                if (!File.Exists(appSettingsFilePath))
                {// アプリケーションフォルダー内に設定ファイルが存在しない場合
                    // デフォルト設定を作成
                    var def = new SettingsManager();
                    // デフォルト設定を保存
                    def.Save();
                }

                if (!File.Exists(tempSettingsFilePath))
                {// 一時フォルダー内に設定ファイルが存在しない場合
                    // アプリケーションフォルダー内の設定ファイルを一時フォルダーに上書きコピー
                    File.Copy(appSettingsFilePath, tempSettingsFilePath, overwrite: true);
                }

                if (!File.Exists(appSettingsFilePath) || !File.Exists(tempSettingsFilePath))
                {// アプリケーションフォルダー内または一時フォルダー内に設定ファイルが存在しない場合
                    // ログに 設定の読み込みに失敗 を書き込む
                    if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Load() Failed to load settings.");
                    if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Load() End");
                    //初回読み込みフラグ初回読み込み済みに設定
                    firstLoadFlag = true;
                    // 新規デフォルト設定を返す
                    return new SettingsManager();
                }

                // 設定ファイルの内容を読み込み
                string json = File.ReadAllText(tempSettingsFilePath);
                // ログに 設定読み込み を書き込む
                if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Load() Settings loaded.");
                if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Load() End");
                //初回読み込みフラグ初回読み込み済みに設定
                firstLoadFlag = true;
                // 設定ファイルをデシリアライズして返す
                // (設定ファイル文字列を設定型オブジェクトに変換し読み込んで返す)
                return JsonSerializer.Deserialize<SettingsManager>(json, _jsonOptionsCaseInsensitive)
                    // 設定ファイルが正常でなければ新規デフォルト設定にする
                    ?? new SettingsManager();
            }
            catch (Exception ex)
            {// 不正であった場合
                // ログに 設定読み込み失敗 を書き込む
                if (firstLoadFlag) LogHelper.LogWrite($"SettingsManager-> Load() Failed to load settings. Message:{ex.Message}");
                if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Load() End");
                //初回読み込みフラグ初回読み込み済みに設定
                firstLoadFlag = true;
                // 新規デフォルト設定を返す
                return new SettingsManager();
            }
        }

        /// <summary>
        /// JSONシリアライズオプション インデント付き
        /// </summary>
        private static readonly JsonSerializerOptions _jsonOptionsIndent =
            new() { WriteIndented = true, };


        /// <summary>
        /// 設定インスタンスを settings.json ファイルに保存する
        /// </summary>
        public void Save()
        {
            if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Save() Start");
            // アプリケーションフォルダー内の設定ファイルパスを取得
            string appSettingsFilePath = GetAppFolderSettingsFilePath();
            // 一時フォルダー内の設定ファイルパスを取得
            string tempSettingsFilePath = GetTempSettingsFilePath();

            try
            {
                // 設定インスタンスをJSON形式の文字列に変換 (インデント付き)
                string json = JsonSerializer.Serialize(this, _jsonOptionsIndent);
                // JSON形式文字列をアプリケーションフォルダの設定ファイルとして書き込む
                File.WriteAllText(appSettingsFilePath, json);
                // アプリケーションフォルダー内の設定ファイルを一時フォルダーに上書きコピー
                File.Copy(appSettingsFilePath, tempSettingsFilePath, overwrite: true);

                if (File.Exists(appSettingsFilePath) && File.Exists(tempSettingsFilePath))
                {// アプリケーションフォルダー内と一時フォルダーに設定ファイルが存在する場合
                    // ログに 設定を保存 を書き込む
                    if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Save() Settings saved.");
                }
                else
                {// アプリケーションフォルダー内または一時フォルダーに設定ファイルが存在しない場合
                 // ログに 設定の保存に失敗 を書き込む
                    if (firstLoadFlag) LogHelper.LogWrite($"SettingsManager-> Save() Failed to save settings.");
                }
            }
            catch (Exception ex)
            {// 設定ファイル保存失敗時
                // ログに 設定の保存に失敗 を書き込む
                LogHelper.LogWrite($"SettingsManager-> Save() Failed to save settings. Message:{ex.Message}");
            }
            if (firstLoadFlag) LogHelper.LogWrite("SettingsManager-> Save() End");
        }
    }
}