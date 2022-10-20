using Mgr;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonListener : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        RewindMgr.Instance.StartRewind();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        RewindMgr.Instance.StopRewind();
    }
    
}
