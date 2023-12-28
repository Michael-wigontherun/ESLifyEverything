using ESLifyEverything.Exceptions;
using ESLifyEverything.FormData;
using ESLifyEverything.PluginHandles;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;
using System.Text.Json;

namespace ESLifyEverything
{
    //The Base ESLify Everything Global Variables
    public static partial class ESLify
    {
        //List of dictionary of all enabled and valid CompactedModData.
        //The entire program functions using this.
        public static Dictionary<string, CompactedModData> CompactedModDataD = new(StringComparer.OrdinalIgnoreCase);

        //When populated it holds all plugin names parsed from your plugins.txt file from your my games folder
        public static string[] ActiveLoadOrder = Array.Empty<string>();

        //When populated it holds the plugins with attached BSAs without the extention
        public static List<string> LoadOrderNoExtensions = new();

        //Populated by _BasicSingleFile.json ModConfigurations
        public static HashSet<BasicSingleFile> BasicSingleModConfigurations = new();

        //Populated by _BasicDirectFolder.json ModConfigurations
        public static HashSet<BasicDirectFolder> BasicDirectFolderModConfigurations = new();

        //Populated by _BasicDataSubfolder.json ModConfigurations
        public static HashSet<BasicDataSubfolder> BasicDataSubfolderModConfigurations = new();

        //Populated by _ComplexTOML.json ModConfigurations
        public static HashSet<ComplexTOML> ComplexTOMLModConfigurations = new();

        //Populated by _DelimitedFormKeys.json ModConfigurations
        public static HashSet<DelimitedFormKeys> DelimitedFormKeysModConfigurations = new();

        //Populated by script names that failed to compile during Script ESLify
        public static HashSet<string> FailedToCompile = new();

        //Imports any CompactedModData or MergeCache's with names matching
        public static HashSet<string> AlwaysCheckList = new(StringComparer.OrdinalIgnoreCase);

        //Ignores any CompactedModData or MergeCache's with names matching
        public static HashSet<string> AlwaysIgnoreList = new();

        //Stores what merges need to be rebuilt after a plugin that was merged into it was edited
        public static HashSet<string> EditedMergedPluginNeedsRebuild = new();

        //This is the switch for the -OnlyRunxEditReader argument
        public static bool OnlyRunxEditReader = false;

        //This is the switch for the -NP argument
        public static bool NP = true;
    }

    //Public Combo and ways to access internal methods 
    public static partial class ESLify
    {
        //Single Call for Building CompactedModData and MergeCaches
        public static void BuildCaches(string logName, HashSet<StartupError> startupError)
        {
            if (!GF.StartupCalled)
            {
                StartUp(out startupError, logName);
                if (startupError.Contains(StartupError.InvalidStartUp))
                {
                    GF.StartUpErrorOutput(startupError);
                    return;
                }
            }

            if (!startupError.Contains(StartupError.xEditLogNotFound))
            {
                XEditSession();
                DevLog.Pause("After Log Reader Pause");
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.SkipingSessionLogNotFound);
            }

            if (!startupError.Contains(StartupError.DataFolderNotFound))
            {
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(GF.stringLoggingData.StartingMergeCache);
                BuildMergedData();
                DevLog.Pause("After zMerge Reader Pause");
            }
        }

        //Single Call for Importing all nessessary data from previously cached data
        public static void ImportData(string logName)
        {
            if (!GF.StartupCalled)
            {
                StartUp(out HashSet<StartupError>? startupError, logName);
                if (startupError.Contains(StartupError.InvalidStartUp))
                {
                    GF.StartUpErrorOutput(startupError);
                    return;
                }
            }

            if (File.Exists(".\\Properties\\CustomPluginOutputLocations.json"))
            {
                HandleMod.CustomPluginOutputLocations = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(".\\Properties\\CustomPluginOutputLocations.json"))!;
            }

            BSAData.GetBSAData();

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine(GF.stringLoggingData.StartBSAExtract);
            Task<int> bsamod = LoadOrderBSAData();
            bsamod.Wait();
            int bsamodResult = bsamod.Result;
            bsamod.Dispose();
            switch (bsamodResult)
            {
                case 0:
                    break;
                case 1:
                    throw new MissingFileException();
                default:
                    throw new Exception("How did you reach a default result in LoadOrderBSAData()? Please report this.");
            }

            DevLog.Pause("After BSA Proccesing Pause");

            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(GF.stringLoggingData.ImportingAllModData);
            ImportModData(Path.Combine(GF.Settings.DataFolderPath, "CompactedForms"));
            ImportModData(GF.CompactedFormsFolder);
            ImportMergeData();

