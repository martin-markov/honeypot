using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Honeypot.DemoApp
{
    public class CreditCard
    {
        public string HolderName { get; set; }
        public string ValidDate { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
    }
}