using Config;
using DFramework;
using Mgr;
using System;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    /// <summary>
    /// 正数时间
    /// </summary>
    private float _gameTime = 0f;
    /// <summary>
    /// 可以回溯的时间
    /// </summary>
    private float _rewindPower = 0f;
    private int _idCounter;

    [HideInInspector]
    /// <summary>
    /// 游戏是否正常在运行（是否正常记录）
    /// </summary>
    public bool IsRunning { private set; get; }
    [HideInInspector]
    public bool IsRewinding { private set; get; }

    /// <summary>
    /// 游戏剩余时间（ms)
    /// </summary>
    public float RemainTime
    {
        private set
        {
            _gameTime = value;
        }
        get
        {
            return Mathf.Round((GameConfig.TotalGameTime - _gameTime) * 1000);
        }
    }

    /// <summary>
    /// 可以回溯的时间
    /// </summary>
    public float Power
    {
        private set
        {
            _rewindPower = value;
        }
        get
        {
            return Mathf.Round(_rewindPower * 1000);
        }
    }

    /// <summary>
    /// 走过的时间
    /// </summary>
    public float TimeGone
    {
        get
        {
            return Mathf.Round(_gameTime * 1000);
        }
    }

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
        IsRunning = false;
        GameStart();
    }

    /// <summary>
    /// 退出游戏前处理
    /// </summary>
    private void ExitGame()
    {
        IsRunning = false;
        RewindMgr.Instance.ReleaseGame();
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

    public void GameStart()
    {
        _gameTime = 0;
        IsRunning = true;
        IsRewinding = false;
    }

    public void GameOver()
    {
        Debug.LogError("游戏结束了");
        IsRunning = false;
        IsRewinding = false;
    }

    public void GameFinish()
    {
        IsRunning = false;
        IsRewinding = false;
    }

    public void ResetGame()
    {
        _gameTime = 0;
        IsRunning = true;
        IsRewinding = false;
    }

    public void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.LogError("开始回放");
        //     RewindMgr.Instance.StartRewind();
        // }
        // if (Input.GetMouseButtonUp(0))
        // {
        //     Debug.LogError("结束回放");
        //     RewindMgr.Instance.StopRewind();
        // }
        // if (Input.GetMouseButtonDown(1))
        // {
        //     _rewindPower += 5;
        // }
    }

    public void FixedUpdate()
    {
        if (!IsRunning) return;
        var dt = Time.fixedDeltaTime;
        if (IsRewinding)
        {
            _gameTime = Mathf.Max(0, _gameTime - dt);
            _rewindPower = Mathf.Max(0, _rewindPower - dt);
            if (_gameTime == 0 || _rewindPower == 0)
            {
                RewindMgr.Instance.StopRewind();
            }
        }
        else
        {
            _gameTime += dt;
            if (_gameTime > GameConfig.TotalGameTime)
            {
                GameOver();
            }
        }
    }

    public void SetRewindState(bool rewinding)
    {
        IsRewinding = rewinding;
    }

    public void AddRewindPower(float power)
    {
        _rewindPower += _rewindPower;
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), RemainTime.ToString());
        GUI.Label(new Rect(10, 30, 100, 20), Power.ToString());
        GUI.Label(new Rect(10, 50, 100, 20), TimeGone.ToString());
    }

}
