using ESLifyEverything.FormData;

namespace ESLifyEverything
{
    public static partial class Program
    {
        public static string[] FormInFileLineReader(string[] fileLines, out bool changed)
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
                                    GF.WriteLine(GF.stringLoggingData.OldLine + line, true, GF.Settings.VerboseFileLoging);
                                    line = line.Replace(form.GetOrigonalFormID(), form.GetCompactedFormID());
                                    GF.WriteLine(GF.stringLoggingData.NewLine + line, true, GF.Settings.VerboseFileLoging);
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

        public static string[] FormInTomlArray(string[] fileLines, string[] arrayFilters, out bool changed)
        {
            bool ContainsArrayFilter(string line)
            {
                foreach (string tomlArrayFilter in arrayFilters)
                {
                    if (line.Contains(tomlArrayFilter, StringComparison.OrdinalIgnoreCase)) 
                        return true;
                }
                return false;
            }

            changed = false;
            string currentPlugin = "";
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (ContainsArrayFilter(fileLines[i]))
                {
                    currentPlugin = FindPlugin();
                    string FindPlugin()//local method
                    {
                        for (int a = i + 1; a < fileLines.Length; a++)
                        {
                            if (ContainsArrayFilter(fileLines[a]))
                            {
                                return "";
                            }
                            foreach (string modName in CompactedModDataD.Keys)
                            {
                                if (fileLines[a].Contains(modName, StringComparison.OrdinalIgnoreCase))
                                {
                                    GF.WriteLine("", GF.Settings.VerboseConsoleLoging, false);
                                    GF.WriteLine(GF.stringLoggingData.ModLine + fileLines[a], true, GF.Settings.VerboseFileLoging);
                                    return modName;
                                }
                            }
                        }
                        return "";
                    }// end local method

                    CompactedModData? mod;
                    if (CompactedModDataD.TryGetValue(currentPlugin, out mod))
                    {
                        for (int a = i + 1; a < fileLines.Length; a++)
                        {
                            //Console.WriteLine(fileLines[a]);
                            if (ContainsArrayFilter(fileLines[a]))
                            {
                                break;
                            }
                            foreach (FormHandler form in mod.CompactedModFormList)
                            {
                                if (fileLines[a].Contains(form.GetOrigonalFormID(), StringComparison.OrdinalIgnoreCase))
                                {
                                    GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[a], true, GF.Settings.VerboseFileLoging);
                                    fileLines[a] = fileLines[a].Replace(form.GetOrigonalFormID(), form.GetCompactedFormID());
                                    GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[a], true, GF.Settings.VerboseFileLoging);
                                    changed = true;
                                }
                            }
                        }
                        currentPlugin = "";
                    }
                }
            }

            return fileLines;
        }

        public static void CopyFormFile(FormHandler form, string origonalDataStartPath, string OrgFilePath, out string newPath)
        {
            GF.WriteLine(GF.stringLoggingData.OriganalPath + OrgFilePath, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);

            newPath = GF.FixOuputPath(OrgFilePath.Replace(form.OrigonalFormID, form.CompactedFormID, StringComparison.OrdinalIgnoreCase), origonalDataStartPath, GF.Settings.OutputFolder);

            GF.WriteLine(GF.stringLoggingData.NewPath + newPath);

            Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));

            File.Copy(OrgFilePath, newPath, true);
        }

        public static void OuputDataFileToOutputFolder(bool changed, string origonalFilePath, string[] newFileLinesArr, string unchangedLogLine)
        {
            if (changed == true)
            {
                string newPath = GF.FixOuputPath(origonalFilePath);
                Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                File.WriteAllLines(newPath, newFileLinesArr);
                GF.WriteLine(GF.stringLoggingData.FileChanged + newPath);
            }
            else
            {
                GF.WriteLine(unchangedLogLine + origonalFilePath, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
            }
        }

    }
}
