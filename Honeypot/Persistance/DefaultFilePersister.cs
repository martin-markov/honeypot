using Honeypot.Interfaces;
using Honeypot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Persistance
{
    internal class DefaultFilePersister : IRequestPersister
    {
        private static readonly string FILE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Honeypot.log");
        public void Log(ILogRecord record)
        {
            var log = (DefaultLogRecord)record;
            File.AppendAllText(FILE_PATH, log.ToString() + Environment.NewLine, Encoding.UTF8);
        }
        public void Dispose()
        {
            
        }
    }
}
