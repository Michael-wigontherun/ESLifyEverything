using ESLifyEverything.PluginHandles;
using ESLifyEverythingGlobalDataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverything.FormData
{
    public class CompactedMergeData
    {
        [JsonInclude]
        public string MergeName { get; set; } = "";
        [JsonInclude]
        public DateTime? LastModified { get; set; }
        [JsonInclude]
        public HashSet<CompactedModData> CompactedModDatas = new HashSet<CompactedModData>();
        [JsonInclude]
        public int? NewRecordCount;
        [JsonInclude]
        public bool PreviouslyESLified { get; set; } = false;

        public CompactedMergeData() { }

        //Reads and caches any nessessary data from the merge.json
        public CompactedMergeData(string mergeJsonPath, out bool success)
        {
            success = false;
            try
            {
                success = true;
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
            }
            catch(Exception e)
            {
                GF.WriteLine(e.Message);
            }
        }

        //Counts how many records are inside of a merge cache 
        public int CoundNewRecords()
        {
            int newRecordCount = 0;
            foreach(CompactedModData compactedModData in CompactedModDatas)
            {
                newRecordCount += compactedModData.CompactedModFormList.Count();
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
                if(File.Exists(Path.Combine(GF.Settings.DataFolderPath, modData.ModName)))
                {
                    Task<int> handlePlugin = HandleMod.HandleSkyrimMod(modData.ModName, GF.Settings.DataFolderPath);
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
                            Program.EditedMergedPluginNeedsRebuild.Add(MergeName);
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
            }

            DevLog.Pause($"After MergedPluginFixer {MergeName} Pause.");

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
    }

}
