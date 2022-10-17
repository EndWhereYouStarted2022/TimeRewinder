using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DFramework
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T _instance = null;
        protected Singleton() { }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //通过反射创建对象
                    //先获取所有非public的构造方法
                    var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    //从ctors中获取无参的构造方法
                    var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    if (ctor == null)
                    {
                        //没有构造抛出异常 及时处理
                        throw new Exception("Non-public ctor() not found");
                    }
                    _instance = ctor.Invoke(null) as T;
                }
                return _instance;
            }
        }
    }
}
