using Mgr;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainView : MonoBehaviour
{
    public Button btnStart;
    private GameObject msgBox;
    void Start()
    {
        msgBox = transform.parent.Find("MessageBox").gameObject;
        btnStart.onClick.AddListener(() =>
        {
            // NotificationMgr.Instance.SendMsg("OnMainStart",new MsgBoxParam()
            //     {tip ="游戏即将开始，请做好准备...",countDown = 3,onCancel =OnBtnCancel,onOk = OnBtnOk});
            if (!msgBox.activeInHierarchy)
            {
                msgBox.SetActive(true);
                var msg = msgBox.GetComponent<MessageBox>();
                msg.SetText("游戏即将开始，请做好准备...");
                msg.SetCountDown(3);
                msg.SetDelegate(OnBtnOk,OnBtnCancel);
            }
        });
    }

    private void OnBtnOk()
    {
        SceneManager.LoadScene("Map");
    }

    private void OnBtnCancel()
    {
        msgBox?.SetActive(false);
    }
    
}
