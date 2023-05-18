using ICSharpCode.SharpZipLib.Zip;
using Mutagen.Bethesda.Skyrim;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MergifyBashTags
{
    public static partial class Program
    {
        public static void ReadLoot(string lootAppDataPath)
        {
            string[] BreakKeywords = new string[] { "group:", "dirty:", "clean:", "msg:", "clean:", "url:", "req:", "after:", "inc:" };
            bool BreakKeyword(string l)
            {
                foreach (string s in BreakKeywords)
                {
                    if (l.Contains(s, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            string[] lootMetaData = File.ReadAllLines(lootAppDataPath);

            bool read = false;
            bool readTags = false;
            PluginTags plugin = new PluginTags();
            for(int i = 0; i < lootMetaData.Length; i++)
            {
                if (lootMetaData[i].Contains("plugins:", StringComparison.OrdinalIgnoreCase)) 
                {
                    read = true;
                    continue;
                }
                if (!read) continue;

                string line = lootMetaData[i];

                if (line.Contains("name:", StringComparison.OrdinalIgnoreCase))
                {
                    AddOrUpdatePluginTags(plugin);
                    plugin = new PluginTags();
                    Regex regex = new Regex("'[^']*'", RegexOptions.IgnoreCase);
                    Match? r = regex.Match(line);
                    if (r.Success)
                    {
                        string val = r.Value;
                        val = val.Remove(0, 1);
                        val = val.Remove(val.Length - 1);
                        plugin.Name = Path.GetFileNameWithoutExtension(val);
                        //Console.WriteLine(plugin.Name);
                        continue;
                    }
                }

                if (BreakKeyword(line))
                {
                    readTags = false;
                    continue;
                }

                if (line.Contains("tag:", StringComparison.OrdinalIgnoreCase))
                {
                    readTags = true;
                    Regex regex = new Regex(@"\[[^\]]*\]", RegexOptions.IgnoreCase);
                    Match? r = regex.Match(line);
                    if (r.Success)
                    {
                        string t = r.Value.Replace("-", "");
                        t = t.Replace("[", "");
                        t = t.Replace("]", "");
                        t = t.Trim();
                        plugin.Tags.Add(t);
                        readTags = false;
                    }
                    continue;
                }
                
                if (readTags)
                {
                    string l = line.Trim();
                    try{
                        plugin.Tags.Add(l.Remove(0, 2));
                    }catch (Exception) { //Console.WriteLine(line); 
                    }
                }
            }
        }

        public static void ReadBashTags(string DataFolderPath)
        {
            string path = Path.Combine(DataFolderPath, "BashTags");
            if (!Directory.Exists(path)) return;
            IEnumerable<string> bashTags = Directory.GetFiles(path);

            foreach (string bashTag in bashTags)
            {
                string[] bashTagFile = File.ReadAllLines(bashTag);
                PluginTags plugin = new PluginTags();
                plugin.Name = Path.GetFileNameWithoutExtension(bashTag);
                for (int i = 0; i < bashTagFile.Length; i++)
                {
                    string[] tags = bashTagFile[i].Split(',');
                    plugin.ImportAllTags(tags);
                }
                AddOrUpdatePluginTags(plugin);
            }
        }

        private static void ReadMergeJson(string DataFolderPath)
        {
            string ExtractFileName(string line)
            {
                Regex lineRegex = new Regex("\"filename\": \"[^\"]*\"", RegexOptions.IgnoreCase);
                Match? lr = lineRegex.Match(line);
                if (lr.Success)
                {
                    Regex regex = new Regex("\"[^\"]*\"", RegexOptions.IgnoreCase);
                    var r = regex.Matches(line);
                    //Console.WriteLine(r[1].Value);
                    return r[1].Value.Replace("\"", "");
                }
                return String.Empty;
            }
            //DataFolderPath = @"E:\SkyrimMods\MO2\mods\RandomThingsMerge";
            IEnumerable<string> mergefolders = Directory.GetDirectories(DataFolderPath, "merge -*", SearchOption.TopDirectoryOnly);
            foreach (string mergeFolder in mergefolders)
            {
                string mergeJsonPath = Path.Combine(mergeFolder, "merge.json");
                string[] mergeJson = File.ReadAllLines(mergeJsonPath);
                bool read = false;
                string name = "";
                List<string> mergedList = new List<string>();
                for (int i = 0; i < mergeJson.Length; i++)
                {
                    string line = mergeJson[i];
                    if (!read)
                    {
                        string n = ExtractFileName(line);
                        if (!n.Equals(String.Empty)) name = n;
                    }

                    if (line.Contains("plugins", StringComparison.OrdinalIgnoreCase))
                    {
                        read = true;
                        continue;
                    }
                    if (line.Contains(']'))
                    {
                        read = false;
                        continue;
                    }

                    if (read)
                    {
                        string n = ExtractFileName(line);
                        if (!n.Equals(String.Empty))
                        {
                            mergedList.Add(n);
                        }
                    }
                }

                if (Merges.ContainsKey(name))
                {
                    Merges[name] = mergedList;
                }
                else
                {
                    Merges.Add(name, mergedList);
                }
            }
        }

        public static bool CheckPluginHeadersSwitch = true;
        public static void CheckPluginHeaders(string DataFolderPath)
        {
            foreach (var mergeList in Merges)
            {
                foreach (string plugin in mergeList.Value)
                {
                    string pluginPath = Path.Combine(DataFolderPath, plugin);
                    using (ISkyrimModDisposableGetter? mod = SkyrimMod.CreateFromBinaryOverlay(pluginPath, SkyrimRelease.SkyrimSE))
                    {
                        if (mod.ModHeader.Description != null)
                        {
                            Regex regex = new Regex("\\{\\{BASH:[^}]*\\}\\}", RegexOptions.IgnoreCase);
                            var match = regex.Match(mod.ModHeader.Description);
                            if (match.Success)
                            {
                                string bashtag = match.Value.Replace("{{BASH:", "");
                                bashtag = bashtag.Replace("}}", "");
                                PluginTags p = new PluginTags();
                                p.Name = Path.GetFileNameWithoutExtension(plugin);

                                WriteLine(p.Name, CheckPluginHeadersSwitch);
                                WriteLine(bashtag, CheckPluginHeadersSwitch);

                                p.ImportAllTags(bashtag.Split(','));
                                AddOrUpdatePluginTags(p);
                            }
                        }
                    }
                }
            }
        }

    }


}
