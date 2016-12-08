using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SharpTools.Common;

namespace SharpTools.DbSelect
{
    public partial class DBOracle : Form
    {
        public DBOracle()
        {
            InitializeComponent();
        }

        string connstr = string.Empty;
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (chbConnectString.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtConnectString.Text))
                {
                    MessageBox.Show("请填写连接字符串!");
                    return;
                }
                connstr = txtConnectString.Text;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(cbbServer.Text))
                {
                    MessageBox.Show("请填写服务!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUserName.Text))
                {
                    MessageBox.Show("请填写用户名!");
                    return;
                }

                connstr = "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Unicode=True";
                connstr = string.Format(connstr, cbbServer.Text, txtUserName.Text, txtPassword.Text);
            }

            try
            {
                DB db = new DB(DataBaseType.Oracle, connstr);
                db.QueryTable("Select 1 From dual");
                MessageBox.Show("连接成功!");
                isConnection = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败!\n\r" + ex.Message);
                isConnection = false;
            }

        }

        bool isConnection = false;


        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);

            if (!isConnection)
            {
                return;
            }


            Entity.Connection connectionModel = new Entity.Connection();
            connectionModel.Database = cbbServer.Text;
            connectionModel.ID = Guid.NewGuid();
            connectionModel.Name = cbbServer.Text+"(Oracle)";
            connectionModel.ConnectionString = connstr;
            connectionModel.DbType = DataBaseType.Oracle;
            Utils.AddConnection(connectionModel);

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbConnectString_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConnectString.Checked)
            {
                panelServer.Enabled = false;
                txtConnectString.Enabled = true;
            }
            else
            {
                panelServer.Enabled = true;
                txtConnectString.Enabled = false;
            }

        }
    }
}
