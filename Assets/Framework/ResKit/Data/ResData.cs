using DFramework.Framework.ResKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Config;
using UnityEngine;
namespace DFramework
{
    public class ResData : Singleton<ResData>
    {
        public List<AssetBundleData> AssetBundleDatas = new List<AssetBundleData>();

        private static AssetBundleManifest mManifest;

        protected ResData()
        {
            Load();
        }
        private void Load()
        {
            AssetBundleDatas.Clear();

            if (ResManager.IsSimulationModeLogic)
            {
#if UNITY_EDITOR
                var di = new DirectoryInfo(BuildConfig.GAME_RESOURCE_PATH);
                var dirs = di.GetDirectories();
                // var assetBundleNames = UnityEditor.AssetDatabase.GetAllAssetBundleNames();
                foreach (var dir in dirs)
                {
                    var assetBundleName = dir.Name + BuildConfig.EXT_NAME;
                    var dirPath = BuildConfig.GAME_RESOURCE_PATH + "/" + dir.Name;
                    var files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);
                    if (files.Length == 0)
                    {
                        continue;
                    }
                    var assetBundleData = new AssetBundleData
                    {
                        Name = assetBundleName.ToLower(),
                        dependencyNames = UnityEditor.AssetDatabase.GetAssetBundleDependencies(assetBundleName, false)
                    };

                    var assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                    foreach (var path in assetPaths)
                    {
                        var assetData = new AssetData
                        {
                            Name = path.Split('/')
                            .Last()
                            .Split('.')
                            .First(),
                            OwnerBundleName = assetBundleName
                        };
                        assetBundleData.assetDataList.Add(assetData);
                    }
                    AssetBundleDatas.Add(assetBundleData);
                }

                // foreach (var ABData in AssetBundleDatas)
                // {
                //     foreach (var assetData in ABData.assetDataList)
                //     {
                //         Debug.LogFormat("AB:{0} AD:{1}", assetData.OwnerBundleName, assetData.Name);
                //     }
                //     foreach (var path in ABData.dependencyNames)
                //     {
                //         Debug.LogFormat("AB:{0} Depend:{1}", ABData.Name, path);
                //     }
                // }
#endif
            }
            else
            {
                var mainBundle = AssetBundle.LoadFromFile(ResKitUtil.FullPathForAssetBundle(ResKitUtil.GetPlatformName()));
                mManifest = mainBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
        }

        public string[] GetDirectDependencies(string bundleName)
        {
            if (ResManager.IsSimulationModeLogic)
            {
                return AssetBundleDatas
                    .Find(abData => abData.Name == bundleName)
                    .dependencyNames;
            }
            return mManifest.GetDirectDependencies(bundleName);
        }
    }
}
