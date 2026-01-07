//-------------------------------------------------------------------
// LangManager.cs
// Form、Menu、Messageなどテキストの言語設定を管理
//-------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DriveIndicatorAI
{
    /// <summary>
    /// Form、Memu、Messageなどテキストの言語設定を管理するクラス
    /// </summary>
    internal class LangManager
    {
        /// <summary>
        /// 多言語対応クラス
        /// </summary>
        public static class Lang
        {
            // 言語テーブル
            private static Dictionary<string, string>? _table;

            /// <summary>
            /// 言語ファイル (Resource\Language\lang_*.json) を読み込む
            /// </summary>
            /// <param name="languageCode">言語コード</param>
            public static void Load(string languageCode)
            {
                LogHelper.LogWrite("LangManager-> Lang.Load() Start");
                if (!string.IsNullOrEmpty(languageCode))
                {// 言語設定 (言語コード) が存在する場合
                    // 言語設定 (言語コード) の言語ファイルパスを取得
                    string filePath = (Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        $"Resources\\Language\\lang_{languageCode}.json"
                        )
                    );
                    // 言語ファイルを読み込む
                    string json = File.ReadAllText(filePath);
                    try
                    {
                        // 言語テーブルをデシリアライズ
                        _table = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    }
                    catch (JsonException ex)
                    {// 不正だった場合
                        // ログにJSONパースエラー(エラーメッセージ含む)を書き込む
                        LogHelper.LogWrite($"LangManager-> Lang.Load() JSON parse error: {ex.Message}");
                        // テーブルを空で作成
                        _table = [];
                    }
                    LogHelper.LogWrite($"LangManager-> Lang.Load() Language file loaded. \"{languageCode}\"");
                }
                LogHelper.LogWrite("LangManager-> Lang.Load() End");
            }

            /// <summary>
            /// 言語テーブルから Form、Menu、Messageなどのキーワードに対応する翻訳文字列を取得
            /// </summary>
            /// <param name="keyword">キーワード</param>
            /// <returns>キーワードに対応する翻訳文字列</returns>
            // 
            public static string T(string keyword)
            {
                LogHelper.LogWrite($"LangManager-> Lang.T() Start (keyword=\"{keyword}\")");
                if (_table != null && _table.TryGetValue(keyword, out var value))
                {// 言語テーブルが存在 かつ 言語テーブルでキーワードに対応する翻訳文字列が取得できた場合
                    LogHelper.LogWrite($"LangManager-> Lang.T() End (return \"{value}\"");
                    return value; // 取得できた翻訳文字列を返す
                }
                LogHelper.LogWrite($"LangManager-> Lang.T() End (return \"{keyword}\" (No value.))");
                return keyword; // 見つからない場合はキーワードをそのまま返す
            }
        }

        /// <summary>
        /// 言語情報クラス(言語名、言語コード、言語名(英語)、言語名(日本語)
        /// </summary>
        public class LanguageInfo
        {
            [JsonPropertyName("Name")]                  // JSONプロパティ "Name"
            public string? Name { get; set; } = "";     // 言語名

            [JsonPropertyName("Code")]                  // JSONプロパティ "Code"
            public string? Code { get; set; } = "";     // 言語コード

            [JsonPropertyName("English_Name")]          // JSONプロパティ "English_Name"
            public string? En_Name { get; set; } = "";  // 言語名(英語)

            [JsonPropertyName("Japanese_Name")]         // JSONプロパティ "Japanese_Name"
            public string? Ja_Name { get; set; } = "";  // 言語名(日本語)

            public override string? ToString() => Name; // ToString()で言語名を返す
        }

        /// <summary>
        /// 言語情報(言語名、言語コード)リスト LanguagesInfoList を設定
        /// </summary>
        public static List<LanguageInfo> LanguagesInfoList { get; private set; } = [];

        /// <summary>
        /// "languages.json" ファイルを読み込み言語情報(言語名、言語コード、言語名(英語)、言語名(日本語))リストを作成
        /// </summary>
        /// <param name="jsonPath">Languages.jsonファイルのパス</param>
        public static void LoadLanguagesJson(string jsonPath)
        {
            LogHelper.LogWrite($"LangManager-> LoadLanguageJson() Start (File Path=\"{jsonPath}\")");
            if (!File.Exists(jsonPath))
            {// 言語ファイルが存在しない場合
                LogHelper.LogWrite($"\"languages.json\" file not found.");
                return;// 何もしない
            }
            // Languages.jsonファイルを読み込み
            string json = File.ReadAllText(jsonPath);
            // JSONデータをデシリアライズして言語情報リストに格納
            LanguagesInfoList = JsonSerializer.Deserialize<List<LanguageInfo>>(json)
                ?? [];
            LogHelper.LogWrite("LangManager-> LoadLanguageJson() \"languages.json\" file loaded. Created a \"LanguageInfoList\".");
        }

        /// <summary>
        /// 言語名から言語コードを取得
        /// </summary>
        /// <param name="langName">言語名</param>
        /// <returns>言語コード</returns>
        public static string GetNameToCode(string langName)
        {
            // 言語名に対応する言語コードを検索
            var lang = LanguagesInfoList.FirstOrDefault(l => l.Name == langName);
            return lang?.Code ?? SettingsManager.DefLangCode;       // 対応言語コードを返す (不明時はデフォルト値)
        }

        /// <summary>
        /// 言語名(英語)から言語コードを取得
        /// </summary>
        /// <param name="langEname">言語名(英語)</param>
        /// <returns>言語コード</returns>
        public static string GetEnameToCode(string langEname)
        {
            // 言語名(英語)に対応する言語コードを検索
            var lang = LanguagesInfoList.FirstOrDefault(l => l.En_Name == langEname);
            return lang?.Code ?? SettingsManager.DefLangCode;       // 対応言語コードを返す (不明時はデフォルト値)
        }

        /// <summary>
        /// 言語コードから言語名を取得
        /// </summary>
        /// <param name="langCode">言語コード</param>
        /// <returns>言語名</returns>
        public static string GetName(string langCode)
        {
            // 言語コードに対応する言語名を検索
            var lang = LanguagesInfoList.FirstOrDefault(l => l.Code == langCode);
            return lang?.Name ?? SettingsManager.DefLangName;       // 対応言語名を返す (不明時はデフォルト値)
        }

        /// <summary>
        /// 言語コードから言語名(英語)を取得
        /// </summary>
        /// <param name="langCode">言語コード</param>
        /// <returns>言語名(英語)</returns>
        public static string GetEname(string langCode)
        {
            // 言語コードに対応する言語名を検索
            var lang = LanguagesInfoList.FirstOrDefault(l => l.Code == langCode);
            return lang?.En_Name ?? SettingsManager.DefLangEname;   // 対応言語名(英語)を返す (不明時はデフォルト値)
        }

        /// <summary>
        /// 言語名から言語名(英語)を取得
        /// </summary>
        /// <param name="langName">言語名</param>
        /// <returns>言語名(英語)</returns>
        public static string GetNameToEname(string langName)
        {
            // 言語名に対応する言語名(英語)を検索
            var lang = LanguagesInfoList.FirstOrDefault(l => l.Name == langName);
            return lang?.En_Name ?? SettingsManager.DefLangEname;   // 対応言語名(英語)を返す (不明時はデフォルト値)
        }
    }
}