using UnityEngine;

namespace DFramework
{
    /// <summary>
    /// 数学工具
    /// </summary>
    public partial class MathUtil
    {
        /// <summary>
        /// 输入百分比返回是否命中概率
        /// </summary>
        /// <param name="percnt">概率</param>
        /// <returns></returns>
        public static bool Percent(int percnt)
        {
            return Random.Range(0, 100) < percnt;
        }
    }
}
