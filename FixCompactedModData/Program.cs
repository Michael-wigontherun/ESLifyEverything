using System.Text.Json;

namespace FixCompactedModData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(
                        ".\\CompactedForms",
                        "*_ESlEverything.json",
                        SearchOption.TopDirectoryOnly);
                foreach(string file in files)
                {
                    Console.WriteLine(Path.GetFullPath(file));
                    EvilCompactedModData evilCompactedModData = JsonSerializer.Deserialize<EvilCompactedModData>(File.ReadAllText(Path.GetFullPath(file)))!;
                    if (!ContainsNullData(evilCompactedModData))
                    {
                        File.WriteAllText($".\\CompactedForms\\{evilCompactedModData.ModName}_ESlEverything.json", JsonSerializer.Serialize(new CorrectedCompactedModData(evilCompactedModData), new JsonSerializerOptions() { WriteIndented = true }));
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
            }
        }

        public static bool ContainsNullData(EvilCompactedModData evilCompactedModData)
        {
            foreach(EvilFormHandler evilFormHandler in evilCompactedModData.CompactedModFormList)
            {
                if (evilFormHandler.OrigonalFormID.Equals("000000"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}