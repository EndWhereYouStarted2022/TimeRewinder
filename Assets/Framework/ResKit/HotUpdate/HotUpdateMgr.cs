using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace DFramework
{
    public enum HotUpdateState
    {
        /// <summary>
        /// 从未更新过
        /// </summary>
        NeverUpdate,
        /// <summary>
        /// 更新过
        /// </summary>
        Updated,
        /// <summary>
        /// 覆盖安装，但是有旧的资源
        /// </summary>
        Overrided
    }

    public class HotUpdateMgr : MonoSingleton<HotUpdateMgr>
    {
        private HotUpdateState mState;
        /// <summary>
        /// 当前的状态
        /// </summary>
        public HotUpdateState State
        {
            get
            {
                return mState;
            }
        }

        public HotUpdateConfig Config
        {
            get;
            set;
        }

        private void Awake()
        {
            Config = new HotUpdateConfig();
        }

        /// <summary>
        /// 检测当前更新状态
        /// </summary>
        /// <param name="done">执行回调</param>
        public void CheckState(Action done)
        {
            var persistResVersionFilePath = Config.HotUpdateAssetBundlesFolder + "ResVersion.json";
            var persistResVersion = Config.LoadHotUpdateAssetBundlesFolderResVersion();
            if (persistResVersion == null)
            {
                mState = HotUpdateState.NeverUpdate;
                done();
            }
            else
            {
                StartCoroutine(Config.GetStreamingAssetResVersion(streamingResVersion =>
                {
                    if (persistResVersion.Version > streamingResVersion.Version)
                    {
                        mState = HotUpdateState.Updated;
                    }
                    else
                    {
                        mState = HotUpdateState.Overrided;
                    }
                    done();
                }));
            }
        }


        /// <summary>
        /// 是否有新的更新
        /// </summary>
        /// <param name="onResult">回调处理</param>
        public void HasNewVersion(Action<bool> onResult)
        {
            FakeResServer.Instance.GetRemoteResVersion(remoteResVersion =>
            {
                GetLocalResVersion(localResVersion =>
                {
                    var resule = remoteResVersion > localResVersion;
                    onResult(resule);
                });
            });
        }

        /// <summary>
        /// 获得当地版本号
        /// </summary>
        /// <returns></returns>
        public void GetLocalResVersion(Action<int> onResult)
        {
            if (mState == HotUpdateState.NeverUpdate || mState == HotUpdateState.Overrided)
            {
                StartCoroutine(Config.GetStreamingAssetResVersion(version => onResult(version.Version)));
                return;
            }
            var localResVersion = Config.LoadHotUpdateAssetBundlesFolderResVersion();
            onResult(localResVersion.Version);
        }

        public void UpdateRes(Action onUpdateDone)
        {
            Debug.Log("更新开始");
            Debug.Log("下载远端资源");
            FakeResServer.Instance.DownloadRes(() =>
            {
                ReplaceLocalRes();
                Debug.Log("更新完成");
                onUpdateDone();
            });
        }

        private void ReplaceLocalRes()
        {
            Debug.Log("替换本地资源");

            var tempAssetBundleFolders = FakeResServer.TempAssetBundlePath;
            var assetBundleFolders = Config.HotUpdateAssetBundlesFolder;
            //删除原有资源
            if (Directory.Exists(assetBundleFolders))
            {
                Directory.Delete(assetBundleFolders, true);
            }
            //将临时只有移动至本地资源
            Directory.Move(tempAssetBundleFolders, assetBundleFolders);
            //删除临时资源
            if (Directory.Exists(tempAssetBundleFolders))
            {
                Directory.Delete(tempAssetBundleFolders, true);
            }

        }
    }
}

