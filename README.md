# ESLifyEverything

**_Important:_** I want to replace xEdit's NIF tools with Nifly or NiflySharp, if anyone knows it or knows how to iterate the texture paths, please message me, I spent hours trying to figure it out to no avail.

## Overview

Many plugins do not need to be regular esp. The only reason they are still a regular esp is that the modder left and didn't open permissions, or they are required by things like DAR and SPID so no one wants to compact and update the SPID or DAR files. One of the exceptionally amazing ideas Bethesda had was to link Voice lines and FaceGen data to the formID instead of a file path inside the form, so larger follower mods are never going to be Eslified when they absolutely should be. A few years ago I created separate programs to handle Eslifing Voice lines and FaceGen data but it could not fix the texture path inside the NIF file. Then MaskedRPGFan created ESPFE Follower - Eslify facegen and voices, which worked for some and others not really at all. There is one major issue with it, it can't handle multiple files. And one minor issue is it uses entirely xEdit scripting API, which is using a Language called Pascal. Nothing wrong with that of course, it is effective as a scripting language for xEdit's needs. However, it is slow and lacks many features, so expecting it to handle everything that Skyrim modding has and needs Eslified would be time-consuming and sloooooooooow.

## Description

ESLify Everything is a .Net 6 program that reads the data outputted by xEdit when it compacts forms using the Right click->Compact FormIDs for ESL. 

Currently Eslifies

- Voice Files
- FaceGen data
- SPID, Base Object Swapper, KID, FLM, Animated Object Swapper
- Dynamic Animation Conditions
- ENB Lights For Effect Shaders
- AutoBody
- Payload Interpreter
- SKSE INI files
- Custom Skills Framework \ Extended Framework

Message me via Nexus comments on this mod, or making a new issue on this mod's Github, for any other file mod data that needs to be eslified.

## Instillation

1. Extract my 2 folders into the folder containing SSEEdit.exe, ensuring the file "_ESLifyEverythingFaceGenFix.pas" is inside of SSEEdit's Edit Scripts Folder.
The "ESLifyEverything" folder doesn't exactly need to be included inside of SSEEdit's folder, but I place it there with the rest of my tools based on SSEEdit's data.

2. Now open the AppSettings.json file and set value of "XEditFolderPath": "xEditFolder", to be the full path to SSEEdit's folder, NOT THE EXE. Ensure " are needed around it and the path's \ need to be doubled \\
Example of mine: "XEditFolderPath": "C:\\Modding\\SkyrimSE\\sseedit",

3. Do not change: "XEditLogFileName": "SSEEdit_log.txt", unless you want to try this with Fallout 4, I do not know what will work and what will not work for Fallout 4

4. "SkyrimDataFolderPath": "Skyrim Special Edition\\Data", needs to be directed towards Skyrim's Data folder like
Example of mine: "SkyrimDataFolderPath": "C:\\Steam\\steamapps\\common\\Skyrim Special Edition\\Data",

5. Skip this step if you use something other then MO2. If you are a MO2 user you can skip using the Optional Output folder by settings "OutputToOptionalFolder": true, to false like, "OutputToOptionalFolder": false, and skip step 6. You do not need to do this if your a MO2 user but MO2 already has a Output new files to mod instead of overwrite.
But there is a benefit to the OutputToOptionalFolder if you don't want the regenerated files to overwrite the original.

6. "OptionalOutputFolder": ".\\OuputFolder" Set this to a folder in your MO2 mods if you want to use the Optional Output Folder. Or where ever you can easily access them from Vortex and NMM.
Example of Mine: "OptionalOutputFolder": "E:\\SkyrimMods\\MO2\\mods\\ESLify Everything"

### Extra Settings

- "VerboseConsoleLoging": true,                                       logs everything to console

- "VerboseFileLoging": true,                                          logs everything to the "ESLifyEverything_Log.txt" file

- "AutoReadAllxEditSeesion": true,                                    Outputs all compacted Mod files from xEdit sessions inside of the "SSEEdit_log.txt" 

- "AutoReadNewestxEditSeesion": true,                                 Outputs Compacted Mod files from only the newest xEdit session, if there are no compacted forms then it will output nothing

- "DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion": true,    Will delete the "SSEEdit_log.txt" file after parsing and outputing the Compacted mods. This requires "AutoReadAllxEditSeesion" to be set to true as well to avoid the loss of data

- "AutoRunESLify": true,                                              will skip the ESLify menus to and just run everything
This can take some time depending on how many mods you have compacted and how many voice files in total, but I believe I have optimized this well enough to not hurt to much. My load order with 50ish mods does not make the program start pause its output at all

## Directions

1. Run xEdit and compact what ever mods you want, THAT CAN BE ESLIFIED, and set the esl flag on the plugin.

2. Close xEdit, xEdit wont output the log until you close it completely.

3. Run "ESLifyEverything.exe"

4. If there is an error inside the config it will warn you and not continue to Eslify. If the only issue is ESLifyEverything can not locate the xEdit log it will skip the xEdit session entirely, if there are no Mod data files this will simply not do anything. 

5. If you have "AutoReadAllxEditSeesion" or "AutoReadNewestxEditSeesion" set to true you can skip this step. This is a console menu that contains lets you select which session you want to read and output Compacted mod data. input the menu item number then press enter

- Skip Steps 6 and 7 if you want to use "AutoRunESLify"

6. This Console menu is for Voice ESLify. You have 2 options:
   - Eslifies every mod: Eslifies every mod that has compacted mod data attached to it
   - Single mod input menu: takes you to a new console menu 
     - This new menu only needs you to input the plugin name and extension with compacted mod data inside of your SkyrimDatafolder\CompactedForms or OptionalOutputfolder\CompactedForms, You only need to input the plugin name with extension.
Example: File:"ArteFake.esp_ESlEverything.json" input only "ArteFake.esp" 
       - ArteFake does not have any voice files or FaceGen, this is just an example
- To leave a menu or skip it enter "XXX", this is not case sensitive. 

7. Is identical to step 6 but is for FaceGen ESLify
8. Wait until ESLify Everything ends. "Press Enter to exit..." this is what you should see at the end of the code. Please verify that you did not get any errors at the end of the console.
9. Run xEdit once more and run the "_ESLifyEverythingFaceGenFix.pas" script, this fixes all the FaceMesh paths inside the mesh files that ESLify Everything just outputted.


## Extra Notes

Releases are always on GitHub before Nexus

Source Code on GitHub: https://github.com/Michael-wigontherun/ESLifyEverything

## Credits
MaskedRPGFan for allowing me to copy some of his code inside ESPFE Follower - Eslify facegen and voices specifically the CreateFaceMesh procedure

ElminsterAU and the xEdit team for SSEEdit 

## Tools used

Visual Studio 2022

Visual Studio Code

NotePad++
