using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SharpDB.SPI
{
    public interface IDB
    {
        AccessType AccessType { get; }

        string ConnectionString { get; }

        string ParamCharacter { get; }

        #region Bool查询

        bool ValidateSql(string strSql, out string msg);

        bool Exists(string strSql, params IDataParameter[] cmdParms);

        #endregion

        #region 查询
        TReturn GetSingle<TReturn>(string strSql, int times, params IDataParameter[] cmdParms);

        DataRow QueryRow(string strSql, int times = 30, params IDataParameter[] cmdParms);

        DataTable QueryTable(string strSql, int times = 30, params IDataParameter[] cmdParms);

        List<DataTable> QueryDS(string strSql, int times = 30, params IDataParameter[] cmdParms);

        DbDataReader ExecuteReader(string strSql, int times = 30, params IDataParameter[] cmdParms);


        #endregion

        #region 执行

        int ExecuteSql(string strSql, int times = 30, params IDataParameter[] cmdParms);

        int ExecuteSqlTran(List<string> SqlCmdList);

        int ExecuteSqlTran(Hashtable strSqlList);

        DataSet RunProcedure(string storedProcName, int times = 30, params IDataParameter[] parameters);

        bool BulkCopy(string query_Sql, string connstring, string tableName, DataTable data, WrireType writeType = WrireType.DataTable, Dictionary<string,string> columnMappings = null);

        #endregion
    }
}
