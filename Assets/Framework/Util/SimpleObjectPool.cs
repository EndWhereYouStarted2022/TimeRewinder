using System;
using System.Collections.Generic;
using UnityEngine;

namespace DFramework
{
    /// <summary>
    /// 池接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPool<T>
    {
        T Allocate();
        void Recycle(T obj);
    }
    /// <summary>
    /// 池实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Pool<T> : IPool<T>
    {
        #region ICountObserverable
        /// <summary>
        /// 当前的缓存队列的数量
        /// </summary>
        public int Count
        {
            get { return CacheStack.Count; }
        }

        #endregion

        protected IObjectFactory<T> Factory;
        protected readonly Stack<T> CacheStack = new Stack<T>();
        /// <summary>
        /// 池子最大容量
        /// </summary>
        protected int MaxCount;

        /// <summary>
        /// 获取池中的一个引用对象
        /// </summary>
        /// <returns>T obj</returns>
        public virtual T Allocate()
        {
            return CacheStack.Count > 0 ? CacheStack.Pop() : Factory.Create();
        }
        
        /// <summary>
        /// 回收子类重写
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>是否成功</returns>
        public abstract void Recycle(T obj);
    }

    /// <summary>
    /// 工厂接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectFactory<T>
    {
        T Create();
    }
    /// <summary>
    /// 工厂实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomObjectFactory<T> : IObjectFactory<T>
    {
        private readonly Func<T> mFactoryMethod;
        public CustomObjectFactory(Func<T> factoryMethod)
        {
            mFactoryMethod = factoryMethod;
        }

        public T Create()
        {
            return mFactoryMethod();
        }
    }


    /// <summary>
    /// 缓存池实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleObjectPool<T> : Pool<T>
    {
        /// <summary>
        /// 获取函数
        /// </summary>
        private readonly Action<T> mAllocateMethod;
        /// <summary>
        /// 回收函数
        /// </summary>
        private readonly Action<T> mResetMethod;
        /// <summary>
        /// 释放函数
        /// </summary>
        private readonly Action<T> mReleaseMethod;

        /// <summary>
        /// 简易对象池构建
        /// </summary>
        /// <param name="factoryMethod">对象新生成调用</param>
        /// <param name="allocateMethod">从池子里获取时调用</param>
        /// <param name="resetMethod">对象放回池子里调用</param>
        /// <param name="releaseMethod">释放时调用</param>
        /// <param name="maxCount">对象池数量(默认5个)</param>
        public SimpleObjectPool(Func<T> factoryMethod, Action<T> allocateMethod = null, Action<T> resetMethod = null, Action<T> releaseMethod = null, int maxCount = 5)
        {
            Factory = new CustomObjectFactory<T>(factoryMethod);
            mAllocateMethod = allocateMethod;
            mResetMethod = resetMethod;
            mReleaseMethod = releaseMethod;
            MaxCount = maxCount;
        }

        /// <summary>
        /// 进行预创建
        /// </summary>
        /// <param name="initCount">预创建进池子里的个数</param>
        public void PrePool(int initCount)
        {
            for (var i = 0; i < initCount; i++)
            {
                CacheStack.Push(Factory.Create());
            }
        }

        /// <summary>
        /// 对象回收
        /// </summary>
        /// <param name="obj">被回收的对象</param>
        /// <returns>是否成功</returns>
        public override void Recycle(T obj)
        {
            if (Count >= MaxCount)
            {
                mReleaseMethod?.Invoke(obj);
            }
            else
            {
                mResetMethod?.Invoke(obj);
                CacheStack.Push(obj);
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public override T Allocate()
        {
            var obj = base.Allocate();
            mAllocateMethod?.Invoke(obj);
            return obj;
        }

        /// <summary>
        /// 释放缓存池
        /// </summary>
        public void Release()
        {
            foreach (var obj in CacheStack)
            {
                mReleaseMethod?.Invoke(obj);
            }
            CacheStack.Clear();
        }
        
    }

}
