using System;
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

        private static object locker = new object();
        private DbConnection _DBConn = null;
        internal DbConnection DBConn
        {
            get
            {
                _DBConn = DBFactory.CreateConnection();
                _DBConn.ConnectionString = ConnectionString;
                return _DBConn;
            }
        }

        internal DbConnection GetDBConn(DbConnection conn)
        {
            if (conn != null)
            {
                conn.ConnectionString = ConnectionString;
                return conn;
            }
            _DBConn = DBFactory.CreateConnection();
            _DBConn.ConnectionString = ConnectionString;
            return _DBConn;
        }


        private DbCommand _DBCmd = null;
        private DbCommand DBCmd
        {
            get
            {
                if (_DBCmd == null)
                {
                    _DBCmd = DBFactory.CreateCommand();
                    _DBCmd.Connection = DBConn;
                }
                return _DBCmd;
            }
        }


        private DbDataAdapter GetDataAdapter(DbCommand dbCmd)
        {
            DbDataAdapter dbadapter = DBFactory.CreateDataAdapter();
            dbadapter.SelectCommand = dbCmd;
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
            using (DbConnection conn = DBConn)
            {
                DbCommand cmd = DBCmd;
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
            using (DbConnection connection = DBConn)
            {
                using (DbCommand cmd = DBCmd)
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
            if (data != null)
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
            using (DbConnection connection = DBConn)
            {
                DbCommand cmd = connection.CreateCommand();
                PrepareCommand(cmd, connection, null, strSql, cmdParms, times);
                using (DbDataAdapter da = GetDataAdapter(cmd))
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
            DbConnection connection = DBConn;
            DbCommand cmd = DBCmd;
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
            using (DbConnection connection = DBConn)
            {
                using (DbCommand cmd = DBCmd)
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
                        //string Sql = "select top 1 * from ZDSJ_CT_WDZX";
                        //DataRow dr = this.QueryRow(Sql);
                        //Hashtable ht = ConvertHashTable(dr);
                        //string currColName = string.Empty;
                        //try
                        //{
                        //    foreach (var item in cmdParms)
                        //    {
                        //        currColName = item.ParameterName.Replace(ParamCharacter, "").ToLower();
                              
                        //        Hashtable htNew = new Hashtable();
                        //        htNew.Add("ID", ht["id"]);
                        //        htNew.Add(currColName, item.Value);
                        //        if (currColName =="id")
                        //        {
                        //            continue;
                        //        }
                        //        bool res = Update(htNew, "ZDSJ_CT_WDZX");
                        //    }
                        //    return 1;
                        //}
                        //catch (Exception newEX)
                        //{
                        //    throw newEX;
                        //}
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

        public int ExecuteSqlTran(List<string> SqlCmdList)
        {
            using (DbConnection conn = DBConn)
            {
                conn.Open();
                DbCommand cmd = DBCmd;
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
            using (DbConnection conn = DBConn)
            {
                conn.Open();
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    DbCommand cmd = DBCmd;
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
            using (DbConnection conn = DBConn)
            {
                conn.Open();
                DbCommand cmd = DBCmd;
                PrepareCommand(cmd, conn, null, storedProcName, parameters, times, CommandType.StoredProcedure);
                DataAdapter adapter = GetDataAdapter(cmd);
                adapter.Fill(ds);
            }
            return ds;
        }

        #endregion

        #region private 
        private void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, string cmdText, IDataParameter[] cmdParms, int times = 30, CommandType cmdType = CommandType.Text)
        {
            conn = GetDBConn(conn);
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
        #endregion

        #endregion
    }
}
