using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonListener : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
    {
        print("按下按钮，开始回放！");
        
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        print("抬起按钮，结束回放！");
        
    }
    
}
