using System.Data;

namespace Common._Extensions.System.Data
{
    public static class DataSetExtensions
    {
        /// <summary>
        /// 数据集是否为空
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static bool DataSetIsEmpty(DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count == 0)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 数据集是否为空
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static bool DataSetIsNotEmpty(DataSet dataSet)
        {
            return !DataSetIsEmpty(dataSet);
        }

        /// <summary>
        /// 验证数据集是否为空
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static DataSet VerifyDataSetEmpty(DataSet dataSet)
        {
            if (DataSetIsEmpty(dataSet))
                return null;
            else
                return dataSet;
        }

        /// <summary>
        /// 判断DataSet是否为空
        /// </summary>
        /// <returns></returns>
        public static bool IsEmpty(this DataSet ds)
        {
            return DataSetIsEmpty(ds);
        }
        /// <summary>
        /// 判断DataSet是否不为空
        /// </summary>
        /// <returns></returns>
        public static bool IsNotEmpty(this DataSet ds)
        {
            return DataSetIsNotEmpty(ds);
        }
    }
}