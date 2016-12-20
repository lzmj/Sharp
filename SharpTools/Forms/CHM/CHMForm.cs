using SharpDB.Entity;
using SharpTools.CHM;
using SharpTools.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SharpTools
{
    public partial class CHMForm : Form
    {
        public CHMForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            //为KeyDown能应用到所有控件上 注册 KeyDown 事件 
            foreach (Control control in this.Controls)
            {
                control.KeyDown += control_KeyDown;
            }
        }

        public void control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private string databaseName;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        private Entity.Connection connectionModel;
        public Entity.Connection ConnectionModel
        {
            get { return connectionModel; }
            set { connectionModel = value; }
        }
        private void btnMakeCHM_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(CHMCompile);
            thread.IsBackground = true;
            thread.Start();
        }


        string defaultHtml = "数据库表目录.html";
        string chm_html_path = string.Empty;
        string indexHtmlpath = string.Empty;
        /// <summary>
        /// 生成表结构 html文件
        /// </summary>
        private void Builder()
        {
            //使用目录
            string useDir = string.Empty;
            //if (CkRetainHtml.Checked)//保留html文件
            //{
            useDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //}
            //else
            //{
            //    useDir = Path.GetTempPath();
            //}

            
            chm_html_path = Path.Combine(useDir, txtCHM_Name.Text);
            string tempPath = chm_html_path;
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            else
            {
                Directory.Delete(tempPath, true);
                Directory.CreateDirectory(tempPath);
            }

            indexHtmlpath = Path.Combine(tempPath, defaultHtml);

            DB db = new DB(connectionModel.DbType, connectionModel.ConnectionString);
            DBInfo dbInfo = db.Info;

            Dictionary<string, string> dict_tabComment = dbInfo.GetAllTableComment();

            ChmHtmlHelper.CreateDirHtml("数据库表目录", dict_tabComment, indexHtmlpath);


            //创建表结构的html
            tempPath = Path.Combine(tempPath, "表结构");
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            else
            {
                Directory.Delete(tempPath, true);
                Directory.CreateDirectory(tempPath);
            }

            List<TableInfo> lstTableInfo = dbInfo.GetAllTableInfos();
            ChmHtmlHelper.CreateHtml(lstTableInfo, tempPath);
        }
        private void CHMCompile()
        {
            if (string.IsNullOrWhiteSpace(chm_html_path))
            {
                Builder();
            }
            txtCHM_Name.Enabled = false;
            //CkRetainHtml.Enabled = false;
            btnMakeCHM.Enabled = false;
            btnMakeCHM.Text = "导出中...";
            try
            {
                //编译CHM文档
                ChmHelp c3 = new ChmHelp();
                c3.DefaultPage = defaultHtml;
                c3.Title = txtCHM_Name.Text;
                c3.ChmFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), c3.Title + ".chm");
                c3.SourcePath = chm_html_path;
                //string result = c3.Compile(CkRetainHtml.Checked);
                string result = c3.Compile(true);
                if (string.IsNullOrWhiteSpace(result))
                {
                    MessageBox.Show("导出CHM成功！");
                    this.Close();
                    Process.Start(c3.ChmFileName);
                }
                else
                {
                    MessageBox.Show(result);
                    this.Close();
                    //if (CkRetainHtml.Checked)//保留html文件
                    {
                        Process.Start(indexHtmlpath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出CHM失败！" + ex.Message);
            }
            txtCHM_Name.Enabled = true;
            //CkRetainHtml.Enabled = true;
            btnMakeCHM.Enabled = true;
            btnMakeCHM.Text = "导出";
            
        }

        private void CHMForm_Load(object sender, EventArgs e)
        {
            CkRetainHtml.Visible = false;
            txtCHM_Name.Text = connectionModel.Database.Replace("/",".") + "表结构信息";
            btnMakeCHM.Focus();
        }

        private void btnBuilder_Click(object sender, EventArgs e)
        {
            Builder();
            if (!string.IsNullOrWhiteSpace(chm_html_path))
            {
                MessageBox.Show("表结构html生成成功！");
                Process.Start(chm_html_path);
            }
        }
    }
}
