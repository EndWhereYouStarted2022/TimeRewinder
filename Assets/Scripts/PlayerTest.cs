using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Transform Center;
    [Range(0.1f, 1f)]public float radius = 0.2f;
    private void OnDrawGizmos()
    {
        if (Center == null) return;
        UnityEditor.Handles.DrawWireDisc(Center.position, Vector3.forward, radius);
    }
}
