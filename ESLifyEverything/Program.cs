using ESLifyEverything.FormData;
using ESLifyEverything.PluginHandles;
using ESLifyEverything.Properties;
using ESLifyEverything.Properties.DataFileTypes;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;

namespace ESLifyEverything
{
    public static partial class Program
    {
        public static Dictionary<string, CompactedModData> CompactedModDataD = new Dictionary<string, CompactedModData>();

        public static string[] LoadOrder = new string[0];
        public static List<string> LoadOrderNoExtensions = new List<string>();
        public static HashSet<BasicSingleFile> BasicSingleModConfigurations = new HashSet<BasicSingleFile>();
        public static HashSet<BasicDirectFolder> BasicDirectFolderModConfigurations = new HashSet<BasicDirectFolder>();
        public static HashSet<BasicDataSubfolder> BasicDataSubfolderModConfigurations = new HashSet<BasicDataSubfolder>();
        public static HashSet<ComplexTOML> ComplexTOMLModConfigurations = new HashSet<ComplexTOML>();
        public static HashSet<DelimitedFormKeys> DelimitedFormKeysModConfigurations = new HashSet<DelimitedFormKeys>();
        public static HashSet<string> FailedToCompile = new HashSet<string>();

        public static bool EditedFaceGen = false;
        public static bool BSAExtracted = true;
        public static bool NewOrUpdatedMods = false;

        static void Main(string[] args)
        {
            try
            { 
                if (GF.Startup(out int StartupError, "ESLifyEverything_Log.txt"))
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
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.ImportingAllModData);
                    ImportModData(Path.Combine(GF.Settings.DataFolderPath, "CompactedForms"));
                    ImportModData(GF.CompactedFormsFolder);

                    Console.WriteLine("\n\n\n\n");
                    Console.WriteLine(GF.stringLoggingData.StartBSAExtract);
                    Task bsamod = LoadOrderBSAData();
                    bsamod.Wait();
                    bsamod.Dispose();

                    if (!GF.Settings.AutoRunESLify)
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingVoiceESLify);
                        VoiceESlIfyMenu();

                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingFaceGenESLify);
                        FaceGenESlIfyMenu();

                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingDataFileESLify);
                        ESLifyDataFilesMainMenu();
                    }
                    //Auto Run
                    else
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingVoiceESLify);
                        VoiceESLifyEverything();

                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingFaceGenESLify);
                        FaceGenESLifyEverything();

                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartingDataFileESLify);
                        GetESLifyModConfigurationFiles();
                        ESLifyAllDataFiles();
                    }

                    //Console.ReadLine();

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingRaceMenuESLify);
                    RaceMenuESLify();

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingCustomSkillsESLify);
                    Task CustomSkills = CustomSkillsFramework();
                    CustomSkills.Wait();
                    CustomSkills.Dispose();

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingScriptESLify);
                    Task Scripts = ExtractScripts();
                    Scripts.Wait();
                    Scripts.Dispose();

                    if (GF.Settings.RunSubPluginCompaction)
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartPluginReader);
                        ReadLoadOrder();
                    }

                }
                switch (StartupError)
                {
                    case 1:
                        GF.GenerateSettingsFileError();
                        break;
                    case 2:
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.xEditlogNotFound);
                        break;
                    case 3:
                        GF.UpdateSettingsFile();
                        break;
                    default:
                        break;
                }
            }
            catch(AggregateException e)
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

            if (EditedFaceGen)
            {
                Console.WriteLine();
                GF.WriteLine(GF.stringLoggingData.EditedFaceGen);
                GF.RunFaceGenFix();
            }

            if (!BSAExtracted)
            {
                Console.WriteLine();
                GF.WriteLine(GF.stringLoggingData.LoadOrderNotDetectedError);
                GF.WriteLine(GF.stringLoggingData.RunOrReport);
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