using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverythingGlobalDataLibrary.FormData
{
    public abstract class IFormHandler
    {
        [JsonInclude]
        public string ModName { get; protected set; } = "";
        [JsonInclude]
        public string OriginalFormID { get; protected set; } = "000000";
        [JsonInclude]
        public string CompactedFormID { get; protected set; } = "000000";
        [JsonInclude]
        public bool IsModified { get; protected set; } = true;

    }
}
