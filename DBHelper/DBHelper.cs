using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Common;
using DBHelper.BaseHelper;
using DBHelper.Common;

namespace DBHelper
{
    public abstract class DBHelper : IDBHelper
    {
        protected readonly string _connectionString;
        /// <summary>
        /// 数据库操作类
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public DBHelper(string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
                _connectionString = connectionString;
            else
                _connectionString = DbConnections.GetDbConnectionString();
        }
        /// <summary>
        /// 当前数据库连接字符串
        /// </summary>
        public string ConnectionString => _connectionString;

        public abstract DataBaseType GetCurrentDataBaseType();

        public string GetCurrentConnectionString()
        {
            return _connectionString;
        }

        /// <summary>
        /// 创建DBHelper实例
        /// </summary>
        /// <param name="connectionString">指定连接字符串</param>
        /// <param name="dataBaseType">指定数据库类型</param>
        /// <returns></returns>
        public static IDBHelper GetDBHelper(string connectionString = null, DataBaseType dataBaseType = DataBaseType.Auto)
        {
            IDBHelper helper = null;
            if (dataBaseType == DataBaseType.Auto)
            {
                dataBaseType = DbConnections.DbType;
            }

            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
                    helper = ReflectHelper.GetInstance("SQLHelper.dll", "SQLHelper.SQLHelper", connectionString) as IDBHelper;
                    break;
                case DataBaseType.MySql:
                    helper = ReflectHelper.GetInstance("MySQLHelper.dll", "MySQLHelper.MySQLHelper", connectionString) as IDBHelper;
                    break;
                case DataBaseType.Oracle:
                    helper = ReflectHelper.GetInstance("OracleHelper.dll", "OracleHelper.OracleHelper", connectionString) as IDBHelper;
                    break;
                case DataBaseType.Sqlite:
                    helper = ReflectHelper.GetInstance("SQLiteHelper.dll", "SQLiteHelper.SQLiteHelper", connectionString) as IDBHelper;
                    break;
                default:
                    throw new Exception("未知数据库类型");
            }

            return helper;
        }

