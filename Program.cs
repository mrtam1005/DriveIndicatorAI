//-------------------------------------------------------------------
// Program.cs
// アプリケーションエントリーポイント
//-------------------------------------------------------------------

using System.Diagnostics;
using System.Security.Principal;

namespace DriveIndicatorAI
{
	/// <summary>
	/// アプリケーションエントリーポイントクラス
	/// </summary>
	internal static class Program
	{
		/// <summary>
		///  アプリケーションエントリーポイント
		/// </summary>
		[STAThread]
		static void Main()
		{
			// 管理者権限チェック
			if (!IsAdministrator())
			{// 管理者権限でない場合
			 // "DeiveIndicatorAI.exe" を再起動する
				Process.Start(new ProcessStartInfo
				{
					FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DriveIndicatorAI.exe"),
					UseShellExecute = true, // シェルを使用する
					Verb = "runas"          // 管理者権限を使用する
				});
				return; // 終了
			}

			// WindowsのDPIスケーリングに対応
			Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
			// Visual Stylesを有効化
			Application.EnableVisualStyles();
			// テキストレンダリングをデフォルト(GDI)に設定
			Application.SetCompatibleTextRenderingDefault(false);

			// ログ出力 Drive Indicator AI 起動
			LogHelper.LogWrite("Program-> Drive Indicator AI run.");

			// Languages.json(言語情報リスト)読み込み
			LangManager.LoadLanguagesJson(
				Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory,
					"Resources\\Language\\languages.json"
				)
			);

			// 言語ファイル読み込み
			LangManager.Lang.Load(SettingsManager.Current.LanguageCode);

			// 多重起動防止
			using var mutex = new Mutex(false, "DriveIndicatorAI_Mutex", out bool createdNew);
			if (!createdNew)
			{// すでに実行中である場合
			 // メッセージ表示 (多言語対応)
				MessageBox.Show(
					LangManager.Lang.T("AlreadyRunningMessage"),    // メッセージ 
					"Drive Indicator AI",                           // タイトル
					MessageBoxButtons.OK,                           // ボタン
					MessageBoxIcon.Information                      // アイコン
				);

				// ログ出力 Drive Indicator AI はすでに実行中です。(言語コード)
				LogHelper.LogWrite($"Program-> Drive Indicator AI is already running. ({SettingsManager.Current.LanguageCode})");
				return; // アプリケーション終了
			}

			// グローバル例外処理
			Application.ThreadException += (s, e) =>
			{// UIスレッドでキャッチされなかった例外
			 // ログ出力 スレッド例外 詳細情報
				LogHelper.LogWrite($"Program-> ThreadException failed.{e.Exception}");
			};

			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
			{// UIスレッド以外でキャッチされなかった例外
			 // ログ出力 未処理の例外 詳細情報
				LogHelper.LogWrite($"Program-> UnhandledException failed.{e.ExceptionObject}");
			};

			// TrayIconManager シングルトンインスタンス 初期化
			TrayIconManager.Instance.Initialize();

			// ログ出力 アプリケーションループ開始
			LogHelper.LogWrite("Program-> Application.Run Start");
			// アプリケーション実行
			Application.Run();
			// ログ出力 アプリケーションループ終了
			LogHelper.LogWrite("Program-> Application.Run Quit");
		}

		/// <summary>
		/// 管理者権限で実行しているか確認する
		/// </summary>
		/// <returns>true:管理者権限である false:管理者権限ではない</returns>
		static bool IsAdministrator()
		{
			// 現在のプロセスを実行しているユーザーの情報(ID)を取得
			var wi = WindowsIdentity.GetCurrent();
			// ユーザーが所持する権限を調べるオブジェクト作成
			var wp = new WindowsPrincipal(wi);
			// ユーザーが "Administrators グループ" に属しているか? の判定を返す
			// true:管理者権限, false:通常権限
			return wp.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}