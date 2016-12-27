using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using SharpDB.Entity;
using SharpDB.SPI;

namespace SharpDB
{
    public partial class DB : IDBEntity
    {

        #region IDBEntity 成员

        #region 插入
        public bool Insert<T>(object obj, params string[] excludeColNames)
        {
            return Insert(obj, typeof(T).Name, excludeColNames);
        }

        public bool Insert(object obj, string tableName, params string[] excludeColNames)
        {
            if (obj == null)
            {
                throw new ArgumentException("obj", "不能为null!");
            }

            CheckTabStuct(tableName);

            StringBuilder sb_beforeSQL = new StringBuilder();
            sb_beforeSQL.Append("insert into " + tableName + " (");
            StringBuilder sb_afterSQl = new StringBuilder();
            sb_afterSQl.Append(") values (");

            string insert_sql = string.Empty;
            DbParameter[] paras = null;
            List<DbParameter> lstParam = new List<DbParameter>();
            //获取所有列名
            List<string> lstAllColName = Info[tableName];
            Hashtable ht = ConvertHashTable(obj, lstAllColName, excludeColNames);
            if (ht.Count == 0)
            {
                return false;
            }
            foreach (DictionaryEntry entry in ht)
            {
                string name = entry.Key.ToString();
                string parameterName = ParamCharacter + name;
                object objvalue = entry.Value;

                sb_beforeSQL.Append(name + ",");
                sb_afterSQl.Append(parameterName + ",");

                DbParameter para = CreateParameter();
                para.ParameterName = parameterName;
                para.Value = (objvalue ?? DBNull.Value);

                if (objvalue != DBNull.Value)
                {
                    object newValue;
                    ColumnInfo cInfo = Info[tableName, name];
                    para.DbType = GetValueDbType(cInfo, objvalue, this.AccessType, out newValue);
                    para.Value = newValue;
                }

                lstParam.Add(para);
            }
            insert_sql = sb_beforeSQL.ToString().TrimEnd(',');
            insert_sql += sb_afterSQl.ToString().TrimEnd(',') + ")";
            paras = lstParam.ToArray();
            int res = ExecuteSql(insert_sql, paras);
            return res > 0 ? true : false;
        }
        #endregion

        #region 更新
        public bool Update<T>(object obj, string pkOrUniqueColName = "Id", params string[] excludeColNames)
        {
            string tableName = typeof(T).Name;
            return Update(obj, tableName, pkOrUniqueColName, excludeColNames);
        }

        public bool Update(object obj, string tableName, string pkOrUniqueColName = "Id", params string[] excludeColNames)
        {
            CheckTabStuct(tableName, pkOrUniqueColName);
            List<string> lstAllColName = Info[tableName];
            Hashtable ht = ConvertHashTable(obj, lstAllColName, excludeColNames);
            StringBuilder sb_beforeSQL = new StringBuilder();
            sb_beforeSQL.Append("update " + tableName + " set ");

            StringBuilder sb_afterSQl = new StringBuilder();
            sb_afterSQl.Append(" where " + pkOrUniqueColName + "=" + (ParamCharacter + pkOrUniqueColName) + "");

            string update_sql = string.Empty;
            DbParameter[] paras = null;
            List<DbParameter> lstParam = new List<DbParameter>();
            foreach (DictionaryEntry entry in ht)
            {
                string name = entry.Key.ToString();
                string parameterName = ParamCharacter + name;
                object objvalue = entry.Value;

                if (!name.Equals(pkOrUniqueColName, StringComparison.OrdinalIgnoreCase))
                {
                    sb_beforeSQL.Append(name + "=" + parameterName + ",");
                }
                DbParameter para = CreateParameter();
                para.ParameterName = parameterName;
                para.Value = (objvalue ?? DBNull.Value);

                if (objvalue != DBNull.Value)
                {
                    object newValue;
                    ColumnInfo cInfo = Info[tableName, name];
                    para.DbType = GetValueDbType(cInfo, objvalue, this.AccessType, out newValue);
                    para.Value = newValue;
                }
               

                lstParam.Add(para);
            }
            update_sql = sb_beforeSQL.ToString().TrimEnd(',');
            update_sql += sb_afterSQl.ToString();
            paras = lstParam.ToArray();
            int res = ExecuteSql(update_sql, paras);
            return res > 0 ? true : false;
        }
        #endregion

