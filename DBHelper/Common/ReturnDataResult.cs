namespace DBHelper.Common
{
    /// <summary>
    /// 返回数据查询结果
    /// </summary>
    public class ReturnDataResult
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页面数量
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// 每页数据数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 查询数据结果
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 错误消息码
        /// </summary>
        public int errCode { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string errMsg { get; set; }
    }
}
