//-------------------------------------------------------------------
// FontHelper.cs
// ドライブ文字フォントのキャッシュ管理
//-------------------------------------------------------------------

namespace DriveIndicatorAI
{
    /// <summary>
    /// ドライブ文字フォントのキャッシュ管理クラス
    /// </summary>
    public static class FontHelper
    {
        /// <summary>
        /// フォントキャッシュ辞書作成
        /// </summary>
        private static readonly Dictionary<int, Font> cache = [];
#pragma warning disable IDE0330                         // IDE0330 メッセージを無効化 開始
        /// <summary>
        /// ロックオブジェクト作成
        /// </summary>
        private static readonly object lockObj = new();
#pragma warning restore IDE0330                         // IDE0330 メッセージを無効化 終了

        /// <summary>
        /// ドライブ文字描画用フォントを返す(キャッシュ使用)
        /// </summary>
        /// <param name="dpi">解像度値 [DPI]</param>
        /// <returns>フォント</returns>
        public static Font GetDriveLetterFontCache(float dpi)
        {
            // DPIを整数化してキーにする
            int dpiKey = (int)Math.Round(dpi);

            // ロックオブジェクトをロック
            lock (lockObj)
            {
                if (cache.TryGetValue(dpiKey, out var cachedFont))
                {// キャッシュしたフォントがある場合
                    return cachedFont;  // キャッシュしたフォントを返す
                }
                // フォントを新規作成
                var font = FontCreat(dpiKey);
                // フォントをキャッシュに保存
                cache[dpiKey] = font;
                return font;    // 新規作成したフォントを返す
            }
        }

        /// <summary>
        /// ドライブ文字描画用フォントを返す(キャッシュ不使用)
        /// </summary>
        /// <param name="dpi">解像度値 [DPI]</param>
        /// <returns>フォント</returns>
        public static Font GetDriveLetterFont(float dpi)
        {
            // DPIを整数化してキーにする
            int dpiKey = (int)Math.Round(dpi);

            // ロックオブジェクトをロック
            lock (lockObj)
            {
                // フォントを新規作成
                var font = FontCreat(dpiKey);
                return font;    // 作成したフォントを返す
            }
        }

        /// <summary>
        /// 解像度に合わせたサイズでフォントを作成
        /// </summary>
        /// <param name="dpi">解像度値 [DPI]</param>
        /// <returns>フォント</returns>
        private static Font FontCreat(int dpi)
        {
            // 解像度値(dpi)が閾値以上のときは大きめ(18)、そうでなければ小さめ( 9)
            float size = dpi >= SettingsManager.SizeChangeDpi ? 18f : 9f;
            // フォントを取得 (フォント名、サイズ、書式、GraphicsUnit.Pixel(これを使用すると環境差が少なく安定))
            var font = new Font("Segoe UI", size, FontStyle.Bold, GraphicsUnit.Pixel);
            LogHelper.LogWrite($"FontHelper-> FontCreat() Created font. \"DPI:{dpi}, {font}\"");
            return font;    // 新規作成したフォントを返す
        }

        /// <summary>
        /// フォントキャッシュを破棄・クリアする
        /// </summary>
        public static void ClearCache()
        {
            // ロックオブジェクトをロック
            lock (lockObj)
            {
                foreach (var f in cache.Values)
                {// キャッシュ内のフォントがある間
                    try
                    {
                        f.Dispose(); // フォント破棄
                        LogHelper.LogWrite("FontHelper-> ClearCache() Disposed of font.");
                    }
                    catch (Exception ex)
                    {// フォント破棄に失敗
                        LogHelper.LogWrite($"FontHelper-> ClearCache() failed.{ex.Message}");
                        // 失敗時は無視
                    }
                }
                cache.Clear();  // フォントキャッシュ辞書をクリア
            }
        }
    }
}