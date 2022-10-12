using System.Text.Json;
using System.Text.Json.Serialization;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins.Cache.Internals.Implementations;

namespace ESLifyEverything.FormData
{
    public class CompactedModData
    {
        [JsonInclude]
        public bool Enabled { get; set; } = true;
        [JsonInclude]
        public string ModName = "";
        [JsonInclude]
        public HashSet<FormHandler> CompactedModFormList = new HashSet<FormHandler>();
        [JsonInclude]
        public DateTime? PluginLastModifiedValidation { get; set; }
        [JsonInclude]
        public bool Recheck { get; set; } = true;

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
        
        public bool IsCompacted(bool useOutputLocation)
        {
            try
            {
                string path = Path.Combine(GF.Settings.DataFolderPath, ModName);

                if (useOutputLocation)
                {
                    if (!GF.Settings.ChangedPluginsOutputToDataFolder)
                    {
                        path = Path.Combine(GF.Settings.OutputFolder, ModName);
                    }
                }

                if (File.Exists(path))
                {
                    using (ISkyrimModDisposableGetter mod = SkyrimMod.CreateFromBinaryOverlay(path, SkyrimRelease.SkyrimSE))
                    {
                        ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter>? recordsDict = mod.ToImmutableLinkCache();
                        
                        foreach (FormHandler form in CompactedModFormList)
                        {
                            if (!recordsDict.TryResolve(form.CreateCompactedFormKey(), out IMajorRecordGetter? rec))
                            {
                                return false;
                            }
                        }
                        uint validMin = 0x000800;
                        uint validMax = 0x000fff;

                        foreach (IMajorRecordGetter? form in mod.EnumerateMajorRecords())
                        {
                            if (form.FormKey.ModKey.ToString().Equals(this.ModName))
                            {
                                if (form.FormKey.ID < validMin && form.FormKey.ID > validMax)
                                {
                                    return false;
                                }
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
            }

            return true;
        }

        public void Import(CompactedModData json)
        {
            ModName = json.ModName;
            CompactedModFormList = json.CompactedModFormList;
            PluginLastModifiedValidation = json.PluginLastModifiedValidation;
            Recheck = json.Recheck;
        }

        public void OutputModData(bool write, bool checkPreviousIfExists)
        {
            string CompactedFormPath = Path.Combine(GF.CompactedFormsFolder, ModName + "_ESlEverything.json");
            if (checkPreviousIfExists)
            {
                if (File.Exists(CompactedFormPath))
                {
                    CompactedModData previous = JsonSerializer.Deserialize<CompactedModData>(File.ReadAllText(CompactedFormPath))!;
                    
                    foreach (FormHandler form in previous.CompactedModFormList)
                    {
                        if (!this.ContainsOrigonalFormID(form))
                        {
                            CompactedModFormList.Add(form);
                        }
                    }
                }
            }
            if (write) Write();
            GF.WriteLine(GF.stringLoggingData.OutputtingTo + CompactedFormPath);
            File.WriteAllText(CompactedFormPath, JsonSerializer.Serialize(this, GF.JsonSerializerOptions));
        }

        private bool ContainsOrigonalFormID(FormHandler previousForm)
        {
            foreach (FormHandler form in CompactedModFormList)
            {
                if (form.OrigonalFormID.Equals(previousForm.OrigonalFormID))
                {
                    return true;
                }
            }
            return false;
        }

        public Dictionary<FormKey, FormKey> ToDictionary()
        {
            Dictionary<FormKey, FormKey> result = new Dictionary<FormKey, FormKey>();

            foreach (FormHandler handler in CompactedModFormList)
            {
                result.TryAdd(handler.CreateOrigonalFormKey(), handler.CreateCompactedFormKey());
            }

            return result;
        }

        public void Write()
        {
            GF.WriteLine(ModName, false, GF.Settings.VerboseFileLoging);
            GF.WriteLine(CompactedModFormList, false, GF.Settings.VerboseFileLoging);
        }
    }
}
