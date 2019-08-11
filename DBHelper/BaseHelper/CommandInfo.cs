using System;
using System.Data;
using System.Data.Common;

namespace DBHelper.BaseHelper
{
    public enum EffentNextType
    {
        /// <summary>
        /// 对其他语句无任何影响 
        /// </summary>
        None,
        /// <summary>
        /// 当前语句影响到的行数必须大于0，否则回滚事务
        /// </summary>
        ExcuteEffectRows
    }

    public class CommandInfo
    {
        public string Text;
        public CommandType Type = CommandType.Text;
    
        public DbParameter[] Parameters;
        public EffentNextType EffentNextType = EffentNextType.None;
        public bool FailRollback = false;
        public CommandInfo()
        {

        }
        public CommandInfo(string sqlText)
        {
            this.Text = sqlText;
        }

        public CommandInfo(string sqlText, DbParameter[] para)
        {
            this.Text = sqlText;
            this.Parameters = para;
        }

        public CommandInfo(string sqlText, CommandType type)
        {
            this.Text = sqlText;
            this.Type = type;
        }

        public CommandInfo(string sqlText, CommandType type, DbParameter[] para)
        {
            this.Text = sqlText;
            this.Type = type;
            this.Parameters = para;
        }

        public CommandInfo(string sqlText, CommandType type, DbParameter[] para, EffentNextType etype)
        {
            this.Text = sqlText;
            this.Type = type;
            this.Parameters = para;
            this.EffentNextType = etype;
        }

        public CommandInfo(string sqlText, CommandType type, DbParameter[] para, EffentNextType etype,bool failRollback)
        {
            this.Text = sqlText;
            this.Type = type;
            this.Parameters = para;
            this.EffentNextType = etype;
            this.FailRollback = failRollback;
        }
    }
}
