using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using SharpTools.Common;
using SharpTools.Forms;
using SharpTools.DbDAL;
using SharpDB.Entity;
using ICSharpCode.TextEditor.Document;

namespace SharpTools
{
    public partial class ContentForm : DockContentForm
    {
        public ContentForm()
        {
            InitializeComponent();

            txtSqlContent.ShowEOLMarkers = false;
            txtSqlContent.ShowHRuler = false;
            txtSqlContent.ShowInvalidLines = false;
            txtSqlContent.ShowMatchingBracket = true;
            txtSqlContent.ShowSpaces = false;
            txtSqlContent.ShowTabs = false;
            txtSqlContent.ShowVRuler = false;
            txtSqlContent.AllowCaretBeyondEOL = false;
            txtSqlContent.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("TSQL");
            txtSqlContent.Encoding = Encoding.GetEncoding("GB2312");
        }

        private void ContentForm_Load(object sender, EventArgs e)
        {
            InitGrid();
            InitCklDbTypes();
        }

        private void InitCklDbTypes()
        {
            List<string> lstEnums = ConnectionModel.DbType.GetListEnums();
            ckl_DBTypes.Items.Add(ConnectionModel.DbType);
            //lstEnums.Remove(ConnectionModel.DbType.ToString());
            //ckl_DBTypes.Items.AddRange(lstEnums.ToArray());
            ckl_DBTypes.SelectedIndex = 0;
            initCheck(0);
        }

        private DB db = null;
        private DBInfo dbInfo = null;
        private List<SharpDB.Entity.ColumnInfo> lstCol = null;
        private string table_Comment = null;

        internal void InitGrid()
        {
            db = new DB(ConnectionModel.DbType, ConnectionModel.ConnectionString);
            dbInfo = db.Info;
            lstCol = dbInfo.GetAllColumnInfo(this.TableName);
            table_Comment = dbInfo.GetTableComment(TableName);
            gridColumns.DataSource = lstCol;            
        }

        //private void BtnUpdateFieldDesc_Click(object sender, EventArgs e)
        //{
        //    var dict = dbInfo.GetColumnsComment(TableName);
        //    int upInt = 0;
        //    int rowCount = gridColumns.Rows.Count;
        //    for (int j = 0; j < rowCount; j++)
        //    {
        //        object columnName = gridColumns.Rows[j].Cells["ColumnName"].Value ?? string.Empty;
        //        object deText = gridColumns.Rows[j].Cells["DeText"].Value ?? string.Empty;
        //        if (dict[columnName.ToString()] != deText.ToString())
        //        {
        //            dbInfo.UpsertColumnComment(TableName, columnName.ToString(), deText.ToString());
        //            upInt++;
        //        }
        //    }
        //    if (upInt > 0)
        //    {
        //        MessageBox.Show("列说明更新成功！");
        //        InitGrid();
        //    }
        //}

        //private void BtnBrowPdm_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog opfiledia = new OpenFileDialog();
        //    opfiledia.Filter = "";

        //    var openFileDialog = new OpenFileDialog
        //    {
        //        InitialDirectory = null,
        //        Filter = "(*.pdm)|*.pdm",
        //        RestoreDirectory = true
        //    };
        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        txtPDM.Text = openFileDialog.FileName;

        //        UpdateGrid(txtPDM.Text);
        //    }
        //}

        //private void UpdateGrid(string pdmPath)
        //{
        //    var pdmReader = new PDM.PdmReader();
        //    var lstTbl = pdmReader.ReadFromFile(pdmPath).Tables;
        //    PdmModels.TableInfo tabInfo = lstTbl.FirstOrDefault(t => t.Code.Equals(TableName, StringComparison.OrdinalIgnoreCase));
        //    if (tabInfo != null)
        //    {
        //        int rowCount = gridColumns.Rows.Count;
        //        for (int j = 0; j < rowCount; j++)
        //        {
        //            object colName = gridColumns.Rows[j].Cells["columnName"].Value ?? string.Empty;
        //            PdmModels.ColumnInfo colInfo = tabInfo.Columns.FirstOrDefault(t => t.Code.Equals(colName.ToString(), StringComparison.OrdinalIgnoreCase));
        //            if (colInfo != null && string.IsNullOrEmpty(colInfo.Description))
        //            {
        //                gridColumns.Rows[j].Cells["detext"].Value = colInfo.Name ?? colInfo.Description ?? string.Empty;
        //            }
        //        }
        //    }
        //}

        private void btnSetColDisplay_Click(object sender, EventArgs e)
        {
            TableColumnDisplayForm form = new TableColumnDisplayForm();
            form.TableName = TableName;
            form.ConnectionModel = ConnectionModel;
            form.ContentThisForm = this;
            form.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            InitGrid();
        }

        private void ckl_DBTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            initCheck(ckl_DBTypes.SelectedIndex);
        }

        void initCheck(int sel_Index)
        {
            #region 当前选中则勾选，其他取消勾选
            if (ckl_DBTypes.Items.Count > 0)
            {
                ckl_DBTypes.SetItemCheckState(sel_Index, CheckState.Checked);
                ckl_DBTypes.SetItemChecked(sel_Index, true);
                for (int i = 0; i < ckl_DBTypes.Items.Count; i++)
                {
                    if (i != sel_Index)
                    {
                        ckl_DBTypes.SetItemCheckState(i, CheckState.Unchecked);
                        ckl_DBTypes.SetItemChecked(i, false);
                    }
                }
            }
            #endregion
            TableInfo tabInfo = new TableInfo();
            tabInfo.LstColInfo = lstCol;
            tabInfo.TableName = TableName;
            tabInfo.TabComment = table_Comment;
            DataBaseType tgtDBType = ckl_DBTypes.SelectedItem.ToString().ConvertTo<DataBaseType>();
            string sql_script = DBScript.GetTableScript(ConnectionModel.DbType, tabInfo, tgtDBType);
            txtSqlContent.Text = sql_script;
        }
    }
}

