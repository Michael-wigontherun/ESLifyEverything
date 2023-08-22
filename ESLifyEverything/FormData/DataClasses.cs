using ESLifyEverythingGlobalDataLibrary;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ESLifyEverything.FormData
{
    public class CustomSkillsFramework
    {
        public class CustomSkillsFrameworkNodeData
        {
            public void Import(int LineNum, string Line, string removeBase)
            {
                if (removeBase.Contains("File", StringComparison.OrdinalIgnoreCase))
                {
                    FileLineNum = LineNum;
                    FileLine = Line;
                    FileName = FileLine.Replace(removeBase, "");
                    FileName = FileName.Replace("=", "");
                    FileName = FileName.Replace("\"", "");
                    FileName = FileName.Trim();
                }
                else
                {
                    IDLineNum = LineNum;
                    IDLine = Line;
                    IDString = IDLine.Replace(removeBase, "");
                    IDString = IDString.Replace("=", "");
                    IDString = IDString.Trim();
                    //TrimIDString();
                }
            }

            public int FileLineNum { get; set; } = -1;
            public string FileLine { get; set; } = string.Empty;
            public string FileName { get; set; } = string.Empty;

            public int IDLineNum { get; set; } = -1;
            public string IDLine { get; set; } = string.Empty;
            private string _IDString  = string.Empty;

            public string IDString 
            { 
                get { return _IDString; } 
                set {
                    string s = value;
                    s = s.Replace("0x", "");
                    if (s.Length > 6)
                    {
                        s = s[^6..];
                    }
                    _IDString = s.TrimStart('0');
                } 
            }
        }

        [JsonInclude]
        public string[] FileLines = Array.Empty<string>();

        [JsonInclude]
        public bool ChangedFile = false;

        public CustomSkillsFramework(string[] customSkillConfigFile)
        {
            FileLines = customSkillConfigFile;
            Regex nodeFileRegex = new("Node[0-9]*.PerkFile");
            Regex nodeIDRegex = new("Node[0-9]*.PerkId");
            for (int i = 0; i < FileLines.Length; i++)
            {
                string line = FileLines[i];

                string baseS = "LevelFile";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    Level.Import(i, line, baseS);
                    continue;
                }
                baseS = "LevelId";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    Level.Import(i, line, baseS);
                    continue;
                }
                baseS = "RatioFile";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    Ratio.Import(i, line, baseS);
                    continue;
                }
                baseS = "RatioId";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    Ratio.Import(i, line, baseS);
                    continue;
                }
                baseS = "ShowLevelupFile";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    ShowLevelup.Import(i, line, baseS);
                    continue;
                }
                baseS = "ShowLevelupId";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    ShowLevelup.Import(i, line, baseS);
                    continue;
                }
                baseS = "ShowMenuFile";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    ShowMenu.Import(i, line, baseS);
                    continue;
                }
                baseS = "ShowMenuId";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    ShowMenu.Import(i, line, baseS);
                    continue;
                }
                baseS = "PerkPointsFile";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    PerkPoints.Import(i, line, baseS);
                    continue;
                }
                baseS = "PerkPointsId";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    PerkPoints.Import(i, line, baseS);
                    continue;
                }
                baseS = "LegendaryFile";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    Legendary.Import(i, line, baseS);
                    continue;
                }
                baseS = "LegendaryId";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    Legendary.Import(i, line, baseS);
                    continue;
                }
                baseS = "ColorFile";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    Color.Import(i, line, baseS);
                    continue;
                }
                baseS = "ColorId";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    Color.Import(i, line, baseS);
                    continue;
                }
                baseS = "DebugReloadFile";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    DebugReload.Import(i, line, baseS);
                    continue;
                }
                baseS = "DebugReloadId";
                if (line.Contains(baseS, StringComparison.OrdinalIgnoreCase))
                {
                    DebugReload.Import(i, line, baseS);
                    continue;
                }

                Match nodeFileMatch = nodeFileRegex.Match(line);
                if (nodeFileMatch.Success)
                {
                    string matchValue = nodeFileMatch.Value;
                    matchValue = matchValue.Replace("Node", "");
                    matchValue = matchValue.Replace(".PerkFile", "");
                    int nodeNum = int.Parse(matchValue);//dictionary key

                    string[] arr = line.Split('=', StringSplitOptions.TrimEntries);
                    if (arr.Length != 2) continue;
                    if (!arr[0].Contains(".PerkFile", StringComparison.OrdinalIgnoreCase)) continue;

                    if (CustomSkillsFrameworkNodeDatas.ContainsKey(nodeNum))
                    {
                        CustomSkillsFrameworkNodeDatas[nodeNum].FileLineNum = i;
                        CustomSkillsFrameworkNodeDatas[nodeNum].FileLine = line;
                        CustomSkillsFrameworkNodeDatas[nodeNum].FileName = arr[1].Replace("\"", "");
                    }
                    else
                    {
                        CustomSkillsFrameworkNodeData customSkillsFrameworkNodeData = new()
                        {
                            FileLineNum = i,
                            FileLine = line,
                            FileName = arr[1].Replace("\"", "")
                        };

                        CustomSkillsFrameworkNodeDatas.Add(nodeNum, customSkillsFrameworkNodeData);
                    }

                    continue;
                }//node PerkFile

                Match nodeIDMatch = nodeIDRegex.Match(line);
                if (nodeIDMatch.Success)
                {
                    string matchValue = nodeIDMatch.Value;
                    matchValue = matchValue.Replace("Node", "");
                    matchValue = matchValue.Replace(".PerkId", "");
                    int nodeNum = int.Parse(matchValue);//dictionary key

                    string[] arr = line.Split('=', StringSplitOptions.TrimEntries);
                    if (arr.Length != 2) continue;
                    if (!arr[0].Contains(".PerkId", StringComparison.OrdinalIgnoreCase)) continue;

                    if (CustomSkillsFrameworkNodeDatas.ContainsKey(nodeNum))
                    {
                        CustomSkillsFrameworkNodeDatas[nodeNum].IDLineNum = i;
                        CustomSkillsFrameworkNodeDatas[nodeNum].IDLine = line;
                        CustomSkillsFrameworkNodeDatas[nodeNum].IDString = arr[1];
                    }
                    else
                    {
                        CustomSkillsFrameworkNodeData customSkillsFrameworkNodeData = new()
                        {
                            IDLineNum = i,
                            IDLine = line,
                            IDString = arr[1]
                        };

                        CustomSkillsFrameworkNodeDatas.Add(nodeNum, customSkillsFrameworkNodeData);
                    }

                    continue;
                }//node PerkID
            }

        }

        [JsonInclude]
        public CustomSkillsFrameworkNodeData Level = new();

        [JsonInclude]
        public CustomSkillsFrameworkNodeData Ratio = new();

        [JsonInclude]
        public CustomSkillsFrameworkNodeData ShowLevelup = new();

        [JsonInclude]
        public CustomSkillsFrameworkNodeData ShowMenu = new();

        [JsonInclude]
        public CustomSkillsFrameworkNodeData PerkPoints = new();

        [JsonInclude]
        public CustomSkillsFrameworkNodeData Legendary = new();

        [JsonInclude]
        public CustomSkillsFrameworkNodeData Color = new();

        [JsonInclude]
        public CustomSkillsFrameworkNodeData DebugReload = new();

        [JsonInclude]
        public Dictionary<int, CustomSkillsFrameworkNodeData> CustomSkillsFrameworkNodeDatas = new();

        public void UpdateFileLines()
        {
            UpdateFileLinesNode(Level);
            UpdateFileLinesNode(Ratio);
            UpdateFileLinesNode(ShowLevelup);
            UpdateFileLinesNode(ShowMenu);
            UpdateFileLinesNode(PerkPoints);
            UpdateFileLinesNode(Legendary);
            UpdateFileLinesNode(Color);
            UpdateFileLinesNode(DebugReload);
            foreach(var node in CustomSkillsFrameworkNodeDatas.Values)
            {
                UpdateFileLinesNode(node);
            }
        }

        private void UpdateFileLinesNode(CustomSkillsFrameworkNodeData node)
        {
            if (!ESLify.CompactedModDataD.TryGetValue(node.FileName, out CompactedModData? compactedModData)) return;
            foreach (var formHandler in compactedModData.CompactedModFormList)
            {
                if (formHandler.GetOriginalFormID().Equals(node.IDString))
                {
                    GF.WriteLine(GF.stringLoggingData.OldLine + node.FileLine);
                    FileLines[node.FileLineNum] = node.FileLine.Replace(node.FileName, formHandler.ModName);
                    GF.WriteLine(GF.stringLoggingData.NewLine + FileLines[node.FileLineNum]);

                    GF.WriteLine(GF.stringLoggingData.OldLine + node.IDLine);
                    FileLines[node.IDLineNum] = node.IDLine.Replace(formHandler.GetOriginalFormID(), formHandler.GetCompactedFormID(true));
                    GF.WriteLine(GF.stringLoggingData.NewLine + FileLines[node.IDLineNum]);

                    ChangedFile = true;
                    break;
                }
            }

        }

    }

    public class OBodyJson
    {
        [JsonInclude]
        public Dictionary<string, List<string>> npc = new();
        [JsonInclude]
        public Dictionary<string, List<string>> factionFemale = new();
        [JsonInclude]
        public Dictionary<string, List<string>> factionMale = new();
        [JsonInclude]
        public Dictionary<string, List<string>> npcPluginFemale = new();
        [JsonInclude]
        public Dictionary<string, List<string>> npcPluginMale = new();
        [JsonInclude]
        public Dictionary<string, List<string>> raceFemale = new();
        [JsonInclude]
        public Dictionary<string, List<string>> raceMale = new();
        [JsonInclude]
        public List<string> blacklistedNpcs = new();
        [JsonInclude]
        public List<string> blacklistedNpcsPluginFemale = new();
        [JsonInclude]
        public List<string> blacklistedNpcsPluginMale = new();
        [JsonInclude]
        public List<string> blacklistedRacesFemale = new();
        [JsonInclude]
        public List<string> blacklistedRacesMale = new();
        [JsonInclude]
        public List<string> blacklistedOutfitsFromORefit = new();
        [JsonInclude]
        public List<string> blacklistedOutfitsFromORefitPlugin = new();
        [JsonInclude]
        public List<string> outfitsForceRefit = new();
        [JsonInclude]
        public List<string> blacklistedPresetsFromRandomDistribution = new();
        [JsonInclude]
        public bool blacklistedPresetsShowInOBodyMenu = true;

        //Contain FormIDs
        [JsonInclude]
        public Dictionary<string, Dictionary<string, List<string>>> npcFormID = new();
        [JsonInclude]
        public Dictionary<string, List<string>> blacklistedNpcsFormID = new();
        [JsonInclude]
        public Dictionary<string, List<string>> blacklistedOutfitsFromORefitFormID = new();
        [JsonInclude]
        public Dictionary<string, List<string>> outfitsForceRefitFormID = new();

        public static OBodyJson? LoadOBodyJson(string path)
        {
            return JsonSerializer.Deserialize<OBodyJson>(File.ReadAllText(path));
        }
    }
}
