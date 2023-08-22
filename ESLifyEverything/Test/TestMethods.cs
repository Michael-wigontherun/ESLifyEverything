namespace ESLifyEverything.Test
{
    internal static class TestMethods
    {
        public const string TestBasePath = "Test\\BaseTest\\";
        public const string TestOutputPath = "Test\\TestJson\\";

        public static System.Text.Json.JsonSerializerOptions jsonSerializerOptions => ESLifyEverythingGlobalDataLibrary.GF.JsonSerializerOptions;

        public static void RunNotePadPlusPlus(string filePath) => System.Diagnostics.Process.Start(@"C:\Program Files\Notepad++\notepad++.exe", "\"" + filePath + "\"");

        //Test CustomSkillsFrameWorkReader
        public static void TestCustomSkillsFrameWorkReader()
        {
            FormData.CompactedModData compactedModData = new()
            {
                ModName = "Ascension CSF v1.esp",
                CompactedModFormList = new()
                {
                    new FormData.FormHandler("Merge.esp", "000800", "000921"),
                    new FormData.FormHandler("Ascension CSF v1.esp", "000801", "000935")
                }
            };
            ESLify.CompactedModDataD.Add(compactedModData.ModName, compactedModData);

            FormData.CustomSkillsFramework customSkillsFramework = new(File.ReadAllLines(TestBasePath + "CustomSkill.ascension.config.txt"));

            customSkillsFramework.UpdateFileLines();

            string pathJson = TestOutputPath + "customSkillsTest.json";
            string pathtxt = TestOutputPath + "TestCustomSkill.ascension.config.txt";
            File.WriteAllText(pathJson, System.Text.Json.JsonSerializer.Serialize(customSkillsFramework, jsonSerializerOptions));
            File.WriteAllLines(pathtxt, customSkillsFramework.FileLines);

            RunNotePadPlusPlus(TestBasePath + "CustomSkill.ascension.config.txt");
            RunNotePadPlusPlus(pathJson);
            RunNotePadPlusPlus(pathtxt);

            ESLify.CompactedModDataD.Remove(compactedModData.ModName);
        }

        //Test OBodyNG
        public static void TestOBodyESLify()
        {
            FormData.CompactedModData compactedModData = new()
            {
                ModName = "Immersive Wenches.esp",
                CompactedModFormList = new()
                {
                    new FormData.FormHandler("Merge.esp", "03197F", "000921"),
                    new FormData.FormHandler("Immersive Wenches.esp", "00C3C0", "000935")
                }
            };
            ESLify.CompactedModDataD.Add(compactedModData.ModName, compactedModData);

            FormData.OBodyJson? oBodyJson = ESLify.OBodyNGESLify(TestBasePath + "OBody_presetDistributionConfig.json", true);

            File.WriteAllText(TestOutputPath + "TestOBody_presetDistributionConfig.json", System.Text.Json.JsonSerializer.Serialize(oBodyJson, jsonSerializerOptions));

            RunNotePadPlusPlus(TestBasePath + "OBody_presetDistributionConfig.json");
            RunNotePadPlusPlus(TestOutputPath + "TestOBody_presetDistributionConfig.json");

            ESLify.CompactedModDataD.Remove(compactedModData.ModName);
        }
    }
}
