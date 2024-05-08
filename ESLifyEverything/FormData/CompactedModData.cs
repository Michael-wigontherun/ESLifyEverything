using System.Text.Json;
using System.Text.Json.Serialization;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins.Cache.Internals.Implementations;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.FormData;

namespace ESLifyEverything.FormData
{
    //Class to store Compacted Mod Data
    public class CompactedModData : IPluginData
    {
        [JsonInclude]
        public new bool Enabled { get; set; } = true;
        //[JsonInclude]
        //public string ModName = "";
        [JsonInclude]
        public new HashSet<FormHandler> CompactedModFormList = new();
        //[JsonInclude]
        //public DateTime? PluginLastModifiedValidation { get; set; }
        [JsonInclude]
        public new bool Recheck { get; set; } = true;
        [JsonInclude]
        public new bool PreviouslyESLified { get; set; } = false;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public new bool? NotCompactedData { get; set; } = null;

        public CompactedModData() { }

        public CompactedModData(string modName) 
        {
            ModName = modName;
        }
        
        public CompactedModData(string modName, HashSet<FormHandler> compactedModFormList)
        {
            ModName = modName;
            CompactedModFormList = compactedModFormList;
        }

        public void AddIfMissing(FormHandler form)
        {
            if (!ContainsFormID(form))
            {
                DevLog.Log(form, "Not found and adding to compacted mod data.");
                CompactedModFormList.Add(form);
            }
        }

        //Checks the plugin to see if it is still Compacted
        public bool IsCompacted(bool usePluginOutputLocation)
        {
            try
            {
                string path = Path.Combine(GF.Settings.DataFolderPath, ModName);

                if (usePluginOutputLocation)
                {
                    if (!GF.Settings.ChangedPluginsOutputToDataFolder)
                    {
                        path = Path.Combine(GF.Settings.OutputFolder, ModName);
                    }
                }

                if (File.Exists(path))
                {
                    using ISkyrimModDisposableGetter mod = SkyrimMod.CreateFromBinaryOverlay(path, SkyrimRelease.SkyrimSE);
                    ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter>? recordsDict = mod.ToImmutableLinkCache();

                    foreach (FormHandler form in CompactedModFormList)
                    {
                        if (recordsDict.TryResolve(form.CreateOriginalFormKey(ModName), out IMajorRecordGetter? rec))
                        {
                            DevLog.Log(form + " Not Found.");
                            return false;
                        }
                    }
                    uint validMin = 0x000000;
                    uint validMax = 0x000fff;

                    foreach (IMajorRecordGetter? form in mod.EnumerateMajorRecords())
                    {
                        if (form.FormKey.ModKey.ToString().Equals(this.ModName))
                        {
                            if (form.FormKey.ID < validMin || form.FormKey.ID > validMax)
                            {
                                DevLog.Log(form.FormKey.ToString() + " out of bounds.");
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                GF.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        //Outputs the CompactedModData to a _ESlEverything.json file.
        //checkPreviousIfExists = true will reimport previous outputed _ESlEverything.json data if it exists
        public override void OutputModData(bool write, bool checkPreviousIfExists)
        {
            
            string CompactedFormPath = Path.Combine(GF.CompactedFormsFolder, ModName + GF.CompactedFormExtension);
            if (checkPreviousIfExists)
            {
                if (File.Exists(CompactedFormPath))
                {
                    CompactedModData previous = JsonSerializer.Deserialize<CompactedModData>(File.ReadAllText(CompactedFormPath))!;
                    
                    foreach (FormHandler form in previous.CompactedModFormList)
                    {
                        if (!ContainsFormID(form))
                        {
                            DevLog.Log(form, "Not found and adding to compacted mod data.");
                            CompactedModFormList.Add(form);
                        }
                    }
                }
            }
            if (write) Write();
            GF.WriteLine(GF.stringLoggingData.OutputtingTo + CompactedFormPath);
            File.WriteAllText(CompactedFormPath, JsonSerializer.Serialize(this, GF.JsonSerializerOptions));
        }

        public bool ContainsFormID(FormHandler previousForm)
        {
            foreach (FormHandler form in CompactedModFormList)
            {
                if (form.OriginalFormID.Equals(previousForm.OriginalFormID))
                {
                    return true;
                }
                if (form.CompactedFormID.Equals(previousForm.CompactedFormID))
                {
                    return true;
                }
            }
            return false;
        }

        //Creates a FormKey Dictionary for Remaping FormLinks inside of plugins
        public Dictionary<FormKey, FormKey> ToDictionary()
        {
            Dictionary<FormKey, FormKey> result = new();

            foreach (FormHandler handler in CompactedModFormList)
            {
                result.TryAdd(handler.CreateOriginalFormKey(ModName), handler.CreateCompactedFormKey());
            }

            return result;
        }

        //Writes CompactedModData to log file
        public override void Write()
        {
            GF.WriteLine(ModName, false, GF.Settings.VerboseFileLoging, GF.DevSettings.DevLoggingOverrideSome);
            foreach(FormHandler handler in CompactedModFormList)
            {
                GF.WriteLine(handler.ToString(), false, GF.Settings.VerboseFileLoging, GF.DevSettings.DevLoggingOverrideSome);
            }
        }

        [JsonIgnore]
        public bool FromMerge { get; set; } = false;
        [JsonIgnore]
        public string MergeName { get; set; } = "";
    }
}
