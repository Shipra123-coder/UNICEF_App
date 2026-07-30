using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class SQLHelper : ISQLHelper
    {
        /// <summary>
        /// Set connection string for the SQL Server database.
        /// </summary>
        //public static string ConnectionString = "Data Source=DESKTOP-UCOMGBU\\MSSQLSERVER01;Initial Catalog=SDGV2_New;Integrated Security=True";
        //Staging
        public static string ConnectionString = "Server=10.70.232.110;Database=UNMonitor; User Id=UNMonitor;Password=unmonitor@2026;Integrated Security=False;";
        //Local
        //public static string ConnectionString = "Data Source=DESKTOP-UD0M5EJ\\SQLEXPRESS;Initial Catalog=UNMonitor;Integrated Security=True";
        //public SqlConnection Connection { get; }
        public SqlConnection Connection = new SqlConnection(ConnectionString);

        /// <summary>
        ///  Initializes a new instance of the zeentech.DLL.SQLHelper class
        /// </summary>
        public SQLHelper()
        {
            Connection = new SqlConnection(ConnectionString);
        }

        public void Dispose() => CloseConnection();
        /// <summary>
        /// do a switch on the state of the connection
        /// </summary>
        public async Task<bool> HandleConnection()
        {
            try
            {
                Task.Delay(TimeSpan.FromMilliseconds(500));
                if (Connection.ConnectionString == "")
                {
                    Connection = new SqlConnection(ConnectionString);
                }
                //do a switch on the state of the connection
                switch (Connection.State)
                {
                    case ConnectionState.Open: //the connection is open //close then re-open
                        Connection.Close();
                        Connection.Open();
                        break;
                    case ConnectionState.Closed: //connection is open //open the connection
                        Connection.Open();
                        break;
                    default:
                        Connection.Close();
                        Connection.Open();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + "HandleConnection()");
            }
            return true;
        }

        /// <summary>
        /// Close SQL Connection
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                Connection.Dispose();
            }
            catch { }
        }

        /// <summary>
        /// Execute SQL Store Procedure
        /// </summary>
        /// <param name="StoreProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns>DataSet</returns>
        public async Task<DataSet> ExecuteProcedure(string storeProcedureName, IDictionary<string, object> Parameters)
        {
            return await ExecuteProcedure(storeProcedureName, Parameters.Select(s => new SqlParameter(s.Key, s.Value)).ToArray());
        }

        /// <summary>
        /// Execute SQL Store Procedure
        /// </summary>
        /// <param name="StoreProcedureName"></param>
        /// <param name="Parameters"></param>
        /// <returns>DataSet</returns>
        public async Task<DataSet> ExecuteProcedure(string storeProcedureName, params SqlParameter[] Parameters)
        {
            DataSet ds = new DataSet();
            SqlTransaction objTrans = null;
            try
            {
                //await HandleConnection();
                SqlConnection Connection = new SqlConnection(ConnectionString);
                //objTrans = Connection.BeginTransaction();
                Connection.Open();
                using (SqlCommand sqlcmd = new SqlCommand { CommandText = storeProcedureName, CommandType = CommandType.StoredProcedure, Connection = Connection, CommandTimeout = 120 })
                {
                    SqlDataAdapter sqlad = new SqlDataAdapter(sqlcmd);
                    if (Parameters.Length > 0)
                        sqlcmd.Parameters.AddRange(Parameters);
                    sqlad.Fill(ds);
                    sqlcmd.Clone();
                }
                Connection.Close();
                Connection.Dispose();
                //objTrans.Commit();
            }
            catch (Exception ex)
            {
                //objTrans.Rollback();
                throw new Exception("ExecStoreProcedure() => " + storeProcedureName + "," + ex.Message.ToString(), ex);
            }
            finally
            {
                // CloseConnection();
            }
            return ds;
        }

        public async Task<object> ExecuteProcedureScalarAsync(string procedureName, params SqlParameter[] param)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(procedureName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (param.Length > 0)
                    cmd.Parameters.AddRange(param);

                await con.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return result;
            }
        }
        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
