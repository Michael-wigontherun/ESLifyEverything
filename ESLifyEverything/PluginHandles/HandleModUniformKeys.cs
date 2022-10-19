using ESLifyEverything.FormData;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using ESLifyEverythingGlobalDataLibrary;

namespace ESLifyEverything.PluginHandles
{
    public static partial class HandleMod
    {
        //Handles changing FormKeys in top level lists in mods
        public static SkyrimMod HandleUniformFormHeaders(SkyrimMod mod, out bool ModEdited)
        {
            ModEdited = false;
            //GameSettings
            var GameSettingsItemSet = mod.GameSettings.ToHashSet();
            foreach (var sourceForm in GameSettingsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("GameSettings: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("GameSettings: Duplicating to " + formCopy.FormKey.ToString());
                        mod.GameSettings.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            GameSettingsItemSet.Clear();
            //Keywords
            var KeywordsItemSet = mod.Keywords.ToHashSet();
            foreach (var sourceForm in KeywordsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Keywords: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Keywords: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Keywords.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            KeywordsItemSet.Clear();
            //LocationReferenceTypes
            var LocationReferenceTypesItemSet = mod.LocationReferenceTypes.ToHashSet();
            foreach (var sourceForm in LocationReferenceTypesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("LocationReferenceTypes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("LocationReferenceTypes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.LocationReferenceTypes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LocationReferenceTypesItemSet.Clear();
            //Actions
            var ActionsItemSet = mod.Actions.ToHashSet();
            foreach (var sourceForm in ActionsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Actions: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Actions: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Actions.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ActionsItemSet.Clear();
            //TextureSets
            var TextureSetsItemSet = mod.TextureSets.ToHashSet();
            foreach (var sourceForm in TextureSetsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("TextureSets: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("TextureSets: Duplicating to " + formCopy.FormKey.ToString());
                        mod.TextureSets.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            TextureSetsItemSet.Clear();
            //Globals
            var GlobalsItemSet = mod.Globals.ToHashSet();
            foreach (var sourceForm in GlobalsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Globals: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Globals: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Globals.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            GlobalsItemSet.Clear();
            //Classes
            var ClassesItemSet = mod.Classes.ToHashSet();
            foreach (var sourceForm in ClassesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Classes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Classes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Classes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ClassesItemSet.Clear();
            //Factions
            var FactionsItemSet = mod.Factions.ToHashSet();
            foreach (var sourceForm in FactionsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Factions: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Factions: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Factions.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            FactionsItemSet.Clear();
            //HeadParts
            var HeadPartsItemSet = mod.HeadParts.ToHashSet();
            foreach (var sourceForm in HeadPartsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("HeadParts: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("HeadParts: Duplicating to " + formCopy.FormKey.ToString());
                        mod.HeadParts.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            HeadPartsItemSet.Clear();
            //Hairs
            var HairsItemSet = mod.Hairs.ToHashSet();
            foreach (var sourceForm in HairsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Hairs: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Hairs: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Hairs.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            HairsItemSet.Clear();
            //Eyes
            var EyesItemSet = mod.Eyes.ToHashSet();
            foreach (var sourceForm in EyesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Eyes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Eyes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Eyes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            EyesItemSet.Clear();
            //Races
            var RacesItemSet = mod.Races.ToHashSet();
            foreach (var sourceForm in RacesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Races: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Races: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Races.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            RacesItemSet.Clear();
            //SoundMarkers
            var SoundMarkersItemSet = mod.SoundMarkers.ToHashSet();
            foreach (var sourceForm in SoundMarkersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("SoundMarkers: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("SoundMarkers: Duplicating to " + formCopy.FormKey.ToString());
                        mod.SoundMarkers.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            SoundMarkersItemSet.Clear();
            //AcousticSpaces
            var AcousticSpacesItemSet = mod.AcousticSpaces.ToHashSet();
            foreach (var sourceForm in AcousticSpacesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("AcousticSpaces: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("AcousticSpaces: Duplicating to " + formCopy.FormKey.ToString());
                        mod.AcousticSpaces.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            AcousticSpacesItemSet.Clear();
            //MagicEffects
            var MagicEffectsItemSet = mod.MagicEffects.ToHashSet();
            foreach (var sourceForm in MagicEffectsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("MagicEffects: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("MagicEffects: Duplicating to " + formCopy.FormKey.ToString());
                        mod.MagicEffects.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MagicEffectsItemSet.Clear();
            //LandscapeTextures
            var LandscapeTexturesItemSet = mod.LandscapeTextures.ToHashSet();
            foreach (var sourceForm in LandscapeTexturesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("LandscapeTextures: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("LandscapeTextures: Duplicating to " + formCopy.FormKey.ToString());
                        mod.LandscapeTextures.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LandscapeTexturesItemSet.Clear();
            //ObjectEffects
            var ObjectEffectsItemSet = mod.ObjectEffects.ToHashSet();
            foreach (var sourceForm in ObjectEffectsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ObjectEffects: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ObjectEffects: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ObjectEffects.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ObjectEffectsItemSet.Clear();
            //Spells
            var SpellsItemSet = mod.Spells.ToHashSet();
            foreach (var sourceForm in SpellsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Spells: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Spells: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Spells.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            SpellsItemSet.Clear();
            //Scrolls
            var ScrollsItemSet = mod.Scrolls.ToHashSet();
            foreach (var sourceForm in ScrollsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Scrolls: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Scrolls: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Scrolls.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ScrollsItemSet.Clear();
            //Activators
            var ActivatorsItemSet = mod.Activators.ToHashSet();
            foreach (var sourceForm in ActivatorsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Activators: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Activators: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Activators.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ActivatorsItemSet.Clear();
            //TalkingActivators
            var TalkingActivatorsItemSet = mod.TalkingActivators.ToHashSet();
            foreach (var sourceForm in TalkingActivatorsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("TalkingActivators: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("TalkingActivators: Duplicating to " + formCopy.FormKey.ToString());
                        mod.TalkingActivators.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            TalkingActivatorsItemSet.Clear();
            //Armors
            var ArmorsItemSet = mod.Armors.ToHashSet();
            foreach (var sourceForm in ArmorsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Armors: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Armors: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Armors.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ArmorsItemSet.Clear();
            //Books
            var BooksItemSet = mod.Books.ToHashSet();
            foreach (var sourceForm in BooksItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Books: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Books: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Books.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            BooksItemSet.Clear();
            //Containers
            var ContainersItemSet = mod.Containers.ToHashSet();
            foreach (var sourceForm in ContainersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Containers: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Containers: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Containers.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ContainersItemSet.Clear();
            //Doors
            var DoorsItemSet = mod.Doors.ToHashSet();
            foreach (var sourceForm in DoorsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Doors: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Doors: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Doors.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            DoorsItemSet.Clear();
            //Ingredients
            var IngredientsItemSet = mod.Ingredients.ToHashSet();
            foreach (var sourceForm in IngredientsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Ingredients: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Ingredients: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Ingredients.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            IngredientsItemSet.Clear();
            //Lights
            var LightsItemSet = mod.Lights.ToHashSet();
            foreach (var sourceForm in LightsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Lights: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Lights: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Lights.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LightsItemSet.Clear();
            //MiscItems
            var MiscItemsItemSet = mod.MiscItems.ToHashSet();
            foreach (var sourceForm in MiscItemsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("MiscItems: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("MiscItems: Duplicating to " + formCopy.FormKey.ToString());
                        mod.MiscItems.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MiscItemsItemSet.Clear();
            //AlchemicalApparatuses
            var AlchemicalApparatusesItemSet = mod.AlchemicalApparatuses.ToHashSet();
            foreach (var sourceForm in AlchemicalApparatusesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("AlchemicalApparatuses: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("AlchemicalApparatuses: Duplicating to " + formCopy.FormKey.ToString());
                        mod.AlchemicalApparatuses.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            AlchemicalApparatusesItemSet.Clear();
            //Statics
            var StaticsItemSet = mod.Statics.ToHashSet();
            foreach (var sourceForm in StaticsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Statics: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Statics: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Statics.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            StaticsItemSet.Clear();
            //MoveableStatics
            var MoveableStaticsItemSet = mod.MoveableStatics.ToHashSet();
            foreach (var sourceForm in MoveableStaticsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("MoveableStatics: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("MoveableStatics: Duplicating to " + formCopy.FormKey.ToString());
                        mod.MoveableStatics.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MoveableStaticsItemSet.Clear();
            //Grasses
            var GrassesItemSet = mod.Grasses.ToHashSet();
            foreach (var sourceForm in GrassesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Grasses: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Grasses: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Grasses.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            GrassesItemSet.Clear();
            //Trees
            var TreesItemSet = mod.Trees.ToHashSet();
            foreach (var sourceForm in TreesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Trees: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Trees: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Trees.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            TreesItemSet.Clear();
            //Florae
            var FloraeItemSet = mod.Florae.ToHashSet();
            foreach (var sourceForm in FloraeItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Florae: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Florae: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Florae.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            FloraeItemSet.Clear();
            //Furniture
            var FurnitureItemSet = mod.Furniture.ToHashSet();
            foreach (var sourceForm in FurnitureItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Furniture: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Furniture: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Furniture.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            FurnitureItemSet.Clear();
            //Weapons
            var WeaponsItemSet = mod.Weapons.ToHashSet();
            foreach (var sourceForm in WeaponsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Weapons: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Weapons: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Weapons.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            WeaponsItemSet.Clear();
            //Ammunitions
            var AmmunitionsItemSet = mod.Ammunitions.ToHashSet();
            foreach (var sourceForm in AmmunitionsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Ammunitions: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Ammunitions: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Ammunitions.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            AmmunitionsItemSet.Clear();
            //Npcs
            var NpcsItemSet = mod.Npcs.ToHashSet();
            foreach (var sourceForm in NpcsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Npcs: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Npcs: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Npcs.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            NpcsItemSet.Clear();
            //LeveledNpcs
            var LeveledNpcsItemSet = mod.LeveledNpcs.ToHashSet();
            foreach (var sourceForm in LeveledNpcsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("LeveledNpcs: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("LeveledNpcs: Duplicating to " + formCopy.FormKey.ToString());
                        mod.LeveledNpcs.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LeveledNpcsItemSet.Clear();
            //Keys
            var KeysItemSet = mod.Keys.ToHashSet();
            foreach (var sourceForm in KeysItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Keys: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Keys: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Keys.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            KeysItemSet.Clear();
            //Ingestibles
            var IngestiblesItemSet = mod.Ingestibles.ToHashSet();
            foreach (var sourceForm in IngestiblesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Ingestibles: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Ingestibles: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Ingestibles.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            IngestiblesItemSet.Clear();
            //IdleMarkers
            var IdleMarkersItemSet = mod.IdleMarkers.ToHashSet();
            foreach (var sourceForm in IdleMarkersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("IdleMarkers: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("IdleMarkers: Duplicating to " + formCopy.FormKey.ToString());
                        mod.IdleMarkers.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            IdleMarkersItemSet.Clear();
            //ConstructibleObjects
            var ConstructibleObjectsItemSet = mod.ConstructibleObjects.ToHashSet();
            foreach (var sourceForm in ConstructibleObjectsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ConstructibleObjects: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ConstructibleObjects: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ConstructibleObjects.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ConstructibleObjectsItemSet.Clear();
            //Projectiles
            var ProjectilesItemSet = mod.Projectiles.ToHashSet();
            foreach (var sourceForm in ProjectilesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Projectiles: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Projectiles: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Projectiles.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ProjectilesItemSet.Clear();
            //Hazards
            var HazardsItemSet = mod.Hazards.ToHashSet();
            foreach (var sourceForm in HazardsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Hazards: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Hazards: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Hazards.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            HazardsItemSet.Clear();
            //SoulGems
            var SoulGemsItemSet = mod.SoulGems.ToHashSet();
            foreach (var sourceForm in SoulGemsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("SoulGems: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("SoulGems: Duplicating to " + formCopy.FormKey.ToString());
                        mod.SoulGems.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            SoulGemsItemSet.Clear();
            //LeveledItems
            var LeveledItemsItemSet = mod.LeveledItems.ToHashSet();
            foreach (var sourceForm in LeveledItemsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("LeveledItems: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("LeveledItems: Duplicating to " + formCopy.FormKey.ToString());
                        mod.LeveledItems.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LeveledItemsItemSet.Clear();
            //Weathers
            var WeathersItemSet = mod.Weathers.ToHashSet();
            foreach (var sourceForm in WeathersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Weathers: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Weathers: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Weathers.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            WeathersItemSet.Clear();
            //Climates
            var ClimatesItemSet = mod.Climates.ToHashSet();
            foreach (var sourceForm in ClimatesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Climates: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Climates: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Climates.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ClimatesItemSet.Clear();
            //ShaderParticleGeometries
            var ShaderParticleGeometriesItemSet = mod.ShaderParticleGeometries.ToHashSet();
            foreach (var sourceForm in ShaderParticleGeometriesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ShaderParticleGeometries: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ShaderParticleGeometries: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ShaderParticleGeometries.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ShaderParticleGeometriesItemSet.Clear();
            //VisualEffects
            var VisualEffectsItemSet = mod.VisualEffects.ToHashSet();
            foreach (var sourceForm in VisualEffectsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("VisualEffects: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("VisualEffects: Duplicating to " + formCopy.FormKey.ToString());
                        mod.VisualEffects.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            VisualEffectsItemSet.Clear();
            //Regions
            var RegionsItemSet = mod.Regions.ToHashSet();
            foreach (var sourceForm in RegionsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Regions: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Regions: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Regions.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            RegionsItemSet.Clear();
            //NavigationMeshInfoMaps
            var NavigationMeshInfoMapsItemSet = mod.NavigationMeshInfoMaps.ToHashSet();
            foreach (var sourceForm in NavigationMeshInfoMapsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("NavigationMeshInfoMaps: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("NavigationMeshInfoMaps: Duplicating to " + formCopy.FormKey.ToString());
                        mod.NavigationMeshInfoMaps.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            NavigationMeshInfoMapsItemSet.Clear();
            //Quests
            var QuestsItemSet = mod.Quests.ToHashSet();
            foreach (var sourceForm in QuestsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Quests: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Quests: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Quests.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            QuestsItemSet.Clear();
            //IdleAnimations
            var IdleAnimationsItemSet = mod.IdleAnimations.ToHashSet();
            foreach (var sourceForm in IdleAnimationsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("IdleAnimations: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("IdleAnimations: Duplicating to " + formCopy.FormKey.ToString());
                        mod.IdleAnimations.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            IdleAnimationsItemSet.Clear();
            //Packages
            var PackagesItemSet = mod.Packages.ToHashSet();
            foreach (var sourceForm in PackagesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Packages: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Packages: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Packages.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            PackagesItemSet.Clear();
            //CombatStyles
            var CombatStylesItemSet = mod.CombatStyles.ToHashSet();
            foreach (var sourceForm in CombatStylesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("CombatStyles: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("CombatStyles: Duplicating to " + formCopy.FormKey.ToString());
                        mod.CombatStyles.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            CombatStylesItemSet.Clear();
            //LoadScreens
            var LoadScreensItemSet = mod.LoadScreens.ToHashSet();
            foreach (var sourceForm in LoadScreensItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("LoadScreens: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("LoadScreens: Duplicating to " + formCopy.FormKey.ToString());
                        mod.LoadScreens.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LoadScreensItemSet.Clear();
            //LeveledSpells
            var LeveledSpellsItemSet = mod.LeveledSpells.ToHashSet();
            foreach (var sourceForm in LeveledSpellsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("LeveledSpells: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("LeveledSpells: Duplicating to " + formCopy.FormKey.ToString());
                        mod.LeveledSpells.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LeveledSpellsItemSet.Clear();
            //AnimatedObjects
            var AnimatedObjectsItemSet = mod.AnimatedObjects.ToHashSet();
            foreach (var sourceForm in AnimatedObjectsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("AnimatedObjects: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("AnimatedObjects: Duplicating to " + formCopy.FormKey.ToString());
                        mod.AnimatedObjects.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            AnimatedObjectsItemSet.Clear();
            //Waters
            var WatersItemSet = mod.Waters.ToHashSet();
            foreach (var sourceForm in WatersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Waters: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Waters: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Waters.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            WatersItemSet.Clear();
            //EffectShaders
            var EffectShadersItemSet = mod.EffectShaders.ToHashSet();
            foreach (var sourceForm in EffectShadersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("EffectShaders: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("EffectShaders: Duplicating to " + formCopy.FormKey.ToString());
                        mod.EffectShaders.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            EffectShadersItemSet.Clear();
            //Explosions
            var ExplosionsItemSet = mod.Explosions.ToHashSet();
            foreach (var sourceForm in ExplosionsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Explosions: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Explosions: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Explosions.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ExplosionsItemSet.Clear();
            //Debris
            var DebrisItemSet = mod.Debris.ToHashSet();
            foreach (var sourceForm in DebrisItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Debris: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Debris: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Debris.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            DebrisItemSet.Clear();
            //ImageSpaces
            var ImageSpacesItemSet = mod.ImageSpaces.ToHashSet();
            foreach (var sourceForm in ImageSpacesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ImageSpaces: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ImageSpaces: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ImageSpaces.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ImageSpacesItemSet.Clear();
            //ImageSpaceAdapters
            var ImageSpaceAdaptersItemSet = mod.ImageSpaceAdapters.ToHashSet();
            foreach (var sourceForm in ImageSpaceAdaptersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ImageSpaceAdapters: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ImageSpaceAdapters: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ImageSpaceAdapters.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ImageSpaceAdaptersItemSet.Clear();
            //FormLists
            var FormListsItemSet = mod.FormLists.ToHashSet();
            foreach (var sourceForm in FormListsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("FormLists: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("FormLists: Duplicating to " + formCopy.FormKey.ToString());
                        mod.FormLists.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            FormListsItemSet.Clear();
            //Perks
            var PerksItemSet = mod.Perks.ToHashSet();
            foreach (var sourceForm in PerksItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Perks: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Perks: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Perks.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            PerksItemSet.Clear();
            //BodyParts
            var BodyPartsItemSet = mod.BodyParts.ToHashSet();
            foreach (var sourceForm in BodyPartsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("BodyParts: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("BodyParts: Duplicating to " + formCopy.FormKey.ToString());
                        mod.BodyParts.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            BodyPartsItemSet.Clear();
            //AddonNodes
            var AddonNodesItemSet = mod.AddonNodes.ToHashSet();
            foreach (var sourceForm in AddonNodesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("AddonNodes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("AddonNodes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.AddonNodes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            AddonNodesItemSet.Clear();
            //ActorValueInformation
            var ActorValueInformationItemSet = mod.ActorValueInformation.ToHashSet();
            foreach (var sourceForm in ActorValueInformationItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ActorValueInformation: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ActorValueInformation: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ActorValueInformation.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ActorValueInformationItemSet.Clear();
            //CameraShots
            var CameraShotsItemSet = mod.CameraShots.ToHashSet();
            foreach (var sourceForm in CameraShotsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("CameraShots: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("CameraShots: Duplicating to " + formCopy.FormKey.ToString());
                        mod.CameraShots.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            CameraShotsItemSet.Clear();
            //CameraPaths
            var CameraPathsItemSet = mod.CameraPaths.ToHashSet();
            foreach (var sourceForm in CameraPathsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("CameraPaths: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("CameraPaths: Duplicating to " + formCopy.FormKey.ToString());
                        mod.CameraPaths.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            CameraPathsItemSet.Clear();
            //VoiceTypes
            var VoiceTypesItemSet = mod.VoiceTypes.ToHashSet();
            foreach (var sourceForm in VoiceTypesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("VoiceTypes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("VoiceTypes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.VoiceTypes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            VoiceTypesItemSet.Clear();
            //MaterialTypes
            var MaterialTypesItemSet = mod.MaterialTypes.ToHashSet();
            foreach (var sourceForm in MaterialTypesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("MaterialTypes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("MaterialTypes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.MaterialTypes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MaterialTypesItemSet.Clear();
            //Impacts
            var ImpactsItemSet = mod.Impacts.ToHashSet();
            foreach (var sourceForm in ImpactsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Impacts: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Impacts: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Impacts.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ImpactsItemSet.Clear();
            //ImpactDataSets
            var ImpactDataSetsItemSet = mod.ImpactDataSets.ToHashSet();
            foreach (var sourceForm in ImpactDataSetsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ImpactDataSets: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ImpactDataSets: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ImpactDataSets.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ImpactDataSetsItemSet.Clear();
            //ArmorAddons
            var ArmorAddonsItemSet = mod.ArmorAddons.ToHashSet();
            foreach (var sourceForm in ArmorAddonsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ArmorAddons: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ArmorAddons: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ArmorAddons.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ArmorAddonsItemSet.Clear();
            //EncounterZones
            var EncounterZonesItemSet = mod.EncounterZones.ToHashSet();
            foreach (var sourceForm in EncounterZonesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("EncounterZones: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("EncounterZones: Duplicating to " + formCopy.FormKey.ToString());
                        mod.EncounterZones.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            EncounterZonesItemSet.Clear();
            //Locations
            var LocationsItemSet = mod.Locations.ToHashSet();
            foreach (var sourceForm in LocationsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Locations: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Locations: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Locations.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LocationsItemSet.Clear();
            //Messages
            var MessagesItemSet = mod.Messages.ToHashSet();
            foreach (var sourceForm in MessagesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Messages: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Messages: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Messages.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MessagesItemSet.Clear();
            //DefaultObjectManagers
            var DefaultObjectManagersItemSet = mod.DefaultObjectManagers.ToHashSet();
            foreach (var sourceForm in DefaultObjectManagersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("DefaultObjectManagers: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("DefaultObjectManagers: Duplicating to " + formCopy.FormKey.ToString());
                        mod.DefaultObjectManagers.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            DefaultObjectManagersItemSet.Clear();
            //LightingTemplates
            var LightingTemplatesItemSet = mod.LightingTemplates.ToHashSet();
            foreach (var sourceForm in LightingTemplatesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("LightingTemplates: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("LightingTemplates: Duplicating to " + formCopy.FormKey.ToString());
                        mod.LightingTemplates.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            LightingTemplatesItemSet.Clear();
            //MusicTypes
            var MusicTypesItemSet = mod.MusicTypes.ToHashSet();
            foreach (var sourceForm in MusicTypesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("MusicTypes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("MusicTypes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.MusicTypes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MusicTypesItemSet.Clear();
            //Footsteps
            var FootstepsItemSet = mod.Footsteps.ToHashSet();
            foreach (var sourceForm in FootstepsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Footsteps: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Footsteps: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Footsteps.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            FootstepsItemSet.Clear();
            //FootstepSets
            var FootstepSetsItemSet = mod.FootstepSets.ToHashSet();
            foreach (var sourceForm in FootstepSetsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("FootstepSets: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("FootstepSets: Duplicating to " + formCopy.FormKey.ToString());
                        mod.FootstepSets.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            FootstepSetsItemSet.Clear();
            //StoryManagerBranchNodes
            var StoryManagerBranchNodesItemSet = mod.StoryManagerBranchNodes.ToHashSet();
            foreach (var sourceForm in StoryManagerBranchNodesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("StoryManagerBranchNodes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("StoryManagerBranchNodes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.StoryManagerBranchNodes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            StoryManagerBranchNodesItemSet.Clear();
            //StoryManagerQuestNodes
            var StoryManagerQuestNodesItemSet = mod.StoryManagerQuestNodes.ToHashSet();
            foreach (var sourceForm in StoryManagerQuestNodesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("StoryManagerQuestNodes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("StoryManagerQuestNodes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.StoryManagerQuestNodes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            StoryManagerQuestNodesItemSet.Clear();
            //StoryManagerEventNodes
            var StoryManagerEventNodesItemSet = mod.StoryManagerEventNodes.ToHashSet();
            foreach (var sourceForm in StoryManagerEventNodesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("StoryManagerEventNodes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("StoryManagerEventNodes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.StoryManagerEventNodes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            StoryManagerEventNodesItemSet.Clear();
            //DialogBranches
            var DialogBranchesItemSet = mod.DialogBranches.ToHashSet();
            foreach (var sourceForm in DialogBranchesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("DialogBranches: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("DialogBranches: Duplicating to " + formCopy.FormKey.ToString());
                        mod.DialogBranches.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            DialogBranchesItemSet.Clear();
            //MusicTracks
            var MusicTracksItemSet = mod.MusicTracks.ToHashSet();
            foreach (var sourceForm in MusicTracksItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("MusicTracks: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("MusicTracks: Duplicating to " + formCopy.FormKey.ToString());
                        mod.MusicTracks.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MusicTracksItemSet.Clear();
            //DialogViews
            var DialogViewsItemSet = mod.DialogViews.ToHashSet();
            foreach (var sourceForm in DialogViewsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("DialogViews: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("DialogViews: Duplicating to " + formCopy.FormKey.ToString());
                        mod.DialogViews.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            DialogViewsItemSet.Clear();
            //WordsOfPower
            var WordsOfPowerItemSet = mod.WordsOfPower.ToHashSet();
            foreach (var sourceForm in WordsOfPowerItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("WordsOfPower: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("WordsOfPower: Duplicating to " + formCopy.FormKey.ToString());
                        mod.WordsOfPower.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            WordsOfPowerItemSet.Clear();
            //Shouts
            var ShoutsItemSet = mod.Shouts.ToHashSet();
            foreach (var sourceForm in ShoutsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Shouts: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Shouts: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Shouts.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ShoutsItemSet.Clear();
            //EquipTypes
            var EquipTypesItemSet = mod.EquipTypes.ToHashSet();
            foreach (var sourceForm in EquipTypesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("EquipTypes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("EquipTypes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.EquipTypes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            EquipTypesItemSet.Clear();
            //Relationships
            var RelationshipsItemSet = mod.Relationships.ToHashSet();
            foreach (var sourceForm in RelationshipsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Relationships: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Relationships: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Relationships.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            RelationshipsItemSet.Clear();
            //Scenes
            var ScenesItemSet = mod.Scenes.ToHashSet();
            foreach (var sourceForm in ScenesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Scenes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Scenes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Scenes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ScenesItemSet.Clear();
            //AssociationTypes
            var AssociationTypesItemSet = mod.AssociationTypes.ToHashSet();
            foreach (var sourceForm in AssociationTypesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("AssociationTypes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("AssociationTypes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.AssociationTypes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            AssociationTypesItemSet.Clear();
            //Outfits
            var OutfitsItemSet = mod.Outfits.ToHashSet();
            foreach (var sourceForm in OutfitsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Outfits: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Outfits: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Outfits.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            OutfitsItemSet.Clear();
            //ArtObjects
            var ArtObjectsItemSet = mod.ArtObjects.ToHashSet();
            foreach (var sourceForm in ArtObjectsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ArtObjects: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ArtObjects: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ArtObjects.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ArtObjectsItemSet.Clear();
            //MaterialObjects
            var MaterialObjectsItemSet = mod.MaterialObjects.ToHashSet();
            foreach (var sourceForm in MaterialObjectsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("MaterialObjects: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("MaterialObjects: Duplicating to " + formCopy.FormKey.ToString());
                        mod.MaterialObjects.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MaterialObjectsItemSet.Clear();
            //MovementTypes
            var MovementTypesItemSet = mod.MovementTypes.ToHashSet();
            foreach (var sourceForm in MovementTypesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("MovementTypes: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("MovementTypes: Duplicating to " + formCopy.FormKey.ToString());
                        mod.MovementTypes.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            MovementTypesItemSet.Clear();
            //SoundDescriptors
            var SoundDescriptorsItemSet = mod.SoundDescriptors.ToHashSet();
            foreach (var sourceForm in SoundDescriptorsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("SoundDescriptors: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("SoundDescriptors: Duplicating to " + formCopy.FormKey.ToString());
                        mod.SoundDescriptors.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            SoundDescriptorsItemSet.Clear();
            //DualCastData
            var DualCastDataItemSet = mod.DualCastData.ToHashSet();
            foreach (var sourceForm in DualCastDataItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("DualCastData: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("DualCastData: Duplicating to " + formCopy.FormKey.ToString());
                        mod.DualCastData.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            DualCastDataItemSet.Clear();
            //SoundCategories
            var SoundCategoriesItemSet = mod.SoundCategories.ToHashSet();
            foreach (var sourceForm in SoundCategoriesItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("SoundCategories: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("SoundCategories: Duplicating to " + formCopy.FormKey.ToString());
                        mod.SoundCategories.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            SoundCategoriesItemSet.Clear();
            //SoundOutputModels
            var SoundOutputModelsItemSet = mod.SoundOutputModels.ToHashSet();
            foreach (var sourceForm in SoundOutputModelsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("SoundOutputModels: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("SoundOutputModels: Duplicating to " + formCopy.FormKey.ToString());
                        mod.SoundOutputModels.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            SoundOutputModelsItemSet.Clear();
            //CollisionLayers
            var CollisionLayersItemSet = mod.CollisionLayers.ToHashSet();
            foreach (var sourceForm in CollisionLayersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("CollisionLayers: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("CollisionLayers: Duplicating to " + formCopy.FormKey.ToString());
                        mod.CollisionLayers.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            CollisionLayersItemSet.Clear();
            //Colors
            var ColorsItemSet = mod.Colors.ToHashSet();
            foreach (var sourceForm in ColorsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("Colors: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("Colors: Duplicating to " + formCopy.FormKey.ToString());
                        mod.Colors.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ColorsItemSet.Clear();
            //ReverbParameters
            var ReverbParametersItemSet = mod.ReverbParameters.ToHashSet();
            foreach (var sourceForm in ReverbParametersItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("ReverbParameters: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("ReverbParameters: Duplicating to " + formCopy.FormKey.ToString());
                        mod.ReverbParameters.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            ReverbParametersItemSet.Clear();
            //VolumetricLightings
            var VolumetricLightingsItemSet = mod.VolumetricLightings.ToHashSet();
            foreach (var sourceForm in VolumetricLightingsItemSet)
            {
                if (CompactedModDataD.TryGetValue(sourceForm.FormKey.ModKey.ToString(), out CompactedModData? compactedModData))
                {
                    FormKey formKey = HandleFormKeyFix(sourceForm.FormKey, compactedModData, out bool changed);
                    if (changed)
                    {
                        var formCopy = sourceForm.Duplicate(formKey);
                        DevLog.Log("VolumetricLightings: Removing " + sourceForm.FormKey.ToString());
                        mod.Remove(sourceForm.FormKey);
                        DevLog.Log("VolumetricLightings: Duplicating to " + formCopy.FormKey.ToString());
                        mod.VolumetricLightings.Add(formCopy);
                        ModEdited = true;
                        break;
                    }
                }
            }
            VolumetricLightingsItemSet.Clear();



            return mod;
        }
    }
}
