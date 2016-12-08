using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SharpTools.Common;
using SharpTools.Entity;

namespace SharpTools.DbSelect
{
    public partial class DBMySql : Form
    {
        public DBMySql()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 是否点击测试连接成功
        /// </summary>
        bool isTestLink = false;

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(cbbServer.Text))
            {
                MessageBox.Show("服务器不能为空!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("登陆名不能为空!");
                return;
            }

            try
            {

                initAllDataBase();
                isTestLink = true;
                MessageBox.Show("连接成功!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败!\n\r" + ex.Message);
                cbbDatabase.Enabled = false;

            }
        }

        private void initAllDataBase()
        {
            string connStr = String.Format("server={0};user id={1}; password={2}; Port={3};database=; pooling=false", cbbServer.Text, txtUserName.Text, txtPassword.Text, txtport.Text);

            DB db = new DB(DataBaseType.MySql, connStr);
            DataTable DBNameTable = db.QueryTable("SHOW DATABASES");
            cbbDatabase.Items.Clear();
            cbbDatabase.Items.Add("全部");
            foreach (DataRow dr in DBNameTable.Rows)
            {
                cbbDatabase.Items.Add(dr[0].ToString());
            }
            cbbDatabase.Enabled = true;
            cbbDatabase.SelectedIndex = 0;
        }


        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(cbbServer.Text))
            {
                MessageBox.Show("服务器不能为空!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("登陆名不能为空!");
                return;
            }

            if (!isTestLink)
            {
                try
                {

                    initAllDataBase();
                    isTestLink = true;
                    cbbDatabase.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接失败!\n\r" + ex.Message);
                    cbbDatabase.Enabled = false;
                    return;
                }
            }

            if (cbbDatabase.SelectedIndex == 0)
            {
                MessageBox.Show("请选择数据库!");
                return;
            }


            string connStr = String.Format("server={0};user id={1}; password={2}; Port={3};database=; pooling=false", cbbServer.Text, txtUserName.Text, txtPassword.Text, txtport.Text);

            if (cbbDatabase.SelectedIndex != 0)
            {
                connStr = String.Format("server={0};user id={1}; password={2}; Port={3};database={4}; pooling=false", cbbServer.Text, txtUserName.Text, txtPassword.Text, txtport.Text, cbbDatabase.Text);
            }


            Connection connectionModel = new Connection();
            connectionModel.ID = Guid.NewGuid();
            connectionModel.Database = cbbDatabase.SelectedIndex == 0 ? "all" : cbbDatabase.Text;
            connectionModel.Name = cbbServer.Text + "(MySql)[" + connectionModel.Database + "]";
            connectionModel.ConnectionString = connStr;
            connectionModel.DbType = DataBaseType.MySql;



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

        private void DBMySql_Load(object sender, EventArgs e)
        {
            cbbDatabase.SelectedIndex = 0;
        }
    }
}
