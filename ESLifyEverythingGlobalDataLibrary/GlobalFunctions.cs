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
        xEditLogNotFound,           //Did not find the xEdit log inside the XEditFolderPath
        OutputtedScriptsFound,      //Found compiled scripts inside OutputFolder\Scripts
        InvalidStartUp,              //Startup was not successful can not run
        DataFolderNotFound,
        CompilerNotFound
    }

    //Global File Extentions
    public static partial class GF
    {
        //readonly property to get the extension for all CompactedFormJson
        public static readonly string CompactedFormExtension = "_ESlEverything.json";

        //readonly property to get the extension for CompactedFormJson that needs to be ignored
        public static readonly string CompactedFormIgnoreExtension = "_ESLEverything.ignore";

        //readonly property to get the extension for all Merge Caches
        public static readonly string MergeCacheExtension = "_ESlEverythingMergeCache.json";

        //readonly property to get the extension for Merge Caches that needs to be ignored
        public static readonly string MergeCacheIgnoreExtension = "_ESlEverythingMergeCache.ignore";

        //readonly property to get the extension for Merge Caches that needs to be ignored
        public static readonly string ModSplitDataExtension = "_ESlEverythingSplitData.json";
    }

    public static partial class GF
    {
        //readonly property to identify what settings version ESLify uses to update settings properly
        public static readonly string SettingsVersion = "4.6.0";

        //readonly property to direct to where the Changed Scripts are stored
        public static readonly string ChangedScriptsPath = ".\\ChangedScripts";

        //readonly property to direct to where the Compacted Forms are stored
        public static readonly string CompactedFormsFolder = ".\\CompactedForms";

        //readonly property to direct to where the Extracted BSA Mod Data is stored
        public static readonly string ExtractedBSAModDataPath = ".\\ExtractedBSAModData";

        //readonly property to direct to where I wish for the Source code to be decompiled and read from.
        public static readonly string SourceSubPath = "Source\\Scripts";

        //readonly property for how many new records are inside of a merge for it to be a large mod
        public static readonly int LargeMergeCount = 10000;

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

        //Reads from .\\Properties\\DefaultPlugins.json, it is a list of the defauly plugins that do not apear inside of plugins.txt
        public static string[] DefaultPlugins = new string[0];

        //Reads from .\\Properties\\IgnoredPugins.json and .\\Properties\\CustomIgnoredPugins.json, if it exists
        //It is a list that holds what plugins should not be looked at by ESLify Everything
        //The base game plugins and a few large mod's plugins are included in IgnoredPugins.json
        //CustomIgnoredPugins.json is created by the user and populated with what you want to make ESLify Everything processing them
        public static HashSet<string> IgnoredPlugins = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        //ESLify Everything's log name
        public static string logName = "log.txt";

        public static bool StartupCalled { get; private set; } = false;

        //Obsolete
        ////Path to the face gen fix list for the xEdit script
        //public static string FaceGenFileFixPath = "";

        //End identifier to prompt that ESLify Everything output edited plugins to MO2 mods folder
        public static bool NewMO2FolderPaths = false;

        //Identifier to start object logging
        public static bool StartUpInitialized = false;

        //Checks whether all AppSettings are valid and should work as intended so long as paths are directed to the correct folders
        public static bool StartUp(out HashSet<StartupError> startupError, string ProgramLogName)
        {
            StartupCalled = true;
            startupError = new HashSet<StartupError>();
            logName = ProgramLogName;
            File.Create(logName).Close();

            GetStringResources();

            if (!File.Exists("AppSettings.json"))
            {
                GF.GenerateSettingsFileError();
                startupError.Add(StartupError.InvalidStartUp);
                return false;
            }

            bool startUp = true;

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddJsonFile(".\\Properties\\DefaultBSAs.json")
                .AddJsonFile(".\\Properties\\IgnoredPugins.json")
                .AddJsonFile(".\\Properties\\DefaultPlugins.json")
                .AddEnvironmentVariables().Build();

            try
            {
                string version = config.GetRequiredSection("SettingsVersion").Get<string>();
                if (!version.Equals(GF.SettingsVersion))
                {
                    UAppSettings.UpdateSettingsFile();
                    Console.WriteLine("Settings Error");
                    startupError.Add(StartupError.InvalidStartUp);
                    return false;
                }
            }
            catch (Exception)
            {
                GF.GenerateSettingsFileError();
                Console.WriteLine("Settings Error");
                startupError.Add(StartupError.InvalidStartUp);
                return false;
            }

            Settings = config.GetRequiredSection("Settings").Get<AppSettings>();
            DefaultScriptBSAs = config.GetRequiredSection("DefaultScriptBSAs").Get<string[]>();
            DefaultPlugins = config.GetRequiredSection("DefaultPlugins").Get<string[]>();

            foreach(string plugin in config.GetRequiredSection("IgnoredPugins").Get<HashSet<string>>())
            {
                IgnoredPlugins.Add(plugin);
            }

            if (File.Exists("DevAppSettings.json"))
            {
                Console.WriteLine("DevAppSettings found");
                DevSettings = JsonSerializer.Deserialize<DevAppSettings>(File.ReadAllText("DevAppSettings.json"))!;
            }

            StartUpInitialized = true;

            if (GF.Settings.AutoReadAllxEditSession == false)
            {
                GF.Settings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession = false;
            }

            if (!Directory.Exists(GF.Settings.DataFolderPath))
            {
                GF.WriteLine(GF.stringLoggingData.DataFolderNotFound);
                Console.WriteLine("Data Folder Path Error");
                startupError.Add(StartupError.DataFolderNotFound);
                startupError.Add(StartupError.InvalidStartUp);
                startUp = false;
            }

            if (!Directory.Exists(GF.Settings.XEditFolderPath))
            {
                GF.WriteLine(GF.stringLoggingData.XEditLogNotFoundStartup);
                Console.WriteLine("xEditFolderPath Error");
                startupError.Add(StartupError.InvalidStartUp);
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

            //if (Directory.Exists(GF.Settings.XEditFolderPath))
            //{
            //    FaceGenFileFixPath = Path.Combine(GF.Settings.XEditFolderPath, "FaceGenEslIfyFix.txt");
            //    File.Create(FaceGenFileFixPath).Close();
            //}

            if (!File.Exists(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName)))
            {
                GF.WriteLine(GF.stringLoggingData.XEditLogNotFound);
                startupError.Add(StartupError.xEditLogNotFound);
            }

            if (!File.Exists(".\\Champollion\\Champollion.exe"))
            {
                Console.WriteLine("Champollion Error");
                startupError.Add(StartupError.InvalidStartUp);
                startUp = false;
                GF.WriteLine(GF.stringLoggingData.ChampollionMissing);
            }

            if (!File.Exists(Path.Combine(GF.GetSkyrimRootFolder(), "Papyrus Compiler\\PapyrusCompiler.exe")))
            {
                Console.WriteLine("Compiler Error");
                startupError.Add(StartupError.InvalidStartUp);
                startupError.Add(StartupError.CompilerNotFound);
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
                Console.WriteLine("Output folder Error");
                startupError.Add(StartupError.InvalidStartUp);
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
                    startupError.Add(StartupError.OutputtedScriptsFound);
                    GF.WriteLine(String.Format(GF.stringLoggingData.ClearYourOutputFolderScripts1, GF.stringLoggingData.PotectOrigonalScripts));
                    GF.WriteLine(GF.stringLoggingData.ClearYourOutputFolderScripts2);
                    Process ds = new Process();
                    ds.StartInfo.FileName = "explorer.exe";
                    ds.StartInfo.Arguments = Path.Combine(GF.Settings.OutputFolder, "scripts");
                    ds.Start();
                    ds.WaitForExit();
                    ds.Dispose();
                    GF.WriteLine(GF.stringLoggingData.ClearYourOutputFolderScripts3);
                }
            }
            return startUp;
        }

        public static void ValidStartUp()
        {

            if (File.Exists(".\\Properties\\CustomIgnoredPugins.json"))
            {
                foreach (string plugin in new ConfigurationBuilder()
                    .AddJsonFile(".\\Properties\\CustomIgnoredPugins.json")
                    .AddEnvironmentVariables()
                    .Build()
                    .GetRequiredSection("CustomIgnoredPugins").Get<string[]>())
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

            if (GF.Settings.MO2.MO2Support)
            {
                if (!Directory.Exists(GF.Settings.MO2.MO2ModFolder))
                {
                    GF.WriteLine(GF.stringLoggingData.MO2ModsFolderDoesNotExist);
                    GF.Settings.MO2.MO2Support = false;
                }
                else
                {
                    if (GF.Settings.MO2.MO2ModFolder.ElementAt(GF.Settings.MO2.MO2ModFolder.Length - 1).Equals('\\'))
                    {
                        GF.Settings.MO2.MO2ModFolder = GF.Settings.MO2.MO2ModFolder.Remove(GF.Settings.MO2.MO2ModFolder.Length - 1);
                    }
                }
            }

            if (GF.Settings.RunAllVoiceAndFaceGen)
            {
                GF.WriteLine(GF.stringLoggingData.ImportAllCompactedModDataTrueWarning);
                GF.Settings.AutoRunESLify = false;
            }

            if (GF.Settings.DataFolderPath.ElementAt(GF.Settings.DataFolderPath.Length - 1).Equals('\\'))
            {
                GF.Settings.DataFolderPath = GF.Settings.DataFolderPath.Remove(GF.Settings.DataFolderPath.Length - 1);
            }

            if (GF.Settings.OutputFolder.ElementAt(GF.Settings.OutputFolder.Length - 1).Equals('\\'))
            {
                GF.Settings.OutputFolder = GF.Settings.OutputFolder.Remove(GF.Settings.OutputFolder.Length - 1);
            }

            if (GF.Settings.XEditFolderPath.ElementAt(GF.Settings.XEditFolderPath.Length - 1).Equals('\\'))
            {
                GF.Settings.XEditFolderPath = GF.Settings.XEditFolderPath.Remove(GF.Settings.XEditFolderPath.Length - 1);
            }

            Directory.CreateDirectory(GF.ExtractedBSAModDataPath);
            Directory.CreateDirectory(GF.ChangedScriptsPath);
            //GF.ClearChangedScripts();

            Directory.CreateDirectory(CompactedFormsFolder);

        }

        //Outputs logging for StartupErrors
        public static void EndStartUpErrorLoggig(HashSet<StartupError> startupError)
        {
            if (startupError.Contains(StartupError.xEditLogNotFound))
            {
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(GF.stringLoggingData.xEditlogNotFound);
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

        //Gets stringLoggingData and stringsResources so there does not need to be multiple exeption calls for this process
        public static void GetStringResources()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(".\\Properties\\StringResources.json").AddEnvironmentVariables().Build();
            stringLoggingData = config.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();
            stringsResources = config.GetRequiredSection("StringResources").Get<StringResources>();
        }

        //Filters out unactive plugins inside of the plugins.txt and returns the list of active plugins
        public static string[] FilterForActiveLoadOrder(string pluginsPath)
        {
            string[] plugins = File.ReadAllLines(pluginsPath);

            List<string> filteredLoadOrder = new List<string>();
            
            foreach(string plugin in DefaultPlugins)
            {
                if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, plugin)))
                {
                    filteredLoadOrder.Add(plugin);
                }
            }

            foreach (string loadOrderItem in plugins)
            {
                if (loadOrderItem.Contains('*'))
                {
                    filteredLoadOrder.Add(loadOrderItem.Replace("*", ""));
                }
            }

            return filteredLoadOrder.ToArray();
        }

        //Outputs the List of StartupErrors
        public static void StartUpErrorOutput(HashSet<StartupError> startupError)
        {
            foreach (var startuperror in startupError)
            {
                GF.WriteLine(startuperror + " : ");

            }
            Console.WriteLine();
        }

        public static void EnterToContinue()
        {
            Console.WriteLine();
            GF.WriteLine(GF.stringLoggingData.EnterToContinue);
            Console.ReadLine();
        }
    }
}
//Obsolete
////Auto Runs the xEdit script to fix FaceGen nif files
//public static void RunFaceGenFix()
//{
//    string loadorder = Path.GetFullPath(".\\Properties\\JustSkyrimLO.txt");
//    string gameType = "-SSE";
//    if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "Edit Scripts\\_ESLifyEverythingFaceGenFix.pas")))
//    {
//        bool run = true;
//        Process RunXEditFaceGenFix = new Process();
//        if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit64.exe")))
//        {
//            RunXEditFaceGenFix.StartInfo.FileName = Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit64.exe");
//        }
//        else if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit.exe")))
//        {
//            RunXEditFaceGenFix.StartInfo.FileName = Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit.exe");
//        }
//        else if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "FO4Edit64.exe")))
//        {
//            loadorder = Path.GetFullPath(".\\Properties\\JustFalloutLO.txt");
//            gameType = "-fo4";
//            RunXEditFaceGenFix.StartInfo.FileName = Path.Combine(GF.Settings.XEditFolderPath, "FO4Edit64.exe");
//        }
//        else if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "FO4Edit.exe")))
//        {
//            loadorder = Path.GetFullPath(".\\Properties\\JustFalloutLO.txt");
//            gameType = "-fo4";
//            RunXEditFaceGenFix.StartInfo.FileName = Path.Combine(GF.Settings.XEditFolderPath, "FO4Edit.exe");
//        }
//        else
//        {
//            GF.WriteLine(GF.stringLoggingData.NoxEditEXE);
//            run = false;
//        }

