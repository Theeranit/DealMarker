using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using KK.DealMaker.Core.Helper;

namespace KK.DealMaker.Core.Helper
{
    public class OracleHelper : Database
    {
        private static OracleHelper instance;

        private OracleHelper()
        { }

        private OracleHelper(string ConnectionString)
        {
            try
            {
                LoggingHelper.Debug("OracleHelper: Start Connection");
                base.Connection = new OracleConnection(ConnectionString);
                base.Command = new OracleCommand();
                base.ConnectionString = ConnectionString;
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("OracleHelper: " + ex.Message);
            }

        }

        public override IDbConnection CreateConnection()
        {
            return new OracleConnection(base.ConnectionString);
        }

        public override IDbCommand CreateCommand()
        {
            return new OracleCommand();
        }

        public override IDbConnection CreateOpenConnection()
        {
            OracleConnection connection = (OracleConnection)CreateConnection();
            connection.Open();

            LoggingHelper.Debug("Oracle: Connection is open!");

            return connection;
        }

        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            OracleCommand command = (OracleCommand)CreateCommand();

            command.CommandText = commandText;
            command.Connection = (OracleConnection)connection;
            command.CommandType = CommandType.Text;

            return command;
        }

        public override IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        {
            OracleCommand command = (OracleCommand)CreateCommand();

            command.CommandText = procName;
            command.Connection = (OracleConnection)connection;
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new OracleParameter(parameterName, parameterValue);
        }

        public static OracleHelper GetOracleInstance(string ConnectionString)
        {
            LoggingHelper.Debug("-----Start get instance-----");

            try
            {
                if (OracleHelper.instance == null)
                    OracleHelper.instance = new OracleHelper(ConnectionString);
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("-----Oracle getInstance Error----");
                LoggingHelper.Debug(ex.Message);
            }
            return OracleHelper.instance;
        }

