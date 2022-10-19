using System.Collections;
using System.Collections.Generic;
using DFramework;
using NPOI.SS.Formula.Functions;
using UnityEngine;

public class DoorMgr : MonoSingleton<DoorMgr>
{
    [SerializeField] private GameObject grid;
    public List<GameObject> doors;
    [SerializeField] private int currentDoor = 1;
    void Start()
    {
        grid = transform.gameObject;
        for (int i = 1; i <= 3; i++)
        {
            doors.Add(grid.transform.Find("door" + i).gameObject);
        }
    }
    /// <summary>
    /// 关闭门户
    /// </summary>
    public void CloseDoor()
    {
        if (currentDoor > doors.Count)
        {
            Debug.Log("参数错误！");
            return;
        }
        doors[currentDoor].SetActive(false);
        currentDoor++;
    }
}
