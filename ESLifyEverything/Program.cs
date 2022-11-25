using ESLifyEverything.FormData;
using ESLifyEverything.PluginHandles;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;
using System.Text.Json;

namespace ESLifyEverything
{
    public static partial class Program
    {
        //List of dictionary of all enabled and valid CompactedModData.
        //The entire program functions using this.
        public static Dictionary<string, CompactedModData> CompactedModDataD = new Dictionary<string, CompactedModData>(StringComparer.OrdinalIgnoreCase);

        ////List of dictionary of all enabled and valid CompactedModData that has already been run over Voice and FaceGen
        //public static Dictionary<string, CompactedModData> CompactedModDataDNoFaceVoice = new Dictionary<string, CompactedModData>();

        //When populated it holds all plugin names parsed from your plugins.txt file from your my games folder
        public static string[] LoadOrder = new string[0];

        //When populated it holds the plugins with attached BSAs without the extention
        public static List<string> LoadOrderNoExtensions = new List<string>();

        //Populated by _BasicSingleFile.json ModConfigurations
        public static HashSet<BasicSingleFile> BasicSingleModConfigurations = new HashSet<BasicSingleFile>();

        //Populated by _BasicDirectFolder.json ModConfigurations
        public static HashSet<BasicDirectFolder> BasicDirectFolderModConfigurations = new HashSet<BasicDirectFolder>();

        //Populated by _BasicDataSubfolder.json ModConfigurations
        public static HashSet<BasicDataSubfolder> BasicDataSubfolderModConfigurations = new HashSet<BasicDataSubfolder>();

        //Populated by _ComplexTOML.json ModConfigurations
        public static HashSet<ComplexTOML> ComplexTOMLModConfigurations = new HashSet<ComplexTOML>();

        //Populated by _DelimitedFormKeys.json ModConfigurations
        public static HashSet<DelimitedFormKeys> DelimitedFormKeysModConfigurations = new HashSet<DelimitedFormKeys>();

        //Populated by script names that failed to compile during Script ESLify
        public static HashSet<string> FailedToCompile = new HashSet<string>();

        //Imports any CompactedModData or MergeCache's with names matching
        public static HashSet<string> AlwaysCheckList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        //Stores what merges need to be rebuilt after a plugin that was merged into it was edited
        public static HashSet<string> EditedMergedPluginNeedsRebuild = new HashSet<string>();

        //Obsolete
        ////End identifier to prompt that ESLify Everything output FaceGen data
        ////public static bool EditedFaceGen = false;

        //End identifier to prompt that ESLify Everything output BSAs were Extracted
        public static bool BSAExtracted = false;

        //identifier for ESLify Everything detected new mods and it needs to run Script ESLify
        public static bool NewOrUpdatedMods = false;

        //Imports all CompactedModData and MergeCache's
        public static bool CheckEverything = false;

        //Main method that starts all features for eslify
        //Currently there are no Console Arguments, I will be adding some eventually
        static void Main(string[] args)
        {
            try
            {
                HandleArgs(args);
                if (StartUp(out StartupError StartupError, "ESLifyEverything_Log.txt"))
                {
                    Console.WriteLine("Sucessful startup");
                    if (StartupError == 0)
                    {
                        XEditSession();
                    }
                    else
                    {
                        GF.WriteLine(GF.stringLoggingData.SkipingSessionLogNotFound);
                    }

                    DevLog.Pause("After Log Reader Pause");

                    GF.MoveCompactedModDataJsons();

                    if (File.Exists(".\\Properties\\CustomPluginOutputLocations.json"))
                    {
                        HandleMod.CustomPluginOutputLocations = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(".\\Properties\\CustomPluginOutputLocations.json"))!;
                    }

                    Console.WriteLine("\n\n\n\n");
                    Console.WriteLine(GF.stringLoggingData.StartBSAExtract);
                    Task bsamod = LoadOrderBSAData();
                    bsamod.Wait();
                    bsamod.Dispose();

                    DevLog.Pause("After BSA Proccesing Pause");

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingMergeCache);
                    BuildMergedData();

                    DevLog.Pause("After zMerge Reader Pause");

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.ImportingAllModData);
                    ImportModData(Path.Combine(GF.Settings.DataFolderPath, "CompactedForms"));
                    ImportModData(GF.CompactedFormsFolder);

                    DevLog.Pause("After CompactedForms Import Pause");

                    if (!GF.Settings.AutoRunESLify)
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
                    //Auto Run
                    else
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingVoiceESLify);
                        VoiceESLifyEverything();

                        DevLog.Pause("After Voice ESLify AutoRun Pause");

                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingFaceGenESLify);
                        FaceGenESLifyEverything();

                        DevLog.Pause("After FaceGen ESLify AutoRun Pause");

                        //MergeDictionaries();

                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingDataFileESLify);
                        GetESLifyModConfigurationFiles();
                        ESLifyAllDataFiles();
                        InternallyCodedDataFileConfigurations();

                        DevLog.Pause("After Data File ESLify AutoRun Pause");
                    }


                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingScriptESLify);
                    Task Scripts = ExtractScripts();
                    Scripts.Wait();
                    Scripts.Dispose();

                    DevLog.Pause("After Script ESLify Pause");

                    if (GF.Settings.RunSubPluginCompaction)
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartPluginReader);
                        ReadLoadOrder();

                        DevLog.Pause("After Plugin ESLify Pause");
                    }



                    FinalizeData();
                }
                else if (File.Exists(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName)))
                {
                    XEditSession();
                }
                switch (StartupError)
                {
                    case StartupError.OK:
                        break;
                    case StartupError.xEditLogNotFound:
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.xEditlogNotFound);
                        break;
                    default:
                        break;
                }
                
            }
            #region Catch
            catch (ArgumentHelpException) { }
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

            if (BSAExtracted)
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
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(GF.stringLoggingData.ImportantBelow);
                GF.WriteLine(GF.stringLoggingData.ImportantBelow1);
                Console.WriteLine();
                Directory.CreateDirectory(Path.Combine(GF.Settings.OutputFolder, GF.SourceSubPath));
                foreach(string error in FailedToCompile)
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

            if (EditedMergedPluginNeedsRebuild.Any())
            {
                GF.WriteLine("Please rebuild these merges, unless you know it doesn't need it.");
                foreach(string plugin in EditedMergedPluginNeedsRebuild)
                {
                    GF.WriteLine(plugin);
                }
            }

            Console.WriteLine();
            GF.WriteLine(GF.stringLoggingData.EnterToExit);
            Console.ReadLine();
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
                    string[] importCMD = arg.Replace("-c=", "").Split(',');
                    foreach (string cmd in importCMD)
                    {
                        AlwaysCheckList.Add(cmd.Trim());
                    }
                }
                else if (arg.IndexOf("-check=", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    DevLog.Log(arg);
                    string[] importCMD = arg.Replace("-check=", "").Split(',');
                    foreach (string cmd in importCMD)
                    {
                        AlwaysCheckList.Add(cmd.Trim());
                    }
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
            Console.WriteLine("-h  or -help       Prints this message output and cancels other processes");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-c= or -check=    followed by a plugin name will check the corresponding CompactedModData on Face and Voice.");
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
        }
    }
}