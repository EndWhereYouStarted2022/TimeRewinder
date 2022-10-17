#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
namespace DFramework
{
    public class BuildAssetBundle:EditorWindow
    {
        private static BuildAssetBundle _window; 
        private string mDefaultPath = "";

        public static void Init()
        {
            _window = EditorWindow.GetWindow(typeof(BuildAssetBundle), true, "资源文件夹打包") as BuildAssetBundle;
            if (_window)
            {
                _window.Show();
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(50, 50, 180, 70), "Click to create AssetBundle"))
            {
                if (mDefaultPath == "")
                {
                    mDefaultPath = Application.dataPath + "/GameAssets/";
                }
                var myDir = new DirectoryInfo(mDefaultPath);
                if (!myDir.Exists)
                {
                }
            }
        }
    }

}
#endif
