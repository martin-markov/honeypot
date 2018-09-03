using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoApp
{
    public class CreditCard
    {
        public string HolderName { get; set; }
        public string ValidDate { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
    }
}