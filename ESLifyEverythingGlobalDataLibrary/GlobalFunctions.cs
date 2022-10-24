using ESLifyEverythingGlobalDataLibrary.FormData;
using ESLifyEverythingGlobalDataLibrary.Properties;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;

namespace ESLifyEverythingGlobalDataLibrary
{
    //For errors in startup causing things not to run
    public enum StartupError
    {
        OK = 0,                 //Everything should run fine
        xEditLogNotFound = 1    //Did not find the xEdit log inside the XEditFolderPath
    }

    public static partial class GF
    {
        //readonly property to identify what settings version ESLify uses to update settings properly
        public static readonly string SettingsVersion = "3.2.0";

        //readonly property to direct to where the Changed Scripts are stored
        public static readonly string ChangedScriptsPath = ".\\ChangedScripts";

        //readonly property to direct to where the Compacted Forms are stored
        public static readonly string CompactedFormsFolder = ".\\CompactedForms";

        //readonly property to direct to where the Extracted BSA Mod Data is stored
        public static readonly string ExtractedBSAModDataPath = ".\\ExtractedBSAModData";

        //readonly property to direct to where I wish for the Source code to be decompiled and read from.
        public static readonly string SourceSubPath = "Source\\Scripts";

        //readonly property to get the extension for all CompactedFormJson
        public static readonly string CompactedFormExtension = "_ESlEverything.json";

        //readonly property to get the extension for CompactedFormJson that needs to be ignored
        public static readonly string CompactedFormIgnoreExtension = "_ESLifyEverything.ignore";

        //readonly property to get the extension for all Merge Caches
        public static readonly string MergeCacheExtension = "_ESlEverythingMergeCache.json";

        //readonly property to get the extension for Merge Caches that needs to be ignored
        public static readonly string MergeCacheIgnoreExtension = "_ESlEverythingMergeCache.ignore";

        //The settings object that the program functions off of
        public static AppSettings Settings = new AppSettings();

        //Setting Object that enable developer console logging
        public static DevAppSettings DevSettings = new DevAppSettings();

        //string resources for support for other languages other then English
        public static StringResources stringsResources = new StringResources();

        //string logging data for log lines in other languages other then English
        public static StringLoggingData stringLoggingData = new StringLoggingData();

        //Global object for outputing json in readable format
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };

        //Reads from .\\Properties\\DefaultBSAs.json, it is a list of the default BSAs from the base game
        public static string[] DefaultScriptBSAs = new string[0];

        //Reads from .\\Properties\\IgnoredPugins.json and .\\Properties\\CustomIgnoredPugins.json, if it exists
        //It is a list that holds what plugins should not be looked at by ESLify Everything
        //The base game plugins and a few large mod's plugins are included in IgnoredPugins.json
        //CustomIgnoredPugins.json is created by the user and populated with what you want to make ESLify Everything processing them
        public static HashSet<string> IgnoredPlugins = new HashSet<string>();

        //ESLify Everything's log name
        public static string logName = "log.txt";

        //Path to the face gen fix list for the xEdit script
        public static string FaceGenFileFixPath = "";

        //Identifier to start object logging
        public static bool StartUpInitialized = false;

