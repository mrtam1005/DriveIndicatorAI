# Drive Indicator AI - README(ja)

[技術資料はこちらから](00_Technical_documents(ja).md)

## 1\. 概要
    Drive Indicator AI は、Windows のドライブ I/O（読み取り・書き込み）状況を
    タスクトレイのアイコンでリアルタイム表示する軽量ユーティリティです。
      • SSD / HDD / USB / SD カード
      • RAM ドライブ（ETW による高精度監視）
      • 複数ドライブ同時監視
      • DPI（高解像度）対応
      • 多言語対応（日本語 / 英語など）
      • 自動起動（タスクスケジューラ方式）
      • ログ機能（ローテーション付き）
    Windows のストレージ動作を視覚的に把握したいユーザー向けに設計されています。

## 2\. 主な特徴
    1. ドライブ I/O をリアルタイム表示
      • 読み取り  → 明るい緑/暗い赤  
      • 書き込み  → 暗い緑  /明るい赤
      • 両方      → 明るい緑/明るい赤
      • アイドル  → 暗い緑  /暗い赤  
      2ドライブ ごとに 1アイコン としてまとめて表示するため、
      多数のドライブでもタスクトレイが圧迫されません。

    2. RAM ドライブも正確に監視（ETW 対応）
      通常の PerformanceCounter では取得できない RAMドライブの I/O を
      ETW (Event Tracing for Windows) を使って高精度に監視します。

    3. DPI 完全対応（16px / 32px 自動切り替え）
      • 100%（96 DPI） → 16×16 アイコン
      • 150%（144 DPI）以上 → 32×32 アイコン
      高 DPI 環境でも美しいアイコン表示を実現。

    4. 多言語対応
      日本語 / 英語 / ドイツ語 / フランス語 / イタリア語 / スペイン語 / 
      オランダ語 / ポルトガル語 / ロシア語 / 韓国語 / 中国語(簡体字) / 
      中国語(繁体字) / アラビア語 / チェコ語 / デンマーク語 / ギリシャ語 / 
      フィンランド語 / ヘブライ語 / ヒンディー語 / ハンガリー語 / 
      インドネシア語 / マレー語 / ノルウェー語 / ポーランド語 / ルーマニア語 / 
      スウェーデン語 / タイ語 / トルコ語 / ウクライナ語 / ベトナム語
      に対応

      "アプリケーションフォルダー\Resources\Language\languages.json" ファイル
      "アプリケーションフォルダー\Resources\Language\lang_*.json" ファイル
      これら2つのファイルを編集するだけで新しい言語を簡単に追加できます。

    5. 自動実行 (タスクスケジューラ方式)
      レジストリ Run キーではなく、
      schtasks.exe を使ったタスクスケジューラ登録のため：
        • 権限不要
        • UAC の影響なし
        • Windows Update で消されにくい

    6. ログ機能（1MB ローテーション）
      • %TEMP%\DriveIndicatorAI\Logs\MessagesLog.log に保存
      • 1MB を超えると自動で MessagesLog.old にローテーション
      • 例外や内部動作を詳細に記録
      ※ ログ書き込みの影響で動作が遅くなる場合があります。

