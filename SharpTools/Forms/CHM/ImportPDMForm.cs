using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpTools.Forms
{
    public partial class ImportPDMForm : Form
    {
        public ImportPDMForm()
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

        static HashSet<string> lstPhs = new HashSet<string>();
        static IList<PdmModels.TableInfo> lstTabs = new List<PdmModels.TableInfo>();

        private void BtnBrow_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                FileName = "",
                Filter = "(*.pdm)|*.pdm",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtMulItem.Text = string.Join("\r\n", openFileDialog.FileNames);
            }
        }

        private void BtnUpdateDisplayName_Click(object sender, EventArgs e)
        {
            string[] paths = txtMulItem.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ph in paths)
            {
                string extName = Path.GetExtension(ph).ToLower();
                if (File.Exists(ph) && extName == ".pdm")
                {
                    lstPhs.Add(ph);
                }
            }
            txtMulItem.Text = string.Join("\r\n", lstPhs);
            lstTabs = GetTables(lstPhs.ToArray());
            DB db = new DB(ConnectionModel.DbType, ConnectionModel.ConnectionString);
            DBInfo dbInfo = db.Info;
            foreach (var tab in lstTabs)
            {
                string tab_Comment = tab.Name;
                if (!string.IsNullOrWhiteSpace(tab_Comment) 
                    && !tab.Code.Equals(tab_Comment, StringComparison.OrdinalIgnoreCase))
                {
                    try 
                    {
                        dbInfo.UpsertTableComment(tab.Code, tab_Comment);
                    }
                    catch (Exception ex)
                    {
                        //若 表 不存在时执行报异常，则继续执行
                    }
                }
                var lstCols = tab.Columns;
                foreach (var col in lstCols)
                {
                    string col_Comment = col.Name;
                    if (!string.IsNullOrWhiteSpace(col_Comment)
                        && !col.Code.Equals(col_Comment, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            dbInfo.UpsertColumnComment(tab.Code, col.Code, col_Comment);
                        }
                        catch (Exception ex)
                        {
                            //若 列 不存在时执行报异常，则继续执行
                        }
                    }
                }
            }
            MessageBox.Show("更新表列别名完成！");
            this.Close();
        }
        static IList<PdmModels.TableInfo> GetTables(params string[] pdmPaths)
        {
            List<PdmModels.TableInfo> lstTables = new List<PdmModels.TableInfo>();
            var pdmReader = new PDM.PdmReader();
            foreach (string path in pdmPaths)
            {
                if (File.Exists(path))
                {
                    var models = pdmReader.ReadFromFile(path);
                    lstTables.AddRange(models.Tables);
                }
            }
            lstTables = lstTables.OrderBy(t => t.Code).ToList();
            return lstTables;
        }
    }
}
