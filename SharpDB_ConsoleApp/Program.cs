using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Timers;
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;

using System.Diagnostics;
using System.Collections.Concurrent;
using System.IO;
using SharpDB;

namespace SharpDB_ConsoleApp
{
    class Program
    {

        static string connStr = @"data source=10.1.19.189\MSSQLSERVER1;uid=aiws;pwd=aiws;database=AIWS";
        static void Main(string[] args)
        {
            DB db = new DB(AccessType.MsSql, connStr);

            string strSql = "SELECT * FROM ZDSJ_CT_WDZX";

            DataTable data = db.QueryTable(strSql);





        }


        void ExecTableDesc()
        {
            DB db = new DB(AccessType.MsSql, connStr);
            DBInfo dbInfo = db.Info;
            string strSql = "SELECT TABLE_NAME,DESCRIPTION FROM dbo.DICT_DATA_TABLE";
            DataTable data = db.QueryTable(strSql);
            foreach (DataRow dr in data.Rows)
            {
                string tabName = dr["TABLE_NAME"].ToString();
                string desc = dr["DESCRIPTION"].ToString();
                if (dbInfo.IsExistTable(tabName))
                {
                    dbInfo.UpsertTableComment(tabName, desc);
                }
            }
        }


        static void SW(Action act)
        {
            Stopwatch wat = new Stopwatch();
            wat.Start();
            act.Invoke();
            wat.Stop();
            long lenTime = wat.ElapsedMilliseconds;
            Console.WriteLine(lenTime);
        }



    }
}
