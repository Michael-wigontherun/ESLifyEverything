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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
                        mod.Remove(sourceForm.FormKey);
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
