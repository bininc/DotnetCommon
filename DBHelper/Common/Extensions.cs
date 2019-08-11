using System;
using System.Data;

namespace DBHelper.Common
{
    /// <summary>
    /// 公用的扩展类
    /// </summary>
    public static class Extensions
    {

         /// <summary>
        /// 将数组数据导入到当前行
        /// </summary>
        /// <returns></returns>
        public static void ImportDataFromArray(this DataRow dr, object[] array)
        {
            if (dr == null || array == null) return;
            if (dr.Table.Columns.Count != array.Length) return;
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                DataColumn column = dr.Table.Columns[i];
                Type dataType = column.DataType;
                dr[column] = Common.Convert2Type(dataType, array[i]);
            }
        }


        /// <summary>
        /// 数据行值是否为空字符串
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataRow dr, string columnName)
        {
            if (dr == null) return true;
            if (dr.IsNull(columnName)) return false;
            return dr[columnName].ToString() == string.Empty;
        }

        /// <summary>
        /// 数据行值是否为空字符串
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataRow dr, int columnIndex)
        {
            if (dr == null) return true;
            if (dr.IsNull(columnIndex)) return false;
            return dr[columnIndex].ToString() == string.Empty;
        }

        /// <summary>
        /// 判断数据行是Null或者空字符串
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this DataRow dr, string columnName)
        {
            if (dr == null) return true;
            return dr.IsNull(columnName) || dr.IsEmpty(columnName);
        }

        /// <summary>
        /// 判断数据行是Null或者空字符串
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this DataRow dr, int columnIndex)
        {
            if (dr == null) return true;
            return dr.IsNull(columnIndex) || dr.IsEmpty(columnIndex);
        }

        /// <summary>
        /// 获取数据行String类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetDataRowStringValue(this DataRow dr, string columnName)
        {
            try
            {
                if (dr == null || dr.IsNull(columnName))
                    return null;
                else
                    return dr[columnName].ToString();
            }
            catch
            {
                return null;
            }
        }
        public static string GetDataRowStringValue(this DataRow dr, int columnIndex)
        {
            try
            {
                if (dr == null || dr[columnIndex] == null)
                    return null;
                else
                    return dr[columnIndex].ToString();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取数据行指定字段int类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int GetDataRowIntValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return -1;
            else
            {
                int val = -1;
                bool suc = int.TryParse(dr.GetDataRowStringValue(columnName).Trim(), out val);
                if (suc)
                    return val;
                else
                    return -2;  //非int类型
            }
        }
        public static int GetDataRowIntValue(this DataRow dr, int columnIndex)
        {
            if (dr.IsNullOrEmpty(columnIndex))
                return -1;
            else
            {
                int val = -1;
                bool suc = int.TryParse(dr.GetDataRowStringValue(columnIndex).Trim(), out val);
                if (suc)
                    return val;
                else
                    return -2;  //非int类型
            }
        }

        /// <summary>
        /// 获取数据行指定字段int类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static uint GetDataRowUintValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return uint.MaxValue;
            else
            {
                uint val;
                bool suc = uint.TryParse(dr.GetDataRowStringValue(columnName).Trim(), out val);
                if (suc)
                    return val;
                else
                    return uint.MaxValue;  //非uint类型
            }
        }
        public static uint GetDataRowUintValue(this DataRow dr, int columnIndex)
        {
            if (dr.IsNullOrEmpty(columnIndex))
                return uint.MaxValue;
            else
            {
                uint val;
                bool suc = uint.TryParse(dr.GetDataRowStringValue(columnIndex).Trim(), out val);
                if (suc)
                    return val;
                else
                    return uint.MaxValue;  //非uint类型
            }
        }
        /// <summary>
        /// 获取数据行指定字段long类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static long GetDataRowLongValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return long.MaxValue;
            else
            {
                long val;
                bool suc = long.TryParse(dr.GetDataRowStringValue(columnName).Trim(), out val);
                if (suc)
                    return val;
                else
                    return long.MaxValue;  //非uint类型
            }
        }
        public static long GetDataRowLongValue(this DataRow dr, int columnIndex)
        {
            if (dr.IsNullOrEmpty(columnIndex))
                return long.MaxValue;
            else
            {
                long val;
                bool suc = long.TryParse(dr.GetDataRowStringValue(columnIndex).Trim(), out val);
                if (suc)
                    return val;
                else
                    return long.MaxValue;  //非uint类型
            }
        }

        /// <summary>
        /// 获取数据行指定字段double类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static double GetDataRowDoubleValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return -1;
            else
            {
                double val = -1;
                bool suc = double.TryParse(dr.GetDataRowStringValue(columnName).Trim(), out val);
                if (suc)
                    return val;
                else
                    return -2;  //非double类型
            }
        }
        public static double GetDataRowDoubleValue(this DataRow dr, int columnIndex)
        {
            if (dr.IsNullOrEmpty(columnIndex))
                return -1;
            else
            {
                double val = -1;
                bool suc = double.TryParse(dr.GetDataRowStringValue(columnIndex).Trim(), out val);
                if (suc)
                    return val;
                else
                    return -2;  //非double类型
            }
        }

        /// <summary>
        /// 获取数据行指定字段float类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static float GetDataRowFloatValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return -1;
            else
            {
                float val = -1;
                bool suc = float.TryParse(dr.GetDataRowStringValue(columnName).Trim(), out val);
                if (suc)
                    return val;
                else
                    return -2;  //非double类型
            }
        }
        public static float GetDataRowFloatValue(this DataRow dr, int columnIndex)
        {
            if (dr.IsNullOrEmpty(columnIndex))
                return -1;
            else
            {
                float val = -1;
                bool suc = float.TryParse(dr.GetDataRowStringValue(columnIndex).Trim(), out val);
                if (suc)
                    return val;
                else
                    return -2;  //非double类型
            }
        }
        /// <summary>
        /// 获取数据行指定字段decimal类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static decimal GetDataRowDecimalValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return -1;
            else
            {
                decimal val = -1;
                bool suc = decimal.TryParse(dr.GetDataRowStringValue(columnName).Trim(), out val);
                if (suc)
                    return val;
                else
                    return -2;  //非double类型
            }
        }
        public static decimal GetDataRowDecimalValue(this DataRow dr, int columnIndex)
        {
            if (dr.IsNullOrEmpty(columnIndex))
                return -1;
            else
            {
                decimal val = -1;
                bool suc = decimal.TryParse(dr.GetDataRowStringValue(columnIndex).Trim(), out val);
                if (suc)
                    return val;
                else
                    return -2;  //非double类型
            }
        }
        /// <summary>
        /// 获取数据行指定字段DateTime类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DateTime GetDataRowDateTimeValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return DateTime.MinValue;
            else
            {
                DateTime val = DateTime.MinValue;
                bool suc = DateTime.TryParse(dr.GetDataRowStringValue(columnName).Trim(), out val);
                if (suc)
                    return val;
                else
                    return DateTime.MaxValue; //非DateTime类型
            }
        }
        public static DateTime GetDataRowDateTimeValue(this DataRow dr, int columnIndex)
        {
            if (dr.IsNullOrEmpty(columnIndex))
                return DateTime.MinValue;
            else
            {
                DateTime val = DateTime.MinValue;
                bool suc = DateTime.TryParse(dr.GetDataRowStringValue(columnIndex).Trim(), out val);
                if (suc)
                    return val;
                else
                    return DateTime.MaxValue; //非DateTime类型
            }
        }
        /// <summary>
        /// 获取数据行指定字段byte[]类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static byte[] GetDataRowBytesValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return new byte[0];
            else
            {
                byte[] bytes = dr[columnName] as byte[];
                if (bytes != null)
                    return bytes;
                else
                    return new byte[0];
            }
        }

        /// <summary>
        /// 获取数据航制定字段byte类型值
        /// </summary>
        /// <returns></returns>
        public static byte GetDataRowByteValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return byte.MinValue;
            else
            {
                byte val = byte.MinValue;
                bool suc = byte.TryParse(dr[columnName].ToString().Trim(), out val);
                if (suc)
                    return val;
                else
                    return byte.MaxValue; //非byte类型
            }
        }
        /// <summary>
        /// 获取指定字段的Bool类型值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool? GetDataRowBoolValue(this DataRow dr, string columnName)
        {
            if (dr.IsNullOrEmpty(columnName))
                return null;
            else
            {
                bool val = false;
                bool suc = bool.TryParse(dr.GetDataRowStringValue(columnName).Trim(), out val);
                if (suc)
                    return val;
                else
                    return null; //非byte类型
            }
        }

        /// <summary>
        /// 判断Table是否为空
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataTable table)
        {
            return Common.DataTableIsEmpty(table);
        }
        /// <summary>
        /// 判断Table是否不为空
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this DataTable table)
        {
            return Common.DataTableIsNotEmpty(table);
        }

        /// <summary>
        /// 判断DataSet是否为空
        /// </summary>
        /// <returns></returns>
        public static bool IsEmpty(this DataSet ds)
        {
            return Common.DataSetIsEmpty(ds);
        }
        /// <summary>
        /// 判断DataSet是否不为空
        /// </summary>
        /// <returns></returns>
        public static bool IsNotEmpty(this DataSet ds)
        {
            return Common.DataSetIsNotEmpty(ds);
        }

    }
}
