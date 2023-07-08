
namespace MergifyBashTags
{
    public static partial class Program
    {
        public static Dictionary<string, PluginTags> PluginsTags = new Dictionary<string, PluginTags>();

        public static Dictionary<string, List<string>> Merges = new Dictionary<string, List<string>>();

        static void Main(string[] args)
        {
            bool noPause = false;
            try
            {
                File.CreateText("MergifyBashTagsLog.txt").Close();
                //args = new string[3];
                //args[0] = @"C:\Steam\steamapps\common\Skyrim Special Edition\Data";
                //args[1] = @"C:\Users\Micha\AppData\Local\LOOT";
                //args[2] = @"E:\SkyrimMods\MO2\mods\ESLify Everything";
                if (args.Length < 3)
                {
                    args = new string[2];
                    WriteLine("Please input Absolute Path to Data folder, then press enter: ");
                    args[0] = Console.ReadLine() ?? "";
                    WriteLine("Please input Absolute Path to Loot's metadata storage: ");
                    WriteLine("   It should be located at C:\\Users\\[User]\\AppData\\Loot");
                    args[1] = Console.ReadLine() ?? "";
                    WriteLine("Please Enter the folder to output the BashTags to: ");
                    args[2] = Console.ReadLine() ?? "";
                }


                {
                    bool arg1 = !Directory.Exists(args[0]);
                    bool arg2 = !Directory.Exists(args[1]);
                    bool arg3 = !Directory.Exists(args[2]);
                    if (arg1 || arg2 || arg3)
                    {
                        WriteLine($"Folders NOT found Datafolder=arg1: {arg1} LootFolder=arg2: {arg2} OutputFolder=arg3: {arg3}");
                        WriteLine("Press Enter to end...");
                        Console.ReadLine();
                        return;
                    }
                    for(int i = 3; i < args.Length; i++)
                    {
                        string arg = args[i];
                        if(arg.Equals("-np", StringComparison.OrdinalIgnoreCase))
                        {
                            noPause = true;
                        }
                    }
                }

                WriteLine("Reading masterlist.");
                string expectedLootPath = Path.Combine(args[1], @"games\Skyrim Special Edition\masterlist.yaml");
                if (File.Exists(expectedLootPath)) ReadLoot(expectedLootPath);
                else
                {
                    IEnumerable<string> args1 = Directory.GetFiles(args[1], "masterlist.yaml", SearchOption.AllDirectories);
                    foreach (string arg in args1)
                    {
                        ReadLoot(arg);
                    }
                }

                WriteLine("Reading userlist.");
                expectedLootPath = Path.Combine(args[1], @"games\Skyrim Special Edition\userlist.yaml");
                if (File.Exists(expectedLootPath)) ReadLoot(expectedLootPath);
                else
                {
                    IEnumerable<string> args1 = Directory.GetFiles(args[1], "userlist.yaml", SearchOption.AllDirectories);
                    foreach (string arg in args1)
                    {
                        ReadLoot(arg);
                    }
                }

                WriteLine("Reading data folder Bash Tags.");
                ReadBashTags(args[0]);
                WriteLine("Reading Merge.json's.");
                ReadMergeJson(args[0]);

                WriteLine("\n\n\n\n");
                WriteLine("Reading Checking plugin headers for bashtags.");
                CheckPluginHeaders(args[0]);

                WriteLine("\n\n\n\n");
                WriteLine("Outputting Bash Tags.");
                OutputBashTagSets(args[2]);
            }
            catch (Exception ex) { WriteLine(ex.Message); WriteLine(ex.StackTrace); }
            
            if (!noPause)
            {
                WriteLine("Press Enter to end...");
                Console.ReadLine();
            }
            
        }

        public static bool OutputBashTagSetsSwitch = true;
        public static void OutputBashTagSets(string OutputFolder)
        {
            string bashTag = Path.Combine(OutputFolder, "bashtags");
            Directory.CreateDirectory(bashTag);

            foreach (var mergeList in Merges)
            {
                WriteLine(mergeList.Key, OutputBashTagSetsSwitch);
                HashSet<string> tags = new HashSet<string>();
                foreach (string plugin in mergeList.Value)
                {
                    if (PluginsTags.TryGetValue(Path.GetFileNameWithoutExtension(plugin), out var pluginTags))
                    {
                        foreach(string tag in pluginTags.Tags)
                        {
                            tags.Add(tag);
                        }
                        
                    }
                }
                
                tags.Remove("Deactivate");
                tags.Remove("deactivate");

                string line = string.Join(",", tags.ToArray());

                if (line.Equals(String.Empty))
                {
                    WriteLine("No Bash Tags to apply. Skipping output.", OutputBashTagSetsSwitch);
                    WriteLine("", OutputBashTagSetsSwitch);
                    continue;
                }

                WriteLine(line, OutputBashTagSetsSwitch);
                string outputPath = Path.Combine(bashTag, Path.ChangeExtension(mergeList.Key, ".txt"));
                File.WriteAllText(outputPath, line);
                WriteLine("Output to: " + outputPath, OutputBashTagSetsSwitch);
                WriteLine("", OutputBashTagSetsSwitch);
            }
        }

        public static void AddOrUpdatePluginTags(PluginTags plugin)
        {
            if (!plugin.Name.Equals(String.Empty))
            {
                if (PluginsTags.ContainsKey(plugin.Name))
                {
                    PluginsTags[plugin.Name].ImportAllTags(plugin.Tags);
                }
                else
                {
                    PluginsTags.Add(plugin.Name, plugin);
                }
            }
        }


        public static void WriteLine(string? line, bool write = true)
        {
            if(write)
            {
                Console.WriteLine(line);
                File.AppendAllTextAsync("MergifyBashTagsLog.txt", line).Wait();
            }
        }
    }

    public class PluginTags
    {
        public string Name = String.Empty;
        public HashSet<string> Tags = new HashSet<string>();

        public void ImportAllTags(HashSet<string> tags)
        {
            foreach (string tag in tags)
            {
                Tags.Add(tag);
            }
        }

        public void ImportAllTags(string[] tags)
        {
            foreach (string tag in tags)
            {
                Tags.Add(tag);
            }
        }
    }

}