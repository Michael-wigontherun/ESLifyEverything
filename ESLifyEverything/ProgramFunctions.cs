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
                                if (line.Contains(form.OrigonalFormID.TrimStart('0'), StringComparison.OrdinalIgnoreCase))
                                {
                                    line = line.Replace(form.OrigonalFormID.TrimStart('0'), form.CompactedFormID.TrimStart('0'));
                                    GF.WriteLine(line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
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
        
        public static void CopyFormFile(FormHandler form, string OrgFilePath, out string newPath)
        {
            GF.WriteLine(GF.stringLoggingData.OriganalPath + OrgFilePath);
            //string newPath = GF.FixOuputPath(OrgFilePath.Replace(form.OrigonalFormID, form.CompactedFormID));
            newPath = GF.FixOuputPath(OrgFilePath.Replace(form.OrigonalFormID, form.CompactedFormID));
            GF.WriteLine(GF.stringLoggingData.NewPath + newPath);
            Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
            //newPath = newPath;
            File.Copy(OrgFilePath, newPath, true);
        }



    }
}
