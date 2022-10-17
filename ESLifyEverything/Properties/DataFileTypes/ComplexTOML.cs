using System.Text.Json.Serialization;

namespace ESLifyEverything.Properties.DataFileTypes
{
    public class ComplexTOML//_ComplexTOML.json
    {//public static async Task<int> InumToml(string startFolder, string fileNameFilter, string arrayStartFilter,
        //string fileAtLogLine, string fileUnchangedLogLine, SearchOption searchLevel = SearchOption.TopDirectoryOnly)
        [JsonInclude]
        public bool Enabled = false;
        [JsonInclude]
        public string Name = String.Empty;
        [JsonInclude]
        public string StartFolder = String.Empty;
        [JsonInclude]
        public string FileNameFilter = String.Empty;
        [JsonInclude]
        public Separator? SeparatorData = null;
        [JsonInclude]
        public string[] ArrayStartFilters = new string[]
        {
            "[[]]",
            "[[]]"
        };
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
