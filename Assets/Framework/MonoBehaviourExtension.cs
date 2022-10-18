using System;
using System.Collections.Generic;
using UnityEngine;
namespace DFramework.Framework
{
    public abstract partial class MonoBehaviourExtension : MonoBehaviour
    {
        #region GameObject
        /// <summary>
        /// 对象显现
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 对象隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region TransformSimplify
        /// <summary>
        /// 对象初始化
        /// </summary>
        public void Identity()
        {
            transform.Identity();
        }

        public void SetLocalPosition(Vector3 pos)
        {
            transform.localPosition = pos;
        }
        
        #endregion

        #region PETimer
        private readonly PETimer mTimer = new PETimer();
        /// <summary>
        /// 添加时间定时任务
        /// </summary>
        /// <param name="callback">任务</param>
        /// <param name="delayTime">延迟时间</param>
        /// <param name="delayType">延迟单位(默认毫秒)</param>
        /// <param name="count">循环次数(默认1次) 0 一直循环</param>
        /// <param name="nextDelay">下次执行时间(默认毫秒)</param>
        protected void AddTimeTask(Action callback, double delayTime, PETimeUnit delayType = PETimeUnit.Millisecond, int count = 1, double nextDelay = 1000)
        {
            mTimer.AddTimeTask(callback, delayTime, delayType, count, nextDelay);
        }
        /// <summary>
        /// 添加帧定时任务
        /// </summary>
        /// <param name="callback">任务</param>
        /// <param name="delayFrame">延迟帧数</param>
        /// <param name="nextDelay">下次执行帧数</param>
        /// <param name="count">循环次数(默认1次) 0 一直循环</param>
        protected void AddFrameTask(Action callback, int delayFrame, int count = 1, int nextDelay = 10)
        {
            mTimer.AddFrameTask(callback, delayFrame, count, nextDelay);
        }
        /// <summary>
        /// 根据tid删除时间定时任务
        /// </summary>
        /// <param name="tid"></param>
        protected void DelTimeTask(int tid)
        {
            mTimer.DelTimeTask(tid);
        }
        /// <summary>
        /// 根据tid删除帧定时任务
        /// </summary>
        /// <param name="tid"></param>
        protected void DelFrameTask(int tid)
        {
            mTimer.DelFrameTask(tid);
        }
        /// <summary>
        /// 替换时间定时任务
        /// </summary>
        /// <param name="tid">唯一id</param>
        /// <param name="callback">任务</param>
        /// <param name="delayTime">延迟时间</param>
        /// <param name="delayType">延迟单位(默认毫秒)</param>
        /// <param name="count">循环次数(默认1次) 0 一直循环</param>
        /// <param name="nextDelay">下次执行时间(默认毫秒)</param>
        protected void ReplaceTimeTask(int tid, Action callback, double delayTime, PETimeUnit delayType = PETimeUnit.Millisecond, int count = 1, double nextDelay = 1000)
        {
            mTimer.ReplaceTimeTask(tid, callback, delayTime, delayType, count, nextDelay);
        }
        /// <summary>
        /// 替换帧定时任务
        /// </summary>
        /// <param name="tid">唯一id</param>
        /// <param name="callback">任务</param>
        /// <param name="delayFrame">延迟帧数</param>
        /// <param name="nextDelay">下次执行帧数</param>
        /// <param name="count">循环次数(默认1次) 0 一直循环</param>
        protected void ReplaceFrameTask(int tid, Action callback, int delayFrame, int nextDelay = 100, int count = 1)
        {
            mTimer.ReplaceFrameTask(tid, callback, delayFrame, nextDelay, count);
        }
        #endregion

        private void Update()
        {
            mTimer.Update();
            OnUpdate();
        }
        protected virtual void OnUpdate()
        {

        }

    }

}
