using System.Text.Json.Serialization;

namespace ESLifyEverything.Properties.DataFileTypes
{
    public class BasicDirectFolder//_BasicDirectFolder.json
    {//InumDirectFolder(string startFolder, string fileNameFilter, string fileAtLogLine, string fileUnchangedLogLine, SearchOption seachLevel = SearchOption.TopDirectoryOnly)
        [JsonInclude]
        public bool Enabled = false;
        [JsonInclude]
        public string Name = String.Empty;
        [JsonInclude]
        public string StartFolder = String.Empty;
        [JsonInclude]
        public string FileNameFilter = String.Empty;
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
