using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _views;

    public void OnClickStart()
    {
        Debug.Log("------ Start ------");
    }
}
