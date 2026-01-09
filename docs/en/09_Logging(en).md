[←Previous](08_Startup(en).md) | [Next→](09_i18n(en).md) | [Top](00_Technical_documents(en).md)  

## 9\. Logging System
    Drive Indicator AI's logging function is designed to record internal app operations and exceptions,  
    allowing for quick identification of problems that occur in the user's environment.  
    Logs are managed in a dedicated directory under the TEMP folder using a 1MB rotation method.  
    This mechanism prevents logs from becoming bloated and ensures stable operation even over long periods of operation.  

### 9.1 Role of LogHelper  
    LogHelper is the component responsible for overall log management for Drive Indicator AI.  

      LogHelper  
        ├─ LogWrite()  
        ├─ ClearLog()  
        ├─ CreateLogFolder()  
        ├─ RotateLog()  
        └─ IsEnabled (ON/OFF via configuration)  

    Main functions :  
      • Log file generation  
      • Log writing (thread-safe)  
      • Rotation (automatic rotation when file size exceeds 1MB)  
      • Log folder management  
      • Log ON/OFF via configuration  

### 9.2 Log File Storage Location  
    Logs are stored in the TEMP folder.  

      %TEMP%\DriveIndicatorAI\Logs\  
        ├─ MessagesLog.log  
        └─ MessagesLog.old  

    Advantages of this structure  
      • No permissions required  
      • Suitable for portable apps  
      • Easy user access  
      • Safe to remain after uninstallation  

### 9.3 Log Writing (LogWrite)  
    LogWrite is a method for writing logs line by line.  

#### 9.3.1 Features  
    • Thread-safe (exclusive control using lock)  
    • Includes timestamps  
    • Safe operation even when exceptions occur  
    • Immediate return when logging is disabled  

#### 9.3.2 Log Format (Example)  
    ───────────────────────────────────────────────────────────────  
    [2025-12-12 14:33:21] DriveMonitor: Read=10240 Write=0 Drive=C  
    [2025-12-12 14:33:22] ETW: R drive write 4096 bytes  
    [2025-12-12 14:33:23] IconRenderer: DPI=144 IconSize=32px  
    ───────────────────────────────────────────────────────────────  

### 9.4 Rotation (RotateLog)  
    If the log file exceeds 1MB, the following process will be performed automatically :  
      MessagesLog.old ← Rename MessagesLog.log  
      MessagesLog.log ← Create a new file  

#### 9.4.1 Benefits of Rotation  
    • Logs do not become bloated  
    • Stable even over long periods of operation  
    • Only one generation of old logs is retained  

### 9.5 Log Usage Scenarios  
    Drive Indicator AI logs are useful in the following situations :  
      1. Investigating the cause when ETW does not work  
        • Session start error  
        • Event reception error  
      2. PerformanceCounter exception  
        • Instance does not exist  
        • Access rights issue  
      3. Icon drawing issue  
        • PNG loading error  
        • DPI detection issue  
      4. Auto-start issue  
        • schtasks.exe error output  
      5. Configuration file loading error  
        • JSON parsing error  
        • File corruption  

### 9.6 Design Features  
    Drive Indicator AI's log design has the following features :  
      1. Logging can be disabled  
          Logging can be turned off in SettingsManager, reducing unnecessary logging in the user environment.  
      2. Thread-safe  
          Safe for simultaneous writing from multiple threads, such as ETW threads, monitoring threads, and UI threads.  
      3. Rotation prevents bloat  
          Automatically switches when the file size exceeds 1MB.  
          Stable even for long-term operation.  
      4. Uses the TEMP folder  
          No permissions are required, allowing reliable writing in any environment.  
      5. Logs are retained even in the event of an exception  
          LogWrite is called within a try/catch block, making it easy to track down the cause of a problem when it occurs.  

### 9.7 Log Folder Operations (For Users)  
    The SettingsForm has an "Open Log Folder" button, allowing users to easily view the log.  
      • Logs can be attached when reporting issues.  
      • Developers can easily identify the cause.  

### 9.8 Log Design Summary  
    Drive Indicator AI's log system is :  
      • Lightweight  
      • Robust  
      • Thread-safe  
      • Includes rotation  
      • Highly debuggable  
    It is designed to combine practicality and quality.  
    It is a component that can be considered the "unsung hero" that supports the stability of the app.  

[←Previous](08_Startup(en).md) | [Next→](09_i18n(en).md) | [Top](00_Technical_documents(en).md)  