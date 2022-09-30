using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverything.Properties
{
    public sealed class StringResources
    {
        public string xEditSessionFilter { get; set; } = "";
        public string xEditCompactedFormFilter { get; set; } = "";
    }
    public sealed class StringLoggingData
    {
        //error & Problems
        public string LoadOrderNotDetectedError { get; set; } = "";
        public string RunOrReport { get; set; } = "";
        public string PapyrusCompilerMissing { get; set; } = "";
        public string PapyrusCompilerMissing2 { get; set; } = "";
        public string PapyrusCompilerMissing3 { get; set; } = "";
        public string PapyrusCompilerMissing4 { get; set; } = "";
        public string PapyrusFlagFileMissing { get; set; } = "";
        public string PapyrusFlagFileMissing2 { get; set; } = "";
        public string FolderNotFoundError { get; set; } = "";

        //General Menu
        public string ExitCodeInput { get; set; } = "";
        public string ESLEveryMod { get; set; } = "";
        public string WithCompactedForms { get; set; } = "";
        public string SingleInputMod { get; set; } = "";
        public string CanLoop { get; set; } = "";
        public string SingleModInputHeader { get; set; } = "";
        public string ExamplePlugin { get; set; } = "";
        public string ExitCodeInputOutput { get; set; } = "";
        public string EslifingEverything { get; set; } = "";
        public string EslifingSingleMod { get; set; } = "";

        //General logging
        public string ModLine { get; set; } = "";
        public string NewLine { get; set; } = "";
        public string OldLine { get; set; } = "";
        public string ProcessedBSAsLogCount { get; set; } = "";
        public string BSAContainsData { get; set; } = "";
        public string OriganalPath { get; set; } = "";
        public string NewPath { get; set; } = "";
        public string FileChanged { get; set; } = "";

        //startup
        public string DataFolderNotFound { get; set; } = "";
        public string XEditFolderSetToFile { get; set; } = "";
        public string XEditLogNotFoundStartup { get; set; } = "";
        public string xEditlogNotFound { get; set; } = "";
        public string XEditFolderNotFound { get; set; } = "";
        public string IntendedForSSE { get; set; } = "";
        public string XEditLogNotFound { get; set; } = "";
        public string ChampollionMissing { get; set; } = "";
        public string OutputFolderNotFound { get; set; } = "";
        public string OutputFolderIsRequired { get; set; } = "";
        public string ClearYourOutputFolderScripts { get; set; } = "";
        public string PotectOrigonalScripts { get; set; } = "";
        public string OutputFolderWarning { get; set; } = "";

        //Gen AppSettings
        public string SettingsFileNotFound { get; set; } = "";
        public string GenSettingsFile { get; set; } = "";
        public string EditYourSettings { get; set; } = "";

        //xEdit Log Reader
        public string NewFormForMod { get; set; } = "";
        public string NewForm { get; set; } = "";
        public string NewMod { get; set; } = "";
        public string SkipingSessionLogNotFound { get; set; } = "";
        public string xEditSessionLog { get; set; } = "";
        public string xEditCompactedFormLog { get; set; } = "";
        public string NoxEditSessions { get; set; } = "";
        public string XEditLogFileSizeWarning { get; set; } = "";
        public string SelectSession { get; set; } = "";
        public string InputSessionPromt { get; set; } = "";
        public string OutputtingTo { get; set; } = "";
        public string GeneratingJson { get; set; } = "";
        public string OuputingJson { get; set; } = "";

        //ImportModData
        public string ImportingAllModData { get; set; } = "";
        public string GetCompDataLog { get; set; } = "";

        //BSAData & LoadOrderBSAData
        public string StartBSAExtract { get; set; } = "";
        public string BSADataImport { get; set; } = "";
        public string BSADataImportNotFound { get; set; } = "";
        public string BSACheckPrev { get; set; } = "";
        public string BSACheckUpdated { get; set; } = "";
        public string BSACheckModReimp { get; set; } = "";
        public string BSACheckNew { get; set; } = "";
        public string BSACheckModImp { get; set; } = "";
        public string BSACheckMod { get; set; } = "";

        //Voice ESLify
        public string StartingVoiceESLify { get; set; } = "";
        public string VoiceESLMenuHeader { get; set; } = "";

        //FaceGen ESLify
        public string StartingFaceGenESLify { get; set; } = "";
        public string FaceGenESLMenuHeader { get; set; } = "";

        //Data Files ESLify
        public string StartingDataFileESLify { get; set; } = "";
        public string InputDataFileExecutionPromt { get; set; } = "";
        public string ESLEveryModConfig { get; set; } = "";
        public string SelectESLModConfig { get; set; } = "";
        public string ConfiguartionFileFailed { get; set; } = "";
        public string NoModConfigurationFilesFound { get; set; } = "";
        public string ModConfigInputPrompt { get; set; } = "";
        public string SwitchToEverythingMenuItem { get; set; } = "";

        //Custom Skills ESLify
        public string StartingCustomSkillsESLify { get; set; } = "";
        public string CustomSkillsFileAt { get; set; } = "";
        public string CustomSkillsFileUnchanged { get; set; } = "";

        //Script ESLify
        public string StartingScriptESLify { get; set; } = "";
        public string ScriptSourceFileChanged { get; set; } = "";
        public string NoChangedScriptsDetected { get; set; } = "";
        public string ScriptFailedCompilation { get; set; } = "";
        public string ScriptFailedCompilation2 { get; set; } = "";
        public string ScriptFailedCompilation3 { get; set; } = "";
        public string ScriptFailedCompilation4 { get; set; } = "";
        public string ScriptESLifyINeedThisDataToBeReported { get; set; } = "";
        public string ImportantBelow { get; set; } = "";
        public string ImportantBelow1 { get; set; } = "";
        public string ImportantAbove { get; set; } = "";
        public string IgnoreBelow { get; set; } = "";
        public string IgnoreAbove { get; set; } = "";
        public string ScriptESLifyMenuA { get; set; } = "";
        public string ScriptESLifyMenu1 { get; set; } = "";
        public string ScriptESLifyMenu2 { get; set; } = "";
        public string ScriptESLifyMenu3 { get; set; } = "";
        public string ScriptESLifyMenuN { get; set; } = "";
        public string ScriptESLifyMenuY { get; set; } = "";
        public string RunningChampBSA { get; set; } = "";
        public string EndedChampBSA { get; set; } = "";
        public string RunningChampLoose { get; set; } = "";
        public string EndedChampLoose { get; set; } = "";

        //Run FaceGenFix
        public string EditedFaceGen { get; set; } = "";
        public string NoxEditEXE { get; set; } = "";
        public string RunningxEditEXE { get; set; } = "";
        public string FixFaceGenScriptNotFound { get; set; } = "";


        //End Report
        public string EnterToExit { get; set; } = "";

    }
}
