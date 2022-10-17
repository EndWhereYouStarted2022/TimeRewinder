using System;
using Object = UnityEngine.Object;

namespace DFramework.Framework.ResKit
{
    public enum ResState
    {
        /// <summary>
        /// 刚刚创建好Res对象，但还没有真正进行加载
        /// </summary>
        Waiting,
        /// <summary>
        /// 资源正在加载
        /// </summary>
        Loading,
        /// <summary>
        /// 资源加载完成
        /// </summary>
        Complete
    }

    /// <summary>
    /// 负责加载、卸载操作
    /// </summary>
    public abstract class Res : SimpleRC
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        public ResState State
        {
            get
            {
                return mState;
            }
            protected set
            {
                mState = value;
                if (mState == ResState.Complete)
                {
                    OnLoadedEvent?.Invoke(this);
                }
            }
        }
        private ResState mState;


        /// <summary>
        /// 资源本体
        /// </summary>
        public Object Asset { get; protected set; }
        /// <summary>
        /// 资源加载路径名
        /// </summary>
        public string Name { get; protected set; }

        public abstract bool LoadSync();

        public abstract void LoadAsync();

        /// <summary>
        /// 卸载处理函数（子类重写）
        /// </summary>
        protected abstract void OnReleaseRes();

        /// <summary>
        /// 无引用处理函数（不推荐子类进行重写）
        /// </summary>
        protected override void OnZeroRef()
        {
            OnReleaseRes();
        }

        private event Action<Res> OnLoadedEvent;

        public void RegisterOnLoadedEvent(Action<Res> onLoaded)
        {
            OnLoadedEvent += onLoaded;
        }

        public void UnRegisterOnLoadedEvent(Action<Res> onLoaded)
        {
            OnLoadedEvent -= onLoaded;
        }
    }
}
