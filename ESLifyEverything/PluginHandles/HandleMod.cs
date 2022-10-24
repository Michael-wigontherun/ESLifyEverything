using ESLifyEverything.FormData;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using ESLifyEverythingGlobalDataLibrary;

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
            DevLog.Log("Handling " + orgMod.ModKey.ToString());
            SkyrimMod mod = new SkyrimMod(orgMod.ModKey, SkyrimRelease.SkyrimSE);
            mod.DeepCopyIn(orgMod);
            DevLog.Log("Coppied " + mod.ModKey.ToString() + " for handling comparison.");
            //SkyrimMod mod = SkyrimMod.CreateFromBinary(path, SkyrimRelease.SkyrimSE);

            bool ModEdited = false;

            mod = HandleUniformFormHeaders(mod, out bool ModEditedU);
            DevLog.Log("Finnished handling uniform keys in " + mod.ModKey.ToString());

            mod = HandleSubFormHeaders(mod, out bool ModEditedS);
            DevLog.Log("Finnished handling sub form keys in " + mod.ModKey.ToString());

            if (ModEditedS || ModEditedU)
            {
                ModEdited = true;
                DevLog.Log(mod.ModKey.ToString() + " was changed.");
            }
            foreach (IMasterReferenceGetter masterReference in mod.ModHeader.MasterReferences.ToHashSet())
            {
                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                {
                    if (masterReference.Master.ToString().Equals(compactedModData.ModName))
                    {
                        DevLog.Log(mod.ModKey.ToString() + " attempting remapping with CompactedModData from " + compactedModData.ModName);
                        mod.RemapLinks(compactedModData.ToDictionary());
                    }
                    if (compactedModData.ModName.Equals(mod.ModKey.ToString()))
                    {
                        DevLog.Log(mod.ModKey.ToString() + " attempting remapping with CompactedModData from " + compactedModData.ModName);
                        mod.RemapLinks(compactedModData.ToDictionary());
                    }
                }
            }

            if (!mod.Equals(orgMod))
            {
                DevLog.Log(mod.ModKey.ToString() + " was changed.");
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

            GF.WriteLine(mod.ModKey.ToString() + " was not output.", GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);

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
