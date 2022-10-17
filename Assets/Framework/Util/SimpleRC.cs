using UnityEngine;
namespace DFramework
{
    public interface IRefCounter
    {
        int RefCount { get; }
        void Retain(object refOwner = null);
        void Release(object refOwner = null);
    }

    /// <summary>
    /// 简易应用计数器
    /// </summary>
    public class SimpleRC : IRefCounter
    {
        public int RefCount { get; private set; }

        public void Retain(object refOwner = null)
        {
            if (RefCount == 0)
            {
                OnFirstRef();
            }
            ++RefCount;
        }

        public void Release(object refOwner = null)
        {
            --RefCount;
            if (RefCount == 0)
            {
                OnZeroRef();
            }
        }

        protected virtual void OnZeroRef()
        {

        }

        protected virtual void OnFirstRef()
        {

        }
    }
}
