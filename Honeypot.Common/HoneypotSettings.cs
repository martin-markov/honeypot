using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Common
{
    public class HoneypotSettings : ConfigurationSection
    {
        /// <summary>
        /// Get the configuration section
        /// </summary>
        private static HoneypotSettings _settings = ConfigurationManager.GetSection("Honeypot") as HoneypotSettings;

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
        [ConfigurationProperty("SQLServerName", DefaultValue = "", IsRequired = true)]
        public string SQLServerName
        {
            get { return (string)this.SQLServerName; }
            set { this.SQLServerName = value; }
        }

        [ConfigurationProperty("SQLDatabaseName", DefaultValue = "", IsRequired = true)]
        public string SQLDatabaseName
        {
            get { return (string)this.SQLDatabaseName; }
            set { this.SQLDatabaseName = value; }
        }

        /// <summary>
        /// If true the logging is enabled
        /// </summary>
        [ConfigurationProperty("Enabled", DefaultValue = true, IsRequired = false)]
        public bool Enabled
        {
            get { return (bool)this["Enabled"]; }
            set { this["Enabled"] = value; }
        }

        /// <summary>
        /// Determine if the request should be logged on the BeginRequest event or on the EndRequest event
        /// </summary>
        [ConfigurationProperty("LogOnBeginRequest", DefaultValue = true, IsRequired = false)]
        public bool LogOnBeginRequest
        {
            get { return (bool)this["LogOnBeginRequest"]; }
            set { this["LogOnBeginRequest"] = value; }
        }

        /// <summary>
        /// if true the URLs that are requesting files such as .css, .jpg etc. 
        /// will be skipped and such requests will not be logged.
        /// </summary>
        [ConfigurationProperty("SkipFiles", DefaultValue = true, IsRequired = false)]
        public bool SkipFiles
        {
            get { return (bool)this["SkipFiles"]; }
            set { this["SkipFiles"] = value; }
        }

    }
}
