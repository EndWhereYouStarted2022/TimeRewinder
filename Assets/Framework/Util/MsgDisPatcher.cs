using System;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
#if UNITY_EDITOR
#endif


namespace DFramework
{
    /// <summary>
    /// 简易消息工具
    /// </summary>
    public class MsgDisPatcher
    {
        private static Dictionary<string, Action<object>> RegisteredMsgs = new Dictionary<string, Action<object>>();
        public static void Register(string msgName, Action<object> onMsgReceived)
        {
            if (!RegisteredMsgs.ContainsKey(msgName))
            {
                RegisteredMsgs.Add(msgName, _ => { });
            }
            RegisteredMsgs[msgName] += onMsgReceived;
        }

        /// <summary>
        /// 清除所有
        /// </summary>
        public static void UnRegisterAll()
        {
            RegisteredMsgs.Clear();
        }

        /// <summary>
        /// 注销消息名的全部事件
        /// </summary>
        /// <param name="msgName">消息名</param>
        public static void UnRegister(string msgName)
        {
            if (RegisteredMsgs.ContainsKey(msgName))
            {
                RegisteredMsgs.Remove(msgName);
            }
            else
            {
                Debug.LogErrorFormat("{0} is empty", msgName);
            }
        }

        /// <summary>
        /// 注销消息名的某一个事件
        /// </summary>
        /// <param name="msgName">消息名</param>
        /// <param name="onMsgReceived">事件名</param>
        public static void UnRegister(string msgName, Action<object> onMsgReceived)
        {
            if (RegisteredMsgs.ContainsKey(msgName))
            {
                RegisteredMsgs[msgName] -= onMsgReceived;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgName">消息名</param>
        /// <param name="param">参数</param>
        public static void Send(string msgName, object param)
        {
            if (RegisteredMsgs.ContainsKey(msgName))
            {
                RegisteredMsgs[msgName](param);
            }
        }
    }
}