        //Checks whether all AppSettings are valid and should work as intended so long as paths are directed to the correct folders
        public static bool Startup(out StartupError startupError, string ProgramLogName)
        {
            logName = ProgramLogName;
            File.Create(logName).Close();
            startupError = StartupError.OK;

            if (!File.Exists("AppSettings.json"))
            {

                IConfiguration stringResorsConfig = new ConfigurationBuilder().AddJsonFile(".\\Properties\\StringResources.json").AddEnvironmentVariables().Build();
                stringLoggingData = stringResorsConfig.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();
                GF.GenerateSettingsFileError();
                return false;
            }

            bool startUp = true;

            IConfigurationBuilder? configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddJsonFile(".\\Properties\\StringResources.json")
                .AddJsonFile(".\\Properties\\DefaultBSAs.json")
                .AddJsonFile(".\\Properties\\IgnoredPugins.json");

            bool customIgnoredPlugins = false;

            if (File.Exists(".\\Properties\\CustomIgnoredPugins.json"))
            {
                configurationBuilder.AddJsonFile(".\\Properties\\CustomIgnoredPugins.json");
                customIgnoredPlugins = true;
            }

            IConfiguration config = configurationBuilder.AddEnvironmentVariables().Build();
            stringLoggingData = config.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();

            try
            {
                string version = config.GetRequiredSection("SettingsVersion").Get<string>();
                if (!version.Equals(GF.SettingsVersion))
                {
                    GF.UpdateSettingsFile();
                    return false;
                }
            }
            catch (Exception)
            {
                GF.GenerateSettingsFileError();
                return false;
            }

            Settings = config.GetRequiredSection("Settings").Get<AppSettings>();
            stringsResources = config.GetRequiredSection("StringResources").Get<StringResources>();
            DefaultScriptBSAs = config.GetRequiredSection("DefaultScriptBSAs").Get<string[]>();
            IgnoredPlugins = config.GetRequiredSection("IgnoredPugins").Get<HashSet<string>>();

            if (File.Exists("DevAppSettings.json"))
            {
                Console.WriteLine("DevAppSettings found");
                DevSettings = JsonSerializer.Deserialize<DevAppSettings>(File.ReadAllText("DevAppSettings.json"))!;
            }

            StartUpInitialized = true;

            if (GF.Settings.AutoReadAllxEditSeesion == false)
            {
                GF.Settings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion = false;
            }

            if (!Directory.Exists(GF.Settings.DataFolderPath))
            {
                GF.WriteLine(GF.stringLoggingData.DataFolderNotFound);
                startUp = false;
            }

            if (!Directory.Exists(GF.Settings.XEditFolderPath))
            {
                GF.WriteLine(GF.stringLoggingData.XEditLogNotFoundStartup);
                startUp = false;
                if (File.Exists(GF.Settings.XEditFolderPath))
                {
                    GF.WriteLine(GF.stringLoggingData.XEditFolderSetToFile);
                }
            }
            else if (!File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit.exe")))
            {
                GF.WriteLine(GF.stringLoggingData.IntendedForSSE);
            }

            if (Directory.Exists(GF.Settings.XEditFolderPath))
            {
                FaceGenFileFixPath = Path.Combine(GF.Settings.XEditFolderPath, "FaceGenEslIfyFix.txt");
                File.Create(FaceGenFileFixPath).Close();
            }

            if (!File.Exists(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName)))
            {
                GF.WriteLine(GF.stringLoggingData.XEditLogNotFound);
                startupError = StartupError.xEditLogNotFound;
            }

            if (!File.Exists(".\\Champollion\\Champollion.exe"))
            {
                startUp = false;
                GF.WriteLine(GF.stringLoggingData.ChampollionMissing);
            }

            if (!File.Exists(Path.Combine(GF.GetSkyrimRootFolder(), "Papyrus Compiler\\PapyrusCompiler.exe")))
            {
                startUp = false;
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing);
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing2);
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing3);
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing4);
            }
            else
            {
                if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, $"{GF.SourceSubPath}\\{GF.Settings.PapyrusFlag}")))
                {
                    Directory.CreateDirectory(Path.Combine(GF.ExtractedBSAModDataPath, $"{GF.SourceSubPath}"));
                    File.Copy(Path.Combine(GF.Settings.DataFolderPath, $"{GF.SourceSubPath}\\{GF.Settings.PapyrusFlag}"),
                        Path.Combine(GF.ExtractedBSAModDataPath, $"{GF.SourceSubPath}\\{GF.Settings.PapyrusFlag}"), true);
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.PapyrusFlagFileMissing);
                    GF.WriteLine(String.Format(GF.stringLoggingData.PapyrusFlagFileMissing2, GF.Settings.PapyrusFlag));
                    GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing4);
                }
            }

            if (!Directory.Exists(GF.Settings.OutputFolder))
            {
                startUp = false;
                GF.WriteLine(GF.stringLoggingData.OutputFolderNotFound);
                GF.WriteLine(String.Format(GF.stringLoggingData.OutputFolderIsRequired, GF.stringLoggingData.PotectOrigonalScripts));
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.OutputFolderWarning, true, false);
            }

            if (Directory.Exists(Path.Combine(GF.Settings.OutputFolder, "scripts")))
            {
                IEnumerable<string> scripts = Directory.EnumerateFiles(
                    Path.Combine(GF.Settings.OutputFolder, "scripts"),
                    "*.pex",
                    SearchOption.TopDirectoryOnly);
                if (scripts.Any())
                {
                    startUp = false;
                    GF.WriteLine(String.Format(GF.stringLoggingData.ClearYourOutputFolderScripts, GF.stringLoggingData.PotectOrigonalScripts));
                }
            }

            if (!startUp)
            {
                return startUp;
            }

            if (customIgnoredPlugins)
            {
                HashSet<string> customIgnoredPluginsSet = config.GetRequiredSection("CustomIgnoredPugins").Get<HashSet<string>>();
                foreach (string plugin in customIgnoredPluginsSet)
                {
                    IgnoredPlugins.Add(plugin);
                }
            }

            if (GF.Settings.RunSubPluginCompaction)
            {
                if (!GF.Settings.PapyrusFlag.Equals("TESV_Papyrus_Flags.flg"))
                {
                    if (!File.Exists(Path.Combine(GF.Settings.DataFolderPath, "Skyrim.esm")))
                    {
                        GF.WriteLine(GF.stringLoggingData.ESLifyEverythingIsNotSetUpForSkyrim);
                        GF.Settings.RunSubPluginCompaction = false;
                    }
                }
            }

            if (GF.Settings.MO2Support)
            {
                if (!Directory.Exists(GF.Settings.MO2ModFolder))
                {
                    GF.WriteLine(GF.stringLoggingData.MO2ModsFolderDoesNotExist);
                    GF.Settings.MO2Support = false;
                }
            }

            Directory.CreateDirectory(GF.ExtractedBSAModDataPath);
            Directory.CreateDirectory(GF.ChangedScriptsPath);
            GF.ClearChangedScripts();

            Directory.CreateDirectory(CompactedFormsFolder);

            if (Directory.Exists(Path.Combine(GF.ExtractedBSAModDataPath, GF.SourceSubPath)))
            {
                IEnumerable<string> scripts = Directory.EnumerateFiles(
                    Path.Combine(GF.ExtractedBSAModDataPath, GF.SourceSubPath),
                    "*.psc",
                    SearchOption.TopDirectoryOnly);
                if (!scripts.Any())
                {
                    GF.Settings.AutoRunScriptDecompile = true;
                }
            }




            return startUp;
        }

        //Clears the ChangedScripts folder from previous ESLify Everything session
        private static void ClearChangedScripts()
        {
            IEnumerable<string> changedSouce = Directory.EnumerateFiles(
                    GF.ChangedScriptsPath,
                    "*.psc",
                    SearchOption.TopDirectoryOnly);
            if (changedSouce.Any())
            {
                foreach (string script in changedSouce)
                {
                    File.Delete(script);
                }
            }
        }

        //Writes a console line or file line when logging is set to true
        public static void WriteLine(string logLine, bool consoleLog = true, bool fileLogging = true, bool devOverride = false)
        {
            if (GF.DevSettings.DevLogging && !devOverride)
            {
                Console.WriteLine(logLine);
                using (StreamWriter stream = File.AppendText(logName))
                {
                    stream.WriteLine(logLine);
                }
                return;
            }

            if (consoleLog)
            {
                Console.WriteLine(logLine);
            }
            if (fileLogging)
            {
                using (StreamWriter stream = File.AppendText(logName))
                {
                    stream.WriteLine(logLine);
                }
            }
        }

        //returns true when a valid input is inputed
        //-1 = return exit code in selectedMenuItem
        public static bool WhileMenuSelect(int menuMaxNum, out int selectedMenuItem, int MenuMinNum = 0)
        {
            string input = Console.ReadLine() ?? "";
            if (input.Equals("XXX", StringComparison.OrdinalIgnoreCase))
            {
                selectedMenuItem = -1;
                return true;
            }

            if (Int32.TryParse(input, out selectedMenuItem))
            {
                if (selectedMenuItem >= MenuMinNum && selectedMenuItem <= menuMaxNum)
                {
                    return true;
                }
            }

            return false;
        }

        //Generates and logs the AppSettings.json file
        public static void GenerateSettingsFileError()
        {
            GF.WriteLine(GF.stringLoggingData.SettingsFileNotFound);
            GF.WriteLine(GF.stringLoggingData.GenSettingsFile);
            GF.WriteLine(GF.stringLoggingData.EditYourSettings);
            new AppSettings().Build();
        }

        //Updates and logs the AppSettings.json file
        public static void UpdateSettingsFile()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddEnvironmentVariables().Build();

