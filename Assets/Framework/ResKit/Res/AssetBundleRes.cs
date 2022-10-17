using DFramework.Framework.ResKit;
using System;
using UnityEngine;

namespace DFramework.Framework.ResKit
{
    public class AssetBundleRes : Res
    {
        private readonly string mPath;
        private ResLoader mResLoader = new ResLoader();

        public AssetBundle AssetBundle
        {
            get { return Asset as AssetBundle; }
            set { Asset = value; }
        }
        public AssetBundleRes(string assetName)
        {
            mPath = ResKitUtil.FullPathForAssetBundle(assetName);
            Name = assetName;
            State = ResState.Waiting;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <returns>是否加载完成</returns>
        public override bool LoadSync()
        {
            State = ResState.Loading;
            //获得所有依赖资源名

            var dependencyBundleNames = ResData.Instance.GetDirectDependencies(Name);

            //加载所有依赖资源
            foreach (var name in dependencyBundleNames)
            {
                mResLoader.LoadSync<AssetBundle>(name);
            }

            if (!ResManager.IsSimulationModeLogic)
            {
                AssetBundle = AssetBundle.LoadFromFile(mPath);
            }

            State = ResState.Complete;
            return AssetBundle;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            State = ResState.Loading;
            LoadDependencyBundlesAsync(() =>
            {
                if (ResManager.IsSimulationModeLogic)
                {
                    State = ResState.Complete;
                }
                else
                {
                    var resRequest = AssetBundle.LoadFromFileAsync(mPath);
                    resRequest.completed += operation =>
                    {
                        AssetBundle = resRequest.assetBundle;
                        State = ResState.Complete;
                    };
                }
            });
        }

        /// <summary>
        /// 异步加载依赖资源
        /// </summary>
        /// <param name="onAllComplete">全部依赖资源加载完成后回调</param>
        private void LoadDependencyBundlesAsync(Action onAllComplete)
        {
            var dependencyBundleNames = ResData.Instance.GetDirectDependencies(Name);
            var loadedCount = 0;

            if (dependencyBundleNames.Length == 0)
            {
                //没有依赖资源
                onAllComplete();
            }
            else
            {
                //异步加载依赖资源
                foreach (var name in dependencyBundleNames)
                {
                    mResLoader.LoadAsync<AssetBundle>(name,
                        dependBundle =>
                    {
                        loadedCount++;
                        if (loadedCount == dependencyBundleNames.Length)
                        {
                            //最后一个依赖资源加载完毕处理
                            onAllComplete();
                        }
                    });
                }
            }
        }



        protected override void OnReleaseRes()
        {
            if (AssetBundle != null)
            {
                AssetBundle.Unload(true);
                AssetBundle = null;

                mResLoader.ReleaseAll();
                mResLoader = null;
            }
            ResManager.Instance.ShareLoaderRes.Remove(this);
        }
    }
}

