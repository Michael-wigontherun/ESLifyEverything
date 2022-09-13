using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverything.FormData
{
    public class CompactedModData
    {
        [JsonInclude]
        public string ModName = "";
        [JsonInclude]
        public List<FormHandler> CompactedModFormList = new List<FormHandler>();

        public CompactedModData() { }

        public CompactedModData(string modName) 
        {
            ModName = modName;
        }

        public CompactedModData(string modName, List<FormHandler> compactedModFormList)
        {
            ModName = modName;
            CompactedModFormList = compactedModFormList;
        }

        public void OutputModData()
        {
            Write();
            GF.WriteLine(GF.stringLoggingData.OutputtingTo + Path.Combine(Path.Combine(GF.Settings.OptionalOutputFolder, "CompactedForms"), ModName + "_ESlEverything.json"));
            File.WriteAllText(Path.Combine(Path.Combine(GF.Settings.OptionalOutputFolder, "CompactedForms"), ModName + "_ESlEverything.json"), 
                JsonSerializer.Serialize(this, GF.JsonSerializerOptions));
        }

        public CompactedModData GetModData(string modFileName) 
        {
            string filePath = Path.Combine(Path.Combine(GF.Settings.OptionalOutputFolder, "CompactedForms"), modFileName + "_ESlEverything.json");
            GF.WriteLine(GF.stringLoggingData.GetCompDataLog + filePath);

            Import(JsonSerializer.Deserialize<CompactedModData>(File.ReadAllText(filePath))!);

            return this;
        }

        public void Import(CompactedModData json)
        {
            ModName = json.ModName;
            CompactedModFormList = json.CompactedModFormList;
        }

        public void Write()
        {
            Console.WriteLine();
            GF.WriteLine(ModName, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
            GF.WriteLine(CompactedModFormList, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
            Console.WriteLine();
        }
    }
}
