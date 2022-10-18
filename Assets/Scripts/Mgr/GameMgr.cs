using DFramework;
using System;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    private int _idCounter;
    private GameMgr()
    {
        EnterGame();
    }

    private void OnApplicationQuit()
    {
        ExitGame();
    }

    /// <summary>
    /// 开始游戏的初始化
    /// </summary>
    private void EnterGame()
    {
        _idCounter = 0;
    }

    /// <summary>
    /// 退出游戏前处理
    /// </summary>
    private void ExitGame()
    {
        
    }

    /// <summary>
    /// 获得游戏唯一id
    /// </summary>
    /// <returns></returns>
    public int GetGameOnlyId()
    {
        _idCounter++;
        return _idCounter;
    }
    
}
