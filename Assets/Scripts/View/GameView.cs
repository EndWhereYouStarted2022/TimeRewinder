using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Button btnJump;
    public Button btnRewind;
    public Image remainTime;
    public Text pastTime;
    
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
}
