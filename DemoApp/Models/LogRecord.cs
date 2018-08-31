using Honeypot.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace DemoApp
{
    public class LogRecord : ILogRecord
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string ClientIP { get; set; }
        public string ClientBrowser { get; set; }
        public string PostData { get; set; }
        public bool IsBotRequest { get; set; }

        public void MapModelToRequest(HttpRequest request, bool isTrapped)
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
    }
}