using System.Text.Json.Serialization;

namespace ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes
{
    //Separator class for handling FormKeys in Mod Configurations
    public class Separator
    {
        [JsonInclude]
        public string FormKeySeparator = String.Empty;
        [JsonInclude]
        public bool IDIsSecond = false;
        [JsonInclude]
        public bool ModNameAsString = false;
        [JsonInclude]
        public char ModNameStringCharater = '\"';
        [JsonInclude]
        public bool TrimStart = true;
        [JsonInclude]
        public bool UseRegex = false;
        [JsonInclude]
        public string RegexMakeUp = String.Empty;
        [JsonInclude]
        public string FormKeyMakeUp = String.Empty;
    }
}
