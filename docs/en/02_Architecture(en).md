[←前へ](01_Overview(en).md) | [次へ→](03_Components(en).md) | [先頭へ](00_Technical_Documents(en).md)  

## 2\. アーキテクチャ構成図 (Architecture Overview)  
    Drive Indicator AI は、  
      監視 → 状態更新 → アイコン描画 → 通知領域更新  
    という一連の処理を、複数の独立したコンポーネントが協力して実現する構造になっています。  
    ここでは、全体構造を ｢コンポーネント図｣ ｢クラス依存関係図｣ ｢処理フロー図｣ の3つに分けて説明します。  

### 2.1 コンポーネント構成図 (Component Diagram)  

    ┌──────────────┐  
    │      SettingsManager       │  
    │  設定の読み書き・永続化    │  
    └──────┬───────┘  
                  │  
                  ▼  
    ┌──────────────┐  
    │        DriveMonitor        │  
    │ ・PerformanceCounter 監視  │  
    │ ・ETW (RAMドライブ) 監視   │  
    │  → DriveStatus\[] を生成  │  
    └──────┬───────┘  
                  │  
                  ▼  
    ┌──────────────┐  
    │        IconRenderer        │  
    │ ・DPI判定 (16px / 32px)    │  
    │ ・PNG合成描画              │  
    │ ・フォント描画(FontHelper) │  
    │  → Icon\[] を生成         │  
    └──────┬───────┘  
                  │  
                  ▼  
    ┌──────────────┐  
    │       TrayIconManager      │  
    │ ・通知領域アイコンの管理   │  
    │ ・コンテキストメニュー     │  
    │ ・アイコン更新ループ       │  
    └──────────────┘  

### 2.2 クラス依存関係図 (Class Dependency Diagram)  

    SettingsManager  
          ▲  
          │  設定値を参照  
          │  
     DriveMonitor ───────┐  
          │                    │ DriveStatus\[]  
          ▼                    ▼  
    EtwRamIoMonitor        IconRenderer ────→ FontHelper  
          │                    │  
          │ ETWイベント        │ Icon  
          ▼                    ▼  
       LogHelper ←─── TrayIconManager  

    依存関係のポイント  
      ･ TrayIconManager はアプリの中心で、DriveMonitor と IconRenderer の両方を使う  
      ･ DriveMonitor は PerfCounter と ETW を統合  
      ･ IconRenderer は FontHelper を利用  
      ･ LogHelper は全コンポーネントから参照される (横断的関心事)  
      ･ SettingsManager は全体の設定を提供する基盤  

### 2.3 処理フロー図 (監視 → 描画 → 表示)  
    Drive Indicator AI のメインループは以下のように動作します。  
    ───────────────────  
                 アプリ起動  
                     │  
                     ▼  
            SettingsManager.Load()  
                     │  
                     ▼  
            TrayIconManager.Start()  
                     │  
                     ▼  
    ───────────────────  
      メインループ (一定間隔で繰り返し)  
    ───────────────────  
                     │  
                     ▼  
           DriveMonitor.Update()  
            ･ PerfCounter で I/O 取得  
            ･ ETW で RAMドライブ I/O 取得  
            → DriveStatus\[] を生成  
                     │  
                     ▼  
    IconRenderer.RenderIcons(DriveStatus\[])  
            ･ PNG 読み込み  
            ･ DPI に応じて描画  
            ･ DriveLetter を描画  
            → Icon\[]  
                     │  
                     ▼  
      TrayIconManager.UpdateIcons(Icon\[])  
          ・通知領域アイコンを更新  
    ───────────────────  

### 2.4 アーキテクチャの特徴 (まとめ)  
    ･ 責務分離が明確  
        UI / 設定 / 監視 / 描画 / ログ が完全に独立  
    ･ 拡張性が高い  
        新しい言語・アイコンテーマ・監視方式を追加しやすい  
    ･ DPI 対応が堅牢  
        IconRenderer と FontHelper が DPI 差を吸収  
    ･ RAMドライブ 監視が強力  
        ETW を使うことで一般的なツールでは取得できない情報を取得可能  
    ･ ログが全体を支える  
        例外や内部動作をすべて記録し、デバッグしやすい  

[←前へ](01_Overview(en).md) | [次へ→](03_Components(en).md) | [先頭へ](00_Technical_Documents(en).md)  