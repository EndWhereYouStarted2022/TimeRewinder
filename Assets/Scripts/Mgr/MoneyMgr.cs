using System;
using System.Collections;
using System.Collections.Generic;
using DFramework;
using UnityEngine;

public class MoneyMgr : MonoSingleton<MoneyMgr>
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //开启大门
            EntryMgr.Instance.OpenEntry();
            GameMgr.Instance.HaveKey = true;
            Destroy(transform.gameObject);
        }
    }
}
