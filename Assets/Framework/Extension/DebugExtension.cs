using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DFramework
{
    public static partial class DebugExtension
    {
        // public static void Log(this Debug debug,string log)
        // {
        //     Debug.Log(log);
        // }
        //
        // public static void LogWarning(this Debug debug, string warning)
        // {
        //     Debug.LogWarning(warning);
        // }
        //
        // public static void LogError(this Debug debug, string error)
        // {
        //     Debug.LogError(error);
        // }

        public static void LogTable(this Debug debug, int[] tab)
        {
            Debug.Log("这里该输出列表");
        }
    }
}
