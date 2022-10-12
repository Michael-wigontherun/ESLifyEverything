using ESLifyEverything.FormData;
using System.Text;

namespace ESLifyEverything
{
    public static partial class Program
    {
        public static string[] FormInFileLineReader(string[] fileLines, out bool changed)
        {
            changed = false;
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(".esp", StringComparison.OrdinalIgnoreCase) || fileLines[i].Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (CompactedModData modData in CompactedModDataD.Values)
                    {
                        if (fileLines[i].Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (FormHandler form in modData.CompactedModFormList)
                            {
                                if (fileLines[i].Contains(form.GetOrigonalFormID(), StringComparison.OrdinalIgnoreCase))
                                {
                                    GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                    fileLines[i] = fileLines[i].Replace(form.GetOrigonalFormID(), form.GetCompactedFormID());
                                    GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            return fileLines;
        }

        public static string[] FormInFileLineReaderDecimal(string[] fileLines, out bool changed)
        {
            string FixLineToHex(string line)
            {
                string afterGetFormFromFile = line.Substring(line.IndexOf("GetFormFromFile(") + "GetFormFromFile(".Length);
                string stringDecimal = afterGetFormFromFile.Substring(0, afterGetFormFromFile.IndexOf(','));
                if(long.TryParse(stringDecimal, out long value))
                {
                    string hex = "0x" + string.Format("{0:x}", value);
                    line = line.Replace(stringDecimal, hex, StringComparison.OrdinalIgnoreCase);
                }
                return line;
            }
            
            changed = false;
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(".esp", StringComparison.OrdinalIgnoreCase) || fileLines[i].Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    if (fileLines[i].Contains("GetFormFromFile("))
                    {
                        fileLines[i] = FixLineToHex(fileLines[i]);
                    }
                    foreach (CompactedModData modData in CompactedModDataD.Values)
                    {
                        if (fileLines[i].Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (FormHandler form in modData.CompactedModFormList)
                            {
                                if (fileLines[i].Contains(form.GetOrigonalFormID(), StringComparison.OrdinalIgnoreCase))
                                {
                                    GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                    fileLines[i] = fileLines[i].Replace(form.GetOrigonalFormID(), form.CompactedFormID, StringComparison.OrdinalIgnoreCase);
                                    GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            }
            return fileLines;
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

        public static string[] DelimitedFormKeysInFileLineReader(string[] fileLines, string delimiter, out bool changed)
        {
            changed = false;
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(".esp", StringComparison.OrdinalIgnoreCase) || fileLines[i].Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (CompactedModData modData in CompactedModDataD.Values)
                    {
                        if (fileLines[i].Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                        {
                            string[] delimitedLine = fileLines[i].Split('|');
                            StringBuilder reconstructedLine = new StringBuilder();
                            bool lineChanged = false;
                            for (int a = 0; a < delimitedLine.Length; a++)
                            {
                                foreach (FormHandler form in modData.CompactedModFormList)
                                {
                                    if (delimitedLine[a].Contains(form.GetOrigonalFormID(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (delimitedLine[a].Contains(form.ModName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            delimitedLine[a] = delimitedLine[a].Replace(form.GetOrigonalFormID(), form.GetCompactedFormID());
                                            lineChanged = true;
                                            changed = true;
                                        }
                                    }
                                }
                            }
                            for (int b = 0; b < delimitedLine.Length; b++)
                            {
                                reconstructedLine.Append(delimitedLine[b]);
                                if (b < delimitedLine.Length - 1)
                                {
                                    reconstructedLine.Append('|');
                                }
                            }
                            if (lineChanged)
                            {
                                GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                fileLines[i] = reconstructedLine.ToString();
                                GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                            }
                        }
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