        protected void PrepareCommand(DbCommand cmd, DbConnection conn, string cmdText, DbTransaction trans = null, CommandType cmdType = CommandType.Text, params DbParameter[] cmdParameters)
        {
            //Open the connection if required
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //Set up the command
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            //Bind it to the transaction if it exists
            if (trans != null)
                cmd.Transaction = trans;
            else
                cmd.Transaction = null;

            cmd.Parameters.Clear(); //清除Parameter
            // Bind the parameters passed in
            if (cmdParameters != null)
            {
                foreach (DbParameter parm in cmdParameters)
                {
                    if (parm.Value == null && (parm.Direction == ParameterDirection.Input || parm.Direction == ParameterDirection.InputOutput))
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
            }
            PrepareCommandEx(cmd, conn, cmdText, trans, cmdType, cmdParameters);
        }

        protected virtual void PrepareCommandEx(DbCommand cmd, DbConnection conn, string cmdText, DbTransaction trans, CommandType cmdType, params DbParameter[] cmdParameters)
        {

        }

        public abstract DataSet Query(string sqlString, params DbParameter[] dbParameter);
        public DataTable QueryTable(string sqlString, params DbParameter[] dbParameter)
        {
            DataTable dt = new DataTable("dt");
            DataSet ds = Query(sqlString, dbParameter);
            if (ds.IsNotEmpty())
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        public abstract bool TestConnectionString();
        public abstract object QueryScalar(string sqlString, CommandType cmdType = CommandType.Text, params DbParameter[] dbParameter);
        public bool Exists(string sqlString, params DbParameter[] dbParameter)
        {
            object obj = QueryScalar(sqlString, CommandType.Text, dbParameter);
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                bool suc = int.TryParse(obj.ToString(), out cmdresult);
                if (!suc && !string.IsNullOrEmpty(obj.ToString()))
                {
                    cmdresult = 1;
                }
            }
            if (cmdresult == 0)
            {
                //记录不存在
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool Exists(string tableName, Dictionary<string, string> colValues, Dictionary<string, string> excludeVal = null)
        {
            if (string.IsNullOrWhiteSpace(tableName) || colValues == null || colValues.Count < 1) throw new Exception("DBHelper出错，输入参数有误！");

            StringBuilder sb = new StringBuilder();

            foreach (var item in colValues)
            {
                string col = item.Key;
                string val = item.Value;
                if (val == null) val = ",NULL"; //排除空值

                val = val.StartsWith(",") ? val.TrimStart(',') : string.Format("'{0}'", val);
                sb.AppendFormat(" {0}={1} and", col, val);
            }
            if (excludeVal != null && excludeVal.Count > 0)
            {
                foreach (var item in excludeVal)
                {
                    string col = item.Key;
                    string val = item.Value;
                    if (val == null) val = ",NULL"; //排除空值

                    val = val.StartsWith(",") ? val.TrimStart(',') : string.Format("'{0}'", val);
                    sb.AppendFormat(" {0}<>{1} and", col, val);
                }
            }
            sb.Append(" 1=1");
            string sql = string.Format("select count(*) from {0} where {1}", tableName, sb.ToString());
            return Exists(sql);
        }

        public DataRow QueryRow(string sqlString, params DbParameter[] dbParameter)
        {
            DataTable dt = QueryTable(sqlString, dbParameter);
            if (dt.IsNotEmpty())
            {
                return dt.Rows[0];
            }

            return null;
        }

        public DataRow QueryRow(string tableName, string pkName, string pkVal)
        {
            if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(pkName) || string.IsNullOrWhiteSpace(pkVal)) throw new Exception("DBHelper.QueryRow出错，输入参数有误！");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from {0} where {1}='{2}'", tableName, pkName, pkVal);
            DataTable dt = QueryTable(sb.ToString());
            if (dt.IsNotEmpty())
                return dt.Rows[0];
            else
                return null;
        }

        public DataTable QueryTableStruct(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new Exception("DBHelper.QueryTableStruct表名无效");
            string SQLString = string.Format("select * from {0} where 0>1", tableName);
            return QueryTable(SQLString);
        }

        public DataTable QueryPageData(GetDataParams param)
        {
            if (param == null) return null;
            if (string.IsNullOrWhiteSpace(param.Table)) return null;

            string columnsStr;
            //列筛选条件处理
            if (param.Columns == null || param.Columns.Length == 0)
                columnsStr = "*";
            else
                columnsStr = string.Join(",", param.Columns);

            StringBuilder sbJoin = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(param.TableAsName))
                sbJoin.AppendFormat(" {0}", param.TableAsName);

            //join条件处理
            if (param.JoinConditions_Json != null && param.JoinConditions_Json.Any())
                param.JoinConditions.AddRange(param.JoinConditions_Json);
            if (param.JoinConditions.Any())
            {
                foreach (var jc in param.JoinConditions)
                {
                    if (string.IsNullOrWhiteSpace(jc.Table) || string.IsNullOrWhiteSpace(jc.OnCol)) continue;  //忽略无效的连接条件
                    if (string.IsNullOrWhiteSpace(jc.MainTable))
                    {
                        if (!string.IsNullOrWhiteSpace(param.TableAsName))
                            jc.MainTable = param.TableAsName;
                        else
                            jc.MainTable = param.Table;
                    }
                    if (string.IsNullOrWhiteSpace(jc.MainCol)) jc.MainCol = jc.OnCol;
                    sbJoin.AppendFormat(" {0}", DescriptionAttribute.GetText(jc.JoinType));
                    if (string.IsNullOrWhiteSpace(jc.TableAsName))
                        sbJoin.AppendFormat(" {0} on {0}.{1}={2}.{3}", jc.Table, jc.OnCol, jc.MainTable, jc.MainCol);
                    else
                        sbJoin.AppendFormat(" {0} {4} on {4}.{1}={2}.{3}", jc.Table, jc.OnCol, jc.MainTable, jc.MainCol, jc.TableAsName);

                    if (!string.IsNullOrEmpty(jc.AndCondition))
                    {
                        jc.AndCondition = jc.AndCondition.TrimStart();
                        if (jc.AndCondition.StartsWith("and", StringComparison.OrdinalIgnoreCase))
                        {
                            sbJoin.AppendFormat(" {0}", jc.AndCondition);
                        }
                        else
                        {
                            sbJoin.AppendFormat(" and {0}", jc.AndCondition);
                        }
                    }
                }
            }
            param.Table += sbJoin.ToString();

            //Where特殊情况处理
            if (param.Where.ToString().TrimEnd().EndsWith("and", StringComparison.CurrentCultureIgnoreCase)) //包含and字符
                param.Where.Append(" 1=1 ");

            if (!string.IsNullOrWhiteSpace(param.WhereStr))
            {
                if (param.WhereStr.TrimEnd().EndsWith("and", StringComparison.CurrentCultureIgnoreCase))
                    param.WhereStr += " 2=2 ";

                if (param.WhereStr.TrimStart().StartsWith("and", StringComparison.CurrentCultureIgnoreCase))
                    param.Where.AppendFormat(" {0} ", param.WhereStr);
                else
                    param.Where.AppendFormat(" and {0} ", param.WhereStr);
            }

            //DicWhere条件处理
            if (param.DicWhere_Json != null && param.DicWhere_Json.Any())
                foreach (var keyValuePair in param.DicWhere_Json)
                    param.DicWhere.Add(keyValuePair.Key, keyValuePair.Value);

            if (param.DicWhere.Any())
            {
                foreach (var item in param.DicWhere)
                {
                    string col = item.Key;
                    string val = item.Value;
                    if (string.IsNullOrWhiteSpace(col) || string.IsNullOrWhiteSpace(val)) continue;//筛选无效值

                    val = val.StartsWith(",")
                        ? string.Format("{0} {1}", col, val.TrimStart(','))
                        : string.Format("{0}='{1}'", col, val);

                    param.Where.AppendFormat(" and {0} ", val);
                }
            }

            //主键条件附加
            if (!string.IsNullOrWhiteSpace(param.PrimaryKey) && !string.IsNullOrWhiteSpace(param.PrimaryKeyValue))
                param.Where.AppendFormat(" and {0}='{1}'", param.PrimaryKey, param.PrimaryKeyValue);

            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbSqlCount = new StringBuilder();
            sbSql.AppendFormat("select {0} from {1}", columnsStr, param.Table);
            sbSqlCount.AppendFormat("select count(*) from {0}", param.Table);
            if (param.Where.Length > 0)
            {
                sbSql.AppendFormat(" where {0}", param.Where);
                sbSqlCount.AppendFormat(" where {0}", param.Where);
            }

            //GroupBy列字段
            if (param.GroupByColumns != null && param.GroupByColumns.Length > 0)
            {
                sbSql.AppendFormat(" group by {0}", string.Join(",", param.GroupByColumns));
                sbSqlCount.AppendFormat(" group by {0}", string.Join(",", param.GroupByColumns));
                sbSqlCount.Insert(0, "select count(*) from (");
                sbSqlCount.Append(")");
            }

            //排序条件附加
            if (param.OrderByConditons != null && param.OrderByConditons.Length > 0)
            {
                string orderCols = null;
                foreach (OrderByCondition o in param.OrderByConditons)
                {
                    if (string.IsNullOrWhiteSpace(o.Col)) continue;
                    orderCols += string.Format(" {0} {1},", o.Col, o.IsDesc ? "desc" : "asc");
                }
                if (!string.IsNullOrWhiteSpace(orderCols))
                    sbSql.AppendFormat(" order by {0}", orderCols.TrimEnd(','));
            }

            if (param.PageSize != -1 || param.PageIndex != -1)
            {   //分页模式
                if (param.PageSize < 1) param.PageSize = 100;   //错误页码过滤
                if (param.PageIndex < 1) param.PageIndex = 1;
                int count = 0;

                //if (param.Columns.Any(col => new Regex(@"count\s*\(",RegexOptions.IgnoreCase).IsMatch(col)))
                //{
                //    sbSqlCount.Insert(0, "select count(*) from (");
                //    sbSqlCount.Append(")");
                //}

                object objCount = QueryScalar(sbSqlCount.ToString());
                if (objCount != null)
                    count = Convert.ToInt32(objCount);

                param.RowCount = count;
                param.PageCount = (int)Math.Ceiling((double)count / param.PageSize);
                if (param.PageIndex > param.PageCount)
                    param.PageIndex = param.PageCount;

                if (count != 0)
                {
                    int startRow = (param.PageIndex - 1) * param.PageSize;
                    int endRow = param.PageIndex * param.PageSize;
                    string sql = GetPageRowNumSql(sbSql.ToString(), startRow, endRow);

                    DataTable dt = QueryTable(sql);
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            else
            {   //无需分页
                DataTable dt = QueryTable(sbSql.ToString());
                return dt;
            }

        }

        public abstract string GetPageRowNumSql(string dataSql, int startRowNum, int endRowNum);

        public DataTable QueryData(string tableName, Dictionary<string, string> dicWhere, string orderStr = null, int rowLimit = -1)
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new Exception("DBHelper.QueryData，输入参数有误！");

            StringBuilder sbSql = new StringBuilder();
            if (dicWhere != null && dicWhere.Count > 0)
            {
                var where = dicWhere.Where(x => !string.IsNullOrWhiteSpace(x.Key) && !string.IsNullOrWhiteSpace(x.Value));  //筛选无效值
                if (where != null && where.Count() > 0)
                {
                    sbSql.AppendFormat("select * from {0} where ", tableName);
                    int i = 0;
                    foreach (var item in where)
                    {
                        i++;
                        string col = item.Key;
                        string val = item.Value;
                        val = val.StartsWith(",")
                            ? string.Format("{0} {1}", col, val.TrimStart(','))
                            : string.Format("{0}='{1}'", col, val);

                        if (i < where.Count())
                        {
                            sbSql.AppendFormat("{0} and ", val);
                        }
                        else
                        {
                            sbSql.AppendFormat("{0}", val);
                        }
                    }
                }
            }
            else
            {
                sbSql.AppendFormat("select * from {0}", tableName);
            }

            if (!string.IsNullOrWhiteSpace(orderStr))
            {
                sbSql.AppendFormat(" {0} ", orderStr);
            }

            string sql = sbSql.ToString();
            if (rowLimit > 0)
                sql = GetRowLimitSql(sql, rowLimit);

            DataTable ds = QueryTable(sql);
            return ds;
        }

        public abstract string GetRowLimitSql(string dataSql, int rowLimit);
        public abstract int ExecuteNonQuery(CommandInfo cmdInfo);

        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] dbParameter)
        {
            return ExecuteNonQuery(new CommandInfo(cmdText, cmdType, dbParameter));
        }

