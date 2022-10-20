
using UnityEngine;

namespace DFramework
{
    /// <summary>
    /// Mono单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T mInstance = null;
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogWarning("More than 1");
                        return mInstance;
                    }
                    if (mInstance == null)
                    {
                        var instanceName = typeof(T).Name;
                        //Debug.LogFormat("Instance Name:{0}", instanceName);
                        var instanceObj = GameObject.Find(instanceName);
                        if (!instanceObj)
                        {
                            instanceObj = new GameObject(instanceName);
                        }
                        mInstance = instanceObj.AddComponent<T>();
                        DontDestroyOnLoad(instanceObj);
                        //Debug.LogFormat("Add New Singleton {0} in Game!", instanceName);
                    }
                    else
                    {
                        DontDestroyOnLoad(mInstance.gameObject);
                        Debug.LogFormat("Already exist:{0}", mInstance.name);
                    }
                }
                return mInstance;
            }
        }
        protected virtual void OnDestroy()
        {
            mInstance = null;
        }
    }
}
