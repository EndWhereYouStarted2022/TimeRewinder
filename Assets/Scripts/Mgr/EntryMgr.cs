using System;
using System.Collections;
using System.Collections.Generic;
using DFramework;
using UnityEngine;

public class EntryMgr : MonoSingleton<EntryMgr>
{
    private Animator entryAni;
    private BoxCollider2D entryBox;

    private void Awake()
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
        entryAni = transform.GetComponent<Animator>();
        entryBox = transform.GetComponent<BoxCollider2D>();
        CloseEntry();
    }
    /// <summary>
    /// 打开入口
    /// </summary>
    public void OpenEntry()
    {
        entryAni.SetInteger("State",2);
        entryBox.isTrigger = true;
    }
    
    /// <summary>
    /// 关闭入口
    /// </summary>
    public void CloseEntry()
    {
        entryAni.SetInteger("State",1);
        entryBox.isTrigger = false;
    }
    /// <summary>
    /// 打开门的时候游戏结束
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Game Over
        }
    }
}
