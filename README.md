# ESLifyEverything

**_Important:_** I want to replace xEdit's NIF tools with Nifly or NiflySharp, if anyone knows it or knows how to iterate the texture paths, please message me, I spent hours trying to figure it out to no avail.

## Overview

Many plugins do not need to be regular esp. The only reason they are still a regular esp is that the modder left and didn't open permissions, or they are required by things like DAR and SPID so no one wants to compact and update the SPID or DAR files. One of the exceptionally amazing ideas Bethesda had was to link Voice lines and FaceGen data to the formID instead of a file path inside the form, so larger follower mods are never going to be Eslified when they absolutely should be. A few years ago I created separate programs to handle Eslifing Voice lines and FaceGen data but it could not fix the texture path inside the NIF file. Then MaskedRPGFan created ESPFE Follower - Eslify facegen and voices, which worked for some and others not really at all. There is one major issue with it, it can't handle multiple files. And one minor issue is it uses entirely xEdit scripting API, which is using a Language called Pascal. Nothing wrong with that of course, it is effective as a scripting language for xEdit's needs. However, it is slow and lacks many features, so expecting it to handle everything that Skyrim modding has and needs Eslified would be time-consuming and sloooooooooow.

## Description

ESLify Everything is a .Net 6 program that reads the data outputted by xEdit when it compacts forms using the Right click->Compact FormIDs for ESL. 

Currently Eslifies

    -Voice Files
    -FaceGen data
    -SPID
    -Dynamic Animation Conditions

Message me via Nexus comments on this mod, or making a new issue on this mod's Github.

## Directions

Inside of this Article found in the article sections on the Nexus page.

### Extra Notes

Releases are always on GitHub before Nexus
