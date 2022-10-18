using ESLifyEverything.FormData;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;

namespace ESLifyEverything.PluginHandles
{
    public static partial class HandleMod
    {
        //Lambda get for the Program.CompactedModDataD located in the Program data
        public static Dictionary<string, CompactedModData> CompactedModDataD => Program.CompactedModDataD;

        //Uses the Plugin name to find and read the plugin
        //Changing FormKeys on Forms are handled by HandleSubFormHeaders() and HandleUniformFormHeaders()
        //FormLinks are handled using RemapLinks()
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
                string outputPath = Program.GetPluginModOutputPath(pluginName);

                mod.WriteToBinary(Path.Combine(outputPath, pluginName));

                GF.WriteLine(String.Format(GF.stringLoggingData.PluginOutputTo, pluginName, outputPath));

                return await Task.FromResult(1);
            }

            return await Task.FromResult(2);
        }

        //Gets the the Compacted FormKey that the Original was changed to
        public static FormKey HandleFormKeyFix(FormKey OrgFormKey, CompactedModData compactedModData, out bool changed)
		{
			changed = false;
			foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
			{
				if (OrgFormKey.IDString().Equals(formHandler.OriginalFormID))
				{
					changed = true;
					return formHandler.CreateCompactedFormKey();
				}
			}
			return OrgFormKey;
		}

    }
}
