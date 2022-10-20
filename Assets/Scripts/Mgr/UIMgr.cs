using DFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIMgr : MonoSingleton<UIMgr>
{
    private UIMgr() { }
    [SerializeField] private List<GameObject> _views;   //the views' prefabs 
    private Dictionary<string, GameObject> Views;  // the views you instantiate in the scene
    private Transform canvas;
    public void Init()
    {
        canvas = GameObject.Find("Canvas").transform;
    }
    
    public void OnClickStart()
    {
        Debug.Log("------ Start ------");
    }
    //if you don't need countDown, let countDown = 0 
    public void OpenMsgBox(string tip,int countDown,UnityAction onOk,UnityAction onCancel)
    {
        if (_views[0] && canvas != null)
        {
            GameObject box = Instantiate(_views[0], canvas);
            var msg = box.GetComponent<MessageBox>();
            msg.SetText(tip);
            msg.SetCountDown(countDown);
            msg.SetDelegate(onOk, onCancel);
            Views.Add("MessageBox",box);
        }
    }
    public void CloseMsgBox()
    {
        if (Views.ContainsKey("MessageBox"))
        {
            Destroy(Views["MessageBox"]);
            Views.Remove("MessageBox");
        }
    }
}
