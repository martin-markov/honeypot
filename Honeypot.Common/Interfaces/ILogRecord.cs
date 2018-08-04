using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Common
{
    public interface ILogRecord
    {
        string ClientIP { get; set; }
        DateTime RequestDate { get; set; }
        string ClientBrowser { get; set; }
        string PostData { get; set; }
    }
}
