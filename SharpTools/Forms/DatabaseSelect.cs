using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SharpTools
{
    public partial class DatabaseSelect : Form
    {
        public DatabaseSelect()
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


        public static DataBaseType? DatabaseType = null;


        /// <summary>
        /// 选择数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            if (rbSqlServer.Checked)
            {
                DatabaseType = DataBaseType.MsSql;
            }
            else if (rbOracle.Checked)
            {
                DatabaseType = DataBaseType.Oracle;
            }
            else if (rbSQLite.Checked)
            {
                DatabaseType = DataBaseType.Sqlite;
            }
            else if (rbMySql.Checked)
            {
                DatabaseType = DataBaseType.MySql;
            }
            else if (rbMariaDB.Checked)
            {
                DatabaseType = DataBaseType.MySql;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
