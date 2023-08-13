using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MergifyBashTags
{
    
    public class Plugin
    {
        [JsonInclude]
        public string filename { get; set; } = "";
        [JsonInclude]
        public string hash { get; set; } = "";
        [JsonInclude]
        public string dataFolder { get; set; } = "";
    }

    public class MergeJson
    {
        [JsonInclude]
        public string name { get; set; } = "";
        [JsonInclude]
        public string filename { get; set; } = "";
        [JsonInclude]
        public string method { get; set; } = "";
        [JsonInclude]
        public List<string> loadOrder { get; set; }
        [JsonInclude]
        public string archiveAction { get; set; } = "";
        [JsonInclude]
        public bool buildMergedArchive { get; set; }
        [JsonInclude]
        public bool useGameLoadOrder { get; set; }
        [JsonInclude]
        public bool handleFaceData { get; set; }
        [JsonInclude]
        public bool handleVoiceData { get; set; }
        [JsonInclude]
        public bool handleBillboards { get; set; }
        [JsonInclude]
        public bool handleStringFiles { get; set; }
        [JsonInclude]
        public bool handleTranslations { get; set; }
        [JsonInclude]
        public bool handleIniFiles { get; set; }
        [JsonInclude]
        public bool handleDialogViews { get; set; }
        [JsonInclude]
        public bool copyGeneralAssets { get; set; }
        [JsonIgnore]
        public CustomMetadata customMetadata { get; set; }
        [JsonInclude]
        public DateTime dateBuilt { get; set; }
        [JsonInclude]
        public List<Plugin> plugins { get; set; } = new();
    }

    public class CustomMetadata
    {
    }
}