## 3\. インストール方法
    1. .NET Runtime 10.0.1(Windows用)をダウンロードしてインストールする。
    2. Release ページから Drive Indicator AI の ZIPファイル をダウンロード
    3. 任意のフォルダに展開
    4. DriveIndicatorAI.exe を実行
   [1. の .NET Runtime 10.0.1 (Windows用) はこちらから](https://dotnet.microsoft.com/ja-jp/download/dotnet/10.0)  
   [2. の Drive Indicator AI の Release ページ はこちらから](https://github.com/mrtam1005/DriveIndicatorAI/tree/master/Release)  

## 4\. 使い方
    DriveIndicatorAI.exe を実行するとタスクトレイにアイコンが表示されます。

## 5\. 設定
    タスクトレイアイコンを右クリック → 設定

    <設定できる項目>
    1. Windows起動時に自動実行 有効/無効
          有効 : タスクスケジューラーに "DriveIndicatorAI_AutoStart" というタスクが登録されます。
          無効 : タスクスケジューラーに "DriveIndicatorAI_AutoStart" というタスクが削除されます。
       デフォルト : 無効

    2. 表示ドライブ選択 : ドライブ文字ごとに 有効/無効
       デフォルト : C

    3. 表示間隔 [msec] (設定範囲 1～250 msec)
       デフォルト : 50 msec

    4. アイコン画像フォルダ
        ･ "アプリケーションフォルダー\Resources\Icons\[テーマ名]\32" フォルダーに
          解像度 144 DPI 以上用アイコン画像 : 幅16×高さ32ドット PNGファイル
        ･ "アプリケーションフォルダー\Resources\Icons\[テーマ名]\16 フォルダーに
          解像度 144 DPI 未満用アイコン画像 : 幅8×高さ16ドット PNGファイル
        これらのファイルを保存しておき、[テーマ名] のフォルダーを選択する。
        各フォルダーには以下のファイル名でPNGファイルを保存しておく。
          読み取り無し/書き込み無し : write_off_read_off.png
          読み取り有り/書き込み無し : write_off_read_on_.png
          読み取り無し/書き込み有り : write_on__read_off.png
          読み取り有り/書き込み有り : write_on__read_on_.png
       デフォルト : "アプリケーションフォルダー\Resources\Icons\Default"

    5. ドライブ文字色
       デフォルト : "#BFBFBF" (R=191,G=191.B=191 Silver)

    6. Language(言語)
       デフォルト : "ja" (日本語)

    7. logを記録する 有効/無効
       デフォルト : 無効

    "保存"       ボタン をクリックすると設定が有効になります。
    "キャンセル" ボタン をクリックすると設定画面上の設定値は破棄されます。

## 6\. 対応OS
    ･ Windows10 64bit
    ･ Windows11 64bit

## 7\. フォルダー構成
    1.アプリケーションフォルダー
      DriveIndicatorAI/          アプリケーションフォルダー
      ├─ DriveIndicatorAI.exe  アプリケーションファイル
      ├─ settings.json         設定ファイル(自動生成)
      └─ Resources/
           ├─ Language/
           │   ├─ lang_ja.json        言語ファイル(日本語)
           │   ├─ lang_en.json        言語ファイル(英語)
           │   ├─ lang_de.json        言語ファイル(ドイツ語)
           │   ├─ lang_**.json         ･
           │   ├─ lang_**.json         ･
           │   ├─ lang_**.json         ･
           │   ├─ lang_**.json         ･
           │   └─ languages.json      言語リストファイル
           └─ Icons/
                ├─ app.ico
                ├─ appIcon.bmp
                └─ Default/
                     ├─ 16/                            解像度 144 DPI 未満用アイコン画像フォルダー
                     │   ├─ write_off_read_off.png    幅8×高さ16ドット 書き込み無し,読み取り無し PNGファイル
                     │   ├─ write_off_read_on_.png    幅8×高さ16ドット 書き込み無し,読み取り有り PNGファイル
                     │   ├─ write_on__read_off.png    幅8×高さ16ドット 書き込み有り,読み取り無し PNGファイル
                     │   └─ write_on__read_on_.png    幅8×高さ16ドット 書き込み有り,読み取り有り PNGファイル
                     └─ 32/                            解像度 144 DPI 以上用アイコン画像フォルダー
                          ├─ write_off_read_off.png    幅16×高さ32ドット 書き込み無し,読み取り無し PNGファイル
                          ├─ write_off_read_on_.png    幅16×高さ32ドット 書き込み無し,読み取り有り PNGファイル
                          ├─ write_on__read_off.png    幅16×高さ32ドット 書き込み有り,読み取り無し PNGファイル
                          └─ write_on__read_on_.png    幅16×高さ32ドット 書き込み有り,読み取り有り PNGファイル

    2.一時フォルダー
      %TEMP%/                                 一時フォルダー(Windowsで設定されている一時フォルダー)
       └─ DriveIndicatorAI/                 アプリケーションフォルダー
            ├─ DriveIndicatorAI_Task.xml    タスクスケジューラー用プロパティーファイル
            ├─ settings.json                設定ファイル
            ├─ Icons/
            │   ├─ 16/                            解像度 144 DPI 未満用アイコン画像フォルダーのコピー
            │   │   ├─ write_off_read_off.png
            │   │   ├─ write_off_read_on_.png
            │   │   ├─ write_on__read_off.png
            │   │   └─ write_on__read_on_.png
            │   └─ 32/                            解像度 144 DPI 以上用アイコン画像フォルダーのコピー
            │        ├─ write_off_read_off.png
            │        ├─ write_off_read_on_.png
            │        ├─ write_on__read_off.png
            │        └─ write_on__read_on_.png
            └─ Logs/
                 ├─ MessagesLog.log            ログファイル (自動生成)
                 └─ MessagesLog.old            古いログファイル (ログファイルの容量が 1MB を超える場合 自動生成)

## 8\. 技術情報 (開発者向け)
    1. I/O 監視
      ･ 通常ドライブ : PerformanceCounter (LogicalDisk)
      ･ RAM ドライブ : ETW (FileIORead / FileIOWrite)
    2. DPI 対応
      ･ IconRenderer が DPI を判定し、アイコンサイズを 16px / 32px に自動切り替え
      ･ フォントは GraphicsUnit.Pixel で DPI 依存を排除
    3. ログ
      ･ %TEMP% フォルダーに保存
      ･ 1MB ローテーション
      ･ 例外はDebug.WriteLine にも出力
    4. 多言語
      ･ JSON ファイルベース
      ･ LangManager がロード
      ･ 見つからない場合は日本語で表示

## 9\. ライセンス
    1. ライセンス
      本ソフトウェアは無償で提供されます。
      個人利用および非商用利用は自由に行うことができます。
      詳細はアプリ内の License ダイアログをご確認ください。

    2. 開発者
      mrtam1005
      Github:
        https://github.com/mrtam1005/DriveIndicatorAI
   [Github: mrtam1005/DriveIndicatorAI  はこちらから](https://github.com/mrtam1005/DriveIndicatorAI)  

## 更新履歴

