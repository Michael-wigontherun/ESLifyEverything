using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FixCompactedModData
{
    internal class EvilFormHandler
    {
        [JsonInclude]
        public string ModName { get; protected set; } = "";
        [JsonInclude]
        public string OrigonalFormID { get; protected set; } = "000000";
        [JsonInclude]
        public string CompactedFormID { get; protected set; } = "000000";
        [JsonInclude]
        public bool IsModified { get; protected set; } = true;
    }

    internal class EvilCompactedModData
    {
        [JsonInclude]
        public bool Enabled { get; set; } = true;
        [JsonInclude]
        public string ModName = "";
        [JsonInclude]
        public HashSet<EvilFormHandler> CompactedModFormList = new HashSet<EvilFormHandler>();
        [JsonInclude]
        public DateTime? PluginLastModifiedValidation { get; set; }
        [JsonInclude]
        public bool Recheck { get; set; } = true;
    }

    internal class CorrectedFormHandler
    {
        public CorrectedFormHandler(EvilFormHandler evilFormHandler)
        {
            this.ModName = evilFormHandler.ModName;
            this.OriginalFormID = evilFormHandler.OrigonalFormID;
            this.CompactedFormID = evilFormHandler.CompactedFormID;
            this.IsModified = evilFormHandler.IsModified;
        }

        [JsonInclude]
        public string ModName { get; protected set; } = "";
        [JsonInclude]
        public string OriginalFormID { get; protected set; } = "000000";
        [JsonInclude]
        public string CompactedFormID { get; protected set; } = "000000";
        [JsonInclude]
        public bool IsModified { get; protected set; } = true;

    }

    internal class CorrectedCompactedModData
    {
        [JsonInclude]
        public bool Enabled { get; set; } = true;
        [JsonInclude]
        public string ModName = "";
        [JsonInclude]
        public HashSet<CorrectedFormHandler> CompactedModFormList = new HashSet<CorrectedFormHandler>();
        [JsonInclude]
        public DateTime? PluginLastModifiedValidation { get; set; }
        [JsonInclude]
        public bool Recheck { get; set; } = true;

        public CorrectedCompactedModData(EvilCompactedModData evilCompactedModData)
        {
            Enabled = evilCompactedModData.Enabled;
            ModName = evilCompactedModData.ModName;
            PluginLastModifiedValidation = evilCompactedModData.PluginLastModifiedValidation;
            Recheck = evilCompactedModData.Recheck;
            foreach(EvilFormHandler evilFormHandler in evilCompactedModData.CompactedModFormList)
            {
                CompactedModFormList.Add(new CorrectedFormHandler(evilFormHandler));
            }
        }
    }
}
