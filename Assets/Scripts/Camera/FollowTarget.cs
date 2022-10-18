using System;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target ;	// the target camera will follow
    [SerializeField] private float smooth = 20 ;	// how smoothly the camera follows
    private Vector3 Pos;

    private void Start()
    {
        _target = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (_target)
        {
            Pos = _target.position - transform.position;
            Pos.z = 0;    //摄像机的图层不能变化，所以z一直是0
            transform.position += Pos / smooth;
        }
    }
}
