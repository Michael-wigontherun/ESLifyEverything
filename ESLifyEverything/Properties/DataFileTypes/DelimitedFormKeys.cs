using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverything.Properties.DataFileTypes
{
    public class DelimitedFormKeys
    {//EnumDelimitedFormKeys(DelimitedFormKeys)
        [JsonInclude]
        public bool Enabled = false;
        [JsonInclude]
        public string Name = String.Empty;
        [JsonInclude]
        public string StartFolder = String.Empty;
        [JsonInclude]
        public string FileNameFilter = String.Empty;
        [JsonInclude]
        public string Delimiter = String.Empty;
        [JsonInclude]
        public string StartingLogLine = String.Empty;
        [JsonInclude]
        public string FileAtLogLine = String.Empty;
        [JsonInclude]
        public string FileUnchangedLogLine = String.Empty;
        [JsonInclude]
        public SearchOption SeachLevel = SearchOption.TopDirectoryOnly;
    }
}
