using ESLifyEverything.BSAHandlers;
using ESLifyEverything.FormData;
using ESLifyEverything.XEdit;
using System;
using System.Diagnostics;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace ESLifyEverything
{
    public static partial class Program
    {
        public static Dictionary<string, CompactedModData> CompactedModDataD = new Dictionary<string, CompactedModData>();

        public static List<string> LoadOrderNoExtensions = new List<string>();
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
                    ImportModData(Path.Combine(GF.Settings.OutputFolder, "CompactedForms"));

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
                    }

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingDARESLify);
                    //Task DAR = InumDAR();
                    Task DAR = InumDataSubfolder(Path.Combine(GF.Settings.DataFolderPath, "meshes"), "_CustomConditions", "_conditions.txt", GF.stringLoggingData.DARFileAt, GF.stringLoggingData.ConditionsUnchanged);
                    DAR.Wait();
                    DAR.Dispose();

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingSPIDESLify);
                    InumSwappers(GF.Settings.DataFolderPath, "*_DISTR.ini", GF.stringLoggingData.SPIDFileAt, GF.stringLoggingData.SPIDFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingBaseObjectESLify);
                    InumSwappers(GF.Settings.DataFolderPath, "*_SWAP.ini", GF.stringLoggingData.BaseObjectFileAt, GF.stringLoggingData.BaseObjectFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingKIDESLify);
                    InumSwappers(GF.Settings.DataFolderPath, "*_KID.ini", GF.stringLoggingData.KIDFileAt, GF.stringLoggingData.KIDFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingFLMESLify);
                    InumSwappers(GF.Settings.DataFolderPath, "*_FLM.ini", GF.stringLoggingData.FLMFileAt, GF.stringLoggingData.FLMFileUnchanged);
                    
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingAnimObjectESLify);
                    InumSwappers(GF.Settings.DataFolderPath, "*_ANIO.ini", GF.stringLoggingData.AnimObjectFileAt, GF.stringLoggingData.AnimObjectFileUnchanged);
                    
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingENBLightsForEffectShadersESLify);
                    InumSwappers(GF.Settings.DataFolderPath, "*_ENBL.ini", GF.stringLoggingData.ENBLightsForEffectShadersFileAt, GF.stringLoggingData.ENBLightsForEffectShadersFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingAutoBodyESLify);
                    AutoBody();

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingPayloadInterpreterESLify);
                    InumSwappers(Path.Combine(GF.Settings.DataFolderPath, "SKSE\\PayloadInterpreter\\Config"), "*.ini", GF.stringLoggingData.PayloadInterpreterFileAt, GF.stringLoggingData.PayloadInterpreterFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingSKSEINIESLify);
                    //Task DAR = InumDAR();
                    Task SKSE = InumDataSubfolder(Path.Combine(GF.Settings.DataFolderPath, "skse"), "plugins", "*.ini", GF.stringLoggingData.SKSEINIFileAt, GF.stringLoggingData.SKSEINIFileUnchanged);
                    SKSE.Wait();
                    SKSE.Dispose();

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

                    Console.WriteLine("\n\n\n\n");
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
                    default:
                        break;
                }
            }
            catch(AggregateException e)
            {
                GF.WriteLine("AggregateException. Please report to GitHub with log file.");
                GF.WriteLine(e.ToString());
                GF.WriteLine(e.Message);
            }
            catch (ObjectDisposedException e)
            {
                GF.WriteLine("ObjectDisposedException. Please report to GitHub with log file.");
                GF.WriteLine(e.ToString());
                GF.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                GF.WriteLine(e.ToString());
                GF.WriteLine(e.Message);
                
            }

            if (EditedFaceGen)
            {
                Console.WriteLine();
                GF.WriteLine(GF.stringLoggingData.EditedFaceGen);
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