#pragma warning disable CS0618 // Type or member is obsolete
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
                default:
                    GenerateSettingsFileError();
                    break;
            }
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public static string FixOuputPath(string origonalPath)
        {
            string newPath = origonalPath.Replace(GF.Settings.DataFolderPath, "");
            if (newPath[0] == '\\')
            {
                newPath = newPath.Substring(1);
            }
            return Path.Combine(GF.Settings.OutputFolder, newPath);
        }

        public static string FixOuputPath(string origonalPath, string origonalDataStartPath, string newStartPath)
        {
            string newPath = origonalPath.Replace(origonalDataStartPath, "");
            if (newPath[0] == '\\')
            {
                newPath = newPath.Substring(1);
            }
            return Path.Combine(newStartPath, newPath);
        }

        //Gets the root folder containing the Data folder
        public static string GetSkyrimRootFolder()
        {
            return Path.GetFullPath(GF.Settings.DataFolderPath).Replace("\\Data", "");
        }

        //Auto Runs the xEdit script to fix FaceGen nif files
        public static void RunFaceGenFix()
        {
            string loadorder = Path.GetFullPath(".\\Properties\\JustSkyrimLO.txt");
            string gameType = "-SSE";
            if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "Edit Scripts\\_ESLifyEverythingFaceGenFix.pas")))
            {
                bool run = true;
                Process RunXEditFaceGenFix = new Process();
                if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit64.exe")))
                {
                    RunXEditFaceGenFix.StartInfo.FileName = Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit64.exe");
                }
                else if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit.exe")))
                {
                    RunXEditFaceGenFix.StartInfo.FileName = Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit.exe");
                }
                else if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "FO4Edit64.exe")))
                {
                    loadorder = Path.GetFullPath(".\\Properties\\JustFalloutLO.txt");
                    gameType = "-fo4";
                    RunXEditFaceGenFix.StartInfo.FileName = Path.Combine(GF.Settings.XEditFolderPath, "FO4Edit64.exe");
                }
                else if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "FO4Edit.exe")))
                {
                    loadorder = Path.GetFullPath(".\\Properties\\JustFalloutLO.txt");
                    gameType = "-fo4";
                    RunXEditFaceGenFix.StartInfo.FileName = Path.Combine(GF.Settings.XEditFolderPath, "FO4Edit.exe");
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.NoxEditEXE);
                    run = false;
                }

                if (run)
                {
                    if (File.Exists(Path.Combine(Environment.GetEnvironmentVariable("LocalAppData")!, "Skyrim Special Edition", "Skyrim.ini")))
                    {
                        RunXEditFaceGenFix.StartInfo.Arguments = $"{gameType} " +
                        $"-D:\"{GF.Settings.DataFolderPath}\" " +
                        $"-I:\"{Path.Combine(Environment.GetEnvironmentVariable("LocalAppData")!, "Skyrim Special Edition", "Skyrim.ini")}\" " +
                        $" {loadorder}" +
                        "-script:\"_ESLifyEverythingFaceGenFix.pas\" -autoload";
                        GF.WriteLine(GF.stringLoggingData.RunningxEditEXE);
                        RunXEditFaceGenFix.Start();
                        RunXEditFaceGenFix.WaitForExit();
                    }
                    else
                    {
                        RunXEditFaceGenFix.StartInfo.Arguments = "-TES5 -script:\"_ESLifyEverythingFaceGenFix.pas\" -autoload";
                        GF.WriteLine(GF.stringLoggingData.RunningxEditEXE);
                        RunXEditFaceGenFix.Start();
                        RunXEditFaceGenFix.WaitForExit();
                    }
                }
                RunXEditFaceGenFix.Dispose();

            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.FixFaceGenScriptNotFound);
            }

        }

        //Moves Compacted Mod Data files from the Data folder to the new folder in ESLify Everything
        public static void MoveCompactedModDataJsons()
        {
            string oldCompactedFormsFolder = Path.Combine(GF.Settings.OutputFolder, "CompactedForms");
            if (Directory.Exists(oldCompactedFormsFolder))
            {
                IEnumerable<string> compactedFormsModFiles = Directory.EnumerateFiles(oldCompactedFormsFolder, "*" + GF.CompactedFormExtension, SearchOption.TopDirectoryOnly);

                foreach (string files in compactedFormsModFiles)
                {
                    File.Move(files, Path.Combine(CompactedFormsFolder, Path.GetFileName(files)), true);
                }

            }

        }

    }
}