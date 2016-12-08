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
            DB db = new DB(AccessType.OracleDDTek);
            DBInfo dbInfo = db.Info;
            var lstTableNames = dbInfo.TableNames;
            lstTableNames.Remove("PARAM_VALUES_TAB");


            lstTableNames = lstTableNames.Where(t => !t.Contains("SYS_IOT") && !t.Contains("MGMT_")).ToList();

            Dictionary<string, long> dict = new Dictionary<string, long>();
            foreach (string tableName in lstTableNames)
            {
                string query_sql = "select count(1) from " + tableName;
                long lg = db.GetSingle<long>(query_sql);
                dict.Add(tableName, lg);
            }

            var sortDict = dict.OrderByDescending(t => t.Value).ToDictionary(k => k.Key, v => v.Value);
            

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
