using Mgr;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Entity
{
    public class EntityBody : MonoBehaviour
    {
        public int UID { private set; get; }
        public List<PointInTime> PointInTimes;
        private void Awake()
        {
            //TODO 获得唯一的ID
            UID = 1;
            GameTimeMgr.Instance.AddEntity(UID,this);
            PointInTimes = new List<PointInTime>();
        }

        private void FixedUpdate()
        {
            throw new NotImplementedException();
        }

        private void OnDestroy()
        {
            GameTimeMgr.Instance.RemoveEntity(UID);
            PointInTimes.Clear();
        }
    }
}
