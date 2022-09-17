using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESLifyEverything.Properties
{
    public sealed class AppSettings
    {
        public bool VerboseConsoleLoging { get; set; } = true;

        public bool VerboseFileLoging { get; set; } = true;

        public bool AutoReadNewestxEditSeesion { get; set; } = false;

        public bool AutoReadAllxEditSeesion { get; set; } = false;

        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion { get; set; } = false;

        public bool AutoRunESLify { get; set; } = false;

        public string XEditFolderPath { get; set; } = "xEditFolderPath";

        public string XEditLogFileName { get; set; } = "SSEEdit_log.txt";

        public string SkyrimDataFolderPath { get; set; } = "Skyrim Special Edition\\Data";

        public bool OutputToOptionalFolder { get; set; } = true;

        public string OptionalOutputFolder { get; set; } = "MO2\\Mods\\OuputFolder";
    }

    public sealed class GeneratedAppSettings
    {
        [JsonInclude]
        public AppSettings Settings = new AppSettings();
    }
}
