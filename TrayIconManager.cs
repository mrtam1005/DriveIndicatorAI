//-------------------------------------------------------------------
// TrayIconManager.cs
// 通知領域アイコンの表示・更新・右クリックメニューの管理
//-------------------------------------------------------------------

using System.Diagnostics;

namespace DriveIndicatorAI
{
    /// <summary>
    /// 通知領域アイコン管理クラス
    /// </summary>
    public class TrayIconManager : IDisposable
    {
        private readonly List<NotifyIcon> notifyIcons = []; // 通知領域アイコンのリスト
        private ContextMenuStrip? sharedMenu;               // 共有右クリックメニュー
        private DriveMonitor? monitor;                      // 再利用インスタンス
        private IconRenderer? renderer;                     // アイコンレンダラー
        private System.Windows.Forms.Timer? updateTimer;    // 更新用タイマー
        private string tempIconRoot = "";                   // TEMP アイコンフォルダーパス
        private bool disposed = false;                      // Dispose フラグ
        private SettingsForm? settingsForm;                 // 設定フォーム

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static TrayIconManager Instance { get; } = new TrayIconManager();

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            LogHelper.LogWrite("TrayIconManager-> Initialize() Start");
            PrepareTempIcons();                     // TEMP にアイコンをコピー
            monitor = new DriveMonitor();           // 再利用インスタンスを作成
            renderer = new IconRenderer(GetDpi());  // アイコンレンダラーを作成
            sharedMenu = CreateContextMenu();       // 共有メニューを作成

            // 更新タイマーを作成
            updateTimer = new System.Windows.Forms.Timer
            {
                Interval = Math.Max(1, SettingsManager.Current.DisplayInterval)  // ミリ秒単位で更新間隔を取得
            };

            updateTimer.Tick += (_, __) => SafeUpdateIcon();    // タイマーイベントハンドラを設定
            updateTimer.Start();                                // タイマー開始
            Application.ApplicationExit += (_, __) => Dispose();// アプリケーション終了時にクリーンアップ

            SafeUpdateIcon();// 初回更新
            LogHelper.LogWrite("TrayIconManager-> Initialize() End");
        }

        /// <summary>
        /// DPI 値を取得する
        /// </summary>
        /// <returns></returns>
        private static float GetDpi()
        {
            LogHelper.LogWrite("TrayIconManager-> GetDpi() Start");
            // Graphics オブジェクトを取得
            using var g = Graphics.FromHwnd(IntPtr.Zero);
            // DPI を返す
            LogHelper.LogWrite($"TrayIconManager-> GetDpi() End (return DPI={g.DpiX})");
            return g.DpiX;
        }

        /// <summary>
        /// 一時フォルダー (TEMP) にアイコンフォルダーをコピーする
        /// </summary>
        private void PrepareTempIcons()
        {
            LogHelper.LogWrite("TrayIconManager-> PrepareTempIcons() Start");
            // アプリケーション内のアイコンフォルダーパス
            string source = SettingsManager.Current.IconFolderPath;
            // 一時アイコンフォルダーパスを取得
            tempIconRoot = Path.Combine(Path.GetTempPath(), "DriveIndicatorAI", "Icons");

            if (!Directory.Exists(tempIconRoot))
            {// TEMP アイコンフォルダーが存在しない場合
                // TEMP アイコンフォルダーを作成
                Directory.CreateDirectory(tempIconRoot);
            }

            if (!Directory.Exists(source))
            {// 元アイコンフォルダーが存在しない場合
                LogHelper.LogWrite("TrayIconManager-> PrepareTempIcons() Icons folder does not exist.");
                return; // 何もしない
            }

            foreach (string dpi in new[] { "16", "32" })
            {// 解像度フォルダーごとに処理
                // コピー元のアイコンフォルダー
                string src = Path.Combine(source, dpi);
                // コピー先のアイコンフォルダー(TEMP)
                string dst = Path.Combine(tempIconRoot, dpi);

                // コピー先のアイコンフォルダー(TEMP)を作成
                Directory.CreateDirectory(dst);

                if (!Directory.Exists(src))
                {// コピー元のアイコンフォルダーが存在しない場合
                    continue;   // スキップ
                }

                foreach (var file in Directory.GetFiles(src, "*.png"))
                {// PNG ファイルすべてに対し
                    try
                    {
                        // 同一ファイル名で上書きコピー
                        File.Copy(file, Path.Combine(dst, Path.GetFileName(file)), overwrite: true);
                    }
                    catch (Exception ex)
                    {// PNG ファイルコピー失敗時
                        LogHelper.LogWrite($"TrayIconManager-> Failed to copy icon file: {ex.Message}");
                        // 無視
                    }
                }
            }
            LogHelper.LogWrite("TrayIconManager-> PrepareTempIcons() End");
        }

