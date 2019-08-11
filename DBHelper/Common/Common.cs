using System;
using System.Data;
using Common;

namespace DBHelper.Common {
    public class Common {
        /// <summary>
        /// 数据集是否为空
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static bool DataSetIsEmpty (DataSet dataSet) {
            if (dataSet == null || dataSet.Tables.Count == 0) {
                return true;
            } else
                return false;
        }

        /// <summary>
        /// 数据集是否为空
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static bool DataSetIsNotEmpty (DataSet dataSet) {
            return !DataSetIsEmpty (dataSet);
        }

        /// <summary>
        /// 验证数据集是否为空
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static DataSet VerifyDataSetEmpty (DataSet dataSet) {
            if (DataSetIsEmpty (dataSet))
                return null;
            else
                return dataSet;
        }

        /// <summary>
        /// 验证DataTable是否有数据 否则传入datatable赋为null
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static DataTable DataTableVerify (DataTable dt) {
            if (dt == null || dt.Rows.Count == 0) {
                dt = null;
            }
            return dt;
        }

        /// <summary>
        /// 验证DataTable是否有数据 否则传入datatable赋为null
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static bool DataTableIsEmpty (DataTable dt) {
            if (DataTableVerify (dt) == null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 验证DataTable是否有数据 否则传入datatable赋为null
        /// </summary>
        /// <param name="dt">The dt.</param>
        public static bool DataTableIsNotEmpty (DataTable dt) {
            return !DataTableIsEmpty (dt);
        }

        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="val">数据值</param>
        /// <returns></returns>
        public static object Convert2Type(Type type, object val)
        {
            if (string.IsNullOrWhiteSpace(val?.ToString()) && type.IsValueType)
                return DBNull.Value;
            //val = val.ToString();   //先转换成String类型再进行转换
            if (val is DBNull)
                return null;
            else
            {
                return CommonHelper.Convert2Type(type, val);
            }
        }
    }
}