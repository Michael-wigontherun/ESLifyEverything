using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

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
    }
}
