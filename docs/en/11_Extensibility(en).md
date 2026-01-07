[←前へ](10_i18n(en).md) | [次へ→](12_Limitations(en).md) | [先頭へ](00_Technical_Documents(en).md)  

## 11. 拡張方法 (Extensibility Guide)  
    Drive Indicator AI は、UI・監視ロジック・描画ロジック・国際化などが明確に分離されているため、  
    後から機能を追加したり、カスタマイズしたりするのが容易です。  
    この章では、開発者が Drive Indicator AI を拡張する際に必要なポイントをまとめます。  

### 11.1 新しい言語の追加 (i18n 拡張)  
    Drive Indicator AI の多言語対応は JSON ベースで、非常に簡単に拡張できます。  

      手順 1 : 言語ファイルをコピー  
        Resources/Language/lang_en.json をコピーして lang_xx.json のように名前を変更。  

      手順 2 : 翻訳を記述  
        lang_xx.json の値を翻訳するだけ。  

      手順 3 : languages.json に追加  
        ─────────────────────────────────  
        [
          { "Name": "عربي", "Code": "ar", "English_Name": "Arabic", "Japanese_Name": "アラビア語" },
          {    ･
          {    ･
          {    ･
          { "Name": "中国語 (繁体字) ", "Code": "zh-TW", "English_Name": "Chinese (Traditional)", "Japanese_Name": "中国語 (繁体字) " }
          { "Name": "ＸＸＸＸ", "Code": "xx", "English_Name": "xxxx", "Japanese_Name": "ＸＸＸＸ語" }
        ]
        ─────────────────────────────────  

      手順 4 : SettingsForm に自動反映  
        LangManager が自動で読み込むため、コード変更は不要。  

### 11.2 新しいアイコンテーマの追加  
    Drive Indicator AI のアイコンは PNG ベースで、以下のフォルダ構造になっています :   

      Resources/  
        └─ Icons/  
              └─ Default/  
                    ├─ 16/  
                    └─ 32/  

#### 11.2.1 新テーマを追加する方法  
    1. 新しいテーマフォルダーを作成  

      Resources/  
        └─ Icons/  
              ├─ Default/  
              └─ MyTheme/  

    2. 新しいテーマフォルダー内に 16/ と 32/ フォルダーを作成  

      MyTheme/  
        ├─ 16/  ← W16×H16ドット用フォルダー  
        └─ 32/  ← W32×H32ドット用フォルダー  

    3. 16/ と 32/ に新しい PNG を配置  
      ファイル名は既存と同じにする (例 : write_off_read_off.png, write_off_read_on_.png)  

      MyTheme/  
        ├─ 16/  
        │   ├─ write_off_Read_off.png  ← W8×H16ドット Write OFF, Read OFF  
        │   ├─ write_off_Read_on_.png  ← W8×H16ドット Write OFF, Read ON  
        │   ├─ write_on__Read_off.png  ← W8×H16ドット Write ON , Read OFF  
        │   └─ write_on__Read_on_.png  ← W8×H16ドット Write ON , Read ON  
        └─ 32/
             ├─ write_off_Read_off.png  ← W16×H32ドット Write OFF, Read OFF  
             ├─ write_off_Read_on_.png  ← W16×H32ドット Write OFF, Read ON  
             ├─ write_on__Read_off.png  ← W16×H32ドット Write ON , Read OFF  
             └─ write_on__Read_on_.png  ← W16×H32ドット Write ON , Read ON  

    4. SettingsManager のアイコン画像フォルダ設定で新しいテーマフォルダーパスを指定  
      "アプリケーションフォルダー/Resources/Icons/MyTheme/"  

#### 11.2.2 メリット  
    ･ 再コンパイル不要  
    ･ ユーザーが自由にテーマを作れる  
    ･ DPI に応じて自動切り替えされる  

### 11.3 新しい監視方式の追加 (高度な拡張)  
    Drive Indicator AI の監視ロジックは以下の構造になっています :  

      DriveMonitor  
        ├─ PerfCounter (通常ドライブ)  
        └─ EtwRamIoMonitor (RAMドライブ)  

#### 11.3.1 新しい監視方式を追加する場合  
    例 : NVMe 専用 API、WMI、SMART 情報など  
      1. 新しい監視クラスを作成  
        ･ 例 : NvmeIoMonitor  
      2. DriveMonitor に統合  
        ･ Update() 内で新しい監視結果を DriveStatus に反映  
      3. DriveStatus に必要なプロパティを追加  

#### 11.3.2 設計上のメリット  
    ･ DriveMonitor が ｢統合ポイント｣ になっているため追加が容易  
    ･ UI や描画ロジックは変更不要  
    ･ ログで動作確認がしやすい  

### 11.4 設定項目の追加  
    SettingsManager は JSON ベースで柔軟に拡張できます。  

#### 11.4.1 手順  
    1. Settings クラスにプロパティを追加  
    2. SettingsManager.Load() / Save() に項目を追加  
    3. SettingsForm に UI を追加  
    4. TrayIconManager や DriveMonitor で参照  

#### 11.4.2 設計上のメリット  
    ･ 設定ファイルが壊れてもデフォルト値で復元  
    ･ UI とロジックが分離されているため安全  

### 11.5 UI の拡張 (SettingsForm の項目追加)  
    SettingsForm は ApplyLanguage() によって UI テキストが一括管理されているため、  
    項目を追加しても国際化が壊れにくい構造です。  

#### 11.5.1 拡張手順  
    1. コントロールを追加  
    2. Lang.T("Key") を使ってテキストを設定  
    3. SettingsManager と値を同期  

#### 11.5.2 注意点  
    ･ DPI サンプル描画を追加する場合は IconRenderer を利用  
    ･ Anchor / Dock を適切に設定して UI 崩れを防止  

### 11.6 アイコン描画ロジックの拡張  
    IconRenderer は以下のように責務が分離されています :  
      ･ PNG 読み込み  
      ･ DPI 判定  
      ･ DriveLetter 描画  
      ･ 色の合成  

#### 11.6.1 新しい描画要素を追加する場合  
    例 : I/O グラフ、アニメーション、使用率バーなど  
      1. RenderIcon() に描画処理を追加  
      2. DriveStatus に必要な情報を追加  
      3. SettingsManager に ON/OFF 設定を追加 (任意)  

#### 11.6.2 設計上のメリット  
    ･ 描画ロジックが 1 箇所に集約されている  
    ･ DPI 対応は既存の仕組みを流用できる  

### 11.7 拡張性のまとめ  
    Drive Indicator AI は、以下の点で非常に拡張しやすい設計になっています :  
      1. JSON ベースの国際化  
        → 言語追加が簡単  
      2. PNG ベースのアイコンテーマ  
        → 見た目を自由に変更可能  
      3. DriveMonitor の統合構造  
        → 新しい監視方式を追加しやすい  
      4. SettingsManager の柔軟性  
        → 設定項目の追加が容易  
      5. IconRenderer の責務分離  
        → 描画ロジックの拡張が安全  
      6. UI とロジックの完全分離  
        → 変更が他の部分に影響しにくい  
    Drive Indicator AI は、個人ユーティリティでありながら、  
    企業向けアプリ並みの拡張性を持つ設計になっています。  

[←前へ](10_i18n(en).md) | [次へ→](12_Limitations(en).md) | [先頭へ](00_Technical_Documents(en).md)  