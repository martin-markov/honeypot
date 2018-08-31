
using Honeypot.Helpers;
using Honeypot.Interfaces;
using Honeypot.Logging;
using Honeypot.Models;
using Honeypot.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Honeypot.Filter
{
    public class HoneypotFilter : ActionFilterAttribute
    {
        #region Fields
        private string[] honeypots;
        private bool isTrapped;
        #endregion

        #region Properties
        /// <summary>
        /// Attribute property indicating whethet honeypot is triggered
        /// </summary>
        public bool IsTrapped
        {
            get
            {
                return this.isTrapped;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// List of model property names to be validated for honeypot
        /// </summary>
        /// <param name="honeypots">String array of model properties to be honeypot validated</param>
        public HoneypotFilter(params string[] honeypots)
        {
            this.honeypots = honeypots;
        }
        /// <summary>
        /// All public properties of the object will be added
        /// </summary>
        /// <param name="type"></param>
        public HoneypotFilter(Type type)
        {
            var modelProps = type.GetProperties();
            this.honeypots = modelProps.Select(x=>x.Name).ToArray();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set IsTrapped propery and HttpRequest form field
        /// </summary>
        /// <param name="value">Trap triggered or not</param>
        private void SetIsTrapped(bool value)
        {
            this.isTrapped = value;
            if (value)
            {
                var collection = HttpContext.Current.Request.Form;
                var propInfo = collection.GetType().GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                propInfo.SetValue(collection, false, new object[] { });
                collection.Add("HasHoneypotTrapped", true.ToString());
            }
        }
        #endregion

        #region Attribute handler methods
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetIsTrapped(false);
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
                return;

            var requestData = HttpContext.Current.Request.Form;
            if (requestData.Count == 0)
                return;

            foreach (string honeypotField in honeypots)
            {
                //Trap any field that is contained in the passed array of honeypotFields 
                if (!String.IsNullOrWhiteSpace(requestData[honeypotField]))
                {
                    isTrapped = true;
                }
                //if not traped set original name before hashing and appopriate value 
                else
                {
                    string hashedName = HtmlHelpers.GetHashedPropertyName(honeypotField);
                    if (requestData.AllKeys.Contains(hashedName))
                    {
                        string val = HttpContext.Current.Request.Form[hashedName];
                        foreach (var actionValue in filterContext.ActionParameters)
                        {
                            foreach (var prop in actionValue.Value.GetType().GetProperties())
                            {
                                if (prop.Name == honeypotField && prop.CanWrite && prop.PropertyType == typeof(string))
                                {
                                    if (prop.PropertyType == val.GetType())
                                    {
                                        prop.SetValue(actionValue.Value, val);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (IsTrapped)
            {
                SetIsTrapped(true);
            }
            LogRequest(HttpContext.Current.Request);
        }

        private void LogRequest(HttpRequest request)
        {
            string recordModelNameSpace = HoneypotSettings.Settings.RecordModel;
            Type requestPersister = TypeHelper.GetTypeFromAllAssemblies(recordModelNameSpace);
            ILogRecord record = Activator.CreateInstance(requestPersister) as ILogRecord;
            record.MapModelToRequest(request, IsTrapped);

            Logger.Log(record);
        }
        #endregion
    }
}

