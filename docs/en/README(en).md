# Drive Indicator AI - README(en)

[Technical documents can be found here](00_Technical_documents(en).md)

## 1\. Overview
    Drive Indicator AI is a lightweight utility 
    that displays real-time drive I/O (read/write) status in Windows using a task tray icon.
      • SSD / HDD / USB / SD card
      • RAM drive (high-precision monitoring using ETW)
      • Simultaneous monitoring of multiple drives
      • DPI (high-resolution) support
      • Multilingual support (Japanese, English, etc.)
      • Automatic startup (Task Scheduler method)
      • Logging function (with rotation)
    Designed for users who want to visually understand Windows storage activity.

## 2\. Key Features
    1. Real-time drive I/O display
      • Read  → Bright green / Dark red
      • Write → Dark green   / Bright red
      • Both  → Bright green / Bright red
      • Idle  → Dark green   / Dark red
      Every two drives are displayed as a single icon, 
      so even with a large number of drives, the task tray won't be overwhelmed.

    2. Accurate RAM Drive Monitoring (ETW Support)
      ETW (Event Tracing for Windows) is used to accurately monitor RAM drive I/O, 
      which cannot be captured using standard PerformanceCounters.

    3. Full DPI Support (Automatic 16px/32px Switching)
      • 100% (96 DPI)            → 16x16 Icon
      • 150% (144 DPI) and above → 32x32 Icon
      Beautiful icon display even in high-DPI environments.

    4. Multilingual Support
      Supports Japanese, English, German, French, Italian, Spanish, 
      Dutch, Portuguese, Russian, Korean, Simplified Chinese, 
      Traditional Chinese, Arabic, Czech, Danish, Greek, 
      Finnish, Hebrew, Hindi, Hungarian, 
      Indonesian, Malay, Norwegian, Polish, Romanian, 
      Swedish, Thai, Turkish, Ukrainian, and Vietnamese.


      "Application Folder\Resources\Language\languages.json" file
      "Application Folder\Resources\Language\lang_*.json" file
      New languages ​​can be easily added by simply editing these two files.

    5. Automatic Startup (Task Scheduler Method)
      Registering the task scheduler using schtasks.exe 
      instead of the registry Run key:
        • No privileges required
        • Not affected by UAC
        • Less likely to be deleted by Windows Update

    6. Logging (1MB Rotation)
      • Saved to "%TEMP%\DriveIndicator\Logs\MessagesLog.log"
      • Automatically rotates to "MessagesLog.old" when it exceeds 1MB
      • Detailed recording of exceptions and internal operations
      * Log writing may slow down operation.

