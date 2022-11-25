using ESLifyEverything.FormData;
using Mutagen.Bethesda.Skyrim;
using ESLifyEverythingGlobalDataLibrary;
using Mutagen.Bethesda.Plugins;

namespace ESLifyEverything.PluginHandles
{
    public static partial class HandleMod
    {
        //Changes the FormKeys of plugins located in internalized lists
        public static SkyrimMod HandleSubFormHeaders(SkyrimMod mod, out bool ModEdited)
        {
            ModEdited = false;

            //DialogTopics
            foreach (var dialogTopic in mod.DialogTopics.ToArray())
            {
                if (CompactedModDataD.TryGetValue(dialogTopic.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(dialogTopic.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = dialogTopic.Duplicate(formKey);
                        DevLog.Log("DialogTopics: Removing " + dialogTopic.FormKey.ToString());
                        mod.DialogTopics.Remove(dialogTopic.FormKey);
                        DevLog.Log("DialogTopics: Duplicating to " + formCopy.FormKey.ToString());
                        mod.DialogTopics.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            
            foreach (var dialogTopic in mod.DialogTopics)
            {
                foreach(DialogResponses? response in dialogTopic.Responses.ToArray())
                {
                    if (CompactedModDataD.TryGetValue(dialogTopic.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                    {
                        FormKey formKey = HandleFormKeyFix(dialogTopic.FormKey, compactedModData, out bool changed);
                        if (changed)
                        {
                            var formCopy = response.Duplicate(formKey);
                            DevLog.Log("GameSettings: Removing " + dialogTopic.FormKey.ToString());
                            dialogTopic.Responses.Remove(response);
                            DevLog.Log("GameSettings: Duplicating to " + formCopy.FormKey.ToString());
                            dialogTopic.Responses.Add(formCopy);
                            ModEdited = true;
                            break;
                        }
                    }
                }
            }

            //Worldspaces
            foreach (var worldspace in mod.Worldspaces.ToArray())
            {
                //sourceForm.SubCells[1].Items[1].Items[1]
                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                {
                    if (worldspace.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                    {
                        var formCopy = worldspace.DeepCopy();
                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                        {
                            if (worldspace.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                            {
                                formCopy = worldspace.Duplicate(formHandler.CreateCompactedFormKey());
                                DevLog.Log("Removing WorldSpace Cell " + worldspace.FormKey.ToString());
                                mod.Remove(worldspace.FormKey);
                                DevLog.Log("Duplicating to WorldSpace Cell " + formCopy.FormKey.ToString());
                                mod.Worldspaces.Add(formCopy);
                                ModEdited = true;
                                break;
                            }
                        }
                    }
                }
            }
            
            foreach (var worldspace in mod.Worldspaces)
            {
                foreach(var worldSpaceBlock in worldspace.SubCells)
                {
                    foreach(var worldspaceSubBlock in worldSpaceBlock.Items)
                    {
                        foreach(Cell? cell in worldspaceSubBlock.Items.ToArray())
                        {
                            foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                            {
                                if (cell.FormKey.ModKey.Equals(compactedModData.ModName))
                                {
                                    var formCopy = cell.DeepCopy();
                                    foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                    {
                                        if (cell.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                        {
                                            formCopy = cell.Duplicate(formHandler.CreateCompactedFormKey());
                                            DevLog.Log("Removing W Cell " + cell.FormKey.ToString());
                                            worldspaceSubBlock.Items.Remove(cell);
                                            DevLog.Log("Duplicating to W Cell " + formCopy.FormKey.ToString());
                                            worldspaceSubBlock.Items.Add(formCopy);
                                            ModEdited = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var worldspace in mod.Worldspaces)
            {
                foreach (var worldSpaceBlock in worldspace.SubCells)
                {
                    foreach (var worldspaceSubBlock in worldSpaceBlock.Items)
                    {
                        foreach (Cell? cell in worldspaceSubBlock.Items)
                        {
                            foreach (IPlaced persistentRef in cell.Persistent.ToArray())
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        IPlaced formCopy = (IPlaced)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (IPlaced)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing W P PlacedNpc " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating W P PlacedNpc to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (IPlaced temporaryRef in cell.Temporary.ToArray())
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        IPlaced formCopy = (IPlaced)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (IPlaced)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing W T PlacedNpc " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating W T PlacedNpc to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }

            //Cells
            foreach (var cellRec in mod.Cells.Records)
            {
                foreach(var cellBlock in cellRec.SubBlocks)
                {
                    foreach(var cell in cellBlock.Cells.ToArray())
                    {
                        foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                        {
                            if (cell.FormKey.ModKey.Equals(compactedModData.ModName))
                            {
                                var formCopy = cell.DeepCopy();
                                foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                {
                                    if (cell.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                    {
                                        formCopy = cell.Duplicate(formHandler.CreateCompactedFormKey());
                                        DevLog.Log("Removing Cell " + cell.FormKey.ToString());
                                        cellBlock.Cells.Remove(cell);
                                        DevLog.Log("Duplicating to Cell " + formCopy.FormKey.ToString());
                                        cellBlock.Cells.Add(formCopy);
                                        ModEdited = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var cellRec in mod.Cells.Records)
            {
                foreach (var cellBlock in cellRec.SubBlocks)
                {
                    foreach (var cell in cellBlock.Cells)
                    {
                        foreach(IPlaced persistentRef in cell.Persistent.ToArray())
                        {
                            foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                            {
                                if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                {
                                    IPlaced formCopy = (IPlaced)persistentRef.DeepCopy();
                                    foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                    {
                                        if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                        {
                                            formCopy = (IPlaced)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                            DevLog.Log("Removing P PlacedNpc " + persistentRef.FormKey.ToString());
                                            cell.Persistent.Remove(persistentRef);
                                            DevLog.Log("Duplicating P PlacedNpc to " + formCopy.FormKey.ToString());
                                            cell.Persistent.Add(formCopy);
                                            ModEdited = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        foreach (IPlaced temporaryRef in cell.Temporary.ToArray())
                        {
                            foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                            {
                                if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                {
                                    IPlaced formCopy = (IPlaced)temporaryRef.DeepCopy();
                                    foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                    {
                                        if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                        {
                                            formCopy = (IPlaced)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                            DevLog.Log("Removing T PlacedNpc " + temporaryRef.FormKey.ToString());
                                            cell.Temporary.Remove(temporaryRef);
                                            DevLog.Log("Duplicating T PlacedNpc to " + formCopy.FormKey.ToString());
                                            cell.Temporary.Add(formCopy);
                                            ModEdited = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return mod;
        }
    }
}