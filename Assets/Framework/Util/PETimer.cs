using System;
using System.Collections.Generic;
using System.Linq;

namespace DFramework
{
    #region 定时工具
    public class PETimer
    {
        private Action<string> mTaskLog;
        private Action<string> mTaskWarningLog;

        private const string PETIME_LOCK = "PETIME_LOCK";
        private readonly DateTime mStartDataTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        //private double nowTime = 0;
        private int mTid = 0;

        private readonly List<PeTimeTask> mTempTimeList = new List<PeTimeTask>();
        private readonly List<PeTimeTask> mTaskTimeList = new List<PeTimeTask>();

        private int mFrameCounter = 0;
        private readonly List<PeFrameTask> mTempFrameList = new List<PeFrameTask>();
        private readonly List<PeFrameTask> mTaskFrameList = new List<PeFrameTask>();

        private readonly List<int> mTidList = new List<int>();
        private readonly List<int> mRecTidList = new List<int>();

        public PETimer()
        {
            this.mFrameCounter = 0;
            this.mTid = 0;

            this.mTempTimeList.Clear();
            this.mTaskTimeList.Clear();

            this.mTempFrameList.Clear();
            this.mTaskFrameList.Clear();

            this.mTidList.Clear();
            this.mRecTidList.Clear();
        }

        public void Release()
        {
            this.mFrameCounter = 0;
            this.mTid = 0;
            this.mTempTimeList.Clear();
            this.mTaskTimeList.Clear();

            this.mTempFrameList.Clear();
            this.mTaskFrameList.Clear();

            this.mTidList.Clear();
            this.mRecTidList.Clear();
        }

        /*
         * 定时器运行心脏
         */
        public void Update()
        {
            mFrameCounter++;
            ClickTimeTask();
            ClickFrameTask();
            RecycleTid();
        }

        /*
        * update时间定时处理函数
        */
        private void ClickTimeTask()
        {
            foreach (var t in mTempTimeList)
            {
                mTaskTimeList.Add(t);
            }
            mTempTimeList.Clear();

            var nowTime = GetUtcMillisecond();
            for (var index = 0; index < mTaskTimeList.Count; index++)
            {
                var task = mTaskTimeList[index];
                if (nowTime.CompareTo(task.DelayTime) < 0)
                {
                    continue;
                }
                else
                {
                    var call = task.Callback;
                    /*//安全程序，捕获异常，反正回调函数出错
                       try
                       {
                           if (call != null)
                               call();
                       }
                       catch (Exception e)//如果异常，打印错误信息
                       {
                           LogWarningInfo(e.ToString());
                       }
                    */
                    call?.Invoke();

                    //结束定时次数
                    if (task.Count == 1)
                    {
                        mRecTidList.Add(mTaskTimeList[index].Tid);
                        mTaskTimeList.RemoveAt(index);
                        index--;
                    }
                    else
                    {
                        if (task.Count != 0)
                        {
                            task.Count--;
                        }
                        task.DelayTime += task.NextDelay;
                    }

                }
            }
        }

        /*
         * update时间定时处理函数
         */
        private void ClickFrameTask()
        {
            foreach (var t in mTempFrameList)
            {
                mTaskFrameList.Add(t);
            }
            mTempFrameList.Clear();

            for (var index = 0; index < mTaskFrameList.Count; index++)
            {
                var task = mTaskFrameList[index];
                if (task.DelayFrame > mFrameCounter)
                {
                    continue;
                }
                else
                {
                    var call = task.Callback;
                    //安全程序，捕获异常，反正回调函数出错
                    try
                    {
                        call?.Invoke();
                    }
                    catch (Exception e)//如果异常，打印错误信息
                    {
                        LogWarningInfo(e.ToString());
                    }

                    //结束定时次数
                    if (task.Count == 1)
                    {
                        mRecTidList.Add(mTaskFrameList[index].Tid);
                        mTaskFrameList.RemoveAt(index);
                        index--;
                    }
                    else
                    {
                        if (task.Count != 0)
                        {
                            task.Count--;
                        }
                        task.DelayFrame += task.NextDelay;
                    }

                }
            }
        }

