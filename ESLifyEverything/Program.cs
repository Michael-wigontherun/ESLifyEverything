using ESLifyEverything.FormData;
using ESLifyEverything.PluginHandles;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;
using System.Diagnostics;
using System.Text.Json;

namespace ESLifyEverything
{
    public static partial class Program
    {
        //List of dictionary of all enabled and valid CompactedModData.
        //The entire program functions using this.
        public static Dictionary<string, CompactedModData> CompactedModDataD = new(StringComparer.OrdinalIgnoreCase);

        ////List of dictionary of all enabled and valid CompactedModData that has already been run over Voice and FaceGen
        //public static Dictionary<string, CompactedModData> CompactedModDataDNoFaceVoice = new Dictionary<string, CompactedModData>();

        //When populated it holds all plugin names parsed from your plugins.txt file from your my games folder
        public static string[] ActiveLoadOrder = Array.Empty<string>();

        //When populated it holds the plugins with attached BSAs without the extention
        public static List<string> LoadOrderNoExtensions = new ();

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

        //Obsolete
        ////End identifier to prompt that ESLify Everything output FaceGen data
        ////public static bool EditedFaceGen = false;

        //End identifier to prompt that ESLify Everything output BSAs were Extracted
        public static bool BSANotExtracted = false;

        //identifier for ESLify Everything detected new mods and it needs to run Script ESLify
        public static bool NewOrUpdatedMods = false;

        //Imports all CompactedModData and MergeCache's
        public static bool CheckEverything = false;

        //Ignore found Outputted scripts and run all processes except Script ESLify CMD Argument
        public static bool _IgnoreScripts = false;

        //This will toggle whether to ask or attempt to run mergify bash tags
        public static bool MergesFound = false;

        //This is the switch for the -OnlyRunxEditReader argument
        public static bool OnlyRunxEditReader = false;

        //This is the switch for the -NP argument
        public static bool NP = true;

        public static void BuildStuff(HashSet<StartupError> startupError)
        {
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

        public static void ImportData()
        {
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

        public static void AutoRunESLifyDataFiles(bool PausesPossible = false)
        {
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

        //Main method that starts all features for eslify
        //Currently there are no Console Arguments, I will be adding some eventually
        static void Main(string[] args)
        {
            try
            {
                HandleArgs(args);

                if (OnlyRunxEditReader)
                {
                    GF.StartUp(out HashSet<StartupError> startupErrorlogReader, "ESLifyEverythingxEditLogReader_Log.txt");

                    BuildStuff(startupErrorlogReader);

                    if (NP)
                    {
                        GF.WriteLine(GF.stringLoggingData.EnterToExit);
                        Console.ReadLine();
                    }
                    return;
                }

                bool startUp = StartUp(out HashSet<StartupError> startupError, "ESLifyEverything_Log.txt");

                if (!startupError.Contains(StartupError.InvalidStartUp) && startUp)
                {
                    Console.WriteLine("Sucessful startup");

                    GF.ValidStartUp();

                    BuildStuff(startupError);

                    GF.MoveCompactedModDataJsons();

                    ImportData();

                    if (GF.Settings.AutoRunESLify) AutoRunESLifyDataFiles(true);
                    else//Opens Menus
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingVoiceESLify);
                        VoiceESlIfyMenu();

                        DevLog.Pause("After Voice ESLify Menu Pause");

                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingFaceGenESLify);
                        FaceGenESlIfyMenu();

                        DevLog.Pause("After FaceGen ESLify Menu Pause");

                        //MergeDictionaries();

                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingDataFileESLify);
                        ESLifyDataFilesMainMenu();

                        DevLog.Pause("After Data File ESLify Menu Pause");
                    }

                    if (!startupError.Contains(StartupError.OutputtedScriptsFound))
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingScriptESLify);
                        Task Scripts = ExtractScripts();
                        Scripts.Wait();
                        Scripts.Dispose();

                        DevLog.Pause("After Script ESLify Pause");
                    }

