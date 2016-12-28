using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Linq;
using SharpTools.Common;
using WeifenLuo.WinFormsUI.Docking;
using SharpTools.Forms;

namespace SharpTools
{
    public partial class LeftPanel : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public LeftPanel()
        {
            InitializeComponent();
        }

        public delegate void NewContentForm(DockContentForm dkContentFm, Entity.Connection conModel, string tableName, bool isView);

        public event NewContentForm newcontentForm;

        /// <summary>
        /// 新建数据库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolStripButton1_Click(object sender, EventArgs e)
        {
            showDbSelect();
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        internal void showDbSelect()
        {
            DatabaseSelect dbSelect = new DatabaseSelect();

            if (dbSelect.ShowDialog() == DialogResult.OK)
            {
                DialogResult dia = DialogResult.No;

                switch (DatabaseSelect.DatabaseType)
                {
                    case DataBaseType.MsSql:
                        DbSelect.DBSqlServer dbsqlserver = new SharpTools.DbSelect.DBSqlServer();
                        dia = dbsqlserver.ShowDialog();
                        break;
                    case DataBaseType.Oracle:
                        DbSelect.DBOracle dbOracle = new SharpTools.DbSelect.DBOracle();
                        dia = dbOracle.ShowDialog();
                        break;
                    case DataBaseType.Sqlite:
                        DbSelect.DbSqlite dbSqlite = new SharpTools.DbSelect.DbSqlite();
                        dia = dbSqlite.ShowDialog();
                        break;
                    case DataBaseType.MySql:
                        DbSelect.DBMySql dbMySql = new SharpTools.DbSelect.DBMySql();
                        dia = dbMySql.ShowDialog();
                        break;
                    default:
                        break;
                }

                if (dia == DialogResult.OK)
                {
                    refreshConnectionList();
                }
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        List<Entity.Connection> list;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftPanel_Load(object sender, EventArgs e)
        {
            this.CloseButtonVisible = false;
            getServers();
        }

        #region top servers


        /// <summary>
        /// 获取服务器列表
        /// </summary>
        private void getServers()
        {
            tview.Nodes.Clear();

            tview.Nodes.Add("服务器", "服务器", 0);

            list = Utils.GetConnectionList();

            TreeNode node = tview.Nodes[0];

            node.ContextMenuStrip = contextMenuStripTop;
            foreach (Entity.Connection connection in list)
            {
                TreeNode nnode = new TreeNode(connection.Name, 0, 0);
                nnode.ContextMenuStrip = contextMenuStripDatabase;
                nnode.Tag = connection.ID.ToString();
                //窗体加载时就直接 把节点生成
                getDatabaseinfo(nnode);
                node.Nodes.Add(nnode);
            }
            tview.MouseDoubleClick += Tview_MouseDoubleClick;
        }

        private void Tview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNode currNode = tview.SelectedNode;
            if (currNode.Level == 4 && currNode.Tag != null && currNode.Tag.ToString() == "T")
            {
                打开ToolStripMenuItem_Click(sender, e);
            }
        }

        /// <summary>
        /// 右击选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ((TreeView)sender).SelectedNode = e.Node;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showDbSelect();
        }

        /// <summary>
        /// 刷新服务器
        /// </summary>
        private void refreshConnectionList()
        {
            List<Entity.Connection> connList = Utils.GetConnectionList();

            foreach (Entity.Connection conn in connList)
            {
                Entity.Connection tempconn = list.Find(delegate (Entity.Connection connin) { return conn.ID.ToString().Equals(connin.ID.ToString()); });
                if (null == tempconn)
                {
                    TreeNode nnode = new TreeNode(conn.Name, 0, 0);
                    nnode.ContextMenuStrip = contextMenuStripDatabase;
                    nnode.Tag = conn.ID.ToString();
                    tview.Nodes[0].Nodes.Add(nnode);
                }
            }

            list = connList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshConnectionList();
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string stringid = tview.SelectedNode.Tag.ToString();
            Utils.DeleteConnection(stringid);
            Entity.Connection tempconn = list.Find(delegate (Entity.Connection conn) { return conn.ID.ToString().Equals(stringid); });
            if (null != tempconn)
                list.Remove(tempconn);
            tview.Nodes.Remove(tview.SelectedNode);
        }

        #endregion

        #region database

        private void 连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = tview.SelectedNode;
            node.Nodes.Clear();
            getDatabaseinfo(node);
        }