//        if (run)
//        {
//            if (File.Exists(Path.Combine(Environment.GetEnvironmentVariable("LocalAppData")!, "Skyrim Special Edition", "Skyrim.ini")))
//            {
//                RunXEditFaceGenFix.StartInfo.Arguments = $"{gameType} " +
//                $"-D:\"{GF.Settings.DataFolderPath}\" " +
//                $"-I:\"{Path.Combine(Environment.GetEnvironmentVariable("LocalAppData")!, "Skyrim Special Edition", "Skyrim.ini")}\" " +
//                $" {loadorder}" +
//                "-script:\"_ESLifyEverythingFaceGenFix.pas\" -autoload";
//                GF.WriteLine(GF.stringLoggingData.RunningxEditEXE);
//                RunXEditFaceGenFix.Start();
//                RunXEditFaceGenFix.WaitForExit();
//            }
//            else
//            {
//                RunXEditFaceGenFix.StartInfo.Arguments = "-TES5 -script:\"_ESLifyEverythingFaceGenFix.pas\" -autoload";
//                GF.WriteLine(GF.stringLoggingData.RunningxEditEXE);
//                RunXEditFaceGenFix.Start();
//                RunXEditFaceGenFix.WaitForExit();
//            }
//        }
//        RunXEditFaceGenFix.Dispose();

//    }
//    else
//    {
//        GF.WriteLine(GF.stringLoggingData.FixFaceGenScriptNotFound);
//    }

//}