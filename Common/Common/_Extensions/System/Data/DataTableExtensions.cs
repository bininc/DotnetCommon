using System.Data;

namespace Common._Extensions.System.Data
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// 验证DataTable是否有数据 否则传入datatable赋为null
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static DataTable DataTableVerify(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = null;
            }
            return dt;
        }
        
        /// <summary>
        /// 验证DataTable是否有数据 否则传入datatable赋为null
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static bool DataTableIsEmpty(DataTable dt)
        {
            if (DataTableVerify(dt) == null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证DataTable是否有数据 否则传入datatable赋为null
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static bool DataTableIsNotEmpty(DataTable dt)
        {
            return !DataTableIsEmpty(dt);
        }
        
        /// <summary>
        /// 判断Table是否为空
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataTable table)
        {
            return DataTableIsEmpty(table);
        }
        /// <summary>
        /// 判断Table是否不为空
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this DataTable table)
        {
            return DataTableIsNotEmpty(table);
        }
    }
}