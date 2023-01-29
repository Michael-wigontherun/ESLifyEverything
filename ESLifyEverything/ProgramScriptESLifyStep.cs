using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ESLifyEverything.FormData;
using ESLifyEverythingGlobalDataLibrary;
using Noggog;

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
                ClearChangedScripts();

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

                    Console.WriteLine();
                    GF.WriteLine(GF.stringLoggingData.RunningChampFailsafe);
                    Console.WriteLine();

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
                    SourceChamp.Dispose();

                    Console.WriteLine();
                    GF.WriteLine(GF.stringLoggingData.RunningChampFailsafe);
                    Console.WriteLine();

                    Task<int> SourceChampSlow = DecompileScriptsSlow(GF.Settings.DataFolderPath);
                    SourceChampSlow.Wait();
                    SourceChampSlow.Dispose();
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
                        stream.WriteLine("Champollion Error: " + line);
                        Console.WriteLine("Champollion Error: " + line);
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
                GF.WriteLine(GF.stringLoggingData.ChampCrash);
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
            long scriptTotal = scripts.Count();
            long scriptCounter = 1;

            Console.WriteLine(string.Format("{0} of {1} finished decompiling.", scriptCounter, scriptTotal));

            using (StreamWriter stream = File.AppendText(GF.logName))
            {
                foreach (string script in scripts)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = ".\\Champollion\\champollion.exe";
                    p.StartInfo.Arguments = $"\"{Path.GetFullPath(script)}\" -p \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\\{GF.SourceSubPath}\" -t";//files processed
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
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

                    while (!p.StandardError.EndOfStream)
                    {
                        line = p.StandardError.ReadLine()!;
                        stream.WriteLine("Champollion Error: " + line);
                        Console.WriteLine("Champollion Error: " + line);
                    }

                    p.WaitForExit();
                    p.Dispose();

                    bool errorLineWrite = false;
                    if (!line.Contains("1 files processed in "))
                    {
                        Console.WriteLine(script);
                        stream.WriteLine(script);
                        Console.WriteLine(line);
                        stream.WriteLine(line);
                        
                        errorLineWrite = true;
                    }

                    if (errorLineWrite)
                    {
                        Console.WriteLine(string.Format("{0} of {1} finished decompiling.", scriptCounter, scriptTotal));
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.WriteLine(string.Format("{0} of {1} finished decompiling.", scriptCounter, scriptTotal));
                    }
                    scriptCounter++;

                }
            }

            Console.WriteLine("Ended one by one decompile.");
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
                string[] fileLines = File.ReadAllLines(script);
                fileLines = FormInScriptFileLineReader(fileLines, out bool changed, out bool changedImport);

                if (changedImport || changed)
                {
                    File.WriteAllLines(script, fileLines);
                }
                if (changed)
                {
                    GF.WriteLine(GF.stringLoggingData.ScriptSourceFileChanged + script, false, GF.Settings.VerboseFileLoging);
                    string newFilePath = GF.FixOuputPath(script, startFolder, GF.ChangedScriptsPath);
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
                        DevLog.Pause("Before Compiler Start");

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
                        p.StartInfo.Arguments = $"\"{Path.GetFullPath(GF.ChangedScriptsPath)}\" -q -f=\"{GF.Settings.PapyrusFlag}\" -a -i=\"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\\{GF.SourceSubPath}\" -o=\"{Path.Combine(Path.GetFullPath(GF.Settings.OutputFolder), "Scripts")}\"";
                        
                        GF.WriteLine("PapyrusCompiler.exe " + p.StartInfo.Arguments);
                        Console.WriteLine();

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

                            while (!p.StandardError.EndOfStream)
                            {
                                string line = p.StandardError.ReadLine()!;
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

                        foreach (var changedFile in changedFiles)
                        {
                            if (!File.Exists(Path.Combine(GF.Settings.OutputFolder, "Scripts\\" + Path.ChangeExtension(Path.GetFileName(changedFile), null) + ".pex")))
                            {
                                Console.WriteLine(GF.stringLoggingData.FailedToCompileCompiler + Path.GetFileName(changedFile));
                                FailedToCompile.Add(Path.GetFileName(changedFile));
                            }
                        }

                        if (FailedToCompile.Any())
                        {
                            CompileOneByOne();
                        }

                        GF.WriteLine(GF.stringLoggingData.ImportantAbove);
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
        
        public static void CompileOneByOne()
        {
            GF.WriteLine(GF.stringLoggingData.AttemptOneByOneCompiler);
            foreach(string file in FailedToCompile)
            {
                Process p = new Process();
                p.StartInfo.FileName = Path.Combine(GF.GetSkyrimRootFolder(), "Papyrus Compiler\\PapyrusCompiler.exe");
                p.StartInfo.Arguments = $"\"{Path.Combine(Path.GetFullPath(GF.ChangedScriptsPath), file)}\" -q -f=\"{GF.Settings.PapyrusFlag}\" -i=\"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\\{GF.SourceSubPath}\" -o=\"{Path.Combine(Path.GetFullPath(GF.Settings.OutputFolder), "Scripts")}\"";
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

                    while (!p.StandardError.EndOfStream)
                    {
                        string line = p.StandardError.ReadLine()!;
                        Console.WriteLine(line);
                        if (!line.Equals(string.Empty))
                        {
                            stream.WriteLine(line);
                        }
                    }
                }
                p.WaitForExit();
                p.Dispose();
                if (File.Exists(Path.Combine(GF.Settings.OutputFolder, "Scripts\\" + Path.ChangeExtension(file, null) + ".pex")))
                {
                    GF.WriteLine(GF.stringLoggingData.CompiledSuccessfully);
                    FailedToCompile.Remove(file);
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.CompiledUnsuccessfully);
                }
            }
        }

        //Clears the ChangedScripts folder from previous ESLify Everything session
        private static void ClearChangedScripts()
        {
            IEnumerable<string> changedSouce = Directory.EnumerateFiles(
                    GF.ChangedScriptsPath,
                    "*.psc",
                    SearchOption.TopDirectoryOnly);
            if (changedSouce.Any())
            {
                foreach (string script in changedSouce)
                {
                    File.Delete(script);
                }
            }
        }

        enum ValueTypes
        {
            None,
            Int,
            Bool,
            Float,
            String
        }

        public static string SetDefaultValue(string line, out bool changedImport)
        {
            changedImport = false;
            if (line.Contains(" function ", StringComparison.OrdinalIgnoreCase)) return line;
            if (line.IsNullOrEmpty()) return line;
            if (line.IsNullOrWhitespace()) return line;
            if (line.Contains('=', StringComparison.OrdinalIgnoreCase)) return line;

            ValueTypes valueType = ValueTypes.None;

            if (line.IndexOf("int ", StringComparison.OrdinalIgnoreCase) == 0) valueType = ValueTypes.Int;
            else if (line.IndexOf("String ", StringComparison.OrdinalIgnoreCase) == 0) valueType = ValueTypes.String;
            else if (line.IndexOf("Bool ", StringComparison.OrdinalIgnoreCase) == 0) valueType = ValueTypes.Bool;
            else if (line.IndexOf("Float ", StringComparison.OrdinalIgnoreCase) == 0) valueType = ValueTypes.Float;

            if (valueType != ValueTypes.None)
            {
                string[] words = line.Split(' ');
                int vNameIndex = 1;

                if (words[vNameIndex].Equals("property", StringComparison.OrdinalIgnoreCase)) vNameIndex = 2;

                StringBuilder sb = new StringBuilder();
                for (int a = 0; a < words.Length; a++)
                {
                    sb.Append(words[a]);

                    if(a == vNameIndex)
                    {
                        if(valueType == ValueTypes.Int) sb.Append(" = 0 ");
                        else if (valueType == ValueTypes.String) sb.Append(" = \"\" ");
                        else if (valueType == ValueTypes.Bool) sb.Append(" = false ");
                        else if (valueType == ValueTypes.Float) sb.Append(" = 0.0 ");
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                line = sb.ToString();
                changedImport = true;
            }

            return line;
        }

        //Parses Script files
        public static string[] FormInScriptFileLineReader(string[] fileLines, out bool changed, out bool changedImport)
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
                            }
                            exactHexValueTrimmed = hexNum;
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
            changedImport = false;
            bool StartCheck = false;
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(";-- Properties", StringComparison.OrdinalIgnoreCase)) StartCheck = true;
                if (fileLines[i].Contains(";-- Variables", StringComparison.OrdinalIgnoreCase)) StartCheck = true;
                if (fileLines[i].Contains(";-- Functions", StringComparison.OrdinalIgnoreCase)) StartCheck = false;
                if (StartCheck)
                {
                    if (i != (fileLines.Length - 1) && !fileLines[i + 1].Contains(" function ", StringComparison.OrdinalIgnoreCase))
                    {
                        fileLines[i] = SetDefaultValue(fileLines[i], out bool changedImportValue);
                        if (changedImportValue) changedImport = true;
                    }
                }

                if (fileLines[i].Contains(".esp", StringComparison.OrdinalIgnoreCase) || fileLines[i].Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    string? exactHexValueTrimmed = null;
                    if (fileLines[i].Contains("GetFormFromFile(", StringComparison.OrdinalIgnoreCase) && !fileLines[i].Contains("0x", StringComparison.OrdinalIgnoreCase))
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
