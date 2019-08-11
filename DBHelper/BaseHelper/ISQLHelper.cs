using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DBHelper.Common;

namespace DBHelper.BaseHelper
{
    public interface IDBHelper
    {
        /// <summary>
        /// 获取当前数据库类型
        /// </summary>
        /// <returns></returns>
        DataBaseType GetCurrentDataBaseType();
        /// <summary>
        /// 获取当前连接字符串
        /// </summary>
        /// <returns></returns>
        string GetCurrentConnectionString();
        /// <summary>
        /// 测试数据库连接是否正常
        /// </summary>
        /// <returns></returns>
        bool TestConnectionString();
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="dbParameter">查询语句参数</param>
        /// <returns></returns>
        DataSet Query(string sqlString, params DbParameter[] dbParameter);
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="dbParameter">查询语句参数</param>
        /// <returns></returns>
        DataTable QueryTable(string sqlString, params DbParameter[] dbParameter);
        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="dbParameter">查询语句参数</param>
        /// <returns></returns>
        DataRow QueryRow(string sqlString, params DbParameter[] dbParameter);
        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">主键字段</param>
        /// <param name="pkVal">主键字段值</param>
        /// <returns></returns>
        DataRow QueryRow(string tableName, string pkName, string pkVal);
        /// <summary>
        /// 执行查询语句，返回第一行第一列数据
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="dbParameter">查询语句参数</param>
        /// <returns></returns>
        object QueryScalar(string sqlString, CommandType cmdType = CommandType.Text, params DbParameter[] dbParameter);
        /// <summary>
        /// 是否存在相同数据
        /// </summary>
        /// <param name="strString">查询判断语句</param>
        /// <param name="dbParameter">查询语句参数</param>
        /// <returns></returns>
        bool Exists(string sqlString, params DbParameter[] dbParameter);
        /// <summary>
        /// 是否存在相同数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="colValues">列名和值</param>
        /// <param name="excludeVal">排除的列名和值</param>
        /// <returns></returns>
        bool Exists(string tableName, Dictionary<string, string> colValues, Dictionary<string, string> excludeVal = null);
        /// <summary>
        /// 查询表结构(空表)
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataTable QueryTableStruct(string tableName);
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        DataTable QueryPageData(GetDataParams param);
        /// <summary>
        /// 没有分页的数据查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dicWhere">条件列名和值</param>
        /// <param name="orderStr">排序Sql</param>
        /// <param name="rowLimit">查询数据行数</param>
        /// <returns></returns>
        DataTable QueryData(string tableName, Dictionary<string, string> dicWhere = null, string orderStr = null, int rowLimit = -1);
        /// <summary>
        /// 获取查询数据DataReader
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="dbParameter">查询参数</param>
        /// <returns></returns>
        DbDataReader ExecuteReader(string sqlString, params DbParameter[] dbParameter);
        /// <summary>
        /// 执行SQL语句 返回受影响的行数
        /// </summary>
        /// <param name="cmdInfo">语句</param>
        /// <returns></returns>
        int ExecuteNonQuery(CommandInfo cmdInfo);
        /// <summary>
        /// 执行SQL语句 返回受影响的行数
        /// </summary>
        /// <param name="cmdType">语句类型</param>
        /// <param name="cmdText">语句文本</param>
        /// <param name="dbParameter">语句参数</param>
        /// <returns></returns>
        int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] dbParameter);
        /// <summary>
        /// 执行SQL语句 返回受影响的行数
        /// </summary>
        /// <param name="cmdText">语句文本</param>
        /// <param name="dbParameter">语句参数</param>
        /// <returns></returns>
        int ExecuteNonQuery(string cmdText, params DbParameter[] dbParameter);
        /// <summary>
        /// 执行带事务的SQL语句
        /// </summary>
        /// <param name="cmdInfo"></param>
        /// <returns></returns>
        int ExecuteSqlTran(CommandInfo cmdInfo);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        int ExecuteProcedure(string storedProcName, params DbParameter[] parameters);
        /// <summary>
        /// 执行带事务的存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        int ExecuteProcedureTran(string storedProcName, params DbParameter[] parameters);
        /// <summary>
        /// 批量执行带事务的SQL语句
        /// </summary>
        /// <param name="cmdList">批量SQL语句</param>
        /// <param name="num">每次提交条数(针对耗时大量数据)</param>
        /// <returns></returns>
        int ExecuteSqlsTran(List<CommandInfo> cmdList, int num = 5000);
        /// <summary>
        /// 批量执行带事务的SQL语句
        /// </summary>
        /// <param name="SQLStringList">批量SQL语句</param>
        /// <param name="failStop">失败时回滚停止</param>
        /// <param name="num">每次提交条数(针对耗时大量数据)</param>
        /// <returns></returns>
        int ExecuteSqlsTran(List<string> sqlStringList, bool failStop = true, int num = 5000);
        /// <summary>
        /// 批量执行带事务的SQL语句
        /// </summary>
        /// <param name="SQLStringList">批量SQL语句和语句参数</param>
        /// <param name="failStop">失败时回滚停止</param>
        /// <param name="num">每次提交条数(针对耗时大量数据)</param>
        /// <returns></returns>
        int ExecuteSqlsTran(Dictionary<string, DbParameter[]> SQLStringList, bool failStop = true, int num = 5000);
        /// <summary>
        /// 得到不重复的下个最大ID(整数)
        /// </summary>
        /// <param name="ColName">字段名</param>
        /// <param name="TableName">表名</param>
        /// <param name="startID">最小开始ID</param>
        /// <param name="recycle">重复利用不连续的ID</param>
        /// <returns></returns>
        string GetNextMaxID(string ColName, string TableName, int startID = 1, bool recycle = true);
        /// <summary>
        /// 向表中添加一条数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="datas">数据列和数据值</param>
        /// <param name="sqlParameters">语句参数</param>
        /// <returns></returns>
        bool AddData(string tableName, Dictionary<string, string> datas, params DbParameter[] sqlParameters);
        /// <summary>
        /// 向表中添加多条数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="datas">数据列和数据值数组</param>
        /// <returns></returns>

        bool AddDatas(string tableName, Dictionary<string, string>[] datas);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">主键字段名</param>
        /// <param name="pkVal">主键值</param>
        /// <param name="datas">要更新的数据列表</param>
        /// <returns></returns>
        bool UpdateData(string tableName, string pkName, string pkVal, Dictionary<string, string> datas);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">主键字段名</param>
        /// <param name="pkVal">主键值</param>
        /// <returns></returns>
        bool DeleteData(string tableName, string pkName, string pkVal);
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        bool TableExists(string tableName);
    }
}
