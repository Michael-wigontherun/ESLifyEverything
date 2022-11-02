using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ESLifyEverythingGlobalDataLibrary.Properties
{
#pragma warning disable CS0618 // Type or member is obsolete
    //Static class to update settings
    public static class UAppSettings
    {
        //Updates and logs the AppSettings.json file
        public static void UpdateSettingsFile()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddEnvironmentVariables().Build();
            switch (config.GetRequiredSection("SettingsVersion").Get<string>())
            {
                case "1.9":
                    GF.WriteLine(GF.stringLoggingData.GenSettingsFile);
                    GF.WriteLine(GF.stringLoggingData.EditYourSettings);
                    UAppSettings.AppSettings(config.GetRequiredSection("Settings").Get<AppSettings19>()).Build();
                    break;
                case "3.0.0":
                    GF.WriteLine(GF.stringLoggingData.GenSettingsFile);
                    GF.WriteLine(GF.stringLoggingData.EditYourSettings);
                    UAppSettings.AppSettings(config.GetRequiredSection("Settings").Get<AppSettings3>()).Build();
                    break;
                case "3.2.0":
                    GF.WriteLine(GF.stringLoggingData.GenSettingsFile);
                    GF.WriteLine(GF.stringLoggingData.EditYourSettings);
                    UAppSettings.AppSettings(config.GetRequiredSection("Settings").Get<AppSettings320>()).Build();
                    break;
                case "3.5.0":
                    GF.WriteLine(GF.stringLoggingData.GenSettingsFile);
                    GF.WriteLine(GF.stringLoggingData.EditYourSettings);
                    UAppSettings.AppSettings(config.GetRequiredSection("Settings").Get<AppSettings350>()).Build();
                    break;
                case "3.5.2":
                    GF.WriteLine(GF.stringLoggingData.GenSettingsFile);
                    GF.WriteLine(GF.stringLoggingData.EditYourSettings);
                    UAppSettings.AppSettings(config.GetRequiredSection("Settings").Get<AppSettings352>()).Build();
                    break;
                default:
                    GF.GenerateSettingsFileError();
                    break;
            }
        }

        public static AppSettings AppSettings(AppSettings19 appSettings19)

        {
            AppSettings appSettings = new AppSettings();
            appSettings.XEditLogFileName = appSettings19.XEditLogFileName;
            appSettings.PapyrusFlag = appSettings19.PapyrusFlag;
            appSettings.VerboseConsoleLoging = appSettings19.VerboseConsoleLoging;
            appSettings.VerboseFileLoging = appSettings19.VerboseFileLoging;
            appSettings.AutoReadNewestxEditSession = appSettings19.AutoReadNewestxEditSeesion;
            appSettings.AutoReadAllxEditSession = appSettings19.AutoReadAllxEditSeesion;
            appSettings.AutoRunESLify = appSettings19.AutoRunESLify;
            appSettings.AutoRunScriptDecompile = appSettings19.AutoRunScriptDecompile;
            appSettings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession = appSettings19.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion;
            appSettings.XEditFolderPath = appSettings19.XEditFolderPath;
            appSettings.DataFolderPath = appSettings19.DataFolderPath;
            appSettings.OutputFolder = appSettings19.OutputFolder;
            return appSettings;
        }

        public static AppSettings AppSettings(AppSettings3 appSettings3)
        {
            AppSettings appSettings = new AppSettings();
            appSettings.XEditLogFileName = appSettings3.XEditLogFileName;
            appSettings.PapyrusFlag = appSettings3.PapyrusFlag;
            appSettings.VerboseConsoleLoging = appSettings3.VerboseConsoleLoging;
            appSettings.VerboseFileLoging = appSettings3.VerboseFileLoging;
            appSettings.AutoReadNewestxEditSession = appSettings3.AutoReadNewestxEditSeesion;
            appSettings.AutoReadAllxEditSession = appSettings3.AutoReadAllxEditSeesion;
            appSettings.AutoRunESLify = appSettings3.AutoRunESLify;
            appSettings.AutoRunScriptDecompile = appSettings3.AutoRunScriptDecompile;
            appSettings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession = appSettings3.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion;
            appSettings.RunSubPluginCompaction = appSettings3.RunSubPluginCompaction;
            appSettings.ChangedPluginsOutputToDataFolder = appSettings3.ChangedPluginsOutputToDataFolder;
            appSettings.MO2.MO2Support = appSettings3.MO2Support;
            appSettings.MO2.MO2ModFolder = appSettings3.MO2ModFolder;
            appSettings.XEditFolderPath = appSettings3.XEditFolderPath;
            appSettings.DataFolderPath = appSettings3.DataFolderPath;
            appSettings.OutputFolder = appSettings3.OutputFolder;
            return appSettings;
        }

        public static AppSettings AppSettings(AppSettings320 appSettings320)
        {
            AppSettings appSettings = new AppSettings();
            appSettings.XEditLogFileName = appSettings320.XEditLogFileName;
            appSettings.PapyrusFlag = appSettings320.PapyrusFlag;
            appSettings.VerboseConsoleLoging = appSettings320.VerboseConsoleLoging;
            appSettings.VerboseFileLoging = appSettings320.VerboseFileLoging;
            appSettings.AutoReadNewestxEditSession = appSettings320.AutoReadNewestxEditSeesion;
            appSettings.AutoReadAllxEditSession = appSettings320.AutoReadAllxEditSeesion;
            appSettings.AutoRunESLify = appSettings320.AutoRunESLify;
            appSettings.AutoRunScriptDecompile = appSettings320.AutoRunScriptDecompile;
            appSettings.EnableCompiler = appSettings320.EnableCompiler;
            appSettings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession = appSettings320.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion;
            appSettings.RunSubPluginCompaction = appSettings320.RunSubPluginCompaction;
            appSettings.ChangedPluginsOutputToDataFolder = appSettings320.ChangedPluginsOutputToDataFolder;
            appSettings.MO2.MO2Support = appSettings320.MO2Support;
            appSettings.MO2.MO2ModFolder = appSettings320.MO2ModFolder;
            appSettings.XEditFolderPath = appSettings320.XEditFolderPath;
            appSettings.DataFolderPath = appSettings320.DataFolderPath;
            appSettings.OutputFolder = appSettings320.OutputFolder;
            return appSettings;
        }

        public static AppSettings AppSettings(AppSettings350 appSettings350)
        {
            AppSettings appSettings = new AppSettings();
            appSettings.XEditLogFileName = appSettings350.XEditLogFileName;
            appSettings.PapyrusFlag = appSettings350.PapyrusFlag;
            appSettings.VerboseConsoleLoging = appSettings350.VerboseConsoleLoging;
            appSettings.VerboseFileLoging = appSettings350.VerboseFileLoging;
            appSettings.AutoReadNewestxEditSession = appSettings350.AutoReadNewestxEditSession;
            appSettings.AutoReadAllxEditSession = appSettings350.AutoReadAllxEditSession;
            appSettings.AutoRunESLify = appSettings350.AutoRunESLify;
            appSettings.AutoRunScriptDecompile = appSettings350.AutoRunScriptDecompile;
            appSettings.EnableCompiler = appSettings350.EnableCompiler;
            appSettings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession = appSettings350.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession;
            appSettings.RunSubPluginCompaction = appSettings350.RunSubPluginCompaction;
            appSettings.ChangedPluginsOutputToDataFolder = appSettings350.ChangedPluginsOutputToDataFolder;
            appSettings.XEditFolderPath = appSettings350.XEditFolderPath;
            appSettings.DataFolderPath = appSettings350.DataFolderPath;
            appSettings.OutputFolder = appSettings350.OutputFolder;
            appSettings.MO2.MO2Support = appSettings350.MO2.MO2Support;
            appSettings.MO2.MO2ModFolder = appSettings350.MO2.MO2ModFolder;
            return appSettings;
        }

        public static AppSettings AppSettings(AppSettings352 appSettings352)
        {
            AppSettings appSettings = new AppSettings();
            appSettings.XEditLogFileName = appSettings352.XEditLogFileName;
            appSettings.PapyrusFlag = appSettings352.PapyrusFlag;
            appSettings.VerboseConsoleLoging = appSettings352.VerboseConsoleLoging;
            appSettings.VerboseFileLoging = appSettings352.VerboseFileLoging;
            appSettings.AutoReadNewestxEditSession = appSettings352.AutoReadNewestxEditSession;
            appSettings.AutoReadAllxEditSession = appSettings352.AutoReadAllxEditSession;
            appSettings.AutoRunESLify = appSettings352.AutoRunESLify;
            appSettings.AutoRunScriptDecompile = appSettings352.AutoRunScriptDecompile;
            appSettings.EnableCompiler = appSettings352.EnableCompiler;
            appSettings.EnableLargeMerges = appSettings352.EnableLargeMerges;
            appSettings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession = appSettings352.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession;
            appSettings.RunSubPluginCompaction = appSettings352.RunSubPluginCompaction;
            appSettings.ChangedPluginsOutputToDataFolder = appSettings352.ChangedPluginsOutputToDataFolder;
            appSettings.XEditFolderPath = appSettings352.XEditFolderPath;
            appSettings.DataFolderPath = appSettings352.DataFolderPath;
            appSettings.OutputFolder = appSettings352.OutputFolder;
            appSettings.MO2.MO2Support = appSettings352.MO2.MO2Support;
            appSettings.MO2.MO2ModFolder = appSettings352.MO2.MO2ModFolder;
            return appSettings;
        }

    }
