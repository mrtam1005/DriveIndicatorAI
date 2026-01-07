[←前へ](05_Dataflow(ja).md) | [次へ→](07_ETW(ja).md) | [先頭へ](00_Technical_documents(ja).md)  

## 6\. DPI 対応の詳細 (High-DPI Support)  
    Drive Indicator AI は、Windows の DPI 設定に応じて 16px / 32px のアイコンを自動切り替えし、  
    高解像度環境でも美しいアイコン表示を実現しています。  
    WinForms は DPI 対応が難しいことで有名ですが、Drive Indicator AI は 独自の DPI 設計により、  
    安定した描画と UI 表示を実現しています。  

### 6.1 DPI 対応の目的  
    Windows では DPI によってタスクトレイのアイコンサイズが変化します :  
      ┌─────┬─────┬────────┐  
      │   DPI    │ スケール │ アイコンサイズ │  
      ┝━━━━━┿━━━━━┿━━━━━━━━┥  
      │  96 DPI  │  100 %   │ W16×H16ドット │  
      ├─────┼─────┼────────┤  
      │ 120 DPI  │  125 %   │ W16×H16ドット │  
      ├─────┼─────┼────────┤  
      │ 144 DPI  │  150 %   │ W32×H32ドット │  
      ├─────┼─────┼────────┤  
      │ 192 DPI  │  200 %   │ W32×H32ドット │  
      └─────┴─────┴────────┘  
    Drive Indicator AI はこの仕様に合わせて、DPI に応じた最適なアイコンサイズを自動選択します。  

### 6.2 DPI 判定の仕組み  
    DPI 判定は以下の 2つの情報を組み合わせて行われます :  
      1. SettingsManager.SizeChangeDpi (固定値 144 DPI)  
      2. Graphics.DpiX (実行時 DPI)  

#### 6.2.1 DPI 判定ロジック (簡略版)  
      ──────────────────────────────────  
      if (dpi >= SettingsManager.SizeChangeDpi)  
          IconSize = 32px  
      else  
          IconSize = 16px  
      ──────────────────────────────────  
    SizeChangeDpi は設定ファイルで管理されており、デフォルトでは 144 DPI (150%) に設定されています。  

#### 6.3 IconRenderer による DPI 対応  
    IconRenderer は DPI に応じて以下を切り替えます :  
      • アイコンサイズ (16px / 32px)  
      • ドライブ文字のフォントサイズ  
      • PNG アイコンの読み込み先フォルダ (16 / 32)  
      • 描画位置 (DriveSize / LetterSize)  

#### 6.3.1 アイコンサイズの決定  
    ──────────────────────────────────  
    int iconSize = (dpi >= SizeChangeDpi) ? 32  : 16;  
    ──────────────────────────────────  

#### 6.3.2 PNG の読み込み先  
    ──────────────────────────────────  
    Icons/Default/16/xxx.png  
    Icons/Default/32/xxx.png  
    ──────────────────────────────────  

#### 6.3.3 描画位置の調整  
    DriveSize / LetterSize は DPI に応じて自動計算されるため、文字が潰れたり、位置がずれたりしない。  

### 6.4 FontHelper による DPI フォント管理  
    DPI 対応で最も難しいのが フォントサイズの管理です。  
    Drive Indicator AI では、GraphicsUnit.Pixel を使用することで   
    DPI の影響を受けないフォントを生成しています。  

#### 6.4.1 フォント生成のポイント  
    ──────────────────────────────────  
    new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);  
    ──────────────────────────────────  
      • Pixel 指定 → DPI による自動拡大縮小が発生しない  
      • 文字が潰れない  
      • アイコン内の DriveLetter が常に美しい  

#### 6.4.2 フォントキャッシュ  
    FontHelper は DPI ごとにフォントをキャッシュし、毎回生成しないことでパフォーマンスを向上。  

### 6.5 SettingsForm の DPI サンプル表示  
    SettingsForm の PictureBox では、実際のタスクトレイと同じロジックでサンプルアイコンを描画します。  
      • IconRenderer と同じ描画処理  
      • FontHelper の DPI フォント  
      • PNG 合成  
      • DriveLetter の描画  
    これにより、ユーザーは 設定変更前に見た目を確認できる。  

### 6.6 DPI 対応の設計思想 (まとめ)  
    Drive Indicator AI の DPI 対応は、以下の特徴を持っています :  
      1. WinForms の DPI 問題を回避  
        • 自動スケーリングを使わず、すべて手動で制御  
        • GraphicsUnit.Pixel による DPI 非依存フォント  
      2. アイコン描画が DPI 完全対応  
        • 16px / 32px の PNG を用意  
        • DPI に応じて自動切り替え  
        • DriveLetter の位置・サイズも DPI で調整  
      3. UI も DPI に強い  
        • SettingsForm のサンプル描画  
        • PictureBox の DPI 対応  
        • アイコンの見た目が常に安定  
      4. 拡張性が高い  
        • 新しい DPI (48px など) にも簡単に対応可能  
        • アイコンテーマの追加も容易  

[←前へ](05_Dataflow(ja).md) | [次へ→](07_ETW(ja).md) | [先頭へ](00_Technical_documents(ja).md)  