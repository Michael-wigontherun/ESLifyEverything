using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverything.XEdit
{
    public static class XEditLogReader
    {
        public static XEditLog xEditLog = new XEditLog();
        
        public static void ReadLog(string logPath)
        {
            string[] xEditLogTextArr = File.ReadAllLines(logPath);
            XEditSession xEditSession = new XEditSession();
            string xEditSessionFilter = GF.stringsResources.xEditSessionFilter;
            foreach (string line in xEditLogTextArr)
            {
                if (line.Contains(xEditSessionFilter))
                {
                    xEditLog.AddSession(xEditSession);
                    xEditSession = new XEditSession();
                    xEditSession.SessionTimeStamp = line.Substring(line.IndexOf(xEditSessionFilter) + xEditSessionFilter.Length);
                    GF.WriteLine(GF.stringLoggingData.xEditSessionLog + xEditSession.SessionTimeStamp, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                }
                if (line.Contains(GF.stringsResources.xEditCompactedFormFilter))
                {
                    xEditSession.CompactedSessionText.Add(line);
                    GF.WriteLine(GF.stringLoggingData.xEditCompactedFormLog + line, GF.Settings.VerboseConsoleLoging, GF.Settings.VerboseFileLoging);
                }

            }
            xEditLog.AddSession(xEditSession);
        }
    }
}