        #region 存在则更新，不存在则插入
        public bool Upsert<T>(object obj, string pkOrUniqueColName = "Id", params string[] excludeColNames)
        {
            string tableName = typeof(T).Name;
            return Upsert(obj, tableName, pkOrUniqueColName, excludeColNames);
        }

        public bool Upsert(object obj, string tableName, string pkOrUniqueColName = "Id", params string[] excludeColNames)
        {
            if (obj == null)
            {
                throw new ArgumentException("obj", "不能为null!");
            }

            CheckTabStuct(tableName, pkOrUniqueColName);

            List<string> lstAllColName = Info[tableName];
            List<DbParameter> lstParam = new List<DbParameter>();
            Hashtable ht = ConvertHashTable(obj, lstAllColName, excludeColNames);
            if (ht.Count == 0)
            {
                return false;
            }
            object pkOrUniqueValue = ht[pkOrUniqueColName];
            string exist_sql = "select count(1) from " + tableName + " where " + pkOrUniqueColName + "=" + (ParamCharacter + pkOrUniqueColName);
            DbParameter para = CreateParameter();
            para.ParameterName = (ParamCharacter + pkOrUniqueColName);
            para.Value = pkOrUniqueValue;

            object newValue;
            ColumnInfo cInfo = Info[tableName, pkOrUniqueColName];
            para.DbType = GetValueDbType(cInfo, pkOrUniqueValue, this.AccessType, out newValue);
            para.Value = newValue;

            lstParam.Add(para);
            if (Exists(exist_sql, lstParam.ToArray()))
            {
                return Update(obj, tableName, pkOrUniqueColName, excludeColNames);
            }
            else
            {
                return Insert(obj, tableName, excludeColNames);
            }
        }
        #endregion

        #region 更新 表的 单个值

        public bool UpSingle<T>(string columnName, object columnValue, object pkOrUniqueValue, string pkOrUniqueColName = "Id")
        {
            string tableName = typeof(T).Name;
            return UpSingle(tableName, columnName, columnValue, pkOrUniqueValue, pkOrUniqueColName);
        }

        public bool UpSingle(string tableName, string columnName, object columnValue, object pkOrUniqueValue, string pkOrUniqueColName = "Id")
        {
            List<string> lstAllColName = Info[tableName];

            if (pkOrUniqueValue == null)
            {
                throw new ArgumentNullException("pkOrUniqueValue", "不能为null！");
            }

            CheckTabStuct(tableName, columnName, pkOrUniqueColName);

            Hashtable ht = new Hashtable();
            ht.Add(pkOrUniqueColName, pkOrUniqueValue);
            ht.Add(columnName, columnValue);

            StringBuilder sb_beforeSQL = new StringBuilder();
            sb_beforeSQL.Append("update " + tableName + " set ");

            StringBuilder sb_afterSQl = new StringBuilder();
            sb_afterSQl.Append(" where " + pkOrUniqueColName + "=" + (ParamCharacter + pkOrUniqueColName) + "");

            string update_sql = string.Empty;
            DbParameter[] paras = null;
            List<DbParameter> lstParam = new List<DbParameter>();
            foreach (DictionaryEntry entry in ht)
            {
                string name = entry.Key.ToString();
                string parameterName = ParamCharacter + name;
                object objvalue = entry.Value;

                if (!name.Equals(pkOrUniqueColName, StringComparison.OrdinalIgnoreCase))
                {
                    sb_beforeSQL.Append(name + "=" + parameterName + ",");
                }
                DbParameter para = CreateParameter();
                para.ParameterName = parameterName;
                para.Value = (objvalue ?? DBNull.Value);

                if(objvalue != DBNull.Value)
                {
                    object newValue;
                    ColumnInfo cInfo = Info[tableName, name];
                    para.DbType = GetValueDbType(cInfo, objvalue, this.AccessType, out newValue);
                    para.Value = newValue;
                }
                
                lstParam.Add(para);
            }
            update_sql = sb_beforeSQL.ToString().TrimEnd(',');
            update_sql += sb_afterSQl.ToString();
            paras = lstParam.ToArray();
            int res = ExecuteSql(update_sql, paras);
            return res > 0 ? true : false;
        }

