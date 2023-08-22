namespace ESLifyCreateJson
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.ComplexTOML> ComplexTOMLlist = new()
            {
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.ComplexTOML(),
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.ComplexTOML()
            };

            List<ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicDataSubfolder> BasicDataSubfolderlist = new()
            {
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicDataSubfolder(),
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicDataSubfolder()
            };

            List<ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicDirectFolder> BasicDirectFolderlist = new()
            {
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicDirectFolder(),
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicDirectFolder()
            };

            List<ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicSingleFile> BasicSingleFilelist = new()
            {
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicSingleFile(),
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.BasicSingleFile()
            };

            List<ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.DelimitedFormKeys> DelimitedFormKeysFilelist = new()
            {
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.DelimitedFormKeys(),
                new ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes.DelimitedFormKeys()
            };

            Dictionary<string, string> CustomPluginOutputLocations = new()
            {
                { "Skyrim.esm", "E:\\SkyrimMods\\MO2\\mods\\Skyrim_plugin_override" },
                { "Dawnguard.esm", "@mods\\Dawnguard_plugin_override" },
                { "Dont do either of those", "They are examples" },
                { "Again \\ is nessessary", "inside json files" },
                { "Place and edit this", "inside of the Properties folder" }
            };

            string dfJSON = ".\\ESLifyEverythingDefaultJSON";

            Directory.CreateDirectory(dfJSON);

            File.WriteAllText($"{dfJSON}\\Template_DelimitedFormKeys.json", System.Text.Json.JsonSerializer.Serialize(DelimitedFormKeysFilelist, ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\Template_ComplexTOML.json", System.Text.Json.JsonSerializer.Serialize(ComplexTOMLlist, ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\Template_BasicDataSubfolder.json", System.Text.Json.JsonSerializer.Serialize(BasicDataSubfolderlist, ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\Template_BasicDirectFolder.json", System.Text.Json.JsonSerializer.Serialize(BasicDirectFolderlist, ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\Template_BasicSingleFile.json", System.Text.Json.JsonSerializer.Serialize(BasicSingleFilelist, ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions));

            File.WriteAllText($"{dfJSON}\\CustomPluginOutputLocations.json", System.Text.Json.JsonSerializer.Serialize(CustomPluginOutputLocations, ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions));

            File.WriteAllText($"{dfJSON}\\AppSettings.json", System.Text.Json.JsonSerializer.Serialize(new ESLifyEverythingGlobalDataLibrary.Properties.GeneratedAppSettings(), ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions));
            File.WriteAllText($"{dfJSON}\\DevAppSettings.json", System.Text.Json.JsonSerializer.Serialize(new ESLifyEverythingGlobalDataLibrary.Properties.DevAppSettings(), ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions));

            System.Diagnostics.Process.Start("explorer.exe", Path.GetFullPath(dfJSON));
        }
    }
}