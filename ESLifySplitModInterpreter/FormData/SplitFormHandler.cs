using ESLifyEverythingGlobalDataLibrary.FormData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifySplitModInterpreter.FormData
{
    internal class SplitFormHandler : IFormHandler
    {
        public new bool IsModified { get; private set; } = false;

        public SplitFormHandler(string modName, string originalFormID)
        {
            this.ModName = modName;
            this.OriginalFormID = originalFormID;
        }

        public void SetChangedForm(string changedForm)
        {
            this.CompactedFormID = changedForm;
            if (!OriginalFormID.Equals(CompactedFormID))
            {
                IsModified = true;
            }
        }

        public void ChangedModName(string modName)
        {
            if (!ModName.Equals(modName))
            {
                this.ModName = modName;
                IsModified = true;
            }
        }
    }
}
