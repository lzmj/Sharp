using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data;
using SharpTools.Entity;
using SharpDB.Entity;
using System.Linq;

namespace SharpTools.Common
{
    public class Utils
    {
        static Utils()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), "SharpTools");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 数据库连接配置文件
        /// </summary>
        public static readonly string DatabaseconfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), "SharpTools", "databaseconfig.xml");


        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public static List<Entity.Connection> GetConnectionList()
        {
            List<Entity.Connection> list = new List<SharpTools.Entity.Connection>();
            XmlDocument doc = getXmlDocument();

            XmlNodeList xmlNodeList = doc.SelectNodes("servers/server");
            if (null != xmlNodeList && xmlNodeList.Count > 0)
            {
                foreach (XmlNode node in xmlNodeList)
                {
                    if (!node.HasChildNodes)
                        continue;

                    Entity.Connection connection = new SharpTools.Entity.Connection();

                    connection.ID = new Guid(node.Attributes["id"].Value);
                    connection.Name = node.Attributes["name"].Value;
                    connection.Database = node.Attributes["database"].Value;
                    connection.ConnectionString = node.FirstChild.InnerText;
                    connection.DbType = node.Attributes["dbtype"].Value.ConvertTo<DataBaseType>();
                    list.Add(connection);

                }
            }
            return list;
        }


        static XmlDocument getXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(DatabaseconfigPath))
            {
                File.WriteAllText(DatabaseconfigPath, @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<servers>
</servers>
", Encoding.UTF8);
                //System.Threading.Thread.Sleep(2000);
            }


            doc.Load(DatabaseconfigPath);


            return doc;

        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteConnection(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            XmlDocument doc = getXmlDocument();

            XmlNodeList xmlNodeList = doc.SelectNodes("servers/server");
            if (null != xmlNodeList && xmlNodeList.Count > 0)
            {
                foreach (XmlNode node in xmlNodeList)
                {
                    if (node.Attributes["id"].Value.Trim().ToLower().Equals(id.Trim().ToLower()))
                    {
                        node.ParentNode.RemoveChild(node);
                        break;
                    }
                }
            }

            doc.Save(DatabaseconfigPath);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="conection"></param>
        public static void AddConnection(Entity.Connection conection)
        {
            XmlDocument doc = getXmlDocument();

            XmlNode root = doc.SelectSingleNode("servers");

            XmlElement xe = doc.CreateElement("server");

            xe.SetAttribute("id", conection.ID.ToString());
            xe.SetAttribute("name", conection.Name);
            xe.SetAttribute("database", conection.Database);
            xe.SetAttribute("dbtype", conection.DbType.ToString());

            XmlElement xe1 = doc.CreateElement("connectionstring");
            XmlCDataSection cdata = doc.CreateCDataSection(conection.ConnectionString);
            xe1.AppendChild(cdata);

            xe.AppendChild(xe1);

            root.AppendChild(xe);

            doc.Save(DatabaseconfigPath);
        }

        /// <summary>
        /// 获取一个连接配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.Connection GetConnectionModel(string id)
        {
            Entity.Connection connModel = null;
            if (string.IsNullOrEmpty(id))
                return connModel;

            XmlDocument doc = new XmlDocument();
            doc.Load(DatabaseconfigPath);

            XmlNode xmlNode = doc.SelectSingleNode("servers/server[@id='" + id.ToString() + "']");
            if (null != xmlNode)
            {
                connModel = new SharpTools.Entity.Connection();
                connModel.ID = new Guid(xmlNode.Attributes["id"].Value);
                connModel.Name = xmlNode.Attributes["name"].Value;
                connModel.Database = xmlNode.Attributes["database"].Value;
                connModel.ConnectionString = xmlNode.FirstChild.InnerText;
                connModel.DbType = xmlNode.Attributes["dbtype"].Value.ConvertTo<DataBaseType>();
            }

            return connModel;
        }

        /// <summary>
        /// 系统配置路径
        /// </summary>
        public static string SysconfigPath = Application.StartupPath + "/Config/sysconfig.xml";


        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        public static Entity.Sysconfig GetSysconfigModel()
        {
            Entity.Sysconfig sysconfigModel = new Sysconfig();

            XmlDocument doc = new XmlDocument();
            doc.Load(SysconfigPath);
            XmlNode node = doc.SelectSingleNode("configs/config[@key='namespace']");
            if (null != node)
            {
                sysconfigModel.Namespace = node.FirstChild.InnerText;
            }
            node = doc.SelectSingleNode("configs/config[@key='batchdirectorypath']");
            if (null != node)
            {
                sysconfigModel.BatchDirectoryPath = node.FirstChild.InnerText;
            }

            return sysconfigModel;
        }


        /// <summary>
        /// 设置系统配置
        /// </summary>
        /// <returns></returns>
        public static void GetSysconfigModel(Entity.Sysconfig sysconfigModel)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SysconfigPath);
            XmlNode node = doc.SelectSingleNode("configs/config[@key='namespace']");
            if (null != node)
            {
                node.FirstChild.Value = sysconfigModel.Namespace;
            }
            node = doc.SelectSingleNode("configs/config[@key='batchdirectorypath']");
            if (null != node)
            {
                node.FirstChild.Value = sysconfigModel.BatchDirectoryPath;
            }

            doc.Save(SysconfigPath);
        }

        /// <summary>
        /// 写命名空间
        /// </summary>
        /// <param name="names"></param>
        public static void WriteNamespace(string names)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SysconfigPath);
            XmlNode node = doc.SelectSingleNode("configs/config[@key='namespace']");
            node.FirstChild.Value = names;

            //XmlCDataSection cdata = doc.CreateCDataSection(names);
            //node.AppendChild(cdata);

            doc.Save(SysconfigPath);
        }


        /// <summary>
        /// 写批量路径
        /// </summary>
        /// <param name="names"></param>
        public static void WriteBatchDirectoryPath(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SysconfigPath);
            XmlNode node = doc.SelectSingleNode("configs/config[@key='batchdirectorypath']");
            node.FirstChild.Value = path;

            //XmlCDataSection cdata = doc.CreateCDataSection(path);
            //node.AppendChild(cdata);

            doc.Save(SysconfigPath);
        }

        /// <summary>
        /// 读命名空间
        /// </summary>
        /// <returns></returns>
        public static string ReadNamespace()
        {
            return string.Empty;
            //return GetSysconfigModel().Namespace;
        }


        /// <summary>
        /// 读保存的批量导出路径
        /// </summary>
        /// <returns></returns>
        public static string ReadBatchDirectoryPath()
        {
            return string.Empty;
            //return GetSysconfigModel().BatchDirectoryPath;
        }


        /// <summary>
        /// 列信息装换
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<ColumnInfo> GetColumnInfos(DataTable dt)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();
            

            return list;
        }

        public static DataTable GetColumnInfoDataTable(List<ColumnInfo> collist)
        {
            DataTable table = new DataTable();
           
            return table;
        }

        private static System.Text.RegularExpressions.Regex regSpace = new System.Text.RegularExpressions.Regex(@"\s");

        /// <summary>
        /// 去掉空格
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceSpace(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            char firstChar = value[0];
            if (firstChar >= 48 && firstChar <= 57)
            {
                //value = "F" + value;
                value = "_" + value;
            }
            return regSpace.Replace(value.Trim(), " ");
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUpperFirstword(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            string result = string.Empty;
            string[] words = value.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (value.Length <= 1)
            {
                return value.ToUpper();
            }
            else if (words.Length <= 1)
            {
                result = value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1).ToLower();
            }
            else
            {
                List<string> lst = new List<string>();
                foreach (string word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        string temp = word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1).ToLower();
                        lst.Add(temp);
                    }
                }
                result = string.Join("_", lst);
            }
            return result;
        }


        public static DataTable CreateDataTable(params string[] columnNames)
        {
            DataTable data = new DataTable();
            if (columnNames != null && columnNames.Length > 0)
            {
                data.Columns.AddRange(columnNames.Select(t => new DataColumn(t)).ToArray());
            }
            return data;
        }


        /// <summary>
        /// 判断磁盘路径下是否安装存在某个文件，最后返回存在某个文件的路径
        /// </summary>
        /// <param name="installPaths"></param>
        /// <param name="installPath"></param>
        /// <returns></returns>
        public static bool IsInstall(string[] installPaths, out string installPath)
        {
            installPath = string.Empty;
            var driInfos = DriveInfo.GetDrives();
            foreach (DriveInfo dInfo in driInfos)
            {
                if (dInfo.DriveType == DriveType.Fixed)
                {
                    foreach (string ipath in installPaths)
                    {
                        string path = Path.Combine(dInfo.Name, ipath);
                        if (File.Exists(path))
                        {
                            installPath = path;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
