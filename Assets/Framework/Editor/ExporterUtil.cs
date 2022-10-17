using UnityEngine;
using System.IO;

namespace DFramework
{
    public class ExporterUtil
    {

#if UNITY_EDITOR
        [UnityEditor.MenuItem("DFramework/Framework/Editor/导出UnityPackage %E", false, 1)]
        private static void MenuClicked1()
        {
            CreateUnityPackage();
        }
        /// <summary>
        /// 生成 Unity package到根目录并打开
        /// </summary>
        private static void CreateUnityPackage()
        {
            string assetPathName = "Assets/DFrameWork";
            //生成package包
            UnityEditor.AssetDatabase.ExportPackage(assetPathName, EditorUtil.CreatePackageName(), UnityEditor.ExportPackageOptions.Recurse);

            EditorUtil.OpenInFolder(Path.Combine(Application.dataPath, "../"));
        }
#endif
    }
}
