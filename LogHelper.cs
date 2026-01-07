//-------------------------------------------------------------------
// LogHelper.cs 
// ログ書き込みを行うヘルパークラス
//-------------------------------------------------------------------

namespace DriveIndicatorAI
{
    /// <summary>
    /// ログ書き込みを行うヘルパークラス
    /// </summary>
    public static class LogHelper
    {
#pragma warning disable IDE0330                         // IDE0330 メッセージを無効化 開始
        /// <summary>
        /// ログファイルアクセス用ロックオブジェクト
        /// </summary>
        private static readonly object lockObj = new();
#pragma warning restore IDE0330                         // IDE0330 メッセージを無効化 終了

        /// <summary>
        /// TEMP 内のログフォルダーのパス
        /// </summary>
        private static readonly string logFolderPath =
            Path.Combine(Path.GetTempPath(), "DriveIndicatorAI", "Logs");

        /// <summary>
        /// ログファイルのパス
        /// </summary>
        private static readonly string logFilePath = Path.Combine(logFolderPath, "MessagesLog.log");

        /// <summary>
        /// 一時ログファイルのパス
        /// </summary>
        private static readonly string oldLogPath = Path.Combine(logFolderPath, "MessagesLog.old");

        /// <summary>
        /// ログファイルの最大サイズ (1MB)
        /// </summary>
        private const long MaxLogSize = 1_000_000;

        /// <summary>
        /// ログフォルダーのパスを取得
        /// </summary>
        public static readonly string LogFolderPath = logFolderPath;

        /// <summary>
        /// ログを書き込む
        /// </summary>
        /// <param name="logMessage">ログに書き込むメッセージ</param>
        public static void LogWrite(string logMessage)
        {
            if (!SettingsManager.Current.LogEnabled)
            {// ログ無効時は何もしない
                return;
            }

            // ロックしてログファイルにアクセス
            lock (lockObj)
            {
                try
                {
                    // ログフォルダーを作成
                    Directory.CreateDirectory(logFolderPath);

                    if (File.Exists(logFilePath))
                    {// ログファイルが存在する場合
                     // ログファイル情報を取得
                        var info = new FileInfo(logFilePath);
                        if (info.Length >= MaxLogSize)
                        {// ログファイルサイズが最大値を以上である場合
                            File.Copy(logFilePath, oldLogPath, overwrite: true);    // 古いログとして保存
                            File.WriteAllText(logFilePath, "");                     // 新規ログとして空にする
                        }
                    }

                    // ログエントリの作成
                    string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {logMessage}";
                    // ログエントリをログファイルに追記
                    File.AppendAllText(logFilePath, entry + Environment.NewLine);
                }
                catch (Exception ex)
                {// ログ書き込み失敗時
                 //デバッグ出力に ログ書き込み失敗 と表示
                    System.Diagnostics.Debug.WriteLine($"Log write failed: {ex}");
                }
            }

        }

        /// <summary>
        /// ログファイル ("MessagesLog.log", "MessagesLog.old" をクリア(削除)する
        /// </summary>
        public static void ClearLog()
        {
            // ロックしてログファイルにアクセス
            lock (lockObj)
            {//
                try
                {
                    if (File.Exists(logFilePath))
                    {// ログファイルが存在する場合
                        File.Delete(logFilePath);   // ログファイルを削除
                    }
                    if (File.Exists(oldLogPath))
                    {// 古いログファイルが存在する場合
                        File.Delete(oldLogPath);    // 古いログファイルを削除
                    }
                }
                catch (Exception ex)
                {// ログクリア失敗時
                    // デバッグ出力に ログクリア失敗 と表示
                    System.Diagnostics.Debug.WriteLine($"Log clear failed: {ex.Message}");
                }
            }
        }
    }
}