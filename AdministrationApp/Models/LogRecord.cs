using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Honeypot.AdminApp
{
    public class LogRecord
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string ClientIP { get; set; }
        public string ClientBrowser { get; set; }
        public string PostData { get; set; }
        public bool IsBotRequest { get; set; }

    }
}