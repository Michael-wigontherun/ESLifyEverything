using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
// this is just an idea its not implemented or fully worked
namespace ESLifyEverything.BSA
{
    public class BSAData
    {
        [JsonInclude]//  BSA name                           BSA name
        public Dictionary<string, BSA> BSAs = new Dictionary<string, BSA>();

        public void AddNew(string BSAName_NoExtention)
        {
            if(BSAs.ContainsKey(BSAName_NoExtention))
            {
                BSAs.Remove(BSAName_NoExtention);
            }
            BSAs.Add(BSAName_NoExtention, new BSA(BSAName_NoExtention));
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
        public List<string> FaceGenModConnections = new List<string>();
        [JsonInclude]
        public List<string> VoiceModConnections = new List<string>();

        public BSA(string bSAName_NoExtention)
        {
            BSAName_NoExtention = bSAName_NoExtention;
            NewBSAData();
        }

        [JsonIgnore]
        private string tempFullPath = string.Empty;

        public void GetTempFullPath()
        {
            tempFullPath =  Path.GetFullPath($".\\TempData\\{BSAName_NoExtention}");
            Directory.CreateDirectory(tempFullPath);
        }

        public void NewBSAData()
        {
            GetTempFullPath();
            BSALastModified = File.GetLastWriteTime(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + ".bsa"));
            if(File.Exists(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + " - Textures.bsa")))
            {
                HasTextureBSA = true;
            }
            Task extData = ExtractBSAModData();
            extData.Wait();
            if (Directory.Exists(Path.Combine(tempFullPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom")))
            {
                IEnumerable<string> FaceGenFilePaths = Directory.EnumerateDirectories(
                        Path.Combine(tempFullPath, "Meshes\\Actors\\Character\\FaceGenData\\FaceGeom"),
                        "*",
                        SearchOption.TopDirectoryOnly);
                foreach(string FaceGenFilePath in FaceGenFilePaths)
                {

                }
            }

            //Directory.Delete(tempFullPath);
        }

        private async Task<int> ExtractBSAModData()
        {
            Process v = new Process();
            v.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
            v.StartInfo.Arguments = $"\"{Path.GetFullPath(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + ".bsa"))}\" -f \"voice\"  -e -o \"{tempFullPath}\"";
            v.Start();
            v.WaitForExit();
            v.Dispose();

            Process p = new Process();
            p.StartInfo.FileName = ".\\BSABrowser\\bsab.exe";
            p.StartInfo.Arguments = $"\"{Path.GetFullPath(Path.Combine(GF.Settings.DataFolderPath, BSAName_NoExtention + ".bsa"))}\" -f \"FaceGeom\"  -e -o \"{tempFullPath}\"";
            p.Start();
            p.WaitForExit();
            p.Dispose();
            return await Task.FromResult(0);
        }
    }
    public class BSAModConnection
    {
        [JsonInclude]
        public string ModName { get; set; } = "";
        [JsonInclude]
        public bool HasFaceGen { get; set; } = false;
        [JsonInclude]
        public bool HasVoice { get; set; } = false;

    }
}
