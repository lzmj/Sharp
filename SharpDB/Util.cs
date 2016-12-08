using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpDB
{
    internal static class Util
    {


        internal static DbType GetValueDbType(object objvalue, DbParameter para)
        {
            Type tyval = objvalue.GetType();
            Type typeVal = Nullable.GetUnderlyingType(tyval) ?? tyval;

            DbType dbType = DbType.AnsiString;
            DateTime dtime;
            Guid gid;
            if (typeVal.IsEnum && Enum.GetUnderlyingType(typeVal) == typeof(byte))
            {
                dbType = DbType.Int32;
            }
            else if (typeVal == typeof(byte[]))
            {
                dbType = DbType.Binary;
            }
            else if (DateTime.TryParse(objvalue.ToString(), out dtime)
               && !Regex.IsMatch(objvalue.ToString(), @"^\d{4}[.-/]{1}\d{1,2}$"))
            {
                dbType = DbType.DateTime;
            }
            else if (Guid.TryParse(objvalue.ToString(), out gid))
            {
                dbType = DbType.String;
                para.Value = objvalue.ToString();
            }
            else
            {

            }
            return dbType;
        }

        /// <summary>
        /// 拼接in sql语句
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="columnName">列名</param>
        /// <param name="values">元素值</param>
        /// <param name="isNotIn">not in 或 in </param>
        /// <returns></returns>
        public static string SqlIn<T>(string columnName, T[] values, bool isNotIn = false)
        {
            string result = string.Empty;
            if (values == null || values.Length <= 0)
            {
                return string.Empty;
            }
            List<string> lst = new List<string>();
            foreach (T obj in values)
            {
                if (obj != null)
                {
                    string val = obj.ToString();
                    if (val.StartsWith("'") && val.EndsWith("'"))
                    {
                        val = val.Replace("'", "'''");
                        lst.Add(val);
                        continue;
                    }
                    lst.Add("'" + val + "'");
                }
            }
            if (lst.Count > 0)
            {
                result = " and " + columnName + " " + (isNotIn ? "not" : "") + " in (" + string.Join(",", lst) + ") ";
            }
            return result;
        }

        /// <summary>
        /// 拼接多条 like 语句
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="columnName">列名</param>
        /// <param name="values">元素值</param>
        /// <param name="isOrLike">or like 或 and like</param>
        /// <returns></returns>
        public static string SqlLike<T>(string columnName, T[] values, bool isOrLike = true)
        {
            string result = string.Empty;
            if (values == null || values.Length <= 0)
            {
                return string.Empty;
            }
            List<string> lst = new List<string>();
            foreach (T obj in values)
            {
                if (obj != null)
                {
                    string like_sql = columnName + " like '%{0}%' ";
                    string temp_sql = string.Empty;
                    string val = obj.ToString();
                    if (val.StartsWith("'") && val.EndsWith("'"))
                    {
                        val = val.Replace("'", "''");
                        temp_sql = string.Format(like_sql, val);
                        lst.Add(temp_sql);
                        continue;
                    }
                    temp_sql = string.Format(like_sql, val);
                    lst.Add(temp_sql);
                }
            }
            if (lst.Count>0)
            {
                result = " and (" + (string.Join((isOrLike ? " or" : " and "), lst)) + ") ";
            }            
            return result;
        }


        /// <summary>
        /// 创建指定 列名字的数据表
        /// </summary>
        /// <param name="columnNames">数据列名</param>
        /// <returns></returns>
        public static DataTable CreateDataTable(params string[] columnNames)
        {
            DataTable data = new DataTable();
            if (columnNames != null && columnNames.Length > 0)
            {
                data.Columns.AddRange(columnNames.Select(t => new DataColumn(t)).ToArray());
            }
            return data;
        }
        
    }
}
