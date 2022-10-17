using UnityEngine;
namespace DFramework.Framework.ResKit
{
    public class ResKitUtil
    {
        public static string FullPathForAssetBundle(string assetBundleName)
        {
            var hotUpdateState = HotUpdateMgr.Instance.State;
            if (hotUpdateState == HotUpdateState.NeverUpdate || hotUpdateState == HotUpdateState.Overrided)
            {
                return HotUpdateMgr.Instance.Config.LocalAssetBundlesFolder + assetBundleName;
            }
            return HotUpdateMgr.Instance.Config.HotUpdateAssetBundlesFolder + assetBundleName;
        }

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformName(UnityEditor.EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformName(Application.platform);
#endif
        }

#if UNITY_EDITOR
        private static string GetPlatformName(UnityEditor.BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    return "Windows";
                case UnityEditor.BuildTarget.iOS:
                    return "IOS";
                case UnityEditor.BuildTarget.StandaloneLinux64:
                    return "Linux";
                case UnityEditor.BuildTarget.StandaloneOSX:
                    return "OSX";
                case UnityEditor.BuildTarget.WebGL:
                    return "WebGl";
                default:
                    return null;
            }

        }
#endif
        private static string GetPlatformName(RuntimePlatform runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.WindowsPlayer:
                    return "Window";
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                default:
                    return null;
            }
        }
    }
}
