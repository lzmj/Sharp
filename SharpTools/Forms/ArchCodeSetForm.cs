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
    public partial class ArchCodeSetForm : Form
    {
        public ArchCodeSetForm()
        {
            InitializeComponent();
        }

        public DataTable columnsdt = null;

        public IniFileHelp ini = new IniFileHelp();

        public ArchType codeType;
        private void ArchCodeSetForm_Load(object sender, EventArgs e)
        {
            List<string> lstEnums = ArchType.简单三层.GetListEnums();
            tplComboBox.DataSource = lstEnums;
            //txtClassName.Text = TableName;
        }
        
        private void InitConfig()
        {
            //txtClassName.Text = TableName.Trim().Replace(' ', '_');
            //txtProjName.Text = ini.GetString(DatabaseName, "ProjName", string.Empty);
        }


        /// <summary>
        /// 代码生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuilder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProjName.Text))
            {
                MessageBox.Show("项目名称不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                txtProjName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtClassName.Text))
            {
                MessageBox.Show("类名不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                txtClassName.Focus();
                return;
            }

            if (Ckd_Lbx.CheckedItems.Count <= 0)
            {
                MessageBox.Show("代码层至少为1个!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
                Ckd_Lbx.Focus();
                return;
            }

            //List<Entity.ColumnInfo> columns = Utils.GetColumnInfos(columnsdt);

            //foreach (Entity.ColumnInfo col in columns)
            //{

            //    col.IsPK = false;

            //    //foreach (object o in cbPrimarykey.Items)
            //    //{
            //    //    if (col.ColumnName.Equals(o.ToString()))
            //    //    {
            //    //        col.IsPK = true;
            //    //        break;
            //    //    }
            //    //}
            //}


            //var lstSelItem = Ckd_Lbx.CheckedItems;

            //foreach (var selItem in lstSelItem)
            //{
            //    tabControl1.TabPages.Add(selItem.ToString(), selItem.ToString(), 1);
            //}


            ////EntityBuilder builder = new EntityBuilder(TableName, txtnamespace.Text, txtClassName.Text, columns, IsView, cbToupperFrstword.Checked, ConnectionModel.DbType);

            ////txtContent.Text = builder.Builder(tplContent.Text);

            //tabControl1.SelectedIndex = 1;






        }

        private void cbToupperFrstword_CheckedChanged(object sender, EventArgs e)
        {
            //txtClassName.Text = TableName;
            //if (cbToupperFrstword.Checked)
            //{
            //    txtClassName.Text = txtClassName.Text.ToUpperFirstword();
            //}
        }


        private void tplComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ckd_Lbx.Items.Clear();

            string codeType = tplComboBox.SelectedValue.ToString();

            if (codeType == ArchType.简单三层.ToString())
            {
                Ckd_Lbx.Items.Add("Model");
                Ckd_Lbx.Items.Add("BLL");
                Ckd_Lbx.Items.Add("DAL");
            }
            //else if (codeType == CodeType.简单三层.ToString())
            //{
            //    Ckd_Lbx.Items.Add("Model");
            //    Ckd_Lbx.Items.Add("BLL");
            //    Ckd_Lbx.Items.Add("DAL");   
            //    Ckd_Lbx.Items.Add("IBLL");
            //    Ckd_Lbx.Items.Add("IDAL");
            //}
            else if (codeType == ArchType.WCF简单三层.ToString())
            {
                Ckd_Lbx.Items.Add("Model");
                Ckd_Lbx.Items.Add("BLL");
                Ckd_Lbx.Items.Add("DAL");

                Ckd_Lbx.Items.Add("WCF");
            }
            else if (codeType == ArchType.WCF工厂三层.ToString())
            {
                Ckd_Lbx.Items.Add("Model");
                Ckd_Lbx.Items.Add("BLL");
                Ckd_Lbx.Items.Add("DAL");

                Ckd_Lbx.Items.Add("IBLL");
                Ckd_Lbx.Items.Add("IDAL");
                Ckd_Lbx.Items.Add("WCF");
            }

        }

        private void Ckd_Lbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox cklbx = (sender as CheckedListBox);
            if (cklbx != null)
            {
                int index = cklbx.SelectedIndex;

                if (index > -1)
                {
                    CheckState ckState = cklbx.GetItemCheckState(index);
                    CheckState setState = (ckState == CheckState.Checked) ? CheckState.Unchecked : CheckState.Checked;
                    cklbx.SetItemCheckState(index, setState);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as TabControl).SelectedTab.Text == "生成代码")
            {
                btnBuilder_Click(null, null);
            }
        }


    }
}
