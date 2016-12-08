using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Sunisoft.IrisSkin;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;
using System.Data.SqlClient;

namespace SharpTools
{
    public partial class MainForm : Form
    {
        SkinEngine skin = new SkinEngine();
        public MainForm()
        {
            InitializeComponent();
        }

        #region 打开数据库视图

        private void MainForm_Load(object sender, EventArgs e)
        {
            var path = Environment.CurrentDirectory + "\\Skin\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string[] ssks = Directory.GetFiles(path, "*.ssk");
            //List<string> lstFileNames = ssks.Select(ssk => Path.GetFileNameWithoutExtension(ssk)).ToList();
            //if (lstFileNames.Count > 0)
            //{
            //    skin.SkinFile = path + lstFileNames[0] + ".ssk";
            //    skin.Active = true;
            //}

            ToolStripItemCollection tspc = 换肤ToolStripMenuItem.DropDownItems;
            tspc.Add("无", null, ToolStripMenuItem_Click);
            foreach (string ssk in ssks)
            {
                string fileName = Path.GetFileNameWithoutExtension(ssk);
                if (!tspc.ContainsKey(fileName))
                {
                    tspc.Add(fileName, null, ToolStripMenuItem_Click);
                }
            }

            showSJKST();
            ExpandLevel(lp.tview.Nodes, 2);
        }

        /// <summary>
        /// 设置TreeView 要展开节点的深度
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="index"></param>
        private void ExpandLevel(TreeNodeCollection nodes, int index)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Level <= index)
                {
                    node.Expand();
                    ExpandLevel(node.Nodes, index);
                }
            }
        }

        LeftPanel lp = new LeftPanel();

        /// <summary>
        /// 
        /// </summary>
        private void showSJKST()
        {
            lp.newcontentForm += new LeftPanel.NewContentForm(lp_newcontentForm);
            lp.Show(dpleft);
            lp.DockTo(dpleft, DockStyle.Left);

        }

        /// <summary>
        /// 创建生成
        /// </summary>
        /// <param name="conModel"></param>
        /// <param name="tableName"></param>
        void lp_newcontentForm(DockContentForm dkContentFm, Entity.Connection conModel, string tableName, bool isView)
        {
            dkContentFm.Text = "(" + new DB(conModel.DbType, conModel.ConnectionString).DBaseType + ")" + tableName;
            dkContentFm.TableName = tableName;
            dkContentFm.IsView = isView;
            dkContentFm.ConnectionModel = conModel;
            dkContentFm.Show(dpleft);
        }

        /// <summary>
        /// 数据库视图打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 数据库视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lp.Show();
        }

        #endregion

        #region 关闭程序

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Application.Exit();
        }

        #endregion

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }

        private void 日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogShow ls = new LogShow();
            ls.ShowDialog();
        }
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender.ToString() == "无")
            {
                skin.SkinFile = null;
                skin.Active = false;
            }
            var path = Environment.CurrentDirectory + "\\Skin\\";
            string sskPath = path + sender + ".ssk";
            if (File.Exists(sskPath))
            {
                skin.SkinFile = sskPath;
                skin.Active = true;
            }
        }

        private void 默认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skin.Active = false;
        }

        private void 新建数据库连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lp.toolStripButton1_Click(sender, e);
        }
    }
}
