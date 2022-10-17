using System.Text.Json.Serialization;

namespace ESLifyEverything.Properties.DataFileTypes
{
    public class BasicSingleFile//_BasicSingleFile.json
    {//public static void SingleBasicFile(string path, string fileAtLogLine, string fileUnchangedLogLine)
        [JsonInclude]
        public bool Enabled = false;
        [JsonInclude]
        public string Name = String.Empty;
        [JsonInclude]
        public string DataPath = String.Empty;
        [JsonInclude]
        public Separator? SeparatorData = null;
        [JsonInclude]
        public string StartingLogLine = String.Empty;
        [JsonInclude]
        public string FileAtLogLine = String.Empty;
        [JsonInclude]
        public string FileUnchangedLogLine = String.Empty;
    }
}
