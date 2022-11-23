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
                                if (persistentRef is PlacedNpc)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedNpc)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedNpc)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
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

                                if (persistentRef is PlacedObject)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedObject)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedObject)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P PlacedObject " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P PlacedObject to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (persistentRef is APlacedTrap)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (APlacedTrap)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (APlacedTrap)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P APlacedTrap " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P APlacedTrap to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (persistentRef is PlacedArrow)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedArrow)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedArrow)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P PlacedArrow " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P PlacedArrow to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (persistentRef is PlacedBarrier)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedBarrier)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedBarrier)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P PlacedBarrier " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P PlacedBarrier to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (persistentRef is PlacedBeam)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedBeam)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedBeam)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P PlacedBeam " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P PlacedBeam to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (persistentRef is PlacedCone)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedCone)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedCone)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P PlacedCone " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P PlacedCone to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (persistentRef is PlacedFlame)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedFlame)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedFlame)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P PlacedFlame " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P PlacedFlame to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (persistentRef is PlacedHazard)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedHazard)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedHazard)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P PlacedHazard " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P PlacedHazard to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (persistentRef is PlacedMissile)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedMissile)persistentRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedMissile)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W P PlacedMissile " + persistentRef.FormKey.ToString());
                                                    cell.Persistent.Remove(persistentRef);
                                                    DevLog.Log("Duplicating W P PlacedMissile to " + formCopy.FormKey.ToString());
                                                    cell.Persistent.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                            foreach (IPlaced temporaryRef in cell.Temporary.ToArray())
                            {
                                if (temporaryRef is PlacedNpc)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedNpc)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedNpc)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
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

                                if (temporaryRef is PlacedObject)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedObject)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedObject)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T PlacedObject " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating W T PlacedObject to " + formCopy.FormKey.ToString());
                                                    cell.Temporary.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (temporaryRef is APlacedTrap)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (APlacedTrap)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (APlacedTrap)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T APlacedTrap " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating W T APlacedTrap to " + formCopy.FormKey.ToString());
                                                    cell.Temporary.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (temporaryRef is PlacedArrow)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedArrow)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedArrow)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T PlacedArrow " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating W T PlacedArrow to " + formCopy.FormKey.ToString());
                                                    cell.Temporary.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (temporaryRef is PlacedBarrier)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedBarrier)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedBarrier)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T PlacedBarrier " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating W T PlacedBarrier to " + formCopy.FormKey.ToString());
                                                    cell.Temporary.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (temporaryRef is PlacedBeam)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedBeam)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedBeam)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T PlacedBeam " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating W T PlacedBeam to " + formCopy.FormKey.ToString());
                                                    cell.Temporary.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (temporaryRef is PlacedCone)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedCone)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedCone)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T PlacedCone " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating W T PlacedCone to " + formCopy.FormKey.ToString());
                                                    cell.Temporary.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (temporaryRef is PlacedFlame)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedFlame)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedFlame)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T PlacedFlame " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating W T PlacedFlame to " + formCopy.FormKey.ToString());
                                                    cell.Temporary.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (temporaryRef is PlacedHazard)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedHazard)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedHazard)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T PlacedHazard " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating W T PlacedHazard to " + formCopy.FormKey.ToString());
                                                    cell.Temporary.Add(formCopy);
                                                    ModEdited = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (temporaryRef is PlacedMissile)
                                {
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                    {
                                        if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                        {
                                            var formCopy = (PlacedMissile)temporaryRef.DeepCopy();
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                            {
                                                if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                                {
                                                    formCopy = (PlacedMissile)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                    DevLog.Log("Removing W T PlacedMissile " + temporaryRef.FormKey.ToString());
                                                    cell.Temporary.Remove(temporaryRef);
                                                    DevLog.Log("Duplicating T PlacedMissile to " + formCopy.FormKey.ToString());
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
                            if(persistentRef is PlacedNpc)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedNpc)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedNpc)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
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

                            if (persistentRef is PlacedObject)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedObject)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedObject)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P PlacedObject " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P PlacedObject to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persistentRef is APlacedTrap)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (APlacedTrap)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (APlacedTrap)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P APlacedTrap " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P APlacedTrap to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persistentRef is PlacedArrow)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedArrow)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedArrow)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P PlacedArrow " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P PlacedArrow to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persistentRef is PlacedBarrier)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedBarrier)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedBarrier)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P PlacedBarrier " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P PlacedBarrier to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persistentRef is PlacedBeam)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedBeam)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedBeam)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P PlacedBeam " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P PlacedBeam to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persistentRef is PlacedCone)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedCone)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedCone)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P PlacedCone " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P PlacedCone to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persistentRef is PlacedFlame)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedFlame)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedFlame)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P PlacedFlame " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P PlacedFlame to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persistentRef is PlacedHazard)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedHazard)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedHazard)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P PlacedHazard " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P PlacedHazard to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (persistentRef is PlacedMissile)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (persistentRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedMissile)persistentRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (persistentRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedMissile)persistentRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing P PlacedMissile " + persistentRef.FormKey.ToString());
                                                cell.Persistent.Remove(persistentRef);
                                                DevLog.Log("Duplicating P PlacedMissile to " + formCopy.FormKey.ToString());
                                                cell.Persistent.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        foreach (IPlaced temporaryRef in cell.Temporary.ToArray())
                        {
                            if (temporaryRef is PlacedNpc)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedNpc)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedNpc)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
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

                            if (temporaryRef is PlacedObject)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedObject)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedObject)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T PlacedObject " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T PlacedObject to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (temporaryRef is APlacedTrap)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (APlacedTrap)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (APlacedTrap)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T APlacedTrap " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T APlacedTrap to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (temporaryRef is PlacedArrow)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedArrow)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedArrow)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T PlacedArrow " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T PlacedArrow to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (temporaryRef is PlacedBarrier)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedBarrier)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedBarrier)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T PlacedBarrier " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T PlacedBarrier to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (temporaryRef is PlacedBeam)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedBeam)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedBeam)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T PlacedBeam " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T PlacedBeam to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (temporaryRef is PlacedCone)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedCone)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedCone)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T PlacedCone " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T PlacedCone to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (temporaryRef is PlacedFlame)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedFlame)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedFlame)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T PlacedFlame " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T PlacedFlame to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (temporaryRef is PlacedHazard)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedHazard)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedHazard)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T PlacedHazard " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T PlacedHazard to " + formCopy.FormKey.ToString());
                                                cell.Temporary.Add(formCopy);
                                                ModEdited = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (temporaryRef is PlacedMissile)
                            {
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)
                                {
                                    if (temporaryRef.FormKey.ModKey.Equals(compactedModData.ModName))
                                    {
                                        var formCopy = (PlacedMissile)temporaryRef.DeepCopy();
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)
                                        {
                                            if (temporaryRef.FormKey.Equals(formHandler.CreateOriginalFormKey(compactedModData.ModName)))
                                            {
                                                formCopy = (PlacedMissile)temporaryRef.Duplicate(formHandler.CreateCompactedFormKey());
                                                DevLog.Log("Removing T PlacedMissile " + temporaryRef.FormKey.ToString());
                                                cell.Temporary.Remove(temporaryRef);
                                                DevLog.Log("Duplicating T PlacedMissile to " + formCopy.FormKey.ToString());
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

            return mod;
        }
    }
}