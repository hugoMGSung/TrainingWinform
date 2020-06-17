using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class SqlHelper : IDisposable
    {
        private SqlConnection connection = null;
        private String connectionString = "dbConnection";

        // {0} : IP, {1} : Port, {2} : DB Name, {3} : userID, {4} : userPW
        private String connectionFormat = "Data Source={0},{1};Initial Catalog={2};Persist Security Info=True;User ID={3};Password={4}";

        public String ConnectionString
        {
            set { connectionString = value; }
        }

        #region Constructors

        public SqlHelper()
        {
        }

        public SqlHelper(string pConnectionString)
        {
            if (!string.IsNullOrEmpty(pConnectionString.Trim()))
            {
                connectionString = pConnectionString;
            }
        }

        public SqlHelper(string pIP, string pPort, string pDbName, string pUserID, string pUserPW)
        {
            connectionString = string.Format(this.connectionFormat, pIP, pPort, pDbName, pUserID, pUserPW);

        }
        #endregion

        #region Add Parameter TO Query

        private void AddParameter(SqlCommand command, string parameterName, SqlDbType dbType, int size, ParameterDirection direction, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            SqlParameter p = new SqlParameter(parameterName, dbType, size, direction, precision, scale, sourceColumn,
                sourceVersion, true, value, null, null, null);
            command.Parameters.Add(p);
        }

        public void AddParameter(SqlCommand command, string parameterName, SqlDbType dbType, int size, ParameterDirection direction, object value)
        {
            AddParameter(command, parameterName, dbType, size, direction, 0, 0, null, DataRowVersion.Current, value);
        }

        public void AddInParameter(SqlCommand command, string parameterName, SqlDbType dbType, object value)
        {
            AddParameter(command, parameterName, dbType, 0, ParameterDirection.Input, value);
        }

        public void AddOutParameter(SqlCommand command, string parameterName, SqlDbType dbType, int size)
        {
            AddParameter(command, parameterName, dbType, size, ParameterDirection.Output, null);
        }

        public object GetParameterValue(SqlCommand command, string parameterName)
        {
            return command.Parameters[parameterName].Value;
        }
        #endregion

        #region Generating SqlCommand

        private SqlCommand PrepareCommand(CommandType commandType, string commandText)
        {
            if (connection == null)
            {
                connection = new SqlConnection(this.connectionString);
            }
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }
            SqlCommand command = new SqlCommand(commandText, connection);
            command.CommandType = commandType;
            return command;
        }

        public SqlCommand GetStoreProcedureCommand(string spname)
        {
            return PrepareCommand(CommandType.StoredProcedure, spname);
        }

        public SqlCommand GetSqlQueryCommand(string query)
        {
            return PrepareCommand(CommandType.Text, query);
        }
        #endregion

        #region Direct Quer

        public int DirectNonQuery(string query)
        {
            SqlCommand sc = GetSqlQueryCommand(query);
            int iResult = ExecuteNonQuery(sc);
            return iResult;
        }

        public DataTable DirectQuery(string query)
        {
            SqlCommand sc = GetSqlQueryCommand(query);
            return LoadDataTable(sc, string.Empty);
        }

        public DataTable DirectQuery(string query, string tableName)
        {
            SqlCommand sc = GetSqlQueryCommand(query);
            return LoadDataTable(sc, string.Empty);
        }
        #endregion

        #region Database Related Command

        public int ExecuteNonQuery(SqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        public object ExecuteScalar(SqlCommand command)
        {
            return command.ExecuteScalar();
        }

        public SqlDataReader ExecuteReader(SqlCommand command)
        {
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader ExecuteReader(SqlCommand command, CommandBehavior commandBehavior)
        {
            return command.ExecuteReader(commandBehavior);
        }

        public DataTable LoadDataTable(SqlCommand command, string tableName)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                using (DataTable dt = new DataTable(tableName))
                {
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataSet LoadDataSet(SqlCommand command, string[] tableNames)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                using (DataSet ds = new DataSet())
                {
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        ds.Tables[i].TableName = tableNames[i];
                    }

                    return ds;
                }
            }
        }

        private SqlTransaction PrepareTransaction(IsolationLevel isolationLevel)
        {
            if (connection == null)
            {
                connection = new SqlConnection(this.connectionString);
            }
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }
            return connection.BeginTransaction(isolationLevel);
        }

        public SqlTransaction BeginTransaction()
        {
            return PrepareTransaction(IsolationLevel.ReadCommitted);
        }

        public SqlTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return PrepareTransaction(isolationLevel);
        }

        public void Commit(SqlTransaction transaction)
        {
            if (transaction != null)
                transaction.Commit();
        }

        public void RollBack(SqlTransaction transaction)
        {
            if (transaction != null)
                transaction.Rollback();
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Destructor

        ~SqlHelper()
        {
            Dispose();
        }
        #endregion

        void IDisposable.Dispose()
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

    }   // end of class

}   // end of namespace