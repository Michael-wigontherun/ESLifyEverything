using ESLifyEverything.FormData;
using ESLifyEverything.XEdit;
using System;
using System.Resources;
using System.Text.RegularExpressions;

namespace ESLifyEverything
{
    public static partial class Program
    {
        public static Dictionary<string, CompactedModData> CompactedModData = new Dictionary<string, CompactedModData>();

        static void Main(string[] args)
        {
            try
            { 
                if (GF.Startup(out bool onlyxEditLogFail))
                {
                    if (!onlyxEditLogFail)
                    {
                        XEditSession();
                    }
                    else
                    {
                        GF.WriteLine(GF.stringLoggingData.SkipingSessionLogNotFound);
                    }
                    Console.WriteLine("\n\n\n\n");
                    Console.WriteLine(GF.stringLoggingData.ImportingAllModData);
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
                    InumDAR();

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.StartingSPIDESLify);
                    InumSPID();
                }
                else
                {
                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.ExitingFolderNotFound);
                    GF.WriteLine(GF.stringLoggingData.EnterToExit);
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("\n\n\n\n");
                GF.WriteLine(GF.stringLoggingData.EnterToExit);
                Console.ReadLine();
            }
            catch(Exception e)
            {
                GF.WriteLine(e.ToString());
                GF.WriteLine(e.Message);
                GF.WriteLine(GF.stringLoggingData.EnterToExit);
                Console.ReadLine();
            }
        }

        




    }
}