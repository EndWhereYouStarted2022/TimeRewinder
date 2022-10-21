using UnityEngine.UI;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    [SerializeField] private Button btnOK;
    [SerializeField] private Text txtTip;
    
    void Start()
    {
        btnOK.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            //开始游戏，开始计时
            GameMgr.Instance.EnterGame();
        });
        
    }
    
    public void SetText(string tip)
    {
        txtTip.text = tip;
    }
    
}
