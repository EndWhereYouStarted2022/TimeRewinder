using System;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Button btnJump;
    public Button btnRewind;
    public RectTransform timeBar;//timeBar's width will grow as time goes by 
    public Image rewindBar;//rewind time
    public Text txtPast;

    private float widthPerMinute = 100; //1分钟对应的timeBar宽度
    private float _pastTime = 0;  //已经过时间
    private float _rewindTime = 0;  //剩余回溯时间
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
    /// 传入游戏开始至今经过的时间
    /// </summary>
    /// <param name="time">单位为秒的时间</param>
    private void UpdateTime(float time)
    {
        //update time
        _pastTime = time;
        minute = (int) (time / 60);
        second = (int) (time - minute*60);
        millisecond = (int) ((time - (int) time) * 1000);
        txtPast.text = String.Format("{0:D2}:{1:D2}:{2:D2}",minute,second,millisecond);
        
        //update timeBar and rewindBar
        timeBar.sizeDelta = new Vector2(_pastTime/60*widthPerMinute,50) ;
        rewindBar.fillAmount = _rewindTime / _pastTime;
    }
    /// <summary>
    /// 设置剩余的可回溯时间
    /// </summary>
    /// <param name="restTime"></param>
    private void SetRemainRewind(float restTime)
    {
        if (restTime < 0) return;
        _rewindTime = restTime;
    }
}
