using ESLifyEverything.FormData;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;

namespace ESLifyEverything
{
    public static partial class Program
    {
        //List of dictionary of all enabled and valid CompactedModData.
        //The entire program functions using this.
        public static Dictionary<string, CompactedModData> CompactedModDataD = new Dictionary<string, CompactedModData>();

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

        //End identifier to prompt that ESLify Everything output FaceGen data
        public static bool EditedFaceGen = false;

        //End identifier to prompt that ESLify Everything output BSAs were Extracted
        public static bool BSAExtracted = false;

        //identifier for ESLify Everything detected new mods and it needs to run Script ESLify
        public static bool NewOrUpdatedMods = false;

        //Main method that starts all features for eslify
        //Currently there are no Console Arguments, I will be adding some eventually
        static void Main(string[] args)
        {
            try
            {
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

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingMergeCache);
                    BuildMergedData();

                    DevLog.Pause("After zMerge Reader Pause");

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.ImportingAllModData);
                    ImportModData(Path.Combine(GF.Settings.DataFolderPath, "CompactedForms"));
                    ImportModData(GF.CompactedFormsFolder);

                    DevLog.Pause("After CompactedForms Import Pause");

                    Console.WriteLine("\n\n\n\n");
                    Console.WriteLine(GF.stringLoggingData.StartBSAExtract);
                    Task bsamod = LoadOrderBSAData();
                    bsamod.Wait();
                    bsamod.Dispose();

                    DevLog.Pause("After BSA Proccesing Pause");

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
            
            if (EditedFaceGen)
            {
                Console.WriteLine();
                GF.WriteLine(GF.stringLoggingData.EditedFaceGen);
                GF.RunFaceGenFix();
            }

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
                    GF.WriteLine(error + GF.stringLoggingData.ScriptFailedCompilation);
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

            Console.WriteLine();
            GF.WriteLine(GF.stringLoggingData.EnterToExit);
            Console.ReadLine();
        }
        
        

    }
}