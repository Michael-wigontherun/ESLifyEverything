using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESLifyEverything.FormData;

namespace ESLifyEverything.XEdit
{
    public class XEditLog
    {
        private List<XEditSession> xEditSessionsList = new List<XEditSession>();
        public XEditSession[] xEditSessions { get { return xEditSessionsList.ToArray(); } }

        //Adds the session to the list of found sessions
        public void AddSession(XEditSession newSession)
        {
            if (newSession != null && newSession.SessionTimeStamp != "" && newSession.CompactedSessionText.Any())
            {
                xEditSessionsList.Add(newSession!);
            }
        }
    }

}