        /// <summary>
        /// 右クリックメニューを作成する
        /// </summary>
        /// <returns>作成したメニュー</returns>
        private ContextMenuStrip CreateContextMenu()
        {
            LogHelper.LogWrite("TrayIconManager-> CreateContextMenu() Start");
            // コンテキストメニューを作成
            var menu = new ContextMenuStrip();

            // メニュー項目追加ヘルパー
            void Add(string text, Action action)
            {
                menu.Items.Add(text, null, (_, __) => action());
            }

            // メニューを作成（多言語対応）
            Add(LangManager.Lang.T("MenuSetting"), ShowSettings);           // "設定" メニュー
            Add(LangManager.Lang.T("MenuQuit"), () => Application.Exit());  // "終了" メニュー

            LogHelper.LogWrite("TrayIconManager-> CreateContextMenu() End (return menu)");
            return menu;    // 作成したメニューを返す
        }

        /// <summary>
        /// 例外を吸収してタイマー停止やクラッシュを防ぎアイコンを更新する
        /// </summary>
        private void SafeUpdateIcon()
        {
            try
            {
                UpdateIcon(); // アイコンの更新処理
            }
            catch (Exception ex)
            {// 例外が発生した場合
                LogHelper.LogWrite($"TrayIconManager-> SafeUpdateIcon() Failed to update icon: {ex.Message}");
                // 無視
            }
        }

        /// <summary>
        /// アイコンの更新処理をする
        /// </summary>
        private void UpdateIcon()
        {
            if (monitor == null || renderer == null)
            {// モニターまたはレンダラーが初期化されていない場合
                return; // 何もしない
            }

            // monitor は Initialize で作成済み
            var statuses = monitor!.GetDriveStatuses()      // ドライブ状態辞書を取得
                                  .OrderBy(kv => kv.Key)    // ドライブ文字でソート
                                  .Select(kv => kv.Value)   // ドライブ状態リストに変換
                                  .ToList();                // リスト化

            // IconRenderer は再利用 (内部で DPI 固定)
            var icons = renderer!.RenderIcons(statuses); // 2ドライブずつ分割描画

            // アイコン数が変わった場合は全入れ替え
            if (icons.Count != notifyIcons.Count)
            {// アイコン数に変化があった場合
                foreach (var nIcons in notifyIcons)
                {// 既存の 通知領域アイコン 全てに対し
                    try
                    {
                        nIcons.Visible = false;         // 非表示にしてから
                        nIcons.ContextMenuStrip = null; // コンテキストメニューを解除
                        nIcons.Dispose();               // 破棄
                    }
                    catch (Exception ex)
                    {// コンテキストメニュー解除や破棄が不正な場合
                        LogHelper.LogWrite($"TrayIconManager-> UpdateIcon() Failed to dispose NotifyIcon: {ex.Message}");
                        // 無視
                    }
                }
                notifyIcons.Clear();    // 通知領域アイコンリストをクリア
                LogHelper.LogWrite("TrayIconManager-> UpdateIcon() Cleared the NotifyIcons list.");

                for (int i = 0; i < icons.Count; i++)
                {// アイコン数分ループ
                    // 通知領域アイコンリスト作成
                    var nIcon = new NotifyIcon
                    {
                        Icon = icons[i],                // アイコンを設定
                        Visible = true,                 // 表示を有効化
                        Text = $"Drive Indicator AI",   // ツールチップを設定
                        ContextMenuStrip = sharedMenu   // 共有メニューを設定
                    };
                    // リストに追加
                    notifyIcons.Add(nIcon);
                    LogHelper.LogWrite($"TrayIconManager-> UpdateIcon() Added NotifyIcons list. Total Notifyicons={notifyIcons.Count}");
                }
            }
            else
            {// アイコン数に変化がなかった場合
                for (int i = 0; i < icons.Count; i++)
                {// アイコン数分ループ
                    try
                    {
                        int newIndex = icons.Count - 1 - i;     // 通知領域アイコン逆順表示用インデックスを作成
                        var old = notifyIcons[i].Icon;          // 旧アイコンを保存
                        notifyIcons[i].Icon = icons[newIndex];  // 新アイコンを設定
                        old?.Dispose();                         // 旧アイコンを破棄
                    }
                    catch (Exception ex)
                    {// アイコン設定が不正な場合
                        LogHelper.LogWrite($"TrayIconManager-> UpdateIcon() Failed to update NotifyIcon: {ex.Message}");
                        // 無視
                    }
                }
            }
        }

        /// <summary>
        /// SettingsForm を表示する
        /// </summary>
        public void ShowSettings()
        {
            LogHelper.LogWrite("TrayIconManager-> ShowSettings() Start");
            if (settingsForm == null || settingsForm.IsDisposed)
            {// 設定ウインドウを開いていない場合
                // 設定ウインドウを作成
                settingsForm = new SettingsForm();
                // 設定ウインドウを表示
                settingsForm.FormClosed += (s, e) => settingsForm = null;
                settingsForm.Show();
                LogHelper.LogWrite("TrayIconManager-> ShowSettings() Opened Settings Form.");
            }
            else
            {// 設定ウインドウを開いている場合
                if (settingsForm.WindowState == FormWindowState.Minimized)
                {// 設定ウインドウを最小化している場合
                    // 設定ウインドウを通常表示に戻す
                    settingsForm.WindowState = FormWindowState.Normal;
                }
                // 設定ウインドウをアクティブにする
                settingsForm.Activate();
                LogHelper.LogWrite("TrayIconManager-> ShowSettings() Activated Settings Form.");
            }
            LogHelper.LogWrite("TrayIconManager-> ShowSettings() End");
        }

