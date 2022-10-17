using DFramework.Framework.ResKit;
using System;
using UnityEngine;

namespace DFramework.Framework.ResKit
{
    class ResourcesRes : Res
    {
        private readonly string mAssetPath;
        public ResourcesRes(string path)
        {
            mAssetPath = path.Substring("resources://".Length);
            Name = path;
            State = ResState.Waiting;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <returns>加载是否成功</returns>
        public override bool LoadSync()
        {
            State = ResState.Loading;
            Asset = Resources.Load(mAssetPath);
            State = ResState.Complete;
            return Asset;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            State = ResState.Loading;
            var resRequest = Resources.LoadAsync(mAssetPath);
            resRequest.completed += operation =>
            {
                Asset = resRequest.asset;
                State = ResState.Complete;
            };
        }

        protected override void OnReleaseRes()
        {
            if (Asset is GameObject)
            {
                Asset = null;
                Resources.UnloadUnusedAssets();
            }
            else
            {
                Resources.UnloadAsset(Asset);
            }
            ResManager.Instance.ShareLoaderRes.Remove(this);
            Asset = null;
        }
    }
}
