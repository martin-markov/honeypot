using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Honeypot.AdminApp
{
    public class DataSourceRequestData
    {
        public int total { get; set; }
        public int page { get; set; }
        public List<LogRecord> rows { get; set; }
        public int records { get; set; }

    }
}