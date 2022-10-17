using DFramework.Framework.ResKit;
using UnityEngine;
namespace DFramework.Framework.ResKit
{
    /// <summary>
    /// 从AssetBundle中加载资源
    /// </summary>
    public class AssetRes : Res
    {
        private readonly string mOwnerBundleName;
        private ResLoader mResLoader = new ResLoader();
        public AssetRes(string assetName, string ownerBundleName)
        {
            Name = assetName;
            mOwnerBundleName = ownerBundleName;
            State = ResState.Waiting;
        }

        public override bool LoadSync()
        {
            State = ResState.Loading;

            var ownerBundle = mResLoader.LoadSync<AssetBundle>(mOwnerBundleName);
            if (ResManager.IsSimulationModeLogic)
            {
#if UNITY_EDITOR
                var assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(mOwnerBundleName, Name);
                Asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(assetPaths[0]);
#endif
            }
            else
            {
                Asset = ownerBundle.LoadAsset(Name);
            }

            State = ResState.Complete;
            return Asset;
        }

        public override void LoadAsync()
        {
            State = ResState.Loading;
            mResLoader.LoadAsync<AssetBundle>(mOwnerBundleName, ownerBundle =>
            {
                if (ResManager.IsSimulationModeLogic)
                {
#if UNITY_EDITOR
                    var assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(mOwnerBundleName, Name);
                    Asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(assetPaths[0]);
                    State = ResState.Complete;
#endif
                }
                else
                {
                    var assetBundleRequest = ownerBundle.LoadAssetAsync(Name);
                    assetBundleRequest.completed += operation =>
                    {
                        Asset = assetBundleRequest.asset;
                        State = ResState.Complete;
                    };
                }
            });
        }


        protected override void OnReleaseRes()
        {
            if (Asset is GameObject)
            {

            }
            else
            {
                Resources.UnloadAsset(Asset);
            }
            Asset = null;
            mResLoader.ReleaseAll();
            mResLoader = null;

            ResManager.Instance.ShareLoaderRes.Remove(this);
        }
    }
}