        private void 刷新ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = tview.SelectedNode;
            node.Nodes.Clear();
            getDatabaseinfo(node);
        }


        /// <summary>
        /// 
        /// </summary>
        private void getDatabaseinfo(TreeNode currNode)
        {
            TreeNode node = currNode;

            Entity.Connection conModel = list.Find(t => t.ID.ToString().Equals(node.Tag.ToString(), StringComparison.OrdinalIgnoreCase));

            DB db = null;
            DBInfo dbInfo = null;

            db = new DB(conModel.DbType, conModel.ConnectionString);
            dbInfo = db.Info;

            if (!conModel.Database.Equals("all"))
            {
                TreeNode tnode = new TreeNode(conModel.Database, 1, 1);
                tnode.Tag = conModel.ConnectionString;
                tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                node.Nodes.Add(tnode);

                gettables(tnode, dbInfo.TableNames, dbInfo.GetAllViews());
            }
            else
            {
                DataTable dt = new DataTable();
                if (conModel.DbType == DataBaseType.MsSql)
                {
                    dt = db.QueryTable("Select name from master..sysdatabases");
                }
                else if (conModel.DbType == DataBaseType.MySql)
                {
                    dt = db.QueryTable("SHOW DATABASES");
                }


                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode tnode = new TreeNode(dr[0].ToString(), 1, 1);
                    if (conModel.DbType == DataBaseType.MsSql)
                    {
                        tnode.Tag = conModel.ConnectionString.Replace("master", dr[0].ToString());
                    }
                    else if (conModel.DbType == DataBaseType.MySql)
                    {
                        tnode.Tag = conModel.ConnectionString.ToLower().Replace("database=", "database=" + dr[0].ToString());
                    }

                    tnode.ContextMenuStrip = contextMenuStripOneDataBase;
                    node.Nodes.Add(tnode);

                    db = new DB(conModel.DbType, tnode.Tag.ToString());
                    dbInfo = db.Info;

                    gettables(tnode, dbInfo.TableNames, dbInfo.GetAllViews());
                }
            }
        }

        private void gettables(TreeNode databaseNodel, List<string> tables, List<string> views)
        {
            TreeNode tableNode = new TreeNode("表", 2, 3);
            if (null != tables && tables.Count > 0)
            {
                foreach (string tabName in tables)
                {
                    TreeNode tnode = new TreeNode(tabName, 4, 4);
                    tnode.Tag = "T";
                    tnode.ContextMenuStrip = contextMenuStripTable;
                    tableNode.Nodes.Add(tnode);
                }
            }
            databaseNodel.Nodes.Add(tableNode);

            TreeNode viewNode = new TreeNode("视图", 2, 3);
            if (null != views && views.Count > 0)
            {
                foreach (string vwName in views)
                {
                    TreeNode tnode = new TreeNode(vwName, 4, 4);
                    tnode.Tag = "V";
                    tnode.ContextMenuStrip = contextMenuStripTable;
                    viewNode.Nodes.Add(tnode);
                }
            }
            databaseNodel.Nodes.Add(viewNode);
        }


        #endregion



        /// <summary>
        /// 代码生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 生成代码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }


        /// <summary>
        /// 刷新数据库表和视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 刷新ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            TreeNode node = tview.SelectedNode;

            node.Nodes.Clear();

            Entity.Connection conModel = list.Find(delegate (Entity.Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });

            DB db = null;
            db = new DB(conModel.DbType, conModel.ConnectionString);
            DBInfo dbInfo = db.Info;
            gettables(node, dbInfo.TableNames, dbInfo.GetAllViews());

        }


        /// <summary>
        /// 批量生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 批量生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = tview.SelectedNode;

            Entity.Connection conModel = list.Find(delegate (Entity.Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });

            BatchForm bf = new BatchForm();
            bf.DatabaseName = node.Text;
            bf.ConnectionModel = conModel;
            bf.ShowDialog();

        }

        private void 插入测试数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TreeNode node = tview.SelectedNode;
            //Entity.Connection conModel = list.Find(delegate (Entity.Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });

            //InsertForm insertForm = new InsertForm();
            //insertForm.DatabaseName = node.Text;
            //insertForm.ConnectionModel = conModel;
            //insertForm.ShowDialog();


            if (null != newcontentForm)
            {
                Entity.Connection conModel = list.Find(delegate (Entity.Connection con) { return con.ID.ToString().Equals(tview.SelectedNode.Parent.Parent.Parent.Tag.ToString()); });
                conModel.ConnectionString = tview.SelectedNode.Parent.Parent.Tag.ToString();
                InsertForm contentFm = new InsertForm();
                newcontentForm(contentFm, conModel, tview.SelectedNode.Text, tview.SelectedNode.Tag.ToString().Equals("V"));
            }
        }

        private void 表别名设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = tview.SelectedNode;
            Entity.Connection conModel = list.Find(delegate (Entity.Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
            TableDisplayForm form = new TableDisplayForm();
            form.ConnectionModel = conModel;
            form.ShowDialog();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != newcontentForm)
            {
                Entity.Connection conModel = list.Find(delegate (Entity.Connection con) { return con.ID.ToString().Equals(tview.SelectedNode.Parent.Parent.Parent.Tag.ToString()); });
                conModel.ConnectionString = tview.SelectedNode.Parent.Parent.Tag.ToString();
                ContentForm contentFm = new ContentForm();
                newcontentForm(contentFm, conModel, tview.SelectedNode.Text, tview.SelectedNode.Tag.ToString().Equals("V"));
            }
        }

        private void 导出CHM文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = tview.SelectedNode;
            Entity.Connection conModel = list.Find(delegate (Entity.Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
            CHMForm cmf = new CHMForm();
            cmf.DatabaseName = node.Text;
            cmf.ConnectionModel = conModel;
            cmf.ShowDialog();
        }

        private void 导入PDM文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = tview.SelectedNode;
            Entity.Connection conModel = list.Find(delegate (Entity.Connection con) { return con.ID.ToString().Equals(node.Parent.Tag.ToString()); });
            ImportPDMForm cmf = new ImportPDMForm();
            cmf.DatabaseName = node.Text;
            cmf.ConnectionModel = conModel;
            cmf.ShowDialog();
        }
    }
}
