using System;
using System.Collections;
using System.Collections.Generic;
using DFramework;
using UnityEngine;

public class MoneyMgr : MonoSingleton<MoneyMgr>
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //开启大门
            EntryMgr.Instance.OpenEntry();
        }
    }
}
