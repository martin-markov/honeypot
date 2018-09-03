using Honeypot.Interfaces;
using Honeypot.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    public class DbPersister : IRequestPersister
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

        public void Log(ILogRecord record)
        {
            var log = (LogRecord)record;
            string query = @"INSERT INTO [dbo].[RequestLog]
                                               ([ClientIp]
                                               ,[ClientBrowser]
                                               ,[PostData]
                                               ,[CreatedDate]
                                               ,[IsBotRequest])
                                         VALUES
                                               (@ClientIp
                                               ,@ClientBrowser
                                               ,@PostData
                                               ,@CreatedDate
                                               ,@IsBotRequest)";
            SqlCommand insertCmd = new SqlCommand(query, MyConnection);
            insertCmd.Parameters.AddWithValue("ClientIp", log.ClientIP);
            insertCmd.Parameters.AddWithValue("ClientBrowser", log.ClientBrowser);
            insertCmd.Parameters.AddWithValue("PostData", log.PostData);
            insertCmd.Parameters.AddWithValue("CreatedDate", log.RequestDate);
            insertCmd.Parameters.AddWithValue("IsBotRequest", log.IsBotRequest);

            insertCmd.ExecuteNonQuery();
        }

        
        private SqlConnection GetConnection()
        {
            string connString = HoneypotSettings.Settings.SQLConnectionString;

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

        public void Dispose()
        {
            TerminateConnection();
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
