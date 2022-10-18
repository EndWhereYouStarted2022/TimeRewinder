using DFramework;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private UIManager() { }
    [SerializeField] private List<GameObject> _views;

    public void Init()
    {
        
    }
    
    public void OnClickStart()
    {
        Debug.Log("------ Start ------");
    }
}
