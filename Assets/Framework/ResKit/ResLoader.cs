using Config;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFramework.Framework.ResKit
{
    /// <summary>
    /// 负责记录目标脚本已经加载的资源
    /// </summary>
    public class ResLoader
    {
        #region API
        /// <summary>
        /// 同步加载单个资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetName">资源路径或名字</param>
        /// <returns>T</returns>
        public T LoadSync<T>(string assetName) where T : Object
        {
            return DoLoadSync<T>(assetName);
        }
        /// <summary>
        /// 同步加载AssetBundle资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetBundleName">assetBundle名</param>
        /// <param name="assetName">资源名</param>
        /// <returns>T</returns>
        public T LoadSync<T>(string assetBundleName, string assetName) where T : Object
        {
            return DoLoadSync<T>(assetName, assetBundleName);
        }

        /// <summary>
        /// 异步加载一个资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetName">资源路径或名字</param>
        /// <param name="onLoaded">加载回调</param>
        public void LoadAsync<T>(string assetName, Action<T> onLoaded) where T : Object
        {
            DoLoadAsync<T>(assetName, null, onLoaded);
        }
        /// <summary>
        /// 异步加载AssetBundle资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="assetBundleName">AssetBundle名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="onLoaded">加载回调</param>
        public void LoadAsync<T>(string assetBundleName, string assetName, Action<T> onLoaded) where T : Object
        {
            DoLoadAsync<T>(assetName, assetBundleName, onLoaded);
        }

        /// <summary>
        /// 释放所以加载资源
        /// </summary>
        public void ReleaseAll()
        {
            mResRecord.ForEach(res => res.Release());
            mResRecord.Clear();
        }
        #endregion

        #region Private
        private readonly List<Res> mResRecord = new List<Res>();

        /// <summary>
        /// 从本地缓存中获取Res
        /// </summary>
        /// <param name="assetName">路径</param>
        /// <returns>res</returns>
        private Res GetResFromRecord(string assetName)
        {
            return mResRecord.Find(loadAsset => loadAsset.Name == assetName);
        }

        /// <summary>
        /// 从ResManager缓存中获取Res
        /// </summary>
        /// <param name="assetName">路径</param>
        /// <returns>res</returns>
        private Res GetResFromResMgr(string assetName)
        {
            return ResManager.Instance.ShareLoaderRes.Find(loadAsset => loadAsset.Name == assetName);
        }

        /// <summary>
        /// 往本地缓存中加入res
        /// </summary>
        /// <param name="res">res</param>
        private void AddRes2Record(Res res)
        {
            res.Retain();
            mResRecord.Add(res);
        }

        /// <summary>
        /// 从本地或ResManager缓存中获得res
        /// </summary>
        /// <param name="assetName">路径</param>
        /// <returns>res</returns>
        private Res GetRecordRes(string assetName)
        {
            // 查询当前资源记录
            var res = GetResFromRecord(assetName);
            if (res != null)
            {
                return res;
            }

            // 查询全局资源池
            res = GetResFromResMgr(assetName);
            if (res != null)
            {
                AddRes2Record(res);
            }
            return res;
        }

        /// <summary>
        /// 执行同步加载
        /// </summary>
        /// <typeparam name="T">加载类型</typeparam>
        /// <param name="assetName">资源名</param>
        /// <param name="assetBundleName">AssetBundle名 默认null</param>
        /// <returns>T</returns>
        private T DoLoadSync<T>(string assetName, string assetBundleName = null) where T : Object
        {
            // 查询当前资源记录
            var res = GetRecordRes(assetName);
            if (res != null)
            {
                switch (res.State)
                {
                    case ResState.Loading:
                        throw new Exception(string.Format("不能在{0}异步加载的途中加载{0}资源", res.Name));
                    case ResState.Complete:
                        if (res.Asset == null)
                            res.LoadSync();
                        return res.Asset as T;
                    case ResState.Waiting:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (ResManager.IsSimulationModeLogic)
            {
                res = CreateRes(typeof(T), assetName, assetBundleName);
            }
            else
            {
                //加上包体后缀
                if (assetBundleName != null && !assetBundleName.EndsWith(BuildConfig.EXT_NAME))
                {
                    assetBundleName += BuildConfig.EXT_NAME;
                }
                res = CreateRes(assetName, assetBundleName);
            }
            res.LoadSync();
            return res.Asset as T;
        }

        /// <summary>
        /// 执行异步加载
        /// </summary>
        /// <typeparam name="T">加载类型</typeparam>
        /// <param name="assetName">资源名</param>
        /// <param name="assetBundleName">AssetBundle名</param>
        /// <param name="onLoaded">加载回调</param>
        private void DoLoadAsync<T>(string assetName, string assetBundleName, Action<T> onLoaded) where T : Object
        {
            // 查询当前资源记录
            var res = GetRecordRes(assetName);

            void OnResLoaded(Res loadedRes)
            {
                onLoaded(loadedRes.Asset as T);
                res.UnRegisterOnLoadedEvent(OnResLoaded);
            }

            if (res != null)
            {
                switch (res.State)
                {
                    case ResState.Loading:
                        res.RegisterOnLoadedEvent(OnResLoaded);
                        return;
                    case ResState.Complete:
                        if (res.Asset == null)
                        {
                            res.RegisterOnLoadedEvent(OnResLoaded);
                            res.LoadAsync();
                        }
                        else
                        {
                            onLoaded(res.Asset as T);
                        }
                        break;
                    case ResState.Waiting:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return;
            }
            if (ResManager.IsSimulationModeLogic)
            {
                res = CreateRes(typeof(T), assetName, assetBundleName);
            }
            else
            {
                //加上包体后缀
                if (!assetBundleName.EndsWith(BuildConfig.EXT_NAME))
                {
                    assetBundleName += BuildConfig.EXT_NAME;
                }
                res = CreateRes(assetName, assetBundleName);
            }
            res.RegisterOnLoadedEvent(OnResLoaded);
            res.LoadAsync();
        }

        /// <summary>
        /// 创建一个新的Res
        /// </summary>
        /// <param name="assetName">路径</param>
        /// <param name="ownerBundle">所属的AssetBundle</param>
        /// <returns>res</returns>
        private Res CreateRes(string assetName, string ownerBundle = null)
        {
            var res = ResFactory.Create(assetName, ownerBundle);
            ResManager.Instance.ShareLoaderRes.Add(res);
            AddRes2Record(res);
            return res;
        }

        /// <summary>
        /// 创建一个跳过AB包加载的Res
        /// </summary>
        /// <param name="T"></param>
        /// <param name="assetName"></param>
        /// <param name="ownerBundle"></param>
        /// <returns></returns>
        private Res CreateRes(Type T, string assetName, string ownerBundle = null)
        {
            Res res;
            if (ownerBundle != null)
            {
                res = new DirectoryRes(ownerBundle, assetName, T);
            }
            else
            {
                res = new ResourcesRes(assetName);
            }
            ResManager.Instance.ShareLoaderRes.Add(res);
            AddRes2Record(res);
            return res;
        }

        /// <summary>
        /// 当Res加载完毕并且
        /// </summary>
        /// <param name="res"></param>
        private void RecordResByComplete(Res res)
        {
            ResManager.Instance.ShareLoaderRes.Add(res);
            AddRes2Record(res);
        }
        #endregion

        #region StaticFunc
        private T ResourcesLoad<T>(string assetName, string bundleName = null) where T : Object
        {
#if UNITY_EDITOR
            var resPath = BuildConfig.GAME_RESOURCE_PATH + "/" + bundleName;
            if (bundleName != null)
            {
                if (Directory.Exists(resPath))
                {
                    var files = Directory.GetFiles(resPath, bundleName + ".*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        var correctFilePath = file.Replace("\\", "/");
                        if (correctFilePath.EndsWith(".meta", StringComparison.Ordinal))
                        {
                            continue;
                        }
                        if (GetAssetName(correctFilePath) != GetAssetName(assetName)) continue;
                        var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(correctFilePath);
                        //TODO 存到本地缓存里
                        return obj;
                    }
                }
                Debug.LogError($"没有{resPath}目录");
            }
            else
            {
                var res = CreateRes("resources://" + assetName);
                res.LoadSync();
                return res.Asset as T;
            }
#endif
            return null;
        }

        private static string GetAssetName(string assetName)
        {
            //例如这种：hall/click.ogg 取click
            if (assetName.IndexOf('/') >= 0)
            {
                assetName = assetName.Substring(assetName.LastIndexOf('/') + 1);
            }
            if (assetName.IndexOf('.') < 0) return assetName.ToLower();
            var strArr = assetName.Split(new string[] {"."}, StringSplitOptions.None);
            if (strArr.Length > 0)
            {
                assetName = strArr[0];
            }
            return assetName.ToLower();
        }
        #endregion
    }
}
