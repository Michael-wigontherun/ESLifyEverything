using System.Text.Json;

namespace Test
{
    public static class TestESLifyEverything
    {
        public static void TestCustomSkillsFrameWorkReader()
        {
            ESLifyEverything.FormData.CustomSkillsFramework customSkillsFramework = new ESLifyEverything.FormData.CustomSkillsFramework(File.ReadAllLines(@"E:\SkyrimMods\MO2\mods\Ascension 2 for CSF v1 (Skyrim 1.5.97)\NetScriptFramework\Plugins\CustomSkill.ascension.config.txt"));

            ESLifyEverything.FormData.CompactedModData compactedModData = new ESLifyEverything.FormData.CompactedModData();
            compactedModData.ModName = "Ascension CSF v1.esp";
            compactedModData.CompactedModFormList.Add(new ESLifyEverything.FormData.FormHandler("Merge.esp", "000800", "000921"));
            compactedModData.CompactedModFormList.Add(new ESLifyEverything.FormData.FormHandler("Ascension CSF v1.esp", "000801", "000935"));
            ESLifyEverything.Program.CompactedModDataD.Add(compactedModData.ModName, compactedModData);

            customSkillsFramework.UpdateFileLines();

            File.WriteAllText(Test.Program.TestJsonOutputPath + "customSkillsTest.json", JsonSerializer.Serialize(customSkillsFramework, Test.Program.jsonSerializerOptions));

            ESLifyEverything.Program.CompactedModDataD = new();
        }

        public static void TestOBodyESLify()
        {
            ESLifyEverything.FormData.CompactedModData compactedModData = new ESLifyEverything.FormData.CompactedModData();
            compactedModData.ModName = "Immersive Wenches.esp";
            compactedModData.CompactedModFormList.Add(new ESLifyEverything.FormData.FormHandler("Merge.esp", "03197F", "000921"));
            compactedModData.CompactedModFormList.Add(new ESLifyEverything.FormData.FormHandler("Immersive Wenches.esp", "00C3C0", "000935"));
            ESLifyEverything.Program.CompactedModDataD.Add(compactedModData.ModName, compactedModData);

            ESLifyEverything.FormData.OBodyJson? oBodyJson = ESLifyEverything.Program.OBodyNGESLify("E:\\SkyrimMods\\MO2\\mods\\OBody NG - Custom Morphs\\SKSE\\Plugins\\OBody_presetDistributionConfig.json");

            Console.WriteLine(JsonSerializer.Serialize(oBodyJson, Test.Program.jsonSerializerOptions));

            ESLifyEverything.Program.CompactedModDataD = new();
        }
    }
}
