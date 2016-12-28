using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDB.Entity;

namespace SharpTools
{
    public partial class InsertForm : DockContentForm
    {
        public InsertForm()
        {
            InitializeComponent();
        }

        private List<ColumnInfo> lstCol = new List<ColumnInfo>();
        private void InsertForm_Load(object sender, EventArgs e)
        {
            DB db = new DB(ConnectionModel.DbType, ConnectionModel.ConnectionString);
            var lstCol = db.Info.GetAllColumnInfo(this.TableName);
            foreach (ColumnInfo colInfo in lstCol)
            {
                CKlst.Items.Add(colInfo.ColumnName + "\t" + colInfo.DeText);
            }
        }

        private void CKlst_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
