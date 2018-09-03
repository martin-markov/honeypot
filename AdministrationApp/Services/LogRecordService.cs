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
            List<LogRecord> records = new List<LogRecord>();
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

        public bool DeleteLogRecord(int id)
        {
            if (id == 0)
                return true;
            using (var db = new DbManager())
            {
                try
                {
                    db.DeleteLogRecord(id);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}