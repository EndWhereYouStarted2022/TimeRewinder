using DFramework.Framework.ResKit;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;

namespace DFramework
{
    [Serializable]
    public class ResVersion
    {
        public int Version;
        public List<string> AssetBundleNames = new List<string>();
    }

    public class FakeResServer : MonoSingleton<FakeResServer>
    {
        public static string TempAssetBundlePath
        {
            get
            {
                return Application.persistentDataPath + "/TempAssetBundle/";
            }
        }

        private IEnumerator DoDownloadRes(ResVersion remoteResVersion, Action donwloadDone)
        {
            //创建临时目录
            if (!Directory.Exists(TempAssetBundlePath))
            {
                Directory.CreateDirectory(TempAssetBundlePath);
            }

            //保存 ResVersion.json 文件
            var tempResVersionFilePath = TempAssetBundlePath + "ResVersion.json";
            var tempResVersionJson = JsonUtility.ToJson(remoteResVersion);
            File.WriteAllText(tempResVersionFilePath, tempResVersionJson);

            var remoteBasePath = HotUpdateMgr.Instance.Config.RemoteAssetBundlesBaseURL;

            //补上 AssetBundleMenifest 文件 比如:Windows
            remoteResVersion.AssetBundleNames.Add(ResKitUtil.GetPlatformName());

            foreach (var assetBundleName in remoteResVersion.AssetBundleNames)
            {
                var uwr = UnityWebRequest.Get(remoteBasePath + assetBundleName);
                yield return uwr.SendWebRequest();
                if (uwr.error != null)
                {
                    throw new Exception("www download had an error " + uwr.error);
                }
                if (uwr.isDone)
                {
                    var bytes = uwr.downloadHandler.data;
                    var filePath = TempAssetBundlePath + assetBundleName;
                    File.WriteAllBytes(filePath, bytes);
                }
            }
            donwloadDone();
        }

        /// <summary>
        /// 获得远端版本号
        /// </summary>
        /// <param name="onRemoteResVersionGet">回调处理</param>
        public void GetRemoteResVersion(Action<int> onRemoteResVersionGet)
        {
            StartCoroutine(HotUpdateMgr.Instance.Config.RequestRemoteResVersion(resVersion => onRemoteResVersionGet(resVersion.Version)));
        }

        /// <summary>
        /// 下载新内容
        /// </summary>
        /// <param name="donwloadDone"></param>
        public void DownloadRes(Action donwloadDone)
        {
            StartCoroutine(HotUpdateMgr.Instance.Config.RequestRemoteResVersion(remoteResVersion =>
            {
                StartCoroutine(DoDownloadRes(remoteResVersion, donwloadDone));
            }));
        }
    }
}
