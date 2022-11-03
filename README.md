# ESLifyEverything

**_Important:_** I want to replace xEdit's NIF tools with Nifly or NiflySharp, if anyone knows it or knows how to iterate the texture paths, please message me, I spent hours trying to figure it out to no avail.

## Overview

Many plugins do not need to be regular esp. The only reason they are still a regular esp is that the modder left and didn't open permissions, or they are required by things like DAR and SPID so no one wants to compact and update the SPID or DAR files. One of the exceptionally amazing ideas Bethesda had was to link Voice lines and FaceGen data to the formID instead of a file path inside the form, so larger follower mods are never going to be Eslified when they absolutely should be. A few years ago I created separate programs to handle Eslifing Voice lines and FaceGen data but it could not fix the texture path inside the NIF file. Then [MaskedRPGFan](https://www.nexusmods.com/skyrimspecialedition/users/22822094) created [ESPFE Follower - Eslify facegen and voices](https://www.nexusmods.com/skyrimspecialedition/mods/56396), which worked for some and others not really at all. There is one major issue with it, it can't handle multiple files. And one minor issue is it uses entirely xEdit scripting API, which is using a Language called Pascal. Nothing wrong with that of course, it is effective as a scripting language for xEdit's needs. However, it is slow and lacks many features, so expecting it to handle everything that Skyrim modding has and needs Eslified would be time-consuming and sloooooooooow.

## Description

ESLify Everything is a .Net 6 program that reads the data outputted by xEdit when it compacts forms using the Right click->Compact FormIDs for ESL. 

Currently Eslifies

- Recompacts plugins and updates Sub plugins
- Voice Files, Internally coded
- FaceGen data, Internally coded
- SPID, Base Object Swapper, KID, FLM, Animated Object Swapper
- Dynamic Animation Conditions
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

Message me via Nexus comments on this mod, or making a new issue on this mod's Github, for any other file mod data that needs to be eslified.

