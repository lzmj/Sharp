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

        static void Main(string[] args)
        {
            //DB db = new DB(AccessType.OracleDDTek);
            //DBInfo dbInfo = db.Info;


            //int pageSize = 10;
            //int currentPage = 1;
            //string fields = "*";
            //string orderString = "CREATE_TIME asc"; string whereString = "LEN(EMAIL)>0";
            //string tablename = "BASE_USER";
            //int recordCount = 0;
            //DataTable data = null;

            //data= PageUtil.GetDataTableByPager(pageSize, currentPage, fields, orderString, whereString, tablename, out recordCount);

            DB db = new DB(AccessType.MsSql);
            
         
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
