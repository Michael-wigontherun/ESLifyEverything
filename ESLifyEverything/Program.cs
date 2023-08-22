using ESLifyEverything.Exceptions;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties;

namespace ESLifyEverything
{
    //Main Handle for ESLifyEverything.exe, Not Excessable
    //public static partial class ESLify
    internal static class Program
    {
        //Main method that starts all base features for ESLify Everything
        static void Main(string[] args)
        {
            try
            {
                ESLify.HandleArgs(args);

                if (ESLify.OnlyRunxEditReader)
                {
                    GF.StartUp(out HashSet<StartupError> startupErrorlogReader, "ESLifyEverythingxEditLogReader_Log.txt");

                    ESLify.BuildCaches(GF.logName, startupErrorlogReader);

                    ESLify.EndPause();
                    return;
                }

                bool startUp = ESLify.StartUp(out HashSet<StartupError> startupError, "ESLifyEverything_Log.txt");

                //Main features
                if (!startupError.Contains(StartupError.InvalidStartUp) && startUp)
                {
                    Console.WriteLine("Sucessful startup");

                    GF.ValidStartUp();

                    ESLify.BuildCaches(GF.logName, startupError);

                    GF.MoveCompactedModDataJsons();

                    ESLify.ImportData(GF.logName);

                    ESLify.ESLifyDataFiles(GF.logName, true);

                    ESLify.StartScriptESLify(GF.logName, startupError);

                    if (GF.Settings.RunSubPluginCompaction)
                    {
                        Console.WriteLine("\n\n\n\n");
                        GF.WriteLine(GF.stringLoggingData.StartPluginReader);
                        ESLify.ReadLoadOrder();

                        DevLog.Pause("After Plugin ESLify Pause");
                    }

                    GF.EnterToContinue();

                    ESLify.FinalizeData();
                }
                //Start Up Fail but still run the xEdit Log Reader and builds Merge Caches
                else
                {
                    GF.StartUpErrorOutput(startupError);

                    ESLify.BuildCaches(GF.logName, startupError);
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

            ESLify.EverythingEndChecks();

            ESLify.RunMergifyBashTagsSettings();

            ESLify.EndPause();
        }
    }

    //ESLify Everything extra Program Execution methods
    public static partial class ESLify
    {
        //Extra Startup stuff that ESLify Everything needs
        internal static bool StartUp(out HashSet<StartupError> startupError, string ProgramLogName)
        {
            bool startup = GF.StartUp(out startupError, ProgramLogName);
            if (startupError.Contains(StartupError.OutputtedScriptsFound) && ESLify._IgnoreScripts)
            {
                startup = true;
            }
            return startup;
        }

        //Handles the Arguments
        public static void HandleArgs(string[] args, bool UknownArgeThrowException = true)
        {
            foreach (string arg in args)
            {
                if (arg.Equals("-h", StringComparison.OrdinalIgnoreCase) || arg.Equals("-help", StringComparison.OrdinalIgnoreCase))
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
                else if (arg.Equals("-IgnoreScripts", StringComparison.OrdinalIgnoreCase))
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
                    if (UknownArgeThrowException)
                    {
                        GF.WriteLine($"Invalid Argument exception, {arg} not known by ESLify Everything.");
                        throw new ArgumentHelpException();
                    }
                }
            }
        }

        //Outputs Help Menu for Console Arguments
        public static void Help()
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
            Console.WriteLine("-IgnoreScripts      will start up all normal processes except Script ESLify. Aslong as all other processes are" +
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

        //Method for -NP Argument
        public static void EndPause()
        {
            if (NP)
            {
                Console.WriteLine();
                GF.WriteLine(GF.stringLoggingData.EnterToExit);
                Console.ReadLine();
            }
        }
    }
}