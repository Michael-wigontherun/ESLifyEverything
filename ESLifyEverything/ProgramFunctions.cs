using ESLifyEverything.FormData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverything
{
    public static partial class Program
    {
        
        public static string[] FormInFileReader(string[] fileLines, out bool changed)
        {
            changed = false;
            string[] lines = new string[fileLines.Length];
            for (int i = 0; i < fileLines.Length; i++)
            {
                string line = fileLines[i];
                if (line.Contains(".esp", StringComparison.OrdinalIgnoreCase) || line.Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (CompactedModData modData in CompactedModDataD.Values)
                    {
                        if (line.Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (FormHandler form in modData.CompactedModFormList)
                            {
                                if (line.Contains(form.GetOrigonalFormID(), StringComparison.OrdinalIgnoreCase))
                                {
                                    GF.WriteLine(GF.stringLoggingData.OldLine + line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                    line = line.Replace(form.GetOrigonalFormID(), form.GetCompactedFormID());
                                    GF.WriteLine(GF.stringLoggingData.NewLine + line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
                lines[i] = line;
            }
            return lines;
        }

        public static void CopyFormFile(FormHandler form, string origonalDataStartPath, string OrgFilePath, out string newPath)
        {
            GF.WriteLine(GF.stringLoggingData.OriganalPath + OrgFilePath);
            //string newPath = GF.FixOuputPath(OrgFilePath.Replace(form.OrigonalFormID, form.CompactedFormID));
            newPath = GF.FixOuputPath(OrgFilePath.Replace(form.OrigonalFormID, form.CompactedFormID, StringComparison.OrdinalIgnoreCase), origonalDataStartPath, GF.Settings.OutputFolder);
            GF.WriteLine(GF.stringLoggingData.NewPath + newPath);
            Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
            //newPath = newPath;
            File.Copy(OrgFilePath, newPath, true);
        }



    }
}
