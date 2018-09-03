using Honeypot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Interfaces
{
    /// <summary>
    /// Implement this interface to create your own persistance for trapped requests
    /// </summary>
    public interface IRequestPersister : IDisposable
    {
        /// <summary>
        /// Implement this method to save data from request
        /// </summary>
        /// <param name="record">Log to save</param>
        void Log(ILogRecord record);
    }
}