## 3\. Installation Instructions
    1. Download and install .NET Runtime 10.0.1 (for Windows) .
    2. Download the Drive Indicator AI ZIP file from the Release page.
    3. Extract the files to a folder of your choice.
    4. Run DriveIndicatorAI.exe.
   [1. .NET Runtime 10.0.1 (for Windows) is available here](https://dotnet.microsoft.com/ja-jp/download/dotnet/10.0)  
   [2. Drive Indicator AI Release page is available here](https://github.com/mrtam1005/DriveIndicatorAI)  

## 4\. How to Use
    When you run DriveIndicatorAI.exe, an icon will appear in the task tray.

## 5\. Settings
    Right-click the task tray icon → Settings

    <Settable Items>
    1. Enable/Disable Automatic Run at Windows Startup
        Enabled: A task called "DriveIndicatorAI_AutoStart" will be registered in the Task Scheduler.
        Disabled: The task called "DriveIndicatorAI_AutoStart" will be removed from the Task Scheduler.
      Default : Disabled

    2. Display Drive Selection: Enable/Disable for each drive letter
      Default : C

    3. Display Interval [msec] (Range: 1-250 msec)
      Default : 50 msec

    4. Icon Image Folder
        ･ Icon images for resolutions of 144 DPI or higher: 
          W16xH32 dots PNG files in "Application Folder\Resources\Icons\[Theme Name]\32" folder
        ･ Icon images for resolutions below 144 DPI: 
          W8xH16  dots PNG files in "Application Folder\Resources\Icons\[Theme Name]\16" folder
        Save these files and select the "Theme Name" folder.
        Save PNG files in each folder with the following filenames:
          read off / write off : write_off_read_off.png
          read on  / write off : write_off_read_on_.png
          read off / write on  : write_on__read_off.png
          read on  / write on  : write_on__read_on_.png
      Default : "Application Folder\Resources\Icons\Default"

    5. Drive Letter Color
      Default : "#BFBFBF" (R=191,G=191,B=191 Silver)

    6. Language
      Default : "ja" (Japanese)

    7. Logging : Enable/Disable
      Default : Disable

    Click the "Save"   button to apply the settings.
    Click the "Cancel" button to discard the settings.

## 6\. Supported OS
    ･ Windows 10 64-bit
    ･ Windows 11 64-bit

## 7\. Folder Structure
    1. Application Folder
      DriveIndicatorAI/            Application Folder
      ├─ DriveIndicatorAI.exe    Application File
      ├─ settings.json           Settings File (Auto-Generated)
      └─ Resources/
           ├─ Language/
           │   ├─ lang_ja.json        Language File (Japanese)
           │   ├─ lang_en.json        Language File (English)
           │   ├─ lang_de.json        Language File (German)
           │   ├─ lang_**.json         ･
           │   ├─ lang_**.json         ･
           │   ├─ lang_**.json         ･
           │   ├─ lang_**.json         ･
           │   └─ languages.json      Language List File
           └─ Icons/
                ├─ app.ico
                ├─ appIcon.bmp
                └─ Default/
                     ├─ 16/            Icon image folder for resolutions below 144 DPI
                     │   ├─ write_off_read_off.png    W8xH16 PNG file (write off, read off)
                     │   ├─ write_off_read_on_.png    W8xH16 PNG file (write off, read on )
                     │   ├─ write_on__read_off.png    W8xH16 PNG file (write on , read off)
                     │   └─ write_on__read_on_.png    W8xH16 PNG file (write on , read on )
                     └─ 32/            Icon image folder for resolutions above 144 DPI
                          ├─ write_off_read_off.png    W16xH32 PNG file (write off, read off)
                          ├─ write_off_read_on_.png    W16xH32 PNG file (write off, read on )
                          ├─ write_on__read_off.png    W16xH32 PNG file (write on , read off)
                          └─ write_on__read_on_.png    W16xH32 PNG file (write on , read on )

    2. Temporary Folder
      %TEMP%/                                Temporary folder (Temporary folder set in Windows)
      └─ DriveIndicatorAI/                 Application folder
           ├─ DriveIndicatorAI_Task.xml    Task Scheduler properties file
           ├─ settings.json                Settings file
           ├─ Icons/
           │   ├─ 16/                            Copy of icon image folder for resolutions below 144 DPI
           │   │   ├─ write_off_read_off.png
           │   │   ├─ write_off_read_on_.png
           │   │   ├─ write_on__read_off.png
           │   │   └─ write_on__read_on_.png
           │   └─ 32/                            Copy of icon image folder for resolutions above 144 DPI
           │        ├─ write_off_read_off.png
           │        ├─ write_off_read_on_.png
           │        ├─ write_on__read_off.png
           │        └─ write_on__read_on_.png
           └─ Logs/
                ├─ MessagesLog.log    Log file (automatically generated)
                └─ MessagesLog.old    Old log file (automatically generated if the log file size exceeds 1MB)

## 8\. Technical Information (for Developers)
    1. I/O Monitoring
      ･ Normal Drive: PerformanceCounter (LogicalDisk)
      ･ RAM Drive: ETW (FileIORead / FileIOWrite)
    2. DPI Support
      ･ IconRenderer detects DPI and automatically switches icon size between 16px and 32px
      ･ Fonts are handled using GraphicsUnit.Pixel to eliminate DPI dependency
    3. Logging
      ･ Saved to %TEMP% folder
      ･ 1MB rotation
      ･ Exceptions are also output to Debug.WriteLine
    4. Multilingual
      ･ JSON file-based
      ･ LangManager loaded
      ･ Displayed in Japanese if not found

## 9\. License
    1. License
      This software is provided free of charge.
      Personal and non-commercial use is permitted.
      For details, please see the License dialog in the app.

    2. Developer
      mrtam1005
      Github:
        https://github.com/mrtam1005/DriveIndicatorAI
   [Github: "mrtam1005/DriveIndicatorAI" is available here](https://github.com/mrtam1005/DriveIndicatorAI)  

## Update History

