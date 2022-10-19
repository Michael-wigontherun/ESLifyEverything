using System.Text.Json.Serialization;

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

        //Creates a string of the Object's data for logging usually
        public new string ToString()
        {
            return "Mod Name: " + ModName + " | Origonal FormID: " + OriginalFormID + " | Compacted FormID: " + CompactedFormID + " | IsModified: " + IsModified;
        }

    }
}
