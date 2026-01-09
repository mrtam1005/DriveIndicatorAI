[←前へ](03_Components(ja).md) | [次へ→](05_Dataflow(ja).md) | [先頭へ](00_Technical_documents(ja).md)  

## 4\. UI コンポーネント  
    Drive Indicator AI の UI は、ユーザー操作を最小限に抑えつつ、  
    設定変更･バージョン確認･ライセンス確認など必要な機能を提供するよう設計されています。  
    UI コンポーネントは以下の 3 つで構成されます :  
      1. SettingsForm (設定画面)  
      2. VersionInfoForm (バージョン情報)  
      3. LicenseDialog (ライセンス表示)  
    これらはすべて TrayIconManager のコンテキストメニューから起動され、  
    アプリ本体のロジックとは疎結合になっています。  

### 4.1 SettingsForm — 設定画面  

#### 4.1.1 役割  
    ･ アプリの設定をユーザーが変更するための UI  
    ･ SettingsManager と連携して設定を読み書き  
    ･ 言語切り替え・DPI サンプル表示など、Drive Indicator AI の中で最も複雑な UI  

#### 4.1.2 主な機能  
    ･ 監視対象ドライブの選択  
    ･ 表示間隔 (更新周期) の設定  
    ･ アイコンフォルダの選択  
    ･ ドライブ文字の色設定  
    ･ 言語選択 (日本語 / 英語)  
    ･ 自動起動 (タスクスケジューラ登録)  
    ･ ログ有効化  
    ･ DPI に応じたサンプルアイコンの描画 (PictureBox)  

#### 4.1.3 技術的ポイント  
    1. 言語切り替え (ApplyLanguage)  
          ･ コンボボックスの言語名  
          ･ ラベル・ボタンのテキスト  
          ･ メッセージボックスの文言  
        すべて JSON の翻訳テーブルから取得されます。  
    2. DPI サンプルアイコンの描画  
        PictureBox に表示されるサンプルアイコンは :  
          ･ IconRenderer と同じロジック  
          ･ FontHelper の DPI フォント  
          ･ PNG アイコンの合成  
        を使って描画されます。  
        これにより、実際のタスクトレイと同じ見た目を事前に確認できるようになっています。  
    3. 設定の保存
         OK ボタンで SettingsManager.Save() が呼ばれ、設定ファイル (JSON) に永続化されます。  

### 4.2 VersionInfoForm — バージョン情報画面  

#### 4.2.1 役割  
    ･ アプリのバージョン情報を表示  
    ･ Github リンクを開く  
    ･ ライセンスダイアログを開く  

#### 4.2.2 主な機能  
    ･ FileVersionInfo からバージョン番号を取得  
    ･ Github リンクを既定ブラウザで開く  
    ･ LicenseDialog を非モーダルで表示  

#### 4.2.3 技術的ポイント  
    1. FileVersionInfo を使用したバージョン取得  
      ───────────────────────────────────────────────────────────────────────  
      FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion  
      ───────────────────────────────────────────────────────────────────────  
      → AssemblyVersion ではなく FileVersion を使うことで、ユーザーに見せるバージョン番号を  
         柔軟に管理できます。  
    2. 非モーダルダイアログの管理  
      LicenseDialog は再利用され、複数開かないように制御されています。  

### 4.3 LicenseDialog — ライセンス表示ダイアログ  

#### 4.3.1 役割  
    ･ ライセンス文を表示  
    ･ OK ボタンで閉じる  

#### 4.3.2 主な機能  
    ･ 言語コードに応じて日本語/英語のライセンス文を表示  
    ･ textBoxLicense は ReadOnly + ScrollBars.Vertical  
    ･ ControlBox = false で閉じる方法を統一  

#### 4.3.3 技術的ポイント  
    1. ライセンス文はハードコード  
      国際化は JSON ではなく、LicenseDialog 内で直接切り替えています。  
      理由 :  
        ･ ライセンス文はアプリの一部であり、外部ファイルに依存させない方が安全  
        ･ JSON の破損や翻訳漏れの影響を受けない  
    2. UI が壊れにくい構造  
      ･ DockStyle.Top のテキストボックス  
      ･ Anchor で固定された OK ボタン  
      ･ ControlBox = false で UX が明確  

### 4.4 UI コンポーネントの設計思想 (まとめ)  
    Drive Indicator AI の UI は、以下の特徴を持っています :  
      ･ ロジックと UI の完全分離  
          設定変更は SettingsManager、監視は DriveMonitor が担当し、UI はあくまで入力と表示に徹している。  
      ･ 多言語対応が自然に組み込まれている  
          LangManager による JSON ベースの翻訳。  
      ･ DPI 対応が UI にも反映されている  
          サンプルアイコン描画で DPI の違いを確認可能。  
      ･ 壊れにくい UI  
          Dispose、Anchor、Dock、ControlBox など WinForms の落とし穴を避けている。  

[←前へ](03_Components(ja).md) | [次へ→](05_Dataflow(ja).md) | [先頭へ](00_Technical_documents(ja).md)  