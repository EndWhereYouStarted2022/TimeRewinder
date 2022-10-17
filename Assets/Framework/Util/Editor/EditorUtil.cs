using System;
using System.IO;
using UnityEngine;

namespace DFramework
{
    /// <summary>
    /// 编译器工具
    /// </summary>
    public partial class EditorUtil
    {
        //[Obsolete("该方法已弃用，请使用Exporter.ExecuteMenu")]
        public static void OldFunction()
        {
            Debug.Log("这是一个已经被弃用的方法");
        }

        /// <summary>
        /// 生成打包名
        /// </summary>
        /// <returns></returns>
        public static string CreatePackageName()
        {
            return "DFramework_" + DateTime.Now.ToString("yyyyMMdd_hh") + ".unitypackage";
        }

        /// <summary>
        /// 打开文件夹目录
        /// </summary>
        /// <param name="path">文件夹所在目录</param>
        public static void OpenInFolder(string path)
        {
            //打开文件夹
            //Application.OpenURL("file:///" + "D:/DuLibrary/DuLibrary");
            //上级目录
            Application.OpenURL("file:///" + path);
            //FileUtil.CopyFileOrDirectory(Path.Combine(Application.dataPath,"../") + fileName, Path.Combine(Application.dataPath, "../DFramework") + fileName);
        }


        /// <summary>
        /// MenuItem的复用
        /// </summary>
        public static void ExecuteMenu(string menuName)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExecuteMenuItem(menuName);
#endif
        }

        /// <summary>
        /// 将路径中所有 '\' 替换成 '/'
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string CorrectPath(string path)
        {
            return path.Replace("\\", "/");
        }

        public static void ClearDirectory(string path)
        {
            if (!path.EndsWith("/"))
            {
                path += '/';
            }
            DeleteDirectory(path);
            CreateDirectory(path);
        }

        /// <summary>
        /// 删除dirPath目录下以xx结尾的文件
        /// </summary>
        /// <param name="dirPath">目录</param>
        /// <param name="endStr">结尾</param>
        /// <param name="recursion">是否递归删除</param>
        public static void DeleteFilesEndsWith(string dirPath, string endStr, bool recursion = false)
        {
            if (Directory.Exists(dirPath))
            {
                var files = Directory.GetFiles(dirPath, "*.*");
                foreach (var file in files)
                {
                    if (file.EndsWith(endStr))
                    {
                        File.Delete(file);
                    }
                }

                if (!recursion) return;
                var folders = Directory.GetDirectories(dirPath);
                foreach (var folder in folders)
                {
                    DeleteFilesEndsWith(folder, endStr, true);
                }
            }
            else
            {
                ClearDirectory(dirPath);
            }
        }

        /// <summary>
        /// 删除dirPath目录下pattern类型的文件,如果目录不存在？给它创建一个
        /// </summary>
        /// <param name="dirPath">目录</param>
        /// <param name="pattern">类型</param>
        /// <param name="recursion">是否递归删除子目录下面该类型文件</param>
        public static void DeleteSomeKindFiles(string dirPath, string pattern, bool recursion = false )
        {
            if (Directory.Exists(dirPath))
            {
                var files = Directory.GetFiles(dirPath,pattern);
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                if (!recursion) return;
                var folders = Directory.GetDirectories(dirPath);
                foreach (var folder in folders)
                {
                    DeleteSomeKindFiles(folder,pattern,true);
                }
            }
            else
            {
                ClearDirectory(dirPath);
            }
        }

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static void CreateDirectory(string path)
        {
            path = System.IO.Path.GetDirectoryName(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path ?? string.Empty);
            }
        }


    }
}
