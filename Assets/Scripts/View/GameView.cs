using System;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Button btnJump;
    public Button btnRewind;
    public Image remainBar;
    public Text pastTime;

    private float totalRewind = 300; //暂定可回溯总时间300s
    private float remainRewind = 0;  //剩余回溯时间
    private float minute = 0;
    private float second = 0;
    private float millisecond = 0;
    void Start()
    {
        btnJump.onClick.AddListener(OnJump);
        btnRewind.onClick.AddListener(OnRewind);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnJump()
    {
        
    }
    
    private void OnRewind()
    {
        
    }
    /// <summary>
    /// 传入游戏开始以来的时间
    /// </summary>
    /// <param name="time"></param>
    private void UpdateTime(float time)
    {
        minute = (int) (time / 60);
        second = (int) (time - minute*60);
        millisecond = (int) ((time - (int) time) * 1000);
        pastTime.text = String.Format("{0:D2}:{1:D2}:{2:D2}",minute,second,millisecond);
    }
    /// <summary>
    /// 设置剩余的可回溯时间
    /// </summary>
    /// <param name="restTime"></param>
    private void SetRemainRewind(float restTime)
    {
        if (restTime < 0) return;
        this.remainRewind = restTime;
        remainBar.fillAmount = remainRewind / totalRewind;
    }
}
