namespace Test
{
    public static class TestESLifyEverything
    {
        public const string TestBasePath = "BaseTest\\";
        public const string TestOutputPath = "TestJson\\";

        public static System.Text.Json.JsonSerializerOptions jsonSerializerOptions => ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions;

        public static void RunNotePadPlusPlus(string filePath) => System.Diagnostics.Process.Start(@"C:\Program Files\Notepad++\notepad++.exe", "\"" + filePath + "\"");

        //Test CustomSkillsFrameWorkReader
        public static void TestCustomSkillsFrameWorkReader()
        {
            ESLifyEverything.FormData.CompactedModData compactedModData = new ESLifyEverything.FormData.CompactedModData();
            compactedModData.ModName = "Ascension CSF v1.esp";
            compactedModData.CompactedModFormList.Add(new ESLifyEverything.FormData.FormHandler("Merge.esp", "000800", "000921"));
            compactedModData.CompactedModFormList.Add(new ESLifyEverything.FormData.FormHandler("Ascension CSF v1.esp", "000801", "000935"));
            ESLifyEverything.Program.CompactedModDataD.Add(compactedModData.ModName, compactedModData);

            ESLifyEverything.FormData.CustomSkillsFramework customSkillsFramework = new ESLifyEverything.FormData.CustomSkillsFramework(File.ReadAllLines(TestBasePath + "CustomSkill.ascension.config.txt"));

            customSkillsFramework.UpdateFileLines();

            string pathJson = TestOutputPath + "customSkillsTest.json";
            string pathtxt = TestOutputPath + "TestCustomSkill.ascension.config.txt";
            File.WriteAllText(pathJson, System.Text.Json.JsonSerializer.Serialize(customSkillsFramework, jsonSerializerOptions));
            File.WriteAllLines(pathtxt, customSkillsFramework.FileLines);

            RunNotePadPlusPlus(TestBasePath + "CustomSkill.ascension.config.txt");
            RunNotePadPlusPlus(pathJson);
            RunNotePadPlusPlus(pathtxt);

            ESLifyEverything.Program.CompactedModDataD = new();
        }

        //Test OBodyNG
        public static void TestOBodyESLify()
        {
            ESLifyEverything.FormData.CompactedModData compactedModData = new ESLifyEverything.FormData.CompactedModData();
            compactedModData.ModName = "Immersive Wenches.esp";
            compactedModData.CompactedModFormList.Add(new ESLifyEverything.FormData.FormHandler("Merge.esp", "03197F", "000921"));
            compactedModData.CompactedModFormList.Add(new ESLifyEverything.FormData.FormHandler("Immersive Wenches.esp", "00C3C0", "000935"));
            ESLifyEverything.Program.CompactedModDataD.Add(compactedModData.ModName, compactedModData);

            ESLifyEverything.FormData.OBodyJson? oBodyJson = ESLifyEverything.Program.OBodyNGESLify(TestBasePath +"OBody_presetDistributionConfig.json", true);

            File.WriteAllText(TestOutputPath + "TestOBody_presetDistributionConfig.json", System.Text.Json.JsonSerializer.Serialize(oBodyJson, jsonSerializerOptions));

            RunNotePadPlusPlus(TestBasePath + "OBody_presetDistributionConfig.json");
            RunNotePadPlusPlus(TestOutputPath + "TestOBody_presetDistributionConfig.json");

            ESLifyEverything.Program.CompactedModDataD = new();
        }
    }
}
