﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Common
{
    public static class Logger
    {

        public static void Log(LogRecord record)
        {
            try
            {
                Type requestPersister = TypeHelper.GetTypeFromAllAssemblies(HoneypotSettings.Settings.RequestPersister);
                if (HoneypotSettings.Settings.LogingEnabled)
                {
                    using (var logWriter = Activator.CreateInstance(requestPersister) as IRequestPersister)
                    {
                        logWriter.Log(record);
                    }
                }
            }
            catch (Exception)
            {
                //log exception
            }
            
        }
    }
}
