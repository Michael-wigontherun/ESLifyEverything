using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;
using System.Text.Json;

namespace ESLifyCreateJson
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<ComplexTOML> ComplexTOMLlist = new List<ComplexTOML>();
            ComplexTOMLlist.Add(new ComplexTOML());
            ComplexTOMLlist.Add(new ComplexTOML());

            List<BasicDataSubfolder> BasicDataSubfolderlist = new List<BasicDataSubfolder>();

            BasicDataSubfolderlist.Add(new BasicDataSubfolder());
            BasicDataSubfolderlist.Add(new BasicDataSubfolder());

            List<BasicDirectFolder> BasicDirectFolderlist = new List<BasicDirectFolder>();

            BasicDirectFolderlist.Add(new BasicDirectFolder());
            BasicDirectFolderlist.Add(new BasicDirectFolder());

            List<BasicSingleFile> BasicSingleFilelist = new List<BasicSingleFile>();

            BasicSingleFilelist.Add(new BasicSingleFile());
            BasicSingleFilelist.Add(new BasicSingleFile());

            List<DelimitedFormKeys> DelimitedFormKeysFilelist = new List<DelimitedFormKeys>();

            DelimitedFormKeysFilelist.Add(new DelimitedFormKeys());
            DelimitedFormKeysFilelist.Add(new DelimitedFormKeys());

            Dictionary<string, string> CustomPluginOutputLocations = new Dictionary<string, string>();

            CustomPluginOutputLocations.Add("Skyrim.esm", "E:\\SkyrimMods\\MO2\\mods\\Skyrim_plugin_override");
            CustomPluginOutputLocations.Add("Dawnguard.esm", "@mods\\Dawnguard_plugin_override");
            CustomPluginOutputLocations.Add("Dont do either of those", "They are examples");
            CustomPluginOutputLocations.Add("Again \\ is nessessary", "inside json files");
            CustomPluginOutputLocations.Add("Place and edit this", "inside of the Properties folder");

            string dfJSON = ".\\ESLifyEverythingDefaultJSON";

            Directory.CreateDirectory(dfJSON);

            File.WriteAllText($"{dfJSON}\\Template_DelimitedFormKeys.json", JsonSerializer.Serialize(DelimitedFormKeysFilelist, GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\Template_ComplexTOML.json", JsonSerializer.Serialize(ComplexTOMLlist, GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\Template_BasicDataSubfolder.json", JsonSerializer.Serialize(BasicDataSubfolderlist, GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\Template_BasicDirectFolder.json", JsonSerializer.Serialize(BasicDirectFolderlist, GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\Template_BasicSingleFile.json", JsonSerializer.Serialize(BasicSingleFilelist, GF.JsonSerializerOptions));

            File.WriteAllText($"{dfJSON}\\CustomPluginOutputLocations.json", JsonSerializer.Serialize(CustomPluginOutputLocations, GF.JsonSerializerOptions));

            File.WriteAllText($"{dfJSON}\\AppSettings.json", JsonSerializer.Serialize(new GeneratedAppSettings(), GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\DevAppSettings.json", JsonSerializer.Serialize(new DevAppSettings(), GF.JsonSerializerOptions));

            System.Diagnostics.Process.Start("explorer.exe", Path.GetFullPath(dfJSON));
        }
    }
}