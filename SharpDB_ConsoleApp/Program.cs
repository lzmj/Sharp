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
using SharpDB.Entity;
using System.Data.SqlClient;

namespace SharpDB_ConsoleApp
{
    class Program
    {

        static string connStr = @"data source=10.1.19.189\MSSQLSERVER1;uid=aiws;pwd=aiws;database=AIWS;Pooling=true;";
        static void Main(string[] args)
        {
            //connStr = @"data source=10.5.98.136\SJYJDB1,62427;uid=aiws;pwd=aiws;database=AIWS_V2";
            //ExecDesc();
            //Console.WriteLine("执行完成！");
            //Console.ReadKey();


            //DB db = new DB(AccessType.MsSql, connStr);

            //DBInfo dbInfo = db.Info;

            //string table_Name = "ZDSJ_CT_WDZX";

            //List<ColumnInfo> lstColInfo = dbInfo.GetAllColumnInfo(table_Name);

            //while (true)
            //{
            //    Dictionary<string, object> dict = new Dictionary<string, object>();
            //    foreach (ColumnInfo colInfo in lstColInfo)
            //    {
            //        if (colInfo.IsIdentity)
            //        {
            //            continue;
            //        }
            //        dict.Add(colInfo.ColumnName, Rdom(colInfo));
            //    }
            //    bool res = db.Insert(dict, table_Name);
            //}

        }