                    if (GF.Settings.RunSubPluginCompaction)
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartPluginReader);
                        ReadLoadOrder();

                        DevLog.Pause("After Plugin ESLify Pause");
                    }

                    Console.WriteLine();
                    GF.WriteLine(GF.stringLoggingData.EnterToContinue);
                    Console.ReadLine();

                    FinalizeData();
                }
                else
                {
                    foreach(var startuperror in startupError)
                    {
                        Console.Write(startuperror + " : ");
                        
                    }
                    Console.WriteLine();

                    if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName)))
                    {
                        XEditSession();

                        DevLog.Pause("After Startup Invalid Log Reader Pause");
                    }
                    if (Directory.Exists(GF.Settings.DataFolderPath))
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingMergeCache);
                        BuildMergedData();
                        
                        DevLog.Pause("After Startup Invalid zMerge Reader Pause");
                    }
                }
                
                GF.EndStartUpErrorLoggig(startupError);


            }
            #region Catch
            catch (ArgumentHelpException) { }
            catch (MissingFileException e)
            {
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(e.ToString());
                GF.WriteLine(e.Message);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine("AggregateException. Please report to GitHub with log file.");
                GF.WriteLine(e.ToString());
                GF.WriteLine(e.Message);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine("ObjectDisposedException. Please report to GitHub with log file.");
                GF.WriteLine(e.ToString());
                GF.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(e.ToString());
                GF.WriteLine(e.Message);
            }
            #endregion Catch

            //if (EditedFaceGen)
            //{
            //    Console.WriteLine();
            //    GF.WriteLine(GF.stringLoggingData.EditedFaceGen);
            //    GF.RunFaceGenFix();
            //}

            if (BSANotExtracted)
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

            if (FailedToCompile.Any())
            {
                try
                {
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.ImportantBelow);
                    GF.WriteLine(GF.stringLoggingData.ImportantBelow1);
                    Console.WriteLine();
                    Directory.CreateDirectory(Path.Combine(GF.Settings.OutputFolder, GF.SourceSubPath));
                    foreach (string error in FailedToCompile)
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
                    GF.WriteLine(GF.stringLoggingData.EnterToContinue);
                    Console.ReadLine();
                }
                
            }

            if (EditedMergedPluginNeedsRebuild.Any())
            {
                GF.WriteLine("Please rebuild these merges, unless you know it doesn't need it.");
                foreach(string plugin in EditedMergedPluginNeedsRebuild)
                {
                    GF.WriteLine(plugin);
                }
            }

            if (MergesFound)
            {
                if (GF.Settings.AutoRunMergifyBashedTags)
                {
                    RunMergifyBashTags();
                }
                else
                {
                    MergifyBashTagsMenu();
                }
            }

            Console.WriteLine();
            if (NP)
            {
                GF.WriteLine(GF.stringLoggingData.EnterToExit);
                Console.ReadLine();
            }
        }

        //Extra Startup stuff that ESLify Everything needs
        private static bool StartUp(out HashSet<StartupError> startupError, string ProgramLogName)
        {
            bool startup = GF.StartUp(out startupError, ProgramLogName);
            if (startupError.Contains(StartupError.OutputtedScriptsFound) && _IgnoreScripts)
            {
                startup = true;
            }
            return startup;
        }

        private static void HandleArgs(string[] args)
        {
            foreach(string arg in args)
            {
                if(arg.Equals("-h", StringComparison.OrdinalIgnoreCase) || arg.Equals("-help", StringComparison.OrdinalIgnoreCase))
                {
                    GF.GetStringResources();
                    Help();
                    throw new ArgumentHelpException();
                }
                else if (arg.Equals("-c=a", StringComparison.OrdinalIgnoreCase))
                {
                    CheckEverything = true;
                    DevLog.Log("CheckEverything: true");
                }
                else if (arg.Equals("-check=a", StringComparison.OrdinalIgnoreCase))
                {
                    CheckEverything = true;
                    DevLog.Log("CheckEverything: true");
                }
                else if (arg.IndexOf("-c=", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    DevLog.Log(arg);
                    string[] importCMD = arg.Replace("-c=", "").Split(',', StringSplitOptions.TrimEntries);
                    foreach (string cmd in importCMD)
                    {
                        AlwaysCheckList.Add(cmd);
                    }
                }
                else if (arg.IndexOf("-check=", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    DevLog.Log(arg);
                    string[] importCMD = arg.Replace("-check=", "").Split(',', StringSplitOptions.TrimEntries);
                    foreach (string cmd in importCMD)
                    {
                        AlwaysCheckList.Add(cmd);
                    }
                }
                else if (arg.IndexOf("-IMD=", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    DevLog.Log(arg);
                    string[] importCMD = arg.Replace("-IMD=", "").Split(',', StringSplitOptions.TrimEntries);
                    foreach (string cmd in importCMD)
                    {
                        AlwaysIgnoreList.Add(cmd);
                    }
                }
                else if (arg.IndexOf("-IgnoreModData=", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    DevLog.Log(arg);
                    string[] importCMD = arg.Replace("-IgnoreModData=", "").Split(',', StringSplitOptions.TrimEntries);
                    foreach (string cmd in importCMD)
                    {
                        AlwaysIgnoreList.Add(cmd);
                    }
                }
                else if(arg.Equals("-IgnoreScripts", StringComparison.OrdinalIgnoreCase))
                {
                    _IgnoreScripts = true;
                }
                else if (arg.Equals("-OnlyRunxEditReader", StringComparison.OrdinalIgnoreCase))
                {
                    OnlyRunxEditReader = true;
                }
                else if (arg.Equals("-NP", StringComparison.OrdinalIgnoreCase))
                {
                    NP = false;
                }
                else
                {
                    GF.WriteLine($"Invalid Argument exception, {arg} not known.");
                    throw new ArgumentHelpException();
                }
            }
        }

        private static void Help()
        {
            Console.WriteLine("Case does not matter for any of the following.");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-h  or -help        Prints this message output and cancels other processes");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-c= or -check=      followed by a plugin name will check the corresponding CompactedModData on Face and Voice.");
            Console.WriteLine();
            Console.WriteLine("Example: -c=\"DIVERSE SKYRIM.esp\" will always check the CompactedModData associated with DIVERSE SKYRIM.esp.");
            Console.WriteLine("Or if it was a Merge it would check the merge.");
            Console.WriteLine();
            Console.WriteLine("You have 2 options you can do repeated -i=\"[plugin name]\" or you can add them delimit them with \",\".");
            Console.WriteLine();
            Console.WriteLine("Example: -c=\"DIVERSE SKYRIM.esp\" -i=\"GIST soul trap.esp\" -c=\"Castle Volkihar Rebuilt.esp\"");
            Console.WriteLine("Example: -c=\"DIVERSE SKYRIM.esp, GIST soul trap.esp, Castle Volkihar Rebuilt.esp\"");
            Console.WriteLine("  * The second is preferred");
            Console.WriteLine("  * Spaces are not necessary after comma's.");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-IMD or IgnoreModData     same aditional syntax as the -check argument.");
            Console.WriteLine();
            Console.WriteLine("Example: -IMD=\"LOTDPatchMerge.esp\" will ignore my LOTDPatchMerge.esp merge");
            Console.WriteLine();
            Console.WriteLine("It can be used for either a MergeCache or CompactedModData.");
            Console.WriteLine("It is not recomend to use this for anything unless you know there will never be mod data for it.");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(       "-IgnoreScripts      will start up all normal processes except Script ESLify. Aslong as all other processes are" +
                                "valid.");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-OnlyRunxEditReader      will only run the xEdit Reader and nothing else. ");
            Console.WriteLine("Set another prosess of ESLify Everything with this argument to just quickly run the xEdit Log reader");
            Console.WriteLine("Add -NP to have it close everything as well.");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-NP       This will disable the pause at the end and it will close.");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
        }
    }
}