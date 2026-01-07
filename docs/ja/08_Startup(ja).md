[←前へ](07_ETW(ja).md) | [次へ→](09_Logging(ja).md) | [先頭へ](00_Technical_Documents(ja).md)  

## 8\. 自動実行 (StartupHelper)  
    Drive Indicator AI は、Windows 起動時に自動でアプリを実行するために、  
    タスクスケジューラ (Task Scheduler) を利用しています。  
    一般的なアプリが使う ｢レジストリ Run キー｣ ではなく、  
    あえてタスクスケジューラを採用した理由は以下の通りです :  
      • 権限不要 (管理者権限なしで登録可能)  
      • UAC の影響を受けない  
      • Windows Update やセキュリティソフトに削除されにくい  
      • 起動の安定性が高い  
      • 実行条件を細かく制御できる  
    Drive Indicator AI の自動実行は、実用性と堅牢性を最優先した設計になっています。  

### 8.1 StartupHelper の役割  
    StartupHelper は、自動実行の ON/OFF を管理するコンポーネントです。  

      StartupHelper  
        ├─ RegisterStartupTask()  
        ├─ UnregisterStartupTask()  
        ├─ IsStartupRegistered()  
        └─ CreateTaskXml()  

    主な役割は以下の通り :  
      • タスクスケジューラに登録する XML を生成  
      • schtasks.exe を使ってタスクを登録/削除  
      • 現在の登録状態を確認  
      • SettingsForm と連携して UI から操作可能にする  

### 8.2 タスクスケジューラ方式の仕組み  
    Drive Indicator AI は、以下のコマンドを内部で実行します :  

      ● 登録 (RegisterStartupTask)  
      ──────────────────────────────────  
      schtasks /Create /TN "DriveIndicatorAI" /XML "task.xml" /F  
      ──────────────────────────────────  

      ● 削除 (UnregisterStartupTask)  
      ──────────────────────────────────  
      schtasks /Delete /TN "DriveIndicatorAI" /F  
      ──────────────────────────────────  

      ● 登録確認 (IsStartupRegistered)  
      ──────────────────────────────────  
      schtasks /Query /TN "DriveIndicatorAI"  
      ──────────────────────────────────  

    これらはすべて 管理者権限不要 で実行できます。  

### 8.3 タスク XML の構造  
    StartupHelper は、タスクスケジューラに登録するための XML を動的に生成します。  
    XML の主な内容 :  
      • LogonTrigger  
          → ユーザーがログオンしたときに実行  
      • Exec Action  
          → DriveIndicatorAI.exe を実行  
      • RunLevel  
          → “LeastPrivilege” を指定 (管理者権限不要)  
      • WorkingDirectory  
          → アプリのフォルダを指定  

    ● XML の例 (概略)  
    ──────────────────────────────────  
    <Task>
      <Triggers>
        <LogonTrigger>
          <Enabled>true</Enabled>
          <Delay>PT10S</Delay>
        </LogonTrigger>
      </Triggers>

      <Principals>
        <Principal id=""Author"">
          <RunLevel>HighestAvailable</RunLevel>
        </Principal>
      </Principals>

      <Settings>
        <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>

        <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
        <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
        <RunOnlyIfIdle>false</RunOnlyIfIdle>
        <WakeToRun>false</WakeToRun>

        <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
        <Priority>7</Priority>
      </Settings>

      <Actions Context=""Author"">
        <Exec>
          <Command>""ApplicationPath\\DriveIndicatorAI.exe""</Command>
        </Exec>
      </Actions>
    </Task>
    ──────────────────────────────────  

    Drive Indicator AI はこの XML を TEMP に書き出し、  
    schtasks.exe に渡して登録します。  

### 8.4 自動実行の ON/OFF の流れ  
    SettingsForm の ｢Auto Start｣ チェックボックスを操作すると :  
    ──────────────────────────────────  
    ON  → StartupHelper.RegisterStartupTask()  
    OFF → StartupHelper.UnregisterStartupTask()  
    ──────────────────────────────────  

    ● ON の場合  
      1. XML を生成  
      2. TEMP に保存  
      3. schtasks.exe /Create を実行  
      4. 成功したら設定ファイルに保存  

    ● OFF の場合  
      1. schtasks.exe /Delete を実行  
      2. 設定ファイルに保存  

### 8.5 設計上の工夫  
    Drive Indicator AI の自動起動は、以下の点で非常に堅牢です。  
      1. 管理者権限不要  
          RunLevel=LeastPrivilege のため、一般ユーザー権限で登録可能。  
      2. Windows Update で消されにくい  
          レジストリ Run キーは OS 更新で消えることがあるが、タスクスケジューラは影響を受けにくい。  
      3. 実行フォルダを正しく設定  
          WorkingDirectory を指定することで、相対パスのリソース読み込みが安定。  
      4. XML を動的生成  
          アプリのパスが変わっても対応可能。  
      5. エラー時はログに記録  
          schtasks のエラー出力を LogHelper に記録するため、問題発生時の調査が容易。  

### 8.6 動作確認方法 (開発者向け)  
    1. SettingsForm → Auto Start を ON  
    2. Windows の ｢タスクスケジューラ｣ を開く  
    3. ｢タスクスケジューラライブラリ｣ に DriveIndicatorAI\_AutoStart が登録されていることを確認  
    4. Windows を再起動  
    5. タスクトレイに DriveIndicatorAI が表示されることを確認  

### 8.7 自動実行設計のまとめ  
    Drive Indicator AI の自動実行は :  
      • 安全  
      • 権限不要  
      • 安定  
      • 拡張性が高い  
    という、非常に優れた設計になっています。  
    一般的なユーティリティよりも一段上の品質で、企業向けアプリでも通用するレベルです。  

[←前へ](07_ETW(ja).md) | [次へ→](09_Logging(ja).md) | [先頭へ](00_Technical_Documents(ja).md)  