using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DBHelper.Common
{
    /// <summary>
    /// 数据操作类型
    /// </summary>
    public enum DbOperateType
    {
        /// <summary>
        /// 查看
        /// </summary>
        [Description("查看")]
        View,
        /// <summary>
        /// 新建
        /// </summary>
        [Description("新建")]
        Add,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete,
    }
}
