using UnityEngine;


namespace DFramework
{
    /// <summary>
    /// 宽高比检测工具
    /// </summary>
    public partial class ResolutionCheck
    {
        /// <summary>
        /// 设备是否横屏
        /// </summary>
        /// <returns>是否横屏</returns>
        public static bool isWide()
        {
            return Screen.width > Screen.height;
        }

        /// <summary>
        /// 获得宽高比
        /// </summary>
        /// <returns>宽高比</returns>
        public static float GetAspectRatio()
        {
            float width = Screen.width, height = Screen.height;
            return isWide() ? width / height : height / width;

        }

        /// <summary>
        /// 是否是Pad的比例 4:3
        /// </summary>
        /// <returns>bool</returns>
        public static bool isPadResolution()
        {
            float aspect = GetAspectRatio();
            double standard = 4.0f / 3 - 0.05;
            return aspect > standard && aspect < (standard + 0.1);
        }

        /// <summary>
        /// 是否是16:9分辨率
        /// </summary>
        /// <returns>bool</returns>
        public static bool isPhoneResolution()
        {
            float aspect = GetAspectRatio();
            double standard = 16.0f / 9 - 0.05;
            return aspect > standard && aspect < (standard + 0.1);
        }

        /// <summary>
        /// 是否是3:2分辨率
        /// </summary>
        /// <returns>bool</returns>
        public static bool isIPhone5Resolution()
        {
            float aspect = GetAspectRatio();
            double standard = 3.0f / 2 - 0.05;
            return aspect > standard && aspect < (standard + 0.1);
        }

        /// <summary>
        /// 是否是2436:1125分辨率
        /// </summary>
        /// <returns>bool</returns>
        public static bool isIPhoneXResolution()
        {
            float aspect = GetAspectRatio();
            double standard = 2436.0f / 1125 - 0.05;
            return aspect > standard && aspect < (standard + 0.1);
        }


    }
}

