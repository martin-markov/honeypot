using Honeypot.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Common
{
    public class DbManager : IRequestPersister
    {
        SqlConnection connection;
        protected SqlConnection MyConnection
        {
            get
            {
                if (connection == null)
                {
                    connection = GetConnection();
                }
                else if (connection.State != ConnectionState.Open)
                {
                    connection.Close();

                    connection = GetConnection();
                }

                return connection;
            }
        }


        public void Log(LogRecord record)
        {
            string query = @"INSERT INTO [dbo].[RequestLog]
                                               ([ClientIp]
                                               ,[ClientBrowser]
                                               ,[PostData]
                                               ,[CreatedDate])
                                         VALUES
                                               (@ClientIp
                                               ,@ClientBrowser
                                               ,@PostData
                                               ,@CreatedDate)";
            SqlCommand insertCmd = new SqlCommand(query, MyConnection);
            insertCmd.Parameters.AddWithValue("ClientIp", record.ClientIP);
            insertCmd.Parameters.AddWithValue("ClientBrowser", record.ClientBrowser);
            insertCmd.Parameters.AddWithValue("PostData", record.PostData);
            insertCmd.Parameters.AddWithValue("CreatedDate", record.RequestDate);
            insertCmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            TerminateConnection();
        }

        private string GetConnectionString()
        {


            //var sb = new StringBuilder();
            //sb.Append("Data Source=");
            //sb.Append(configDatabaseServer);
            //sb.Append(";Initial Catalog=");
            //sb.Append(databaseName);
            //sb.Append(";User id=");
            //sb.Append(databaseUserId);
            //sb.Append(";Password=");
            //sb.Append(databasePassword);
            //sb.Append(";Integrated security=false;");
            //sb.Append(";MultipleActiveResultSets=True;Max Pool Size=1000;;Connect Timeout = 30;");
            //return sb.ToString();
            return "Data Source=DESKTOP-KPVE2RK\\SQLSERVER;Initial Catalog=Honeypot;Integrated Security=true;";
        }

        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());

            connection.Open();

            int iteration = 0;
            while (connection.State != ConnectionState.Open && iteration < 5)
            {
                System.Threading.Thread.Sleep(5000);
                iteration++;
                connection.Open();
            }

            return connection;
        }


        private void TerminateConnection()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }

        public List<LogRecord> GetAllLogRecords()
        {
            string query = @"SELECT [Id]
                                  ,[ClientIp]
                                  ,[ClientBrowser]
                                  ,[PostData]
                                  ,[CreatedDate]
                              FROM [dbo].[RequestLog]";

            SqlCommand selectCmd = new SqlCommand(query, MyConnection);
            List<LogRecord> records = new List<LogRecord>();
            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var record = new LogRecord()
                    {
                        Id = reader.GetInt32(0),
                        ClientIP = reader.GetString(1),
                        ClientBrowser = reader.GetString(2),
                        PostData = reader.GetString(3),
                        RequestDate = reader.GetDateTime(4)
                    };
                    records.Add(record);
                }
            }
            return records;
        }
    }
}
