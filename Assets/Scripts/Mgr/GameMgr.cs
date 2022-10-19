using DFramework;
using System;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    private int _idCounter;

    /// <summary>
    /// 游戏是否正常在运行（是否正常记录）
    /// </summary>
    public bool IsRunning { private set; get; }

    private GameMgr() { }

    private void OnApplicationQuit()
    {
        ExitGame();
    }

    public void Start()
    {
        EnterGame();
    }

    /// <summary>
    /// 开始游戏的初始化
    /// </summary>
    private void EnterGame()
    {
        _idCounter = 0;
        IsRunning = true;
    }

    /// <summary>
    /// 退出游戏前处理
    /// </summary>
    private void ExitGame()
    {
        IsRunning = false;
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
