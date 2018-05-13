using Honeypot.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.RequestLogger
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
            insertCmd.Parameters.Add("ClientIp", record.ClientIP);
            insertCmd.Parameters.Add("ClientBrowser", record.ClientBrowser);
            insertCmd.Parameters.Add("PostData", record.PostData);
            insertCmd.Parameters.Add("CreatedDate", record.RequestDate);
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
            return "Data Source=DESKTOP-KPVE2RK;Initial Catalog=Honeypot;Integrated Security=SSPI;";
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
    }
}
