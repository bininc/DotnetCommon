using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Common;
using DBHelper.BaseHelper;
using DBHelper.Common;
using MySql.Data.MySqlClient;

namespace MySQLHelper
{
    /// <summary>
    /// MySQL数据访问工具类类 
    /// </summary>
    public class MySQLHelper : DBHelper.DBHelper
    {
        public MySQLHelper(string connectionString=null) : base(connectionString)
        {

        }

        public override DataSet Query(string sqlString, params DbParameter[] dbParameter)
        {
            DataSet ds = new DataSet("ds");
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        PrepareCommand(cmd, connection, sqlString, null, CommandType.Text, dbParameter);
                        using (MySqlDataAdapter command = new MySqlDataAdapter())
                        {
                            command.SelectCommand = cmd;
                            command.Fill(ds, "dt");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        throw new Exception(ex.Message);
                    else
                        LogHelper.Error(ex, "MySqlHelper.Query");
                }
            }
            return ds;
        }

        public override bool TestConnectionString()
        {
            DataTable dt = QueryTable("select 1");
            if (dt.IsNotEmpty())
            {
                if (dt.Rows[0][0].ToString() == "1")
                {
                    return true;
                }
            }
            return false;
        }

        public override object QueryScalar(string sqlString, CommandType cmdType = CommandType.Text, params DbParameter[] dbParameter)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, connection, sqlString, null, cmdType, dbParameter);
                    object obj = cmd.ExecuteScalar();
                    if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
            }
        }

        public override string GetPageRowNumSql(string dataSql, int startRowNum, int endRowNum)
        {
            return $"{dataSql} limit {startRowNum},{endRowNum - startRowNum}";
        }

        public override string GetRowLimitSql(string dataSql, int rowLimit)
        {
            return $"{dataSql} limit {rowLimit}";
        }

        public override int ExecuteNonQuery(CommandInfo cmdInfo)
        {
            //Create a connection
            using (MySqlConnection connection = new MySqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    // Create a new MySql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Prepare the command
                    PrepareCommand(cmd, connection, cmdInfo.Text, null, cmdInfo.Type, cmdInfo.Parameters);

                    //Execute the command
                    int val = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return val;
                }
                catch (Exception ex)
                {
                    if (Debugger.IsAttached)
                        throw new Exception(ex.Message);
                    else
                        LogHelper.Error(ex, "MySqlHelper.ExecuteNonQuery");
                    return -1;
                }
            }
        }

        public override DbDataReader ExecuteReader(string sqlString, params DbParameter[] dbParameter)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = ConnectionString;
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader rdr = null;
            try
            {
                //Prepare the command to execute
                PrepareCommand(cmd, conn, sqlString, null, CommandType.Text, dbParameter);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                rdr?.Close();
                cmd.Dispose();
                conn.Close();
            }
            return null;
        }

        public override int ExecuteSqlTran(CommandInfo cmdInfo)
        {
            //Create a connection
            using (MySqlConnection connection = new MySqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();
                MySqlTransaction tran = connection.BeginTransaction();
                try
                {
                    // Create a new MySql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Prepare the command
                    PrepareCommand(cmd, connection, cmdInfo.Text, tran, cmdInfo.Type, cmdInfo.Parameters);

                    //Execute the command
                    int val = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return val;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    if (Debugger.IsAttached)
                        throw new Exception(ex.Message);
                    else
                        LogHelper.Error(ex, "MySqlHelper.ExecuteSqlTran");
                    return -1;
                }
                finally
                {
                    tran.Dispose();
                }
            }
        }

        public override int ExecuteProcedure(string storedProcName, params DbParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    PrepareCommand(cmd, conn, storedProcName, null, CommandType.StoredProcedure, parameters);
                    int i = cmd.ExecuteNonQuery();
                    return i;
                }
                catch (Exception ex)
                {
                    if (Debugger.IsAttached)
                        throw new Exception(ex.Message);
                    else
                        LogHelper.Error(ex, "MySqlHelper.ExecuteProcedure");
                    return -1;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
        }

        public override int ExecuteProcedureTran(string storedProcName, params DbParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                MySqlTransaction tran = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    PrepareCommand(cmd, conn, storedProcName, tran, CommandType.StoredProcedure, parameters);
                    int i = cmd.ExecuteNonQuery();
                    tran.Commit();
                    return i;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    if (Debugger.IsAttached)
                        throw new Exception(ex.Message);
                    else
                        LogHelper.Error(ex, "MySqlHelper.ExecuteProcedureTran");
                    return -1;
                }
                finally
                {
                    tran.Dispose();
                    cmd.Dispose();
                }
            }
        }

        public override int ExecuteSqlsTran(List<CommandInfo> cmdList, int num = 5000)
        {
            if (cmdList == null || cmdList.Count == 0) return -1;

            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                int allCount = 0;

                //Stopwatch watch = new Stopwatch();
                while (cmdList.Count > 0)
                {
                    //watch.Reset();
                    //watch.Start();                
                    var submitSQLs = cmdList.Take(num);
                    MySqlTransaction tx = conn.BeginTransaction();
                    MySqlCommand cmd = new MySqlCommand();
                    int count = 0;
                    try
                    {
                        foreach (CommandInfo c in submitSQLs)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(c.Text))
                                {
                                    PrepareCommand(cmd, conn, c.Text, tx, c.Type, c.Parameters);
                                    int res = cmd.ExecuteNonQuery();
                                    if (c.EffentNextType == EffentNextType.ExcuteEffectRows && res == 0)
                                    {
                                        throw new Exception("MySql:违背要求" + c.Text + "必须有影响行");
                                    }
                                    count += res;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (c.FailRollback)
                                    throw ex;
                            }
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        if (Debugger.IsAttached)
                            throw new Exception(ex.Message);
                        else
                            LogHelper.Error(ex, "MySqlHelper.ExecuteSqlsTran");
                        count = 0;
                        break;
                    }
                    finally
                    {
                        cmd.Dispose();
                        tx.Dispose();
                        allCount += count;
                    }

                    int removeCount = cmdList.Count >= num ? num : cmdList.Count; //每次最多执行1000行
                    cmdList.RemoveRange(0, removeCount);
                    //watch.Stop();
                    //Console.WriteLine(cmdList.Count + "-" + allCount + "-" + watch.ElapsedMilliseconds / 1000);
                }
                return allCount;
            }
        }

        public override DataBaseType GetCurrentDataBaseType()
        {
            return DataBaseType.MySql;
        }

        public override bool TableExists(string tableName)
        {
            string sql=$"SELECT TABLE_NAME FROM information_schema.TABLES WHERE table_name ='{tableName}'";
            return QueryScalar(sql)!=null;
        }
    }
}
