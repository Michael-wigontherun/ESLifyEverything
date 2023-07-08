using ESLifyEverything.FormData;
using ESLifyEverything.PluginHandles;
using ESLifyEverything.XEdit;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Masters;
using System.Diagnostics;
using System.Text.Json;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace ESLifyEverything
{
    public static partial class Program
    {
        //Region for reading the xEdit log
        #region xEdit Log
        //Parses the xEdit log and readys it for output
        private static void XEditSession()
        {
            XEditLogReader.ReadLog(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName));
            int xEditSessionsCount = XEditLogReader.xEditLog.xEditSessions?.Length ?? 0;
            if (xEditSessionsCount <= 0)
            {
                GF.WriteLine(GF.stringLoggingData.NoxEditSessions);
            }
            else if (GF.Settings.AutoReadAllxEditSession)
            {
                string fileName = Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName);
                FileInfo fi = new FileInfo(fileName);
                if (fi.Length > 10000000)//Path.Combine(GF.Settings.XEditFilePath, GF.Settings.XEditLogFileName)
                {
                    GF.WriteLine(GF.stringLoggingData.XEditLogFileSizeWarning);
                }
                XEditSessionAutoAll();
                if (GF.Settings.DeletexEditLogAfterRun_Requires_AutoReadAllxEditSession)
                {
                    File.Delete(Path.Combine(GF.Settings.XEditFolderPath, GF.Settings.XEditLogFileName));
                }
            }
            else if (GF.Settings.AutoReadNewestxEditSession)
            {
                XEditLogReader.xEditLog.xEditSessions![xEditSessionsCount - 1].GenerateCompactedModDatas();
            }
            else
            {
                XEditSessionMenu();
            }
        }

        //Menu to pick which sessions to output
        private static void XEditSessionMenu()
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
            while (GF.WhileMenuSelect(xEditSessionsCount - 1, out selectedMenuItem) == false);
            if (selectedMenuItem != -1) XEditLogReader.xEditLog.xEditSessions![selectedMenuItem].GenerateCompactedModDatas();
        }

        //Outputs all xEdit sessions to output to Compacted Mod Data
        private static void XEditSessionAutoAll()
        {
            foreach (XEditSession session in XEditLogReader.xEditLog.xEditSessions!)
            {
                session.GenerateCompactedModDatas();
            }
        }
        #endregion xEdit Log

        //Trys to locate and then build a zMerge cache
        private static void BuildMergedData()
        {
            IEnumerable<string> mergeFolders = Directory.EnumerateDirectories(
                GF.Settings.DataFolderPath,
                "merge - *",
                SearchOption.TopDirectoryOnly);
            foreach(string folder in mergeFolders)
            {
                string mergeJsonPath = Path.Combine(folder, "merge.json");
                if (File.Exists(mergeJsonPath))
                {
                    GF.WriteLine(GF.stringLoggingData.MergeFound + folder, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    CompactedMergeData mergeData = new CompactedMergeData(mergeJsonPath, out bool success);
                    if (success)
                    {
                        string potentialMergeDataCachPath = Path.Combine(GF.CompactedFormsFolder, mergeData.MergeName + GF.MergeCacheExtension);

                        if (File.Exists(potentialMergeDataCachPath))
                        {
                            CompactedMergeData previouslyCachedMergeData = JsonSerializer.Deserialize<CompactedMergeData>(File.ReadAllText(potentialMergeDataCachPath))!;
                            if (previouslyCachedMergeData.AlreadyCached())
                            {
                                GF.WriteLine(mergeData.MergeName + GF.stringLoggingData.PluginCheckPrev, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                GF.WriteLine(string.Format(GF.stringLoggingData.SkippingImport, mergeData.MergeName + GF.MergeCacheExtension), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                continue;
                            }
                        }

                        string mapPath = Path.Combine(folder, "map.json");
                        string fidCachePath = Path.Combine(folder, "fidCache.json");

                        if (File.Exists(mapPath) && File.Exists(fidCachePath))
                        {
                            if (mergeData.CompactedModDataD != null)
                            {
                                CompactedModData? outputtedCompactedModData = null;
                                string potentialCompactedModDataPath = Path.Combine(GF.CompactedFormsFolder, mergeData.MergeName + GF.CompactedFormExtension);
                                if (File.Exists(potentialCompactedModDataPath))
                                {
                                    GF.WriteLine(GF.stringLoggingData.ReadingCompDataLog + potentialCompactedModDataPath, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                    outputtedCompactedModData = JsonSerializer.Deserialize<CompactedModData>(File.ReadAllText(potentialCompactedModDataPath))!;
                                }

                                IConfiguration fidCache = new ConfigurationBuilder()
                                    .AddJsonFile(fidCachePath)
                                    .AddEnvironmentVariables().Build();

                                IConfiguration mergeMap = new ConfigurationBuilder()
                                    .AddJsonFile(mapPath)
                                    .AddEnvironmentVariables().Build();
                                foreach (string key in mergeData.CompactedModDataD.Keys)
                                {
                                    if(mergeData.CompactedModDataD.TryGetValue(key, out CompactedModData? compactedModData))
                                    {
                                        try
                                        {
                                            foreach (KeyValuePair<string, string> mapping in mergeMap.GetRequiredSection(compactedModData.ModName).Get<Dictionary<string, string>>())
                                            {
                                                FormHandler form = new FormHandler(mergeData.MergeName, mapping.Key, mapping.Value);

                                                if (outputtedCompactedModData != null)
                                                {
                                                    foreach (FormHandler formHandler in outputtedCompactedModData.CompactedModFormList)
                                                    {
                                                        if (formHandler.OriginalFormID.Equals(mapping.Value))
                                                        {
                                                            form.ChangeCompactedID(formHandler.CompactedFormID);
                                                        }
                                                    }
                                                }

                                                compactedModData.CompactedModFormList.Add(form);
                                            }
                                        }
                                        catch(InvalidOperationException)
                                        {
                                            GF.WriteLine(String.Format(GF.stringLoggingData.NoChangedFormsFor, key, mergeData.MergeName), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                        }
                                        catch(Exception e)
                                        {
                                            GF.WriteLine(mapPath);
                                            GF.WriteLine(e.Message);
                                        }

                                        try
                                        {
                                            foreach (string fidCacheKP in fidCache.GetRequiredSection(compactedModData.ModName).Get<HashSet<string>>())
                                            {
                                                FormHandler form = new FormHandler(mergeData.MergeName, fidCacheKP, fidCacheKP);
                                                if (outputtedCompactedModData != null)
                                                {
                                                    foreach (FormHandler formHandler in outputtedCompactedModData.CompactedModFormList)
                                                    {
                                                        if (formHandler.OriginalFormID.Equals(fidCacheKP))
                                                        {
                                                            form.ChangeCompactedID(formHandler.CompactedFormID);
                                                        }
                                                    }
                                                }
                                                compactedModData.AddIfMissing(form);
                                                
                                            }
                                        }
                                        catch (InvalidOperationException)
                                        {
                                            GF.WriteLine(String.Format(GF.stringLoggingData.NoChangedFormsFor, key, mergeData.MergeName), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                        }

                                        mergeData.CompactedModDatas.Add(compactedModData);
                                    }
                                }

                                if (outputtedCompactedModData != null)
                                {
                                    GF.WriteLine(string.Format(GF.stringLoggingData.SetToIgnore, mergeData.MergeName + GF.CompactedFormExtension, mergeData.MergeName + GF.CompactedFormIgnoreExtension));
                                    File.Move(potentialCompactedModDataPath, Path.Combine(GF.CompactedFormsFolder, mergeData.MergeName + GF.CompactedFormIgnoreExtension), true);
                                }

                                mergeData.NewRecordCount = mergeData.CoundNewRecords();

                                mergeData.OutputModData(true);
                            }
                        }
                        else
                        {
                            GF.WriteLine(string.Format(GF.stringLoggingData.MapNotFound, mergeData.MergeName + GF.MergeCacheExtension), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                            GF.WriteLine(string.Format(GF.stringLoggingData.SkippingImport, mergeData.MergeName + GF.MergeCacheExtension), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        }
                    }
                    else
                    {
                        GF.WriteLine(String.Format(GF.stringLoggingData.PluginNotFoundImport, mergeData.MergeName), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        GF.WriteLine(String.Format(GF.stringLoggingData.SkippingImport, mergeData.MergeName), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    }
                }
            }
        }

        //Region for importing Compacted Mod Data Files
        #region Import Mod Data
        //Imports all _CompactedModData.json files for ESLify Everything
        private static void ImportModData(string compactedFormsLocation)
        {
            if (!Directory.Exists(compactedFormsLocation))
            {
                GF.WriteLine(String.Format(GF.stringLoggingData.NoCMDinDataFolder, compactedFormsLocation));
                return;
            }

            IEnumerable<string> compactedFormsModFiles = Directory.EnumerateFiles(
                compactedFormsLocation,
                "*" + GF.CompactedFormExtension,
                SearchOption.AllDirectories);

            if (!compactedFormsModFiles.Any())
            {
                GF.WriteLine(String.Format(GF.stringLoggingData.NoCMDinDataFolder, compactedFormsLocation));
                return;
            }

            foreach (string compactedFormsModFile in compactedFormsModFiles)
            {
                GF.WriteLine(GF.stringLoggingData.ReadingCompDataLog + compactedFormsModFile);
                CompactedModData modData = JsonSerializer.Deserialize<CompactedModData>(File.ReadAllText(compactedFormsModFile))!;
                modData.Write();
                if (!File.Exists(Path.Combine(GF.Settings.DataFolderPath, modData.ModName)))
                {
                    GF.WriteLine(String.Format(GF.stringLoggingData.PluginNotFoundImport, compactedFormsModFile));
                    continue;
                }
                
                if (modData.Recheck == true)
                {
                    if (modData.PluginLastModifiedValidation is null)
                    {
                        modData = ValidateCompactedModDataJson(modData);
                    }
                    else
                    {
                        if (!modData.PluginLastModifiedValidation!.Value.Equals(File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, modData.ModName))))
                        {
                            modData = ValidateCompactedModDataJson(modData);
                        }
                    }
                }

                if (modData.Enabled == true)
                {
                    if (modData.PluginLastModifiedValidation is not null)
                    {
                        string splitModDataPath = Path.Combine(compactedFormsLocation, modData.ModName + GF.ModSplitDataExtension);
                        if (File.Exists(splitModDataPath))
                        {
                            CompactedModData splitModData = JsonSerializer.Deserialize<CompactedModData>(File.ReadAllText(splitModDataPath))!;
                            if (splitModData.PluginLastModifiedValidation.Equals(modData.PluginLastModifiedValidation))
                            {
                                foreach (FormHandler form in splitModData.CompactedModFormList)
                                {
                                    modData.CompactedModFormList.Add(form);
                                }
                            }
                        }

                        //if (!modData.PreviouslyESLified || GF.Settings.ImportAllCompactedModData || ImportModDataCheck(modData.ModName))
                        //{
                            GF.WriteLine(GF.stringLoggingData.ImportingCompDataLog + modData.ModName);
                            CompactedModDataD.TryAdd(modData.ModName, modData);
                        //}
                        //else
                        //{
                        //    GF.WriteLine(GF.stringLoggingData.ImportingCompDataLogOSP + modData.ModName);
                        //}
                        
                        //CompactedModDataDNoFaceVoice.TryAdd(modData.ModName, modData);

                    }
                }
                else
                {
                    GF.WriteLine(string.Format(GF.stringLoggingData.SkippingImport, modData.ModName + GF.CompactedFormExtension));
                }
                
            }

            
        }

        private static void ImportMergeData()
        {
            IEnumerable<string> compactedFormsModFiles = Directory.EnumerateFiles(
                GF.CompactedFormsFolder,
                "*" + GF.MergeCacheExtension,
                SearchOption.AllDirectories);

            if(compactedFormsModFiles.Any()) MergesFound = true;

            foreach(string file in compactedFormsModFiles)
            {
                CompactedMergeData mergeData = JsonSerializer.Deserialize<CompactedMergeData>(File.ReadAllText(file))!;
                string pluginPath = Path.Combine(GF.Settings.DataFolderPath, mergeData.MergeName);
                if(mergeData.NewRecordCount != null)
                {
                    if (mergeData.NewRecordCount >= GF.LargeMergeCount)
                    {
                        if (!GF.Settings.EnableLargeMerges)
                        {
                            GF.WriteLine(GF.stringLoggingData.SkippingMergeCache + mergeData.MergeName);
                            continue;
                        }
                    }
                }
                else
                {
                    mergeData.NewRecordCount = mergeData.CoundNewRecords();
                    if(mergeData.NewRecordCount >= GF.LargeMergeCount)
                    {
                        if (!GF.Settings.EnableLargeMerges)
                        {
                            GF.WriteLine(GF.stringLoggingData.SkippingMergeCache + mergeData.MergeName);
                            continue;
                        }
                    }
                    mergeData.OutputModData(false);
                }

                if (File.Exists(pluginPath) && ActiveLoadOrder.Contains(mergeData.MergeName))
                {
                    if (GF.Settings.AutoRunMergedPluginFixer)
                    {
                        mergeData.MergedPluginFixer();
                    }
                    else
                    {
                        GF.WriteLine("");
                        GF.WriteLine("");
                        GF.WriteLine(String.Format("Do you want to check over plugins added to the merge {0} for other CompactedModData?", mergeData.MergeName));
                        GF.WriteLine("Any input other then N will start the MergedPluginFixer.");
                        string? input = Console.ReadLine();
                        if(input != null)
                        {
                            GF.WriteLine("Input: " + input, false, true);
                            if (!input.Equals("N", StringComparison.OrdinalIgnoreCase))
                            {
                                mergeData.MergedPluginFixer();
                            }
                        }
                        else
                        {
                            GF.WriteLine("Input: " + "Empty.String", false, true);
                        }
                    }

                    if (mergeData.AlreadyCached())
                    {
                        GF.WriteLine(GF.stringLoggingData.ImportingMergeCache + file);
                        foreach (CompactedModData compactedModData in mergeData.CompactedModDatas)
                        {
                            if (compactedModData.CompactedModFormList.Any())
                            {
                                compactedModData.FromMerge = true;
                                compactedModData.MergeName = mergeData.MergeName;
                                GF.IgnoredPlugins.Add(compactedModData.ModName);
                                GF.WriteLine(GF.stringLoggingData.ImportingMergeCompactedModData + compactedModData.ModName, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                if(!CompactedModDataD.TryAdd(compactedModData.ModName, compactedModData))
                                {
                                    if(CompactedModDataD.ContainsKey(compactedModData.ModName))
                                    {
                                        CompactedModDataD[compactedModData.ModName] = compactedModData;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    GF.WriteLine(String.Format(GF.stringLoggingData.PluginNotFoundImport, mergeData.MergeName), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    GF.WriteLine(String.Format(GF.stringLoggingData.SkippingImport, mergeData.MergeName), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                }
            }
        }

        //Validates whether the CompactedModData is still valid compared to the Plugin
        private static CompactedModData ValidateCompactedModDataJson(CompactedModData modData)
        {
            if (modData.NotCompactedData.HasValue && modData.NotCompactedData.Value) 
            {
                modData.PluginLastModifiedValidation = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, modData.ModName));
                modData.Enabled = true;
                modData.Recheck = true;
            }
            else if (modData.IsCompacted(false))
            {
                modData.PluginLastModifiedValidation = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, modData.ModName));
                modData.Enabled = true;
                modData.Recheck = true;
            }
            else if (modData.IsCompacted(true))
            {
                modData.PluginLastModifiedValidation = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, modData.ModName));
                modData.Enabled = true;
                modData.Recheck = true;
            }
            else
            {
                modData.PreviouslyESLified = false;
                modData.PluginLastModifiedValidation = null;
                GF.WriteLine("");
                GF.WriteLine("", false, true);
                GF.WriteLine("", false, true);
                modData.Enabled = false;
                GF.WriteLine(String.Format(GF.stringLoggingData.OutOfDateCMData1, modData.ModName + GF.CompactedFormExtension));
                Console.WriteLine();

                bool notReCompactedFully = true;

                if (GF.Settings.RunSubPluginCompaction)
                {
                    GF.WriteLine(GF.stringLoggingData.RunPluginRecompactionMenu1);
                    GF.WriteLine(GF.stringLoggingData.RunPluginRecompactionMenu2);
                    GF.WriteLine(GF.stringLoggingData.RunPluginRecompactionMenu3);
                    GF.WriteLine(GF.stringLoggingData.RunPluginRecompactionEnterPrompt);

                    string input = Console.ReadLine()!;
                    GF.WriteLine("Input: " + input, false, true);

                    if (input != null)
                    {
                        if (input.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            bool added = CompactedModDataD.TryAdd(modData.ModName, modData);
                            RunRecompact(modData.ModName);
                            if (added)
                            {
                                CompactedModDataD.Remove(modData.ModName);
                            }

                            if (modData.IsCompacted(true))
                            {
                                modData.PluginLastModifiedValidation = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, modData.ModName));
                                modData.Enabled = true;
                                modData.Recheck = true;
                                notReCompactedFully = false;
                            }
                        }
                    }
                }

                if(notReCompactedFully)
                {
                    GF.WriteLine(String.Format(GF.stringLoggingData.OutOfDateCMData2, modData.ModName));
                    GF.WriteLine(String.Format(GF.stringLoggingData.OutOfDateCMData3, modData.ModName));
                    GF.WriteLine(String.Format(GF.stringLoggingData.OutOfDateCMData4, modData.ModName));
                    GF.WriteLine(String.Format(GF.stringLoggingData.OutOfDateCMData5, modData.ModName));
                }

                GF.WriteLine(GF.stringLoggingData.EnterToContinue);
                Console.ReadLine();
                GF.WriteLine("", false, true);
                GF.WriteLine("", false, true);
                GF.WriteLine("");
            }
            modData.OutputModData(false, false);
            return modData;
        }

        //Recompacts the known Forms that relate to the CompactedModData
        //This does not change Forms that are not known inside the CompactedModData
        private static void RunRecompact(string pluginName)
        {
            Task<int> handlePluginTask = HandleMod.HandleSkyrimMod(pluginName);
            handlePluginTask.Wait();
            switch (handlePluginTask.Result)
            {
                case 0:
                    GF.WriteLine(pluginName + GF.stringLoggingData.PluginNotFound);
                    break;
                case 1:
                    break;
                case 2:
                    GF.WriteLine(pluginName + GF.stringLoggingData.PluginNotChanged);
                    break;
                case 3:
                    GF.WriteLine(pluginName + GF.stringLoggingData.PluginMissingMasterFile);
                    break;
                default:
                    GF.WriteLine(GF.stringLoggingData.PluginSwitchDefaultMessage);
                    break;
            }
            handlePluginTask.Dispose();
        }
        #endregion Import Mod Data

        //Parses the BSA's in the Data folder
        private static async Task<int> LoadOrderBSAData()
        {
            string pluginsFilePath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData")!, "Skyrim Special Edition", "plugins.txt");
            if (!File.Exists(pluginsFilePath))
            {
                pluginsFilePath = "plugins.txt";
                if (File.Exists(pluginsFilePath))
                {
                    ActiveLoadOrder = GF.FilterForActiveLoadOrder(pluginsFilePath);
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.LoadOrderNotDetectedError);
                    GF.WriteLine(GF.stringLoggingData.RunOrReport);
                    BSANotExtracted = true;
                    return await Task.FromResult(1);
                }
            }
            else
            {
                ActiveLoadOrder = GF.FilterForActiveLoadOrder(pluginsFilePath);
            }

            string loadOrderFilePath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData")!, "Skyrim Special Edition", "loadorder.txt");
            if (!File.Exists(pluginsFilePath))
            {
                pluginsFilePath = "loadorder.txt";
            }
            if (File.Exists(loadOrderFilePath))
            {
                string[] loadOrder = File.ReadAllLines(loadOrderFilePath);
                
                foreach (string plugin in loadOrder)
                {
                    string pluginNoExtension = Path.ChangeExtension(plugin, null);
                    if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, pluginNoExtension + ".bsa")))
                    {
                        LoadOrderNoExtensions.Add(pluginNoExtension);
                    }
                }

                int loadorderCount = LoadOrderNoExtensions.Count;
                for (int i = 0; i < loadorderCount; i++)
                {
                    if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, LoadOrderNoExtensions[i] + ".bsa")))
                    {
                        GF.WriteLine(String.Format(GF.stringLoggingData.BSACheckMod, LoadOrderNoExtensions[i]));
                        BSAData.AddNew(LoadOrderNoExtensions[i]);
                    }
                    Console.WriteLine();
                    GF.WriteLine(String.Format(GF.stringLoggingData.ProcessedBSAsLogCount, i + 1, loadorderCount));
                    Console.WriteLine();
                }
                BSAData.Output();
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.LoadOrderNotDetectedError);
                GF.WriteLine(GF.stringLoggingData.RunOrReport);
                BSANotExtracted = true;
                return await Task.FromResult(1);
            }
            return await Task.FromResult(0);
        }

        private static bool CheckModDataCheck(string modName)
        {
            if (CheckEverything)
            {
                return true;
            }
            return AlwaysCheckList.Contains(modName);
        }

        //Region for Voice Eslify
        #region Voice Eslify
        //Voice Eslify Main Menu
        private static void VoiceESlIfyMenu()
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

        //Runs all Compacted Mod Data
        private static bool VoiceESLifyEverything()
        {
            foreach (CompactedModData modData in CompactedModDataD.Values)
            {
                if (!modData.PreviouslyESLified || GF.Settings.RunAllVoiceAndFaceGen || CheckModDataCheck(modData.ModName))
                {
                    VoiceESLifyMod(modData);
                }
            }
            return false;
        }

        //Voice Eslify menu to select which Compacted Mod Data to check
        private static void VoiceESLifySingleMod()
        {
            bool whileContinue = true;
            string input;
            CompactedModData? modData;
            do
            {
                GF.WriteLine($"{GF.stringLoggingData.SingleModInputHeader}{GF.stringLoggingData.ExamplePlugin}");
                GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
                input = Console.ReadLine() ?? "";
                if (CompactedModDataD.TryGetValue(input, out modData))
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

        //Runs all needed methods to acuretly find Voice lines from the Compacted Mod Data
        private static void VoiceESLifyMod(CompactedModData modData)
        {
            Task v = ExtractBSAVoiceData(modData.ModName);
            v.Wait();
            v.Dispose();
            Task e = VoiceESLifyModData(modData, GF.ExtractedBSAModDataPath);
            e.Wait();
            e.Dispose();
            Task l = VoiceESLifyModData(modData, GF.Settings.DataFolderPath);
            l.Wait();
            l.Dispose();
        }

        //Extracts Voice Lines from BSA with Voice lines connected to the plugin
        private static async Task<int> ExtractBSAVoiceData(string pluginName)
        {
            foreach (string plugin in LoadOrderNoExtensions)
            {
                BSA? bsa;
                if (BSAData.BSAs.TryGetValue(plugin, out bsa))
                {
                    foreach (string connectedVoice in bsa.VoiceModConnections)
                    {
                        if (connectedVoice.Equals(pluginName, StringComparison.OrdinalIgnoreCase))
                        {
                            string line = "";
                            GF.WriteLine(String.Format(GF.stringLoggingData.BSAContainsData, bsa.BSAName_NoExtention, pluginName));
                            Process p = new Process();
                            p.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
                            p.StartInfo.Arguments = $"\"{Path.GetFullPath(Path.Combine(GF.Settings.DataFolderPath, bsa.BSAName_NoExtention + ".bsa"))}\" -f \"{pluginName}\"  -e -o \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\"";
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
                        }
                    }
                }
            }
            return await Task.FromResult(0);
        }

        //Checks the given Compacted Mod Data for Voice lines and fixes them from targeted locations
        private static async Task<int> VoiceESLifyModData(CompactedModData modData, string dataStartPath)
        {
            DevLog.Log("Voice ESLify: " + modData.ModName);
            if (Directory.Exists(Path.Combine(dataStartPath, "sound\\voice", modData.ModName)))
            {
                DevLog.Log("Voice Lines Found: " + modData.ModName);
                foreach (FormHandler form in modData.CompactedModFormList)
                {
                    IEnumerable<string> voiceFilePaths = Directory.EnumerateFiles(
                        Path.Combine(dataStartPath, "sound\\voice", modData.ModName),
                        "*" + form.OriginalFormID + "*",
                        SearchOption.AllDirectories);
                    foreach (string voiceFilePath in voiceFilePaths)
                    {
                        GF.WriteLine(GF.stringLoggingData.OriganalPath + voiceFilePath, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        string[] pathArr = voiceFilePath.Split('\\');

                        string newStartPath = Path.Combine(GF.Settings.OutputFolder, $"sound\\voice\\{form.ModName}\\{pathArr[pathArr.Length - 2]}");
                        Directory.CreateDirectory(newStartPath);
                        string newPath = Path.Combine(newStartPath, pathArr[pathArr.Length - 1].Replace(form.OriginalFormID, form.CompactedFormID, StringComparison.OrdinalIgnoreCase));

                        //File.Copy(voiceFilePath, newPath, true);
                        byte[] vfile = File.ReadAllBytes(voiceFilePath);
                        File.WriteAllBytes(newPath, vfile);


                        GF.WriteLine(GF.stringLoggingData.NewPath + newPath);
                    }
                }
            }
            return await Task.FromResult(1);
        }
        #endregion Voice Eslify

        //Region for FaceGen Eslify
        #region FaceGen Eslify
        //FaceGen Eslify Main Menu
        private static void FaceGenESlIfyMenu()
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

        //Runs all Compacted Mod Data
        private static bool FaceGenESLifyEverything()
        {
            foreach (CompactedModData modData in CompactedModDataD.Values)
            {
                if (!modData.PreviouslyESLified || GF.Settings.RunAllVoiceAndFaceGen || CheckModDataCheck(modData.ModName))
                {
                    FaceGenESLifyMod(modData);
                }
            }
            return false;
        }

        //FaceGen Eslify menu to select which Compacted Mod Data to check
        private static void FaceGenESLifySingleMod()
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

        //Runs all needed methods to acuretly find FaceGen from the Compacted Mod Data
        private static void FaceGenESLifyMod(CompactedModData modData)
        {
            Task f = ExtractBSAFaceGenData(modData.ModName);
            f.Wait();
            f.Dispose();
            Task e = FaceGenEslifyModData(modData, GF.ExtractedBSAModDataPath);
            e.Wait();
            e.Dispose();
            Task l = FaceGenEslifyModData(modData, GF.Settings.DataFolderPath);
            l.Wait();
            l.Dispose();

        }

        //Extracts Voice Lines from BSA with FaceGen connected to the plugin
        private static async Task<int> ExtractBSAFaceGenData(string pluginName)
        {
            foreach (string plugin in LoadOrderNoExtensions)
            {
                BSA? bsa;
                if (BSAData.BSAs.TryGetValue(plugin, out bsa))
                {
                    foreach (string connectedFaceGen in bsa.FaceGenModConnections)
                    {
                        if (connectedFaceGen.Equals(pluginName, StringComparison.OrdinalIgnoreCase))
                        {
                            string line = "";
                            GF.WriteLine(String.Format(GF.stringLoggingData.BSAContainsData, bsa.BSAName_NoExtention, pluginName));
                            Process m = new Process();
                            m.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
                            m.StartInfo.Arguments = $"\"{Path.GetFullPath(Path.Combine(GF.Settings.DataFolderPath, bsa.BSAName_NoExtention + ".bsa"))}\" -f \"{pluginName}\"  -e -o \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\"";
                            if (GF.DevSettings.DevLogging)
                            {
                                m.StartInfo.UseShellExecute = false;
                                m.StartInfo.RedirectStandardOutput = true;
                                m.StartInfo.RedirectStandardError = true;
                                m.StartInfo.CreateNoWindow = true;
                                m.Start();
                                while (!m.StandardOutput.EndOfStream)
                                {
                                    string tempLine = m.StandardOutput.ReadLine()!;
                                    if (tempLine != string.Empty)
                                    {
                                        line = tempLine;
                                    }
                                }
                            }
                            else
                            {
                                m.Start();
                            }
                            m.WaitForExit();
                            m.Dispose();
                            DevLog.Log(line);
                            if (bsa.HasTextureBSA)
                            {
                                line = "";
                                Process t = new Process();
                                t.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
                                t.StartInfo.Arguments = $"\"{Path.GetFullPath(Path.Combine(GF.Settings.DataFolderPath, bsa.BSAName_NoExtention + " - Textures.bsa"))}\" -f \"{pluginName}\"  -e -o \"{Path.GetFullPath(GF.ExtractedBSAModDataPath)}\"";
                                if (GF.DevSettings.DevLogging)
                                {
                                    t.StartInfo.UseShellExecute = false;
                                    t.StartInfo.RedirectStandardOutput = true;
                                    t.StartInfo.RedirectStandardError = true;
                                    t.StartInfo.CreateNoWindow = true;
                                    t.Start();
                                    while (!t.StandardOutput.EndOfStream)
                                    {
                                        string tempLine = t.StandardOutput.ReadLine()!;
                                        if (tempLine != string.Empty)
                                        {
                                            line = tempLine;
                                        }
                                    }
                                }
                                else
                                {
                                    t.Start();
                                }
                                t.WaitForExit();
                                t.Dispose();
                                DevLog.Log(line);
                            }
                        }
                    }
                }
            }
            return await Task.FromResult(0);
        }

        //Checks the given Compacted Mod Data for FaceGen and fixes them from targeted locations
        private static async Task<int> FaceGenEslifyModData(CompactedModData modData, string dataStartPath)
        {
            DevLog.Log("FaceGen ESLify: " + modData.ModName);
            if (Directory.Exists(Path.Combine(dataStartPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom\\", modData.ModName)))
            {
                DevLog.Log("FaceGen Lines Found: " + modData.ModName);
                foreach (FormHandler form in modData.CompactedModFormList)
                {
                    //IEnumerable<string> FaceGenTexFilePaths = Directory.EnumerateFiles(
                    //    Path.Combine(dataStartPath, "Textures\\Actors\\Character\\FaceGenData\\FaceTint\\", modData.ModName),
                    //    "*" + form.OriginalFormID + "*.dds",
                    //    SearchOption.AllDirectories);
                    string FaceTintFilePath = Path.Combine(dataStartPath, "Textures\\Actors\\Character\\FaceGenData\\FaceTint\\", modData.ModName + "\\00" + form.OriginalFormID + ".dds");
                    //foreach (string FaceTintFilePath in FaceGenTexFilePaths)
                    if(File.Exists(FaceTintFilePath))
                    {
                        GF.WriteLine(GF.stringLoggingData.OriganalPath + FaceTintFilePath, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);

                        string newStartPath = Path.Combine(GF.Settings.OutputFolder, "Textures\\Actors\\Character\\FaceGenData\\FaceTint\\" + form.ModName);
                        Directory.CreateDirectory(newStartPath);
                        string newPath = Path.Combine(newStartPath, "00" + form.CompactedFormID + ".dds");

                        File.Copy(FaceTintFilePath, newPath, true);
                        GF.WriteLine(GF.stringLoggingData.NewPath + newPath);
                    }

                    //IEnumerable<string> FaceGenFilePaths = Directory.EnumerateFiles(
                    //    Path.Combine(dataStartPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom\\", modData.ModName),
                    //    "*" + form.OriginalFormID + "*.nif",
                    //    SearchOption.AllDirectories);
                    string FaceGenFilePath = Path.Combine(dataStartPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom\\", modData.ModName + "\\00" + form.OriginalFormID + ".nif");
                    //foreach (string FaceGenFilePath in FaceGenFilePaths)
                    if(File.Exists(FaceGenFilePath))
                    {
                        GF.WriteLine(GF.stringLoggingData.OriganalPath + FaceGenFilePath, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);

                        NifFileWrapper OrigonalNifFile = new NifFileWrapper(FaceGenFilePath);
                        OrigonalNifFile = PatchNif(OrigonalNifFile, form.OriginalFormID, modData.ModName, form.CompactedFormID, form.ModName);

                        string newStartPath = Path.Combine(GF.Settings.OutputFolder, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom\\" + form.ModName);
                        Directory.CreateDirectory(newStartPath);
                        string newPath = Path.Combine(newStartPath, "00" + form.CompactedFormID + ".nif");

                        OrigonalNifFile.SaveAs(newPath, true);

                        GF.WriteLine(GF.stringLoggingData.NewPath + newPath);

                        //File.Copy(FaceGenFilePath, newPath, true);
                        //EditedFaceGen = true;
                        //using (StreamWriter stream = File.AppendText(GF.FaceGenFileFixPath))
                        //{
                        //    stream.WriteLine(Path.GetFullPath(newPath) + ";" + form.OriginalFormID + ";" + form.CompactedFormID);
                        //}
                    }

                }
            }
            return await Task.FromResult(1);
        }

        //Changes the OrigonalFormID to the CompactedFormID
        //mostly from https://github.com/Jampi0n/Skyrim-NifPatcher/blob/f71a5e5a532cf011790a978d20406b4a3208d856/NifPatcher/RuleParser.cs#L424
        public static NifFileWrapper PatchNif(NifFileWrapper nif, string OrgID, string OrgPluginName, string CompID, string CompPluginName)
        {
            for (var i = 0; i < nif.GetNumShapes(); ++i)
            {
                var shape = nif.GetShape(i);
                var subSurface = shape.SubsurfaceMap.ToLower();
                if (subSurface.Contains(OrgID, StringComparison.OrdinalIgnoreCase))
                {
                    subSurface = subSurface.Replace($"00{OrgID}.dds", $"00{CompID}.dds", StringComparison.OrdinalIgnoreCase);
                    subSurface = subSurface.Replace(OrgPluginName, CompPluginName);
                    shape.SubsurfaceMap = subSurface;
                }
            }
            return nif;
        }
        #endregion FaceGen Eslify

        //Region for Eslifying Data files
        #region ESLify Data Files
        //Data Files Eslify Main Menu
        private static void ESLifyDataFilesMainMenu()
        {
            GetESLifyModConfigurationFiles();

            Console.WriteLine(GF.stringLoggingData.InputDataFileExecutionPromt);
            Console.WriteLine($"1. {GF.stringLoggingData.ESLEveryModConfig}", true, false);
            Console.WriteLine($"2. {GF.stringLoggingData.SelectESLModConfig}", true, false);
            GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
            int selectedMenuItem;
            while (GF.WhileMenuSelect(2, out selectedMenuItem, 1) == false) ;
            switch (selectedMenuItem)
            {
                case -1:
                    GF.WriteLine(GF.stringLoggingData.ExitCodeInputOutput);
                    break;
                case 1:
                    ESLifyAllDataFiles();
                    break;
                case 2:
                    ESLifySelectedDataFilesMenu();
                    break;
                default:
                    break;
            }
            InternallyCodedDataFileConfigurations();
        }

        //Runs all Mod Configurations
        private static void ESLifyAllDataFiles()
        {
            foreach (var modConfiguration in BasicSingleModConfigurations)
            {
                HandleConfigurationType(modConfiguration);
            }
            foreach (var modConfiguration in BasicDirectFolderModConfigurations)
            {
                HandleConfigurationType(modConfiguration);
            }
            foreach (var modConfiguration in BasicDataSubfolderModConfigurations)
            {
                HandleConfigurationType(modConfiguration);
            }
            foreach (var modConfiguration in ComplexTOMLModConfigurations)
            {
                HandleConfigurationType(modConfiguration);
            }
            foreach (var modConfiguration in DelimitedFormKeysModConfigurations)
            {
                HandleConfigurationType(modConfiguration);
            }
        }

        //Runs the Internally coded Mod Configurations
        private static void InternallyCodedDataFileConfigurations()
        {
            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(GF.stringLoggingData.StartingRaceMenuESLify);
            RaceMenuESLify();

            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(GF.stringLoggingData.StartingCustomSkillsESLify);
            Task CustomSkills = CustomSkillsFramework();
            CustomSkills.Wait();
            CustomSkills.Dispose();

            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(GF.stringLoggingData.StartingOARESLify);
            OARESLify();
        }

        //Gets the Mod Configuration files for ESLifying Data Files
        private static void GetESLifyModConfigurationFiles()
        {
            //                 BasicSingleFile
            IEnumerable<string> basicSingleFilesModConfigurations = Directory.EnumerateFiles(
                    ".\\Properties\\DataFileTypes",
                    "*_BasicSingleFile.json",
                    SearchOption.TopDirectoryOnly);
            foreach (string file in basicSingleFilesModConfigurations)
            {
                try
                {
                    HashSet<BasicSingleFile> basicSingleFile = JsonSerializer.Deserialize<HashSet<BasicSingleFile>>(File.ReadAllText(file))!;
                    if (basicSingleFile != null)
                    {
                        foreach (BasicSingleFile modConfiguration in basicSingleFile)
                        {
                            if (modConfiguration.Enabled)
                            {
                                BasicSingleModConfigurations.Add(modConfiguration);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    GF.WriteLine(GF.stringLoggingData.ConfiguartionFileFailed + Path.GetFileName(file));
                }
            }
            //                 BasicDirectFolder
            IEnumerable<string> basicDirectFolderModConfigurations = Directory.EnumerateFiles(
                    ".\\Properties\\DataFileTypes",
                    "*_BasicDirectFolder.json",
                    SearchOption.TopDirectoryOnly);

            foreach (var file in basicDirectFolderModConfigurations)
            {
                try
                {
                    HashSet<BasicDirectFolder> basicDirectFolderFile = JsonSerializer.Deserialize<HashSet<BasicDirectFolder>>(File.ReadAllText(file))!;
                    if (basicDirectFolderFile != null)
                    {
                        foreach (BasicDirectFolder modConfiguration in basicDirectFolderFile)
                        {
                            if (modConfiguration.Enabled)
                            {
                                BasicDirectFolderModConfigurations.Add(modConfiguration);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    GF.WriteLine(GF.stringLoggingData.ConfiguartionFileFailed + Path.GetFileName(file));
                }
            }
            //                 BasicDataSubfolder
            IEnumerable<string> basicDataSubfolderModConfigurations = Directory.EnumerateFiles(
                    ".\\Properties\\DataFileTypes",
                    "*_BasicDataSubfolder.json",
                    SearchOption.TopDirectoryOnly);

            foreach (var file in basicDataSubfolderModConfigurations)
            {
                try
                {
                    HashSet<BasicDataSubfolder> basicDirectFolderFile = JsonSerializer.Deserialize<HashSet<BasicDataSubfolder>>(File.ReadAllText(file))!;
                    if (basicDirectFolderFile != null)
                    {
                        foreach (BasicDataSubfolder modConfiguration in basicDirectFolderFile)
                        {
                            if (modConfiguration.Enabled)
                            {
                                BasicDataSubfolderModConfigurations.Add(modConfiguration);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    GF.WriteLine(GF.stringLoggingData.ConfiguartionFileFailed + Path.GetFileName(file));
                }
            }
            //                 ComplexTOML
            IEnumerable<string> complexTOMLModConfigurations = Directory.EnumerateFiles(
                    ".\\Properties\\DataFileTypes",
                    "*_ComplexTOML.json",
                    SearchOption.TopDirectoryOnly);
            foreach (var file in complexTOMLModConfigurations)
            {
                try
                {
                    HashSet<ComplexTOML> basicDirectFolderFile = JsonSerializer.Deserialize<HashSet<ComplexTOML>>(File.ReadAllText(file))!;
                    if (basicDirectFolderFile != null)
                    {
                        foreach (ComplexTOML modConfiguration in basicDirectFolderFile)
                        {
                            if (modConfiguration.Enabled)
                            {
                                ComplexTOMLModConfigurations.Add(modConfiguration);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    GF.WriteLine(GF.stringLoggingData.ConfiguartionFileFailed + Path.GetFileName(file));
                }
            }
            //                 delimitedFormKeys
            IEnumerable<string> delimitedFormKeysModConfigurations = Directory.EnumerateFiles(
                    ".\\Properties\\DataFileTypes",
                    "*_DelimitedFormKeys.json",
                    SearchOption.TopDirectoryOnly);
            foreach (var file in delimitedFormKeysModConfigurations)
            {
                try
                {
                    HashSet<DelimitedFormKeys> basicDirectFolderFile = JsonSerializer.Deserialize<HashSet<DelimitedFormKeys>>(File.ReadAllText(file))!;
                    if (basicDirectFolderFile != null)
                    {
                        foreach (DelimitedFormKeys modConfiguration in basicDirectFolderFile)
                        {
                            if (modConfiguration.Enabled)
                            {
                                DelimitedFormKeysModConfigurations.Add(modConfiguration);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    GF.WriteLine(GF.stringLoggingData.ConfiguartionFileFailed + Path.GetFileName(file));
                }
            }
        }

        //Handles Logging for BasicSingleFile
        private static void HandleConfigurationType(BasicSingleFile ModConfiguration)
        {
            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(ModConfiguration.StartingLogLine);
            SingleBasicFile(ModConfiguration);
        }

        //Handles Logging for BasicDirectFolder
        private static void HandleConfigurationType(BasicDirectFolder ModConfiguration)
        {
            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(ModConfiguration.StartingLogLine);
            EnumDirectFolder(ModConfiguration);
        }

        //Handles Logging for BasicDataSubfolder
        private static void HandleConfigurationType(BasicDataSubfolder ModConfiguration)
        {
            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(ModConfiguration.StartingLogLine);
            Task t = EnumDataSubfolder(ModConfiguration);
            t.Wait();
            t.Dispose();
        }

        //Handles Logging for ComplexTOML
        private static void HandleConfigurationType(ComplexTOML ModConfiguration)
        {
            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(ModConfiguration.StartingLogLine);
            Task t = EnumToml(ModConfiguration);
            t.Wait();
            t.Dispose();
        }

        //Handles Logging for DelimitedFormKeys
        private static void HandleConfigurationType(DelimitedFormKeys ModConfiguration)
        {
            Console.WriteLine("\n\n\n\n");
            GF.WriteLine(ModConfiguration.StartingLogLine);
            EnumDelimitedFormKeys(ModConfiguration);
        }

        //Menu for selecting what Mod Configuration to check to run on Data Files
        private static void ESLifySelectedDataFilesMenu()
        {
            bool endMenu = false;
            string[] modConMenuList = GetModConList();
            string[] GetModConList()
            {
                List<string> modConMenuList = new List<string>();
                foreach (var modConfiguration in BasicSingleModConfigurations)
                {
                    modConMenuList.Add(modConfiguration.Name);
                }
                foreach (var modConfiguration in BasicDirectFolderModConfigurations)
                {
                    modConMenuList.Add(modConfiguration.Name);
                }
                foreach (var modConfiguration in BasicDataSubfolderModConfigurations)
                {
                    modConMenuList.Add(modConfiguration.Name);
                }
                foreach (var modConfiguration in ComplexTOMLModConfigurations)
                {
                    modConMenuList.Add(modConfiguration.Name);
                }
                foreach (var modConfiguration in DelimitedFormKeysModConfigurations)
                {
                    modConMenuList.Add(modConfiguration.Name);
                }
                return modConMenuList.ToArray();
            }
            if (modConMenuList.Length <= 0)
            {
                GF.WriteLine(GF.stringLoggingData.NoModConfigurationFilesFound);
                return;
            }

            do
            {
                Console.WriteLine("\n\n");
                Console.WriteLine(GF.stringLoggingData.ModConfigInputPrompt);
                Console.WriteLine($"0. {GF.stringLoggingData.SwitchToEverythingMenuItem}");
                for (int i = 0; i < modConMenuList.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {modConMenuList[i]}");
                }
                GF.WriteLine(GF.stringLoggingData.ExitCodeInput, true, false);
                if (GF.WhileMenuSelect(modConMenuList.Length + 1, out int selectedMenuItem, 0))
                {
                    if (selectedMenuItem == 0)
                    {
                        ESLifyAllDataFiles();
                        endMenu = true;
                    }
                    else if (selectedMenuItem == -1)
                    {
                        GF.WriteLine(GF.stringLoggingData.ExitCodeInputOutput);
                        endMenu = true;
                    }
                    else
                    {
                        RunSelectedModConfig(modConMenuList, selectedMenuItem -1);
                    }
                }


            } while (endMenu == false);
        }

        //Runs the selected Mod Configuration over Data Files
        private static void RunSelectedModConfig(string[] modConMenuList, int selectedMenuItem)
        {
            foreach (var modConfiguration in BasicSingleModConfigurations)
            {
                if (modConfiguration.Name.Equals(modConMenuList[selectedMenuItem]))
                {
                    HandleConfigurationType(modConfiguration);
                }
            }
            foreach (var modConfiguration in BasicDirectFolderModConfigurations)
            {
                if (modConfiguration.Name.Equals(modConMenuList[selectedMenuItem]))
                {
                    HandleConfigurationType(modConfiguration);
                }
            }
            foreach (var modConfiguration in BasicDataSubfolderModConfigurations)
            {
                if (modConfiguration.Name.Equals(modConMenuList[selectedMenuItem]))
                {
                    HandleConfigurationType(modConfiguration);
                }
            }
            foreach (var modConfiguration in ComplexTOMLModConfigurations)
            {
                if (modConfiguration.Name.Equals(modConMenuList[selectedMenuItem]))
                {
                    HandleConfigurationType(modConfiguration);
                }
            }
            foreach (var modConfiguration in DelimitedFormKeysModConfigurations)
            {
                if (modConfiguration.Name.Equals(modConMenuList[selectedMenuItem]))
                {
                    HandleConfigurationType(modConfiguration);
                }
            }

        }
        #endregion ESLify Data Files

        //Region for RaceMenu Eslify
        private static void RaceMenuESLify()
        {
            if (Directory.Exists(Path.Combine(GF.Settings.DataFolderPath, "SKSE\\Plugins\\CharGen\\Presets")))
            {
                string FixDecimalValue(string line, string CompactedFormID)
                {
                    string[] arr = line.Split(':');
                    string decStr = arr[1].Replace(",", "").Trim();
                    string inGameFormID = string.Format("{0:x}", long.Parse(decStr)).Substring(0, 2) + CompactedFormID;
                    string compDec = Convert.ToInt64(inGameFormID, 16).ToString();
                    return line.Replace(decStr, compDec);
                }

                IEnumerable<string> jslotFiles = Directory.EnumerateFiles(
                    Path.Combine(GF.Settings.DataFolderPath, "SKSE\\plugins\\CharGen\\Presets"),
                    "*.jslot",
                    SearchOption.AllDirectories);
                foreach (string jslotFile in jslotFiles)
                {
                    bool changed = false;
                    GF.WriteLine(GF.stringLoggingData.RaceMenuFileAt + jslotFile, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    string[] jslotFileLines = File.ReadAllLines(jslotFile);
                    for (int i = 0; i < jslotFileLines.Length; i++)
                    {
                        if (jslotFileLines[i].Contains("\"formIdentifier\"", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (string modName in CompactedModDataD.Keys)
                            {
                                if (jslotFileLines[i].Contains(modName, StringComparison.OrdinalIgnoreCase))
                                {
                                    CompactedModData? mod;
                                    if (CompactedModDataD.TryGetValue(modName, out mod))
                                    {
                                        foreach (FormHandler form in mod.CompactedModFormList)
                                        {
                                            if (jslotFileLines[i].Contains(form.GetOriginalFormID(), StringComparison.OrdinalIgnoreCase))
                                            {
                                                GF.WriteLine(GF.stringLoggingData.OldLine + jslotFileLines[i], true, GF.Settings.VerboseFileLoging);
                                                jslotFileLines[i] = jslotFileLines[i].Replace(form.GetOriginalFormID(), form.GetCompactedFormID());
                                                jslotFileLines[i] = jslotFileLines[i].Replace(modName, form.ModName);
                                                GF.WriteLine(GF.stringLoggingData.NewLine + jslotFileLines[i], true, GF.Settings.VerboseFileLoging);

                                                GF.WriteLine(GF.stringLoggingData.OldLine + jslotFileLines[i - 1], true, GF.Settings.VerboseFileLoging);
                                                jslotFileLines[i - 1] = FixDecimalValue(jslotFileLines[i - 1], form.CompactedFormID);
                                                GF.WriteLine(GF.stringLoggingData.NewLine + jslotFileLines[i - 1], true, GF.Settings.VerboseFileLoging);
                                                changed = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    OuputDataFileToOutputFolder(changed, jslotFile, jslotFileLines, GF.stringLoggingData.RaceMenuFileUnchanged);

                }
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + Path.Combine(GF.Settings.DataFolderPath, "SKSE\\plugins\\CharGen\\Presets"));
            }
        }

        //Region for Custom Skills Framework Eslify
        private static async Task<int> CustomSkillsFramework()
        {
            string startSearchPath = Path.Combine(GF.Settings.DataFolderPath, "NetScriptFramework\\Plugins");
            if (Directory.Exists(startSearchPath))
            {
                IEnumerable<string> customSkillConfigs = Directory.EnumerateFiles(
                    startSearchPath,
                    "CustomSkill*config.txt",
                    SearchOption.TopDirectoryOnly);
                foreach (string customSkillConfig in customSkillConfigs)
                {
                    GF.WriteLine(GF.stringLoggingData.CustomSkillsFileAt + customSkillConfig, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);

                    #region OldCustSkillsFrameWork_NotWorkingWell_integration
                    //bool changed = false;
                    //string[] customSkillConfigFile = File.ReadAllLines(customSkillConfig);
                    //string[] newCustomSkillConfigFile = new string[customSkillConfigFile.Length];
                    //string currentModName = "";
                    //int currentModNameLine = -1;
                    //CompactedModData currentMod = new CompactedModData();

                    //for (int i = 0; i < customSkillConfigFile.Length; i++)
                    //{
                    //    string line = customSkillConfigFile[i];
                    //    foreach (string modName in CompactedModDataD.Keys)
                    //    {
                    //        if (customSkillConfigFile[i].Contains(modName, StringComparison.OrdinalIgnoreCase))
                    //        {
                    //            currentModName = modName;
                    //            currentModNameLine = i;
                    //            CompactedModDataD.TryGetValue(modName, out currentMod!);
                    //            GF.WriteLine("", GF.Settings.VerboseConsoleLoging, false);
                    //            GF.WriteLine(GF.stringLoggingData.ModLine + line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    //        }
                    //    }
                    //    if (!currentModName.Equals(""))
                    //    {
                    //        foreach (FormHandler form in currentMod.CompactedModFormList)
                    //        {
                    //            if (line.Contains(form.GetOriginalFormID()))
                    //            {
                    //                GF.WriteLine(GF.stringLoggingData.OldLine + line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    //                line = line.Replace(form.GetOriginalFormID(), form.GetCompactedFormID());
                    //                customSkillConfigFile[currentModNameLine] = customSkillConfigFile[currentModNameLine].Replace(currentModName, form.ModName);
                    //                GF.WriteLine(GF.stringLoggingData.NewLine + line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    //                currentModName = "";
                    //                changed = true;
                    //            }
                    //        }
                    //    }
                    //    newCustomSkillConfigFile[i] = line;
                    //}
                    #endregion OldCustSkillsFrameWork_NotWorkingWell_integration
                    CustomSkillsFramework customSkillsFramework = new(File.ReadAllLines(customSkillConfig));

                    customSkillsFramework.UpdateFileLines();

                    OuputDataFileToOutputFolder(customSkillsFramework.ChangedFile, customSkillConfig, customSkillsFramework.FileLines, GF.stringLoggingData.CustomSkillsFileUnchanged);
                }

            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + startSearchPath);
            }
            return await Task.FromResult(0);
        }

        private static void OARESLify()
        {
            IEnumerable<string> openAnimationReplacerFolders = Directory.GetDirectories(Path.Combine(GF.Settings.DataFolderPath, "Meshes"), 
                "OpenAnimationReplacer", 
                SearchOption.AllDirectories);
            foreach (string openAnimationReplacerFolder in openAnimationReplacerFolders)
            {
                IEnumerable<string> configFiles = Directory.GetDirectories(openAnimationReplacerFolder,
                    "config.json",
                    SearchOption.AllDirectories);
                foreach(string configFile in configFiles)
                {
                    GF.WriteLine(GF.stringLoggingData.OARFileFound + configFile);
                    string[] fileLines = File.ReadAllLines(configFile);
                    if(fileLines.Length == 1)
                    {
                        GF.WriteLine(GF.stringLoggingData.OARFileOneLineSkip);
                        continue;
                    }
                    bool changed = false;
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        string line = fileLines[i];
                        foreach (var modData in CompactedModDataD)
                        {
                            if (!line.Contains(modData.Key, StringComparison.OrdinalIgnoreCase)) continue;

                            string prevline = string.Empty;
                            string nextLine = string.Empty;
                            if (i != 0) prevline = fileLines[i - 1];
                            if(fileLines.Length-1 != i) nextLine = fileLines[i + 1];
                            bool foundThisLine = false;
                            foreach (var formHandler in modData.Value.CompactedModFormList)
                            {
                                Regex regex = new Regex($"\"formID\"[ 0]*:[ 0]*\"[0]*{formHandler.GetOriginalFormID()}\"");
                                if(!prevline.Equals(string.Empty))
                                {
                                    Match match = regex.Match(prevline);
                                    if (match.Success)
                                    {
                                        GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i - 1].Trim());
                                        fileLines[i - 1] = fileLines[i - 1].Replace(formHandler.GetOriginalFormID(), formHandler.GetCompactedFormID(true));
                                        GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i - 1].Trim());
                                        changed = true;
                                        foundThisLine = true;
                                    }
                                }

                                if (!line.Contains("\"formID\"", StringComparison.OrdinalIgnoreCase))
                                {
                                    Match match = regex.Match(line);
                                    if (match.Success)
                                    {
                                        GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i].Trim());
                                        fileLines[i] = fileLines[i].Replace(formHandler.GetOriginalFormID(), formHandler.GetCompactedFormID(true));
                                        GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i].Trim());
                                        changed = true;
                                        foundThisLine = true;
                                    }
                                }

                                if (!nextLine.Equals(string.Empty))
                                {
                                    Match match = regex.Match(nextLine);
                                    if (match.Success)
                                    {
                                        GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i + 1].Trim());
                                        fileLines[i + 1] = fileLines[i + 1].Replace(formHandler.GetOriginalFormID(), formHandler.GetCompactedFormID(true));
                                        GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i + 1].Trim());
                                        changed = true;
                                        foundThisLine = true;
                                    }
                                }
                            }

                            if (!foundThisLine)
                            {
                                GF.WriteLine(GF.stringLoggingData.OARErrorLine1 + modData.Key);
                                GF.WriteLine(GF.stringLoggingData.OARErrorLine2 + i);
                                GF.WriteLine(GF.stringLoggingData.OARErrorLine3);
                                GF.WriteLine(GF.stringLoggingData.OARErrorLine4);
                            }
                        }
                    }
                    OuputDataFileToOutputFolder(changed, configFile, fileLines, GF.stringLoggingData.OARFileUnchanged);




                }
            }
        }

        //Region for fixing records and references inside of plugins
        #region Plugins
        //Starts the chack for Master file and runs what was selected in SelectCompactedModsMenu()
        private static void ReadLoadOrder()
        {
            HashSet<string> checkPlugins = SelectCompactedModsMenu();

            HashSet<string> runPlugins = new HashSet<string>();
            for (int i = 1; i < ActiveLoadOrder.Length; i++)
            {
                if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, ActiveLoadOrder[i])))
                {
                    if (!GF.IgnoredPlugins.Contains(ActiveLoadOrder[i]))
                    {
                        GF.WriteLine(String.Format(GF.stringLoggingData.PluginCheckMod, ActiveLoadOrder[i]), GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, ActiveLoadOrder[i])))
                        {
                            MasterReferenceCollection? masterCollection = MasterReferenceCollection.FromPath(Path.Combine(GF.Settings.DataFolderPath, ActiveLoadOrder[i]), GameRelease.SkyrimSE);
                            foreach (var master in masterCollection.Masters.ToHashSet())
                            {
                                if (checkPlugins.Contains(master.Master.FileName))
                                {
                                    GF.WriteLine(String.Format(GF.stringLoggingData.PluginAttemptFix, ActiveLoadOrder[i]));
                                    runPlugins.Add(ActiveLoadOrder[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
                GF.WriteLine(String.Format(GF.stringLoggingData.ProcessedPluginsLogCount, i, ActiveLoadOrder.Length, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging));
            }

            //Fix and output plugins that still use uncompacted data
            foreach (string pluginToRun in runPlugins)
            {
                Task<int> handlePluginTask = HandleMod.HandleSkyrimMod(pluginToRun);
                handlePluginTask.Wait();
                switch (handlePluginTask.Result)
                {
                    case 0:
                        GF.WriteLine(pluginToRun + GF.stringLoggingData.PluginNotFound, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        break;
                    case 1:
                        GF.WriteLine(String.Format(GF.stringLoggingData.PluginFixed, pluginToRun));
                        break;
                    case 2:
                        GF.WriteLine(pluginToRun + GF.stringLoggingData.PluginNotChanged, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        break;
                    case 3:
                        GF.WriteLine(pluginToRun + GF.stringLoggingData.PluginMissingMasterFile);
                        break;
                    default:
                        GF.WriteLine(GF.stringLoggingData.PluginSwitchDefaultMessage);
                        break;
                }
                handlePluginTask.Dispose();
            }

        }

        //Menu to select Compacted Mod Data to check over load order
        private static HashSet<string> SelectCompactedModsMenu()
        {
            HashSet<string> slectedCompactedMods = new HashSet<string>();
            List<string> menuList = new List<string>();
            menuList.AddRange(CompactedModDataD.Keys);

            foreach(CompactedModData compactedModData in CompactedModDataD.Values)
            {
                if (!compactedModData.PreviouslyESLified)
                {
                    menuList.Remove(compactedModData.ModName);
                    slectedCompactedMods.Add(compactedModData.ModName);
                }
            }

            bool exit = false;
            int menuModifier = 3;//1 is for offsetting the 0. in the menu add one for each extra menu item.
            do
            {
                Console.WriteLine("\n\n");
                GF.WriteLine(GF.stringLoggingData.SelectCompactedModsMenuHeader);
                Console.WriteLine(GF.stringLoggingData.ExitCodeInput);
                Console.WriteLine("1. " + GF.stringLoggingData.RunAllPluginChecks);//menuModifier = 2
                Console.WriteLine("2. " + "Check the selected plugins");//menuModifier = 3
                for (int i = 0; i < menuList.Count; i++)
                {
                    Console.WriteLine($"{i + menuModifier}. {menuList.ElementAt(i)}");
                }
                Console.WriteLine();
                Console.WriteLine("1. " + GF.stringLoggingData.RunAllPluginChecks);
                Console.WriteLine("2. " + "Check the selected plugins");
                if (GF.WhileMenuSelect(menuList.Count + menuModifier, out int selectedMenuItem, 1))
                {
                    if (selectedMenuItem == -1)
                    {
                        GF.WriteLine(GF.stringLoggingData.ExitCodeInputOutput);
                        exit = true;
                    }
                    else if (selectedMenuItem == 1)
                    {
                        GF.WriteLine(GF.stringLoggingData.RunAllPluginChecks + GF.stringLoggingData.SingleWordSelected);
                        return CompactedModDataD.Keys.ToHashSet();
                    }
                    else if (selectedMenuItem == 2)
                    {
                        exit = true;
                    }
                    else
                    {
                        GF.WriteLine(menuList.ElementAt(selectedMenuItem - menuModifier) + GF.stringLoggingData.SingleWordSelected);
                        slectedCompactedMods.Add(menuList.ElementAt(selectedMenuItem - menuModifier));
                        menuList.RemoveAt(selectedMenuItem - menuModifier);
                    }
                    Console.WriteLine();
                }
            } while (exit == false);

            return slectedCompactedMods;
        }
        #endregion Plugins

        public static void MergifyBashTagsMenu()
        {
            GF.WriteLine(GF.stringLoggingData.AskToStartMergifyBashTags);
            GF.WriteLine(GF.stringLoggingData.PressYToStartMergifyBashTags);
            string input = Console.ReadLine() ?? "";
            GF.WriteLine(input, consoleLog: false);
            if (input.Equals("Y", StringComparison.OrdinalIgnoreCase)) RunMergifyBashTags();
        }

        public static void RunMergifyBashTags()
        {
            GF.WriteLine(GF.stringLoggingData.StartingMBT);
            Process p = new Process();
            p.StartInfo.FileName = "MergifyBashTags.exe";
            p.StartInfo.Arguments = $"\"{GF.Settings.DataFolderPath}\" \"{GF.Settings.LootAppDataFolder}\" \"{GF.Settings.OutputFolder}\" -np";
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

        }


        private static void FinalizeData()
        {
            GF.WriteLine(GF.stringLoggingData.FinalizingDataHeader);
            HashSet<string> mergeDatasNames = new HashSet<string>();
            foreach (CompactedModData compactedModData in CompactedModDataD.Values)
            {
                if (compactedModData.FromMerge)
                {
                    mergeDatasNames.Add(compactedModData.MergeName);
                }
                else
                {
                    compactedModData.PreviouslyESLified = true;
                    compactedModData.OutputModData(false, false);
                }
            }

            foreach (string mergeName in mergeDatasNames)
            {
                string path = Path.Combine(GF.CompactedFormsFolder, mergeName + GF.MergeCacheExtension);
                if (File.Exists(path))
                {
                    CompactedMergeData mergeData = JsonSerializer.Deserialize<CompactedMergeData>(File.ReadAllText(path))!;
                    mergeData.PreviouslyESLified = true;
                    mergeData.OutputModData(false);
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.WhyMustYouChangeMyStuff);
                    IEnumerable<string> compactedFormsModFiles = Directory.EnumerateFiles(
                        GF.CompactedFormsFolder,
                        "*" + GF.MergeCacheExtension,
                        SearchOption.AllDirectories);
                    foreach (string file in compactedFormsModFiles)
                    {
                        CompactedMergeData mergeData = JsonSerializer.Deserialize<CompactedMergeData>(File.ReadAllText(file))!;
                        if (mergeName.Equals(mergeData.MergeName))
                        {
                            File.Delete(file);
                            mergeData.PreviouslyESLified = true;
                            mergeData.OutputModData(false);
                            break;
                        }
                    }
                }
            }


        }

    }
}
