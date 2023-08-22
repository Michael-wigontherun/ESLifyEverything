using ESLifyEverything.PluginHandles;
using ESLifyEverythingGlobalDataLibrary;
using System.Text.Json;
using ESLifyEverythingGlobalDataLibrary.FormData;
using System.Text.Json.Serialization;

namespace ESLifyEverything.FormData
{
    public class CompactedMergeData : IMergeData
    {
        [JsonInclude]
        public new string MergeName { get; set; } = "";
        [JsonInclude]
        public new DateTime? LastModified { get; set; }
        [JsonInclude]
        public new HashSet<CompactedModData> CompactedModDatas = new HashSet<CompactedModData>();
        [JsonInclude]
        public new int? NewRecordCount;
        [JsonInclude]
        public new bool PreviouslyESLified { get; set; } = false;

        public CompactedMergeData() { }

        //Reads and caches any nessessary data from the merge.json
        public CompactedMergeData(string mergeJsonPath, out bool success)
        {
            try
            {
                JSONFile = JsonSerializer.Deserialize<MergeJSON>(File.ReadAllText(mergeJsonPath))!;
                MergeName = JSONFile.filename;
                if (!File.Exists(Path.Combine(GF.Settings.DataFolderPath, MergeName)))
                {
                    success = false;
                    return;
                }
                LastModified = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, this.MergeName));
                CompactedModDataD = new Dictionary<string, CompactedModData>();
                foreach (MergeJSONPlugin plugin in JSONFile.plugins)
                {
                    CompactedModDataD.TryAdd(plugin.filename, new CompactedModData(plugin.filename));
                }
                JSONFile = null;
                success = true;
            }
            catch(Exception e)
            {
                GF.WriteLine(e.Message);
                success = false;
            }
        }

        //Counts how many records are inside of a merge cache 
        public int CoundNewRecords()
        {
            int newRecordCount = 0;
            foreach(CompactedModData compactedModData in CompactedModDatas)
            {
                newRecordCount += compactedModData.CompactedModFormList.Count;
            }
            return newRecordCount;
        }

        //Checks the stored and the current file time of the merged plugin
        public bool AlreadyCached()
        {
            return LastModified.Equals(File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, this.MergeName)));
        }

        //Outputs the merge cache
        public void OutputModData(bool write)
        {
            if (write) Write();
            string outputPath = Path.Combine(GF.CompactedFormsFolder, MergeName + GF.MergeCacheExtension);
            GF.WriteLine(GF.stringLoggingData.OutputtingTo + outputPath);
            File.WriteAllText(outputPath, JsonSerializer.Serialize(this, GF.JsonSerializerOptions));
        }

        //Writes the merge cache to json if VerboseFileLogging is set to true
        public void Write()
        {
            GF.WriteLine(MergeName, false, GF.Settings.VerboseFileLoging, GF.DevSettings.DevLoggingOverrideSome);
            GF.WriteLine("LastModified: " + LastModified?.ToString() ?? "null", false, GF.Settings.VerboseFileLoging, GF.DevSettings.DevLoggingOverrideSome);
            foreach(CompactedModData compactedModData in CompactedModDatas)
            {
                compactedModData.Write();
            }
        }

        public void MergedPluginFixer()
        {
            GF.WriteLine("");
            GF.WriteLine("");
            GF.WriteLine("");
            GF.WriteLine("");
            GF.WriteLine("MergedPluginFixer: " + MergeName);

            foreach (CompactedModData modData in CompactedModDatas)
            {
                string baseModPath = Path.Combine(GF.Settings.DataFolderPath, modData.ModName);
                if (File.Exists(baseModPath)) continue;
                
                Task<int>? handlePlugin = null;
                try
                {
                    handlePlugin = HandleMod.HandleSkyrimMod(modData.ModName, GF.Settings.DataFolderPath);
                    handlePlugin.Wait();
                    switch (handlePlugin.Result)
                    {
                        case 0:
                            GF.WriteLine(modData.ModName + GF.stringLoggingData.PluginNotFound);
                            GF.WriteLine(String.Format("Please remerge the plugin: {0}", MergeName));
                            break;
                        case 1:
                            GF.WriteLine(String.Format(GF.stringLoggingData.PluginFixed, modData.ModName));
                            GF.WriteLine(String.Format("Please remerge the plugin: {0}", MergeName));
                            ESLify.EditedMergedPluginNeedsRebuild.Add(MergeName);
                            break;
                        case 2:
                            GF.WriteLine(modData.ModName + GF.stringLoggingData.PluginNotChanged, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseConsoleLoging);
                            break;
                        case 3:
                            GF.WriteLine(modData.ModName + GF.stringLoggingData.PluginNotChanged, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseConsoleLoging);
                            break;
                        default:
                            GF.WriteLine(GF.stringLoggingData.PluginSwitchDefaultMessage);
                            break;
                    }
                    handlePlugin.Dispose();
                }
                catch (Exception e)
                {
                    if (handlePlugin != null) handlePlugin.Dispose();
                    GF.WriteLine("Error reading " + modData.ModName);
                    GF.WriteLine(e.Message);
                    if (e.StackTrace != null) GF.WriteLine(e.StackTrace);

                }

            }

            DevLog.Pause($"After MergedPluginFixer {MergeName} Pause.", !GF.DevSettings.DisableMergeFixerPauses);

        }

        [JsonIgnore]
        public Dictionary<string, CompactedModData>? CompactedModDataD;

        [JsonIgnore]
        private MergeJSON? JSONFile;
        private class MergeJSON
        {
            
            public string name { get; set; } = String.Empty;
            public string filename { get; set; } = String.Empty;
            public string method { get; set; } = String.Empty;
            public List<string> loadOrder { get; set; } = new();
            public string archiveAction { get; set; } = String.Empty;
            public bool buildMergedArchive { get; set; } = false;
            public bool useGameLoadOrder { get; set; } = false;
            public bool handleFaceData { get; set; } = false;
            public bool handleVoiceData { get; set; } = false;
            public bool handleBillboards { get; set; } = false;
            public bool handleStringFiles { get; set; } = false;
            public bool handleTranslations { get; set; } = false;
            public bool handleIniFiles { get; set; } = false;
            public bool handleDialogViews { get; set; } = false;
            public bool copyGeneralAssets { get; set; } = false;
            public DateTime dateBuilt { get; set; } = DateTime.Now;
            public List<MergeJSONPlugin> plugins { get; set; } = new();
        }
        private class MergeJSONPlugin
        {
            public string filename { get; set; } = String.Empty;
            public string hash { get; set; } = String.Empty;
            public string dataFolder { get; set; } = String.Empty;
        }

        //out returns the CompactedMergeData assosiated with the plugin name if true, or null if it fails
        public static bool GetCompactedMergeDataFromMergeName(string mergeName, out CompactedMergeData? success)
        {
            string path = Path.Combine(GF.CompactedFormsFolder, mergeName, GF.MergeCacheExtension);
            if (File.Exists(path))
            {
                success = JsonSerializer.Deserialize<CompactedMergeData>(File.ReadAllText(path))!;
                return true;
            }
            else
            {
                success = null;
                return false;
            }
        }
    }

}
