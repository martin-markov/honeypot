using Honeypot.Models;
using MvcJqGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Honeypot.AdminApp
{
    public class LogRecordService
    {
        public DataSourceRequestData GetLogRecords(GridSettings gridSettings)
        {
            List<DefaultLogRecord> records = new List<DefaultLogRecord>();
            using (var db = new DbManager())
            {
                records = db.GetAllLogRecords();
            }
            int totalRecords = records.Count;
            var jsonData = new DataSourceRequestData
            {
                total = totalRecords / gridSettings.PageSize + 1,
                page = gridSettings.PageIndex,
                records = totalRecords,
                rows = records
            };

            return jsonData;
        }
    }
}