## Prerequisites
1. Install .Net 6 Desktop Runtime, This is the same as what Synthesis uses.
2. Install the Creation Kit from Steam, and use the [Skyrim Creation Kit Downgrade Patcher](https://www.nexusmods.com/skyrimspecialedition/mods/67096) if you are still running SE

     If you are a MO2 user, you can extract the new scripts.zip, located inside your Data folder, directly into your Data folder, or create a new mod with it, just make sure EVERYTHING IS OVERWRITING IT.

     If you are a non-MO2 user WITH MODS INSTALLED, you must not extract the scripts.zip, open it and find the file "TESV_Papyrus_Flags.flg". Take only this file and extract it to "Data\Source\Scripts"

     If you are a non-MO2 user with a fresh install of skyrim with no mods installed, extract scripts.zip into your data folder.

3. Download Champollion, it is on LE's page but as far as I know there is not a version specific to SE.

## Instillation

1. Extract my 2 folders into the folder containing SSEEdit.exe, ensuring the file "_ESLifyEverythingFaceGenFix.pas" is inside of SSEEdit's Edit Scripts Folder.

   The "ESLifyEverything" folder doesn't need to be included inside of SSEEdit's folder, but I place it there with the rest of my tools based on SSEEdit's data.

2. Extract Champollion into "ESLifyEverything\Champollion", Champollion.exe must be in this folder for Script ESLify to work.

3. If this is your first time using version 1.9.0+, ESLify Everything will generate a new AppSettings.json on your first run. For the release of 1.9.0, the new AppSettings.json is included.

4. Now inside of AppSettings.json, you have to set:
```
    "XEditFolderPath": "xEditFolderPath",
    "DataFolderPath": "Skyrim Special Edition\\Data",
    "OutputFolder": "MO2\\Mods\\OuputFolder"
```
You must use double \\ instead of single \ for the folder paths.

This is an example of mine
```
    "XEditFolderPath": "C:\\Modding\\SkyrimSE\\sseedit",
    "DataFolderPath": "C:\\Steam\\steamapps\\common\\Skyrim Special Edition\\Data",
    "OutputFolder": "E:\\SkyrimMods\\MO2\\mods\\ESLify Everything"
```


### Extra Settings
```
"PapyrusFlag": "TESV_Papyrus_Flags.flg",      : Do not Edit, unless you are trying to use this on Fallout 4
"XEditLogFileName": "SSEEdit_log.txt",        : Do not Edit, unless you are trying to use this on Fallout 4
"VerboseConsoleLoging": true,                 : Enables/Disables Verbose logging to the console
"VerboseFileLoging": true,                    : Enables/Disables Verbose logging to the console
"AutoReadNewestxEditSeesion": false,          : Enables/Disables Skips menu and imports only the first xEdit session data
"AutoReadAllxEditSeesion": false,             : Enables/Disables Skips menu and imports all session data
"AutoRunESLify": false,                       : Enables/Disables Skips FaceGen and Voice ESLify menus and runs over all mods
"AutoRunScriptDecompile": false,              : Enables/Disables Skips Script menu and decompiles all scripts
"DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion": false, : Enables/Disables Delete xEdit log after import
```

## Directions

1.  Run xEdit and compact what ever mods you want, THAT CAN BE ESLIFIED, and set the esl flag on the plugin.
        If it there is a SEQ file for the plugin, right click on the plugin in xEdit, at the bottom of the menu go to Other -> Create SEQ file.
        How to determine if the plugin has a SEQ file it will be in your Data\seq\[Plugin file name].seq

2.  Close xEdit

3.  Run ESLifyEverything.exe

4.  If there is an error inside the config it will warn you and not continue to Eslify. If the only issue is ESLifyEverything can not locate the xEdit log it will skip the xEdit session entirely, if there are no Mod data files this will simply not do anything. 

5.  If you have "AutoReadAllxEditSeesion" or "AutoReadNewestxEditSeesion" set to true you can skip this step. This is a console menu that lets you select which session you want to read and output Compacted mod data. Input the menu item number then press enter.

Skip Steps 6, 7 and 8, if you want to use "AutoRunESLify"

6.  This Console menu is for Voice ESLify. You have 2 options:
    1. Eslifies every mod: Eslifies every mod that has compacted mod data attached to it
    2. Single mod input menu: takes you to a new console menu 
            This new menu only needs you to input the plugin name and extension with compacted mod data inside of your SkyrimDatafolder\CompactedForms or OptionalOutputfolder\CompactedForms, you only need to input the plugin name with extension
            Example: File:"ArteFake.esp_ESlEverything.json" input only "ArteFake.esp" 
                   ArteFake does not have any voice files or FaceGen, this is just an example

To leave a menu or skip it enter "XXX", this is not case sensitive. 

7.  Is identical to step 6, but is for FaceGen ESLify.

8.  Is identical to step 6, but is for Mod Data Files ESLify.

9.  Once you reach the Scripts Eslify, it will ask if you have installed any new mods or updated a mod since the last time you ran ESLify Everything 1.9.0.
    - Press N if you have not and it will skip decompiling scripts and go strait to Eslifing the scripts that it already knows.
    - If ESLify Everything does not detect any scripts inside of "ExtractedBSAModData\Source\Scripts" it will auto run the decompile process.

10.  Wait until ESLify Everything ends. "Press Enter to exit..." this is what you should see at the end of the code. Please verify that you did not get any errors at the end of the console.

## Extra Directions
Before each run of Eslify everything, you need to clear the outputted scripts. This needs to happen in order to protect data integrity. Script Eslify needs to decompile the BSA scripts and the Loose scripts in order to Eslify and decompile, because there is no way I could incorporate a optimized and scripted intellisense to find what scripts need to be decompiled and recompiled each run.

If you ever want to reset your AppSettings.json because you think you accidentally messed up something you can't see, just delete AppSettings.json and rerun the ESLify Everything.

Changed scripts are stored first at "Eslify everything\ChangedScripts" and if the script fails to compile a copy will be moved to "OutputFolder\Source\Scripts".
- If you come across a script that fails to recompile, Report it as a bug with the: script name, mod it originated from, and the error message in the console.
- I will not download anything.
- Take screenshots of the log lines between Important < and > Important, then use Imgur or another image sharing system.
![Image of log lines I need](https://i.postimg.cc/NjXMBgYN/Needed-Failed-Script-Logging-Line.png)
- This Data is important to identify what checks Script Eslify needs in place before attempting to run the compiler.
## Extra Notes

Releases are always on GitHub before Nexus

## Credits
[MaskedRPGFan](https://www.nexusmods.com/skyrimspecialedition/users/22822094) for allowing me to copy some of his code inside [ESPFE Follower - Eslify facegen and voices](https://www.nexusmods.com/skyrimspecialedition/mods/56396) specifically the CreateFaceMesh procedure

[ElminsterAU](https://www.nexusmods.com/skyrimspecialedition/users/167469) and the xEdit team for [SEEdit](https://www.nexusmods.com/skyrimspecialedition/mods/164)

[AlexxEG](https://www.nexusmods.com/skyrimspecialedition/users/1115735) for allowing me to including [BSA Browser](https://www.nexusmods.com/skyrimspecialedition/mods/1756) command line exe, to extract BSA data

## Tools used

Visual Studio 2022

Visual Studio Code

NotePad++
