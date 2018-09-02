using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Honeypot.Interfaces
{
    /// <summary>
    /// Use this Interface to extend Log record with additional data or behaviour
    /// </summary>
    public interface ILogRecord
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// DateTime of the request
        /// </summary>
        DateTime RequestDate { get; set; }
        /// <summary>
        /// IP from which the request comes
        /// </summary>
        string ClientIP { get; set; }
        /// <summary>
        /// If the request has made from a bot
        /// </summary>
        bool IsBotRequest { get; set; }
        /// <summary>
        /// Implement this method to map System.Web.HttpRequest properties to your implemented class 
        /// </summary>
        /// <param name="request">Current System.Web.HttpRequest</param>
        /// <param name="isTrapped">true if request was made from bot</param>
        /// <param name="additionalData">additional data not contained in HttpRequest</param>
        void MapModelToRequest(HttpRequest request, bool isTrapped, object additionalData = null);
    }
}
