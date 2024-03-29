﻿using System.Text.Json.Serialization;

namespace ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes
{
    public class BasicSingleFile//_BasicSingleFile.json
    {//SingleBasicFile(BasicSingleFile)
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
