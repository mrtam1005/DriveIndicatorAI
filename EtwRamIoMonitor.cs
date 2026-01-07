//-------------------------------------------------------------------
// EtwRamIoMonitor.cs
// ETWを使用してRAMディスクのファイルI/Oを監視する
//-------------------------------------------------------------------

using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System.Collections.Concurrent;

namespace DriveIndicatorAI
{
    /// <summary>
    /// ETWを使用してRAMドライブのファイルI/Oを監視するクラス
    /// (安全な形でしか利用できない + 自動破棄される)
    /// </summary>
    /// <param name="driveLetters">ドライブレター配列</param>
    public sealed class EtwRamIoMonitor(params char[] driveLetters) : IDisposable
    {
        /// <summary>
        /// 監視対象ドライブ文字を大文字に変換し配列として展開
        /// </summary>
        private readonly char[] targets = [.. driveLetters.Select(char.ToUpperInvariant)];
        /// <summary>
        /// ETWセッション
        /// </summary>
        private TraceEventSession? session;
        /// <summary>
        /// ETWリーダースレッド
        /// </summary>
        private Thread? readerThread;
        /// <summary>
        /// キャンセレーショントークン
        /// </summary>
        private readonly CancellationTokenSource cts = new();
        /// <summary>
        /// 読み取りフラグ
        /// </summary>
        private readonly ConcurrentDictionary<char, bool> readFlags = [];
        /// <summary>
        /// 書き込みフラグ
        /// </summary>
        private readonly ConcurrentDictionary<char, bool> writeFlags = [];
        /// <summary>
        /// 開始済みフラグ
        /// </summary>
        private volatile bool started;

        /// <summary>
        /// ETWセッションを開始する
        /// </summary>
        /// <returns>true:セッション開始済み、false:セッション開始失敗</returns>
        public bool Start()
        {
            if (started) return true;   // すでに開始済みだった場合、開始済みを返す

            try
            {
                // ETWセッションの作成
                string sessionName = "DriveIndicatorAI-FileIo";

                // 既存セッションが残っている場合は削除
                try
                {
                    using var existing = new TraceEventSession(sessionName);
                    existing.Stop();
                }
                catch { }   // 存在しない場合は無視

                // セッション名が重複する場合に備えて一意の名前を生成
                session = new TraceEventSession(sessionName, TraceEventSessionOptions.Create)
                {
                    StopOnDispose = true    // Dispose時にセッションを停止
                };

                // カーネルプロバイダーの有効化（ファイルI/Oイベントを監視）
                session.EnableKernelProvider(
                    KernelTraceEventParser.Keywords.FileIOInit |    // 初期化イベントも有効化
                    KernelTraceEventParser.Keywords.FileIO          // ファイルI/Oイベントも有効化
                );

                // ETWリーダースレッドの開始
                readerThread = new Thread(() => Process(cts.Token))
                {
                    IsBackground = true,        // バックグラウンドスレッドとして実行
                    Name = "ETW-FileIo-Reader"  // スレッド名を設定
                };
                readerThread.Start();   // スレッド開始

                started = true;         // 開始済みフラグを設定
                return true;            // 開始済みを返す
            }
            catch
            {// エラー発生時
                Stop();         // 監視停止
                return false;   // 開始失敗を返す
            }
        }

        /// <summary>
        /// ETWイベントの処理
        /// </summary>
        /// <param name="token">キャンセル状態を共有するための"読み取り専用フラグ"</param>
        private void Process(CancellationToken token)
        {
            if (session == null) return;    // セッションが無効な場合は終了

            // ファイルI/O読み取りイベントの処理
            session.Source.Kernel.FileIORead += data =>
            {
                if (token.IsCancellationRequested) return;      // キャンセル要求があった場合は処理を中止
                var d = ExtractDrive(data.FileName);            // ドライブ文字を抽出
                if (d.HasValue && targets.Contains(d.Value))    // 監視対象ドライブの場合
                    readFlags[d.Value] = true;                  // 読み取りフラグを設定
            };

            // ファイルI/O書き込みイベントの処理
            session.Source.Kernel.FileIOWrite += data =>
            {
                if (token.IsCancellationRequested) return;      // キャンセル要求があった場合は処理を中止
                var d = ExtractDrive(data.FileName);            // ドライブ文字を抽出
                if (d.HasValue && targets.Contains(d.Value))    // 監視対象ドライブの場合
                    writeFlags[d.Value] = true;                 // 書き込みフラグを設定
            };

            // リアルタイム処理
            session.Source.Process();
        }

        /// <summary>
        /// パスからドライブ文字を抽出する
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>A～Z(ドライブレター) または null</returns>
        private static char? ExtractDrive(string? path)
        {
            if (string.IsNullOrEmpty(path)) return null;    // 無効なパスの場合はnullを返す
            if (path.StartsWith(@"\\?\")) path = path[4..]; // 長いパスプレフィックスを削除
            if (path.Length >= 2 && path[1] == ':')
            {// ドライブレター形式の場合
                char d = char.ToUpperInvariant(path[0]);    // ドライブ文字を大文字に変換
                return (d >= 'A' && d <= 'Z') ? d : null;   // A-Zの範囲内であれば返す
            }
            return null; // UNC等は対象外
        }

        /// <summary>
        /// 読み取りフラグの取得
        /// </summary>
        /// <param name="driveLetter">A～Z(ドライブレター)</param>
        /// <returns>true:読み取り中、false:読み取り停止中</returns>
        public bool ConsumeRead(char driveLetter)
        {
            driveLetter = char.ToUpperInvariant(driveLetter);   // 大文字に変換
            return readFlags.TryRemove(driveLetter, out _);     // フラグを削除して存在を返す
        }

        /// <summary>
        /// 書き込みフラグの取得
        /// </summary>
        /// <param name="driveLetter">A～Z(ドライブレター)</param>
        /// <returns>true:書き込み中、false:書き込み停止中</returns>
        public bool ConsumeWrite(char driveLetter)
        {
            driveLetter = char.ToUpperInvariant(driveLetter);   // 大文字に変換
            return writeFlags.TryRemove(driveLetter, out _);    // フラグを削除して存在を返す
        }

        /// <summary>
        /// ETWセッション停止
        /// </summary>
        public void Stop()
        {
            try
            {
                cts.Cancel();       // キャンセル要求を送信
                session?.Dispose(); // セッションを破棄（停止）
            }
            catch { }   // 不正処理時は無視
            finally
            {// 必ず実行されるブロック
                started = false;        // 開始済みフラグをクリア
                session = null;         // セッションを無効化
                readerThread = null;    // スレッドを無効化
            }
        }

        /// <summary>
        /// Dispose (監視停止)
        /// </summary>
        public void Dispose() => Stop();
    }
}