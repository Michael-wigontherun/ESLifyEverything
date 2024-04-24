using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;

namespace ESLifyEverything
{
    //Methods that use a Single Mod Data Configuration
    public static partial class ESLify
    {
        //Enumerates BasicSingleFile Mod Configurations
        public static void SingleBasicFile(BasicSingleFile basicSingleFile)
        {
            //string path = Path.Combine(GF.Settings.DataFolderPath, "autoBody\\Config\\morphs.ini");
            if (!File.Exists(Path.Combine(GF.Settings.DataFolderPath, basicSingleFile.DataPath)))
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + Path.Combine(GF.Settings.DataFolderPath, basicSingleFile.DataPath));
                return;
            }

            GF.WriteLine(basicSingleFile.FileAtLogLine + Path.Combine(GF.Settings.DataFolderPath, basicSingleFile.DataPath));
            bool changed = false;
            string[] fileLines = FormInFileLineReader(
                File.ReadAllLines(Path.Combine(GF.Settings.DataFolderPath, basicSingleFile.DataPath)), 
                basicSingleFile.SeparatorData, out changed);
            if (changed == true)
            {
                string newPath = GF.FixOuputPath(Path.Combine(GF.Settings.DataFolderPath, basicSingleFile.DataPath), GF.Settings.DataFolderPath, GF.Settings.OutputFolder);
                Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                File.WriteAllLines(newPath, fileLines);
            }
            else
            {
                GF.WriteLine(basicSingleFile.FileUnchangedLogLine + Path.Combine(GF.Settings.DataFolderPath, basicSingleFile.DataPath));
            }
            
        }

        //Enumerates BasicDirectFolder Mod Configurations
        public static void EnumDirectFolder(BasicDirectFolder basicDirectFolder)
        {
            if (!Directory.Exists(Path.Combine(GF.Settings.DataFolderPath, basicDirectFolder.StartFolder)))
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + Path.Combine(GF.Settings.DataFolderPath, basicDirectFolder.StartFolder));
                return;
            }
            IEnumerable<string> files = Directory.EnumerateFiles(
                    Path.Combine(GF.Settings.DataFolderPath, basicDirectFolder.StartFolder),
                    basicDirectFolder.FileNameFilter,
                    basicDirectFolder.SeachLevel);
            foreach (string file in files)
            {
                GF.WriteLine(basicDirectFolder.FileAtLogLine + file, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                bool changed = false;
                string[] fileLines = FormInFileLineReader(File.ReadAllLines(file), basicDirectFolder.SeparatorData, out changed);
                OuputDataFileToOutputFolder(changed, file, fileLines, basicDirectFolder.FileUnchangedLogLine);

            }
        }

        //Enumerates BasicDataSubfolder Mod Configurations
        public static async Task<int> EnumDataSubfolder(BasicDataSubfolder basicDataSubfolder)
        {
            if (!Directory.Exists(Path.Combine(GF.Settings.DataFolderPath, basicDataSubfolder.StartDataSubFolder)))
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + Path.Combine(GF.Settings.DataFolderPath, basicDataSubfolder.StartDataSubFolder));
                return await Task.FromResult(0);
            }
            IEnumerable<string> dataSubFolders = Directory.EnumerateDirectories(
                Path.Combine(GF.Settings.DataFolderPath, basicDataSubfolder.StartDataSubFolder),
                basicDataSubfolder.DirectoryFilter,
                basicDataSubfolder.StartSeachLevel);

            foreach (string dataSubFolder in dataSubFolders)
            {
                IEnumerable<string> files = Directory.EnumerateFiles(
                    dataSubFolder,
                    basicDataSubfolder.FileFilter,
                    basicDataSubfolder.SubFolderSeachLevel);
                foreach (string condition in files)
                {
                    GF.WriteLine(basicDataSubfolder.FileAtLogLine + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                    bool changed = false;
                    string[] fileLines = FormInFileLineReader(File.ReadAllLines(condition), basicDataSubfolder.SeparatorData, out changed);
                    OuputDataFileToOutputFolder(changed, condition, fileLines, basicDataSubfolder.FileUnchangedLogLine);
                }
            }

            return await Task.FromResult(1);
        }

        //Enumerates ComplexTOML Mod Configurations
        public static async Task<int> EnumToml(ComplexTOML complexTOML)
        {
            if (!Directory.Exists(Path.Combine(GF.Settings.DataFolderPath, complexTOML.StartFolder)))
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + Path.Combine(GF.Settings.DataFolderPath, complexTOML.StartFolder));
                return await Task.FromResult(0);
            }
            IEnumerable<string> tomlFiles = Directory.EnumerateFiles(
                Path.Combine(GF.Settings.DataFolderPath, complexTOML.StartFolder),
                complexTOML.FileNameFilter,
                complexTOML.SeachLevel);
            foreach (string file in tomlFiles)
            {
                string[] fileLines = File.ReadAllLines(file);
                GF.WriteLine(complexTOML.FileAtLogLine + file, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);

                fileLines = FormInTomlArray(fileLines, complexTOML.ArrayStartFilters, out bool changed);
                fileLines = FormInFileLineReader(fileLines, complexTOML.SeparatorData, out bool changedLines);

                if (changed || changedLines)
                {
                    changed = true;
                }

                OuputDataFileToOutputFolder(changed, file, fileLines, complexTOML.FileUnchangedLogLine);
            }

            return await Task.FromResult(1);
        }

        //Enumerates DelimitedFormKeys Mod Configurations
        public static void EnumDelimitedFormKeys(DelimitedFormKeys delimitedFormKeys)
        {
            if (!Directory.Exists(Path.Combine(GF.Settings.DataFolderPath, delimitedFormKeys.StartFolder)))
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + Path.Combine(GF.Settings.DataFolderPath, delimitedFormKeys.StartFolder));
                return;
            }
            IEnumerable<string> files = Directory.EnumerateFiles(
                    Path.Combine(GF.Settings.DataFolderPath, delimitedFormKeys.StartFolder),
                    delimitedFormKeys.FileNameFilter,
                    delimitedFormKeys.SeachLevel);
            foreach (string file in files)
            {
                GF.WriteLine(delimitedFormKeys.FileAtLogLine + file, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                bool changed = false;
                string[] fileLines = DelimitedFormKeysInFileLineReader(File.ReadAllLines(file), delimitedFormKeys.Delimiter, out changed);
                OuputDataFileToOutputFolder(changed, file, fileLines, delimitedFormKeys.FileUnchangedLogLine);
            }
        }
        
    }
}