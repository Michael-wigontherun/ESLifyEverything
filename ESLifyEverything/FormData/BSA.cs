using Mutagen.Bethesda;
using Mutagen.Bethesda.Archives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverything.FormData
{
    public static class BSAData
    {
        //                      BSA name
        public static Dictionary<string, BSA> BSAs = new Dictionary<string, BSA>();

        public static bool Imported = false;

        public static void GetBSAData()
        {
            if (File.Exists(".\\Properties\\BSAModConnections.json"))
            {
                BSAs = JsonSerializer.Deserialize<Dictionary<string, BSA>>(File.ReadAllText(".\\Properties\\BSAModConnections.json"))!;
                Imported = true;
                GF.WriteLine(GF.stringLoggingData.BSADataImport);
            }
            else
            {
                Program.NewOrUpdatedMods = true;
                GF.WriteLine(GF.stringLoggingData.BSADataImportNotFound);
            }
        }

        public static void AddNew(string BSAName_NoExtention)
        {
            BSA? bsa;
            if (BSAs.TryGetValue(BSAName_NoExtention, out bsa))
            {
                GF.WriteLine(BSAName_NoExtention + GF.stringLoggingData.BSACheckPrev);
                if (!bsa.BSALastModified.Equals(File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + ".bsa"))))
                {
                    GF.WriteLine(BSAName_NoExtention + GF.stringLoggingData.BSACheckUpdated);
                    BSAs.Remove(BSAName_NoExtention);
                    BSA newBSA = new BSA(BSAName_NoExtention);
                    GF.WriteLine(BSAName_NoExtention + GF.stringLoggingData.BSACheckModReimp);
                    BSAs.Add(BSAName_NoExtention, newBSA);
                    Program.NewOrUpdatedMods = true;
                }
            }
            else
            {
                GF.WriteLine(BSAName_NoExtention + GF.stringLoggingData.BSACheckNew);
                BSA newBSA = new BSA(BSAName_NoExtention);
                BSAs.Add(BSAName_NoExtention, newBSA);
                Program.NewOrUpdatedMods = true;
                GF.WriteLine(BSAName_NoExtention + GF.stringLoggingData.BSACheckModImp);
            }
        }

        public static void Output()
        {
            File.WriteAllText(".\\Properties\\BSAModConnections.json", JsonSerializer.Serialize(BSAs, GF.JsonSerializerOptions));
        }
    }

    public class BSA
    {
        [JsonInclude]
        public DateTime BSALastModified { get; private set; }
        [JsonInclude]
        public string BSAName_NoExtention { get; private set; } = "";
        [JsonInclude]
        public bool HasTextureBSA { get; private set; } = false;
        [JsonInclude]
        public HashSet<string> FaceGenModConnections = new HashSet<string>();
        [JsonInclude]
        public HashSet<string> VoiceModConnections = new HashSet<string>();

        public BSA() { }

        public BSA(string bSAName_NoExtention)
        {
            BSAName_NoExtention = bSAName_NoExtention;
            NewBSAData();
        }

        //[JsonIgnore]
        //private string tempFullPath = string.Empty;

        //public void GetTempFullPath()
        //{
        //    tempFullPath = Path.GetFullPath($".\\TempData\\{BSAName_NoExtention}");
        //    Directory.CreateDirectory(tempFullPath);
        //}

        public void NewBSAData()
        {
            //GetTempFullPath();
            BSALastModified = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + ".bsa"));
            if (File.Exists(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + " - Textures.bsa")))
            {
                HasTextureBSA = true;
            }
            //Task extData = ExtractBSAModData();
            //extData.Wait();
            //extData.Dispose();

            IArchiveReader? reader = Archive.CreateReader(GameRelease.SkyrimSE, Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + ".bsa"));
            
            foreach (IArchiveFile? file in reader.Files)
            {
                string[] path = file.Path.Split('\\');
                
                if (path.Length > 5)
                {
                    if (file.Path.Contains("meshes\\actors\\character\\facegendata\\facegeom"))
                    {
                        string pluginName = path[5];
                        if (pluginName.Contains(".esp") || pluginName.Contains(".esm") || pluginName.Contains(".esl"))
                        {
                            FaceGenModConnections.Add(pluginName);
                        }
                    }
                }

                if (path.Length > 2)
                {
                    if (file.Path.Contains("sound\\voice\\"))
                    {
                        string pluginName = path[2];
                        if (pluginName.Contains(".esp") || pluginName.Contains(".esm") || pluginName.Contains(".esl"))
                        {
                            VoiceModConnections.Add(pluginName);
                        }
                    }
                }
            }

            FaceGenModConnections = FaceGenModConnections.Distinct().ToHashSet();
            VoiceModConnections = VoiceModConnections.Distinct().ToHashSet();

            //if (Directory.Exists(Path.Combine(tempFullPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom")))
            //{
            //    IEnumerable<string> FaceGenFilePaths = Directory.EnumerateDirectories(
            //            Path.Combine(tempFullPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom"),
            //            "*",
            //            SearchOption.TopDirectoryOnly);
            //    foreach (string FaceGenFilePath in FaceGenFilePaths)
            //    {
            //        FaceGenModConnections.Add(Path.GetFileName(FaceGenFilePath));
            //    }
            //}

            //if (Directory.Exists(Path.Combine(tempFullPath, "sound\\voice")))
            //{
            //    IEnumerable<string> VoiceFilePaths = Directory.EnumerateDirectories(
            //            Path.Combine(tempFullPath, "sound\\voice"),
            //            "*",
            //            SearchOption.TopDirectoryOnly);
            //    foreach (string VoiceFilePath in VoiceFilePaths)
            //    {
            //        VoiceModConnections.Add(Path.GetFileName(VoiceFilePath));
            //    }
            //}

            //Directory.Delete(tempFullPath, true);
        }

        //private async Task<int> ExtractBSAModData()
        //{
        //    Process v = new Process();
        //    v.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
        //    v.StartInfo.Arguments = $"\"{Path.GetFullPath(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + ".bsa"))}\" -f \"voice\"  -e -o \"{tempFullPath}\"";
        //    v.Start();
        //    v.WaitForExit();
        //    v.Dispose();

        //    Process p = new Process();
        //    p.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
        //    p.StartInfo.Arguments = $"\"{Path.GetFullPath(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + ".bsa"))}\" -f \"FaceGeom\"  -e -o \"{tempFullPath}\"";
        //    p.Start();
        //    p.WaitForExit();
        //    p.Dispose();
        //    return await Task.FromResult(0);
        //}
    }
}
