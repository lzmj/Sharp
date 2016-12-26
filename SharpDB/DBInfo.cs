using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using SharpDB.Entity;
using System.Timers;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Collections;
using System.IO;


namespace SharpDB
{
    public class DBInfo
    {
        /// <summary>
        /// 当前数据连接
        /// </summary>
        private DB Db
        {
            get; set;
        }

        private bool _IsAllMyISAM = false;
        /// <summary>
        /// 如果是MySql,当前数据库的表的存储引擎是否都是 MyISAM。 
        /// 如果表的存储引擎是 InnoDB，那么即使该表的字段有更新，information_schema 查出的 UPDATE_TIME 依旧会为null。
        /// </summary>
        public bool IsAllMyISAM
        {
            get { return _IsAllMyISAM; }
        }

        /// <summary>
        /// 缓存对象
        /// </summary>
        private static Hashtable Cache = null;


        /// <summary>
        /// 内部缓存的过期依赖于表结构更改时间
        /// </summary>
        private Dictionary<string, DateTime> _Dict_TableStruct_Modify = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 表以及最后更新时间(只针对SqlServer/Oracle/MySql。MySql的所有表存储引擎必须为MyISAM。)
        /// </summary>
        public Dictionary<string, DateTime> Dict_TableStruct_Modify
        {
            get { return _Dict_TableStruct_Modify; }
        }


        public DBInfo(DB db)
        {
            this.Db = db;
            Cache = new Hashtable(StringComparer.OrdinalIgnoreCase);

            //初始化查询所有表
            initTableName();

            //开启轮询 查询数据库的表结构是否改变
            Timer timer = new Timer();
            timer.Interval = 3 * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }



        /// <summary>
        /// 间隔时间段内查询 表结构是否有改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool isAllMyISAM = false;
            List<string> lstTabNames = new List<string>();
            var dict_this = GetTableStruct_Modify(out isAllMyISAM, out lstTabNames);

            var arrExcept = dict_this.Except(_Dict_TableStruct_Modify);

