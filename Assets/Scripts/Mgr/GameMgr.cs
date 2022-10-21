using Config;
using DFramework;
using Mgr;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoSingleton<GameMgr>
{
    public List<GameObject> rewindEffect;
    
    /// <summary>
    /// 正数时间
    /// </summary>
    private float _gameTime = 0f;
    /// <summary>
    /// 可以回溯的时间
    /// </summary>
    private float _rewindPower = 0f;
    private int _idCounter;

    /// <summary>
    /// 游戏是否正常在运行（是否正常记录）
    /// </summary>
    public bool IsRunning { private set; get; }
    public bool IsRewinding { private set; get; }

    public bool HaveKey;
    private bool IsWinning;
    

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
    
    /// <summary>
    /// 开始游戏的初始化
    /// </summary>
    public void EnterGame()
    {
        _idCounter = 0;
        GameStart();
        SetRewindEffectActive(false);
        IsWinning = false;
    }

    /// <summary>
    /// 退出游戏前处理
    /// </summary>
    public void ExitGame()
    {
        _gameTime = 0;
        IsRunning = false;
        IsRewinding = false;
        IsRunning = false;
        RewindMgr.Instance.ReleaseGame();
        SceneManager.LoadScene("Map");
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
        IsWinning = false;
        rewindEffect.Clear();
        var allEffect = GameObject.FindGameObjectsWithTag("RewindEffect");
        foreach (var obj in allEffect)
        {
            rewindEffect.Add(obj);
        }
        SetRewindEffectActive(false);
    }

    public void GameOver()
    {
        Debug.LogError("游戏结束了");
        IsRunning = false;
        IsRewinding = false;
    }

    public void GameFinish()
    {
        GameObject.Find("Canvas").transform.Find("MessageBox").Show();
    }

    public void Update()
    {
        if (HaveKey && _rewindPower > _gameTime && IsWinning)
        {
            IsRunning = false;
            IsWinning = true;
            RewindMgr.Instance.StartRewind();
        }

        if (Input.GetMouseButtonDown(2))
        {
            AddRewindPower(200);
            // IsRunning = false;
            // IsWinning = true;
            // RewindMgr.Instance.StartRewind();
        }
    }

    public void FixedUpdate()
    {
        if (!IsRunning && !IsWinning) return;
        var dt = Time.fixedDeltaTime;
        if (IsRewinding)
        {
            _gameTime = Mathf.Max(0, _gameTime - dt);
            _rewindPower = Mathf.Max(0, _rewindPower - dt);
            if (_gameTime == 0 || _rewindPower == 0)
            {
                RewindMgr.Instance.StopRewind();
                if (IsWinning)
                {
                    GameFinish();
                }
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
        SetRewindEffectActive(rewinding);
    }

    public void AddRewindPower(float power)
    {
        _rewindPower += power;
    }

    private void SetRewindEffectActive(bool show)
    {
        foreach (var obj in rewindEffect)
        {
            obj.SetActive(show);
        }
    }

}
