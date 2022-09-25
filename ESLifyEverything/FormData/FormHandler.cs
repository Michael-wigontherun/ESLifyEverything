using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverything.FormData
{
    public class FormHandler
    {
        [JsonInclude]
        public string ModName { get; private set; } = "";
        [JsonInclude]
        public string OrigonalFormID { get; private set; } = "000000";
        [JsonInclude]
        public string CompactedFormID { get; private set; } = "000000";
        [JsonInclude]
        public bool IsModified { get; private set; } = true;

        public FormHandler(){}

        public FormHandler(string xEditLogCompactedLine)
        {
            CreateCompactedForm(xEditLogCompactedLine);
        }

        public string GetOrigonalFormID()
        {
            return OrigonalFormID.TrimStart('0');
        }

        public string GetCompactedFormID()
        {
            int len = 6 - GetOrigonalFormID().Length;
            if (len > 3)
            {
                len = 3;
            }
            return CompactedFormID.Substring(len);
        }

        public void CreateCompactedForm(string xEditLogCompactedLine)
        {
            string logLineFilter = GF.stringsResources.xEditCompactedFormFilter;
            OrigonalFormID = xEditLogCompactedLine.Substring(xEditLogCompactedLine.IndexOf(logLineFilter) + logLineFilter.Length + 2, 6);

            xEditLogCompactedLine = xEditLogCompactedLine.Substring(xEditLogCompactedLine.IndexOf('"') + 1);
            ModName = xEditLogCompactedLine.Substring(0, xEditLogCompactedLine.IndexOf('"'));

            CompactedFormID = xEditLogCompactedLine.Substring(xEditLogCompactedLine.IndexOf('[') + 3, 6);

            if (OrigonalFormID.Equals(CompactedFormID))
            {
                IsModified = false;
            }
        }

        public new string ToString()
        {
            return "Mod Name: " + ModName + " | Origonal FormID: " + OrigonalFormID + " | Compacted FormID: " + CompactedFormID + " | IsModified: " + IsModified;
        }
    }
}
