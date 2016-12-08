using System;
using System.Collections.Generic;
using System.Text;

namespace SharpTools.Entity
{
    public class Connection
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Database { get; set; }
        public DataBaseType DbType { get; set; }
        public string ConnectionString { get; set; }
    }
}
