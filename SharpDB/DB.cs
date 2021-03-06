﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using SharpDB.Entity;
using SharpDB.SPI;
using SharpDB;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace SharpDB
{
    public partial class DB : IDB
    {
        #region IDB 实现

        #region DbProvider

        private DbProviderFactory _dbfactory = null;
        private DbProviderFactory DBFactory
        {
            get { return _dbfactory; }
        }

        private string _providerName = null;

        /// <summary>
        /// 数据提供程序
        /// </summary>
        public string ProviderName
        {
            get { return _providerName; }
        }
        #endregion


        #region 属性

        private AccessType _AccessType = AccessType.MsSql;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public AccessType AccessType
        {
            get { return _AccessType; }
        }

        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    if (ConfigurationManager.ConnectionStrings[AccessType.ToString()] != null)
                    {
                        this._connectionString = ConfigurationManager.ConnectionStrings[AccessType.ToString()].ConnectionString;
                    }
                }
                return _connectionString;
            }
        }

        private string _paramCharacter = "@";

        /// <summary>
        /// 参数化时 使用的字符，默认"@"
        /// </summary>
        public string ParamCharacter
        {
            get { return _paramCharacter; }
        }

        #endregion 属性



        private DBInfo _dbInfo = null;
        /// <summary>
        /// 数据库相关信息
        /// </summary>
        public DBInfo Info
        {
            get
            {
                return _dbInfo;
            }
            set
            {
                this._dbInfo = value;
            }
        }


        #region 构造函数

        /// <summary>
        /// 当前数据库
        /// </summary>
        /// <param name="accessType">数据库类型</param>
        public DB(AccessType accessType)
            : this(accessType, string.Empty)
        {

        }

        /// <summary>
        /// 当前数据库
        /// </summary>
        /// <param name="accessType">数据库类型</param>
        /// <param name="connectionString">数据库连接字符串</param>
        public DB(AccessType accessType,string connectionString)
        {
            this._AccessType = accessType;
            SetProviderName(accessType, connectionString);
            this.Info = new DBInfo(this);
        }
        private void SetProviderName(AccessType accessType, string connectionString)
        {
            switch (accessType)
            {
                case AccessType.MsSql:
                    _providerName = "System.Data.SqlClient";
                    _paramCharacter = "@";
                    break;
                case AccessType.MySql:
                    _providerName = "MySql.Data.MySqlClient";
                    _paramCharacter = "@";
                    break;
                case AccessType.Oracle:
                    _providerName = "System.Data.OracleClient";
                    _paramCharacter = ":";
                    break;
                case AccessType.OracleManaged:
                    _providerName = "Oracle.ManagedDataAccess.Client";
                    _paramCharacter = ":";
                    break;
                case AccessType.OracleDDTek:
                    _providerName = "DDTek.Oracle";
                    _paramCharacter = ":";
                    break;
                case AccessType.Sqlite:
                    _providerName = "System.Data.SQLite";
                    _paramCharacter = "@";
                    break;
                default:
                    _providerName = "System.Data.SqlClient";
                    _paramCharacter = "@";
                    break;
            }
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                this._connectionString = connectionString;
            }
            else if (ConfigurationManager.ConnectionStrings[accessType.ToString()] != null)
            {
                this._connectionString = ConfigurationManager.ConnectionStrings[accessType.ToString()].ConnectionString;
            }
            else
            {
                throw new ArgumentNullException("connectionString", accessType.ToString() + "的connectionString不能为空！");
            }

            //处理 如果Sqlite 连接字符串只提供 路径的情况
            if (accessType == AccessType.Sqlite
                    && Regex.IsMatch(this._connectionString, @"^(\w):\\(.*)(.+\.db)$",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled))
            {
                this._connectionString = "Data Source=" + this._connectionString;
            }

            try
            {
                _dbfactory = DbProviderFactories.GetFactory(ProviderName);
            }
            catch (ConfigurationErrorsException ex)
            {

                throw new ConfigurationErrorsException(ex.Message + "(" + ProviderName + ")", ex);
            }

        }


        #endregion

        #region Ado.Net 对象

        private string _Database = string.Empty;
        public string Database
        {
            get { return _Database; }
        }
        internal DbConnection CreateConn()
        {
            return CreateConn(this.ConnectionString);
        }

        internal DbConnection CreateConn(string connectionString)
        {
            DbConnection _DBConn = DBFactory.CreateConnection();
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                _DBConn.ConnectionString = connectionString;
            }
            else
            {
                _DBConn.ConnectionString = this.ConnectionString;
            }
            _Database = _DBConn.Database;
            return _DBConn;
        }

        private DbCommand CreateCmd()
        {
            DbCommand _DBCmd = DBFactory.CreateCommand();
            return _DBCmd;
        }

        private DbCommand CreateCmd(string commandText = null, DbConnection DbConn = null)
        {
            DbCommand _DBCmd = DBFactory.CreateCommand();
            if (DbConn != null)
            {
                _DBCmd.Connection = DbConn;
            }
            if (!string.IsNullOrWhiteSpace(commandText))
            {
                _DBCmd.CommandText = commandText;
            }           
            return _DBCmd;
        }


        private DbDataAdapter CreateAdapter(DbCommand dbCmd = null)
        {
            DbDataAdapter dbadapter = DBFactory.CreateDataAdapter();
            if (dbCmd != null)
            {
                dbadapter.SelectCommand = dbCmd;
            }
            return dbadapter;
        }
        private DbDataAdapter CreateAdapter(string commandText, DbConnection DbConn = null)
        {
            DbDataAdapter dbadapter = DBFactory.CreateDataAdapter();
            if (!string.IsNullOrWhiteSpace(commandText))
            {
                dbadapter.SelectCommand = CreateCmd(commandText, DbConn);
            }
            return dbadapter;
        }

        private DbParameter CreateParameter()
        {
            DbParameter dbparameter = DBFactory.CreateParameter();
            return dbparameter;
        }

        #endregion

        #region Bool值返回
        public bool ValidateSql(string strSql, out string msg)
        {
            bool bResult = false;
            msg = string.Empty;
            using (DbConnection conn = CreateConn())
            {
                DbCommand cmd = CreateCmd();
                cmd.Connection = conn;
                conn.Open();
                try
                {
                    if (AccessType == AccessType.MsSql)
                    {
                        cmd.CommandText = "set noexec on;";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = strSql;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "set noexec off;";
                        cmd.ExecuteNonQuery();
                    }
                    else if (AccessType == AccessType.Oracle || AccessType == AccessType.OracleManaged)
                    {
                        cmd.CommandText = "explain plan for " + strSql;
                    }
                    else
                    {
                        throw new NotImplementedException("未能实现" + AccessType + "方式的验证方法！");
                    }
                    cmd.ExecuteNonQuery();
                    bResult = true;
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    bResult = false;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
            return bResult;
        }

        public bool Exists(string strSql, params System.Data.IDataParameter[] cmdParms)
        {
            object obj = GetSingle<object>(strSql, 30, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 单值、DataRow、DataTable、List<DataTable>、DbDataReader 返回
        public TReturn GetSingle<TReturn>(string strSql, int times, params System.Data.IDataParameter[] cmdParms)
        {
            using (DbConnection connection = CreateConn())
            {
                using (DbCommand cmd = CreateCmd())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, strSql, cmdParms, times);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        return obj.ConvertTo<TReturn>();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();

                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }

        public DataRow QueryRow(string strSql, int times = 30, params System.Data.IDataParameter[] cmdParms)
        {
            DataTable data = QueryDS(strSql, times, cmdParms).FirstOrDefault();
            if (data != null && data.Rows.Count > 0)
            {
                return data.Rows[0];
            }
            return null;
        }

        public DataTable QueryTable(string strSql, int times = 30, params System.Data.IDataParameter[] cmdParms)
        {
            return QueryDS(strSql, times, cmdParms).FirstOrDefault();
        }

        public List<DataTable> QueryDS(string strSql, int times = 30, params System.Data.IDataParameter[] cmdParms)
        {
            using (DbConnection connection = CreateConn())
            {
                DbCommand cmd = connection.CreateCommand();
                PrepareCommand(cmd, connection, null, strSql, cmdParms, times);
                using (DbDataAdapter da = CreateAdapter(cmd))
                {
                    List<DataTable> lstTab = new List<DataTable>();
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();

                        if (ds != null && ds.Tables.Count > 0)
                        {
                            foreach (DataTable table in ds.Tables)
                            {
                                lstTab.Add(table);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        connection.Close();

                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                    return lstTab;
                }
            }
        }

        public DbDataReader ExecuteReader(string strSql, int times = 30, params System.Data.IDataParameter[] cmdParms)
        {
            DbConnection connection = CreateConn();
            DbCommand cmd = CreateCmd();
            try
            {
                PrepareCommand(cmd, connection, null, strSql, cmdParms, times);
                DbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 插入、更新、删除 执行命令/执行存储过程
        public int ExecuteSql(string strSql, int times = 30, params System.Data.IDataParameter[] cmdParms)
        {
            using (DbConnection connection = CreateConn())
            {
                using (DbCommand cmd = CreateCmd())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, strSql, cmdParms, times);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (Exception ex)
                    {
                        //string currColName = string.Empty;                        
                        //string update_sql = string.Empty;
                        //foreach (var item in cmdParms)
                        //{
                        //    object val = string.Empty;
                           
                        //    val = "'" + item.Value + "'";
                        //    update_sql = "update ZDSJ_CT_WDZX set " + item.ParameterName.Replace("@", "") + "=" + val + " where id = 1";
                        //    ExecuteSqlTran(update_sql);
                        //}
                        //return 1;
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }




        public int ExecuteSqlTran(params string[] SqlCmd)
        {
            if (SqlCmd == null || SqlCmd.Length ==0)
            {
                return -1;
            }
            using (DbConnection conn = CreateConn())
            {
                conn.Open();
                DbCommand cmd = CreateCmd();
                cmd.Connection = conn;
                DbTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SqlCmd.Length; n++)
                    {
                        string strsql = SqlCmd[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                    return count;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return -1;
                }
            }
        }

        public int ExecuteSqlTran(List<string> SqlCmdList)
        {
            using (DbConnection conn = CreateConn())
            {
                conn.Open();
                DbCommand cmd = CreateCmd();
                cmd.Connection = conn;
                DbTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SqlCmdList.Count; n++)
                    {
                        string strsql = SqlCmdList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                    return count;
                }
                catch
                {
                    tran.Rollback();
                    return -1;
                }
            }
        }

        public int ExecuteSqlTran(Hashtable SqlCmdList)
        {
            using (DbConnection conn = CreateConn())
            {
                conn.Open();
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    DbCommand cmd = CreateCmd();
                    try
                    {
                        int count = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SqlCmdList)
                        {
                            string cmdText = myDE.Key.ToString();
                            DbParameter[] cmdParms = ((List<DbParameter>)myDE.Value).ToArray();
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            count += cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return count;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                    }
                }
            }
        }


        public List<string> GetListColumn(string sel_Column_Sql, string columnName = "")
        {
            return QueryTable(sel_Column_Sql).GetFirstCol<string>(columnName);
        }

        public DataSet RunProcedure(string storedProcName, int times = 30, params IDataParameter[] parameters)
        {
            DataSet ds = new DataSet("ds");
            using (DbConnection conn = CreateConn())
            {
                conn.Open();
                DbCommand cmd = CreateCmd();
                PrepareCommand(cmd, conn, null, storedProcName, parameters, times, CommandType.StoredProcedure);
                DataAdapter adapter = CreateAdapter(cmd);
                adapter.Fill(ds);
            }
            return ds;
        }

        #endregion

        #region private 
        private void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, string cmdText, IDataParameter[] cmdParms, int times = 30, CommandType cmdType = CommandType.Text)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (times > 0)//（以秒为单位）。默认为 30 秒
            {
                cmd.CommandTimeout = times;
            }
            else
            {
                cmd.CommandTimeout = 30;
            }
            if (cmdParms != null && cmdParms.Length > 0)
            {
                foreach (IDataParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput
                        || parameter.Direction == ParameterDirection.Input)
                        && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region 调用 IDB 实现方法
        public bool Exists(string strSql)
        {
            return Exists(strSql, null);
        }

        public TReturn GetSingle<TReturn>(string strSql, int times = 30)
        {
            return GetSingle<TReturn>(strSql, times, null);
        }

        public int ExecuteSql(string strSql, params IDataParameter[] cmdParms)
        {
            return ExecuteSql(strSql, 30, cmdParms);
        }

        public bool BulkCopy(string query_Sql, string connString, string tableName, WrireType writeType = WrireType.DataTable, Dictionary<string, string> columnMappings = null)
        {
            DataTable data = null;
            DbDataReader reader = null;
            
            if (this.AccessType == AccessType.MsSql)
            {
                SqlBulkCopy bulk = null;
                try
                {
                    bulk = new SqlBulkCopy(connString);
                    using (bulk)
                    {
                        bulk.DestinationTableName = tableName;
                        bulk.BulkCopyTimeout = 300;

                        if (columnMappings != null)
                        {
                            foreach (var colMapping in columnMappings)
                            {
                                bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping(colMapping.Key, colMapping.Value));
                            }
                        }

                        if (writeType == WrireType.DataTable)
                        {
                            data = QueryTable(query_Sql);
                            bulk.WriteToServer(data);
                        }
                        else if (writeType == WrireType.DataReader)
                        {
                            reader = ExecuteReader(query_Sql);
                            bulk.WriteToServer(reader);
                            reader.Close();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("未实现批量处理功能！");
            }
        }
        #endregion

        #endregion
    }
}