            DevLog.Pause("After CompactedForms Import Pause");
        }

        //Single Call for Running every ESLify single Configuration, ModConfigurations and Internally coded data
        public static void AutoRunESLifyDataFiles(string logName, bool PausesPossible = false)
        {
            if (!GF.StartupCalled)
            {
                StartUp(out HashSet<StartupError>? startupError, logName);
                if (startupError.Contains(StartupError.InvalidStartUp))
                {
                    GF.StartUpErrorOutput(startupError);
                    return;
                }
            }

            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(GF.stringLoggingData.StartingVoiceESLify);
            VoiceESLifyEverything();

            if (PausesPossible) DevLog.Pause("After Voice ESLify AutoRun Pause");

            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(GF.stringLoggingData.StartingFaceGenESLify);
            FaceGenESLifyEverything();

            if (PausesPossible) DevLog.Pause("After FaceGen ESLify AutoRun Pause");

            //MergeDictionaries();

            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(GF.stringLoggingData.StartingDataFileESLify);
            GetESLifyModConfigurationFiles();
            ESLifyAllDataFiles();

            if (PausesPossible) DevLog.Pause("After Data File ESLify AutoRun Pause");
        }

        //Single Call for Running every ESLify Configuration based off AppSettings
        //Will run menus if AutoRunESLify is false
        //Or AutoRunESLifyDataFiles() if true
        public static void ESLifyDataFiles(string logName, bool PausesPossible = false)
        {
            if (!GF.StartupCalled)
            {
                StartUp(out HashSet<StartupError>? startupError, logName);
                if (startupError.Contains(StartupError.InvalidStartUp))
                {
                    GF.StartUpErrorOutput(startupError);
                    return;
                }
            }

            if (GF.Settings.AutoRunESLify) ESLify.AutoRunESLifyDataFiles(logName, PausesPossible);
            else//Opens Menus
            {
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(GF.stringLoggingData.StartingVoiceESLify);
                ESLify.VoiceESlIfyMenu();

                if (PausesPossible) DevLog.Pause("After Voice ESLify Menu Pause");

                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(GF.stringLoggingData.StartingFaceGenESLify);
                ESLify.FaceGenESlIfyMenu();

                if (PausesPossible) DevLog.Pause("After FaceGen ESLify Menu Pause");

                //MergeDictionaries();

                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(GF.stringLoggingData.StartingDataFileESLify);
                ESLify.ESLifyDataFilesMainMenu();

                if (PausesPossible) DevLog.Pause("After Data File ESLify Menu Pause");
            }
        }

        //Runs Script ESLify
        public static void StartScriptESLify(string logName, HashSet<StartupError> startupError)
        {
            if (!GF.StartupCalled)
            {
                StartUp(out startupError, logName);
                if (startupError.Contains(StartupError.InvalidStartUp))
                {
                    GF.StartUpErrorOutput(startupError);
                    return;
                }
            }

            if (!startupError.Contains(StartupError.OutputtedScriptsFound))
            {
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(GF.stringLoggingData.StartingScriptESLify);
                Task Scripts = ESLify.ExtractScripts();
                Scripts.Wait();
                Scripts.Dispose();

                DevLog.Pause("After Script ESLify Pause");
            }
        }

        //Runs Mergify Bash Tags based of settings
        //If AutoRunMergifyBashedTags is true it skips the menu
        //if false then it Opens the menu to run it or not
        public static void RunMergifyBashTagsSettings()
        {
            if (ESLify.MergesFound)
            {
                if (GF.Settings.AutoRunMergifyBashedTags)
                {
                    ESLify.RunMergifyBashTags();
                }
                else
                {
                    ESLify.MergifyBashTagsMenu();
                }
            }
        }
    }

    //Data used to output extra warnings at the end of ESLify Everything
    public static partial class ESLify
    {
        //End identifier to prompt that ESLify Everything output BSAs were Extracted
        public static bool BSANotExtracted { get; internal set; } = false;

        //identifier for ESLify Everything detected new mods and it needs to run Script ESLify
        public static bool NewOrUpdatedMods { get; internal set; } = false;

        //Imports all CompactedModData and MergeCache's
        public static bool CheckEverything { get; internal set; } = false;

        //Ignore found Outputted scripts and run all processes except Script ESLify CMD Argument
        public static bool _IgnoreScripts { get; internal set; } = false;

        //This will toggle whether to ask or attempt to run mergify bash tags
        public static bool MergesFound { get; internal set; } = false;

        //Checks and outputs warnings for users. Should be run at the end of everything
        public static void EverythingEndChecks()
        {
            if (ESLify.BSANotExtracted)
            {
                Console.WriteLine();
                GF.WriteLine(GF.stringLoggingData.LoadOrderNotDetectedError);
                GF.WriteLine(GF.stringLoggingData.RunOrReport);
            }

            if (GF.NewMO2FolderPaths)
            {
                GF.WriteLine(GF.stringLoggingData.NewMO2FoldersWarning);
                GF.WriteLine(GF.stringLoggingData.HowToDisableMO2Folders);
            }

            if (ESLify.FailedToCompile.Any())
            {
                try
                {
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.ImportantBelow);
                    GF.WriteLine(GF.stringLoggingData.ImportantBelow1);
                    Console.WriteLine();
                    Directory.CreateDirectory(Path.Combine(GF.Settings.OutputFolder, GF.SourceSubPath));
                    foreach (string error in ESLify.FailedToCompile)
                    {
                        File.Copy(Path.Combine(GF.ChangedScriptsPath, error + ".psc"), Path.Combine(GF.Settings.OutputFolder, GF.SourceSubPath, error + ".psc"), true);
                        GF.WriteLine(Path.ChangeExtension(error, null) + GF.stringLoggingData.ScriptFailedCompilation);
                        GF.WriteLine(String.Format(GF.stringLoggingData.ScriptFailedCompilation2, error));
                        Console.WriteLine();

                    }
                    GF.WriteLine(GF.stringLoggingData.ScriptFailedCompilation3);
                    Console.WriteLine();
                    GF.WriteLine(GF.stringLoggingData.ScriptFailedCompilation4);
                    Console.WriteLine();
                    GF.WriteLine(GF.stringLoggingData.ImportantAbove);
                    Console.WriteLine("\n\n\n\n");
                }
                catch (Exception e)
                {
                    GF.WriteLine(e.Message);
                    GF.WriteLine(e.StackTrace!);
                    GF.EnterToContinue();
                }

            }

            if (ESLify.EditedMergedPluginNeedsRebuild.Any())
            {
                GF.WriteLine("Please rebuild these merges, unless you know it doesn't need it.");
                foreach (string plugin in ESLify.EditedMergedPluginNeedsRebuild)
                {
                    GF.WriteLine(plugin);
                }
            }
        }

    }
}