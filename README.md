# Please use [ESLifier](https://www.nexusmods.com/skyrimspecialedition/mods/145168) instead.
Due to work related developments and a moral drop from Nexus, I stopped developing this a while back. I tried to fix some things for the new updates to skyrim but I never could never find any more joy in developing ESLify Everything. Thank you everyone who supported me and encuriged me to make and further develop this before.

ESLifier has support for a lot more and it has a much better user experience then this. I am really happy someone continued the cause for streamlined large load orders.

# ESLifyEverything

## Overview

Many plugins do not need to be regular esp. The only reason they are still a regular esp is that the modder left and didn't open permissions, or they are required by things like DAR and SPID so no one wants to compact and update the SPID or DAR files. One of the exceptionally amazing ideas Bethesda had was to link Voice lines and FaceGen data to the formID instead of a file path inside the form, so larger follower mods are never going to be Eslified when they absolutely should be. A few years ago I created separate programs to handle Eslifing Voice lines and FaceGen data but it could not fix the texture path inside the NIF file. Then [MaskedRPGFan](https://www.nexusmods.com/skyrimspecialedition/users/22822094) created [ESPFE Follower - Eslify facegen and voices](https://www.nexusmods.com/skyrimspecialedition/mods/56396), which worked for some and others not really at all. There is one major issue with it, it can't handle multiple files. And one minor issue is it uses entirely xEdit scripting API, which is using a Language called Pascal. Nothing wrong with that of course, it is effective as a scripting language for xEdit's needs. However, it is slow and lacks many features, so expecting it to handle everything that Skyrim modding has and needs Eslified would be time-consuming and sloooooooooow.

## Description

ESLify Everything is a .Net 6 program that reads the data outputted by xEdit when it compacts forms using the Right click->Compact FormIDs for ESL. 

[ESLify, For people who actually want to understand](https://github.com/Michael-wigontherun/ESLifyEverything/wiki/ESLify,-For-people-who-actually-want-to-understand)

Currently Eslifies

- Recompacts plugins and updates Sub plugins
- Voice Files, Internally coded
- FaceGen data, Internally coded
- SPID, Base Object Swapper, KID, FLM, Animated Object Swapper
- Dynamic Animation Replacer Conditions
- Open Animation Replacer config.json
- ENB Lights For Effect Shaders
- AutoBody
- Payload Interpreter
- SKSE INI files
- Custom Skills Framework \ Extended Framework, Internally coded
- Papyrus Scripts, Internally coded
- Precision
- True Directional Movement
- POISE - Stagger Overhaul
- Racemenu Presets, Internally coded
- CoMap MapMarker json files
- VSU
- Sound Record Distributor
- Dynamic Armor Varients
- OBody NG, Internally coded
- Description Framework

Message me via Nexus comments on this mod, or making a new issue on this mod's Github, for any other file mod data that needs to be eslified.

# Directions
Version 4.0.0+ directions: [Article](https://github.com/Michael-wigontherun/ESLifyEverything/wiki/Directions)

ESLify Split - Directions: [Article](https://github.com/Michael-wigontherun/ESLifyEverything/wiki/ESLify-Split-‐-Directions)

ESLify Everything zMerge Support: [Article](https://github.com/Michael-wigontherun/ESLifyEverything/wiki/ESLify-Everything-zMerge-Support-and-MergifyBashTags)

Additional Features list: [Article](https://github.com/Michael-wigontherun/ESLifyEverything/wiki/Additional-Features-list)

## Extra Notes

Releases are always on GitHub before Nexus

## Credits
[MaskedRPGFan](https://www.nexusmods.com/skyrimspecialedition/users/22822094) for allowing me to copy some of his code inside [ESPFE Follower - Eslify facegen and voices](https://www.nexusmods.com/skyrimspecialedition/mods/56396) specifically the CreateFaceMesh procedure

[ElminsterAU](https://www.nexusmods.com/skyrimspecialedition/users/167469) and the xEdit team for [SEEdit](https://www.nexusmods.com/skyrimspecialedition/mods/164)

[AlexxEG](https://www.nexusmods.com/skyrimspecialedition/users/1115735) for allowing me to including [BSA Browser](https://www.nexusmods.com/skyrimspecialedition/mods/1756) command line exe, to extract BSA data

[Jampi0n](https://www.nexusmods.com/skyrimspecialedition/users/25815700) for showing me how to use NiflySharp and allowing me to include his [NifWrapper.cs](https://github.com/Jampi0n/Skyrim-NifPatcher/blob/main/NifPatcher/NifWrapper.cs) class file.

[Orvid's Champollion team](https://github.com/Orvid/Champollion) They have fixed many things in Champollion, as well as allowing me to included in the download.

## Tools used

Visual Studio 2022

Visual Studio Code

NotePad++
