using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverything
{
    public static partial class Program
    {

        public static bool ExtractScriptsMenu()
        {
            if (GF.Settings.AutoRunScriptDecompile)
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

        public static async Task<int> ExtractScripts()
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

                GF.WriteLine(GF.stringLoggingData.RunningChampBSA);
                Task BSAChamp = DecompileScripts(GF.ExtractedBSAModDataPath);
                BSAChamp.Wait();
                BSAChamp.Dispose();
                GF.WriteLine(GF.stringLoggingData.EndedChampBSA);

                GF.WriteLine(GF.stringLoggingData.RunningChampLoose);
                Task SourceChamp = DecompileScripts(GF.Settings.DataFolderPath);
                SourceChamp.Wait();
                SourceChamp.Dispose();
                GF.WriteLine(GF.stringLoggingData.EndedChampLoose);

                Console.WriteLine(GF.stringLoggingData.IgnoreAbove);
            }


            Task inmScripts = CompileScripts();
            inmScripts.Wait();
            inmScripts.Dispose();

            return await Task.FromResult(0);
        }

        public static async Task<int> ExtractBSAScripts(string bsaPath)
        {
            Process p = new Process();
            p.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
            p.StartInfo.Arguments = $"\"{Path.GetFullPath(bsaPath)}\" -f \".pex\"  -e -o \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\"";
            p.Start();
            p.WaitForExit();
            p.Dispose();

            return await Task.FromResult(0);
        }

        public static async Task<int> DecompileScripts(string startPath)
        {
            Process p = new Process();
            p.StartInfo.FileName = ".\\Champollion\\champollion.exe";
            p.StartInfo.Arguments = $"\"{Path.GetFullPath(startPath)}\\Scripts\" -p \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\\{GF.SourceSubPath}\" -t";
            p.StartInfo.UseShellExecute = true;
            p.Start();
            p.WaitForExit();
            p.Dispose();

            return await Task.FromResult(0);
        }

        public static async Task<int> CompileScripts()
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
                string[] fileLines = FormInFileReader(File.ReadAllLines(script), out changed);
                if (changed)
                {
                    GF.WriteLine(GF.stringLoggingData.ScriptSourceFileChanged + script, false, GF.Settings.VerboseFileLoging);
                    string newFilePath = GF.FixOuputPath(script, startFolder, GF.ChangedScriptsPath);
                    //File.WriteAllLines(script, fileLines);
                    //changedFiles.Add(script);
                    File.WriteAllLines(newFilePath, fileLines);
                    changedFiles.Add(newFilePath);
                }
            }
            GF.Settings.VerboseConsoleLoging = tempLogging;

            if (File.Exists(Path.Combine(GF.GetSkyrimRootFolder(), "Papyrus Compiler\\PapyrusCompiler.exe")))
            {
                Console.WriteLine();
                Console.WriteLine(GF.stringLoggingData.ImportantBelow);
                Console.WriteLine(GF.stringLoggingData.ScriptFailedCompilation3);
                Console.WriteLine();
                Console.WriteLine(GF.stringLoggingData.ScriptESLifyINeedThisDataToBeReported);
                Console.WriteLine();
                Console.WriteLine(GF.stringLoggingData.ImportantBelow1);
                Console.WriteLine();
                Process p = new Process();
                p.StartInfo.FileName = Path.Combine(GF.GetSkyrimRootFolder(), "Papyrus Compiler\\PapyrusCompiler.exe");
                p.StartInfo.Arguments = $"\"{Path.GetFullPath(GF.ChangedScriptsPath)}\" -q -f=\"TESV_Papyrus_Flags.flg\" -a -i=\"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\\{GF.SourceSubPath}\" -o=\"{Path.Combine(Path.GetFullPath(GF.Settings.OutputFolder), "Scripts")}\"";
                p.Start();
                p.WaitForExit();
                p.Dispose();
                Console.WriteLine();
                Console.WriteLine(GF.stringLoggingData.ImportantAbove);

                foreach (var changedFile in changedFiles)
                {
                    ScriptCompiled(Path.ChangeExtension(Path.GetFileName(changedFile), null));
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

        public static void ScriptCompiled(string sourceFileNameNoExtention)
        {
            if (!File.Exists(Path.Combine(GF.Settings.OutputFolder, "Scripts\\" + sourceFileNameNoExtention + ".pex")))
            {
                FailedToCompile.Add(sourceFileNameNoExtention);
            }
        }
    }
}
