using System.Collections.Generic;
using System.Text;
using Common;

namespace DBHelper.Common
{
    /// <summary>
    /// 查询数据参数
    /// </summary>
    public class GetDataParams
    {
        /// <summary>
        /// 页码(和PageSize同为-1时不分页)
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页数据数量(和PageIndex同为-1时不分页)
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 页面数量
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// 查询表名
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// 表的别名
        /// </summary>
        public string TableAsName { get; set; }
        /// <summary>
        /// 列名（为空查找所有）
        /// </summary>
        public string[] Columns { get; set; }
        /// <summary>
        /// 条件字符串
        /// </summary>
        public string WhereStr { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public readonly StringBuilder Where = new StringBuilder("0=0 ");
        /// <summary>
        /// 查询条件
        /// </summary>
        public readonly Dictionary<string, string> DicWhere = new Dictionary<string, string>();
        /// <summary>
        /// Json传输的查询条件
        /// </summary>
        public Dictionary<string, string> DicWhere_Json { get; set; }
        /// <summary>
        /// GroupBy列字段
        /// </summary>
        public string[] GroupByColumns { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public OrderByCondition[] OrderByConditons { get; set; }
        /// <summary>
        /// 表连接条件
        /// </summary>
        public readonly List<JoinCondition> JoinConditions = new List<JoinCondition>();
        /// <summary>
        /// JSON传输的表连接条件
        /// </summary>
        public JoinCondition[] JoinConditions_Json { get; set; }

        /// <summary>
        /// 主键字段（单条数据用）
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 主键字段值（单条数据用）
        /// </summary>
        public string PrimaryKeyValue { get; set; }
    }

    /// <summary>
    /// 表连接方式枚举
    /// </summary>
    public enum EmJoinType
    {
        [Description("left join")]
        LeftJoin,
        [Description("inner join")]
        InnerJoin,
        [Description("right join")]
        RightJoin
    }

    /// <summary>
    /// Join条件
    /// </summary>
    public class JoinCondition
    {
        /// <summary>
        /// 要连接的表名
        /// </summary>
        public string Table { get; set; }
        /// <summary>
        /// 表的别名
        /// </summary>
        public string TableAsName { get; set; }
        /// <summary>
        /// 链接相同字段
        /// </summary>
        public string OnCol { get; set; }
        /// <summary>
        /// 表链接方式
        /// </summary>
        public EmJoinType JoinType { get; set; }
        /// <summary>
        /// 主表（默认为空）
        /// </summary>
        public string MainTable { get; set; }
        /// <summary>
        /// 主表相同字段
        /// </summary>
        public string MainCol { get; set; }
        /// <summary>
        /// 其他条件
        /// </summary>
        public string AndCondition { get; set; }
    }

    /// <summary>
    /// 排序条件
    /// </summary>
    public class OrderByCondition
    {
        /// <summary>
        /// 排序列
        /// </summary>
        public string Col { get; set; }
        /// <summary>
        /// 是否降序排序
        /// </summary>
        public bool IsDesc { get; set; }
    }
}
