using ESLifyEverything.FormData;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;

namespace ESLifyEverything.PluginHandles
{
    public static partial class HandleMod
    {
        public static Dictionary<string, CompactedModData> CompactedModDataD => Program.CompactedModDataD;

        public static async Task<int> HandleSkyrimMod(string pluginName)
        {
            string path = Path.Combine(GF.Settings.DataFolderPath, pluginName);
            if (!File.Exists(path))
            {
                return await Task.FromResult(0);
            }
            ISkyrimModGetter orgMod = SkyrimMod.CreateFromBinary(path, SkyrimRelease.SkyrimSE);
            SkyrimMod mod = new SkyrimMod(orgMod.ModKey, SkyrimRelease.SkyrimSE);
            mod.DeepCopyIn(orgMod);
            //SkyrimMod mod = SkyrimMod.CreateFromBinary(path, SkyrimRelease.SkyrimSE);

            bool ModEdited = false;

            mod = HandleUniformFormHeaders(mod, out bool ModEditedU);
            mod = HandleSubFormHeaders(mod, out bool ModEditedS);

            if (ModEditedS || ModEditedU) ModEdited = true;

            foreach (IMasterReferenceGetter masterReference in mod.ModHeader.MasterReferences.ToHashSet())
            {
                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                {
                    if (masterReference.Master.ToString().Equals(compactedModData.ModName))
                    {
                        mod.RemapLinks(compactedModData.ToDictionary());
                    }
                    if (compactedModData.ModName.Equals(mod.ModKey.ToString()))
                    {
                        mod.RemapLinks(compactedModData.ToDictionary());
                    }
                }
            }

            if (!mod.Equals(orgMod))
            {
                ModEdited = true;
            }

            //ModEdited = true;
            if (ModEdited)
            {
                foreach (var rec in mod.EnumerateMajorRecords())
                {
                    rec.IsCompressed = false;
                }
                
                mod.WriteToBinary(Path.Combine(GetPluginModOutputPath(pluginName, mod.ModHeader.MasterReferences.ToHashSet()), pluginName));

                return await Task.FromResult(1);
            }
            else GF.WriteLine(GF.stringLoggingData.PluginUnchanged);

            return await Task.FromResult(2);
        }

		public static FormKey HandleFormKeyFix(FormKey OrgFormKey, CompactedModData compactedModData, out bool changed)
		{
			changed = false;
			foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
			{
				if (OrgFormKey.IDString().Equals(formHandler.OrigonalFormID))
				{
					changed = true;
					return formHandler.CreateCompactedFormKey();
				}
			}
			return OrgFormKey;
		}

        private static string GetPluginModOutputPath(string pluginName, HashSet<MasterReference> masters)
        {
            if (GF.Settings.MO2Support)
            {
                string masterExtentions = "";
                foreach(var master in masters)
                {
                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                    {
                        if (master.Master.ToString().Equals(compactedModData.ModName))
                        {
                            masterExtentions = masterExtentions + Path.GetFileNameWithoutExtension(compactedModData.ModName) + "_";
                        }
                    }
                }

                string OutputPath = Path.Combine(GF.Settings.MO2ModFolder, pluginName + $" {masterExtentions}ESlEverything");
                Directory.CreateDirectory(OutputPath);
                return OutputPath;
            }
            else if (GF.Settings.ChangedPluginsOutputToDataFolder)
            {
                return GF.Settings.DataFolderPath;
            }
            return GF.Settings.OutputFolder;
        }





    }
}
