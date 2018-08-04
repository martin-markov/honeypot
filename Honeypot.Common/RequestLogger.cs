﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Common
{
    public static class Logger
    {
        private static Type persistenceType = Type.GetType("Honeypot.RequestLogger.DbPersister");

        public static void Log(LogRecord record)
        {
            using (var logWriter = new DbManager())        
            {
                logWriter.Log(record);
            }
        }
    }
}
