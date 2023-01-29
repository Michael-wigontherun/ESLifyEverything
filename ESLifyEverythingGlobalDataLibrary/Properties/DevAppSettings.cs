using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverythingGlobalDataLibrary.Properties
{
    public class DevAppSettings
    {
        public bool DevLogging { get; set; } = false;
        public bool DisableMergeFixerPauses { get; set; } = false;
        public bool DevLoggingOverrideSome { get; set; } = false;
    }
}