        /*
         * 时间定时部分空间
         */
        #region TimeTask
        //往tempTaskList中添加定时器
        public int AddTimeTask(Action callback, double delayTime, PETimeUnit delayType = PETimeUnit.Millisecond, int count = 1, double nextDelay = 1000)
        {
            double nowTime = GetUtcMillisecond();
            if (delayType != PETimeUnit.Millisecond)
            {
                switch (delayType)
                {
                    case PETimeUnit.Second: delayTime = delayTime * 1000; nextDelay = nextDelay * 1000; break;
                    case PETimeUnit.Minute: delayTime = delayTime * 1000 * 60; nextDelay = nextDelay * 1000 * 60; break;
                    case PETimeUnit.Hour: delayTime = delayTime * 1000 * 60 * 60; nextDelay = nextDelay * 1000 * 60 * 60; break;
                    case PETimeUnit.Day: delayTime = delayTime * 1000 * 60 * 60 * 24; nextDelay = nextDelay * 1000 * 60 * 60 * 24; break;
                    default: LogWarningInfo("delayType is Error"); break;
                }
            }
            int tid = GetTid();
            double destTime = nowTime + delayTime;
            PeTimeTask timeTask = new PeTimeTask(tid, callback, destTime, nextDelay, count);
            mTempTimeList.Add(timeTask);
            mTidList.Add(tid);
            return tid;
        }
        //替换tid对应的定时器
        public bool ReplaceTimeTask(int tid, Action callback, double delayTime, PETimeUnit delayType = PETimeUnit.Millisecond, int count = 1, double nextDelay = 1000)
        {
            bool isRep = false;
            double nowTime = GetUtcMillisecond();
            if (delayType != PETimeUnit.Millisecond)
            {
                switch (delayType)
                {
                    case PETimeUnit.Second: delayTime = delayTime * 1000; nextDelay = nextDelay * 1000; break;
                    case PETimeUnit.Minute: delayTime = delayTime * 1000 * 60; nextDelay = nextDelay * 1000 * 60; break;
                    case PETimeUnit.Hour: delayTime = delayTime * 1000 * 60 * 60; nextDelay = nextDelay * 1000 * 60 * 60; break;
                    case PETimeUnit.Day: delayTime = delayTime * 1000 * 60 * 60 * 24; nextDelay = nextDelay * 1000 * 60 * 60 * 24; break;
                    default: LogWarningInfo("delayType is Error"); break;
                }
            }
            double destTime = nowTime + delayTime;
            PeTimeTask newTask = new PeTimeTask(tid, callback, destTime, nextDelay, count);

            for (int i = 0; i < mTaskTimeList.Count; i++)
            {
                if (mTaskTimeList[i].Tid == tid)
                {
                    mTaskTimeList[i] = newTask;
                    isRep = true;
                    break;
                }
            }

            for (int j = 0; j < mTempTimeList.Count; j++)
            {
                if (mTempTimeList[j].Tid == tid)
                {
                    mTempTimeList[j] = newTask;
                    isRep = true;
                    break;
                }
            }

            return isRep;
        }
        //删除tid对应的定时器
        public bool DelTimeTask(int tid)
        {
            bool exist = false;

            for (int i = 0; i < this.mTaskTimeList.Count; i++)
            {
                PeTimeTask task = this.mTaskTimeList[i];
                if (tid == task.Tid)
                {
                    mTaskTimeList.RemoveAt(i);
                    for (int j = 0; j < this.mTidList.Count; j++)
                    {
                        if (mTidList[j] == tid)
                        {
                            mTidList.RemoveAt(j);
                            break;
                        }
                    }
                    exist = true;
                    break;
                }
            }
            if (!exist)
            {
                for (int i = 0; i < this.mTempTimeList.Count; i++)
                {
                    PeTimeTask task = this.mTempTimeList[i];
                    if (tid == task.Tid)
                    {
                        mTempTimeList.RemoveAt(i);
                        for (int j = 0; j < this.mTidList.Count; j++)
                        {
                            if (mTidList[j] == tid)
                            {
                                mTidList.RemoveAt(j);
                                break;
                            }
                        }
                        exist = true;
                        break;
                    }
                }
            }

            return exist;
        }
        #endregion

