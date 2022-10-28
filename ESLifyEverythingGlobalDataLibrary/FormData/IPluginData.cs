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
        public string ModName { get; set; } = String.Empty;
        [JsonInclude]
        public HashSet<IFormHandler> CompactedModFormList { get; set; } = new HashSet<IFormHandler>();
        [JsonInclude]
        public DateTime? PluginLastModifiedValidation { get; set; }

        public abstract void OutputModData(bool write, bool checkPreviousIfExists);

        public abstract void Write();
    }
}
