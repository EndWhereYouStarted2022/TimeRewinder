using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mgr;

public class Item : MonoBehaviour
{
    /// <summary>
    /// 回溯时间
    /// </summary>
    private float timer = 34;
    
    /// <summary>
    /// 回溯时间
    /// </summary>
    public float Timer
    {
        set { timer = value; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //判断玩家销毁并增加回溯时间
        if (other.tag == "Player")
        {
            GameMgr.Instance.AddRewindPower(timer);
            Destroy(transform.gameObject);
        }
    }
}
