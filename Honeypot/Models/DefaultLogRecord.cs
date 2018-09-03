using Honeypot.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Honeypot.Models
{
    public class DefaultLogRecord : ILogRecord
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string ClientIP { get; set; }
        public string ClientBrowser { get; set; }
        public string PostData { get; set; }
        public bool IsBotRequest { get; set; }

        public void MapModelToRequest(HttpRequest request, bool isTrapped, object additionalData = null)
        {
            ClientIP = request.UserHostAddress;
            ClientBrowser = request.UserAgent;
            RequestDate = DateTime.Now;
            PostData = GetJsonStringFromFormData(request.Form);
            IsBotRequest = isTrapped;
        }
        private string GetJsonStringFromFormData(NameValueCollection formData)
        {
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(formData.AllKeys.ToDictionary(k => k, k => formData.GetValues(k).First()));
            return result;
        }
        public override string ToString()
        {
            return RequestDate.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "|"
                + ClientIP + "|"
                + ClientBrowser + "|"
                + PostData + "|"
                + IsBotRequest;
        }
    }
}
