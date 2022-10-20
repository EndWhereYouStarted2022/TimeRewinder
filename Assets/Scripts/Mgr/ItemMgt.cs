using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class ItemMgt : MonoBehaviour
{
    public GameObject itemPrefab;

    public List<GameObject> points;

    public Transform pointParent;

    public int itemCount;
    //怎么方式创建道具 根据点位来创建道具
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < pointParent.childCount+1; i++)
        {
            points.Add(pointParent.Find(i.ToString()).gameObject);
        }
        CreateItem();
    }
    /// <summary>
    /// 创建道具
    /// </summary>
    void CreateItem()
    {
        while (itemCount > 0)
        {
            if (points.Count <= 0)
            {
                Debug.LogError("创建点位不");
                break;
            }
            int index = Random.Range(0, points.Count - 1);
            GameObject.Instantiate(itemPrefab,points[index].transform);
            points.RemoveAt(index);
            itemCount--;
        }
    }
}
