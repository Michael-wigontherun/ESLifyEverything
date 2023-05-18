using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ESLifyEverythingGlobalDataLibrary.FormData
{
    public class IMergeData
    {
        [JsonInclude]
        public string MergeName { get; set; } = "";
        [JsonInclude]
        public DateTime? LastModified { get; set; }
        [JsonInclude]
        public HashSet<IPluginData> CompactedModDatas = new HashSet<IPluginData>();
        [JsonInclude]
        public int? NewRecordCount;
        [JsonInclude]
        public bool PreviouslyESLified { get; set; } = false;
    }
}