        static Random rdom = new Random();
        public static object Rdom(ColumnInfo colInfo)
        {
            object result = null;
            string columnName = colInfo.ColumnName;
            if (columnName == "LOC_CURRCY")
            {
                result = "RMB";
            }
            else if (columnName == "LOGSYS")
            {
                #region
                List<string> lst = new List<string>
                {
                    "P02CLNT900",
                    "P03CLNT900",
                    "P05CLNT900",
                    "P06CLNT900",
                    "P07CLNT900",
                    "P08CLNT880",
                    "P09CLNT960",
                    "P10CLNT900",
                    "P14CLNT800",
                    "P16_800",
                    "P17CLNT800",
                    "P18CLNT960",
                    "P19CLNT900",
                    "P20CLNT900",
                    "P21CLNT900",
                    "P22CLNT900",
                    "P23CLNT900",
                    "P24CLNT900",
                    "P25CLNT900",
                    "P26900",
                    "P28CLNT800",
                    "P31CLNT800",
                    "P33CLNT900",
                    "P34CLNT900",
                    "P35CLNT900",
                    "P36CLNT960",
                    "P37CLNT900",
                    "P38CLNT900",
                    "P39CLNT900",
                    "P40CLNT900",
                    "P41CLNT900",
                    "P42CLNT960",
                    "P43CLNT900",
                    "P45CLNT900",
                    "P46_900",
                    "P50CLNT800",
                    "P52CLNT900",
                    "P53CLNT800",
                    "P54CLNT900",
                    "P56CLNT900",
                    "P63CLNT800",
                    "PI1CLNT800",
                    "PO1CLNT900",
                    "PO3CLNT800",
                    "PR1CLNT900",
                    "PR3CLNT800",
                    "PRDCLNT900",
                    "PS1CLNT800",
                    "PS3CLNT800",
                    "PS5CLNT800",
                    "PS7CLNT800"
                };




                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "BUKRS")
            {
                #region

                List<string> lst = new List<string>
                {"100F",
                "1010",
                "1011",
                "1013",
                "1015",
                "1016",
                "1018",
                "1019",
                "101A",
                "101B",
                "101C",
                "101D",
                "1020",
                "1022",
                "1040",
                "1041",
                "1046",
                "1047",
                "1060",
                "1061",
                "1062",
                "1063",
                "1080",
                "1090",
                "1091",
                "1092",
                "1093",
                "1100",
                "1101",
                "1120",
                "1121",
                "1122",
                "1123",
                "1124",
                "1125",
                "1127",
                "1130",
                "1140",
                "1146",
                "1147",
                "1149",
                "114A",
                "114B",
                "114C",
                "114D",
                "114E",
                "114F",
                "114G"
                };

                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "GJAHR")
            {
                #region MyRegion
                List<int> lst = new List<int>
                {
                    2014,2015,2016
                }; 
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "MONAT")
            {
                #region MyRegion
                List<int> lst = new List<int>
                {
                    1,
                    3,
                    15,
                    16,
                    14,
                    2,
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "PRCTR")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
                    "9350035661",
                    "9361031007",
                    "9361051006",
                    "9345003636",
                    "9345012628",
                    "9321008861",
                    "9350007408",
                    "9350039711",
                    "92666DK700",
                    "9345016610",
                    "9345003200",
                    "9345011658",
                    "9SR0029650",

                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "HBKID")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
                    "12902",
                    "30001",
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "BANKL")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
                    "103599054664",
                    "302143001182",
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "HKTID")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
                    "ID020",
                    "ID021",
                    "ID010",
                    "ID025",

                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "HKONT")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
                    "1009111610",
                    "1011061002",
                    "10211274P0",
                    "5503020100",
                    "1021103872",
                    "1002127030",
                    "1021127971",
                    "10211022X0",
                    "1805050000",
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "TEXT1")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {

                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "QYTZHJE")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    10000.00,
                    20000.00,
                    30000.00,
                    40000.00,
                    50000.00
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "YJTZHJE")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    1000.00,
                    2000.00,
                    3000.00,
                    4000.00,
                    5000.00
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "TZHCY")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    100.00,
                    200.00,
                    300.00,
                    400.00,
                    500.00
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "QYYE")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    10.00,
                    20.00,
                    30.00,
                    40.00,
                    50.00
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "YSQWS")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    1.00,
                    2.00,
                    3.00,
                    4.00,
                    5.00
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "YFQWF")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    0.100,
                    0.200,
                    0.300,
                    0.400,
                    0.500
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "QSYWS")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    0.0100,
                    0.0200,
                    0.0300,
                    0.0400,
                    0.0500
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "QFYWF")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    10.00,
                    20.00,
                    30.00,
                    40.00,
                    50.00
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "YHYE")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    100.00,
                    200.00,
                    300.00,
                    400.00,
                    500.00
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "RSNUM")//预留/相关需求的编号
            {
                return rdom.Next(1000000000, 1000000009);
            }
            else if (columnName == "VWEZW")//支付注解说明
            {
                #region MyRegion
                List<string> lst = new List<string>
                {

                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "KWBTR")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    10000,
                    20000,
                    30000
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "AZDAT")
            {
                return rdom.NextDateTime(DateTime.Parse("2013-01-01"), DateTime.Now).ToString("yyyyMMdd");
            }
            else if (columnName == "BELNR")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
                    "1020004220",
                    "0200017910",
                    "0200015734",
                    "1020002835",
                    "0200142045",
                    "4900002882",
                    "0200298646",

                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "BUDAT")
            {
                return rdom.NextDateTime(DateTime.Parse("2013-01-01"), DateTime.Now).ToString("yyyyMMdd");
            }
            else if (columnName == "DMBTR")
            {
                #region MyRegion
                List<double> lst = new List<double>
                {
                    1000,
                    2000,
                    3000
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "BKTXT")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
"12345123451234512345123451",
"北京立鑫伟业建材厂",
"北京圆三角玛钢厂",
"三河市宏达钢管厂",
"北京市丁各庄砖厂",
"北京南火垡砖厂",
"北京丰阳页岩砖厂",
"龙湖置业有限公司",
"北京恒勤开源科技有限公司",
"北京市卡利特精细化工公司",
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "TABTYPE")
            {
                #region MyRegion
                List<int> lst = new List<int>
                {
                    1,2,3,4
                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "G0LRZXMS")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
"中石化宁夏石油塑料制品有限公司",
"中石化江苏江阴油品储运有限公司",
"宁夏控股兴俊银川金凤区商客中心",
"陕西中联有限高陵通华站商客中心",
"宁夏控股常道大武口区润滑油中心",
"宁夏控股兴俊银川永宁县零售中心",
"陕西中联有限周至顺达站商客中心",
"陕西中联高陵通华非油品业务中心",
"宁夏控股兴俊银川西夏区零售中心",
"宁夏控股易捷沙坡头区非油品中心",
"新疆奎屯独山子分公司本部非油品",
"宁夏控股兴俊银川金凤区物流中心",
"宁夏控股兴俊银川灵武市零售中心",


                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (columnName == "GSMC")
            {
                #region MyRegion
                List<string> lst = new List<string>
                {
                    "中国石油化工股份有限公司石油化工科学研究",
"北京兴普精细化工技术开发有限公司",
"中国石油化工股份有限公司北京化工研究院",
"北京北化院石化设计院有限公司",
"中国石油化工股份有限公司北京北化院燕山分",
"北京中石瑞柏质检有限公司",
"中国石油化工股份有限公司抚顺石油化工研究",
"中国石油化工股份有限公司上海石油化工研究",
"中国石油化工股份有限公司青岛安全工程研究",
"青岛中化阳光管理体系认证中心有限公司",
"中国石油化工股份有限公司石油勘探开发研究",
"石油勘探开发研究院德州石油钻井研究所",

                };
                #endregion
                return rdom.NextRdomItem(lst.ToArray());
            }
            else if (
                columnName == "CREATOR_ID"
                || columnName == "MODIFY_ID"
                || columnName == "SESSION_ID"
                )
            {
                result = "0B7ABE24-7AB2-4BE3-9036-382B085D177B";
            }
            else if (columnName == "CREATE_TIME"
                || columnName == "MODIFY_TIME"
                )
            {
                result = DateTime.Now;
            }
            return result;
        }





        static void ExecDesc()
        {
            DB db = new DB(AccessType.MsSql, connStr);
            DBInfo dbInfo = db.Info;
            string strSql = "SELECT TABLE_ID,TABLE_NAME,DESCRIPTION FROM dbo.DICT_DATA_TABLE";
            DataTable data = db.QueryTable(strSql);
            foreach (DataRow dr in data.Rows)
            {
                string tabName = dr["TABLE_NAME"].ToString();
                string desc = dr["DESCRIPTION"].ToString();
                if (dbInfo.IsExistTable(tabName))
                {
                    dbInfo.UpsertTableComment(tabName, desc);

                    string table_Id = dr["TABLE_ID"].ToString();
                    strSql = @"SELECT b.FIELD_CODE,b.FIELD_NAME FROM dbo.DICT_DATA_COLUMNS a 
LEFT JOIN dbo.DICT_DATA_FIELD b ON a.FIELD_ID = b.FIELD_ID
where a.TABLE_ID = '{0}'";
                    strSql = string.Format(strSql, table_Id);

                    DataTable data_col = db.QueryTable(strSql);

                    foreach (DataRow drCol in data_col.Rows)
                    {
                        string fieldCode = drCol["FIELD_CODE"].ToString();
                        string fieldName = drCol["FIELD_NAME"].ToString();

                        if (dbInfo.IsExistColumn(tabName, fieldCode))
                        {
                            dbInfo.UpsertColumnComment(tabName, fieldCode, fieldName);
                        }
                    }

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
