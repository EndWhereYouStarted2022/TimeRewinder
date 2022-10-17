using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DFramework { 
    /// <summary>
    /// 通用工具
    /// </summary>
    public static partial class CommonUtil
    {
        /// <summary>
        /// 复制文本到剪切板
        /// </summary>
        /// <param name="s">要复制的文本</param>
        public static void Copy2Clipboard(string s)
        {
            GUIUtility.systemCopyBuffer = s;
        }
    }
}

