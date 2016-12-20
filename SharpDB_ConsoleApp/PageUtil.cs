using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SharpDB_ConsoleApp
{
    public  class PageUtil
    {
       private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["MsSql"].ConnectionString;

        #region 存储过程分页
        /// <summary>
        /// 调用 存储过程分页
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageSize">当前分页索引</param>
        /// <param name="tablename">表名 字查询则括号括起来 并加表别名</param>
        /// <param name="whereString">过滤条件</param>
        /// <param name="orderString">排序</param>
        /// <param name="fields">查询的字段，默认 * </param>
        /// <param name="recordCount">记录数</param>
        /// <returns>查询数据</returns>
        public static DataTable GetDataTableByPager(int pageSize, int currentPage, string fields, string orderString, string whereString, string tablename, out int recordCount)
        {
            if (fields == String.Empty)
            {
                fields = "*";
            }
            recordCount = 0;
            DataTable table = new DataTable();
            SqlParameter[] param ={ new SqlParameter("@pageSize",pageSize) ,
               new SqlParameter("@currentPage",currentPage) ,
               new SqlParameter("@fields",fields) ,
               new SqlParameter("@orderString",orderString) ,
               new SqlParameter("@tablename",tablename) ,
               new SqlParameter("@whereString",whereString) ,
           };

            DataSet ds = new DataSet();
            ds = RunProcedure("sp_GetListByPageAndFileds", param);
            table = ds.Tables[0];
            recordCount = Convert.ToInt32(ds.Tables[1].Rows[0]["cnt"].ToString());
            return table;
        }

        private static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet);
                connection.Close();
                return dataSet;
            }
        }
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
        #endregion
    }
}
