using ESLifyEverything.FormData;
using ESLifyEverything.XEdit;
using System;
using System.Collections.Generic;
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
                mod.GetModData(Path.GetFileName(compactedFormsModFile).Replace("_ESlEverything.json", "").Replace("_ESlEverything", ""));
                mod.Write();
                CompactedModData.TryAdd(mod.ModName, mod);
            }
        }

        public static void CopyFormFile(FormHandler form, string OrgFilePath, out string origonalFilePath)
        {
            GF.WriteLine(GF.stringLoggingData.OriganalPath + OrgFilePath);
            string newPath = GF.FixOuputPath(OrgFilePath.Replace(form.OrigonalFormID, form.CompactedFormID));
            GF.WriteLine(GF.stringLoggingData.NewPath + newPath);
            Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
            origonalFilePath = newPath;
            File.Copy(OrgFilePath, newPath, true);
        }

        #region Voice Eslify
        public static void VoiceESlIfyMenu()
        {
            GF.WriteLine(GF.stringLoggingData.VoiceESLMenuHeader,true, false);
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
            foreach (CompactedModData modData in CompactedModData.Values)
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
                if (CompactedModData.TryGetValue(input, out modData!))
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
            if (Directory.Exists(Path.Combine(GF.Settings.SkyrimDataFolderPath, "sound\\voice", modData.ModName))) {
                foreach (FormHandler form in modData.CompactedModFormList)
                {
                    IEnumerable<string> voiceFilePaths = Directory.EnumerateFiles(
                        Path.Combine(GF.Settings.SkyrimDataFolderPath, "sound\\voice", modData.ModName),
                        "*" + form.OrigonalFormID + "*",
                        SearchOption.AllDirectories);
                    foreach (string voiceFilePath in voiceFilePaths)
                    {
                        CopyFormFile(form, voiceFilePath, out string filePath);
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
            foreach (CompactedModData modData in CompactedModData.Values)
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
                GF.WriteLine($"{GF.stringLoggingData.SingleModInputHeader}{GF.stringLoggingData.ExamplePlugin}");
                GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
                input = Console.ReadLine() ?? "";
                if (CompactedModData.TryGetValue(input, out modData!))
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
            if (Directory.Exists(Path.Combine(GF.Settings.SkyrimDataFolderPath, "Textures\\Actors\\Character\\FaceGenData\\FaceTint\\", modData.ModName)))
            {
                foreach (FormHandler form in modData.CompactedModFormList)
                {
                    IEnumerable<string> FaceGenTexFilePaths = Directory.EnumerateFiles(
                        Path.Combine(GF.Settings.SkyrimDataFolderPath, "Textures\\Actors\\Character\\FaceGenData\\FaceTint\\", modData.ModName),
                        "*" + form.OrigonalFormID + "*",
                        SearchOption.AllDirectories);
                    foreach (string FaceGenFilePath in FaceGenTexFilePaths)
                    {
                        CopyFormFile(form, FaceGenFilePath, out string filePath);
                    }

                    IEnumerable<string> FaceGenFilePaths = Directory.EnumerateFiles(
                        Path.Combine(GF.Settings.SkyrimDataFolderPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom\\", modData.ModName),
                        "*" + form.OrigonalFormID + "*",
                        SearchOption.AllDirectories);

                    foreach (string FaceGenFilePath in FaceGenFilePaths)
                    {
                        CopyFormFile(form, FaceGenFilePath, out string filePath);
                        Console.WriteLine(filePath);
                        using (StreamWriter stream = File.AppendText(GF.FaceGenFileFixPath))
                        {
                            stream.WriteLine(Path.GetFullPath(filePath) + ";" + form.OrigonalFormID + ";" + form.CompactedFormID);
                        }
                    }

                }
            }
        }
        #endregion FaceGen Eslify

        #region DAR Eslify
        public static void InumDAR()
        {
            IEnumerable<string> _CustomConditions = Directory.EnumerateDirectories(
                Path.Combine(GF.Settings.SkyrimDataFolderPath, "meshes"),
                "_CustomConditions",
                SearchOption.AllDirectories);
            IEnumerable<string> _Conditions;

            foreach (string _CustomConditionsFolder in _CustomConditions)
            {
                _Conditions = Directory.EnumerateFiles(
                    _CustomConditionsFolder,
                    "_conditions.txt",
                    SearchOption.AllDirectories);
                foreach (string condition in _Conditions)
                {
                    GF.WriteLine(GF.stringLoggingData.DARFileAt + condition);
                    bool changed = false;
                    string[] fileLines = DARFileReader(File.ReadAllLines(condition), out changed);
                    if (changed == true)
                    {
                        string newPath = GF.FixOuputPath(condition);
                        Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                        File.WriteAllLines(newPath, fileLines);
                    }
                    else
                    {
                        GF.WriteLine($"{GF.stringLoggingData.ConditionsUnchanged}{condition}");
                    }
                }
            }
        }

        public static string[] DARFileReader(string[] fileLines, out bool changed)
        {
            changed = false;
            string[] lines = new string[fileLines.Length];
            for (int i = 0; i < fileLines.Length; i++)
            {
                string line = fileLines[i];
                if (line.Contains(".esp", StringComparison.OrdinalIgnoreCase) || line.Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (CompactedModData modData in CompactedModData.Values)
                    {
                        if (line.Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (FormHandler form in modData.CompactedModFormList)
                            {
                                if (line.Contains(form.OrigonalFormID, StringComparison.OrdinalIgnoreCase))
                                {
                                    line = line.Replace(form.OrigonalFormID, form.CompactedFormID);
                                    GF.WriteLine(line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
                lines[i] = line;
            }
            return lines;
        }
        #endregion DAR Eslify

        #region SPID Eslify
        public static void InumSPID()
        {

            IEnumerable<string> _Conditions = Directory.EnumerateFiles(
                    GF.Settings.SkyrimDataFolderPath,
                    "*_DISTR.ini",
                    SearchOption.TopDirectoryOnly);
            foreach (string condition in _Conditions)
            {
                GF.WriteLine(GF.stringLoggingData.SPIDFileAt + condition);
                bool changed = false;
                string[] fileLines = SPIDFileReader(File.ReadAllLines(condition), out changed);
                if (changed == true)
                {
                    string newPath = GF.FixOuputPath(condition);
                    Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                    File.WriteAllLines(newPath, fileLines);
                }
                else
                {
                    GF.WriteLine($"{GF.stringLoggingData.SPIDFileUnchanged}{condition}");
                }
            }

        }

        public static string[] SPIDFileReader(string[] fileLines, out bool changed)
        {
            changed = false;
            string[] lines = new string[fileLines.Length];
            for (int i = 0; i < fileLines.Length; i++)
            {
                string line = fileLines[i];
                if (line.Contains(".esp", StringComparison.OrdinalIgnoreCase) || line.Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (CompactedModData modData in CompactedModData.Values)
                    {
                        if (line.Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (FormHandler form in modData.CompactedModFormList)
                            {
                                if (line.Contains(form.OrigonalFormID, StringComparison.OrdinalIgnoreCase))
                                {
                                    line = line.Replace(form.OrigonalFormID, form.CompactedFormID);
                                    Console.WriteLine(line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
                lines[i] = line;
            }
            return lines;
        }
        #endregion SPID Eslify
    }
}
