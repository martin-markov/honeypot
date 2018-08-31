using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Honeypot.Interfaces
{
    public interface ILogRecord
    {
        int Id { get; set; }
        DateTime RequestDate { get; set; }
        string ClientIP { get; set; }
        bool IsBotRequest { get; set; }

        void MapModelToRequest(HttpRequest request, bool isTrapped);
    }
}
