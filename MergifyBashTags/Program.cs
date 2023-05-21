
namespace MergifyBashTags
{
    public static partial class Program
    {
        public static Dictionary<string, PluginTags> PluginsTags = new Dictionary<string, PluginTags>();

        public static Dictionary<string, List<string>> Merges = new Dictionary<string, List<string>>();

        static void Main(string[] args)
        {
            try
            {


                //args = new string[3];
                //args[0] = @"C:\Steam\steamapps\common\Skyrim Special Edition\Data";
                //args[1] = @"C:\Users\Micha\AppData\Local\LOOT";
                //args[2] = @"E:\SkyrimMods\MO2\mods\ESLify Everything";
                if (args.Length != 3)
                {
                    args = new string[2];
                    Console.WriteLine("Please input Absolute Path to Data folder, then press enter: ");
                    args[0] = Console.ReadLine() ?? "";
                    Console.WriteLine("Please input Absolute Path to Loot's metadata storage: ");
                    Console.WriteLine("   It should be located at C:\\Users\\[User]\\AppData\\Loot");
                    args[1] = Console.ReadLine() ?? "";
                    Console.WriteLine("Please Enter the folder to output the BashTags to: ");
                    args[2] = Console.ReadLine() ?? "";
                }

                {
                    bool arg1 = !Directory.Exists(args[0]);
                    bool arg2 = !Directory.Exists(args[1]);
                    bool arg3 = !Directory.Exists(args[2]);
                    if (arg1 || arg2 || arg3)
                    {
                        Console.WriteLine($"Folders not found arg1: {arg1} arg2: {arg2} arg3: {arg3}");
                        Console.ReadLine();
                        return;
                    }
                }

                Console.WriteLine("Reading masterlist.");
                ReadLoot(Path.Combine(args[1], @"games\Skyrim Special Edition\masterlist.yaml"));
                Console.WriteLine("Reading userlist.");
                ReadLoot(Path.Combine(args[1], @"games\Skyrim Special Edition\userlist.yaml"));
                Console.WriteLine("Reading data folder Bash Tags.");
                ReadBashTags(args[0]);
                Console.WriteLine("Reading Merge.json's.");
                ReadMergeJson(args[0]);

                Console.WriteLine("\n\n\n\n");
                Console.WriteLine("Reading Checking plugin headers for bashtags.");
                CheckPluginHeaders(args[0]);

                Console.WriteLine("\n\n\n\n");
                Console.WriteLine("Outputting Bash Tags.");
                OutputBashTagSets(args[2]);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            Console.WriteLine("Press Enter to end...");
            Console.ReadLine();
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


        public static void WriteLine(string line, bool write)
        {
            if(write) Console.WriteLine(line);
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