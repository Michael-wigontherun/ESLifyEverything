using System.Text.Json.Serialization;

namespace ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes
{
    public class BasicDirectFolder//_BasicDirectFolder.json
    {//InumDirectFolder(BasicDirectFolder)
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
        public string StartingLogLine = String.Empty;
        [JsonInclude]
        public string FileAtLogLine = String.Empty;
        [JsonInclude]
        public string FileUnchangedLogLine = String.Empty;
        [JsonInclude]
        public SearchOption SeachLevel = SearchOption.TopDirectoryOnly;
    }
}
