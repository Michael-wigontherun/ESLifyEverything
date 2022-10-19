using ESLifyEverythingGlobalDataLibrary.FormData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLifyEverythingGlobalDataLibrary
{
    public static class DevLog
    {
        //Pauses the program if DevLogging is set to true
        public static void Pause(string Location)
        {
            if (GF.DevSettings.DevLogging)
            {
                Console.WriteLine();
                GF.WriteLine(Location);
                GF.WriteLine(GF.stringLoggingData.EnterToContinue);
                Console.ReadLine();
            }
        }

        //Logs the line to both console and the log
        public static void Log(string logLine)
        {
            if (GF.DevSettings.DevLogging)
            {
                Console.WriteLine(logLine);
                using (StreamWriter stream = File.AppendText(GF.logName))
                {
                    stream.WriteLine(logLine);
                }
            }
        }

        //Logs the line to console
        public static void LogConsole(string logLine)
        {
            if (GF.DevSettings.DevLogging)
            {
                Console.WriteLine(logLine);
            }
        }

        //Logs the line to log
        public static void LogFile(string logLine)
        {
            if (GF.DevSettings.DevLogging)
            {
                using (StreamWriter stream = File.AppendText(GF.logName))
                {
                    stream.WriteLine(logLine);
                }
            }
        }

        //Logs the Form with potentially with extra data
        public static void Log(IFormHandler form, string? extraInfo = null)
        {
            if (GF.DevSettings.DevLogging)
            {
                Console.WriteLine(form.ToString());
                using (StreamWriter stream = File.AppendText(GF.logName))
                {
                    stream.WriteLine(form.ToString());
                }
                if(extraInfo != null)
                {
                    Console.WriteLine(extraInfo);
                    using (StreamWriter stream = File.AppendText(GF.logName))
                    {
                        stream.WriteLine(extraInfo);
                    }
                }
            }
        }
    }
}
