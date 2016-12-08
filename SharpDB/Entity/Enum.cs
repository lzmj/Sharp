
namespace SharpDB
{
    /// <summary>
    /// ConnectionStrings 配置数据库类型
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// Sql Sever 数据库
        /// </summary>
        MsSql,
        /// <summary>
        /// MySql 数据库
        /// </summary>
        MySql,
        /// <summary>
        /// Oracle 微软
        /// </summary>
        Oracle,
        /// <summary>
        /// 破解版DDTek Oracle客户端
        /// </summary>
        OracleDDTek,
        /// <summary>
        /// ODP.NET Managed 
        /// </summary>
        OracleManaged,
        
        /// <summary>
        /// Sqlite 数据库（兼容x64/x86）
        /// </summary>
        Sqlite
    }
}
