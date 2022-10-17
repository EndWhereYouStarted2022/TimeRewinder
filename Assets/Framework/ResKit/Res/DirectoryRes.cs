using System;
using System.IO;
using Config;
using UnityEngine;
using Object = System.Object;
namespace DFramework.Framework.ResKit
{
    public class DirectoryRes : Res
    {
        private readonly string mAssetName;
        private readonly Type mT;
        public DirectoryRes(string path, string assetName, Type T)
        {
            Name = path;
            mT = T;
            mAssetName = assetName;
            State = ResState.Waiting;
        }

        public override bool LoadSync()
        {
            State = ResState.Loading;
#if UNITY_EDITOR
            var resPath = BuildConfig.GAME_RESOURCE_PATH + "/" + Name;
            if (Directory.Exists(resPath))
            {
                var files = Directory.GetFiles(resPath, mAssetName + ".*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var correctFilePath = file.Replace("\\", "/");
                    if (correctFilePath.EndsWith(".meta", StringComparison.Ordinal))
                    {
                        continue;
                    }
                    if (GetAssetName(correctFilePath) != GetAssetName(mAssetName)) continue;
                    Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(correctFilePath, mT);
                }
            }
            else
            {
                Debug.LogError($"没有{resPath}目录");
            }
#endif
            State = ResState.Complete;
            return Asset;
        }
        public override void LoadAsync()
        {
            LoadSync();
        }
        protected override void OnReleaseRes()
        {
            if (Asset is GameObject)
            {
                UnityEngine.Object.Destroy(Asset);
            }
            else
            {
                Resources.UnloadAsset(Asset);
            }
            Asset = null;
            ResManager.Instance.ShareLoaderRes.Remove(this);
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

    }
}
