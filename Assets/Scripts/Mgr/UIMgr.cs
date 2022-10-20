using System;
using DFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIMgr : MonoSingleton<UIMgr>
{
    private UIMgr() { }
    [SerializeField] private List<GameObject> _views;   //the views' prefabs 

    public void Init()
    {
        
    }
    
    
}
