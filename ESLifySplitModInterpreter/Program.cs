using ESLifyEverythingGlobalDataLibrary;
using ESLifySplitModInterpreter.FormData;

namespace ESLifySplitModInterpreter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(GF.StartUp(out var StartupError, "ESLifySplitModInterpreter.txt"))
            {
                Console.WriteLine("\n\n\n\n");
                string orgDataFilePath = Path.Combine(GF.Settings.XEditFolderPath, "ESLifyEverything\\xEditOutput\\OriginalData.csv");
                bool orgDataFile = File.Exists(orgDataFilePath);
                string splitDataFilePath = Path.Combine(GF.Settings.XEditFolderPath, "ESLifyEverything\\xEditOutput\\SplitData.csv");
                bool splitDataFile = File.Exists(splitDataFilePath);
                
                if (orgDataFile && splitDataFile)
                {
                    Dictionary<string, SplitFormHandler> OrgData = new Dictionary<string, SplitFormHandler>();
                    Dictionary<string, SplitModData> SplitData = new Dictionary<string, SplitModData>();

                    GF.WriteLine(GF.stringLoggingData.ReadOrgData);
                    string[] orgFileLines = File.ReadAllLines(orgDataFilePath);
                    foreach (string line in orgFileLines)
                    {
                        if (line.Equals(string.Empty))
                        {
                            continue;
                        }
                        string[] lineArr = line.Split(";:;", StringSplitOptions.None);
                        if(lineArr.Length != 3)
                        {
                            GF.WriteLine(GF.stringLoggingData.IncorrectDataLine);
                            GF.WriteLine(String.Format(GF.stringLoggingData.SkippingImport, line));
                            continue;
                        }

                        if(!OrgData.TryAdd(lineArr[2], new SplitFormHandler(lineArr[0], lineArr[1].Substring(2))))
                        {
                            GF.WriteLine(String.Format(GF.stringLoggingData.EDIDImportedAlready, lineArr[2]));
                            GF.WriteLine(String.Format(GF.stringLoggingData.SkippingImport, line));
                            continue;
                        }
                    }

                    Console.WriteLine("\n\n\n\n");
                    GF.WriteLine(GF.stringLoggingData.ReadSplitData);
                    string[] splitFileLines = File.ReadAllLines(splitDataFilePath);

                    foreach(string line in splitFileLines)
                    {
                        if (line.Equals(string.Empty))
                        {
                            continue;
                        }
                        string[] lineArr = line.Split(";:;", StringSplitOptions.None);
                        if (lineArr.Length != 3)
                        {
                            GF.WriteLine(GF.stringLoggingData.IncorrectDataLine);
                            GF.WriteLine(String.Format(GF.stringLoggingData.SkippingImport, line));
                            continue;
                        }

                        if (OrgData.TryGetValue(lineArr[2], out SplitFormHandler? splitFormHandler))
                        {
                            string orgMod = splitFormHandler.ModName;

                            SplitData.TryAdd(orgMod, new SplitModData(orgMod));

                            splitFormHandler.SetChangedForm(lineArr[1].Substring(2));
                            splitFormHandler.ChangedModName(lineArr[0]);

                            if (splitFormHandler.IsModified)
                            {
                                SplitData[orgMod].CompactedModFormList.Add(splitFormHandler);
                            }
                            
                        }
                    }

                    foreach (var data in SplitData.Values)
                    {
                        data.OutputModData(true);
                    }

                }
                else
                {
                    if (orgDataFile)
                    {
                        GF.WriteLine(GF.stringLoggingData.OrgDataNotFound);
                    }
                    if (splitDataFile)
                    {
                        GF.WriteLine(GF.stringLoggingData.SplitDataNotFound);
                    }
                }

            }

            Console.WriteLine();
            GF.WriteLine(GF.stringLoggingData.EnterToExit);
            Console.ReadLine();
        }
    }
}