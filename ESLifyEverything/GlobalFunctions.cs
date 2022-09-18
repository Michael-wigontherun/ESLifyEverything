using ESLifyEverything.FormData;
using ESLifyEverything.Properties;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESLifyEverything
{
    public static class GF
    {
        public static readonly string SettingsVersion = "1.9";
        public static readonly string ExtractedBSAModDataPath = ".\\ExtractedBSAModData";
        public static readonly string ChangedScriptsPath = ".\\ChangedScripts";
        public static readonly string SourceSubPath = "Source\\Scripts";

        public static AppSettings Settings = new AppSettings();
        public static StringResources stringsResources = new StringResources();
        public static StringLoggingData stringLoggingData = new StringLoggingData();
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
        public static string[] DefaultScriptBSAs = new string[0];

        public static string FaceGenFileFixPath = "";

        public static List<string> BSALoadOrder = new List<string>();

        public static bool Startup(out int startupError, string ProgramLogName)
        {
            File.Create(ProgramLogName).Close();
            startupError = 0;

            if (!File.Exists("AppSettings.json"))
            {
                startupError = 1;
                IConfiguration stringResorsConfig = new ConfigurationBuilder().AddJsonFile(".\\Properties\\StringResources.json").AddEnvironmentVariables().Build();
                stringLoggingData = stringResorsConfig.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();
                return false;
            }
            

            bool startUp = true;
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddJsonFile(".\\Properties\\StringResources.json")
                .AddJsonFile(".\\Properties\\DefaultBSAs.json")
                .AddEnvironmentVariables().Build();
            try
            {
                string version = config.GetRequiredSection("SettingsVersion").Get<string>();
                if (!version.Equals(GF.SettingsVersion))
                {
                    startupError = 1;
                    stringLoggingData = config.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();
                    return false;
                }
            }
            catch (Exception)
            {
                startupError = 1;
                stringLoggingData = config.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();
                return false;
            }
            
            Settings = config.GetRequiredSection("Settings").Get<AppSettings>();
            stringsResources = config.GetRequiredSection("StringResources").Get<StringResources>();
            stringLoggingData = config.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();
            DefaultScriptBSAs = config.GetRequiredSection("DefaultScriptBSAs").Get<string[]>();

            if (GF.Settings.AutoReadAllxEditSeesion == false)
            {
                GF.Settings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion = false;
            }

            if (!Directory.Exists(GF.Settings.DataFolderPath))
            {
                GF.WriteLine(GF.stringLoggingData.DataFolderNotFound);
                startUp = false;
            }
            else if(!Directory.Exists(Path.Combine(GF.Settings.DataFolderPath, "CompactedForms")))
            {
                Directory.CreateDirectory(Path.Combine(GF.Settings.DataFolderPath, "CompactedForms"));
            }
            
            
            if (!Directory.Exists(GF.Settings.XEditFolderPath))
            {
                GF.WriteLine(GF.stringLoggingData.XEditFolderNotFound);
                startUp = false;
                if (File.Exists(GF.Settings.XEditFolderPath))
                {
                    GF.WriteLine(GF.stringLoggingData.XEditFolderSetToFile);
                }
            }
            else if (!File.Exists(Path.Combine(GF.Settings.XEditFolderPath, "SSEEdit.exe")))
            {
                GF.WriteLine(GF.stringLoggingData.IntendedForSSE);
            }

            if (Directory.Exists(GF.Settings.XEditFolderPath))
            {
                FaceGenFileFixPath = Path.Combine(GF.Settings.XEditFolderPath, "FaceGenEslIfyFix.txt");
                File.Create(FaceGenFileFixPath).Close();
            }

            if (!File.Exists(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName)))
            {
                GF.WriteLine(GF.stringLoggingData.XEditLogNotFound);
                startupError = 2;
            }
            if (!File.Exists(Path.Combine(GF.GetSkyrimRootFolder(), "Papyrus Compiler\\PapyrusCompiler.exe")))
            {
                startUp = false;
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing);
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing2);
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing3);
                GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing4);
            }
            else
            {
                if(File.Exists(Path.Combine(GF.Settings.DataFolderPath, $"{GF.SourceSubPath}\\{GF.Settings.PapyrusFlag}")))
                {
                    Directory.CreateDirectory(Path.Combine(GF.ExtractedBSAModDataPath, $"{GF.SourceSubPath}"));
                    File.Copy(Path.Combine(GF.Settings.DataFolderPath, $"{GF.SourceSubPath}\\{GF.Settings.PapyrusFlag}"), 
                        Path.Combine(GF.ExtractedBSAModDataPath, $"{GF.SourceSubPath}\\{GF.Settings.PapyrusFlag}"), true);
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.PapyrusFlagFileMissing);
                    GF.WriteLine(String.Format(GF.stringLoggingData.PapyrusFlagFileMissing2, GF.Settings.PapyrusFlag));
                    GF.WriteLine(GF.stringLoggingData.PapyrusCompilerMissing4);
                }
            }

            if (!Directory.Exists(GF.Settings.OutputFolder))
            {
                startUp = false;
                GF.WriteLine(GF.stringLoggingData.OutputFolderNotFound);
                GF.WriteLine(String.Format(GF.stringLoggingData.OutputFolderIsRequired, GF.stringLoggingData.PotectOrigonalScripts));
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.OutputFolderWarning, true, false);
            }

            if (Directory.Exists(Path.Combine(GF.Settings.OutputFolder, "scripts")))
            {
                IEnumerable<string> scripts = Directory.EnumerateFiles(
                    Path.Combine(GF.Settings.OutputFolder, "scripts"),
                    "*.pex",
                    SearchOption.TopDirectoryOnly);
                if (scripts.Any())
                {
                    startUp = false;
                    GF.WriteLine(String.Format(GF.stringLoggingData.ClearYourOutputFolderScripts, GF.stringLoggingData.PotectOrigonalScripts));
                }
            }

            if (!startUp)
            {
                return startUp;
            }

            Directory.CreateDirectory(GF.ExtractedBSAModDataPath);
            Directory.CreateDirectory(GF.ChangedScriptsPath);
            GF.ClearChangedScripts();
            Directory.CreateDirectory(Path.Combine(GF.Settings.OutputFolder, "CompactedForms"));

            

            return startUp;
        }

        private static void ClearChangedScripts()
        {
            IEnumerable<string> changedSouce = Directory.EnumerateFiles(
                    GF.ChangedScriptsPath,
                    "*.psc",
                    SearchOption.TopDirectoryOnly);
            if (changedSouce.Any())
            {
                foreach(string script in changedSouce)
                {
                    File.Delete(script);
                }
            }
        }

        public static void WriteLine(string logLine, bool consoleLog = true, bool fileLogging = true)
        {
            if (consoleLog)
            {
                Console.WriteLine(logLine);
            }
            if (fileLogging)
            {
                using (StreamWriter stream = File.AppendText("ESLifyEverything_Log.txt"))
                {
                    stream.WriteLine(logLine);
                }
            }
        }

        public static void WriteLine(List<FormHandler> logData, bool consoleLog = true, bool fileLogging = true)
        {
            if (consoleLog)
            {
                foreach (FormHandler item in logData)
                {
                    Console.WriteLine(item!.ToString());
                }
            }
            if (fileLogging)
            {
                using (StreamWriter stream = File.AppendText("ESLifyEverything_Log.txt"))
                {
                    foreach (FormHandler item in logData)
                    {
                        stream.WriteLine(item!.ToString());
                    }
                }
            }
        }

        //true = exit while
        //-1 = return exit code in selectedMenuItem
        public static bool WhileMenuSelect(int menuMaxNum, out int selectedMenuItem, int MenuMinNum = 0)
        {
            string input = Console.ReadLine() ?? "";
            if (input.Equals("XXX", StringComparison.OrdinalIgnoreCase))
            {
                selectedMenuItem = -1;
                return true;
            }

            if (Int32.TryParse(input, out selectedMenuItem))
            {
                if (selectedMenuItem >= MenuMinNum && selectedMenuItem <= menuMaxNum)
                {
                    return true;
                }
            }

            return false;
        }

        public static void GenerateSettingsFileError()
        {
            GF.WriteLine(GF.stringLoggingData.SettingsFileNotFound);
            GF.WriteLine(GF.stringLoggingData.GenSettingsFile);
            GF.WriteLine(GF.stringLoggingData.EditYourSettings);
            File.WriteAllText("AppSettings.json", JsonSerializer.Serialize(new GeneratedAppSettings(), GF.JsonSerializerOptions));
        }

        //origonalPath = Origonal path with replaced origonal FormID if it contains it
        public static string FixOuputPath(string origonalPath)
        {
            string newPath = origonalPath.Replace(GF.Settings.DataFolderPath, "");
            if (newPath[0] == '\\')
            {
                newPath = newPath.Substring(1);
            }
            return Path.Combine(GF.Settings.OutputFolder, newPath);
        }

        public static string FixOuputPath(string origonalPath, string origonalDataStartPath, string newStartPath)
        {
            string newPath = origonalPath.Replace(origonalDataStartPath, "");
            if (newPath[0] == '\\')
            {
                newPath = newPath.Substring(1);
            }
            return Path.Combine(newStartPath, newPath);
        }

        public static void OuputDataFileToOutputFolder(bool changed, string origonalFilePath, string[] newFileLinesArr, string unchangedLogLine)
        {
            if (changed == true)
            {
                string newPath = GF.FixOuputPath(origonalFilePath);
                Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                File.WriteAllLines(newPath, newFileLinesArr);
            }
            else
            {
                GF.WriteLine(unchangedLogLine + origonalFilePath, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
            }
        }

        public static string GetSkyrimRootFolder()
        {
            return Path.GetFullPath(GF.Settings.DataFolderPath).Replace("\\Data", "");
        }
    }
}
