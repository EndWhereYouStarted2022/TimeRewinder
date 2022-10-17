using DFramework.Framework.ResKit;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using Config;
using UnityEditor;
using UnityEditor.Playables;
#endif

namespace DFramework
{
    public class AssetBundleExporter : MonoBehaviour
    {
#if UNITY_EDITOR
        // [MenuItem("DFramework/Framework/Build/Build AssetBundle", false)]
        // private static void BuildAssetBundles()
        // {
        //     var outputPath = BuildConfig.OutputPath + ResKitUtil.GetPlatformName();
        //     if (!Directory.Exists(outputPath))
        //     {
        //         Directory.CreateDirectory(outputPath);
        //     }
        //
        //     BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        //
        //     CreateJson(outputPath);
        // }

        /// <summary>
        /// 按照AssetsPackage中的文件夹打包
        /// </summary>
        [MenuItem("DFramework/Framework/Build/Build AssetBundle", false)]
        private static void BuildTest()
        {
            if (EditorUtility.DisplayDialog("Start Build", "Sure, according to AssetsPackage building?", "build", "cancel"))
            {
                Building(BuildTarget.StandaloneWindows64, BuildConfig.OutputPath + ResKitUtil.GetPlatformName(), BuildConfig.GAME_RESOURCE_PATH,
                    BuildConfig.GamePrefabPaths,
                    BuildConfig.GameScenePaths);
            }
        }

        private static void DeleteOldFiles(string path)
        {
            Debug.Log("正在清除旧文件夹");
            EditorUtil.ClearDirectory(path);
        }

        /// <summary>
        /// 检查是否是忽略打包的对象
        /// </summary>
        /// <param name="filePath">路径名</param>
        /// <returns></returns>
        private static bool CheckIgnore(string filePath)
        {
            return BuildConfig.BundleIgnoreList.Any(filePath.EndsWith);
        }

        /// <summary>
        /// 获取Build的打包Map
        /// </summary>
        /// <param name="path">资源目录</param>
        /// <param name="noteList">记录列表</param>
        /// <param name="buildMap">Map存储列表</param>
        private static void GetBuildMap(string path, ref Dictionary<string, List<string>> buildMap, ref List<string> noteList)
        {
            // 获得文件夹下所有目录
            var di = new DirectoryInfo(path);
            // 获得子目录
            var dirs = di.GetDirectories();
            var len = dirs.Length;
            for (var i = 0; i < len; i++)
            {
                var dir = dirs[i];
                if (EditorUtility.DisplayCancelableProgressBar("Set Bundle Map", "正在设置资源所在AssetBundle包...", (float) (i + 1 / len)))
                {
                    break;
                }
                // EditorUtility.DisplayProgressBar("Set Bundle Map", "正在设置资源所在AssetBundle包...", (float) (i + 1 / len));
                var bundleName = dir.Name + BuildConfig.EXT_NAME;
                var dirPath = path + "/" + dir.Name;
                var files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);
                if (files.Length == 0)
                {
                    //空文件跳过
                    continue;
                }
                var assetNamesList = new List<string>();
                foreach (var t in files)
                {
                    //把路径名正规化
                    var file = EditorUtil.CorrectPath(t);
                    if (CheckIgnore(file)) continue;

                    //设置assetNames
                    assetNamesList.Add(file);
                    noteList.Add(file);
                }
                buildMap.Add(bundleName, assetNamesList);
            }
            EditorUtility.ClearProgressBar();
        }

        private static void Building(BuildTarget target, string outPath, string resPath, List<string> prefabPath, List<string> scenePath)
        {
            if (!Directory.Exists(resPath))
            {
                EditorUtility.DisplayDialog("打包错误", $"资源目录不存在：\n<{resPath}>", "确定");
                return;
            }

            //删除就资源
            DeleteOldFiles(outPath);

            //记录已经设置过的AB包名，用来设置依赖包体时重复设置
            var noteBundlePaths = new List<string>();
            //AB包名和对应文件名
            var buildMap = new Dictionary<string, List<string>>();

            Debug.Log("正在设置资源目录下的AB包...");
            GetBuildMap(resPath, ref buildMap, ref noteBundlePaths);

            Debug.Log("正在打包资源目录外的依赖资源...\n");
            var othersBundleName = "exOthers" + BuildConfig.EXT_NAME;
            var othersNamesList = new List<string>();
            EditorUtility.DisplayCancelableProgressBar("Set Other Bundle Map", "正在设置AB包其他依赖关系...", 0);
            Debug.Log("正在设置需要检查依赖关系的AB包...\n");
            var checkDependencies = prefabPath.ToList();
            checkDependencies.AddRange(scenePath);
            Debug.Log("正在检测对应依赖文件...\n");
            foreach (var path in checkDependencies)
            {
                if (!Directory.Exists(path))
                {
                    EditorUtility.DisplayDialog("打包错误", $"依赖检测路径不存在：\n<{path}>", "确定");
                    EditorUtility.ClearProgressBar();
                    continue;
                }
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                var len = files.Length;
                if (len == 0) continue;
                for (var i = 0; i < files.Length; i++)
                {
                    var file = EditorUtil.CorrectPath(files[i]);
                    if (EditorUtility.DisplayCancelableProgressBar("Set Other Bundle Map", "正在设置AB包其他依赖关系...", (float) (i + 1 / len)))
                    {
                        EditorUtility.ClearProgressBar();
                        return;
                    }
                    // EditorUtility.DisplayProgressBar("Set Other Bundle Map", "正在设置AB包其他依赖关系...", (float) (i + 1 / len));
                    if (CheckIgnore(file)) continue;
                    var dps = AssetDatabase.GetDependencies(file);
                    foreach (var dp in dps)
                    {
                        //忽略文件和已经设置过的文件跳过
                        if (CheckIgnore(dp) || noteBundlePaths.Contains(dp) || dp.EndsWith(".unity")) continue;
                        othersNamesList.Add(dp);
                        noteBundlePaths.Add(dp);
                    }
                }
                EditorUtility.ClearProgressBar();
            }
            buildMap.Add(othersBundleName, othersNamesList);

            Debug.Log("正在创建AB包\n");
            var bundleList = new AssetBundleBuild[buildMap.Count];
            var index = 0;
            var nameList = new List<string>();
            foreach (var item in buildMap)
            {
                bundleList[index].assetBundleName = item.Key;
                bundleList[index].assetNames = item.Value.ToArray();
                index++;
                nameList.Add(item.Key);
            }

            Debug.Log("正在开始打包\n");
            BuildPipeline.BuildAssetBundles(outPath, bundleList, BuildAssetBundleOptions.ChunkBasedCompression, target);
            Debug.Log("正在创建Json文件");
            CreateJson(outPath,nameList);
            Debug.Log("打包完成\n");
        }

        private static void CreateJson(string outputPath,List<string> nameList)
        {
            var versionConfigFilePath = outputPath + "/ResVersion.json";
            var resVersion = new ResVersion()
            {
                Version = BuildConfig.RES_VERSION,
                AssetBundleNames = nameList
            };
            var resVersionJson = JsonUtility.ToJson(resVersion, true);
            File.WriteAllText(versionConfigFilePath, resVersionJson);
            AssetDatabase.Refresh();
        }


#endif
    }

}
