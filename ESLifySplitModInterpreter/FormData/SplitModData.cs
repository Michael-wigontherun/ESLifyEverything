using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.FormData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifySplitModInterpreter.FormData
{
    internal class SplitModData : IPluginData
    {
        [JsonInclude]
        public new HashSet<SplitFormHandler> CompactedModFormList { get; set; } = new HashSet<SplitFormHandler>();

        public SplitModData(string modName)
        {
            ModName = modName;
        }

        //Outputs the CompactedModData to a _ESlEverything.json file.
        //checkPreviousIfExists = true will reimport previous outputed _ESlEverything.json data if it exists
        public override void OutputModData(bool write, bool checkPreviousIfExists = false)
        {
            string SplitFormDataPath = Path.Combine(GF.CompactedFormsFolder, ModName + GF.ModSplitDataExtension);
            if (write) Write();

            string pluginPath = Path.Combine(GF.Settings.DataFolderPath, ModName);
            if (File.Exists(pluginPath))
            {
                this.PluginLastModifiedValidation = File.GetLastWriteTime(pluginPath);
                GF.WriteLine(GF.stringLoggingData.OutputtingTo + SplitFormDataPath);
                File.WriteAllText(SplitFormDataPath, JsonSerializer.Serialize(this, GF.JsonSerializerOptions));
            }
            else
            {
                GF.WriteLine(string.Format(GF.stringLoggingData.PluginNotFoundImport, ModName));
                GF.WriteLine(string.Format(GF.stringLoggingData.SkippingOutput, ModName + GF.ModSplitDataExtension));
            }

        }

        //Writes CompactedModData to log file
        public override void Write()
        {
            GF.WriteLine(ModName, false, true, GF.DevSettings.DevLoggingOverrideSome);
            foreach (SplitFormHandler handler in CompactedModFormList)
            {
                GF.WriteLine(handler.ToString(), false, true, GF.DevSettings.DevLoggingOverrideSome);
            }
        }
    }
}
