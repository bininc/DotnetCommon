using System.Linq;
using Common;
using DBHelper.Common;

namespace DBHelper {
    public class DbConnections {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DataBaseType DbType {
            get {
                string dbTypeStr = ConfigurtaionHelper.Instance.GetAppSettings<DbConnSettings> ("DataBase", "appsettings.json").MainDbTypeStr?.ToLower ();
                switch (dbTypeStr) {
                    case "sqlserver":
                        return DataBaseType.SqlServer;
                    case "mysql":
                        return DataBaseType.MySql;
                    case "oracle":
                        return DataBaseType.Oracle;
                    case "sqlite":
                        return DataBaseType.Sqlite;
                    default:
                        return DataBaseType.Oracle;
                }
            }
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetDbConnectionString () {
            string dbConnStr = ConfigurtaionHelper.Instance.GetAppSettings<DbConnSettings> ("DataBase", "appsettings.json").MainDb?.ConnStr;
            return dbConnStr;
        }
    }

    /// <summary>
    /// 数据库连接字符串配置
    /// </summary>
    public class DbConnSetting {
        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <value></value>
        public string DbTypeStr { get; set; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        /// <value></value>
        public string ConnStr { get; set; }
        /// <summary>
        /// 是否主数据库
        /// </summary>
        /// <value></value>
        public bool IsMain { get; set; }
    }

    public class DbConnSettings {
        public DbConnSetting[] ConnectionStrings { get; set; }
        public DbConnSetting MainDb {
            get {
                if (ConnectionStrings == null || ConnectionStrings.Length == 0) return null;
                DbConnSetting fcs = ConnectionStrings.FirstOrDefault (c => c.IsMain);
                if (fcs == null)
                    fcs = ConnectionStrings.First ();
                return fcs;
            }
        }
        public string MainDbTypeStr {
            get {
                return MainDb?.DbTypeStr;
            }
        }
    }

    /// <summary>
    /// Redis数据库连接字符串
    /// </summary>
    public class RedisConnSetting : DbConnSetting {
        /// <summary>
        /// Redis键前缀
        /// </summary>
        /// <value></value>
        public string Key { get; set; }
    }
}