using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpTools
{
    public class DockContentForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string content;


        private Entity.Connection connectionModel;

        public Entity.Connection ConnectionModel
        {
            set { connectionModel = value; }
            get { return connectionModel; }
        }

        private string tableName;
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        private bool isView = false;
        public bool IsView
        {
            get { return isView; }
            set { isView = value; }
        }

        private string databaseName;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }
    }
}
