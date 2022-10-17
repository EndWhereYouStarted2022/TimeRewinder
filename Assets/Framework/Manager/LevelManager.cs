﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DFramework
{
    public class LevelManager
    {
        private static List<string> mLevelNames;
        public static int Index { get; set; }
        public static void Init(List<string> levelNames)
        {
            Index = 0;
            mLevelNames = levelNames;
        }

        public static void LoadCurrent()
        {
            SceneManager.LoadScene(mLevelNames[Index]);
        }
        public static void LoadNext()
        {
            Index++;
            if (Index >= mLevelNames.Count)
            {
                Index = 0;
            }
            SceneManager.LoadScene(mLevelNames[Index]);
        }
    }
}