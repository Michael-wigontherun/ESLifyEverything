using ESLifyEverything.FormData;
using ESLifyEverything.XEdit;
using System;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace ESLifyEverything
{
    public static partial class Program
    {
        public static Dictionary<string, CompactedModData> CompactedModDataD = new Dictionary<string, CompactedModData>();
        public static bool EditedFaceGen = false;

        static void Main(string[] args)
        {
            try
            { 
                if (GF.Startup(out int StartupError, "ESLifyEverything_Log.txt"))
                {
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
                    ImportModData(Path.Combine(GF.Settings.SkyrimDataFolderPath, "CompactedForms"));
                    if (GF.Settings.OutputToOptionalFolder)
                    {
                        ImportModData(Path.Combine(GF.Settings.OptionalOutputFolder, "CompactedForms"));
                    }

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
                    Task DAR = InumDataSubfolder(Path.Combine(GF.Settings.SkyrimDataFolderPath, "meshes"), "_CustomConditions", "_conditions.txt", GF.stringLoggingData.DARFileAt, GF.stringLoggingData.ConditionsUnchanged);
                    DAR.Wait();
                    
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingSPIDESLify);
                    InumSwappers(GF.Settings.SkyrimDataFolderPath, "*_DISTR.ini", GF.stringLoggingData.SPIDFileAt, GF.stringLoggingData.SPIDFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingBaseObjectESLify);
                    InumSwappers(GF.Settings.SkyrimDataFolderPath, "*_SWAP.ini", GF.stringLoggingData.BaseObjectFileAt, GF.stringLoggingData.BaseObjectFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingKIDESLify);
                    InumSwappers(GF.Settings.SkyrimDataFolderPath, "*_KID.ini", GF.stringLoggingData.KIDFileAt, GF.stringLoggingData.KIDFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingFLMESLify);
                    InumSwappers(GF.Settings.SkyrimDataFolderPath, "*_FLM.ini", GF.stringLoggingData.FLMFileAt, GF.stringLoggingData.FLMFileUnchanged);
                    
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingAnimObjectESLify);
                    InumSwappers(GF.Settings.SkyrimDataFolderPath, "*_ANIO.ini", GF.stringLoggingData.AnimObjectFileAt, GF.stringLoggingData.AnimObjectFileUnchanged);
                    
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingENBLightsForEffectShadersESLify);
                    InumSwappers(GF.Settings.SkyrimDataFolderPath, "*_ENBL.ini", GF.stringLoggingData.ENBLightsForEffectShadersFileAt, GF.stringLoggingData.ENBLightsForEffectShadersFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingAutoBodyESLify);
                    AutoBody();

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingPayloadInterpreterESLify);
                    InumSwappers(Path.Combine(GF.Settings.SkyrimDataFolderPath, "SKSE\\PayloadInterpreter\\Config"), "*.ini", GF.stringLoggingData.PayloadInterpreterFileAt, GF.stringLoggingData.PayloadInterpreterFileUnchanged);

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingSKSEINIESLify);
                    //Task DAR = InumDAR();
                    Task SKSE = InumDataSubfolder(Path.Combine(GF.Settings.SkyrimDataFolderPath, "skse"), "plugins", "*.ini", GF.stringLoggingData.SKSEINIFileAt, GF.stringLoggingData.SKSEINIFileUnchanged);
                    SKSE.Wait();

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingCustomSkillsESLify);
                    Task CustomSkills = CustomSkillsFramework();
                    CustomSkills.Wait();


                    Console.WriteLine("\n\n\n\n");
                }
                
                GF.WriteLine($"Start up Error: {StartupError}");
                switch (StartupError)
                {
                    case 0:
                        GF.GenerateSettingsFileError();
                        break;
                    default:
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.ExitingFolderNotFound);
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
                GF.WriteLine(GF.stringLoggingData.EditedFaceGen);
            }

            GF.WriteLine(GF.stringLoggingData.EnterToExit);
            Console.ReadLine();
        }
        

        



    }
}