        public int ExecuteNonQuery(string cmdText, params DbParameter[] dbParameter)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, dbParameter);
        }

        public abstract DbDataReader ExecuteReader(string sqlString, params DbParameter[] dbParameter);
        public abstract int ExecuteSqlTran(CommandInfo cmdInfo);
        public abstract int ExecuteProcedure(string storedProcName, params DbParameter[] parameters);
        public abstract int ExecuteProcedureTran(string storedProcName, params DbParameter[] parameters);
        public abstract int ExecuteSqlsTran(List<CommandInfo> cmdList, int num = 5000);
        public int ExecuteSqlsTran(List<string> sqlStringList, bool failStop = true, int num = 5000)
        {
            if (sqlStringList == null || sqlStringList.Count == 0) return -1;
            List<CommandInfo> cmdList = new List<CommandInfo>();
            foreach (string sql in sqlStringList)
            {
                CommandInfo c = new CommandInfo(sql);
                if (failStop)
                    c.FailRollback = true;
                cmdList.Add(c);
            }
            return ExecuteSqlsTran(cmdList, num);
        }

        public int ExecuteSqlsTran(Dictionary<string, DbParameter[]> SQLStringList, bool failStop = true, int num = 5000)
        {
            if (SQLStringList == null || SQLStringList.Count == 0) return -1;
            List<CommandInfo> cmdList = new List<CommandInfo>();
            foreach (var sql in SQLStringList)
            {
                CommandInfo c = new CommandInfo(sql.Key, sql.Value);
                if (failStop)
                {
                    c.FailRollback = failStop;
                }
                cmdList.Add(c);
            }
            return ExecuteSqlsTran(cmdList, num);
        }

        public string GetNextMaxID(string ColName, string TableName, int startID = 1, bool recycle = true)
        {
            if (string.IsNullOrWhiteSpace(ColName) || string.IsNullOrWhiteSpace(TableName)) return null;
            string strsql = "select max({1})+1 next_pk from {0} WHERE {1}>={2}";
            string sqlFormat = @"SELECT case when {1}<{2} then {2} else {1}+1 end next_pk FROM {0} tb WHERE
                                    NOT EXISTS (SELECT {1} FROM {0} tbcp WHERE tbcp.{1} = tb.{1}+1)
                                    ORDER BY {1}";

            string sql = string.Format(recycle ? sqlFormat : strsql, TableName, ColName, startID);
            object obj = QueryScalar(sql);
            if (obj == null)
            {
                return startID.ToString();
            }
            else
            {
                return obj.ToString();
            }
        }

        public bool AddData(string tableName, Dictionary<string, string> datas, params DbParameter[] sqlParameters)
        {
            if (string.IsNullOrWhiteSpace(tableName) || datas == null || datas.Count <= 0) throw new Exception("[DBHelper.AddData]添加数据出错，输入参数有误！");
            try
            {
                DataTable dt = QueryTableStruct(tableName);
                if (dt == null) return false;
                StringBuilder sbCol = new StringBuilder();
                sbCol.AppendFormat("insert into {0}(", tableName);
                StringBuilder sbVal = new StringBuilder(" values(");

                foreach (var item in datas)
                {
                    string col = item.Key;
                    string val = item.Value;
                    if (val != null && val.Trim() == "-") continue;   //跳过特殊字段
                    if (string.IsNullOrEmpty(val)) val = ",NULL"; //排除空值
                    if (!dt.Columns.Contains(col)) continue; //排除不在表中的列

                    val = val.StartsWith(",") ? val.TrimStart(',') : string.Format("'{0}'", val);

                    sbCol.AppendFormat("{0},", col);
                    sbVal.AppendFormat("{0},", val);
                }
                sbCol = sbCol.Remove(sbCol.Length - 1, 1).Append(")");
                sbVal = sbVal.Remove(sbVal.Length - 1, 1).Append(")");

                string insertSql = sbCol.ToString() + sbVal.ToString();
                return ExecuteNonQuery(insertSql, sqlParameters) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool AddDatas(string tableName, Dictionary<string, string>[] datas)
        {

            if (string.IsNullOrWhiteSpace(tableName) || datas == null || datas.Length <= 0) throw new Exception("[DBHelper.AddDatas]添加数据出错，输入参数有误！");
            try
            {
                DataTable dt = QueryTableStruct(tableName);
                if (dt == null) return false;
                List<string> sqlList = new List<string>();
                foreach (var item in datas)
                {
                    StringBuilder sbCol = new StringBuilder();
                    sbCol.AppendFormat("insert into {0}(", tableName);
                    StringBuilder sbVal = new StringBuilder(" values(");

                    foreach (var i in item)
                    {
                        string col = i.Key;
                        string val = i.Value;
                        if (val != null && val.Trim() == "-") continue;   //跳过特殊字段
                        if (string.IsNullOrEmpty(val)) val = ",NULL"; //排除空值
                        if (!dt.Columns.Contains(col)) continue; //排除不在表中的列

                        val = val.StartsWith(",") ? val.TrimStart(',') : string.Format("'{0}'", val);

                        sbCol.AppendFormat("{0},", col);
                        sbVal.AppendFormat("{0},", val);
                    }
                    sbCol = sbCol.Remove(sbCol.Length - 1, 1).Append(")");
                    sbVal = sbVal.Remove(sbVal.Length - 1, 1).Append(")");

                    string insertSql = sbCol.ToString() + sbVal.ToString();
                    sqlList.Add(insertSql);
                }

                return ExecuteSqlsTran(sqlList, false) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateData(string tableName, string pkName, string pkVal, Dictionary<string, string> datas)
        {
            if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(pkName) || string.IsNullOrWhiteSpace(pkVal) || datas == null || datas.Count <= 0)
                throw new Exception("[DBHelper.UpdateData]修改数据出错，输入参数有误！");
            try
            {
                DataTable dt = QueryTableStruct(tableName);
                StringBuilder sbCol = new StringBuilder();
                sbCol.AppendFormat("update {0} set ", tableName);
                StringBuilder sbVal = new StringBuilder(string.Format(" where {0}='{1}'", pkName, pkVal));

                foreach (var item in datas)
                {
                    string col = item.Key;
                    string val = item.Value;
                    if (val == null) continue;  //修改数据 跳过空值
                    if (val.Trim() == "-") continue;   //跳过特殊字段
                    if (string.IsNullOrEmpty(val)) val = ",NULL"; //排除空值
                    if (!dt.Columns.Contains(col)) continue; //排除不在表中的列

                    val = val.StartsWith(",") ? val.TrimStart(',') : string.Format("'{0}'", val);
                    sbCol.AppendFormat("{0}={1},", col, val);
                }
                sbCol = sbCol.Remove(sbCol.Length - 1, 1);
                string updateSql = sbCol.ToString() + sbVal.ToString();
                return ExecuteNonQuery(updateSql) >= 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteData(string tableName, string pkName, string pkVal)
        {
            if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(pkName) || string.IsNullOrWhiteSpace(pkVal)) throw new Exception("[DBHelper.DeleteData]删除数据出错，输入参数有误！");
            try
            {
                StringBuilder sbCol = new StringBuilder();
                sbCol.AppendFormat("delete from {0} ", tableName);
                StringBuilder sbVal = new StringBuilder(string.Format(" where {0}='{1}'", pkName, pkVal));

                string deleteSql = sbCol.ToString() + sbVal;
                return ExecuteNonQuery(deleteSql) >= 0;
            }
            catch
            {
                return false;
            }
        }

        public abstract bool TableExists(string tableName);
    }
}


