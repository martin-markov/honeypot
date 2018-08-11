using Honeypot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Persistance
{
    public interface IRequestPersister : IDisposable
    {
        void Log(LogRecord record);
    }
}
