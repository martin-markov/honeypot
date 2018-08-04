using MvcJqGrid;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Honeypot.AdminApp
{
    
    public class HomeController : Controller
    {
        private LogRecordService _logRecordService;

        public LogRecordService LogRecordService
        {
            get
            {
                if (_logRecordService == null)
                    _logRecordService = new LogRecordService();
                return _logRecordService;
            }
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(GridSettings gridSettings)
        {
            var data = LogRecordService.GetLogRecords(gridSettings);
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}
