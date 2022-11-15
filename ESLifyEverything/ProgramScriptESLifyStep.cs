using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ESLifyEverything.FormData;
using ESLifyEverythingGlobalDataLibrary;

namespace ESLifyEverything
{
    public static partial class Program
    {
        //Menu to AutoRun or Ask if extraction and decompile is necessary
        private static bool ExtractScriptsMenu()
        {
            if (GF.Settings.AutoRunScriptDecompile)
            {
                GF.WriteLine(GF.stringLoggingData.ScriptESLifyMenuA, false, true);
                return true;
            }

            if (NewOrUpdatedMods)
            {
                GF.WriteLine(GF.stringLoggingData.ScriptESLifyMenuA, false, true);
                return true;
            }
            Console.WriteLine(GF.stringLoggingData.ScriptESLifyMenu1);
            Console.WriteLine(GF.stringLoggingData.ScriptESLifyMenu2);
            Console.WriteLine(GF.stringLoggingData.ScriptESLifyMenu3);
            string input = Console.ReadLine() ?? "";
            if(input.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                GF.WriteLine(GF.stringLoggingData.ScriptESLifyMenuN, false,true);
                return false;
            }
            GF.WriteLine(GF.stringLoggingData.ScriptESLifyMenuY, false, true);
            return true;
        }

        //Extracts scripts from BSA's in the order of your load order
        private static async Task<int> ExtractScripts()
        {
            if (ExtractScriptsMenu())
            {
                Console.WriteLine(GF.stringLoggingData.IgnoreBelow);

                for (int i = 0; i < GF.DefaultScriptBSAs.Length; i++)
                {
                    Console.WriteLine(GF.DefaultScriptBSAs[i]);
                    if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, GF.DefaultScriptBSAs[i])))
                    {
                        Task BaseBSAs = ExtractBSAScripts(Path.Combine(GF.Settings.DataFolderPath, GF.DefaultScriptBSAs[i]));
                        BaseBSAs.Wait();
                        BaseBSAs.Dispose();
                    }
                    Console.WriteLine();
                    GF.WriteLine(String.Format(GF.stringLoggingData.ProcessedBSAsLogCount, i + 1, GF.DefaultScriptBSAs.Length));
                    Console.WriteLine();
                }

