using DFramework.Framework.ResKit;
using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace DFramework
{
    public class HotUpdateConfig
    {
        /// <summary>
        /// 热更资源存放路径
        /// </summary>
        public virtual string HotUpdateAssetBundlesFolder
        {
            get
            {
                return Application.persistentDataPath + "/AssetBundles/";
            }
        }

        /// <summary>
        /// 本地资源信息文件路径
        /// </summary>
        public virtual string LocalResVersionFilePath
        {
            get
            {
                return Application.streamingAssetsPath + "/AssetBundle/" + ResKitUtil.GetPlatformName() + "/ResVersion.json";
            }
        }

        /// <summary>
        /// 本地资源存放路径
        /// </summary>
        public virtual string LocalAssetBundlesFolder
        {
            get
            {
                return Application.streamingAssetsPath + "/AssetBundle/" + ResKitUtil.GetPlatformName() + "/";
            }
        }

        /// <summary>
        /// 远端版本信息URL
        /// </summary>
        public virtual string RemoteResVersionURL
        {
            get
            {
                return Application.dataPath + "/DFramework/Framework/ResKit/HotUpdate/Remote/ResVersion.json";
            }
        }

        /// <summary>
        /// 远端资源路径URL
        /// </summary>
        public virtual string RemoteAssetBundlesBaseURL
        {
            get
            {
                return Application.dataPath + "/DFramework/Framework/ResKit/HotUpdate/Remote/";
            }
        }

        public virtual ResVersion LoadHotUpdateAssetBundlesFolderResVersion()
        {
            var persistResVersionFilePath = HotUpdateAssetBundlesFolder + "ResVersion.json";

            //文件不存在则直接返回 null
            if (!File.Exists(persistResVersionFilePath))
            {
                return null;
            }

            var persistResVersionJson = File.ReadAllText(persistResVersionFilePath);
            var persistResVersion = JsonUtility.FromJson<ResVersion>(persistResVersionJson);

            return persistResVersion;
        }

        public virtual IEnumerator GetStreamingAssetResVersion(Action<ResVersion> getResVersion)
        {
            var streamingResVersionFilePath = LocalResVersionFilePath;
            var uwr = UnityWebRequest.Get(streamingResVersionFilePath);
            yield return uwr.SendWebRequest();
            // if (uwr.result != UnityWebRequest.Result.Success)
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Show results as text
                var jsonString = uwr.downloadHandler.text;
                var resVersion = JsonUtility.FromJson<ResVersion>(jsonString);
                getResVersion(resVersion);
            }
        }

        /// <summary>
        /// IEnumerator 请求远端获得版本号
        /// </summary>
        /// <param name="onResDownloaded">回调处理</param>
        /// <returns></returns>
        public virtual IEnumerator RequestRemoteResVersion(Action<ResVersion> onResDownloaded)
        {
            var remoteResVersionPath = RemoteResVersionURL;
            var uwr = UnityWebRequest.Get(remoteResVersionPath);
            var www = uwr.SendWebRequest();
            yield return www;
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Show results as text
                var jsonString = uwr.downloadHandler.text;
                var resVersion = JsonUtility.FromJson<ResVersion>(jsonString);
                onResDownloaded(resVersion);
            }
        }
    }
}
