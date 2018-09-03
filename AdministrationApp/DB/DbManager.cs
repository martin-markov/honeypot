using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.AdminApp
{
    public class DbManager : IDisposable
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

        public List<LogRecord> GetAllLogRecords()
        {
            string query = @"SELECT [Id]
                                  ,[ClientIp]
                                  ,[ClientBrowser]
                                  ,[PostData]
                                  ,[CreatedDate]
                                  ,[IsBotRequest]
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
                        RequestDate = reader.GetDateTime(4),
                        IsBotRequest = reader.GetBoolean(5)
                    };
                    records.Add(record);
                }
            }
            return records;
        }

        public void DeleteLogRecord(int id)
        {
            string query = @"DELETE FROM [dbo].[RequestLog] WHERE Id = @Id;";
            SqlCommand deleteCmd = new SqlCommand(query, MyConnection);
            deleteCmd.Parameters.AddWithValue("Id", id);
            deleteCmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            TerminateConnection();
        }
        private SqlConnection GetConnection()
        {
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.Replace("\\\\","\\");

            SqlConnection connection = new SqlConnection(connString);

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
    }
}
