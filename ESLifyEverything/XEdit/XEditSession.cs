using ESLifyEverything.FormData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverything.XEdit
{
    public class XEditSession
    {
        public string SessionTimeStamp { get; set; } = "";
        public List<string> CompactedSessionText = new List<string>();

        //Generates the list of forms from the found changed forms in log during the session
        public List<FormHandler> GenerateCompactedFormsList()
        {
            List<FormHandler> GeneratedForms = new List<FormHandler>();

            foreach (string line in CompactedSessionText)
            {
                GeneratedForms.Add(new FormHandler(line));
            }

            return GeneratedForms;
        }

        //Creates and outputs the found Compacted Mod Data
        public void GenerateCompactedModDatas()
        {
            Console.WriteLine();
            GF.WriteLine(GF.stringLoggingData.GeneratingJson);
            List<FormHandler> GeneratedForms = GenerateCompactedFormsList();
            Dictionary<string, CompactedModData> compactedModsSessionData = new Dictionary<string, CompactedModData>();
            CompactedModData tempData;
            foreach (FormHandler form in GeneratedForms)
            {
                if (compactedModsSessionData.TryGetValue(form.ModName, out tempData!))
                {
                    GF.WriteLine(GF.stringLoggingData.NewFormForMod + form.ModName, GF.Settings.VerboseConsoleLoging);
                    GF.WriteLine(GF.stringLoggingData.NewForm + form.ToString(), GF.Settings.VerboseConsoleLoging);
                    tempData.CompactedModFormList.Add(form);
                    compactedModsSessionData[form.ModName] = tempData;
                }
                else
                {
                    GF.WriteLine(GF.stringLoggingData.NewMod + form.ModName, GF.Settings.VerboseConsoleLoging);
                    tempData = new CompactedModData(form.ModName);
                    tempData.CompactedModFormList.Add(form);
                    GF.WriteLine(GF.stringLoggingData.NewFormForMod + form.ModName, GF.Settings.VerboseConsoleLoging);
                    GF.WriteLine(GF.stringLoggingData.NewForm + form.ToString(), GF.Settings.VerboseConsoleLoging);
                    compactedModsSessionData.Add(form.ModName, tempData);
                }
            }
            Console.WriteLine();
            GF.WriteLine(GF.stringLoggingData.OuputingJson);
            foreach(CompactedModData modData in compactedModsSessionData.Values)
            {
                modData.OutputModData(true, true);
            }
        }
    }

}
