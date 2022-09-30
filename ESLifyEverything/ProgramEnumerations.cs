namespace ESLifyEverything
{
    public static partial class Program
    {
        public static void SingleBasicFile(string path, string fileAtLogLine, string fileUnchangedLogLine)
        {
            //string path = Path.Combine(GF.Settings.DataFolderPath, "autoBody\\Config\\morphs.ini");
            if (File.Exists(path))
            {
                GF.WriteLine(fileAtLogLine + path);
                bool changed = false;
                string[] fileLines = FormInFileLineReader(File.ReadAllLines(path), out changed);
                if (changed == true)
                {
                    string newPath = GF.FixOuputPath(path, GF.Settings.DataFolderPath, GF.Settings.OutputFolder);
                    Directory.CreateDirectory(newPath.Replace(Path.GetFileName(newPath), ""));
                    File.WriteAllLines(newPath, fileLines);
                }
                else
                {
                    GF.WriteLine(fileUnchangedLogLine + path);
                }
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + path);
            }
        }

        public static void EnumDirectFolder(string startFolder, string fileNameFilter, string fileAtLogLine, string fileUnchangedLogLine,
            SearchOption seachLevel = SearchOption.TopDirectoryOnly)
        {
            if (!Directory.Exists(startFolder))
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + startFolder);
                return;
            }
            IEnumerable<string> files = Directory.EnumerateFiles(
                    startFolder,
                    fileNameFilter,
                    seachLevel);
            foreach (string file in files)
            {
                GF.WriteLine(fileAtLogLine + file, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                bool changed = false;
                string[] fileLines = FormInFileLineReader(File.ReadAllLines(file), out changed);
                OuputDataFileToOutputFolder(changed, file, fileLines, fileUnchangedLogLine);
            }
        }

        public static async Task<int> EnumDataSubfolder(string subfolderStart, string directoryFilter, string fileFilter, string fileAtLogLine, string fileUnchangedLogLine,
            SearchOption startSeachLevel = SearchOption.AllDirectories, SearchOption secondSeachLevel = SearchOption.AllDirectories)
        {
            if (Directory.Exists(subfolderStart))
            {
                IEnumerable<string> dataSubFolders = Directory.EnumerateDirectories(
                    subfolderStart,
                    directoryFilter,
                    startSeachLevel);

                foreach (string dataSubFolder in dataSubFolders)
                {
                    IEnumerable<string> files = Directory.EnumerateFiles(
                        dataSubFolder,
                        fileFilter,
                        secondSeachLevel);
                    foreach (string condition in files)
                    {
                        GF.WriteLine(fileAtLogLine + condition, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                        bool changed = false;
                        string[] fileLines = FormInFileLineReader(File.ReadAllLines(condition), out changed);
                        OuputDataFileToOutputFolder(changed, condition, fileLines, fileUnchangedLogLine);
                    }
                }
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + subfolderStart);
            }

            return await Task.FromResult(0);
        }

        public static async Task<int> EnumToml(string startFolder, string fileNameFilter, string[] arrayFilters,
            string fileAtLogLine, string fileUnchangedLogLine, SearchOption searchLevel = SearchOption.TopDirectoryOnly)
        {
            if (Directory.Exists(startFolder))
            {
                IEnumerable<string> tomlFiles = Directory.EnumerateFiles(
                    startFolder,
                    fileNameFilter,
                    searchLevel);
                foreach (string file in tomlFiles)
                {
                    string[] fileLines = File.ReadAllLines(file);
                    GF.WriteLine(fileAtLogLine + file, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);

                    fileLines = FormInTomlArray(fileLines, arrayFilters, out bool changed);
                    fileLines = FormInFileLineReader(fileLines, out bool changedLines);

                    if (changed || changedLines)
                    {
                        changed = true;
                    }

                    OuputDataFileToOutputFolder(changed, file, fileLines, fileUnchangedLogLine);
                }
            }
            else
            {
                GF.WriteLine(GF.stringLoggingData.FolderNotFoundError + startFolder);
            }
            return await Task.FromResult(0);
        }
    }
}
