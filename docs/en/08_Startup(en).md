[←Previous](07_ETW(en).md) | [Next→](09_Logging(en).md) | [Top](00_Technical_documents(en).md)  

## 8\. Autorun (StartupHelper)  
    Drive Indicator AI uses Task Scheduler to automatically run the application when Windows starts.  
    We chose Task Scheduler instead of the "Registry Run key" used by most applications for the following reasons :  
      • No permissions required (can be registered without administrator privileges)  
      • Not affected by UAC  
      • Less likely to be deleted by Windows Update or security software  
      • High startup stability  
      • Detailed control over execution conditions  
    Drive Indicator AI's autorun feature is designed with practicality and robustness as its top priorities.  

### 8.1 Role of StartupHelper  
    StartupHelper is a component that manages the on/off status of autorun.  

      StartupHelper  
      ├─ RegisterStartupTask()  
      ├─ UnregisterStartupTask()  
      ├─ IsStartupRegistered()  
      └─ CreateTaskXml()  

     Its main functions are as follows :  
      • Generates XML to be registered in the Task Scheduler  
      • Registers/deletes tasks using schtasks.exe  
      • Checks the current registration status  
      • Works with SettingsForm to enable operation from the UI  

### 8.2 Task Scheduler Method Mechanism  
    Drive Indicator AI internally executes the following commands :  

      ● Registration (RegisterStartupTask)  
      ───────────────────────────────────────────────────────────  
      schtasks /Create /TN "DriveIndicatorAI" /XML "task.xml" /F  
      ───────────────────────────────────────────────────────────  

      ● Deletion (UnregisterStartupTask)  
      ───────────────────────────────────────────────────────────  
      schtasks /Delete /TN "DriveIndicatorAI" /F  
      ───────────────────────────────────────────────────────────  

      ● Registration Verification (IsStartupRegistered)  
      ───────────────────────────────────────────────────────────  
      schtasks /Query /TN "DriveIndicatorAI"  
      ───────────────────────────────────────────────────────────  

    All of these can be performed without administrator privileges.  

### 8.3 Task XML Structure  
    StartupHelper dynamically generates the XML for registration with the Task Scheduler.  
    XML Key Contents :  
      • LogonTrigger  
          → Executes when the user logs on  
      • Exec Action  
          → Executes DriveIndicatorAI.exe  
      • RunLevel  
          → Specify "LeastPrivilege" (Administrator privileges not required)  
      • WorkingDirectory  
          → Specify the app's folder  

    ● XML Example (Outline)  
    ──────────────────────────────────────────────────────────────────  
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
          <Command>""ApplicationPath\DriveIndicatorAI.exe""</Command>
        </Exec>
      </Actions>
    </Task>
    ──────────────────────────────────────────────────────────────────  

    Drive Indicator AI writes this XML to TEMP and passes it to schtasks.exe for registration.  

### 8.4 How to Turn Auto Start On/Off  
    When you operate the "Auto Start" checkbox in the SettingsForm :  
    ────────────────────────────────────────────  
    ON → StartupHelper.RegisterStartupTask()  
    OFF → StartupHelper.UnregisterStartupTask()  
    ────────────────────────────────────────────  

    ● If ON  
      1. Generate XML  
      2. Save to TEMP  
      3. Execute schtasks.exe /Create  
      4. If successful, save to the settings file  

    ● If OFF  
      1. Execute schtasks.exe /Delete  
      2. Save to the settings file  

### 8.5 Design Features  
    Drive Indicator AI's auto-start feature is extremely robust in the following ways :  
      1. No administrator privileges required  
          Since RunLevel=LeastPrivilege, it can be registered with general user privileges.  
      2. Not easily deleted by Windows Update  
          Registry Run keys may be deleted by OS updates, but Task Scheduler is less affected.  
      3. Correctly set the execution folder  
          Specifying the WorkingDirectory ensures stable loading of relative path resources.  
      4. Dynamically generate XML  
          Can handle changes to the application path.  
      5. Errors are logged  
          schtasks error output is recorded in LogHelper, making it easy to investigate problems when they occur.  

### 8.6 How to Test Operation (For Developers)  
    1. Turn on Auto Start in SettingsForm  
    2. Open the Windows Task Scheduler  
    3. Verify that DriveIndicatorAI\_AutoStart is registered in the Task Scheduler Library  
    4. Restart Windows  
    5. Verify that DriveIndicatorAI appears in the task tray  

### 8.7 Summary of Auto-Execution Design  
    Drive Indicator AI's auto-execution design is :  
      • Secure  
      • No permission required  
      • Stable  
      • Highly scalable  
    It has an excellent design.  
    It is a step above general utilities in quality and is suitable for enterprise applications.  

[←Previous](07_ETW(en).md) | [Next→](09_Logging(en).md) | [Top](00_Technical_documents(en).md)  