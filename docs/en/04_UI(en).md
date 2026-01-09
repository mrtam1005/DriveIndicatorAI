[←Previous](03_Components(en).md) | [Next→](05_Dataflow(en).md) | [Top](00_Technical_documents(en).md)  

## 4\. UI Components (User Interface Components)  
    Drive Indicator AI's UI is designed to provide necessary functionality,  
    such as changing settings, checking the version, and checking the license, while minimizing user interaction.  
    The UI consists of three components :  
      1. SettingsForm (Settings Screen)  
      2. VersionInfoForm (Version Information)  
      3. LicenseDialog (License Display)  
    All of these are launched from the TrayIconManager context menu and are loosely coupled with the app's main logic.  

### 4.1 SettingsForm — Settings Screen  

#### 4.1.1 Role  
    ･ UI for users to change app settings  
    ･ Works with SettingsManager to read and write settings  
    ･ The most complex UI in Drive Indicator AI, including language switching and DPI sample display  

#### 4.1.2 Main Functions  
    ･ Selecting the drive to monitor  
    ･ Setting the display interval (update cycle)  
    ･ Selecting the icon folder  
    ･ Setting the drive letter color  
    ･ Language selection (Japanese/English)  
    ･ Automatic startup (Task Scheduler registration)  
    ･ Enabling logging  
    ･ Drawing sample icons according to DPI (PictureBox)  

#### 4.1.3 Technical Points
    1. Language switching (ApplyLanguage)  
          ･ Combo box language name  
          ･ Label and button text  
          ･ Message box text  
        All JSON The DPI settings are retrived from the translation table.  
    2. Drawing the DPI Sample Icon  
        The sample icon displayed in the PictureBox is drawn using:  
          ･ The same logic as IconRenderer  
          ･ DPI fonts from FontHelper  
          ･ PNG icon composition  
        This allows you to check in advance how the icon will look in the task tray.  
    3. Saving the Settings  
        Clicking the OK button calls SettingsManager.Save(), and the settings are persisted in a settings file (JSON).  

### 4.2 VersionInfoForm — Version Information Screen  

#### 4.2.1 Role  
    ･ Display app version information  
    ･ Open Github links  
    ･ Open license dialog  

#### 4.2.2 Main Functions  
    ･ Get version number from FileVersionInfo  
    ･ Open Github links in the default browser  
    ･ Display LicenseDialog non-modally  

#### 4.2.3 Technical Points  
    1. Obtaining the version using FileVersionInfo  
      ───────────────────────────────────────────────────────────────────────  
      FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion  
      ───────────────────────────────────────────────────────────────────────  
      → Using FileVersion instead of AssemblyVersion allows for more flexible management of the version number  
        shown to the user.  
    2. Non-Modal Dialog Management  
      LicenseDialog is reused and controlled to prevent multiple openings.  

### 4.3 LicenseDialog — License Display Dialog  

#### 4.3.1 Role  
    ･ Displays the license text  
    ･ Closes with the OK button  

#### 4.3.2 Main Functions  
    ･ Displays the license text in Japanese or English depending on the language code  
    ･ textBoxLicense is ReadOnly + ScrollBars.Vertical  
    ･ Standardizes the closing method by setting ControlBox = false  

#### 4.3.3 Technical Points  
    1. The license text is hard-coded  
      Internationalization is controlled directly within LicenseDialog, not via JSON.  
      Reasons:  
        ･ The license text is part of the app, so it's safer not to rely on an external file.  
        ･ It's not affected by JSON corruption or translation errors.  
    2. UI structure that's less susceptible to breakage.  
      ･ Text box with DockStyle.Top.  
      ･ OK button fixed with Anchor.  
      ･ ControlBox = false provides a clear UX.  

### 4.4 UI Component Design Philosophy (Summary)  
    Drive Indicator AI's UI has the following features :  
      ･ Complete separation of logic and UI.  
          Settings changes are handled by SettingsManager, monitoring is handled by DriveMonitor,  
          and the UI is solely focused on input and display.  
      ･ Multilingual support is naturally built in.  
          JSON-based translation by LangManager.  
      ･ DPI support is reflected in the UI.  
          DPI differences can be confirmed by drawing sample icons.  
      ･ Unbreakable UI  
          Avoids WinForms pitfalls such as Dispose, Anchor, Dock, and ControlBox.  

[←Previous](03_Components(en).md) | [Next→](05_Dataflow(en).md) | [Top](00_Technical_documents(en).md)  