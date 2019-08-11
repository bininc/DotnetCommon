namespace DBHelper.Common {
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DataBaseType {
        /// <summary>
        /// SQLServer数据库
        /// </summary>
        SqlServer,
        /// <summary>
        /// MySql数据库
        /// </summary>
        MySql,
        /// <summary>
        /// Oracle数据库
        /// </summary>
        Oracle,
        /// <summary>
        /// Sqlite数据库
        /// </summary>
        Sqlite,
        /// <summary>
        /// 根据配置文件DbType自动选择
        /// </summary>
        Auto
    }
}