                Console.WriteLine();
                int loadorderCount = LoadOrderNoExtensions.Count;
                for (int i = 0; i < loadorderCount; i++)
                {
                    if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, LoadOrderNoExtensions[i] + ".bsa")))
                    {
                        Console.WriteLine(LoadOrderNoExtensions[i]);
                        Task ModBSA = ExtractBSAScripts(Path.Combine(GF.Settings.DataFolderPath, LoadOrderNoExtensions[i] + ".bsa"));
                        ModBSA.Wait();
                        ModBSA.Dispose();
                        if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, LoadOrderNoExtensions[i] + " - Textures.bsa")))
                        {
                            Task ModBSATex = ExtractBSAScripts(Path.Combine(GF.Settings.DataFolderPath, LoadOrderNoExtensions[i] + " - Textures.bsa"));
                            ModBSATex.Wait();
                            ModBSATex.Dispose();
                        }
                    }
                    Console.WriteLine();
                    GF.WriteLine(String.Format(GF.stringLoggingData.ProcessedBSAsLogCount, i + 1, loadorderCount));
                    Console.WriteLine();
                }

                DevLog.Pause("After Script BSA Extraction");

                GF.WriteLine(GF.stringLoggingData.RunningChampBSA);
                Task<int> BSAChamp = DecompileScripts(GF.ExtractedBSAModDataPath);
                BSAChamp.Wait();
                if(BSAChamp.Result == 0)
                {
                    BSAChamp.Dispose();

                    GF.WriteLine(GF.stringLoggingData.RunningChampFailsafe);
                    Task<int> BSAChampSlow = DecompileScriptsSlow(GF.ExtractedBSAModDataPath);
                    BSAChampSlow.Wait();
                    BSAChampSlow.Dispose();
                }
                else
                {
                    BSAChamp.Dispose();
                }
                GF.WriteLine(GF.stringLoggingData.EndedChampBSA);

                GF.WriteLine(GF.stringLoggingData.RunningChampLoose);
                Task<int> SourceChamp = DecompileScripts(GF.Settings.DataFolderPath);
                SourceChamp.Wait();
                if (SourceChamp.Result == 0)
                {
                    BSAChamp.Dispose();

                    GF.WriteLine(GF.stringLoggingData.RunningChampFailsafe);
                    Task<int> BSAChampSlow = DecompileScriptsSlow(GF.Settings.DataFolderPath);
                    BSAChampSlow.Wait();
                    BSAChampSlow.Dispose();
                }
                else
                {
                    SourceChamp.Dispose();
                }

                GF.WriteLine(GF.stringLoggingData.EndedChampLoose);

                Console.WriteLine(GF.stringLoggingData.IgnoreAbove);

                DevLog.Pause("After Script Decompilers");
            }

            
            Task inmScripts = ReadAndCompileScripts();
            inmScripts.Wait();
            inmScripts.Dispose();
            

            return await Task.FromResult(0);
        }

        //Runs BSA Browser to extract scripts from BSA path
        private static async Task<int> ExtractBSAScripts(string bsaPath)
        {
            string line = "";
            Process p = new Process();
            p.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
            p.StartInfo.Arguments = $"\"{Path.GetFullPath(bsaPath)}\" -f \".pex\"  -e -o \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\"";
            if (GF.DevSettings.DevLogging)
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                while (!p.StandardOutput.EndOfStream)
                {
                    string tempLine = p.StandardOutput.ReadLine()!;
                    if (tempLine != string.Empty)
                    {
                        line = tempLine;
                    }
                }
            }
            else
            {
                p.Start();
            }
            p.WaitForExit();
            p.Dispose();
            DevLog.Log(line);

            return await Task.FromResult(0);
        }

        //Runs Champolion on script path and outputting to the "ExtractedBSAModData\Source\Scripts" folder
        private static async Task<int> DecompileScripts(string startPath)
        {
            string line = "";
            string scriptsFolder = Path.Combine(Path.GetFullPath(startPath), "Scripts");
            DevLog.Log(scriptsFolder);
            Process p = new Process();
            p.StartInfo.FileName = ".\\Champollion\\champollion.exe";
            p.StartInfo.Arguments = $"\"{scriptsFolder}\" -p \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\\{GF.SourceSubPath}\" -t";//files processed
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            if (GF.Settings.VerboseFileLoging || GF.DevSettings.DevLogging)
            {
                using (StreamWriter stream = File.AppendText(GF.logName))
                {
                    while (!p.StandardOutput.EndOfStream)
                    {
                        line = p.StandardOutput.ReadLine()!;
                        stream.WriteLine(line);
                    }
                    while (!p.StandardError.EndOfStream)
                    {
                        line = p.StandardError.ReadLine()!;
                        stream.WriteLine(line);
                    }
                }
            }
            else
            {
                while (!p.StandardOutput.EndOfStream)
                {
                    line = p.StandardOutput.ReadLine()!;
                }
            }
            p.WaitForExit();
            p.Dispose();

            if (line.Contains("files processed"))
            {
                GF.WriteLine("Champollion: " + line);
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.ChampCrash1);
                GF.WriteLine(GF.stringLoggingData.ChampCrash2);
                GF.WriteLine(line);
                GF.WriteLine(GF.stringLoggingData.ChampCrash3);
                GF.WriteLine(GF.stringLoggingData.ChampCrash4);
                GF.WriteLine(GF.stringLoggingData.ChampCrash5);
                GF.WriteLine(GF.stringLoggingData.ChampCrash6);
                return await Task.FromResult(0);

            }

            return await Task.FromResult(1);
        }

        private static async Task<int> DecompileScriptsSlow(string startPath)
        {
            string line = "";
            string scriptsFolder = Path.Combine(Path.GetFullPath(startPath), "Scripts");
            DevLog.Log(scriptsFolder);
            IEnumerable<string> scripts = Directory.EnumerateFiles(
                        scriptsFolder,
                        "*.pex",
                        SearchOption.TopDirectoryOnly);
            using (StreamWriter stream = File.AppendText(GF.logName))
            {
                foreach (string script in scripts)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = ".\\Champollion\\champollion.exe";
                    p.StartInfo.Arguments = $"\"{Path.GetFullPath(script)}\" -p \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\\{GF.SourceSubPath}\" -t";//files processed
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();

                    
                    while (!p.StandardOutput.EndOfStream)
                    {
                        line = p.StandardOutput.ReadLine()!;
                        if (!line.Contains("1 files processed in "))
                        {
                            stream.WriteLine(line);
                        }
                    }


                    p.WaitForExit();
                    p.Dispose();

                    if (!line.Contains("1 files processed in "))
                    {
                        Console.WriteLine(script);
                        Console.WriteLine(line);

                    }
                }
            }
            return await Task.FromResult(1);
        }

        //Fixes script Form Keys and Compiles them if Compiler is enabled
        private static async Task<int> ReadAndCompileScripts()
        {
            string startFolder = $"{GF.ExtractedBSAModDataPath}\\{GF.SourceSubPath}";
            if (!Directory.Exists(startFolder))
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + startFolder);
                return await Task.FromResult(1);
            }
            bool tempLogging = GF.Settings.VerboseConsoleLoging;
            GF.Settings.VerboseConsoleLoging = false;
            IEnumerable<string> scripts = Directory.EnumerateFiles(
                    startFolder,
                    "*.psc",
                    SearchOption.TopDirectoryOnly);
            List<string> changedFiles = new List<string>();// changed scripts
            foreach (string script in scripts)
            {
                bool changed = false;
                string[] fileLines = FormInScriptFileLineReader(File.ReadAllLines(script), out changed);
                if (changed)
                {
                    GF.WriteLine(GF.stringLoggingData.ScriptSourceFileChanged + script, false, GF.Settings.VerboseFileLoging);
                    string newFilePath = GF.FixOuputPath(script, startFolder, GF.ChangedScriptsPath);
                    File.WriteAllLines(script, fileLines);
                    File.WriteAllLines(newFilePath, fileLines);
                    changedFiles.Add(newFilePath);
                }
            }
            GF.Settings.VerboseConsoleLoging = tempLogging;
            
            if (File.Exists(Path.Combine(GF.GetSkyrimRootFolder(), "Papyrus Compiler\\PapyrusCompiler.exe")))
            {
                if (changedFiles.Any())
                {
                    if (GF.Settings.EnableCompiler)
                    {
                        Console.WriteLine();
                        GF.WriteLine(GF.stringLoggingData.ImportantBelow);
                        GF.WriteLine(GF.stringLoggingData.ScriptFailedCompilation3);
                        Console.WriteLine();
                        GF.WriteLine(GF.stringLoggingData.ScriptESLifyINeedThisDataToBeReported);
                        Console.WriteLine();
                        GF.WriteLine(GF.stringLoggingData.ImportantBelow1);
                        Console.WriteLine();
                        Process p = new Process();
                        p.StartInfo.FileName = Path.Combine(GF.GetSkyrimRootFolder(), "Papyrus Compiler\\PapyrusCompiler.exe");
                        p.StartInfo.Arguments = $"\"{Path.GetFullPath(GF.ChangedScriptsPath)}\" -q -f=\"TESV_Papyrus_Flags.flg\" -a -i=\"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\\{GF.SourceSubPath}\" -o=\"{Path.Combine(Path.GetFullPath(GF.Settings.OutputFolder), "Scripts")}\"";
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.RedirectStandardError = true;
                        p.StartInfo.CreateNoWindow = true;
                        p.Start();
                        using (StreamWriter stream = File.AppendText(GF.logName))
                        {
                            while (!p.StandardOutput.EndOfStream)
                            {
                                string line = p.StandardOutput.ReadLine()!;
                                Console.WriteLine(line);
                                if (!line.Equals(string.Empty))
                                {
                                    stream.WriteLine(line);
                                }

                            }
                        }
                        p.WaitForExit();
                        p.Dispose();
                        Console.WriteLine();
                        GF.WriteLine(GF.stringLoggingData.ImportantAbove);

                        foreach (var changedFile in changedFiles)
                        {
                            if (!File.Exists(Path.Combine(GF.Settings.OutputFolder, "Scripts\\" + Path.ChangeExtension(Path.GetFileName(changedFile), null) + ".pex")))
                            {
                                FailedToCompile.Add(Path.ChangeExtension(Path.GetFileName(changedFile), null));
                            }
                        }
                    }
                    else
                    {
                        GF.WriteLine(GF.stringLoggingData.CompilerIsDisabled);
                        GF.WriteLine(String.Format(GF.stringLoggingData.ChangedScriptsLocated, Path.GetFullPath(GF.ChangedScriptsPath)));
                    }
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.NoChangedScriptsDetected);
                }
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing);
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing2);
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing3);
            }

            return await Task.FromResult(0);
        }
        
        //Parses Script files
        public static string[] FormInScriptFileLineReader(string[] fileLines, out bool changed)
        {
            string FixLineToHex(string line, out string? exactHexValueTrimmed)
            {
                exactHexValueTrimmed = null;
                Regex fullStringCheck = new Regex("GetFormFromFile\\([0-9]+,", RegexOptions.IgnoreCase);
                if (fullStringCheck.IsMatch(line))
                {
                    string stringDecimal = fullStringCheck.Match(line).Value;
                    stringDecimal = stringDecimal.Replace("GetFormFromFile(", "");
                    stringDecimal = stringDecimal.Replace(",", "");
                    //GF.WriteLine(line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    try
                    {
                        if (long.TryParse(stringDecimal, out long value))
                        {
                            string hexNum = string.Format("{0:x}", value);
                            if (hexNum.Length > 6)
                            {
                                hexNum = hexNum.Substring(hexNum.Length - 6).TrimStart('0');
                                exactHexValueTrimmed = hexNum;
                            }
                            string hex = "0x" + hexNum;
                            line = line.Replace(stringDecimal, hex, StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    catch (Exception e)
                    {
                        GF.WriteLine(GF.stringLoggingData.FixDecToHexError);
                        GF.WriteLine(line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        GF.WriteLine(e.Message);
                    }
                }
                return line;
            }

            changed = false;
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(".esp", StringComparison.OrdinalIgnoreCase) || fileLines[i].Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    string? exactHexValueTrimmed = null;
                    if (fileLines[i].Contains("GetFormFromFile(", StringComparison.OrdinalIgnoreCase))
                    {
                        fileLines[i] = FixLineToHex(fileLines[i], out exactHexValueTrimmed);
                    }
                    foreach (CompactedModData modData in CompactedModDataD.Values)
                    {
                        if (fileLines[i].Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (fileLines[i].Contains("GetModByName", StringComparison.OrdinalIgnoreCase))
                            {
                                fileLines[i] = fileLines[i].Replace(modData.ModName, modData.CompactedModFormList.First().ModName);
                            }
                            else
                            {
                                foreach (FormHandler form in modData.CompactedModFormList)
                                {
                                    if (exactHexValueTrimmed != null)
                                    {
                                        if (exactHexValueTrimmed.Equals(form.GetOriginalFormID(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                            fileLines[i] = fileLines[i].Replace(form.GetOriginalFormID(), form.GetCompactedFormID(), StringComparison.OrdinalIgnoreCase);
                                            fileLines[i] = fileLines[i].Replace(modData.ModName, form.ModName, StringComparison.OrdinalIgnoreCase);
                                            GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                            changed = true;
                                        }
                                    }
                                    else if (fileLines[i].Contains(form.GetOriginalFormID(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                        fileLines[i] = fileLines[i].Replace(form.GetOriginalFormID(), form.GetCompactedFormID(), StringComparison.OrdinalIgnoreCase);
                                        fileLines[i] = fileLines[i].Replace(modData.ModName, form.ModName, StringComparison.OrdinalIgnoreCase);
                                        GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                        changed = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return fileLines;
        }
    }
}
