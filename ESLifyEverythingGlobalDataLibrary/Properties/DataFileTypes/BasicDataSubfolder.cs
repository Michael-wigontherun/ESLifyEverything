using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes
{
    public class BasicDataSubfolder//_BasicDataSubfolder.json
    {//InumDataSubfolder(BasicDataSubfolder)
        [JsonInclude]
        public bool Enabled = false;
        [JsonInclude]
        public string Name = String.Empty;
        [JsonInclude]
        public string StartDataSubFolder = String.Empty;
        [JsonInclude]
        public string DirectoryFilter = String.Empty;
        [JsonInclude]
        public Separator? SeparatorData = null;
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
