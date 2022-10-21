using System;
using Config;
using Mgr;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    private PlayerController _playerController;
    private GameObject msgBox;
    public GameObject btnRewind;
    public GameObject btnReplay;
    public RectTransform timeBar;//timeBar's width will grow as time goes by 
    public Image rewindBar;//rewind time
    public Text txtRest;
    
    private float widthPerMinute = 1000/(GameConfig.TotalGameTime/60); //1分钟对应的timeBar宽度
    private float _restTime = GameConfig.TotalGameTime;
    private float _pastTime = 0;  //已经过时间
    private float _rewindTime = 0;  //剩余回溯时间
    private float minute = 0;
    private float second = 0;
    private float millisecond = 0;
    void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        btnRewind.GetComponent<ButtonListener>().OnDown.AddListener(() =>
        {
            if(GameMgr.Instance.isGameOver || GameMgr.Instance.IsWinning) return;
            RewindMgr.Instance.StartRewind();
        });
        btnRewind.GetComponent<ButtonListener>().OnUp.AddListener(() =>
            {
                if(GameMgr.Instance.isGameOver || GameMgr.Instance.IsWinning) return;
                RewindMgr.Instance.StopRewind();
            });
        btnReplay.GetComponent<ButtonListener>().OnUp.AddListener(() =>
        {
            if(GameMgr.Instance.isGameOver || GameMgr.Instance.IsWinning) return;
            GameMgr.Instance.ReloadGame();
        });
        msgBox = transform.parent.Find("MessageBox").gameObject;
        if (!msgBox.activeInHierarchy)
        {
            var msg = msgBox.GetComponent<MessageBox>();
            msg.SetText("游戏即将开始，请做好准备...");
            msgBox.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    public void OnJump()
    {
        _playerController?.ClickJump();
    }
    
    //更新时间显示
    private void UpdateTime()
    {
        //update time
        _pastTime = GameMgr.Instance.TimeGone / 1000;
        _restTime = GameMgr.Instance.RemainTime / 1000;
        _rewindTime = GameMgr.Instance.Power / 1000;
        minute = (int) (_restTime / 60);
        second = (int) (_restTime - minute*60);
        millisecond = (int) ((_restTime - (int) _restTime) * 100);
        txtRest.text = String.Format("{0}:{1}:{2}",minute.ToString("00"),second.ToString("00"),millisecond.ToString("00"));
        
        //update timeBar and rewindBar
        timeBar.sizeDelta = new Vector2(_pastTime/60*widthPerMinute,50) ;
        rewindBar.fillAmount = _rewindTime / _pastTime;
    }
    
}
