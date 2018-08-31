using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Settings
{
    public class HoneypotSettings : ConfigurationSection
    {
        /// <summary>
        /// Get the configuration section
        /// </summary>
        private static HoneypotSettings _settings = ConfigurationManager.GetSection("HoneypotSettings") as HoneypotSettings;

        /// <summary>
        /// The loaded configuration section
        /// </summary>
        public static HoneypotSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        /// <summary>
        /// The requests are not logged one by one by insted first a buffer 
        /// of requests is accumulated then they are sent to the persistence provider as bulk.
        /// This will minimize the impackt on the request because of logging.
        /// </summary>
        [ConfigurationProperty("SQLConnectionString", DefaultValue = "", IsRequired = true)]
        public string SQLConnectionString
        {
            get
            {
                string val = (string)this["SQLConnectionString"];
                return val.Replace("\\\\", "\\");
            }
            set { this.SQLConnectionString = value; }
        }

        /// <summary>
        /// If true the logging is enabled
        /// </summary>
        [ConfigurationProperty("LogingEnabled", DefaultValue = true, IsRequired = false)]
        public bool LogingEnabled
        {
            get { return (bool)this["LogingEnabled"]; }
            set { this["LogingEnabled"] = value; }
        }

        /// <summary>
        /// If true the logging is requests should be automatically blocked
        /// </summary>
        [ConfigurationProperty("BlockRequests", DefaultValue = false, IsRequired = false)]
        public bool BlockRequests
        {
            get { return (bool)this["BlockRequests"]; }
            set { this["BlockRequests"] = value; }
        }

        /// <summary>
        /// Request persister type
        /// </summary>
        [ConfigurationProperty("RequestPersister", DefaultValue = "Honeypot.Persistance.DefaultDbPersister", IsRequired = false)]
        public string RequestPersister
        {
            get { return (string)this["RequestPersister"]; }
            set { this["RequestPersister"] = value; }
        }

        /// <summary>
        /// Request log type
        /// </summary>
        [ConfigurationProperty("RecordModel", DefaultValue = "Honeypot.Models.DefaultLogRecord", IsRequired = false)]
        public string RecordModel
        {
            get { return (string)this["RecordModel"]; }
            set { this["RecordModel"] = value; }
        }
    }
}
