using ESLifyEverything.FormData;
using System.Text;
using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;
using System.Text.RegularExpressions;

namespace ESLifyEverything
{
    public static partial class Program
    {
        //Parses Data Files with a Separator
        public static string[] FormInFileLineReader(string[] fileLines, Separator? SeparatorData, out bool changed)
        {
            changed = false;
            Console.WriteLine(fileLines[0]);
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
                                if (fileLines[i].Contains(form.GetOriginalFormID(), StringComparison.OrdinalIgnoreCase))
                                {
                                    bool lineChanged = false;
                                    if(SeparatorData != null)
                                    {
                                        if (fileLines[i].Contains(SeparatorData.FormKeySeparator))
                                        {
                                            string line = fileLines[i];

                                            if (SeparatorData.IDIsSecond)
                                            {
                                                line = RemoveExtraFormHex(fileLines[i], form.OriginalFormID, SeparatorData.FormKeySeparator);
                                            }

                                            string orgFormKey = form.GetOriginalFileLineFormKey(SeparatorData, modData.ModName);
                                            if (line.Contains(orgFormKey))
                                            {
                                                GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);

                                                fileLines[i] = line.Replace(
                                                    form.GetOriginalFileLineFormKey(SeparatorData, modData.ModName),
                                                    form.GetCompactedFileLineFormKey(SeparatorData),
                                                    StringComparison.OrdinalIgnoreCase);

                                                GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                                lineChanged = true;
                                                changed = true;
                                            }
                                        }
                                    }

                                    if(!lineChanged)
                                    {
                                        GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                        fileLines[i] = fileLines[i].Replace(form.GetOriginalFormID(), form.GetCompactedFormID(), StringComparison.OrdinalIgnoreCase);
                                        fileLines[i] = fileLines[i].Replace(modData.ModName, form.ModName, StringComparison.OrdinalIgnoreCase);
                                        GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                        changed = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return fileLines;
        }

        //Parses Script files
        public static string[] FormInScriptFileLineReader(string[] fileLines, out bool changed)
        {
            string FixLineToHex(string line, out string? exactHexValueTrimmed)
            {
                exactHexValueTrimmed = null;
                Regex fullStringCheck = new Regex("GetFormFromFile\\([0-9]+,", RegexOptions.IgnoreCase);
                if (fullStringCheck.IsMatch(line))
                {
                    string stringDecimal = fullStringCheck.Match(line).Value;
                    stringDecimal = stringDecimal.Replace("GetFormFromFile(", "");
                    stringDecimal = stringDecimal.Replace(",", "");
                    //GF.WriteLine(line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    try
                    {
                        if (long.TryParse(stringDecimal, out long value))
                        {
                            string hexNum = string.Format("{0:x}", value);
                            if (hexNum.Length > 6)
                            {
                                hexNum = hexNum.Substring(hexNum.Length - 6).TrimStart('0');
                                exactHexValueTrimmed = hexNum;
                            }
                            string hex = "0x" + hexNum;
                            line = line.Replace(stringDecimal, hex, StringComparison.OrdinalIgnoreCase);
                        }
                    }
                    catch (Exception e)
                    {
                        GF.WriteLine(GF.stringLoggingData.FixDecToHexError);
                        GF.WriteLine(line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        GF.WriteLine(e.Message);
                    }
                }
                return line;
            }
            
            changed = false;
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(".esp", StringComparison.OrdinalIgnoreCase) || fileLines[i].Contains(".esm", StringComparison.OrdinalIgnoreCase))
                {
                    string? exactHexValueTrimmed = null;
                    if (fileLines[i].Contains("GetFormFromFile(", StringComparison.OrdinalIgnoreCase))
                    {
                        fileLines[i] = FixLineToHex(fileLines[i], out exactHexValueTrimmed);
                    }
                    foreach (CompactedModData modData in CompactedModDataD.Values)
                    {
                        if (fileLines[i].Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (FormHandler form in modData.CompactedModFormList)
                            {
                                if(exactHexValueTrimmed != null)
                                {
                                    if (exactHexValueTrimmed.Equals(form.GetOriginalFormID(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                        fileLines[i] = fileLines[i].Replace(form.GetOriginalFormID(), form.GetCompactedFormID(), StringComparison.OrdinalIgnoreCase);
                                        fileLines[i] = fileLines[i].Replace(modData.ModName, form.ModName, StringComparison.OrdinalIgnoreCase);
                                        GF.WriteLine(GF.stringLoggingData.NewLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                        changed = true;
                                    }
                                }
                                else if (fileLines[i].Contains(form.GetOriginalFormID(), StringComparison.OrdinalIgnoreCase))
                                {
                                    GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[i], true, GF.Settings.VerboseFileLoging);
                                    fileLines[i] = fileLines[i].Replace(form.GetOriginalFormID(), form.GetCompactedFormID(), StringComparison.OrdinalIgnoreCase);
                                    fileLines[i] = fileLines[i].Replace(modData.ModName, form.ModName, StringComparison.OrdinalIgnoreCase);
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

        //Parses Toml Arrays in Data Files
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
                                if (fileLines[a].Contains(form.GetOriginalFormID(), StringComparison.OrdinalIgnoreCase))
                                {
                                    GF.WriteLine(GF.stringLoggingData.OldLine + fileLines[a], true, GF.Settings.VerboseFileLoging);
                                    fileLines[a] = fileLines[a].Replace(form.GetOriginalFormID(), form.GetCompactedFormID());
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

        //Parses Data Files with Delimiter
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
                            string[] delimitedLine = fileLines[i].Split(delimiter);
                            StringBuilder reconstructedLine = new StringBuilder();
                            bool lineChanged = false;
                            for (int a = 0; a < delimitedLine.Length; a++)
                            {
                                foreach (FormHandler form in modData.CompactedModFormList)
                                {
                                    if (delimitedLine[a].Contains(form.GetOriginalFormID(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (delimitedLine[a].Contains(modData.ModName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            delimitedLine[a] = delimitedLine[a].Replace(form.GetOriginalFormID(), form.GetCompactedFormID(), StringComparison.OrdinalIgnoreCase);
                                            delimitedLine[a] = delimitedLine[a].Replace(modData.ModName, form.ModName, StringComparison.OrdinalIgnoreCase);
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
                                    reconstructedLine.Append(delimiter);
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

        //Removes the extra hex data infront of the FormID and after the separator
        public static string RemoveExtraFormHex(string line, string orgFormID, string separator)
        {
            line = line.Replace(orgFormID, orgFormID.TrimStart('0'));
            orgFormID = orgFormID.TrimStart('0');
            if (separator.Length > 1)
            {
                string startString = line.Substring(line.IndexOf(separator) + separator.Length);
                if (startString.Equals(orgFormID))
                {
                    return line;
                }
                int i = startString.IndexOf(orgFormID);
                if (i <= 0)
                {
                    return line;
                }

                return line.Replace(startString.Substring(0, i), "");
            }
            
            return line;
        }

        //Outputs the Data file to the OutputFolder
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
