using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Masters;
using Noggog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESLifyEverything.PluginHandles
{
    public static class PluginData
    {
        //                  Plugin name
        public static Dictionary<string, Plugin> LoadOrderPlugins = new Dictionary<string, Plugin>();
        public static Dictionary<string, Plugin> CompactedSubPlugins = new Dictionary<string, Plugin>();

        public static bool Imported = false;

        public static void GetPluginData()
        {
            if (File.Exists(".\\Properties\\PluginsData.json"))
            {
                PluginJson pj = JsonSerializer.Deserialize<PluginJson>(File.ReadAllText(".\\Properties\\PluginsData.json"))!;
                LoadOrderPlugins = pj.LoadOrderPlugins;
                CompactedSubPlugins = pj.CompactedSubPlugins;
                Imported = true;
                GF.WriteLine(GF.stringLoggingData.PluginDataImport);
            }
            else
            {
                Program.NewOrUpdatedMods = true;
                GF.WriteLine(GF.stringLoggingData.PluginDataImportNotFound);
            }
        }

        public static void AddNew(string pluginName)
        {
            Plugin? plugin;
            if (LoadOrderPlugins.TryGetValue(pluginName, out plugin))
            {
                GF.WriteLine(pluginName + GF.stringLoggingData.PluginCheckPrev);
                if (!plugin.LastModified.Equals(File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, pluginName))))
                {
                    GF.WriteLine(pluginName + GF.stringLoggingData.PluginCheckUpdated);
                    LoadOrderPlugins.Remove(pluginName);
                    Plugin newPlugin = new Plugin(pluginName);
                    GF.WriteLine(pluginName + GF.stringLoggingData.PluginCheckModReimp);
                    LoadOrderPlugins.Add(pluginName, newPlugin);
                    Program.NewOrUpdatedMods = true;
                }
            }
            else
            {
                GF.WriteLine(pluginName + GF.stringLoggingData.PluginCheckNew);
                Plugin newPlugin = new Plugin(pluginName);
                LoadOrderPlugins.Add(pluginName, newPlugin);
                Program.NewOrUpdatedMods = true;
                GF.WriteLine(pluginName + GF.stringLoggingData.PluginCheckModImp);
            }
        }

        public static void Output()
        {
            PluginJson pj = new PluginJson(LoadOrderPlugins, CompactedSubPlugins);
            File.WriteAllText(".\\Properties\\PluginsData.json", JsonSerializer.Serialize(pj, GF.JsonSerializerOptions));
        }

        public static void UpdateCompactedData(Plugin plugin)
        {
            LoadOrderPlugins.Remove(plugin.PluginName);
            CompactedSubPlugins.Remove(plugin.PluginName);
            plugin.LastModified = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, plugin.PluginName));
            LoadOrderPlugins.Add(plugin.PluginName, plugin);
            CompactedSubPlugins.Add(plugin.PluginName, plugin);
        }

        public static void NewCompactedSubPlugin(Plugin plugin)
        {
            CompactedSubPlugins.Remove(plugin.PluginName);
            CompactedSubPlugins.Add(plugin.PluginName, plugin);
        }
    }

    public class PluginJson
    {
        public Dictionary<string, Plugin> LoadOrderPlugins = new Dictionary<string, Plugin>();
        public Dictionary<string, Plugin> CompactedSubPlugins = new Dictionary<string, Plugin>();

        public PluginJson()
        {
        }

        public PluginJson(Dictionary<string, Plugin> loadOrderPlugins, Dictionary<string, Plugin> compactedSubPlugins)
        {
            LoadOrderPlugins = loadOrderPlugins;
            CompactedSubPlugins = compactedSubPlugins;
        }
    }

    public class Plugin
    {
        public string PluginName { get; set; } = "";
        
        public DateTime LastModified { get; set; } = new DateTime();

        public HashSet<string> Masters { get; set; } = new HashSet<string>();

        public Plugin(string pluginName)
        {
            PluginName = pluginName;
            LastModified = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, PluginName));

            MasterReferenceCollection? masterCollection = MasterReferenceCollection.FromPath(Path.Combine(GF.Settings.DataFolderPath, PluginName), GameRelease.SkyrimSE);
            Masters = new HashSet<string>();
            foreach (var master in masterCollection.Masters.ToHashSet())
            {
                Masters.Add(master.Master.FileName);
            }
        }
    }
}