#pragma warning restore CS0618 // Type or member is obsolete

    public partial class AppSettings
    {
        public AppSettings() { }

        public AppSettings(AppSettings appSettings360)
        {
            AppSettings appSettings = new AppSettings();
            appSettings.XEditLogFileName = appSettings360.XEditLogFileName;
            appSettings.PapyrusFlag = appSettings360.PapyrusFlag;
            appSettings.VerboseConsoleLoging = appSettings360.VerboseConsoleLoging;
            appSettings.VerboseFileLoging = appSettings360.VerboseFileLoging;
            appSettings.AutoReadNewestxEditSession = appSettings360.AutoReadNewestxEditSession;
            appSettings.AutoReadAllxEditSession = appSettings360.AutoReadAllxEditSession;
            appSettings.AutoRunESLify = appSettings360.AutoRunESLify;
            appSettings.AutoRunScriptDecompile = appSettings360.AutoRunScriptDecompile;
            appSettings.EnableCompiler = appSettings360.EnableCompiler;
            appSettings.EnableLargeMerges = appSettings360.EnableLargeMerges;
            appSettings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession = appSettings360.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession;
            appSettings.RunSubPluginCompaction = appSettings360.RunSubPluginCompaction;
            appSettings.ChangedPluginsOutputToDataFolder = appSettings360.ChangedPluginsOutputToDataFolder;
            appSettings.XEditFolderPath = appSettings360.XEditFolderPath;
            appSettings.DataFolderPath = appSettings360.DataFolderPath;
            appSettings.OutputFolder = appSettings360.OutputFolder;
            appSettings.MO2.MO2Support = appSettings360.MO2.MO2Support;
            appSettings.MO2.MO2ModFolder = appSettings360.MO2.MO2ModFolder;
            appSettings.ImportAllCompactedModData = appSettings360.ImportAllCompactedModData;
            //return appSettings;
        }

        public string XEditLogFileName { get; set; } = "SSEEdit_log.txt";

        public string PapyrusFlag { get; set; } = "TESV_Papyrus_Flags.flg";

        public bool VerboseConsoleLoging { get; set; } = false;

        public bool VerboseFileLoging { get; set; } = true;

        public bool ImportAllCompactedModData { get; set; } = false;

        public bool AutoReadNewestxEditSession { get; set; } = false;

        public bool AutoReadAllxEditSession { get; set; } = false;

        public bool AutoRunESLify { get; set; } = false;

        public bool AutoRunScriptDecompile { get; set; } = false;

        public bool EnableCompiler { get; set; } = true;

        public bool EnableLargeMerges { get; set; } = false;

        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession { get; set; } = false;

        public bool RunSubPluginCompaction { get; set; } = false;

        private bool _ChangedPluginsOutputToDataFolder_ = false;
        public bool ChangedPluginsOutputToDataFolder
        {
            get
            {
                return _ChangedPluginsOutputToDataFolder_;
            }
            set
            {
                bool setCPOTDF(bool value)
                {

                    if (!value)
                    {
                        if (GF.StartUpInitialized)
                        {
                            GF.WriteLine(GF.stringLoggingData.PluginEditorDisabled);
                        }
                        return false;
                    }
                    return true;
                }
                _ChangedPluginsOutputToDataFolder_ = setCPOTDF(value);
            }
        }

        public string XEditFolderPath { get; set; } = "xEditFolderPath";

        public string DataFolderPath { get; set; } = "Skyrim Special Edition\\Data";

        public string OutputFolder { get; set; } = "MO2\\Mods\\OutputFolder";

        public MO2SupportSettings MO2 { get; set; } = new MO2SupportSettings();
        
        //Outputs the AppSettings.json
        public void Build()
        {
            File.WriteAllText("AppSettings.json", JsonSerializer.Serialize(new GeneratedAppSettings(this), GF.JsonSerializerOptions));
        }

    }

    public class MO2SupportSettings
    {
        public bool MO2Support { get; set; } = false;

        public string MO2ModFolder { get; set; } = "MO2\\Mods";
    }

    [Obsolete("Only used for Updating")]
    public class AppSettings19
    {
        [JsonInclude]
        public string XEditLogFileName { get; set; } = "SSEEdit_log.txt";
        [JsonInclude]
        public string PapyrusFlag { get; set; } = "TESV_Papyrus_Flags.flg";
        [JsonInclude]
        public bool VerboseConsoleLoging { get; set; } = true;
        [JsonInclude]
        public bool VerboseFileLoging { get; set; } = true;
        [JsonInclude]
        public bool AutoReadNewestxEditSeesion { get; set; } = false;
        [JsonInclude]
        public bool AutoReadAllxEditSeesion { get; set; } = false;
        [JsonInclude]
        public bool AutoRunESLify { get; set; } = false;
        [JsonInclude]
        public bool AutoRunScriptDecompile { get; set; } = false;
        [JsonInclude]
        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion { get; set; } = false;
        [JsonInclude]
        public string XEditFolderPath { get; set; } = "xEditFolderPath";
        [JsonInclude]
        public string DataFolderPath { get; set; } = "Skyrim Special Edition\\Data";
        [JsonInclude]
        public string OutputFolder { get; set; } = "MO2\\Mods\\OuputFolder";
    }

    [Obsolete("Only used for Updating")]
    public class AppSettings3
    {

        public string XEditLogFileName { get; set; } = "SSEEdit_log.txt";
        [JsonInclude]
        public string PapyrusFlag { get; set; } = "TESV_Papyrus_Flags.flg";
        [JsonInclude]
        public bool VerboseConsoleLoging { get; set; } = true;
        [JsonInclude]
        public bool VerboseFileLoging { get; set; } = true;
        [JsonInclude]
        public bool AutoReadNewestxEditSeesion { get; set; } = false;
        [JsonInclude]
        public bool AutoReadAllxEditSeesion { get; set; } = false;
        [JsonInclude]
        public bool AutoRunESLify { get; set; } = false;
        [JsonInclude]
        public bool AutoRunScriptDecompile { get; set; } = false;
        [JsonInclude]
        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion { get; set; } = false;
        [JsonInclude]
        public bool RunSubPluginCompaction { get; set; } = false;
        [JsonInclude]
        public bool ChangedPluginsOutputToDataFolder { get; set; } = false;
        [JsonInclude]
        public bool MO2Support { get; set; } = false;
        [JsonInclude]
        public string MO2ModFolder { get; set; } = "MO2\\Mods";
        [JsonInclude]
        public string XEditFolderPath { get; set; } = "xEditFolderPath";
        [JsonInclude]
        public string DataFolderPath { get; set; } = "Skyrim Special Edition\\Data";
        [JsonInclude]
        public string OutputFolder { get; set; } = "MO2\\Mods\\OuputFolder";

    }

    [Obsolete("Only used for Updating")]
    public class AppSettings320
    {
        public string XEditLogFileName { get; set; } = "SSEEdit_log.txt";

        public string PapyrusFlag { get; set; } = "TESV_Papyrus_Flags.flg";

        public bool VerboseConsoleLoging { get; set; } = true;

        public bool VerboseFileLoging { get; set; } = true;

        public bool AutoReadNewestxEditSeesion { get; set; } = false;

        public bool AutoReadAllxEditSeesion { get; set; } = false;

        public bool AutoRunESLify { get; set; } = false;

        public bool AutoRunScriptDecompile { get; set; } = false;

        public bool EnableCompiler { get; set; } = true;

        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion { get; set; } = false;

        public bool RunSubPluginCompaction { get; set; } = false;

        public bool ChangedPluginsOutputToDataFolder { get; set; } = false;

        public bool MO2Support { get; set; } = false;

        public string MO2ModFolder { get; set; } = "MO2\\Mods";

        public string XEditFolderPath { get; set; } = "xEditFolderPath";

        public string DataFolderPath { get; set; } = "Skyrim Special Edition\\Data";

        public string OutputFolder { get; set; } = "MO2\\Mods\\OuputFolder";
    }

    [Obsolete("Only used for Updating")]
    public class AppSettings350
    {
        public string XEditLogFileName { get; set; } = "SSEEdit_log.txt";

        public string PapyrusFlag { get; set; } = "TESV_Papyrus_Flags.flg";

        public bool VerboseConsoleLoging { get; set; } = true;

        public bool VerboseFileLoging { get; set; } = true;

        public bool AutoReadNewestxEditSession { get; set; } = false;

        public bool AutoReadAllxEditSession { get; set; } = false;

        public bool AutoRunESLify { get; set; } = false;

        public bool AutoRunScriptDecompile { get; set; } = false;

        public bool EnableCompiler { get; set; } = true;

        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession { get; set; } = false;

        public bool RunSubPluginCompaction { get; set; } = false;

        public bool ChangedPluginsOutputToDataFolder = false;

        public string XEditFolderPath { get; set; } = "xEditFolderPath";

        public string DataFolderPath { get; set; } = "Skyrim Special Edition\\Data";

        public string OutputFolder { get; set; } = "MO2\\Mods\\OutputFolder";

        public MO2SupportSettings MO2 { get; set; } = new MO2SupportSettings();

    }

    [Obsolete("Only used for Updating")]
    public class AppSettings352
    {
        public string XEditLogFileName { get; set; } = "SSEEdit_log.txt";

        public string PapyrusFlag { get; set; } = "TESV_Papyrus_Flags.flg";

        public bool VerboseConsoleLoging { get; set; } = false;

        public bool VerboseFileLoging { get; set; } = true;

        public bool AutoReadNewestxEditSession { get; set; } = false;

        public bool AutoReadAllxEditSession { get; set; } = false;

        public bool AutoRunESLify { get; set; } = false;

        public bool AutoRunScriptDecompile { get; set; } = false;

        public bool EnableCompiler { get; set; } = true;

        public bool EnableLargeMerges { get; set; } = false;

        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession { get; set; } = false;

        public bool RunSubPluginCompaction { get; set; } = false;

        public bool ChangedPluginsOutputToDataFolder = false;

        public string XEditFolderPath { get; set; } = "xEditFolderPath";

        public string DataFolderPath { get; set; } = "Skyrim Special Edition\\Data";

        public string OutputFolder { get; set; } = "MO2\\Mods\\OutputFolder";

        public MO2SupportSettings MO2 { get; set; } = new MO2SupportSettings();
    }

    //Class for generate the AppSettings.Json
    public class GeneratedAppSettings
    {
        [JsonInclude]
        public string SettingsVersion { get { return GF.SettingsVersion; } }
        [JsonInclude]
        public AppSettings Settings = new AppSettings();

        public GeneratedAppSettings() { }

        public GeneratedAppSettings(AppSettings settings)
        {
            Settings = settings;
        }
    }
}
