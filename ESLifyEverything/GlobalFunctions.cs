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
        public static AppSettings Settings = new AppSettings();
        public static StringResources stringsResources = new StringResources();
        public static StringLoggingData stringLoggingData = new StringLoggingData();
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
        public static string FaceGenFileFixPath = "";
        public static StringBuilder? AsyncConsoleStringBuilder;
        public static StringBuilder? AsyncFileStringBuilder;

        public static bool Startup(out int startupError, string ProgramLogName)
        {
            File.Create(ProgramLogName).Close();

            startupError = 0;

            if (!File.Exists("AppSettings.json"))
            {
                IConfiguration stringResorsConfig = new ConfigurationBuilder().AddJsonFile(".\\Properties\\StringResources.json").AddEnvironmentVariables().Build();
                stringLoggingData = stringResorsConfig.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();
                return false;
            }

            
            bool startUp = true;
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("AppSettings.json").AddJsonFile(".\\Properties\\StringResources.json").AddEnvironmentVariables().Build();
            Settings = config.GetRequiredSection("Settings").Get<AppSettings>();
            stringsResources = config.GetRequiredSection("StringResources").Get<StringResources>();
            stringLoggingData = config.GetRequiredSection("StringLoggingData").Get<StringLoggingData>();

            if(GF.Settings.AutoReadAllxEditSeesion == false)
            {
                GF.Settings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion = false;
            }

            if (!Directory.Exists(GF.Settings.SkyrimDataFolderPath))
            {
                GF.WriteLine(GF.stringLoggingData.DataFolderNotFound);
                startUp = false;
            }
            else if(!Directory.Exists(Path.Combine(GF.Settings.SkyrimDataFolderPath, "CompactedForms")))
            {
                Directory.CreateDirectory(Path.Combine(GF.Settings.SkyrimDataFolderPath, "CompactedForms"));
            }

            if (!Directory.Exists(GF.Settings.XEditFolderPath))
            {
                GF.WriteLine(GF.stringLoggingData.XEditFolderNotFound);
                startUp = false;
                if (!File.GetAttributes(GF.Settings.XEditFolderPath).HasFlag(FileAttributes.Directory))
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
                startupError = 1;
            }

            if (!startUp)
            {
                return startUp;
            }

            if (GF.Settings.OutputToOptionalFolder)
            {
                GF.WriteLine(GF.stringLoggingData.OutputFolderWarning, true, false);
                GF.WriteLine(GF.stringLoggingData.MOOutputFolderWarning, true, false);
                if (!Directory.Exists(GF.Settings.OptionalOutputFolder))
                {
                    GF.WriteLine(GF.stringLoggingData.OutputFolderNotFound);
                    GF.WriteLine(GF.stringLoggingData.OutputFolderDefaultWarning);
                    GF.Settings.OptionalOutputFolder = GF.Settings.SkyrimDataFolderPath;
                }
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.OutputFolderDefaultWarning);
                GF.Settings.OptionalOutputFolder = GF.Settings.SkyrimDataFolderPath;
            }

            if (!Directory.Exists(Path.Combine(GF.Settings.OptionalOutputFolder, "CompactedForms")))
            {
                Directory.CreateDirectory(Path.Combine(GF.Settings.OptionalOutputFolder, "CompactedForms"));
            }

            return startUp;
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

        internal static void GenerateSettingsFileError()
        {
            GF.WriteLine(GF.stringLoggingData.SettingsFileNotFound);
            GF.WriteLine(GF.stringLoggingData.GenSettingsFile);
            GF.WriteLine(GF.stringLoggingData.EditYourSettings);
            File.WriteAllText("AppSettings.json", JsonSerializer.Serialize(new AppSettings(), GF.JsonSerializerOptions));
        }

        //origonalPath = Origonal path with replaced origonal FormID if it contains it
        public static string FixOuputPath(string origonalPath)
        {
            string newPath = origonalPath.Replace(GF.Settings.SkyrimDataFolderPath, "");
            if (newPath[0] == '\\')
            {
                newPath = newPath.Substring(1);
            }
            return Path.Combine(GF.Settings.OptionalOutputFolder, newPath);
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
    }
}
