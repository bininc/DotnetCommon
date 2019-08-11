using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class CommonHelper
    {
        public CommonHelper()
        {
            
        }

        /// <summary>
        /// 获取一个GUID作为数据库表或者表单的主键
        /// </summary>
        /// <returns></returns>
        public static string GetGuidString()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        #region 验证是否为数字格式
        /// <summary>
        /// 验证是否为数字格式（包括浮点型）
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        /// <summary>
        /// 验证s是否为数字格式（只限整型）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumricForInt(string str)
        {
            if (str == null || str.Length == 0)
            {
                return false;
            }
            foreach (char c in str)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="val">数据值</param>
        /// <returns></returns>
        public static object Convert2Type(Type type, object val)
        {
            try
            {
                if (type == typeof(byte))
                {
                    return Convert.ToByte(val);
                }
                else if (type == typeof(byte[]))
                {
                    return val as byte[];
                }
                else if (type == typeof(short))
                {
                    return Convert.ToInt16(val);
                }
                else if (type == typeof(ushort))
                {
                    if (val?.ToString() == "-1")
                        return ushort.MaxValue;
                    return Convert.ToUInt16(val);
                }
                else if (type == typeof(int)) //int类型
                {
                    return Convert.ToInt32(val);
                }
                else if (type == typeof(uint))
                {
                    try
                    {
                        return Convert.ToUInt32(val);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                        //超出范围默认为0
                        return (uint)0;
                    }
                }
                else if (type == typeof(long))
                {
                    return Convert.ToInt64(val);
                }
                else if (type == typeof(ulong))
                {
                    return Convert.ToUInt64(val);
                }
                else if (type == typeof(float))
                {
                    return Convert.ToSingle(val);
                }
                else if (type == typeof(double))
                {
                    return Convert.ToDouble(val);
                }
                else if (type == typeof(decimal))
                {
                    return Convert.ToDecimal(val);
                }
                else if (type == typeof(bool))
                {
                    return Convert.ToBoolean(val);
                }
                else if (type == typeof(DateTime))
                {
                    return Convert.ToDateTime(val);
                }
                else if (type == typeof(string))
                    return val.ToString();
                else
                    return val;
            }
            catch (Exception ex)
            {
                return "err_" + ex.Message;
            }
        }

    }
}
