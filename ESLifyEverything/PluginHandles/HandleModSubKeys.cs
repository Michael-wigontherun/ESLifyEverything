using ESLifyEverything.FormData;
using Mutagen.Bethesda.Skyrim;

namespace ESLifyEverything.PluginHandles
{
    public static partial class HandleMod
    {
        //Changes the FormKeys of plugins located in internalized lists
        public static SkyrimMod HandleSubFormHeaders(SkyrimMod mod, out bool ModEdited)
        {
            ModEdited = false;

            //DialogTopics
            var DialogTopicsItemSet = mod.DialogTopics.ToHashSet();
            foreach (var sourceForm in DialogTopicsItemSet)
            {
                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                {
                    var responsesItemSet = sourceForm.Responses.ToHashSet();
                    List<DialogResponses> responsesS = new List<DialogResponses>();
                    List<DialogResponses> responsesC = new List<DialogResponses>();
                    for (int i = 0; i < sourceForm.Responses.Count; i++)
                    {
                        if (sourceForm.Responses[i].FormKey.ModKey.Equals(compactedModData.ModName))
                        {
                            //var formCopy = sourceForm.Responses[i].DeepCopy();
                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                            {
                                if (sourceForm.Responses[i].FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                {
                                    responsesC.Add(sourceForm.Responses[i]);
                                    responsesC.Add(sourceForm.Responses[i].Duplicate(formHandler.CreateCompactedFormKey()));
                                    break;
                                }
                            }
                        }
                    }
                    for(int i = 0; i < responsesS.Count; i++)
                    {
                        sourceForm.Responses.Remove(responsesS[i]);
                        sourceForm.Responses.Add(responsesC[i]);
                        ModEdited = true;
                    }
                    responsesS.Clear();
                    responsesC.Clear();
                    if (sourceForm.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                    {
                        //var formCopy = sourceForm.DeepCopy();
                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                        {
                            if (sourceForm.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                            {

                                var formCopy = sourceForm.Duplicate(formHandler.CreateCompactedFormKey());
                                mod.Remove(sourceForm.FormKey);
                                mod.DialogTopics.Add(formCopy);
                                ModEdited = true;
                                break;
                            }
                        }
                    }

                }
            }
            DialogTopicsItemSet.Clear();

            //Worldspaces
            var WorldspacesItemSet = mod.Worldspaces.ToHashSet();
            foreach (var worldSpace in WorldspacesItemSet)
            {
                //worldSpace.SubCells[d].Items[e].Items[f].
                for (int d = 0; d < worldSpace.SubCells.Count; d++)
                {
                    for (int e = 0; e < worldSpace.SubCells[d].Items.Count; e++)
                    {
                        List<Cell> cellsS = new List<Cell>();
                        List<Cell> cellsC = new List<Cell>();
                        for (int f = 0; f < worldSpace.SubCells[d].Items[e].Items.Count; f++)
                        {

                            Cell? sourceForm = worldSpace.SubCells[d].Items[e].Items[f];
                            Cell? formCopy = sourceForm.DeepCopy();
                            bool EditedForm = false;
                            foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                            {
                                if (sourceForm.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                {
                                    foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                    {
                                        if (sourceForm.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                        {

                                            formCopy = sourceForm.Duplicate(formHandler.CreateCompactedFormKey());
                                            EditedForm = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            for (int c = 0; c < worldSpace.SubCells[d].Items[e].Items[f].Persistent.Count; c++)
                            {

                                var sourceFormP = formCopy.Persistent[c];
                                if (sourceFormP is PlacedNpc)
                                {
                                    var formCopyP = (PlacedNpc)sourceFormP.DeepCopy();
                                    bool EditedFormP = false;
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                    {
                                        if (sourceFormP.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                        {
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                            {
                                                if (sourceFormP.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                                {
                                                    formCopyP = (PlacedNpc)sourceFormP.Duplicate(formHandler.CreateCompactedFormKey());
                                                    EditedFormP = true;
                                                    EditedForm = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (EditedFormP)
                                    {
                                        formCopy.Persistent.Remove(sourceFormP);
                                        formCopy.Persistent.Add(formCopyP);
                                        ModEdited = true;
                                    }
                                }
                                if (sourceFormP is PlacedObject)
                                {
                                    var formCopyP = (PlacedObject)sourceFormP.DeepCopy();
                                    bool EditedFormP = false;
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                    {
                                        if (sourceFormP.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                        {
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                            {
                                                if (sourceFormP.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                                {
                                                    formCopyP = (PlacedObject)sourceFormP.Duplicate(formHandler.CreateCompactedFormKey());
                                                    EditedFormP = true;
                                                    EditedForm = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (EditedFormP)
                                    {
                                        formCopy.Persistent.Remove(sourceFormP);
                                        formCopy.Persistent.Add(formCopyP);
                                        ModEdited = true;
                                    }
                                }
                                if (sourceFormP is APlacedTrap)
                                {
                                    var formCopyP = (APlacedTrap)sourceFormP.DeepCopy();
                                    bool EditedFormP = false;
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                    {
                                        if (sourceFormP.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                        {
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                            {
                                                if (sourceFormP.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                                {

                                                    formCopyP = (APlacedTrap)sourceFormP.Duplicate(formHandler.CreateCompactedFormKey());
                                                    EditedFormP = true;
                                                    EditedForm = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (EditedFormP)
                                    {
                                        formCopy.Persistent.Remove(sourceFormP);
                                        formCopy.Persistent.Add(formCopyP);
                                        ModEdited = true;
                                    }
                                }

                            }

                            for (int c = 0; c < worldSpace.SubCells[d].Items[e].Items[f].Temporary.Count; c++)
                            {

                                var sourceFormT = formCopy.Temporary[c];
                                if (sourceFormT is PlacedNpc)
                                {
                                    var formCopyT = (PlacedNpc)sourceFormT.DeepCopy();
                                    bool EditedFormT = false;
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                    {
                                        if (sourceFormT.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                        {
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                            {
                                                if (sourceFormT.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                                {

                                                    formCopyT = (PlacedNpc)sourceFormT.Duplicate(formHandler.CreateCompactedFormKey());
                                                    EditedFormT = true;
                                                    EditedForm = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (EditedFormT)
                                    {
                                        formCopy.Temporary.Remove(sourceFormT);
                                        formCopy.Temporary.Add(formCopyT);
                                        ModEdited = true;
                                    }
                                }
                                if (sourceFormT is PlacedObject)
                                {
                                    var formCopyT = (PlacedObject)sourceFormT.DeepCopy();
                                    bool EditedFormT = false;
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                    {
                                        if (sourceFormT.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                        {
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                            {
                                                if (sourceFormT.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                                {

                                                    formCopyT = (PlacedObject)sourceFormT.Duplicate(formHandler.CreateCompactedFormKey());
                                                    EditedFormT = true;
                                                    EditedForm = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (EditedFormT)
                                    {
                                        formCopy.Temporary.Remove(sourceFormT);
                                        formCopy.Temporary.Add(formCopyT);
                                        ModEdited = true;
                                    }
                                }
                                if (sourceFormT is APlacedTrap)
                                {
                                    var formCopyT = (APlacedTrap)sourceFormT.DeepCopy();
                                    bool EditedFormT = false;
                                    foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                    {
                                        if (sourceFormT.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                        {
                                            foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                            {
                                                if (sourceFormT.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                                {

                                                    formCopyT = (APlacedTrap)sourceFormT.Duplicate(formHandler.CreateCompactedFormKey());
                                                    EditedFormT = true;
                                                    EditedForm = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (EditedFormT)
                                    {
                                        formCopy.Temporary.Remove(sourceFormT);
                                        formCopy.Temporary.Add(formCopyT);
                                        ModEdited = true;
                                    }
                                }

                            }

                            if (EditedForm)
                            {//worldSpace.SubCells[d].Items[e].Items[f]
                                cellsS.Add(sourceForm);
                                cellsC.Add(formCopy);
                                ModEdited = true;
                            }
                        }
                        for (int i = 0; i < cellsS.Count; i++)
                        {
                            worldSpace.SubCells[d].Items[e].Items.Remove(cellsS[i]);
                            worldSpace.SubCells[d].Items[e].Items.Add(cellsC[i]);
                        }
                    }
                }
            }
            WorldspacesItemSet.Clear();
            WorldspacesItemSet = mod.Worldspaces.ToHashSet();
            foreach (var sourceForm in WorldspacesItemSet)
            {
                //sourceForm.SubCells[1].Items[1].Items[1]
                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                {
                    if (sourceForm.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                    {
                        var formCopy = sourceForm.DeepCopy();
                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                        {
                            if (sourceForm.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                            {

                                formCopy = sourceForm.Duplicate(formHandler.CreateCompactedFormKey());
                                mod.Remove(sourceForm.FormKey);
                                mod.Worldspaces.Add(formCopy);
                                ModEdited = true;
                                break;
                            }
                        }
                    }
                }
            }
            WorldspacesItemSet.Clear();

            //Cells
            for (int i = 0; i < mod.Cells.Records.Count; i++)
            {
                for (int a = 0; a < mod.Cells.Records[i].SubBlocks.Count; a++)
                {
                    List<Cell> cellsS = new List<Cell>();
                    List<Cell> cellsC = new List<Cell>();
                    for (int b = 0; b < mod.Cells.Records[i].SubBlocks[a].Cells.Count; b++)
                    {
                        Cell? sourceForm = mod.Cells.Records[i].SubBlocks[a].Cells[b];
                        Cell? formCopy = sourceForm.DeepCopy();
                        bool EditedForm = false;
                        foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                        {
                            if (sourceForm.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                            {
                                foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                {
                                    if (sourceForm.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                    {

                                        formCopy = sourceForm.Duplicate(formHandler.CreateCompactedFormKey());
                                        EditedForm = true;
                                        break;
                                    }
                                }
                            }
                        }

                        for (int c = 0; c < mod.Cells.Records[i].SubBlocks[a].Cells[b].Persistent.Count; c++)
                        {

                            var sourceFormP = formCopy.Persistent[c];
                            if (sourceFormP is PlacedNpc)
                            {
                                var formCopyP = (PlacedNpc)sourceFormP.DeepCopy();
                                bool EditedFormP = false;
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                {
                                    if (sourceFormP.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                    {
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                        {
                                            if (sourceFormP.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                            {
                                                formCopyP = (PlacedNpc)sourceFormP.Duplicate(formHandler.CreateCompactedFormKey());
                                                EditedFormP = true;
                                                EditedForm = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (EditedFormP)
                                {
                                    formCopy.Persistent.Remove(sourceFormP);
                                    formCopy.Persistent.Add(formCopyP);
                                    ModEdited = true;
                                }
                            }
                            if (sourceFormP is PlacedObject)
                            {
                                var formCopyP = (PlacedObject)sourceFormP.DeepCopy();
                                bool EditedFormP = false;
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                {
                                    if (sourceFormP.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                    {
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                        {
                                            if (sourceFormP.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                            {
                                                formCopyP = (PlacedObject)sourceFormP.Duplicate(formHandler.CreateCompactedFormKey());
                                                EditedFormP = true;
                                                EditedForm = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (EditedFormP)
                                {
                                    formCopy.Persistent.Remove(sourceFormP);
                                    formCopy.Persistent.Add(formCopyP);
                                    ModEdited = true;
                                }
                            }
                            if (sourceFormP is APlacedTrap)
                            {
                                var formCopyP = (APlacedTrap)sourceFormP.DeepCopy();
                                bool EditedFormP = false;
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                {
                                    if (sourceFormP.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                    {
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                        {
                                            if (sourceFormP.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                            {

                                                formCopyP = (APlacedTrap)sourceFormP.Duplicate(formHandler.CreateCompactedFormKey());
                                                EditedFormP = true;
                                                EditedForm = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (EditedFormP)
                                {
                                    formCopy.Persistent.Remove(sourceFormP);
                                    formCopy.Persistent.Add(formCopyP);
                                    ModEdited = true;
                                }
                            }

                        }

                        for (int c = 0; c < mod.Cells.Records[i].SubBlocks[a].Cells[b].Temporary.Count; c++)
                        {

                            var sourceFormT = formCopy.Temporary[c];
                            if (sourceFormT is PlacedNpc)
                            {
                                var formCopyT = (PlacedNpc)sourceFormT.DeepCopy();
                                bool EditedFormT = false;
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                {
                                    if (sourceFormT.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                    {
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                        {
                                            if (sourceFormT.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                            {

                                                formCopyT = (PlacedNpc)sourceFormT.Duplicate(formHandler.CreateCompactedFormKey());
                                                EditedFormT = true;
                                                EditedForm = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (EditedFormT)
                                {
                                    formCopy.Temporary.Remove(sourceFormT);
                                    formCopy.Temporary.Add(formCopyT);
                                    ModEdited = true;
                                }
                            }
                            if (sourceFormT is PlacedObject)
                            {
                                var formCopyT = (PlacedObject)sourceFormT.DeepCopy();
                                bool EditedFormT = false;
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                {
                                    if (sourceFormT.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                    {
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                        {
                                            if (sourceFormT.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                            {

                                                formCopyT = (PlacedObject)sourceFormT.Duplicate(formHandler.CreateCompactedFormKey());
                                                EditedFormT = true;
                                                EditedForm = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (EditedFormT)
                                {
                                    formCopy.Temporary.Remove(sourceFormT);
                                    formCopy.Temporary.Add(formCopyT);
                                    ModEdited = true;
                                }
                            }
                            if (sourceFormT is APlacedTrap)
                            {
                                var formCopyT = (APlacedTrap)sourceFormT.DeepCopy();
                                bool EditedFormT = false;
                                foreach (CompactedModData compactedModData in CompactedModDataD.Values)// iterate each compacted mod data
                                {
                                    if (sourceFormT.FormKey.ModKey.Equals(compactedModData.ModName))// check if the form comes from the compacted mod
                                    {
                                        foreach (FormHandler formHandler in compactedModData.CompactedModFormList)// iterate compacted forms
                                        {
                                            if (sourceFormT.FormKey.IDString().Equals(formHandler.OriginalFormID))// check if the mod still uses one of the origonal formIDs
                                            {

                                                formCopyT = (APlacedTrap)sourceFormT.Duplicate(formHandler.CreateCompactedFormKey());
                                                EditedFormT = true;
                                                EditedForm = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (EditedFormT)
                                {
                                    formCopy.Temporary.Remove(sourceFormT);
                                    formCopy.Temporary.Add(formCopyT);
                                    ModEdited = true;
                                }
                            }

                        }

                        if (EditedForm)
                        {
                            cellsS.Add(sourceForm);
                            cellsC.Add(formCopy);
                            ModEdited = true;
                        }
                    }
                    for (int c = 0; c < cellsS.Count; c++)
                    {
                        mod.Cells.Records[i].SubBlocks[a].Cells.Remove(cellsS[c]);
                        mod.Cells.Records[i].SubBlocks[a].Cells.Add(cellsC[c]);
                    }
                }
            }

            return mod;
        }
    }
}
