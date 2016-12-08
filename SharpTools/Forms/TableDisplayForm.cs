using SharpTools.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpTools.Forms
{
    public partial class TableDisplayForm : Form
    {
        public TableDisplayForm()
        {
            InitializeComponent();

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

        public Entity.Connection ConnectionModel
        {
            get; set;
        }
        private void TableDisplayForm_Load(object sender, EventArgs e)
        {
            GV_TabDisplay.AllowUserToAddRows = false;

            DB db = new DB(ConnectionModel.DbType, ConnectionModel.ConnectionString);

            GV_TabDisplay.DataSource = GetData(db.Info.GetAllTableComment(), "表名", "别名");

            GV_TabDisplay.Columns[0].ReadOnly = true;

            GV_TabDisplay.Columns[0].Width = 150;
            GV_TabDisplay.Columns[1].Width = 150;

        }

        private DataTable GetData(IDictionary<string, string> dict, string textKey, string textValue)
        {
            DataTable data = Utils.CreateDataTable(textKey, textValue);
            foreach (var item in dict)
            {
                DataRow dr = data.NewRow();
                dr[textKey] = item.Key;
                dr[textValue] = item.Value;
                data.Rows.Add(dr);
            }
            return data;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DB db = new DB(ConnectionModel.DbType, ConnectionModel.ConnectionString);
            DBInfo dbinfo = db.Info;
            int upInt = 0;
            int rowCount = GV_TabDisplay.Rows.Count;
            var dict = dbinfo.GetAllTableComment();
            for (int j = 0; j < rowCount; j++)
            {
                object tableName = GV_TabDisplay.Rows[j].Cells["表名"].Value ?? string.Empty;
                object displayText = GV_TabDisplay.Rows[j].Cells["别名"].Value ?? string.Empty;
                if (dict[tableName.ToString()] != displayText.ToString()) //与原来的不等则更新
                {
                    dbinfo.UpsertTableComment(tableName.ToString(), displayText.ToString());
                    upInt++;
                }
            }
            if (upInt > 0)
            {
                MessageBox.Show("表别名更新成功！");
                this.Close();
            }
        }
    }
}