        /*
         * 帧定时部分空间
         */
        #region FrameTask
        //往tempTaskList中添加定时器
        public int AddFrameTask(Action callback, int delayFrame, int count = 1, int nextDelay = 10)
        {
            int tid = GetTid();
            int destFrame = mFrameCounter + delayFrame;
            PeFrameTask frameTask = new PeFrameTask(tid, callback, destFrame, nextDelay, count);
            mTempFrameList.Add(frameTask);
            mTidList.Add(tid);
            return tid;
        }
        //替换tid对应的定时器
        public bool ReplaceFrameTask(int tid, Action callback, int delayFrame, int nextDelay = 100, int count = 1)
        {
            bool isRep = false;

            int destFrame = mFrameCounter + delayFrame;
            PeFrameTask newTask = new PeFrameTask(tid, callback, destFrame, nextDelay, count);

            for (int i = 0; i < mTaskFrameList.Count; i++)
            {
                if (mTaskFrameList[i].Tid == tid)
                {
                    mTaskFrameList[i] = newTask;
                    isRep = true;
                    break;
                }
            }

            for (int j = 0; j < mTempFrameList.Count; j++)
            {
                if (mTempFrameList[j].Tid == tid)
                {
                    mTempFrameList[j] = newTask;
                    isRep = true;
                    break;
                }
            }

            return isRep;
        }
        //删除tid对应的定时器
        public bool DelFrameTask(int tid)
        {
            bool exist = false;

            for (int i = 0; i < this.mTaskFrameList.Count; i++)
            {
                PeFrameTask task = this.mTaskFrameList[i];
                if (tid == task.Tid)
                {
                    mTaskFrameList.RemoveAt(i);
                    for (int j = 0; j < this.mTidList.Count; j++)
                    {
                        if (mTidList[j] == tid)
                        {
                            mTidList.RemoveAt(j);
                            break;
                        }
                    }
                    exist = true;
                    break;
                }
            }
            if (!exist)
            {
                for (int i = 0; i < this.mTempFrameList.Count; i++)
                {
                    PeFrameTask task = this.mTempFrameList[i];
                    if (tid == task.Tid)
                    {
                        mTempFrameList.RemoveAt(i);
                        for (int j = 0; j < this.mTidList.Count; j++)
                        {
                            if (mTidList[j] == tid)
                            {
                                mTidList.RemoveAt(j);
                                break;
                            }
                        }
                        exist = true;
                        break;
                    }
                }
            }

            return exist;
        }
        #endregion

        /*
         * 工具部分空间
         */
        #region Tool
        //获得独立的Tid
        private int GetTid()
        {
            lock (PETIME_LOCK)
            {
                mTid += 1;

                //安全代码
                if (mTid == int.MaxValue)
                {
                    mTid = 0;
                }
                while (true)
                {
                    var owned = mTidList.Any(t => mTid == t);
                    if (!owned)
                    {
                        break;
                    }
                    else
                    {
                        mTid += 1;
                    }
                }
            }
            return mTid;
        }
        //回收tid
        private void RecycleTid()
        {
            if (mRecTidList.Count <= 0) return;
            for (var i = 0; i < mRecTidList.Count; i++)
            {
                var tid = mRecTidList[i];
                for (var j = 0; j < mTidList.Count; j++)
                {
                    if (mTidList[i] != tid) continue;
                    mTidList.RemoveAt(j);
                    break;
                }
            }
            mRecTidList.Clear();
            LogInfo(mTidList.Count + "=========");
        }
        //获取UTC毫秒时间
        private double GetUtcMillisecond()
        {
            var ts = DateTime.UtcNow - mStartDataTime;
            return ts.TotalMilliseconds;
        }
        #endregion

        /*
         * 委托处理空间
         */
        #region EntrustClass
        public void SetLog(Action<string> log)
        {
            this.mTaskLog = log;
        }

        public void SetWarning(Action<string> log)
        {
            mTaskWarningLog = log;
        }

        private void LogInfo(string info)
        {
            if (mTaskLog != null)
            {
                mTaskLog(info);
            }
        }

        private void LogWarningInfo(string info)
        {
            if (mTaskWarningLog != null)
            {
                mTaskWarningLog(info);
            }
        }
        #endregion

    }
    #endregion

    #region 定时相关类
    /*
     * 时间定时类
     */
    public class PeTimeTask
    {
        public double DelayTime;
        public readonly Action Callback;
        public int Count;
        public readonly double NextDelay;
        public readonly int Tid;

        public PeTimeTask(int tid, Action callback, double delayTime, double nextDelay, int count)
        {
            this.Tid = tid;
            this.Callback = callback;
            this.DelayTime = delayTime;
            this.NextDelay = nextDelay;
            this.Count = count;
        }

    }

    /*
     * 帧定时类
     */
    public class PeFrameTask
    {
        public int DelayFrame;
        public readonly Action Callback;
        public int Count;
        public readonly int NextDelay;
        public readonly int Tid;

        public PeFrameTask(int tid, Action callback, int delayFrame, int nextDelay, int count)
        {
            this.Tid = tid;
            this.Callback = callback;
            this.DelayFrame = delayFrame;
            this.NextDelay = nextDelay;
            this.Count = count;
        }

    }

    //定时类型
    public enum PETimeUnit
    {
        Millisecond,
        Second,
        Minute,
        Hour,
        Day
    }
    #endregion
}