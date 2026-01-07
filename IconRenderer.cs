//-------------------------------------------------------------------
// IconRenderer.cs
// ドライブ状態に応じたアイコン画像の合成と描画 (DPI対応含む)
//-------------------------------------------------------------------

using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DriveIndicatorAI
{
    /// <summary>
    /// ドライブ状態に応じたアイコン画像の合成と描画をするクラス
    /// </summary>
    /// <param name="dpiValue">解像度[dpi]</param>
    [SupportedOSPlatform("windows")]
    // 
    public partial class IconRenderer(float dpiValue) : IDisposable
    {
        /// <summary>
        /// TEMP にコピーされたアイコンフォルダーのルートパス
        /// </summary>
        private readonly string tempIconRoot = Path.Combine(Path.GetTempPath(), "DriveIndicatorAI", "Icons");
        /// <summary>
        /// 解像度を取得
        /// </summary>
        private readonly float dpi = dpiValue;
        /// <summary>
        /// 破棄フラグ
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// アイコンのサイズ        解像度値が閾値以上なら32x32、未満なら16x16
        /// </summary>
        public Size IconSize => dpi >= SettingsManager.SizeChangeDpi ? new Size(32, 32) : new Size(16, 16);
        /// <summary>
        /// 1ドライブ分の画像サイズ 解像度値が閾値以上なら16x32、未満なら 8x16
        /// </summary>
        public Size DriveSize => dpi >= SettingsManager.SizeChangeDpi ? new Size(16, 32) : new Size(8, 16);
        /// <summary>
        /// ドライブ文字描画サイズ  解像度値が閾値以上なら16x16、未満なら 8x 8
        /// </summary>
        public Size LetterSize => dpi >= SettingsManager.SizeChangeDpi ? new Size(16, 16) : new Size(8, 8);

        /// <summary>
        /// 複数ドライブの状態からアイコンリストを生成
        /// </summary>
        /// <param name="drives">ドライブ状態リスト</param>
        /// <returns>アイコンリスト</returns>
        public List<Icon> RenderIcons(List<DriveStatus> drives)
        {
            // アイコンリストを作成
            var icons = new List<Icon>();
            for (int i = 0; i < drives.Count; i += 2)
            {// ドライブ数に応じて2つずつ処理
                // ドライブ状態のペアを取得
                var pair = drives.GetRange(i, Math.Min(2, drives.Count - i));
                // アイコンを描画してリストに追加
                icons.Add(RenderIcon(pair));
            }
            return icons;   // アイコンリストを返す
        }

        /// <summary>
        /// 単一アイコンの描画 
        /// </summary>
        /// <param name="drives">ドライブ状態リスト</param>
        /// <returns>アイコンオブジェクト(クローン)</returns>
        // 
        public Icon RenderIcon(List<DriveStatus> drives)
        {
            // 描画用ビットマップを作成
            using Bitmap bmp = new(IconSize.Width, IconSize.Height);
            try
            {
                // グラフィックスオブジェクトを取得
                using var g = Graphics.FromImage(bmp);
                // 描画品質設定
                g.CompositingMode = CompositingMode.SourceOver;             // 元の画像を保持しつつ描画
                g.CompositingQuality = CompositingQuality.HighQuality;      // 高品質合成
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; // 高品質補間
                g.SmoothingMode = SmoothingMode.AntiAlias;                  // アンチエイリアス
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;   // クリアタイプテキスト

                // 透明背景でクリア
                g.Clear(Color.Transparent);

                for (int i = 0; i < drives.Count; i++)
                {// 表示ドライブすべてに対して
                    // ドライブ状態を取得
                    var status = drives[i];
                    // ドライブ状態に応じた画像を読み込み
                    using var img = LoadDriveImage(status);
                    // ドライブ画像を描画
                    g.DrawImage(img, i * DriveSize.Width, 0, DriveSize.Width, DriveSize.Height);

                    // ドライブ文字のフォントを取得
                    var font = FontHelper.GetDriveLetterFontCache(dpi);
                    // ドライブ文字の色を取得
                    using var brush = new SolidBrush(ColorTranslator.FromHtml(SettingsManager.Current.DriveLetterColor));
                    // ドライブ文字描画用の矩形サイズを取得
                    var rect = new Rectangle(i * DriveSize.Width, 0, LetterSize.Width, LetterSize.Height);
                    // ドライブ文字のフォーマットを中央揃えに設定
                    var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                    // ドライブ文字を描画
                    g.DrawString(status.DriveLetter.ToString(), font, brush, rect, format);
                }

                // ビットマップ形式でアイコンを取得
                IntPtr hIcon = bmp.GetHicon();
                try
                {
                    using Icon icon = Icon.FromHandle(hIcon);   // アイコンオブジェクトを作成
                    return (Icon)icon.Clone();                  // アイコンをクローン化して返す
                }
                finally
                {// 必ず実行されるブロック
                    DestroyIcon(hIcon);                         // アイコンハンドルを破棄
                }
            }
            catch
            {// 単一アイコンの描画に失敗した場合
                // 代替の空アイコンを作成
                Bitmap fallback = new(IconSize.Width, IconSize.Height);
                try
                {
                    // 代替アイコンのハンドルを取得
                    IntPtr hIcon = fallback.GetHicon();
                    try
                    {
                        using Icon icon = Icon.FromHandle(hIcon);   // アイコンオブジェクトを作成
                        return (Icon)icon.Clone();                  // アイコンをクローン化して返す
                    }
                    finally
                    {// 必ず実行されるブロック
                        DestroyIcon(hIcon);                         // アイコンハンドルを破棄
                    }
                }
                finally
                {// 必ず実行されるブロック
                    fallback.Dispose(); // 代替ビットマップを破棄
                }
            }
            finally
            {// 必ず実行されるブロック
                bmp.Dispose();  // ビットマップを破棄
            }
        }

        /// <summary>
        /// ドライブ状態に応じた画像を読み込み
        /// </summary>
        /// <param name="status">ドライブ状態(読み取り(true/false)、書き込み(true/false))</param>
        /// <returns>ドライブアイコン画像(Bitmap)</returns>
        private Bitmap LoadDriveImage(DriveStatus status)
        {
            try
            {
                // DPIに応じアイコンサイズフォルダー名を選択
                string dpiFolder = dpi >= SettingsManager.SizeChangeDpi ? "32" : "16";
                // アイコン画像ファイルの一時保存フォルダーパスを取得
                string folder = Path.Combine(tempIconRoot, dpiFolder);
                // ドライブ状態に応じたファイル名を決定
                string filename = status switch
                {
                    { IsReadActive: false, IsWriteActive: false } => "write_off_read_off.png",  // 読み取り・書き込み 両方オフ
                    { IsReadActive: false, IsWriteActive: true } => "write_on__read_off.png",  // 書き込みオン、読み取りオフ
                    { IsReadActive: true, IsWriteActive: false } => "write_off_read_on_.png",  // 読み取りオン、書き込みオフ
                    { IsReadActive: true, IsWriteActive: true } => "write_on__read_on_.png",  // 読み取り・書き込み 両方オン
                    _ => "write_off_read_off.png"                                       // 不明時は 読み取り・書き込み 両方オフ
                };

                // 画像ファイルのフルパスを取得
                string path = Path.Combine(folder, filename);

                if (!File.Exists(path))
                {// ファイルが存在しない場合
                    // 空のビットマップを返す
                    return new Bitmap(DriveSize.Width, DriveSize.Height);
                }

                // アイコン画像ファイルパスと読み取り・書き込み状態から FileStream を作成
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                // 画像を FileStream から読み込み
                using var img = Image.FromStream(fs, true, true);

                // 描画用ビットマップを作成
                var bmp = new Bitmap(DriveSize.Width, DriveSize.Height);
                // グラフィックスオブジェクトを取得して描画
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CompositingMode = CompositingMode.SourceOver;             // 元の画像を保持しつつ描画
                    g.CompositingQuality = CompositingQuality.HighQuality;      // 高品質合成
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic; // 高品質補間
                    g.SmoothingMode = SmoothingMode.AntiAlias;                  // アンチエイリアス
                    g.DrawImage(img, 0, 0, DriveSize.Width, DriveSize.Height);  // 画像を描画
                }
                return bmp; // ビットマップを返す
            }
            catch (Exception ex)
            {// 読み込み失敗時
                LogHelper.LogWrite($"Failed to load drive image. {ex.Message}");
                // 空のビットマップを返す
                return new Bitmap(DriveSize.Width, DriveSize.Height);
            }
        }

        /// <summary>
        /// アイコンハンドルを破棄するための外部関数
        /// </summary>
        /// <param name="hIcon">アイコンハンドル</param>
        /// <returns>true:アイコン破棄成功、false:アイコン破棄失敗</returns>
        // 外部関数の宣言
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool DestroyIcon(IntPtr hIcon);

        /// <summary>
        /// IDisposable 実装
        /// </summary>
        public void Dispose()
        {
            Dispose(true);              // マネージリソースの解放
            GC.SuppressFinalize(this);  // ファイナライザの呼び出しを抑制
        }

        /// <summary>
        /// Dispose パターンの実装
        /// (現在は 破棄済みフラグを設定するのみ)
        /// </summary>
        /// <param name="disposing">true:マネージ+アンマネージ、false:アンマネージのみ</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {// まだ破棄されていない場合
                if (disposing)
                {// disposing が true の場合
                    // マネージリソースの解放があればここに書く
                }

                // アンマネージリソースの解放があればここに書く

                // 破棄済みフラグを設定
                disposed = true;
            }
        }
    }
}