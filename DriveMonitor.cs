//-------------------------------------------------------------------
// DriveMonitor.cs
// ドライブのアクセス状況を監視する
//-------------------------------------------------------------------

using System.Diagnostics;
using System.Runtime.Versioning;

namespace DriveIndicatorAI
{
    /// <summary>
    /// ドライブのアクセス状況を監視するクラス
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal class DriveMonitor
    {
        /// <summary>
        /// 非RAMドライブ読み取り監視用 PerformanceCounter 辞書
        /// </summary>
        private readonly Dictionary<char, PerformanceCounter> readCounters = [];
        /// <summary>
        /// 非RAMドライブ書き込み監視用 PerformanceCounter 辞書
        /// </summary>
        private readonly Dictionary<char, PerformanceCounter> writeCounters = [];

        /// <summary>
        /// RAMドライブ用 ETWベースのRAMドライブI/Oモニター
        /// </summary>
        private EtwRamIoMonitor? etw;
#pragma warning disable IDE0330                     // IDE0330 メッセージを無効化 開始
        /// <summary>
        /// ETWモニター用ロックオブジェクト
        /// </summary>
        private readonly object etwLock = new();
#pragma warning restore IDE0330                     // IDE0330 メッセージを無効化 終了

        /// <summary>
        /// RAMドライブ用 Dispose フラグ
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// PerformanceCounter 判定閾値 (Bytes/sec)
        /// </summary>
        public float PerfCounterThreshold { get; set; } = 0.5f;

        /// <summary>
        /// ドライブのアクセス状況を監視
        /// </summary>
        public DriveMonitor()
        {
            InitializePerformanceCounters();    // PerformanceCounter 初期化
            InitializeEtwMonitor();             // ETW モニター初期化    
        }

        /// <summary>
        /// LogicalDisk であるドライブの PerformanceCounter を作成し初期化する
        /// </summary>
        private void InitializePerformanceCounters()
        {
            try
            {
                // LogicalDisk であるPerformanceCounterCategory オブジェクトを作成
                var category = new PerformanceCounterCategory("LogicalDisk");
                // LogicalDisk のインスタンス名一覧を取得
                var instances = category.GetInstanceNames();

                foreach (char drive in SettingsManager.Current.MonitoredDrives)
                {// 監視対象ドライブごとに
                    // インスタンス名を作成
                    string instance = drive + ":";

                    if (instances.Contains(instance))
                    {// LogicalDisk インスタンスが存在する場合
                        try
                        {
                            // PerformanceCounter を作成
                            var read = new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec", instance);
                            var write = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec", instance);
                            // 初回 NextValue を呼んでおく (初回は 0 になりがち)
                            _ = read.NextValue();
                            _ = write.NextValue();
                            // 読み取り・書き込み辞書に登録
                            readCounters[drive] = read;
                            writeCounters[drive] = write;
                        }
                        catch (Exception ex)
                        {// PerformanceCounter が作成できない場合
                            LogHelper.LogWrite($"DriveMonitor-> InitializePerformanceCounters failed for drive {drive}. {ex.Message}");
                            // 何もせず続行 (ETW が拾う)
                        }
                        LogHelper.LogWrite($"{instance} is LogicalDisk. ");
                    }
                }
            }
            catch (Exception ex)
            {// LogicalDisk が取得できない場合
                LogHelper.LogWrite($"DriveMonitor-> InitializePerformanceCounters failed. {ex.Message}");
                // 何もせず続行 (ETW が拾う)
            }
        }

        /// <summary>
        /// ETW モニター作成し初期化する
        /// (RAM ドライブ用)
        /// </summary>
        private void InitializeEtwMonitor()
        {
            try
            {
                // 監視対象ドライブオブジェクトを作成
                var targetDrives = SettingsManager.Current.MonitoredDrives.ToArray();

                // ETW ベースの RAMドライブ I/Oモニター 初期化
                var monitor = new EtwRamIoMonitor(targetDrives);

                if (monitor.Start())
                {// ETW 開始が成功した場合
                    // ETW モニターをセット
                    etw = monitor;
                }
            }
            catch (Exception ex)
            {// ETW が使えない場合
                LogHelper.LogWrite($"DriveMonitor-> InitializeEtwMonitor failed. {ex.Message}");
                // 無視
            }
        }

        /// <summary>
        /// ドライブ状態辞書を作成
        /// </summary>
        /// <returns>ドライブ状態辞書(ドライブレター,読み取り状態(true/false),書き込み状態(true/false)</returns>
        public Dictionary<char, DriveStatus> GetDriveStatuses()
        {
            // ドライブ状態辞書を初期化
            var result = new Dictionary<char, DriveStatus>();

            foreach (char driveLetter in SettingsManager.Current.MonitoredDrives)
            {// 監視対象ドライブごとに
                // 読み取り・書き込みアクティブ判定フラグを初期化
                bool isRead = false;
                bool isWrite = false;

                if (readCounters.TryGetValue(driveLetter, out var readCounter) &&
                    writeCounters.TryGetValue(driveLetter, out var writeCounter))
                {// PerformanceCounter が初期化されている場合
                    try
                    {
                        // PerformanceCounter から読み取り・書き込みバイト数を取得
                        float r = readCounter.NextValue();
                        float w = writeCounter.NextValue();

                        // 閾値超過で読み取り・書き込みアクティブと判定
                        isRead = r > PerfCounterThreshold;
                        isWrite = w > PerfCounterThreshold;
                    }
                    catch (Exception ex)
                    {// PerformanceCounterが不正な場合
                        LogHelper.LogWrite($"DriveMonitor-> GetDriveStatuses failed for drive {driveLetter}. {ex.Message}");
                        // 無視 (ETW にフォールバック)
                    }
                }
                else
                {// PerformanceCounter が初期化されていない場合
                    // ETWモニター用ロックオブジェクトをロック
                    lock (etwLock)
                    {
                        if (etw != null)
                        {// ETWモニターが初期化されている場合
                            if (etw.ConsumeRead(driveLetter))
                            {// ETWで読み取りが検出された場合
                                isRead = true;  // 読み取りアクティブと判定
                            }
                            if (etw.ConsumeWrite(driveLetter))
                            {// ETWで書き込みが検出された場合
                                isWrite = true; // 書き込みアクティブと判定
                            }
                        }
                    }
                }
                // ドライブ状態辞書に各ドライブの状態を書き込む
                result[driveLetter] = new DriveStatus
                {
                    DriveLetter = driveLetter,  // ドライブ文字
                    IsReadActive = isRead,      // 読み取り状態 書き込み
                    IsWriteActive = isWrite     // 書き込み状態 書き込み
                };
            }
            // ドライブ状態辞書を返す
            return result;
        }

        /// <summary>
        /// PerformanceCounter と ETW モニターを再初期化する
        /// </summary>
        public void Reload()
        {
            Dispose();                          // 既存の PerformanceCounter と ETW を破棄
            InitializePerformanceCounters();    // PerformanceCounter 初期化
            InitializeEtwMonitor();             // ETW モニター初期化
        }

        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            if (disposed)
            {// 解放済みの場合
                return;   // 何もせず終了
            }
            foreach (var c in readCounters.Values)  // 読み取りカウンターの解放
                try
                {
                    c.Dispose();    // 読み取りカウンターの解放
                }
                catch (Exception ex)
                {// 読み取りカウンターの解放に失敗した場合
                    LogHelper.LogWrite($"DriveMonitor-> Dispose readCounters failed. {ex.Message}");
                    // 無視
                }

            foreach (var c in writeCounters.Values)  // 書き込みカウンターの解放
                try
                {
                    c.Dispose();    // 書き込みカウンターの解放
                }
                catch (Exception ex)
                {// 書き込みカウンターの解放に失敗した場合
                    LogHelper.LogWrite($"DriveMonitor-> Dispose writeCounters failed. {ex.Message}");
                    // 無視
                }

            readCounters.Clear();   // 読み取りカウンター辞書のクリア
            writeCounters.Clear();  // 書き込みカウンター辞書のクリア

            // ETWモニターロックオブジェクト           
            lock (etwLock)
            {
                try
                {
                    etw?.Dispose(); // ETWモニターの解放
                }
                catch (Exception ex)
                {// ETWモニターの解放に失敗した場合
                    LogHelper.LogWrite($"DriveMonitor-> Dispose etw failed. {ex.Message}");
                    // 無視
                }
                etw = null; // ETWモニターのクリア
            }
            disposed = true;    // 解放済みフラグを設定
        }
    }
}