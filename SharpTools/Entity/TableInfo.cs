using SharpDB.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SharpDB.Entity
{
    public class TableInfo
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string TabComment { get; set; }

        /// <summary>
        /// 该表包含的所有列
        /// </summary>
        public List<ColumnInfo> LstColInfo { get; set; }
    }
}
