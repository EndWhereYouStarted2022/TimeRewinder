using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBox : MonoBehaviour
{
    [SerializeField] private Button btnOK;

    void Start()
    {
        btnOK.onClick.AddListener(() =>
        {
            //重新加载场景，开始游戏
            GameMgr.Instance.ReloadGame();
        });

    }
    
}
