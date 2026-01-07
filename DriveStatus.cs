//-------------------------------------------------------------------
// DriveStatus.cs
// 各ドライブの状態（READ/WRITE）データ
//-------------------------------------------------------------------

namespace DriveIndicatorAI
{
    /// <summary>
    /// ドライブ状態データクラス
    /// </summary>
    public class DriveStatus
    {
        /// <summary>
        /// ドライブ文字 (A～Z)
        /// </summary>
        public char DriveLetter { get; set; }
        /// <summary>
        /// 読み取り中 (true:読み取り中, false:読み取り停止中)
        /// </summary>
        public bool IsReadActive { get; set; }
        /// <summary>
        /// 書き込み中 (true:書き込み中, false:書き込み停止中)
        /// </summary>
        public bool IsWriteActive { get; set; }
    }
}