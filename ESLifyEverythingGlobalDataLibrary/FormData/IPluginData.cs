using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverythingGlobalDataLibrary.FormData
{
    public abstract class IPluginData
    {
        [JsonInclude]
        public bool Enabled { get; set; } = true;
        [JsonInclude]
        public string ModName { get; set; } = String.Empty;
        [JsonInclude]
        public HashSet<IFormHandler> CompactedModFormList { get; set; } = new HashSet<IFormHandler>();
        [JsonInclude]
        public DateTime? PluginLastModifiedValidation { get; set; }
        [JsonInclude]
        public bool Recheck { get; set; } = true;
        [JsonInclude]
        public bool PreviouslyESLified { get; set; } = false;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? NotCompactedData { get; set; } = null;

        public abstract void OutputModData(bool write, bool checkPreviousIfExists);

        public abstract void Write();
    }
}
