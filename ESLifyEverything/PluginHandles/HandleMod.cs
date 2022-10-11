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
                    if (masterReference.Master.Equals(compactedModData.ModName))
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
                if (GF.Settings.ChangedPluginsOutputToDataFolder)
                {
                    mod.WriteToBinary(Path.Combine(GF.Settings.DataFolderPath, pluginName));
                }
                else mod.WriteToBinary(Path.Combine(GF.Settings.OutputFolder, pluginName));
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
	    
    }
}
