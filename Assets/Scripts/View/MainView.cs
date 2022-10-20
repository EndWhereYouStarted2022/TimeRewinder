using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainView : MonoBehaviour
{
    public Button btnStart;
    void Start()
    {
        btnStart.onClick.AddListener(() =>
        {
            UIMgr.Instance.OpenMsgBox("游戏即将开始，请做好准备...",3,OnBtnOk,OnBtnCancel);
        });
    }

    private void OnBtnOk()
    {
        SceneManager.LoadScene("Map");
    }

    private void OnBtnCancel()
    {
        UIMgr.Instance.CloseMsgBox();
    }
}
