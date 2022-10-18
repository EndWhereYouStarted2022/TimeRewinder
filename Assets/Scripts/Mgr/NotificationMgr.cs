using DFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Mgr
{
    public class NotificationMgr : MonoSingleton<NotificationMgr>
    {
        /// <summary>
        /// 消息列表
        /// </summary>
        private readonly List<MsgRecord> mMsgRecorder = new List<MsgRecord>();

        /// <summary>
        /// 消息对象
        /// </summary>
        private class MsgRecord
        {
            private MsgRecord() { }
            /// <summary>
            /// MsgRecord对象池
            /// </summary>
            private static readonly Stack<MsgRecord> MsgRecordPool = new Stack<MsgRecord>();
            /// <summary>
            /// 生成MsgRecord对象
            /// </summary>
            /// <param name="msgName">消息名</param>
            /// <param name="onMsgReceived">消息方法</param>
            /// <returns></returns>
            public static MsgRecord Allocate(string msgName, Action<object> onMsgReceived)
            {
                var retMsgRecord = MsgRecordPool.Count > 0 ? MsgRecordPool.Pop() : new MsgRecord();
                retMsgRecord.Name = msgName;
                retMsgRecord.OnMsgReceived = onMsgReceived;
                return retMsgRecord;
            }
            /// <summary>
            /// 对象回收
            /// </summary>
            public void Recycle()
            {
                Name = null;
                OnMsgReceived = null;
                MsgRecordPool.Push(this);
            }
            public string Name;
            public Action<object> OnMsgReceived;
        }

        /// <summary>
        /// 注册单个消息
        /// </summary>
        /// <param name="msgName">消息名</param>
        /// <param name="onMsgReceived">方法</param>
        public void RegisterMsg(string msgName, Action<object> onMsgReceived)
        {
            MsgDisPatcher.Register(msgName, onMsgReceived);
            mMsgRecorder.Add(MsgRecord.Allocate(msgName, onMsgReceived));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgName">消息名</param>
        /// <param name="data">消息内容</param>
        public void SendMsg(string msgName, object data)
        {
            MsgDisPatcher.Send(msgName, data);
        }

        /// <summary>
        /// 注销整个消息事件
        /// </summary>
        /// <param name="msgName">消息名</param>
        public void UnRegisterMsg(string msgName)
        {
            var unRecorder = mMsgRecorder.FindAll(recorder => recorder.Name == msgName);
            unRecorder.ForEach(select =>
            {
                Debug.Log(select.Name);
                MsgDisPatcher.UnRegister(select.Name);
                mMsgRecorder.Remove(select);
                select.Recycle();
            });
            unRecorder.Clear();
        }

        /// <summary>
        /// 注销单个消息事件
        /// </summary>
        /// <param name="msgName">消息名</param>
        /// <param name="onMsgReceived">消息事件</param>
        public void UnRegisterMsg(string msgName, Action<object> onMsgReceived)
        {
            var unRecorder = mMsgRecorder.FindAll(recorder => recorder.Name == msgName && recorder.OnMsgReceived == onMsgReceived);
            unRecorder.ForEach(select =>
            {
                MsgDisPatcher.UnRegister(select.Name, select.OnMsgReceived);
                mMsgRecorder.Remove(select);
                select.Recycle();
            });
            unRecorder.Clear();
        }

        protected override void OnDestroy()
        {
            foreach (var msgRecord in mMsgRecorder)
            {
                MsgDisPatcher.UnRegister(msgRecord.Name, msgRecord.OnMsgReceived);
                msgRecord.Recycle();
            }
            mMsgRecorder.Clear();
        }
    }
}
