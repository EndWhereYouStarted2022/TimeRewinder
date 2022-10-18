using DFramework;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoSingleton<UIMgr>
{
    private UIMgr() { }
    [SerializeField] private List<GameObject> _views;

    public void Init()
    {
        
    }
    
    public void OnClickStart()
    {
        Debug.Log("------ Start ------");
    }
}
