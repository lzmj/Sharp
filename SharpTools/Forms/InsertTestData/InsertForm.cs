using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDB.Entity;
using SharpTools.Forms.InsertTestData;

namespace SharpTools
{
    public partial class InsertForm : DockContentForm
    {
        public InsertForm()
        {
            InitializeComponent();
        }


        public class KV
        {
            public string Key { get; set; }

            public string Val { get; set; }


            public KV() { }

            public KV(string key,string val)
            {
                this.Key = key;
                this.Val = val;
            }
        }

        private List<ColumnInfo> lstCol = new List<ColumnInfo>();


        private void InsertForm_Load(object sender, EventArgs e)
        {
            DB db = new DB(ConnectionModel.DbType, ConnectionModel.ConnectionString);
            var lstCol = db.Info.GetAllColumnInfo(this.TableName);

            List<KV> lstKV = new List<KV>();
            
                int j = 0;
            foreach (ColumnInfo colInfo in lstCol)
            {
                string btnName = "btn" + colInfo.ColumnName;

                lstKV.Add(new KV(
                        colInfo.ColumnName + "\t" + colInfo.DeText,
                        btnName
                    ));

                CKlst.Items.Add(colInfo.ColumnName + "\t" + colInfo.DeText);

                Button btn = new Button();
                btn.Name = btnName;
                btn.Tag = colInfo;
                btn.Text = "设置";
                btn.Click += Btn_Click;
                btn.Font = new Font(btn.Font.FontFamily, 9, FontStyle.Regular);
                int y = CKlst.Location.Y;

                btn.Location = new Point(70, y + (j * btn.Height));

                gpBoxSet.Controls.Add(btn);

                j++;
            }

            CKlst.DataSource = lstKV;
            CKlst.ValueMember = "Val";
            CKlst.DisplayMember = "Key";
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            InsertSetForm setForm = new InsertSetForm();
            DialogResult disRes = setForm.ShowDialog();
            if (disRes == DialogResult.OK)
            {

            }
        }

        private void CKlst_SelectedIndexChanged(object sender, EventArgs e)
        {

            #region 当前选中则勾选，其他取消勾选
            if (CKlst.Items.Count > 0)
            {
                CKlst.SetItemCheckState(CKlst.SelectedIndex, CheckState.Checked);
                CKlst.SetItemChecked(CKlst.SelectedIndex, true);
                for (int i = 0; i < CKlst.Items.Count; i++)
                {
                    if (i != CKlst.SelectedIndex)
                    {
                        CKlst.SetItemCheckState(i, CheckState.Unchecked);
                        CKlst.SetItemChecked(i, false);
                    }
                }
            }
            #endregion


            var controls = gpBoxSet.Controls;
            string btnName = (CKlst.SelectedItem as KV).Val;
            foreach (Control control in controls)
            {
                Font ft = control.Font;
                if (btnName == control.Name)
                {
                    control.Font = new Font(ft.FontFamily, ft.Size, FontStyle.Bold);
                    control.Focus();
                }
                else
                {
                    control.Font = new Font(ft.FontFamily, ft.Size, FontStyle.Regular);
                }
            }
        }
    }
}
