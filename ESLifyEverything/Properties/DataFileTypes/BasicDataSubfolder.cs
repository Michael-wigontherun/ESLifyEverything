using System.Text.Json.Serialization;

namespace ESLifyEverything.Properties.DataFileTypes
{
    public class BasicDataSubfolder//_BasicDataSubfolder.json
    {//InumDataSubfolder(string subfolderStart, string directoryFilter, string fileFilter, string fileAtLogLine, string fileUnchangedLogLine, 
        //SearchOption startSeachLevel = SearchOption.AllDirectories, SearchOption secondSeachLevel = SearchOption.AllDirectories)
        [JsonInclude]
        public bool Enabled = false;
        [JsonInclude]
        public string Name = String.Empty;
        [JsonInclude]
        public string StartDataSubFolder = String.Empty;
        [JsonInclude]
        public string DirectoryFilter = String.Empty;
        [JsonInclude]
        public string FileFilter = String.Empty;
        [JsonInclude]
        public string StartingLogLine = String.Empty;
        [JsonInclude]
        public string FileAtLogLine = String.Empty;
        [JsonInclude]
        public string FileUnchangedLogLine = String.Empty;
        [JsonInclude]
        public SearchOption StartSeachLevel = SearchOption.AllDirectories;
        [JsonInclude]
        public SearchOption SubFolderSeachLevel = SearchOption.AllDirectories;
    }
}
