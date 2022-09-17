using ESLifyEverything.FormData;
using ESLifyEverything.XEdit;
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

        #region xEdit Log
        public static void XEditSession()
        {
            XEditLogReader.ReadLog(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName));
            int xEditSessionsCount = XEditLogReader.xEditLog.xEditSessions?.Length ?? 0;
            if (xEditSessionsCount <= 0)
            {
                GF.WriteLine(GF.stringLoggingData.NoxEditSessions);
            }
            else if (GF.Settings.AutoReadAllxEditSeesion)
            {
                string fileName = Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName);
                FileInfo fi = new FileInfo(fileName);
                if (fi.Length > 10000000)//Path.Combine(GF.Settings.XEditFilePath, GF.Settings.XEditLogFileName)
                {
                    GF.WriteLine(GF.stringLoggingData.XEditLogFileSizeWarning);
                }
                XEditSessionAutoAll();
                if (GF.Settings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion)
                {
                    File.Delete(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName));
                }
            }
            else if (GF.Settings.AutoReadNewestxEditSeesion)
            {
                XEditLogReader.xEditLog.xEditSessions![xEditSessionsCount - 1].GenerateCompactedModDatas();
            }
            else
            {
                XEditSessionMenu();
            }
        }

        public static void XEditSessionMenu()
        {
            int xEditSessionsCount = XEditLogReader.xEditLog.xEditSessions?.Length ?? 0;
            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(GF.stringLoggingData.SelectSession,true,false);
            for (int i = 0; i < xEditSessionsCount; i++)
            {
                GF.WriteLine($"{i}. " + XEditLogReader.xEditLog.xEditSessions![i].SessionTimeStamp);
            }
            GF.WriteLine(GF.stringLoggingData.InputSessionPromt, true, false);
            GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
            int selectedMenuItem;
            while (GF.WhileMenuSelect(xEditSessionsCount - 1, out selectedMenuItem) == false) ;
            if (selectedMenuItem != -1) XEditLogReader.xEditLog.xEditSessions![selectedMenuItem].GenerateCompactedModDatas();
        }

        public static void XEditSessionAutoAll()
        {
            foreach (XEditSession session in XEditLogReader.xEditLog.xEditSessions!)
            {
                session.GenerateCompactedModDatas();
            }
        }

        #endregion xEdit Log

        public static void ImportModData(string compactedFormsLocation)
        {
            IEnumerable<string> compactedFormsModFiles = Directory.EnumerateFiles(
                compactedFormsLocation,
                "*_ESlEverything.json",
                SearchOption.AllDirectories);
            foreach (string compactedFormsModFile in compactedFormsModFiles)
            {
                CompactedModData mod = new CompactedModData();
                //mod.GetModData(Path.GetFileName(compactedFormsModFile).Replace("_ESlEverything.json", "").Replace("_ESlEverything", ""));
                mod.GetModData(compactedFormsModFile);
                mod.Write();
                CompactedModDataD.TryAdd(mod.ModName, mod);
            }
        }

        #region Plugin Specific BSA Extract
        public static async Task<int> LoadOrderBSAExtract()
        {
            //string loadorderFilePath = "loadorder.txt";
            string loadorderFilePath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData")!, "Skyrim Special Edition", "loadorder.txt");
            if (File.Exists(loadorderFilePath))
            {
                string[] loadorder = File.ReadAllLines(loadorderFilePath);

                foreach (string plugin in loadorder)
                {
                    string pluginNoExtension = Path.ChangeExtension(plugin, null);
                    if (File.Exists(Path.Combine(GF.Settings.SkyrimDataFolderPath, pluginNoExtension + ".bsa")))
                    {
                        LoadOrderNoExtensions.Add(pluginNoExtension);
                    }
                }
                int loadorderCount = LoadOrderNoExtensions.Count;
                for (int i = 0; i < loadorderCount; i++)
                {
                    if (File.Exists(Path.Combine(GF.Settings.SkyrimDataFolderPath, LoadOrderNoExtensions[i] + ".bsa")))
                    {
                        Task BSAmesh = ExtractBSAModData(Path.Combine(GF.Settings.SkyrimDataFolderPath, LoadOrderNoExtensions[i] + ".bsa"));
                        BSAmesh.Wait();
                        BSAmesh.Dispose();
                        if (File.Exists(Path.Combine(GF.Settings.SkyrimDataFolderPath, LoadOrderNoExtensions[i] + " - Textures.bsa")))
                        {
                            Task BSATex = ExtractBSAModData(Path.Combine(GF.Settings.SkyrimDataFolderPath, LoadOrderNoExtensions[i] + " - Textures.bsa"));
                            BSATex.Wait();
                            BSATex.Dispose();
                        }
                    }
                    Console.WriteLine();
                    GF.WriteLine(String.Format(GF.stringLoggingData.ProcessedBSAsLogCount, i + 1, loadorderCount));
                }
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.LoadOrderNotDetectedError);
                GF.WriteLine(GF.stringLoggingData.RunOrReport);
                BSAExtracted = false;
            }
            return await Task.FromResult(0);
        }

        public static async Task<int> ExtractBSAModData(string potentialBSAPath)
        {
            foreach (CompactedModData modData in CompactedModDataD.Values)
            {
                Process p = new Process();
                p.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
                p.StartInfo.Arguments = $"\"{potentialBSAPath}\" -f \"{modData.ModName}\"  -e -o \"{GF.ExtractedBSAModDataPath}\"";
                p.Start();
                p.WaitForExit();
            }
            return await Task.FromResult(0);
        }
        #endregion Plugin Specific BSA Extract

        #region Voice Eslify

        public static void VoiceESlIfyMenu()
        {
            GF.WriteLine(GF.stringLoggingData.VoiceESLMenuHeader);
            GF.WriteLine($"1. {GF.stringLoggingData.ESLEveryMod}{GF.stringLoggingData.WithCompactedForms}", true, false);//with a _ESlEverything.json attached to it inside of the CompactedForms folders.
            GF.WriteLine($"2. {GF.stringLoggingData.SingleInputMod}{GF.stringLoggingData.WithCompactedForms}", true, false);
            GF.WriteLine(GF.stringLoggingData.CanLoop, true, false);
            GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
            bool whileContinue = true;
            do
            {
                GF.WhileMenuSelect(2, out int selectedMenuItem, 1);
                switch (selectedMenuItem)
                {
                    case -1:
                        GF.WriteLine(GF.stringLoggingData.ExitCodeInputOutput);
                        whileContinue = false;
                        break;
                    case 1:
                        GF.WriteLine(GF.stringLoggingData.EslifingEverything);
                        whileContinue = VoiceESLifyEverything();
                        break;
                    case 2:
                        GF.WriteLine(GF.stringLoggingData.EslifingSingleMod);
                        VoiceESLifySingleMod();
                        break;
                }
            } while (whileContinue == true);
        }

        public static bool VoiceESLifyEverything()
        {
            foreach (CompactedModData modData in CompactedModDataD.Values)
            {
                VoiceESLifyMod(modData);
            }
            return false;
        }

        public static void VoiceESLifySingleMod()
        {
            bool whileContinue = true;
            string input;
            CompactedModData modData;
            do
            {
                GF.WriteLine($"{GF.stringLoggingData.SingleModInputHeader}{GF.stringLoggingData.ExamplePlugin}");
                GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
                input = Console.ReadLine() ?? "";
                if (CompactedModDataD.TryGetValue(input, out modData!))
                {
                    modData.Write();
                    VoiceESLifyMod(modData);
                }
                else if (input.Equals("XXX", StringComparison.OrdinalIgnoreCase))
                {
                    whileContinue = false;
                }
            } while (whileContinue == true);
        }

        public static void VoiceESLifyMod(CompactedModData modData)
        {
            VoiceESLifyModData(modData, GF.ExtractedBSAModDataPath);
            VoiceESLifyModData(modData, GF.Settings.SkyrimDataFolderPath);
        }

        public static void VoiceESLifyModData(CompactedModData modData, string dataStartPath)
        {
            if (Directory.Exists(Path.Combine(dataStartPath, "sound\\voice", modData.ModName)))
            {
                foreach (FormHandler form in modData.CompactedModFormList)
                {
                    IEnumerable<string> voiceFilePaths = Directory.EnumerateFiles(
                        Path.Combine(dataStartPath, "sound\\voice", modData.ModName),
                        "*" + form.OrigonalFormID + "*",
                        SearchOption.AllDirectories);
                    foreach (string voiceFilePath in voiceFilePaths)
                    {
                        CopyFormFile(form, dataStartPath, voiceFilePath, out string filePath);
                    }
                }
            }
        }
        #endregion Voice Eslify

        #region FaceGen Eslify
        public static void FaceGenESlIfyMenu()
        {
            GF.WriteLine(GF.stringLoggingData.FaceGenESLMenuHeader);
            GF.WriteLine($"1. {GF.stringLoggingData.ESLEveryMod}{GF.stringLoggingData.WithCompactedForms}",true, false);//with a _ESlEverything.json attached to it inside of the CompactedForms folders.
            GF.WriteLine($"2. {GF.stringLoggingData.SingleInputMod}{GF.stringLoggingData.WithCompactedForms}", true, false);
            GF.WriteLine(GF.stringLoggingData.CanLoop, true, false);
            GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
            bool whileContinue = true;
            do
            {
                GF.WhileMenuSelect(2, out int selectedMenuItem, 1);
                switch (selectedMenuItem)
                {
                    case -1:
                        GF.WriteLine(GF.stringLoggingData.ExitCodeInputOutput);
                        whileContinue = false;
                        break;
                    case 1:
                        GF.WriteLine(GF.stringLoggingData.EslifingEverything);
                        whileContinue = FaceGenESLifyEverything();
                        break;
                    case 2:
                        GF.WriteLine(GF.stringLoggingData.EslifingSingleMod);
                        FaceGenESLifySingleMod();
                        break;
                }
            } while (whileContinue == true);
        }

        public static bool FaceGenESLifyEverything()
        {
            foreach (CompactedModData modData in CompactedModDataD.Values)
            {
                FaceGenESLifyMod(modData);
            }
            return false;
        }

        public static void FaceGenESLifySingleMod()
        {
            bool whileContinue = true;
            string input;
            CompactedModData modData;
            do
            {
                GF.WriteLine(GF.stringLoggingData.SingleModInputHeader + GF.stringLoggingData.ExamplePlugin, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
                input = Console.ReadLine() ?? "";
                if (CompactedModDataD.TryGetValue(input, out modData!))
                {
                    modData.Write();
                    FaceGenESLifyMod(modData);
                }
                else if (input.Equals("XXX", StringComparison.OrdinalIgnoreCase))
                {
                    whileContinue = false;
                }
            } while (whileContinue == true);
        }
        
        public static void FaceGenESLifyMod(CompactedModData modData)
        {
            FaceGenEslifyModData(modData, GF.ExtractedBSAModDataPath);
            FaceGenEslifyModData(modData, GF.Settings.SkyrimDataFolderPath);
            
        }

        public static void FaceGenEslifyModData(CompactedModData modData, string dataStartPath)
        {
            if (Directory.Exists(Path.Combine(dataStartPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom\\", modData.ModName)))
            {
                foreach (FormHandler form in modData.CompactedModFormList)
                {
                    IEnumerable<string> FaceGenTexFilePaths = Directory.EnumerateFiles(
                        Path.Combine(dataStartPath, "Textures\\Actors\\Character\\FaceGenData\\FaceTint\\", modData.ModName),
                        "*" + form.OrigonalFormID + "*",
                        SearchOption.AllDirectories);
                    foreach (string FaceGenFilePath in FaceGenTexFilePaths)
                    {
                        CopyFormFile(form, dataStartPath, FaceGenFilePath, out string filePath);
                    }

                    IEnumerable<string> FaceGenFilePaths = Directory.EnumerateFiles(
                        Path.Combine(dataStartPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom\\", modData.ModName),
                        "*" + form.OrigonalFormID + "*",
                        SearchOption.AllDirectories);

                    foreach (string FaceGenFilePath in FaceGenFilePaths)
                    {
                        CopyFormFile(form, dataStartPath, FaceGenFilePath, out string filePath);
                        EditedFaceGen = true;
                        using (StreamWriter stream = File.AppendText(GF.FaceGenFileFixPath))
                        {
                            stream.WriteLine(Path.GetFullPath(filePath) + ";" + form.OrigonalFormID + ";" + form.CompactedFormID);
                        }
                    }

                }
            }
        }
        #endregion FaceGen Eslify

        public static async Task<int> InumDataSubfolder(string subfolderStart, string directoryFilter, string fileFilter, string fileAtLogLine, string fileUnchangedLogLine)
        {
            if (Directory.Exists(subfolderStart))
            {
                IEnumerable<string> dataSubFolders = Directory.EnumerateDirectories(
                    subfolderStart,
                    directoryFilter,
                    SearchOption.AllDirectories);

                IEnumerable<string> files;

                foreach (string dataSubFolder in dataSubFolders)
                {
                    files = Directory.EnumerateFiles(
                        dataSubFolder,
                        fileFilter,
                        SearchOption.AllDirectories);
                    foreach (string condition in files)
                    {
                        GF.WriteLine(fileAtLogLine + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        bool changed = false;
                        string[] fileLines = FormInFileReader(File.ReadAllLines(condition), out changed);
                        GF.OuputDataFileToOutputFolder(changed, condition, fileLines, fileUnchangedLogLine);
                        //if (changed == true)
                        //{
                        //    string newPath = GF.FixOuputPath(condition);
                        //    Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                        //    File.WriteAllLines(newPath, fileLines);
                        //}
                        //else
                        //{
                        //    GF.WriteLine(fileUnchangedLogLine + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        //}
                    }
                }
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + subfolderStart);
            }

            return await Task.FromResult(0);
        }

        public static void InumSwappers(string startFolder, string fileNameFilter, string fileAtLogLine, string fileUnchangedLogLine)
        {
            if (!Directory.Exists(startFolder))
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + startFolder);
                return;
            }
            IEnumerable<string> _Conditions = Directory.EnumerateFiles(
                    startFolder,
                    fileNameFilter,
                    SearchOption.TopDirectoryOnly);
            foreach (string condition in _Conditions)
            {
                GF.WriteLine(fileAtLogLine + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                bool changed = false;
                string[] fileLines = FormInFileReader(File.ReadAllLines(condition), out changed);
                GF.OuputDataFileToOutputFolder(changed, condition, fileLines, fileUnchangedLogLine);
                //if (changed == true)
                //{
                //    string newPath = GF.FixOuputPath(condition);
                //    Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                //    File.WriteAllLines(newPath, fileLines);
                //}
                //else
                //{
                //    GF.WriteLine(fileUnchangedLogLine + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                //}
            }

        }

        public static void AutoBody()
        {
            string autoBodyMorphsPath = Path.Combine(GF.Settings.SkyrimDataFolderPath, "autoBody\\Config\\morphs.ini");
            if (File.Exists(autoBodyMorphsPath))
            {
                GF.WriteLine(GF.stringLoggingData.AutoBodyFileAt + autoBodyMorphsPath);
                bool changed = false;
                string[] fileLines = FormInFileReader(File.ReadAllLines(autoBodyMorphsPath), out changed);
                if (changed == true)
                {
                    string newPath = GF.FixOuputPath(autoBodyMorphsPath, GF.Settings.SkyrimDataFolderPath);
                    Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                    File.WriteAllLines(newPath, fileLines);
                }
                else
                {
                    GF.WriteLine($"{GF.stringLoggingData.AutoBodyFileUnchanged}{autoBodyMorphsPath}");
                }
            }
        }

        public static Task<int> CustomSkillsFramework()
        {
            string startSearchPath = Path.Combine(GF.Settings.SkyrimDataFolderPath, "NetScriptFramework\\Plugins");
            if (Directory.Exists(startSearchPath))
            {
                IEnumerable<string> customSkillConfigs = Directory.EnumerateFiles(
                    startSearchPath,
                    "CustomSkill*config.txt",
                    SearchOption.TopDirectoryOnly);
                foreach (string customSkillConfig in customSkillConfigs)
                {
                    GF.WriteLine(GF.stringLoggingData.CustomSkillsFileAt + customSkillConfig, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    string[] customSkillConfigFile = File.ReadAllLines(customSkillConfig);
                    string[] newCustomSkillConfigFile = new string[customSkillConfigFile.Length];
                    string currentModName = "";
                    CompactedModData currentMod = new CompactedModData();
                    bool changed = false;
                    for (int i = 0; i < customSkillConfigFile.Length; i++)
                    {
                        string line = customSkillConfigFile[i];
                        foreach (string modName in CompactedModDataD.Keys)
                        {
                            if (customSkillConfigFile[i].Contains(modName, StringComparison.OrdinalIgnoreCase))
                            {
                                currentModName = modName;
                                CompactedModDataD.TryGetValue(modName, out currentMod!);
                                GF.WriteLine(line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                            }
                        }
                        if (!currentModName.Equals(""))
                        {
                            foreach (FormHandler form in currentMod.CompactedModFormList)
                            {
                                if (line.Contains(form.OrigonalFormID.TrimStart('0')))
                                {
                                    GF.WriteLine("Old Line: " + line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                    line = line.Replace(form.OrigonalFormID.TrimStart('0'), form.CompactedFormID.TrimStart('0'));
                                    GF.WriteLine("New Line: " + line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                    currentModName = "";
                                    changed = true;
                                }
                            }
                        }
                        newCustomSkillConfigFile[i] = line;
                    }
                    GF.OuputDataFileToOutputFolder(changed, customSkillConfig, newCustomSkillConfigFile, GF.stringLoggingData.CustomSkillsFileUnchanged);
                }


            }
            return Task.FromResult(0);
        }



    }
}

//public static async Task<int> InumDAR()
//{
//    IEnumerable<string> _CustomConditions = Directory.EnumerateDirectories(
//        Path.Combine(GF.Settings.SkyrimDataFolderPath, "meshes"),
//        "_CustomConditions",
//        SearchOption.AllDirectories);

//    IEnumerable<string> _Conditions;

//    foreach (string _CustomConditionsFolder in _CustomConditions)
//    {
//        _Conditions = Directory.EnumerateFiles(
//            _CustomConditionsFolder,
//            "_conditions.txt",
//            SearchOption.AllDirectories);
//        foreach (string condition in _Conditions)
//        {
//            GF.WriteLine(GF.stringLoggingData.DARFileAt + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
//            bool changed = false;
//            string[] fileLines = FormInFileReader(File.ReadAllLines(condition), out changed);
//            if (changed == true)
//            {
//                string newPath = GF.FixOuputPath(condition);
//                Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
//                File.WriteAllLines(newPath, fileLines);
//            }
//            else
//            {
//                GF.WriteLine(GF.stringLoggingData.ConditionsUnchanged + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
//            }
//        }
//    }

//    return await Task.FromResult(1);
//}

//public static void InumSPID()
//{

//    IEnumerable<string> _Conditions = Directory.EnumerateFiles(
//            GF.Settings.SkyrimDataFolderPath,
//            "*_DISTR.ini",
//            SearchOption.TopDirectoryOnly);
//    foreach (string condition in _Conditions)
//    {
//        GF.WriteLine(GF.stringLoggingData.SPIDFileAt + condition);
//        bool changed = false;
//        string[] fileLines = FormInFileReader(File.ReadAllLines(condition), out changed);
//        if (changed == true)
//        {
//            string newPath = GF.FixOuputPath(condition);
//            Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
//            File.WriteAllLines(newPath, fileLines);
//        }
//        else
//        {
//            GF.WriteLine($"{GF.stringLoggingData.SPIDFileUnchanged}{condition}");
//        }
//    }

//}

//public static void InumBaseObject()
//{

//    IEnumerable<string> _Conditions = Directory.EnumerateFiles(
//            GF.Settings.SkyrimDataFolderPath,
//            "*_SWAP.ini",
//            SearchOption.TopDirectoryOnly);
//    foreach (string condition in _Conditions)
//    {
//        GF.WriteLine(GF.stringLoggingData.BaseObjectFileAt + condition);
//        bool changed = false;
//        string[] fileLines = FormInFileReader(File.ReadAllLines(condition), out changed);
//        if (changed == true)
//        {
//            string newPath = GF.FixOuputPath(condition);
//            Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
//            File.WriteAllLines(newPath, fileLines);
//        }
//        else
//        {
//            GF.WriteLine($"{GF.stringLoggingData.BaseObjectFileUnchanged}{condition}");
//        }
//    }

//}
