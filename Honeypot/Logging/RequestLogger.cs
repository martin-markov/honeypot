using Honeypot.Helpers;
using Honeypot.Interfaces;
using Honeypot.Models;
using Honeypot.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Logging
{
    public static class Logger
    {

        public static void Log(LogRecord record)
        {
            try
            {
                string persisterNameSpace = HoneypotSettings.Settings.RequestPersister;
                Type requestPersister = TypeHelper.GetTypeFromAllAssemblies(persisterNameSpace);
                if (HoneypotSettings.Settings.LogingEnabled)
                {
                    using (var logWriter = Activator.CreateInstance(requestPersister) as IRequestPersister)
                    {
                        logWriter.Log(record);
                    }
                }
            }
            catch (Exception e)
            {
                //log exception
            }
            
        }
    }
}
