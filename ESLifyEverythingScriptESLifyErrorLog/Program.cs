using ESLifyEverythingGlobalDataLibrary;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Archives;
using System.Security.Policy;

namespace ESLifyEverythingScriptESLifyErrorLog
{
    internal class Program
    {
        static List<string> log = new List<string>();
        static Dictionary<string, List<ScriptData>> ProblemScripts = new Dictionary<string, List<ScriptData>>();

        static void Main(string[] args)
        {
            bool startUp = GF.Startup(out HashSet<StartupError> startupError, "ESLifyEverythingScriptESLifyErrorLog_Log.txt");

            if (!startupError.Contains(StartupError.InvalidStartUp) && startUp)
            {
                if (GF.Settings.MO2.MO2Support && Directory.Exists(GF.Settings.MO2.MO2ModFolder))
                {
                    if (CreateLogDataReference())
                    {
                        ExportLog();
                    }
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.MO2SupportNotAvailable1);
                    GF.WriteLine(GF.stringLoggingData.MO2SupportNotAvailable2);
                }
            }

            Console.WriteLine(GF.stringLoggingData.EnterToExit);
            Console.ReadLine();
        }

        static bool CreateLogDataReference()
        {
            GF.WriteLine(GF.stringLoggingData.StartedReadingESLifyLog);
            using (StreamReader sr = File.OpenText("ESLifyEverything_Log.txt"))
            {
                string? s = "";
                bool reading = false;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Contains("PapyrusCompiler.exe")) continue;

                    if (reading)
                    {
                        log.Add(s);
                        string line = s;
                        if (line.Contains("ExtractedBSAModData"))
                        {
                            string subpathID = "Source\\Scripts\\";
                            if (!line.Contains(subpathID))
                            {
                                line = line.Replace("Source\\Scripts", subpathID);
                            }
                            string scriptName = line.Substring(line.IndexOf(subpathID) + subpathID.Length);
                            //scriptName = scriptName.Remove('(');
                            scriptName = scriptName.Remove(scriptName.IndexOf(".psc"));
                            if (ProblemScripts.TryAdd(scriptName, new List<ScriptData>()))
                            {
                                GF.WriteLine(GF.stringLoggingData.FoundProblemScript + scriptName);
                            }
                        }

                        if (line.Contains("No output generated for ", StringComparison.OrdinalIgnoreCase))
                        {
                            string scriptName = line.Replace("No output generated for ", "");
                            scriptName = scriptName.Replace(", compilation failed.", "");
                            scriptName = scriptName.Replace(".psc", "");
                            if (ProblemScripts.TryAdd(scriptName, new List<ScriptData>()))
                            {
                                GF.WriteLine(GF.stringLoggingData.FoundProblemScript + scriptName);
                            }
                        }
                    }
                    if(s.Contains("Script ESLify Seperator", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reading)
                        {
                            GF.WriteLine(GF.stringLoggingData.FoundEndingSeperator);
                            break;
                        }
                        GF.WriteLine(GF.stringLoggingData.FoundBeginingSeperator);
                        reading = true;
                    }
                }
            }

            GF.WriteLine(GF.stringLoggingData.CheckingMO2Folders);
            IEnumerable<string> modFolders = Directory.EnumerateDirectories(GF.Settings.MO2.MO2ModFolder, "*", SearchOption.TopDirectoryOnly);
            
            foreach(string modFolder in modFolders)
            {
                if(Directory.Exists(Path.Combine(modFolder, "Scripts")))
                {
                    foreach(string scriptName in ProblemScripts.Keys)
                    {
                        if(Directory.Exists(Path.Combine(modFolder, $"Scripts\\{scriptName}.pex")))
                        {
                            ScriptData scriptData = new ScriptData();
                            scriptData.location = Path.GetFileName(modFolder);
                            scriptData.modLink = GetNexusLinkFromMetaini(modFolder);

                            ProblemScripts[scriptName].Add(scriptData);
                            GF.WriteLine(String.Format(GF.stringLoggingData.ScriptFoundIn, scriptName) + Path.GetFileName(modFolder));
                            
                        }
                    }
                }

                IEnumerable<string> bsas = Directory.EnumerateFiles(modFolder, "*.bsa", SearchOption.TopDirectoryOnly);
                if (bsas.Any())
                {
                    foreach(string bsa in bsas)
                    {
                        IArchiveReader reader = Archive.CreateReader(GameRelease.SkyrimSE, bsa);
                        if (reader.TryGetFolder("Scripts", out IArchiveFolder? archiveFolder))
                        {
                            foreach (var item in archiveFolder.Files)
                            {
                                foreach (string scriptName in ProblemScripts.Keys)
                                {
                                    if (item.Path.Contains(scriptName, StringComparison.OrdinalIgnoreCase))
                                    {
                                        ScriptData scriptData = new ScriptData();
                                        scriptData.location = Path.GetFileName(modFolder);
                                        scriptData.modLink = GetNexusLinkFromMetaini(modFolder);

                                        ProblemScripts[scriptName].Add(scriptData);
                                        GF.WriteLine(String.Format(GF.stringLoggingData.ScriptFoundIn, scriptName) + Path.GetFileName(modFolder));
                                        GF.WriteLine(GF.stringLoggingData.InsideBSA + Path.GetFileName(bsa));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return ProblemScripts.Any();
        }

        private static void ExportLog()
        {
            string path = "GeneratedErrorReports\\" + DateTime.Now.ToString().Replace('/', '_').Replace(' ', '_').Replace(':', '_');
            GF.WriteLine(GF.stringLoggingData.GeneratingInFolder + path);
            Directory.CreateDirectory(path);
            using (StreamWriter? gitReport = File.CreateText($"{path}\\GitHubReport.txt"))
            {
                gitReport.WriteLine("<details>");
                gitReport.WriteLine("<summary>ESLify Everything Papyrus Compiler log</summary>");
                foreach (string line in log)
                {
                    gitReport.WriteLine(line);
                }
                gitReport.WriteLine("</details>");

                gitReport.WriteLine("Failed Scripts and where to find them: ");
                gitReport.WriteLine();
                foreach (string scriptName in ProblemScripts.Keys)
                {
                    gitReport.WriteLine("- " + scriptName);
                    foreach (ScriptData scriptData in ProblemScripts[scriptName])
                    {
                        gitReport.WriteLine("- MO2 Mod Folder: " + scriptData.location);
                        gitReport.WriteLine("  - Link: " + scriptData.modLink);
                    }
                }
            }

            using (StreamWriter? nexusReport = File.CreateText($"{path}\\NexusReport.txt"))
            {
                nexusReport.WriteLine("ESLify Everything Papyrus Compiler log");
                nexusReport.WriteLine("[spoiler]");
                foreach (string line in log)
                {
                    nexusReport.WriteLine(line);
                }
                nexusReport.WriteLine("[/spoiler]");

                nexusReport.WriteLine("Failed Scripts and where to find them: ");
                int i = 1;
                foreach (string scriptName in ProblemScripts.Keys)
                {
                    nexusReport.WriteLine(i + ". " + scriptName);
                    foreach (ScriptData scriptData in ProblemScripts[scriptName])
                    {
                        nexusReport.WriteLine("- MO2 Mod Folder: " + scriptData.location);
                        nexusReport.WriteLine("- Link: " + scriptData.modLink);
                    }
                    i++;
                }
            }
        }

        private static string GetNexusLinkFromMetaini(string modFolder)
        {
            string metaPath = Path.Combine(modFolder, "meta.ini");
            if (File.Exists(metaPath))
            {
                string[] fileLines = File.ReadAllLines(metaPath);
                for(int i = 0; i < fileLines.Length; i++)
                {
                    if (fileLines[i].Contains("url="))
                    {
                        return fileLines[i].Replace("url=", "");
                    }
                }
            }

            return "Nexus Link unavailible";
        }
    }

    public class ScriptData
    {
        public string location = "";
        public string modLink = "";
    }
}