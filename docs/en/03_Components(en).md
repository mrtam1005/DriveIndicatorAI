[←前へ](02_Architecture(en).md) | [次へ→](04_UI(en).md) | [先頭へ](00_Technical_documents(en).md)  

## 3\. 主要コンポーネントの詳細 (Core Components)  
    Drive Indicator AI は、複数の独立したコンポーネントが協調して動作する構造になっています。  
    ここでは、各コンポーネントの役割・内部構造・他コンポーネントとの関係を詳しく説明します。  

### 3.1 SettingsManager — 設定管理の中核  

#### 3.1.1 役割  
    ･ 設定ファイル (JSON) の読み書き  
    ･ 設定値の永続化  
    ･ デフォルト値の提供  
    ･ 言語・DPI・ログ設定など、アプリ全体の動作に影響する値を保持  

#### 3.1.2 主な機能  
    ･ Load()    : 設定ファイルを読み込む  
    ･ Save()    : 設定ファイルに書き込む  
    ･ Current   : 現在の設定インスタンス  
    ･ DefLangCode / DefLangName  : 言語のデフォルト値  
    ･ SizeChangeDpi  : DPI 切り替え閾値 (16px ↔ 32px)  

#### 3.1.3 他コンポーネントとの関係  
      ･ TrayIconManager  : 更新間隔・監視対象ドライブを参照  
      ･ IconRenderer     : DPI 値を参照  
      ･ LangManager      : 言語コードを参照  
      ･ LogHelper        : ログ有効/無効を参照  
    アプリ全体の設定の“単一情報源 (Single Source of Truth)”になっている。  


## 3.2 TrayIconManager — 通知領域アイコンの管理者  

#### 3.2.1 役割  
    ･ タスクトレイアイコンの生成・更新  
    ･ コンテキストメニューの構築  
    ･ DriveMonitor と IconRenderer を連携させる中心的存在  
    ･ アプリのメインループを管理  

#### 3.2.2 主な機能  
    ･ Start()             : 監視とアイコン更新ループを開始  
    ･ UpdateIcons()       : IconRenderer で生成したアイコンをトレイに反映  
    ･ PrepareTempIcons()  : アイコン PNG を TEMP に展開  
    ･ ApplyLanguage()     : メニューの多言語化  
    ･ Dispose()           : アイコン破棄・ETW 停止  

#### 3.2.3 他コンポーネントとの関係  
      ･ DriveMonitor     : DriveStatus\[] を取得  
      ･ IconRenderer     : アイコン描画を依頼  
      ･ SettingsManager  : 更新間隔・監視対象ドライブを参照  
      ･ LangManager      : メニューの翻訳  
      ･ LogHelper        : 内部動作をログ出力  
    Drive Indicator AI の“司令塔”にあたるコンポーネント。  

### 3.3 DriveMonitor — I/O 監視の中心  

#### 3.3.1 役割  
    ･ 各ドライブの I/O 状態を取得し、DriveStatus にまとめる  
    ･ 通常ドライブ → PerformanceCounter  
    ･ RAMドライブ  → ETW (EtwRamIoMonitor)  

#### 3.3.2 主な機能  
    ･ Update()          : 全ドライブの I/O を取得  
    ･ GetDriveStatus()  : DriveStatus を生成  
    ･ StartEtwMonitor() / StopEtwMonitor()  : RAMドライブ 監視  

#### 3.3.3 DriveStatus の内容  
    ･ DriveLetter  
    ･ IsReadActive  
    ･ IsWriteActive  
    ･ ReadBytes  
    ･ WriteBytes  

#### 3.3.4 他コンポーネントとの関係  
      ･ EtwRamIoMonitor  : RAMドライブ の I/O を取得  
      ･ TrayIconManager  : DriveStatus\[] を渡す  
      ･ SettingsManager  : 監視対象ドライブを参照  
      ･ LogHelper        : 監視エラーを記録  
    Drive Indicator AI の“センサー”にあたるコンポーネント。  

### 3.4 EtwRamIoMonitor — RAMドライブ 専用の高精度監視  

#### 3.4.1 役割  
    ･ ETW (Event Tracing for Windows) を使って RAMドライブ の I/O を監視  
    ･ FileIORead / FileIOWrite イベントを解析  
    ･ DriveMonitor に I/O 情報を提供  

#### 3.4.2 主な機能  
    ･ Start()         : ETW セッション開始  
    ･ Stop()          : ETW セッション停止  
    ･ ProcessEvent()  : イベント解析  
    ･ CancellationToken による安全な停止  

#### 3.4.3 技術的特徴  
      ･ TraceEventSession を使用  
      ･ セッション名の衝突を回避  
      ･ RAMドライブ の I/O を正確に取得できる (一般的なツールでは困難)  
    Drive Indicator AI の“高度な監視能力”を支える重要コンポーネント。  

### 3.5 IconRenderer — DPI 対応アイコン描画の中心  

#### 3.5.1 役割  
    ･ DriveStatus に応じて PNG を合成し、アイコンを生成  
    ･ DPI に応じて 16px / 32px を自動切り替え  
    ･ DriveLetter をフォント描画  
    ･ Win32 DestroyIcon でハンドルを安全に破棄  

#### 3.5.2 主な機能  
    ･ RenderIcons()     : 複数ドライブをまとめてアイコン化  
    ･ RenderIcon()      : 1つのアイコンを描画  
    ･ LoadDriveImage()  : PNG 読み込み  
    ･ IconSize / DriveSize / LetterSize  : DPI に応じたサイズ計算  

#### 3.5.3 他コンポーネントとの関係  
      ･ FontHelper       : フォントキャッシュ  
      ･ SettingsManager  : DPI・文字色  
      ･ TrayIconManager  : 描画結果を渡す  
    Drive Indicator AI の“見た目”を作るコンポーネント。  

### 3.6 FontHelper — DPI に応じたフォントキャッシュ  

#### 3.6.1 役割  

    ･ DPI に応じたフォントをキャッシュし、無駄な生成を防ぐ  
    ･ GraphicsUnit.Pixel を使用し DPI 差を吸収  

#### 3.6.2 主な機能  
      ･ GetDriveLetterFontCache()  : キャッシュから取得  
      ･ FontCreat()   : フォント生成  
      ･ ClearCache()  : Dispose + キャッシュクリア  
    DPI 環境でも文字が美しく表示される理由がここにある。  

### 3.7 LogHelper — ログ管理 (ローテーション付き)  

#### 3.7.1 役割  
    ･ TEMP にログを保存  
    ･ 1MB 超で .old にローテーション  
    ･ スレッドセーフな書き込み  

#### 3.7.2 主な機能  
      ･ LogWrite()  : ログ書き込み  
      ･ ClearLog()  : ログ削除  
      ･ ログフォルダの自動生成  
    Drive Indicator AI の“透明性”と“デバッグ容易性”を支える。  

### 3.8 LangManager — JSON ベースの多言語対応  

#### 3.8.1 役割  
    ･ 言語ファイル (lang\_xx.json) を読み込み  
    ･ 翻訳テーブルを管理  
    ･ UI テキストをキーで取得  

#### 3.8.2 主な機能  
      ･ Lang.Load()          : 翻訳テーブル読み込み  
      ･ Lang.T()             : 翻訳文字列取得  
      ･ LoadLanguagesJson()  : 言語一覧読み込み  
      ･ 言語名 ↔ 言語コードの相互変換  
    Drive Indicator AI の国際化を支える基盤。  

[←前へ](02_Architecture(en).md) | [次へ→](04_UI(en).md) | [先頭へ](00_Technical_Documents.md)