        #endregion

        #region 删除

        public int Delete<T>(string columnName, params object[] columnValues)
        {
            string tableName = typeof(T).Name;
            return Delete(tableName, columnName, columnValues);
        }

        public int Delete(string tableName, string columnName, params object[] columnValues)
        {
            CheckTabStuct(tableName, columnName);
            int res = 0;
            if (columnValues != null && columnValues.Length > 0)
            {
                string del_sql = "delete from " + tableName + " where 1=1 " + (Util.SqlIn(columnName, columnValues)) + "";
                res = ExecuteSql(del_sql);
            }
            return res;
        }


        #endregion

        #region 查询 分页数据获取

        public DataTable GetDataTableByPager(int currentPage, int pageSize, string selColumns, string joinTableName, string whereStr, string orderbyStr, out int totalCount)
        {
            if (string.IsNullOrEmpty(selColumns))
            {
                selColumns = "*";
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 50;
            }

            string cntSQL = string.Empty, strPageSQL = string.Empty;
            DataTable data = new DataTable();
            totalCount = 0;

            if (!string.IsNullOrWhiteSpace(whereStr))
            {
                whereStr = Regex.Replace(whereStr, @"(\s)*(where)?(\s)*(.+)", "and $3$4", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }

            if (!string.IsNullOrWhiteSpace(orderbyStr))
            {
                orderbyStr = Regex.Replace(orderbyStr, @"(\s)*(order)(\s)+(by)(.+)", "$5", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            else
            {
                orderbyStr = " Id desc ";
            }

            if (this.AccessType == AccessType.MsSql)
            {
                cntSQL = "select count(1) from {0} where 1=1 {1}";
                cntSQL = string.Format(cntSQL, joinTableName, whereStr);

                string strSQL = "select {0},ROW_NUMBER() OVER ( ORDER BY {3} ) RN from {1} where 1=1 {2} ";
                strSQL = string.Format(strSQL, selColumns, joinTableName, whereStr, orderbyStr);

                strPageSQL = string.Format(@"SELECT * FROM ({0}) A WHERE   RN BETWEEN {1} AND {2}",
                                           strSQL, (currentPage - 1) * pageSize + 1, (currentPage) * pageSize);
            }
            else if (this.AccessType == AccessType.Oracle
                || this.AccessType == AccessType.OracleDDTek
                || this.AccessType == AccessType.OracleManaged)
            {
                cntSQL = "select count(1) from {0} where 1=1 {1}";
                cntSQL = string.Format(cntSQL, joinTableName, whereStr);

                string strSQL = "select {0} from {1} where 1=1 {2} order by {3}";
                strSQL = string.Format(strSQL, selColumns, joinTableName, whereStr, orderbyStr);


                strPageSQL = string.Format(@"SELECT * FROM (SELECT A.*, ROWNUM RN FROM ({0}) A)
                                    WHERE RN BETWEEN {1} AND {2}",
                                           strSQL, (currentPage - 1) * pageSize + 1, (currentPage) * pageSize);
            }
            else if (this.AccessType == AccessType.MySql)
            {
                cntSQL = "select count(1) from {0} where 1=1 {1}";
                cntSQL = string.Format(cntSQL, joinTableName, whereStr);

                string strSQL = "select {0} from {1} where 1=1 {2} {3} ";
                strSQL = string.Format(strSQL, selColumns, joinTableName, whereStr, orderbyStr);

                strPageSQL = string.Format(@"SELECT * FROM ({0}) A limit {1} {2}",
                                           strSQL, (currentPage - 1) * pageSize + 1, (currentPage) * pageSize);
            }
            data = QueryTable(strPageSQL, 300);
            totalCount = GetSingle<int>(cntSQL, 300);

            return data;
        }

        #endregion


        #region 其他查询

        public TReturn GetSingleById<T, TReturn>(string retColumnName, object Idvalue, string pkOrUniqueColName = "Id")
        {
            string tableName = typeof(T).Name;
            CheckTabStuct(tableName, retColumnName, pkOrUniqueColName);
            string single_sql = "select {0} from {1} where 1=1 and " + (pkOrUniqueColName) + "=" + (ParamCharacter + pkOrUniqueColName);
            single_sql = string.Format(single_sql, retColumnName, tableName);

            ColumnInfo colInfo = Info[tableName, retColumnName];

            DbParameter para = CreateParameter();
            para.ParameterName = (ParamCharacter + pkOrUniqueColName);
            para.Value = Idvalue;

            object newValue;
            para.DbType = GetValueDbType(colInfo, Idvalue, this.AccessType, out newValue);
            para.Value = newValue;
            
            return GetSingle<TReturn>(single_sql, 300, para);
        }

        public bool ExistByColVal(string tableName, string columnName, object columnValue, params object[] excludeValues)
        {
            CheckTabStuct(tableName, columnName);
            string exist_sql = "select count(1) from " + tableName + " where " + columnName + "='" + columnValue + "' ";
            if (excludeValues != null && excludeValues.Length > 0)
            {
                string in_sql = Util.SqlIn(columnName, excludeValues, true);
                exist_sql += in_sql;
            }
            return Exists(exist_sql);
        }

        public T GetEntity<T>(object IdValue)
        {
            string tableName = typeof(T).Name;
            CheckTabStuct(tableName);
            string sel_sql = "select * from " + tableName + " where 1=1 and Id='" + IdValue + "'";
            return QueryRow(sel_sql, 300).ConvertToObjectFromDR<T>();
        }

        public List<T> GetList<T>(string whereStr, string orderByStr = "")
        {
            string tableName = typeof(T).Name;
            CheckTabStuct(tableName);
            string sel_sql = "select * from " + tableName + " where 1=1 " + whereStr + " " + (string.IsNullOrWhiteSpace(orderByStr) ? "" : " order by" + orderByStr);
            return QueryTable(sel_sql, 300).ConvertToListObject<T>();
        }


        public List<string> GetListColumn<T>(string columnName, string whereStr = "", bool isDistinct = false)
        {
            string tableName = typeof(T).Name;
            string sel_sql = "select " + (isDistinct ? "distinct" : "") + columnName + " from " + tableName + " where 1=1 " + whereStr;
            return QueryTable(sel_sql).GetFirstCol<string>();
        }

        public List<KeyValuePair<string, string>> GetListKeyValue<T>(string colKeyName, string colValName, string joinWhere = "")
        {
            string tableName = typeof(T).Name;
            string sel_sel = "select distinct a." + colKeyName + " as text,a." + colValName + " as id" + " from " + tableName + " a " + joinWhere;
            return QueryTable(sel_sel).ConvertToListObject<KeyValuePair<string, string>>();
        }

        public List<T> GetAll<T>(string orderByStr = "")
        {
            string tableName = typeof(T).Name;
            CheckTabStuct(tableName);
            string sel_sql = "select * from " + tableName + " " + (string.IsNullOrWhiteSpace(orderByStr) ? "" : " order by" + orderByStr);
            return QueryTable(sel_sql, 300).ConvertToListObject<T>();
        }

        #endregion

        #region internal
        internal Hashtable ConvertHashTable(object obj, List<string> lstAllColName = null, params string[] excludeColNames)
        {
            Hashtable ht = new Hashtable();
            excludeColNames = excludeColNames ?? new string[] { };
            if (obj is IDictionary || obj is ExpandoObject)
            {
                IDictionary dict = ((IDictionary)obj);
                foreach (DictionaryEntry entry in dict)
                {
                    string name = entry.Key.ToString();
                    if (lstAllColName != null &&
                        lstAllColName.Contains(name, StringComparer.OrdinalIgnoreCase) &&
                        !excludeColNames.Contains(name, StringComparer.OrdinalIgnoreCase)
                        )
                    {
                        ht.Add(name, entry.Value);
                    }
                    else if (lstAllColName == null && !excludeColNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                    {
                        ht.Add(name, entry.Value);
                    }
                }
            }
            else if (obj is NameValueCollection)
            {
                NameValueCollection nvc = obj as NameValueCollection;
                foreach (string key in nvc.AllKeys)
                {
                    string name = key.ToLower();
                    if (lstAllColName != null &&
                        lstAllColName.Contains(name, StringComparer.OrdinalIgnoreCase) &&
                          !excludeColNames.Contains(name, StringComparer.OrdinalIgnoreCase)
                        )
                    {
                        ht.Add(name, nvc[key]);
                    }
                    else if (lstAllColName == null && !excludeColNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                    {
                        ht.Add(name, nvc[key]);
                    }
                }
            }
            else if (obj is DataRow)
            {
                DataRow dr = obj as DataRow;
                DataTable data = dr.Table;
                foreach (DataColumn dc in data.Columns)
                {
                    string name = dc.ColumnName.ToLower();
                    if (lstAllColName != null &&
                        lstAllColName.Contains(name, StringComparer.OrdinalIgnoreCase) &&
                          !excludeColNames.Contains(name, StringComparer.OrdinalIgnoreCase)
                        )
                    {
                        ht.Add(name, dr[dc.ColumnName]);
                    }
                    else if (lstAllColName == null && !excludeColNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                    {
                        ht.Add(name, dr[dc.ColumnName]);
                    }
                }
            }
            else
            {
                Type tyTable = obj.GetType();
                PropertyInfo[] pyInfos = tyTable.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (PropertyInfo pInfo in pyInfos)
                {
                    string name = pInfo.Name.ToString();
                    object objvalue = null;

                    if (lstAllColName != null &&
                        lstAllColName.Contains(name, StringComparer.OrdinalIgnoreCase) &&
                            !excludeColNames.Contains(name, StringComparer.OrdinalIgnoreCase)
                        )
                    {
                        objvalue = pInfo.GetValue(obj, null);
                        ht.Add(name, objvalue);
                    }
                    else if (lstAllColName == null && !excludeColNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                    {
                        ht.Add(name, objvalue);
                    }
                }
            }
            return ht;
        }

        internal void CheckTabStuct(string tableName, params string[] columnNames)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName", "不能为空！");
            }

            if (!Info.TableNames.Contains(tableName, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(string.Format("不存在该表！{0}", "[" + tableName + "]", "tableName"));
            }

            if (columnNames != null && columnNames.Length > 0)
            {
                List<string> lstAllColName = Info[tableName];

                foreach (string columnName in columnNames)
                {
                    if (!lstAllColName.Contains(columnName, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException(string.Format("不存在该列！{0}", "[" + tableName + "." + columnName + "]", "columnName"));
                    }
                }
            }
        }

        /// <summary>
        /// 获取其对应的  DbType 类型
        /// </summary>
        /// <param name="cInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal DbType GetValueDbType(ColumnInfo cInfo, object value, AccessType dbBaseType,out object newValue)
        {
            DbType dbType = DbType.AnsiString;
            string typeName = cInfo.TypeName.ToLower();
            newValue = value;
            if (value == null && !cInfo.IsIdentity)
            {
                if (!cInfo.CanNull)
                {
                    throw new ArgumentNullException(cInfo.ColumnName, "不能为null！");
                }
            }
            else
            {
                //if (cInfo.ColumnName == "BKTXT" && newValue.ToString()== "涿州市利顺来保温材料厂010101010101011")
                //{

                //}
                if (typeName.Contains("char"))
                {
                    int len = 0;
                    if (typeName.Equals("varchar"))
                    {
                        len = newValue.ToString().DataLength();
                    }
                    else if (typeName.Equals("nvarchar"))
                    {
                        len = newValue.ToString().Length * 2;
                    }
                    if (cInfo.Length !=-1 && len > cInfo.Length)
                    {
                        throw new ArgumentNullException(cInfo.ColumnName, cInfo.ColumnName + "字符串长度超出限制！(" + len + ">" + cInfo.Length + ")");
                    }
                }
                if (typeName.Contains("date"))
                {
                    dbType = DbType.DateTime;
                    newValue = value.ConvertTo<DateTime>(cInfo.ColumnName);
                }
                else if (typeName.Contains("uniqueidentifier"))
                {
                    dbType = DbType.Guid;
                    newValue = value.ConvertTo<Guid>(cInfo.ColumnName);
                }
                else if (typeName.Contains("numeric"))
                {
                    dbType = DbType.Double;
                    newValue = value.ConvertTo<double>(cInfo.ColumnName);
                }
                else if (value is byte[])
                {
                    dbType = DbType.Binary;
                }
                else if (true)
                {
                   
                }

            }
            return dbType;
        }
        #endregion

        #endregion


    }
}
