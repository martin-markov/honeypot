﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Honeypot.Common
{
    public class LogRecord : ILogRecord
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string ClientIP { get; set; }
        public string ClientBrowser { get; set; }
        public string PostData { get; set; }
    }
}