        /// <summary>
        /// ドライブモニターの再読み込み
        /// </summary>
        public void ReloadMonitor()
        {
            LogHelper.LogWrite("TrayIconManager-> ReloadMonitor() Start");
            if (monitor == null)
            {// モニターが初期化されていない場合
                LogHelper.LogWrite("TrayIconManager-> ReloadMonitor() Monitor not initialized.");
                return; // 何もしない
            }

            // DriveMonitor を再初期化
            monitor.Reload();
            LogHelper.LogWrite("TrayIconManager-> ReloadMonitor() DriveMonitor reloaded.");

            // 共有メニューを再作成
            sharedMenu = CreateContextMenu();
            LogHelper.LogWrite("TrayIconManager-> ReloadMonitor() ContextMenu reloaded.");

            // 表示サイクルを更新
            if (updateTimer != null)
            {// 表示サイクルのタイマーが初期化されている場合
                // タイマー間隔を更新
                updateTimer.Interval = Math.Max(1, SettingsManager.Current.DisplayInterval);
                LogHelper.LogWrite("TrayIconManager-> ReloadMonitor() DisplayInterval reloaded.");
            }

            foreach (var n in notifyIcons)
            {// 既存の 通知領域アイコン 全てに対し
                try
                {
                    n.Visible = false;  // 通知領域アイコンを非表示
                    n.Dispose();        // 通知領域アイコンを破棄
                    LogHelper.LogWrite("TrayIconManager-> ReloadMonitor() Disposed old NotifyIcon.");
                }
                catch (Exception ex)
                {// 通知領域アイコンの破棄が不正な場合
                    LogHelper.LogWrite($"TrayIconManager-> ReloadMonitor() Failed to dispose NotifyIcon: {ex.Message}");
                    // 無視
                }
            }
            // 通知領域アイコンリストをクリア
            notifyIcons.Clear();
            LogHelper.LogWrite("TrayIconManager-> ReloadMonitor() Cleared the NotifyIcons list.");

            // アイコンを更新
            SafeUpdateIcon();
            LogHelper.LogWrite("TrayIconManager-> ReloadMonitor() End");
        }

        /// <summary>
        /// Dispose パターンで確実にリソースを解放
        /// </summary>
        public void Dispose()
        {
            LogHelper.LogWrite("TrayIconManager-> Dispose() Start");
            if (disposed) return;       // すでに破棄済みの場合は何もしない
            disposed = true;            // 破棄済みフラグを立てる
            GC.SuppressFinalize(this);  // ファイナライザの呼び出しを抑制

            // タイマーの破棄
            try
            {
                updateTimer?.Stop();    // タイマー停止
                updateTimer?.Dispose(); // タイマー破棄
            }
            catch (Exception ex)
            {// タイマーの破棄に失敗した場合
                LogHelper.LogWrite($"TrayIconManager-> Failed to dispose Timer: {ex.Message}");
                // 無視
            }

            // NotifyIcon の破棄
            foreach (var nIcons in notifyIcons)
            {// 既存の 通知領域アイコン 全てに対し
                try
                {
                    nIcons.Visible = false;         // 非表示にしてから
                    nIcons.ContextMenuStrip = null; // コンテキストメニューを解除
                    nIcons.Dispose();               // 破棄   
                }
                catch (Exception ex)
                {// 通知領域アイコンの破棄が不正な場合
                    LogHelper.LogWrite($"TrayIconManager-> Failed to dispose NotifyIcon: {ex.Message}");
                    // 無視
                }
            }
            // 通知領域アイコンリストをクリア
            notifyIcons.Clear();
            LogHelper.LogWrite("TrayIconManager-> Dispose() Cleared the NotifyIcons list");

            // IconRenderer の破棄
            try
            {
                renderer?.Dispose();    // renderer を破棄
            }
            catch (Exception ex)
            {// IconRederer の破棄が不正な場合
                LogHelper.LogWrite($"TrayIconManager-> Failed to dispose IconRenderer: {ex.Message}");
                // 無視
            }

            // DriveMonitor の破棄
            try
            {
                monitor?.Dispose();  // monitor を破棄
            }
            catch (Exception ex)
            {// DriveMonitor の破棄が不正な場合
                LogHelper.LogWrite($"TrayIconManager-> Failed to dispose DriveMonitor: {ex.Message}");
                // 無視
            }

            // FontHelper のフォントキャッシュをクリア
            try
            {
                FontHelper.ClearCache();    // FontHelper のキャッシュをクリア
            }
            catch (Exception ex)
            {// FontHelper のキャッシュクリアが不正な場合
                LogHelper.LogWrite($"TrayIconManager-> Failed to clear FontHelper cache: {ex.Message}");
                //無視
            }
            LogHelper.LogWrite("TrayIconManager-> Dispose() End (Completed disposing resources.)");
        }
    }
}