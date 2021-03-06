﻿using Honeypot.Interfaces;
using Honeypot.Models;
using Honeypot.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Persistance
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

        public void Log(ILogRecord record)
        {
            var log = (DefaultLogRecord)record;
            if (TableExists())
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
                insertCmd.Parameters.AddWithValue("ClientIp", log.ClientIP);
                insertCmd.Parameters.AddWithValue("ClientBrowser", log.ClientBrowser);
                insertCmd.Parameters.AddWithValue("PostData", log.PostData);
                insertCmd.Parameters.AddWithValue("CreatedDate", log.RequestDate);
                insertCmd.Parameters.AddWithValue("IsBotRequest", log.IsBotRequest);

                insertCmd.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("Table RequestLog does not exist");
            }
        }

        private bool TableExists()
        {
            int result = 1;
            string query = @"SELECT
                            CASE   
                                WHEN OBJECT_ID(N'RequestLog', N'U') IS NOT NULL THEN 1   
                                WHEN OBJECT_ID (N'RequestLog', N'U') IS NULL THEN 0   
                            END";

            SqlCommand checkCmd = new SqlCommand(query, MyConnection);
            using (SqlDataReader reader = checkCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            return result == 1;
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
