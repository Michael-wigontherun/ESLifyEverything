using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ESLifyEverything.Properties
{
    public static class UAppSettings
    {
        public static AppSettings AppSettings(AppSettings19 appSettings19)
        {
            AppSettings appSettings = new AppSettings();
            appSettings.XEditLogFileName = appSettings19.XEditLogFileName;
            appSettings.PapyrusFlag = appSettings19.PapyrusFlag;
            appSettings.VerboseConsoleLoging = appSettings19.VerboseConsoleLoging;
            appSettings.VerboseFileLoging = appSettings19.VerboseFileLoging;
            appSettings.AutoReadNewestxEditSeesion = appSettings19.AutoReadNewestxEditSeesion;
            appSettings.AutoReadAllxEditSeesion = appSettings19.AutoReadAllxEditSeesion;
            appSettings.AutoRunESLify = appSettings19.AutoRunESLify;
            appSettings.AutoRunScriptDecompile = appSettings19.AutoRunScriptDecompile;
            appSettings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion = appSettings19.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion;
            appSettings.XEditFolderPath = appSettings19.XEditFolderPath;
            appSettings.DataFolderPath = appSettings19.DataFolderPath;
            appSettings.OutputFolder = appSettings19.OutputFolder;
            return appSettings;
        }
    }

    public partial class AppSettings
    {
        public AppSettings(){}

        public string XEditLogFileName { get; set; } = "SSEEdit_log.txt";

        public string PapyrusFlag { get; set; } = "TESV_Papyrus_Flags.flg";

        public bool VerboseConsoleLoging { get; set; } = true;

        public bool VerboseFileLoging { get; set; } = true;

        public bool AutoReadNewestxEditSeesion { get; set; } = false;

        public bool AutoReadAllxEditSeesion { get; set; } = false;

        public bool AutoRunESLify { get; set; } = false;

        public bool AutoRunScriptDecompile { get; set; } = false;

        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion { get; set; } = false;

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
                        GF.WriteLine(GF.stringLoggingData.PluginEditorDisabled);
                        return false;
                    }
                    return true;
                }
                _ChangedPluginsOutputToDataFolder_ = setCPOTDF(value);
            }
        }
        
        public bool MO2Support { get; set; } = false;

        public string MO2ModFolder { get; set; } = "MO2\\Mods";

        public string XEditFolderPath { get; set; } = "xEditFolderPath";

        public string DataFolderPath { get; set; } = "Skyrim Special Edition\\Data";
        
        public string OutputFolder { get; set; } = "MO2\\Mods\\OuputFolder";

        public void Build()
        {
            File.WriteAllText("AppSettings.json", JsonSerializer.Serialize(new GeneratedAppSettings(this), GF.JsonSerializerOptions));
        } 

    }

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

    public class GeneratedAppSettings
    {
        [JsonInclude]
        public string SettingsVersion { get { return GF.SettingsVersion; } }
        [JsonInclude]
        public AppSettings Settings = new AppSettings();

        public GeneratedAppSettings(){}

        public GeneratedAppSettings(AppSettings settings)
        {
            Settings = settings;
        }
    }
}
