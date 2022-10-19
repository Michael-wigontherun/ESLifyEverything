using System.Text.Json;
using System.Text.Json.Serialization;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins.Cache.Internals.Implementations;
using ESLifyEverythingGlobalDataLibrary;

namespace ESLifyEverything.FormData
{
    //Class to store Compacted Mod Data
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
                    using (ISkyrimModDisposableGetter mod = SkyrimMod.CreateFromBinaryOverlay(path, SkyrimRelease.SkyrimSE))
                    {
                        ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter>? recordsDict = mod.ToImmutableLinkCache();
                        
                        foreach (FormHandler form in CompactedModFormList)
                        {
                            if (!recordsDict.TryResolve(form.CreateCompactedFormKey(), out IMajorRecordGetter? rec))
                            {
                                DevLog.Log(form + " Not Found.");
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
                                    DevLog.Log(form.FormKey.ToString() + " out of bounds.");
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
                return false;
            }

            return true;
        }

        //Outputs the CompactedModData to a _ESlEverything.json file.
        //checkPreviousIfExists = true will reimport previous outputed _ESlEverything.json data if it exists
        public void OutputModData(bool write, bool checkPreviousIfExists)
        {
            bool ContainsFormID(FormHandler previousForm)
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
                            DevLog.Log(form, "Found and adding to compacted mod data.");
                            CompactedModFormList.Add(form);
                        }
                    }
                }
            }
            if (write) Write();
            GF.WriteLine(GF.stringLoggingData.OutputtingTo + CompactedFormPath);
            File.WriteAllText(CompactedFormPath, JsonSerializer.Serialize(this, GF.JsonSerializerOptions));
        }

        //Creates a FormKey Dictionary for Remaping FormLinks inside of plugins
        public Dictionary<FormKey, FormKey> ToDictionary()
        {
            Dictionary<FormKey, FormKey> result = new Dictionary<FormKey, FormKey>();

            foreach (FormHandler handler in CompactedModFormList)
            {
                result.TryAdd(handler.CreateOriginalFormKey(ModName), handler.CreateCompactedFormKey());
            }

            return result;
        }

        //Writes CompactedModData to log file
        public void Write()
        {
            GF.WriteLine(ModName, false, GF.Settings.VerboseFileLoging, GF.DevSettings.DevLoggingOverrideSome);
            GF.WriteLine(CompactedModFormList, false, GF.Settings.VerboseFileLoging, GF.DevSettings.DevLoggingOverrideSome);
        }
    }
}
