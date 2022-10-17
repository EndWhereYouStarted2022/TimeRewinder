using System.Collections.Generic;
namespace Config
{
    public static class BuildConfig
    {
        // 打包资源的资源版本号
        public const int RES_VERSION = 1;
        
        // Bundle 包体后缀名
        public const string EXT_NAME = ".u3d";

        // 游戏输出目录
        public static readonly string OutputPath = UnityEngine.Application.streamingAssetsPath + "/AssetBundle/";

        // 游戏资源目录
        public const string GAME_RESOURCE_PATH = "Assets/AssetsPackage";

        public static readonly List<string> GameScenePaths = new List<string>()
        {
            GAME_RESOURCE_PATH + "/Scenes"
        };
        
        public static readonly List<string> GamePrefabPaths = new List<string>()
        {
            GAME_RESOURCE_PATH + "/Prefabs"
        };

        // 打包资源时过滤文件后缀
        public static readonly List<string> BundleIgnoreList = new List<string>()
        {
            ".meta",
            ".cs",
            ".mask"
        };
    }
}