        public override bool OpenConnection()
        {
            bool flag = false;
            try
            {
                if (base.Connection.State != ConnectionState.Closed)
                {
                    base.Connection.Close();
                }
                base.Connection.Open();
                base.Command.Connection = base.Connection;
                flag = true;
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("-----Oracle OpenConnection Error----");
                LoggingHelper.Debug(ex.Message);
                flag = false;
            }
            return flag;
        }
        public override void CreateCommand(string Sql, CommandType Type)
        {
            try
            {
                base.Command = new OracleCommand(Sql, (OracleConnection)base.Connection);
                base.Command.CommandType = Type;
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("-----Oracle Create Command Error----");
                LoggingHelper.Debug(ex.Message);
            }
        }
        public void CreateTransaction(string Sql, CommandType Type)
        {
            try
            {
                base.Command = new OracleCommand();
                base.Command.CommandText = Sql;
                base.Command.Transaction = base.Transaction;
                base.Command.Connection = base.Connection;
                base.Command.CommandType = Type;
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("-----Oracle Create Transaction Error----");
                LoggingHelper.Debug(ex.Message);
            }
        }
        public override void BeginTransaction()
        {
            base.Transaction = base.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            base.Command = base.Connection.CreateCommand();
            base.Command.Transaction = base.Transaction;
        }
        public override void SetOutputParameter(string ParamName)
        {
            OracleParameter OutputParameter = new OracleParameter(ParamName, OracleDbType.RefCursor, ParameterDirection.ReturnValue);
            //((OracleCommand)base.Command).Parameters.Insert(0, OutputParameter);
            ((OracleCommand)base.Command).Parameters.Add(OutputParameter);
        }
        public override void SetInputParameter(string ParamName, object Value)
        {
            ((OracleCommand)base.Command).Parameters.Add(ParamName, Value);
        }
        public override int ExecuteNonQuery()
        {
            return ((OracleCommand)base.Command).ExecuteNonQuery();
        }
        public override object ExecuteScalar()
        {
            return ((OracleCommand)base.Command).ExecuteScalar();
        }
        public DataTable ExecuteReaderToDataTable(string TableName)
        {
            try
            {
                DataTable dt = new DataTable(TableName);
                OracleDataAdapter da = new OracleDataAdapter((OracleCommand)this.Command);
                da.Fill(dt);
                da.Dispose();
                return dt;
            }
            catch (Exception ex) 
            {
                LoggingHelper.Debug("-----Oracle ExecuteReaderToDataTable Error----");
                LoggingHelper.Debug(ex.Message);
                return null;
            }
        }
        public override object ExecuteReader()
        {
            return ((OracleCommand)base.Command).ExecuteReader();            
        }
        public override object ExecuteReader(string ParamOut)
        {
            OracleDataReader reader = null;
            try
            {

                OracleParameter paramReturn = new OracleParameter(ParamOut, OracleDbType.RefCursor, ParameterDirection.Output);
                ((OracleCommand)base.Command).Parameters.Add(paramReturn);
                int result = ((OracleCommand)base.Command).ExecuteNonQuery();
                OracleRefCursor refCur = (OracleRefCursor)paramReturn.Value;

                reader = refCur.GetDataReader();

            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("-----Oracle ExecuteReader with ParamOut Error----");
                LoggingHelper.Debug(ex.Message);
                return null;
            }
            return reader;
        }
        public override DataSet ExecuteReader(string ParamOut, string TableSpaceName)
        {
            OracleDataReader reader = null;
            DataSet ds = new DataSet();
            try
            {

                OracleParameter paramReturn = new OracleParameter(ParamOut, OracleDbType.RefCursor, ParameterDirection.Output);
                ((OracleCommand)base.Command).Parameters.Add(paramReturn);
                int result = ((OracleCommand)base.Command).ExecuteNonQuery();
                OracleRefCursor refCur = (OracleRefCursor)paramReturn.Value;

                reader = refCur.GetDataReader();

                do
                {
                    DataTable schema = reader.GetSchemaTable();

                    DataTable dt = new DataTable(TableSpaceName);
                    foreach (DataRow dr in schema.Rows)
                    {
                        dt.Columns.Add(new DataColumn(dr["ColumnName"].ToString(), Type.GetType(dr["DataType"].ToString())));
                    }

                    while (reader.Read())
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dr[i] = (Object)reader.GetValue(i);
                        }
                        dt.Rows.Add(dr);
                    }
                    ds.Tables.Add(dt);

                } while (reader.NextResult());
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("-----Oracle ExecuteReader with ParamOut Error----");
                LoggingHelper.Debug(ex.Message);
                return null;
            }
            return ds;
        }

        public DataTable ExecuteReaderToTable(string ParamOut, string TableSpaceName)
        {
            OracleDataReader reader = null;
            DataTable dt = new DataTable(TableSpaceName);

            try
            {

                OracleParameter paramReturn = new OracleParameter(ParamOut, OracleDbType.RefCursor, ParameterDirection.Output);
                ((OracleCommand)base.Command).Parameters.Add(paramReturn);
                int result = ((OracleCommand)base.Command).ExecuteNonQuery();
                OracleRefCursor refCur = (OracleRefCursor)paramReturn.Value;

                reader = refCur.GetDataReader();

                do
                {
                    DataTable schema = reader.GetSchemaTable();

                    foreach (DataRow dr in schema.Rows)
                    {
                        dt.Columns.Add(new DataColumn(dr["ColumnName"].ToString(), Type.GetType(dr["DataType"].ToString())));
                    }

                    while (reader.Read())
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dr[i] = (Object)reader.GetValue(i);
                        }
                        dt.Rows.Add(dr);
                    }

                } while (reader.NextResult());
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("-----Oracle ExecuteReaderToTable with ParamOut Error----");
                LoggingHelper.Debug(ex.Message);
                return null;
            }
            return dt;
        }

        public override void CommitTransaction()
        {
            base.Transaction.Commit();
        }
        public override void RollbackTransaction()
        {
            base.Transaction.Rollback();
        }
        public override bool CloseConnection()
        {
            bool flag = false;
            try
            {
                base.Connection.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                LoggingHelper.Debug("-----Oracle CloseConnection Error----");
                LoggingHelper.Debug(ex.Message);
                flag = false;
            }
            return flag;
        }
    }

    public abstract class Database
    {

        public string connectionString;

        protected string ConnectionString { get; set; }
        protected IDbConnection Connection { get; set; }
        protected IDbCommand Command { get; set; }
        protected IDbTransaction Transaction { get; set; }
        protected IDbDataParameter OutputParameter { get; set; }

        #region Abstract Functions
        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand();
        public abstract IDbConnection CreateOpenConnection();
        public abstract IDbCommand CreateCommand(string commandText, IDbConnection connection);
        public abstract IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);
        public abstract IDataParameter CreateParameter(string parameterName, object parameterValue);

        public abstract bool OpenConnection();
        public abstract void CreateCommand(string Sql, CommandType Type);
        public abstract void SetOutputParameter(string ParamName);
        public abstract void SetInputParameter(string ParamName, object Value);
        public abstract int ExecuteNonQuery();
        public abstract object ExecuteScalar();
        public abstract object ExecuteReader();
        public abstract object ExecuteReader(string ParamOut);
        public abstract DataSet ExecuteReader(string ParamOut, string TableSpaceName);
        public abstract void CommitTransaction();
        public abstract void BeginTransaction();
        public abstract void RollbackTransaction();
        public abstract bool CloseConnection();
        #endregion
    }
}
