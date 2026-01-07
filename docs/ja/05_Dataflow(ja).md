[←前へ](04_UI(ja).md) | [次へ→](06_DPI(ja).md) | [先頭へ](00_Technical_documents(ja).md)  

## 5\. データフロー詳細 (Data Flow Details)  
    Drive Indicator AI は、  
      起動 → 設定読み込み → 監視開始 → アイコン描画 → 通知領域更新 → 終了処理  
    という一連の流れで動作します。  
    ここでは、アプリ内部でどのようにデータが流れ、  
    どのコンポーネントがどのタイミングで動作するのかを詳しく説明します。  

### 5.1 アプリ起動 → 設定読み込み  
    アプリ起動時、Program.cs から以下の処理が行われます。  
      1. SettingsManager.Load()  
      2. LangManager.Load(languageCode)  
      3. TrayIconManager.Start()  

#### 5.1.1 SettingsManager.Load()  
    ･ 設定ファイル (JSON) を読み込み  
    ･ 読み込み失敗時はデフォルト値を使用  
    ･ 言語コード・監視対象ドライブ・更新間隔などをメモリに保持  

#### 5.1.2 LangManager.Load()  
    ･ Resources/Language/lang\_xx.json を読み込み  
    ･ 翻訳テーブルを構築  
    ･ UI テキストの準備が整う  

#### 5.1.3 TrayIconManager.Start()  
    ･ アイコン PNG を TEMP に展開  
    ･ DriveMonitor を初期化  
    ･ アイコン更新ループを開始  

### 5.2 監視開始 → DriveStatus 更新  
    TrayIconManager のメインループでは、一定間隔で DriveMonitor.Update() が呼ばれます。  

      DriveMonitor.Update()  
        ├─ PerfCounter で通常ドライブの I/O を取得  
        ├─ EtwRamIoMonitor で RAMドライブ の I/O を取得  
        └─ DriveStatus\[] を生成  

#### 5.2.1 PerformanceCounter の処理  
    ･ LogicalDisk の ReadBytes/sec  
    ･ LogicalDisk の WriteBytes/sec  
    ･ ドライブ文字ごとに値を取得  
    ･ 閾値を超えたら ｢読み取り/書き込みアクティブ｣ と判定  

#### 5.2.2 ETW の処理 (RAMドライブ )  
    ･ FileIORead / FileIOWrite イベントを監視  
    ･ DriveLetter を抽出  
    ･ バイト数を DriveStatus に反映  
    ･ CancellationToken により安全に停止可能  

#### 5.2.3 DriveStatus の生成  
    DriveStatus は以下の情報を持つ :  
      ┌────────┬─────────┐  
      │ プロパティ     │ 説明             │  
      ┝━━━━━━━━┿━━━━━━━━━┥  
      │ DriveLetter    │ ドライブ文字     │  
      ├────────┼─────────┤  
      │ IsReadActive   │ 読み取り中か     │  
      ├────────┼─────────┤  
      │ IsWriteActive  │ 書き込み中か     │  
      ├────────┼─────────┤  
      │ ReadBytes      │ 読み取りバイト数 │  
      ├────────┼─────────┤  
      │ WriteBytes     │ 書き込みバイト数 │  
      └────────┴─────────┘  

### 5.3 アイコン描画 → IconRenderer  
    DriveStatus\[] を受け取った TrayIconManager は、IconRenderer にアイコン描画を依頼します。  

      IconRenderer.RenderIcons(DriveStatus\[])  
        ├─ DPI 判定 (16px / 32px)  
        ├─ PNG 読み込み  
        ├─ DriveLetter の描画 (FontHelper)  
        ├─ I/O 状態に応じた色の合成  
        └─ Icon\[] を生成  

#### 5.3.1 DPI 判定  
    SettingsManager.Current.Dpi または Graphics.DpiX を参照し :  
      ･ 144 DPI 未満 → 16px  
      ･ 144 DPI 以上 → 32px  
    を自動選択。  

#### 5.3.2 PNG の読み込み  
      ･ 読み取りアイコン        : (write\_off\_read\_on\_.png)  
      ･ 書き込みアイコン        : (write\_on\_\_read\_off.png)  
      ･ アイドルアイコン        : (write\_off\_read\_off.png)  
      ･ 両方アクティブアイコン  : (write\_on\_\_read\_on\_.png)  
    を合成して 1つのアイコンにまとめる。  

#### 5.3.3 DriveLetter の描画  
    FontHelper が DPI に応じたフォントを返すため、高 DPI でも文字が潰れない。  

### 5.4 通知領域アイコン更新 → TrayIconManager  
    IconRenderer が生成した Icon\[] を受け取り、TrayIconManager が通知領域に反映します。  

      TrayIconManager.UpdateIcons(Icon\[])  
        ├─ NotifyIcon.Icon にセット  
        ├─ 古いアイコンを DestroyIcon で破棄  
        ├─ コンテキストメニューの更新  
        └─ 次のループまで待機  

#### 5.4.1 アイコン破棄  
    Win32 API DestroyIcon を使用し、GDI リソースリークを防止。  

#### 5.4.2 コンテキストメニュー  
      ･ 設定 (Settings)  
      ･ 終了 (Quit)  
    などが LangManager により多言語化される。  

### 5.5 終了処理 → ETW 停止・ログ保存  
    アプリ終了時、TrayIconManager.Dispose() が呼ばれます。  

      TrayIconManager.Dispose()  
        ├─ DriveMonitor.StopEtwMonitor()  
        ├─ NotifyIcon.Dispose()  
        ├─ Icon の破棄  
        └─ FontHelper.ClearCache()  

#### 5.5.1 ETW の停止  
    ･ CancellationToken を発行  
    ･ TraceEventSession を安全に停止  
    ･ スレッドを確実に終了  

#### 5.5.2 アイコン破棄  
    ･ GDI リソースをすべて解放  
    ･ TEMP の PNG は残しても問題なし  

#### 5.5.3 フォントキャッシュの破棄  
    FontHelper.ClearCache() により Font.Dispose() が確実に呼ばれる。  

### 5.6 データフローの特徴 (まとめ)  
    Drive Indicator AI のデータフローは以下の特徴を持つ :  
      ･ 非同期処理が安全に管理されている  
          ETW は CancellationToken で停止  
          PerfCounter は例外処理で保護  
      ･ DPI と I/O 状態がリアルタイムに反映される  
          アイコン描画は毎ループで実行  
      ･ 責務分離が徹底されている  
          監視・描画・UI・設定が完全に独立  
      ･ ログが全体を支える  
          例外や内部動作をすべて記録  

[←前へ](04_UI(ja).md) | [次へ→](06_DPI(ja).md) | [先頭へ](00_Technical_documents(ja).md)  