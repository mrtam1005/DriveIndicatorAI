[←Previous](10_i18n(en).md) | [Next→](12_Limitations(en).md) | [Top](00_Technical_documents(en).md)  

## 11. Extensibility Guide  
    Drive Indicator AI clearly separates the UI, monitoring logic,  
    rendering logic, internationalization, etc., making it easy to add features and customize later.  
    This chapter summarizes the key points developers need to know when extending Drive Indicator AI.  

### 11.1 Adding a New Language (i18n Extension)  
    Drive Indicator AI's multilingual support is JSON-based, making it very easy to extend.  

      Step 1: Copy the Language File  
        Copy Resources/Language/lang_en.json and rename it to something like lang_xx.json.  

      Step 2: Write the Translation  
        Simply translate the values ​​in lang_xx.json.  

      Step 3: Add to languages.json  
        ───────────────────────────────────────────────────────────────────────────────────────────  
        [
          { "Name": "عربي", "Code": "ar", "English_Name": "Arabic", "Japanese_Name": "Arabic" },
          { ･
          { ･
          { ･
          { "Name": "Chinese (Traditional)", "Code": "zh-TW", "English_Name": "Chinese (Traditional)", "Japanese_Name": "Chinese (Traditional)" }
          { "Name": "XXXXX", "Code": "xx", "English_Name": "xxxx", "Japanese_Name": "XXXXX" }
        ]
        ───────────────────────────────────────────────────────────────────────────────────────────  

      Step 4: Automatically reflect in SettingsForm  
        LangManager is loaded automatically, so no code changes are required.  

### 11.2 Adding a New Icon Theme  
    Drive Indicator AI icons are PNG-based and have the following folder structure:  

      Resources/  
        └─ Icons/  
               └─ Default/  
                      ├─ 16/  
                      └─ 32/  

#### 11.2.1 How to Add a New Theme  
    1. Create a new theme folder  

      Resources/  
        └─ Icons/  
               ├─ Default/  
               └─ MyTheme/  

    2. Create 16/ and 32/ folders within the new theme folder  

      MyTheme/  
        ├─ 16/  ← Folder for W16×H16 dots  
        └─ 32/  ← Folder for W32×H32 dots  

    3. Place new PNGs in 16/ and 32/  
      Use the same file names as the existing ones.  
      (Example: write_off_read_off.png, write_off_read_on_.png)  

      MyTheme/  
        ├─ 16/  
        │    ├─ write_off_Read_off.png ← W8×H16 dots Write OFF, Read OFF  
        │    ├─ write_off_Read_on_.png ← W8×H16 dots Write OFF, Read ON  
        │    ├─ write_on__Read_off.png ← W8×H16 dots Write ON , Read OFF  
        │    └─ write_on__Read_on_.png ← W8×H16 dots Write ON , Read ON  
        └─ 32/
              ├─ write_off_Read_off.png ← W16×H32 dots Write OFF, Read OFF  
              ├─ write_off_Read_on_.png ← W16×H32 dots Write OFF, Read ON  
              ├─ write_on__Read_off.png ← W16×H32 dots Write ON , Read OFF  
              └─ write_on__Read_on_.png ← W16×H32 dots Write ON , Read ON  

    4. Specify the new theme folder path in the SettingsManager icon image folder settings.  
      "Application folder/Resources/Icons/MyTheme/"  

#### 11.2.2 Benefits  
    · No recompilation required  
    · Users can freely create their own themes  
    · Automatically switches based on DPI  

### 11.3 Adding a New Monitoring Method (Advanced Extension)  
    The Drive Indicator AI monitoring logic has the following structure :  

      DriveMonitor  
        ├─ PerfCounter (normal drive)  
        └─ EtwRamIoMonitor (RAM drive)  

#### 11.3.1 Adding a New Monitoring Method  
    Example : NVMe-specific API, WMI, SMART information, etc.  
      1. Create a new monitoring class  
        ･ Example: NvmeIoMonitor  
      2. Integrate into DriveMonitor  
        • Reflect new monitoring results in DriveStatus within Update()  
      3. Add necessary properties to DriveStatus  

#### 11.3.2 Design Benefits  
    • DriveMonitor is the "integration point," making additions easy  
    • No changes required to the UI or drawing logic  
    • Operation can be easily verified using logs  

### 11.4 Adding Settings  
    SettingsManager is JSON-based and can be flexibly extended.  

#### 11.4.1 Procedure  
    1. Add properties to the Settings class  
    2. Add items to SettingsManager.Load() / Save()  
    3. Add UI to SettingsForm  
    4. Reference with TrayIconManager or DriveMonitor  

#### 11.4.2 Design Benefits  
    ･ Restores default values ​​even if the settings file is corrupted  
    ･ Safety due to separation of UI and logic  

### 11.5 UI Extending (Adding Items to SettingsForm)  
    SettingsForm manages UI text centrally using ApplyLanguage(),  
    so adding items does not disrupt internationalization.  

#### 11.5.1 Extension Procedure  
    1. Add a control  
    2. Set the text using Lang.T("Key")  
    3. Synchronize values ​​with SettingsManager  

#### 11.5.2 Notes  
    ･ Use IconRenderer when adding DPI sample drawing  
    ･ Set Anchor/Dock appropriately to prevent UI collapse  

### 11.6 Extending Icon Drawing Logic  
    IconRenderer's responsibilities are separated as follows :  
      ･ PNG loading  
      ･ DPI detection  
      ･ DriveLetter drawing  
      ･ Color blending  

#### 11.6.1 Adding New Drawing Elements  
    Examples: I/O graphs, animations, utilization bars, etc.  
      1. Add drawing processing to RenderIcon()  
      2. Add necessary information to DriveStatus  
      3. Add ON/OFF settings to SettingsManager (optional)  

#### 11.6.2 Design Benefits  
    ･ Drawing logic is centralized in one place  
    ･ DPI support can utilize existing mechanisms  

### 11.7 Scalability Summary  
    Drive Indicator AI is designed to be highly extensible in the following ways :  
      1. JSON-based internationalization  
        → Adding languages ​​is easy  
      2. PNG-based icon themes  
        → Appearance can be freely changed  
      3. DriveMonitor's integrated structure  
        → Easily adding new monitoring methods  
      4. SettingsManager flexibility  
        → Easily adding settings  
      5. IconRenderer separation of responsibilities  
        → Safely extending drawing logic  
      6. Complete separation of UI and logic  
        → Changes are less likely to affect other parts  
    Although Drive Indicator AI is a personal utility,  
    it is designed to be extensible on the same level as an enterprise app.  

[←Previous](10_i18n(en).md) | [Next→](12_Limitations(en).md) | [Top](00_Technical_documents(en).md)  