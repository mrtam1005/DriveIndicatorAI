//-------------------------------------------------------------------
// StartupHelper.cs
// Windows 起動時の自動実行タスクを登録・解除する
//-------------------------------------------------------------------

using System.Diagnostics;
using System.Text;

namespace DriveIndicatorAI
{
    /// <summary>
    /// Windows 起動時の自動実行タスクを登録・解除するヘルパークラス
    /// </summary>
    public static class StartupHelper
    {
        // タスク名を設定
        private const string TaskName = "DriveIndicatorAI_AutoStart";

        /// <summary>
        /// 自動起動タスクを登録
        /// </summary>
        public static void Register()
        {
            try
            {
                // 実行ファイルのパスを取得
                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DriveIndicatorAI.exe");
                LogHelper.LogWrite($"StartupHelper-> Register() ExeFilePath:\"{exePath}\"");
                if (string.IsNullOrWhiteSpace(exePath) || !File.Exists(exePath))
                {//実行ファイルパスが不正 または 実行ファイルが見つからない場合
                    LogHelper.LogWrite($"StartupHelper-> \"Exe file\" not Found. ExeFilePath:\"{exePath}\"");
                    return; // 登録しない
                }
                // タスクの設定用 XMLファイルパスの取得
                string xmlPath = Path.Combine(Path.GetTempPath(), "DriveIndicatorAI\\DriveIndicatorAI_Task.xml");
                LogHelper.LogWrite($"StartupHelper-> Register() XmlFilePath:\"{xmlPath}\"");

                // XMLテキストを作成
                string xml =
$@"<?xml version=""1.0"" encoding=""UTF-16""?>
<Task version=""1.4"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
  <RegistrationInfo>
    <Date>{DateTime.Now:yyyy-MM-ddTHH:mm:ss}</Date>
    <Author>DriveIndicatorAI</Author>
    <Description>DriveIndicatorAI Auto Start</Description>
  </RegistrationInfo>

  <Triggers>
    <LogonTrigger>
      <Enabled>true</Enabled>
      <Delay>PT10S</Delay>
    </LogonTrigger>
  </Triggers>

  <Principals>
    <Principal id=""Author"">
      <RunLevel>HighestAvailable</RunLevel>
    </Principal>
  </Principals>

  <Settings>
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>

    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
    <RunOnlyIfIdle>false</RunOnlyIfIdle>
    <WakeToRun>false</WakeToRun>

    <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
    <Priority>7</Priority>
  </Settings>

  <Actions Context=""Author"">
    <Exec>
      <Command>""{exePath}""</Command>
    </Exec>
  </Actions>
</Task>";

                // XMLテキスト を XMLファイル に書き出し
                File.WriteAllText(xmlPath, xml, Encoding.Unicode);

                // schtasks.exe の引数を設定
                string args =
                    "/Create " +                // タスク作成
                    $"/TN \"{TaskName}\" " +    // タスク名
                    $"/XML \"\"{xmlPath}\"\" " +    // タスク設定XMLファイル指定
                    "/F ";                      // 強制
                // schtasks.exe を実行してタスクを登録
                RunSchTasks(args);
                LogHelper.LogWrite("StartupHelper-> Register() Task registered");
            }
            catch (Exception ex)
            {// 自動起動タスクの登録に失敗
                LogHelper.LogWrite($"StartupHelper-> Register() Register failed.{ex.Message}");
            }   // 失敗してもアプリは動作可能
        }

        /// <summary>
        /// 自動起動タスクを削除
        /// </summary>
        public static void Unregister()
        {
            try
            {
                // schtasks.exe の引数を設定
                string args =
                    "/Delete " +                // タスク削除
                    $"/TN \"{TaskName}\" " +    // タスク名
                    "/F";                       // 強制
                // schtasks.exe を実行してタスクを削除
                RunSchTasks(args);
                LogHelper.LogWrite("StartupHelper-> Unregister() Task unregistered");
            }
            catch (Exception ex)
            {// 自動起動タスクの削除に失敗
                LogHelper.LogWrite($"StartupHelper-> Unregister() failed.{ex.Message}");
            }   // 失敗してもアプリは動作可能
        }

        /// <summary>
        /// schtasks.exe を実行
        /// </summary>
        /// <param name="arguments">schtasks.exe の引数</param>
        private static void RunSchTasks(string arguments)
        {
            LogHelper.LogWrite("StartupHelper-> RunSchTask() Start");
            try
            {
                // schtasks.exe のプロセス情報を設定
                var psi = new ProcessStartInfo
                {
                    FileName = "schtasks.exe",                  // 実行ファイル名
                    Arguments = arguments,                      // 引数
                    CreateNoWindow = true,                      // コンソールウィンドウを表示しない
                    UseShellExecute = false,                    // シェルを使用しない
                    Verb = "",                                  // 通常の権限で実行
                    RedirectStandardOutput = true,              // 標準出力を読む
                    RedirectStandardError = true,               // 標準エラーを読む
                    WindowStyle = ProcessWindowStyle.Hidden     // ウィンドウスタイルを非表示
                };

                // schtasks.exe を実行
                using var proc = Process.Start(psi);
                // 成功メッセージを取得
                string output = proc?.StandardOutput.ReadToEnd().Trim() ?? "";
                // エラーメッセージを取得
                string error = proc?.StandardError.ReadToEnd().Trim() ?? "";
                // 最大3秒待機して終了
                proc?.WaitForExit(3000);

                // ログに成功メッセージを書き込む                
                LogHelper.LogWrite($"StartupHelper-> RunSchTask() schtasks.exe output:{output}");
                // ログにエラーメッセージを書き込む
                LogHelper.LogWrite($"StartupHelper-> RunSchTask() schtasks.exe error:{error}");
            }
            catch (Exception ex)
            {// schtasks.exe の実行に失敗
                LogHelper.LogWrite($"StartupHelper-> RunSchTasks() failed.{ex.Message}");
            }   // 失敗してもアプリは動作可能
            LogHelper.LogWrite("StartupHelper-> RunSchTask() End");
        }
    }
}