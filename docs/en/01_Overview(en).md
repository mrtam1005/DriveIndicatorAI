[←前へ](00_Technical_documents(en).md) | [次へ→](02_Architecture(en).md) | [先頭へ](00_Technical_documents(en).md)  

## 1\. 概要 (Overview)  

### 1.1 Drive Indicator AI とは  
    Drive Indicator AI は、Windows のストレージ I/O (読み取り・書き込み) 状況を  
    タスクトレイのアイコンでリアルタイムに可視化する軽量ユーティリティです。  
    通常のディスクだけでなく、一般的な監視方法では取得が難しい RAMドライブ の I/O を  
    ETW (Event Tracing for Windows) で高精度に監視できる点が最大の特徴です。  
    また、DPI 対応・多言語対応・自動起動・ログ機能など、実用性と拡張性を重視した設計になっています。  
    このアプリケーションの作成のきっかけですが、  
    フリーソフトの同様の機能を持ったアプリケーションを使用していましたが、  
    アイコンの並びが思い通りでなかったり、OS が Windows11 に変わり正しく動作しなくなったことで、  
    ｢いっそのこと自分で作ってみては？｣ と思い立ち、AI の Copilot に相談したところ、｢簡単にできますよ！｣と  
    回答があったので、作成に着手しました。  
    AI の Copilot の協力に感謝の意を込めアプリケーション名の末尾に AI の文字を付加しました。  

### 1.2 主な特徴 (技術的視点)  
    1. 高精度な I/O 監視  
      ･ PerformanceCounter による通常ドライブの I/O 監視  
      ･ ETW (FileIORead / FileIOWrite) による RAMドライブ 監視  
      ･ DriveStatus に統合し、TrayIconManager に渡す構造  
    2. DPI 完全対応  
      ･ DPI に応じて 16px / 32px アイコンを自動切り替え  
      ･ IconRenderer が DPI を判定し、最適な描画を実施  
      ･ フォントは GraphicsUnit.Pixel を使用し DPI 差を吸収    
    3. 多言語対応 (JSON ベース)  
      ･ ApplicationFolder/Resources/Language フォルダー内の languages.json と lang\_xx.json ファイルを
        読み込む  
      ･ LangManager が翻訳テーブルを管理  
      ･ UIテキスト はすべてキーで管理され、拡張が容易  
    4. 自動実行 (タスクスケジューラ方式)  
      ･ StartupHelper が XML を生成し "schtasks.exe" を実行  
      ･ 権限不要・UAC の影響なし  
      ･ レジストリ Run キーより安全で確実  
    5. ログ機能 (ローテーション付き)  
      ･ %TEMP%/DriveIndicatorAI/Log フォルダー にログを保存  
      ･ 1MB 超で .old にローテーション  
      ･ スレッドセーフな書き込み  

### 1.3 全体アーキテクチャの特徴  
    Drive Indicator AI は、以下のような 明確な責務分離 に基づいて設計されています。  
      ･ SettingsManager   : 設定の読み書き・永続化  
      ･ DriveMonitor      : I/O 監視 (PerfCounter + ETW)  
      ･ IconRenderer      : DPI 対応アイコン描画  
      ･ TrayIconManager   : 通知領域アイコンの管理・更新  
      ･ LangManager       : 多言語テキスト管理  
      ･ LogHelper         : ログ管理 (ローテーション付き)  
      ･ UI (SettingsForm / VersionInfoForm / LicenseDialog)  : ユーザー操作と設定変更  
    この構造により、UI とロジックが完全に分離され、拡張性・保守性が非常に高いアプリになっています。  

### 1.4 データフロー概要 (簡易版)  
    ┌─────────┐  
    │ SettingsManager  │ ← 設定読み込み  
    └────┬────┘  
              │  
              ▼  
    ┌─────────┐  
    │   DriveMonitor   │ ← PerfCounter / ETW  
    └────┬────┘  
              │ DriveStatus\[]  
              ▼  
    ┌─────────┐  
    │   IconRenderer   │ ← DPI に応じて描画  
    └────┬────┘  
              │ Icon\[]  
              ▼  
    ┌─────────┐  
    │ TrayIconManager  │ ← 通知領域に反映  
    └─────────┘  

[←前へ](00_Technical_documents(en).md) | [次へ→](02_Architecture(en).md) | [先頭へ](00_Technical_documents(en).md)  

