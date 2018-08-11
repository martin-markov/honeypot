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
    public class DefaultDbPersister : IRequestPersister
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
                                               ,[CreatedDate]
                                               ,[IsBotRequest])
                                         VALUES
                                               (@ClientIp
                                               ,@ClientBrowser
                                               ,@PostData
                                               ,@CreatedDate
                                               ,@IsBotRequest)";
            SqlCommand insertCmd = new SqlCommand(query, MyConnection);
            insertCmd.Parameters.AddWithValue("ClientIp", record.ClientIP);
            insertCmd.Parameters.AddWithValue("ClientBrowser", record.ClientBrowser);
            insertCmd.Parameters.AddWithValue("PostData", record.PostData);
            insertCmd.Parameters.AddWithValue("CreatedDate", record.RequestDate);
            insertCmd.Parameters.AddWithValue("IsBotRequest", record.IsBotRequest);

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
