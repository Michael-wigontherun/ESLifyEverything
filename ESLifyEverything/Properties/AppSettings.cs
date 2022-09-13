using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverything.Properties
{
    public sealed class AppSettings
    {
        public bool VerboseConsoleLoging { get; set; } = false;

        public bool VerboseFileLoging { get; set; } = false;

        public bool AutoReadNewestxEditSeesion { get; set; } = false;

        public bool AutoReadAllxEditSeesion { get; set; } = false;

        public bool DeletexEditLogAfterRun_Requires_AutoReadAllxEditSeesion { get; set; } = false;

        public bool AutoRunESLify { get; set; } = false;

        public string XEditFolderPath { get; set; } = "";

        public string XEditLogFileName { get; set; } = "";

        public string SkyrimDataFolderPath { get; set; } = "";

        public bool OutputToOptionalFolder { get; set; } = false;

        public string OptionalOutputFolder { get; set; } = "";
    }
}