            if (arrExcept.Any())
            {
                this._IsAllMyISAM = isAllMyISAM;
                this._TableNames = lstTabNames;
                this._Dict_TableStruct_Modify = dict_this;

                Cache["TableNames"] = _TableNames;

                foreach (var item in arrExcept)
                {
                    string tableName = item.Key;
                    Console.WriteLine(tableName);
                    List<ColumnInfo> lstColInfo = GetAllColumnInfo(tableName);
                    List<string> lstColName = lstColInfo.Select(t => t.ColumnName).ToList();
                    Cache[tableName] = lstColName;
                    foreach (ColumnInfo colInfo in lstColInfo)
                    {
                        Cache[colInfo.ColumnName + "@" + tableName] = colInfo;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化表信息
        /// </summary>
        void initTableName()
        {
            this._Dict_TableStruct_Modify = GetTableStruct_Modify(out _IsAllMyISAM, out _TableNames);
            Cache["TableNames"] = _TableNames;
        }

        /// <summary>
        /// 查询数据库所有表及对应的表结构修改历史
        /// </summary>
        /// <param name="isAllMyISAM"></param>
        /// <param name="lstTabNames"></param>
        /// <returns></returns>
        private Dictionary<string, DateTime> GetTableStruct_Modify(out bool isAllMyISAM, out List<string> lstTabNames)
        {
            isAllMyISAM = false;
            lstTabNames = new List<string>();
            Dictionary<string, DateTime> dict_SD = new Dictionary<string, DateTime>();

            string selSql = string.Empty;
            DataTable data = new DataTable();
            if (Db.AccessType == AccessType.MsSql)
            {
                selSql = "select name,modify_date from sys.objects where type='U' and name <> 'dtproperties' and name <>'sysdiagrams'  order by modify_date desc";
                dict_SD = Db.QueryTable(selSql).GetDict<string, DateTime>("name", "modify_date");
                lstTabNames = dict_SD.Keys.ToList();
            }
            else if (Db.AccessType == AccessType.Oracle || Db.AccessType == AccessType.OracleManaged || Db.AccessType == AccessType.OracleDDTek)
            {
                selSql = "select object_name,last_ddl_time from user_objects Where object_Type='TABLE' Order By last_ddl_time Desc";
                dict_SD = Db.QueryTable(selSql).GetDict<string, DateTime>("object_name", "last_ddl_time");
                lstTabNames = dict_SD.Keys.ToList();
                //过滤 Oracle 中表名有小写的情况
                lstTabNames = lstTabNames.Where(t => Regex.IsMatch(t, "^[A-Z_$]+$")).ToList();
            }
            else if (Db.AccessType == AccessType.MySql)
            {
                selSql = "select case when ((SELECT count(1) FROM information_schema.tables where table_type='base table' and TABLE_SCHEMA='{0}' )= (SELECT count(1) FROM information_schema.tables where table_type='base table' and TABLE_SCHEMA = '{0}' and ENGINE = 'MyISAM')) then true else FALSE end as res";
                selSql = string.Format(selSql, Db.DBConn.Database);
                isAllMyISAM = Db.GetSingle<bool>(selSql);
                if (isAllMyISAM)
                {
                    selSql = "SELECT TABLE_NAME,UPDATE_TIME FROM information_schema.tables where TABLE_SCHEMA='" + Db.DBConn.Database + "' and table_type='base table' ORDER BY UPDATE_TIME asc;";
                    dict_SD = Db.QueryTable(selSql).GetDict<string, DateTime>("TABLE_NAME", "UPDATE_TIME");
                    lstTabNames = dict_SD.Keys.ToList();
                }
                else
                {
                    selSql = "SELECT TABLE_NAME FROM information_schema.tables where  table_type='base table' and TABLE_SCHEMA='{0}' order by TABLE_NAME asc ";
                    selSql = string.Format(selSql, Db.DBConn.Database);
                    lstTabNames = Db.QueryTable(selSql).GetFirstCol<string>();
                }
            }
            else if (Db.AccessType == AccessType.Sqlite)
            {
                selSql = "SELECT name FROM sqlite_master WHERE type='table' order by name";
                lstTabNames = Db.QueryTable(selSql).GetFirstCol<string>();
            }
            return dict_SD;
        }



        private List<string> _TableNames = new List<string>();

        /// <summary>
        /// 获取当前数据库的所有表名
        /// </summary>
        public List<string> TableNames
        {
            get
            {
                if (!Cache.ContainsKey("TableNames"))
                {
                    Cache["TableNames"] = _TableNames;
                    return _TableNames;
                }
                return (Cache["TableNames"] as List<string>) ?? new List<string>();
            }
        }

        /// <summary>
        /// 根据表名获取所有列名
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns>所有列名</returns>
        public List<string> this[string tableName]
        {
            get
            {
                if (!Cache.ContainsKey(tableName))
                {
                    List<ColumnInfo> lstColInfo = GetAllColumnInfo(tableName);
                    List<string> lstColName = lstColInfo.Select(t => t.ColumnName).ToList();

                    Cache[tableName] = lstColName;

                    foreach (ColumnInfo colInfo in lstColInfo)
                    {
                        Cache[colInfo.ColumnName + "@" + tableName] = colInfo;
                    }
                }
                return Cache[tableName] as List<string>;
            }
        }

        /// <summary>
        /// 根据表名、列名获取列信息
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public ColumnInfo this[string tableName, string columnName]
        {
            get
            {
                string key = columnName + "@" + tableName;
                if (!Cache.ContainsKey(key))
                {
                    List<ColumnInfo> lstColInfo = GetAllColumnInfo(tableName);
                    foreach (ColumnInfo colInfo in lstColInfo)
                    {
                        Cache[colInfo.ColumnName + "@" + tableName] = colInfo;
                    }
                }
                return Cache[key] as ColumnInfo;
            }
        }

        /// <summary>
        /// 当前数据库是否存在当前表
        /// </summary>
        /// <param name="table_Name">表名</param>
        /// <returns></returns>
        public bool IsExistTable(string table_Name)
        {
            return TableNames.Contains(table_Name, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 当前表是否存在当前列
        /// </summary>
        /// <param name="table_Name">表名</param>
        /// <param name="column_Name">列名</param>
        /// <returns></returns>
        public bool IsExistColumn(string table_Name,string column_Name)
        {
            return this[table_Name].Contains(column_Name, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 当前数据库所有的表名及对应所有的列信息
        /// </summary>
        public Dictionary<string, List<ColumnInfo>> GetAllTableColumnInfo()
        {
            Dictionary<string, List<ColumnInfo>> dict = new Dictionary<string, List<ColumnInfo>>();
            foreach (string tabName in TableNames)
            {
                List<ColumnInfo> lstCol = GetAllColumnInfo(tabName);
                dict.Add(tabName, lstCol);
            }
            return dict;
        }

        /// <summary>
        /// 增加或更新 表 注释说明
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="comment">表说明</param>
        /// <returns></returns>
        public bool UpsertTableComment(string tableName, string comment)
        {
            string upsert_sql = string.Empty;
            bool result = false;
            switch (Db.AccessType)
            {
                case AccessType.MySql:
                    upsert_sql = "ALTER TABLE " + tableName + " COMMENT='" + comment + "';";
                    break;
                case AccessType.MsSql:
                    upsert_sql = @" if exists (
                 SELECT case when a.colorder = 1 then d.name  else '' end as 表名,  case when a.colorder = 1 then isnull(f.value, '')  else '' end as 表说明
                FROM syscolumns a 
                       inner join sysobjects d 
                          on a.id = d.id 
                             and d.xtype = 'U' 
                             and d.name <> 'sys.extended_properties'
                       left join sys.extended_properties   f 
                         on a.id = f.major_id 
                            and f.minor_id = 0
                 where a.colorder = 1 and d.name<>'sysdiagrams'  and d.name='{0}' and f.value is not null
                 )
                 exec sp_updateextendedproperty N'MS_Description', N'{1}', N'user', N'dbo', N'table', N'{0}', NULL, NULL
                 else
                exec sp_addextendedproperty N'MS_Description', N'{1}', N'user', N'dbo', N'table', N'{0}', NULL, NULL";
                    upsert_sql = string.Format(upsert_sql, tableName, comment);
                    break;
                case AccessType.Oracle:
                case AccessType.OracleManaged:
                case AccessType.OracleDDTek:
                    upsert_sql = @"comment on table " + tableName + " is '" + comment + "'";
                    break;
                case AccessType.Sqlite://Sqlite 不支持 表、列 注释说明
                    return false;
                default:
                    throw new NotImplementedException("未知数据库类型！");
            }
            result = Db.ExecuteSql(upsert_sql) > 0;
            return result;
        }

        /// <summary>
        ///  增加或更新 列 注释说明
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="comment">列说明</param>
        /// <returns></returns>
        public bool UpsertColumnComment(string tableName, string columnName, string comment)
        {
            string upsert_sql = string.Empty;
            bool result = false;
            switch (Db.AccessType)
            {
                case AccessType.MySql:
                    //mysql修改字段注释方法:http://blog.sina.com.cn/s/blog_72aace390102uwgg.html
                    string dbName = Db.DBConn.Database;
                    string selsql = "use information_schema;select column_Type from COLUMNS where table_name = '" + tableName + "' and column_name = '" + columnName + "';";
                    string col_type = Db.GetSingle<string>(selsql, 300);
                    upsert_sql = "use " + dbName + ";ALTER TABLE " + tableName + " MODIFY COLUMN " + columnName + " " + col_type + " COMMENT '" + comment + "';";
                    break;
                case AccessType.MsSql:
                    upsert_sql = @"if exists (select * from   ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table', '{0}', 'column', default) where objname = '{1}') EXEC sp_updateextendedproperty   'MS_Description','{2}','user',dbo,'table','{0}','column',{1} else EXEC sp_addextendedproperty @name=N'MS_Description' , @value=N'{2}' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'{0}', @level2type=N'COLUMN', @level2name=N'{1}' ";
                    upsert_sql = string.Format(upsert_sql, tableName, columnName, comment);
                    break;
                case AccessType.Oracle:
                case AccessType.OracleManaged:
                case AccessType.OracleDDTek:
                    upsert_sql = @"comment on column " + tableName + "." + columnName + " is '" + comment + "'";
                    break;
                case AccessType.Sqlite://Sqlite 不支持 表、列 注释说明
                    return false;
                default:
                    throw new NotImplementedException("未知数据库类型！");
            }
            result = Db.ExecuteSql(upsert_sql) > 0;
            return result;
        }

        /// <summary>
        /// 获取 列 注释说明
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public string GetColumnComment(string tableName, string columnName)
        {
            string result = string.Empty;
            string strSql = string.Empty;
            switch (Db.AccessType)
            {
                case AccessType.MySql:
                    strSql = "SELECT COLUMN_COMMENT FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name = '" + tableName + "' AND table_schema = '" + Db.DBConn.Database + "' AND column_name LIKE '" + columnName + "'";
                    break;
                case AccessType.MsSql:
                    strSql = @"SELECT top 1 value FROM ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table', '" + tableName + "', 'column', default) where objname = '" + columnName + "'";
                    break;
                case AccessType.Oracle:
                case AccessType.OracleManaged:
                case AccessType.OracleDDTek:
                    strSql = @"Select  comments From user_col_comments Where table_name ='" + tableName + "' And column_name='" + columnName + "'";
                    break;
                case AccessType.Sqlite://Sqlite 不支持 表、列 注释说明
                    return string.Empty;
                default:
                    throw new NotImplementedException("未知数据库类型！");
            }
            result = Db.GetSingle<string>(strSql);
            return result;
        }

        /// <summary>
        /// 获取 列 注释说明
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public Dictionary<string, string> GetColumnsComment(string tableName)
        {
            Dictionary<string, string> dict_result = new Dictionary<string, string>();
            string strSql = string.Empty;
            switch (Db.AccessType)
            {
                case AccessType.MySql:
                    strSql = "SELECT column_name,COLUMN_COMMENT as comments FROM INFORMATION_SCHEMA.COLUMNS WHERE AND table_schema = '" + Db.DBConn.Database + "' and table_name = '" + tableName + "'";
                    break;
                case AccessType.MsSql:
                    strSql = @"SELECT objname AS column_name,value AS comments FROM ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table', '" + tableName + "', 'column', default) ";
                    break;
                case AccessType.Oracle:
                    strSql = @"Select  column_name ,comments From user_col_comments Where table_name ='" + tableName + "'";
                    break;
                case AccessType.Sqlite://Sqlite 不支持 表、列 注释说明
                    return dict_result;
                default:
                    throw new NotImplementedException("未知数据库类型！");
            }
            dict_result = Db.QueryTable(strSql).GetDict<string, string>("column_name", "comments");
            return dict_result;
        }

        /// <summary>
        /// 获取数据库所有表列注释
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllTableComment()
        {
            string strSql = string.Empty;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            switch (Db.AccessType)
            {
                case AccessType.MySql:
                    strSql = "SELECT table_name name,TABLE_COMMENT value FROM INFORMATION_SCHEMA.TABLES  WHERE table_type='base table' and  table_schema = '" + Db.DBConn.Database + "' order by table_name asc ";
                    break;
                case AccessType.MsSql:
                    strSql = @"SELECT a.NAME,(SELECT TOP 1 value FROM sys.extended_properties b WHERE b.major_id=a.id and b.minor_id=0) AS value FROM sysobjects a
WHERE a.xtype='U' AND a.name <>'sysdiagrams' AND a.name <>'dtproperties' ORDER BY a.name asc";
                    break;
                case AccessType.Oracle:
                case AccessType.OracleManaged:
                case AccessType.OracleDDTek:
                    strSql = @"Select table_Name As Name,Comments As Value From User_Tab_Comments Where table_Type='TABLE' Order By table_Name Asc";
                    break;
                case AccessType.Sqlite://Sqlite 不支持 表、列 注释说明
                    return dict;
                default:
                    throw new NotImplementedException("未知数据库类型！");
            }
            dict = Db.QueryTable(strSql).GetDict<string, string>("name", "value");
            //过滤 Oracle 中表名有小写的情况            
            if (Db.AccessType == AccessType.Oracle)
            {
                dict = dict.Where(t => Regex.IsMatch(t.Key, "^[A-Z_$]+$")).ToDictionary(k => k.Key, v => v.Value);
            }
            else if (Db.AccessType == AccessType.MySql)
            {
                //MySql的表 存储引擎如果是InnoDB，则表 TABLE_COMMENT的值 例如： “流程状态表; InnoDB free: 96256 kB” 
                //如果表存储引擎是MyISAM，则不会有 InnoDB free
                List<string> lstKeys = dict.Keys.ToList();
                for (int j = 0; j < lstKeys.Count; j++)
                {
                    string comment = dict[lstKeys[j]].Split(';')[0];
                    comment = comment.Contains("InnoDB free") ? string.Empty : comment;
                    dict[lstKeys[j]] = comment;
                }
            }
            return dict;
        }

        /// <summary>
        /// 获取数据库当前表的注释
        /// </summary>
        /// <returns></returns>
        public string GetTableComment(string tableName)
        {
            string result = string.Empty;
            string strSql = string.Empty;
            switch (Db.AccessType)
            {
                case AccessType.MySql:
                    strSql = "SELECT TABLE_COMMENT value FROM INFORMATION_SCHEMA.TABLES  WHERE table_type='BASE TABLE' and table_schema = '" + Db.DBConn.Database + "' and table_name='" + tableName + "'";
                    break;
                case AccessType.MsSql:
                    strSql = @"SELECT (SELECT TOP 1 value FROM sys.extended_properties b WHERE b.major_id=a.id and b.minor_id=0) AS value FROM sysobjects a
WHERE a.xtype='U' AND a.name <>'sysdiagrams' AND a.name <>'dtproperties' AND a.name='" + tableName + "' ";
                    break;
                case AccessType.Oracle:
                    strSql = @"Select Comments As Value From User_Tab_Comments  Where table_Type='TABLE' and table_Name=Upper('" + tableName + "') ";
                    break;
                case AccessType.Sqlite://Sqlite 不支持 表、列 注释说明
                    return string.Empty;
                default:
                    throw new NotImplementedException("未知数据库类型！");
            }
            result = Db.GetSingle<string>(strSql);
            return result;
        }

        /// <summary>
        /// 获取当前表的所有列信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public List<ColumnInfo> GetAllColumnInfo(string tableName)
        {
            string strSql = string.Empty;
            List<ColumnInfo> lstCols = new List<ColumnInfo>();
            switch (Db.AccessType)
            {
                case AccessType.MySql:
                    strSql = "SHOW FULL COLUMNS FROM " + tableName;
                    lstCols = MySqlReadColInfo(strSql);
                    break;
                case AccessType.MsSql:
                    strSql = @"SELECT a.colorder Colorder,a.name ColumnName,b.name TypeName,(case when (SELECT count(*) FROM sysobjects  WHERE (name in (SELECT name FROM sysindexes  WHERE (id = a.id) AND (indid in  (SELECT indid FROM sysindexkeys  WHERE (id = a.id) AND (colid in  (SELECT colid FROM syscolumns WHERE (id = a.id) AND (name = a.name)))))))  AND (xtype = 'PK'))>0 then 1 else null end) IsPK,(case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then 1 else null end) IsIdentity,  CASE WHEN (charindex('int',b.name)>0) OR (charindex('time',b.name)>0) THEN NULL 
ELSE  
-- COLUMNPROPERTY(a.id,a.name,'PRECISION') 
a.length
end as [Length], CASE WHEN ((charindex('int',b.name)>0) OR (charindex('time',b.name)>0)) THEN NULL ELSE isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),null) end as Scale,(case when a.isnullable=1 then 1 else null end) CanNull,isnull(e.text,'') DefaultVal,isnull(g.[value], ' ') AS DeText FROM  syscolumns a left join systypes b on a.xtype=b.xusertype inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' left join syscomments e on a.cdefault=e.id left join sys.extended_properties g on a.id=g.major_id AND a.colid=g.minor_id left join sys.extended_properties f on d.id=f.class and f.minor_id=0 where b.name is not NULL and d.name=N'" + tableName + "' order by a.id,a.colorder";
                    lstCols = Db.QueryTable(strSql).ConvertToListObject<ColumnInfo>();
                    break;
                case AccessType.Oracle:
                case AccessType.OracleManaged:
                case AccessType.OracleDDTek:
                    strSql = @"select a.COLUMN_ID As Colorder,a.COLUMN_NAME As ColumnName,a.DATA_TYPE As TypeName,b.comments As DeText,(Case When a.DATA_TYPE='NUMBER' Then a.DATA_PRECISION Else a.DATA_LENGTH End )As Length,a.DATA_SCALE As Scale,
(Case When (select Count(1)  from user_cons_columns aa, user_constraints bb 
 where aa.constraint_name = bb.constraint_name 
 and bb.constraint_type = 'P' and aa.table_name = '{0}' And aa.column_name=a.COLUMN_NAME)>0 Then 1 Else Null End
 ) As IsPK,
 (Case When (select Count(1) from all_source aa where aa.type='TRIGGER' And aa.Name =
  (SELECT abc.TRIGGER_NAME 
  FROM all_triggers abc
  Where abc.triggering_Event='INSERT' And abc.table_name='{0}') 
  And lower(aa.text) Like '%nextval%' And lower(aa.text) Like '%:new.%' And 
  ( lower(aa.text) Like concat('%',Concat(Lower(a.COLUMN_NAME),' %')) Or  lower(aa.text) Like concat('%.',Concat(Lower(a.COLUMN_NAME),':%'))) -- 2种情况 （nextval into :new.IdNum、:new.id:=primary_key_value ; ）
   )>0 Then 1 Else Null End 
  ) As IsIdentity, 
  Case a.NULLABLE  When 'Y' Then 1 Else Null End As CanNull,
  a.data_default As DefaultVal 
from user_tab_columns a 
Inner Join user_col_comments b On a.TABLE_NAME=b.table_name 
Where b.COLUMN_NAME= a.COLUMN_NAME   and a.Table_Name='{0}'  order by a.column_ID Asc";
                    strSql = string.Format(strSql, tableName.ToUpper());
                    lstCols = Db.QueryTable(strSql).ConvertToListObject<ColumnInfo>();
                    break;
                case AccessType.Sqlite:
                    lstCols = SqliteColInfo(tableName);
                    break;
                default:
                    throw new NotImplementedException("未知数据库类型！");
            }
            return lstCols;
        }


        /// <summary>
        /// 得到当前数据库所有视图名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllViews()
        {
            List<string> lstView = new List<string>();
            string result = string.Empty;
            string strSql = string.Empty;
            switch (Db.AccessType)
            {
                case AccessType.MySql:
                    strSql = "SELECT Table_Name as Name from information_schema.VIEWS";
                    break;
                case AccessType.MsSql:
                    strSql = @"select [name] from sysobjects where xtype='V' order by [name] asc";
                    break;
                case AccessType.Oracle:
                case AccessType.OracleManaged:
                case AccessType.OracleDDTek:
                    strSql = @"select view_name as Name from user_views Order By view_name Asc";
                    break;
                case AccessType.Sqlite:
                    strSql = "select name from sqlite_master WHERE type='view' AND name NOT LIKE 'sqlite_%' order by name asc";
                    break;
                default:
                    throw new NotImplementedException("未知数据库类型！");
            }
            lstView = Db.QueryTable(strSql).GetFirstCol<string>();
            return lstView;
        }


        #region MySql 获取列信息
        private List<ColumnInfo> MySqlReadColInfo(string strSql)
        {
            List<ColumnInfo> lstCols = new List<ColumnInfo>();
            DbDataReader reader = null;
            try
            {
                reader = Db.ExecuteReader(strSql);
                int colorder = 1;
                while (reader.Read())
                {
                    ColumnInfo colInfo = new ColumnInfo();
                    colInfo.Colorder = colorder;

                    if ((!Object.Equals(reader["Field"], null)) && (!Object.Equals(reader["Field"], System.DBNull.Value)))
                    {
                        colInfo.ColumnName = reader["Field"].ToString();
                    }

                    if ((!Object.Equals(reader["Type"], null)) && (!Object.Equals(reader["Type"], System.DBNull.Value)))
                    {
                        string typename = reader["Type"].ToString();
                        string len = "", pre = "", scal = "";
                        TypeNameProcess(typename, out typename, out len, out pre, out scal);

                        colInfo.TypeName = typename;
                        colInfo.Length = len.ConvertTo<int?>(null);
                        //colInfo.Preci = pre.ConvertTo<int?>(null);
                        colInfo.Scale = scal.ConvertTo<int?>(null);
                    }

                    if ((!Object.Equals(reader["Key"], null)) && (!Object.Equals(reader["Key"], System.DBNull.Value)))
                    {
                        string skey = reader["Key"].ToString();
                        colInfo.IsPK = (skey.Trim() == "PRI") ? true : false;
                    }

                    if ((!Object.Equals(reader["Null"], null)) && (!Object.Equals(reader["Null"], System.DBNull.Value)))
                    {
                        string snull = reader["Null"].ToString();
                        colInfo.CanNull = (snull.Trim() == "YES") ? true : false;
                    }
                    if ((!Object.Equals(reader["Default"], null)) && (!Object.Equals(reader["Default"], System.DBNull.Value)))
                    {
                        string defaultVal = reader["Default"].ToString();
                        colInfo.DefaultVal = defaultVal;
                    }

                    if ((!Object.Equals(reader["Comment"], null)) && (!Object.Equals(reader["Comment"], System.DBNull.Value)))
                    {
                        string deText = reader["Comment"].ToString();
                        colInfo.DeText = deText;
                    }
                    if ((!Object.Equals(reader["Extra"], null)) && (!Object.Equals(reader["Extra"], System.DBNull.Value)))
                    {
                        string extra = reader["Extra"].ToString();
                        colInfo.IsIdentity = (extra.Trim() == "auto_increment") ? true : false;
                    }
                    lstCols.Add(colInfo);
                    colorder++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return lstCols;
        }

        //对类型名称 解析
        private void TypeNameProcess(string strName, out string TypeName, out string Length, out string Preci, out string Scale)
        {
            TypeName = strName;
            Length = string.Empty;
            Preci = string.Empty;
            Scale = string.Empty;

            if (strName.Contains("("))
            {
                if (!strName.Contains(","))
                {
                    //TypeName = Regex.Replace(strName, @"(\w+)\((\d+)\)", "$1", RegexOptions.Compiled);
                    Length = Regex.Replace(strName, @"(\w+)\((\d+)\)", "$2", RegexOptions.Compiled);
                }
                else
                {
                    //TypeName = Regex.Replace(strName, @"(\w+)\((\d+)\)", "$1", RegexOptions.Compiled);
                    Length = Regex.Replace(strName, @"(\w+)\((\d+),(\d+)\)", "$2", RegexOptions.Compiled);
                    Scale = Regex.Replace(strName, @"(\w+)\((\d+),(\d+)\)", "$3", RegexOptions.Compiled);
                }
            }
        }
        #endregion

        #region Sqlite获取列信息
        private List<ColumnInfo> SqliteColInfo(string tableName)
        {
            List<ColumnInfo> lstCols = new List<ColumnInfo>();
            var dbConn = Db.GetDBConn(Db.DBConn);
            dbConn.Open();
            DataRow[] columns = dbConn.GetSchema("COLUMNS").Select("TABLE_NAME='" + tableName + "'");
            foreach (DataRow dr in columns)
            {
                ColumnInfo colInfo = new ColumnInfo();
                colInfo.Colorder = dr["ORDINAL_POSITION"].ToString().ConvertTo<int>(0);
                colInfo.ColumnName = dr["COLUMN_NAME"].ToString();
                colInfo.Length = dr["CHARACTER_MAXIMUM_LENGTH"].ToString().ConvertTo<int?>(null);
                //colInfo.Preci = dr["NUMERIC_PRECISION"].ToString().ConvertTo<int?>(null);
                colInfo.Scale = dr["NUMERIC_SCALE"].ToString().ConvertTo<int?>(null);
                colInfo.IsPK = dr["PRIMARY_KEY"].ToString().ToLower() == "true" ? true : false;
                colInfo.CanNull = dr["IS_NULLABLE"].ToString().ToLower() == "true" ? true : false;
                colInfo.DefaultVal = dr["COLUMN_DEFAULT"].ToString();
                colInfo.TypeName = dr["DATA_TYPE"].ToString();
                if (colInfo.IsPK && string.Compare(colInfo.TypeName, "integer", true) == 0)
                {
                    colInfo.IsIdentity = true;
                }
                lstCols.Add(colInfo);
            }
            dbConn.Close();
            return lstCols;
        }
        #endregion



        /// <summary>
        /// 获取所有表、表描述、包含列的相关信息
        /// </summary>
        /// <returns></returns>
        public List<TableInfo> GetAllTableInfos()
        {
            List<TableInfo> lstTabInfo = new List<TableInfo>();
            Dictionary<string, string> dict_tabComment = GetAllTableComment();
            Dictionary<string, List<ColumnInfo>> dictColInfo = GetAllTableColumnInfo();

            foreach (var item in dict_tabComment)
            {
                TableInfo tabInfo = new TableInfo();
                tabInfo.TableName = item.Key;
                tabInfo.TabComment = item.Value;
                if (dictColInfo.ContainsKey(tabInfo.TableName))
                {
                    List<ColumnInfo> lstColInfo = dictColInfo[tabInfo.TableName];
                    tabInfo.LstColInfo = lstColInfo;
                    lstTabInfo.Add(tabInfo);
                }
                else
                {

                }
            }
            return lstTabInfo;
        }

    }
}

