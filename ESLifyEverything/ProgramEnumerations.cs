using ESLifyEverythingGlobalDataLibrary;
using ESLifyEverythingGlobalDataLibrary.Properties.DataFileTypes;

namespace ESLifyEverything
{
    public static partial class Program
    {
        //Unumerates BasicSingleFile Mod Configurations
        private static void SingleBasicFile(BasicSingleFile basicSingleFile)
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

        //Unumerates BasicDirectFolder Mod Configurations
        private static void EnumDirectFolder(BasicDirectFolder basicDirectFolder)
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

        //Unumerates BasicDataSubfolder Mod Configurations
        private static async Task<int> EnumDataSubfolder(BasicDataSubfolder basicDataSubfolder)
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

        //Unumerates ComplexTOML Mod Configurations
        private static async Task<int> EnumToml(ComplexTOML complexTOML)
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

        //Unumerates DelimitedFormKeys Mod Configurations
        private static void EnumDelimitedFormKeys(DelimitedFormKeys delimitedFormKeys)
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

//public static void SingleBasicFile(string path, string? formKeySeparator, string fileAtLogLine, string fileUnchangedLogLine)
//{
//    //string path = Path.Combine(GF.Settings.DataFolderPath, "autoBody\\Config\\morphs.ini");
//    if (File.Exists(path))
//    {
//        GF.WriteLine(fileAtLogLine + path);
//        bool changed = false;
//        string[] fileLines = FormInFileLineReader(File.ReadAllLines(path), formKeySeparator, out changed);
//        if (changed == true)
//        {
//            string newPath = GF.FixOuputPath(path, GF.Settings.DataFolderPath, GF.Settings.OutputFolder);
//            Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
//            File.WriteAllLines(newPath, fileLines);
//        }
//        else
//        {
//            GF.WriteLine(fileUnchangedLogLine + path);
//        }
//    }
//    else
//    {
//        GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + path);
//    }
//}

//public static void EnumDirectFolder(string startFolder, string fileNameFilter, string? formKeySeparator, string fileAtLogLine, string fileUnchangedLogLine,
//    SearchOption seachLevel = SearchOption.TopDirectoryOnly)
//{
//    if (!Directory.Exists(startFolder))
//    {
//        GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + startFolder);
//        return;
//    }
//    IEnumerable<string> files = Directory.EnumerateFiles(
//            startFolder,
//            fileNameFilter,
//            seachLevel);
//    foreach (string file in files)
//    {
//        GF.WriteLine(fileAtLogLine + file, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
//        bool changed = false;
//        string[] fileLines = FormInFileLineReader(File.ReadAllLines(file), formKeySeparator, out changed);
//        OuputDataFileToOutputFolder(changed, file, fileLines, fileUnchangedLogLine);
//    }
//}

//public static async Task<int> EnumDataSubfolder(string subfolderStart, string directoryFilter, string fileFilter, string? formKeySeparator, string fileAtLogLine, string fileUnchangedLogLine,
//    SearchOption startSeachLevel = SearchOption.AllDirectories, SearchOption secondSeachLevel = SearchOption.AllDirectories)
//{
//    if (Directory.Exists(subfolderStart))
//    {
//        IEnumerable<string> dataSubFolders = Directory.EnumerateDirectories(
//            subfolderStart,
//            directoryFilter,
//            startSeachLevel);

//        foreach (string dataSubFolder in dataSubFolders)
//        {
//            IEnumerable<string> files = Directory.EnumerateFiles(
//                dataSubFolder,
//                fileFilter,
//                secondSeachLevel);
//            foreach (string condition in files)
//            {
//                GF.WriteLine(fileAtLogLine + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
//                bool changed = false;
//                string[] fileLines = FormInFileLineReader(File.ReadAllLines(condition), formKeySeparator, out changed);
//                OuputDataFileToOutputFolder(changed, condition, fileLines, fileUnchangedLogLine);
//            }
//        }
//    }
//    else
//    {
//        GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + subfolderStart);
//    }

//    return await Task.FromResult(0);
//}

//public static async Task<int> EnumToml(string startFolder, string fileNameFilter, string? formKeySeparator, string[] arrayFilters,
//    string fileAtLogLine, string fileUnchangedLogLine, SearchOption searchLevel = SearchOption.TopDirectoryOnly)
//{
//    if (Directory.Exists(startFolder))
//    {
//        IEnumerable<string> tomlFiles = Directory.EnumerateFiles(
//            startFolder,
//            fileNameFilter,
//            searchLevel);
//        foreach (string file in tomlFiles)
//        {
//            string[] fileLines = File.ReadAllLines(file);
//            GF.WriteLine(fileAtLogLine + file, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);

//            fileLines = FormInTomlArray(fileLines, arrayFilters, out bool changed);
//            fileLines = FormInFileLineReader(fileLines, formKeySeparator, out bool changedLines);

//            if (changed || changedLines)
//            {
//                changed = true;
//            }

//            OuputDataFileToOutputFolder(changed, file, fileLines, fileUnchangedLogLine);
//        }
//    }
//    else
//    {
//        GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + startFolder);
//    }
//    return await Task.FromResult(0);
//}

//public static void EnumDelimitedFormKeys(string startFolder, string fileNameFilter, string? formKeySeparator, string delimiter, string fileAtLogLine, string fileUnchangedLogLine,
//    SearchOption seachLevel = SearchOption.TopDirectoryOnly)
//{
//    if (!Directory.Exists(startFolder))
//    {
//        GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + startFolder);
//        return;
//    }
//    IEnumerable<string> files = Directory.EnumerateFiles(
//            startFolder,
//            fileNameFilter,
//            seachLevel);
//    foreach (string file in files)
//    {
//        GF.WriteLine(fileAtLogLine + file, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
//        bool changed = false;
//        string[] fileLines = DelimitedFormKeysInFileLineReader(File.ReadAllLines(file), formKeySeparator, delimiter, out changed);
//        OuputDataFileToOutputFolder(changed, file, fileLines, fileUnchangedLogLine);
//    }
//}