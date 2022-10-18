﻿using Mgr;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Rewind
{
    public class RewindEntityOld : MonoBehaviour
    {
        
        public Rigidbody2D Rigidbody;
        public Collider2D Collider;
        
        [HideInInspector] public int UID { private set; get; }
        [HideInInspector]
        public bool IsRewinding
        {
            get
            {
                return isRewinding;
            }
            set
            {
                if (value)
                {
                    OnRewindStart();
                }
                else
                {
                    OnRewindStop();
                }
                isRewinding = value;
            }
        }
        [HideInInspector] public bool IsPause { get; set; } = false;

        private List<PointInTime> PointInTimes = new List<PointInTime>();
        private bool isRewinding = false;

        private void Awake()
        {
            UID = GameMgr.Instance.GetGameOnlyId();
            RewindMgr.Instance.AddEntity(UID, this);
            
            PointInTimes.Clear();
            Rigidbody ??=  transform.GetComponentInChildren<Rigidbody2D>();
            Collider ??= transform.GetComponentInChildren<Collider2D>();
        }

        private void OnDestroy()
        {
            RewindMgr.Instance.RemoveEntity(UID);
            PointInTimes.Clear();
        }

        private void FixedUpdate()
        {
            if (IsPause) return;
            if (isRewinding)
            {
                Rewind();
            }
            else
            {
                Record();
            }
        }

        private void OnRewindStart()
        {
            if (Collider)
            {
                Collider.enabled = false;
            }
            if (Rigidbody)
            {
                Rigidbody.velocity = Vector2.zero;
                Rigidbody.gravityScale = 0;
            }
            BroadcastMessage("OnRewindStart");
        }

        private void OnRewindStop()
        {
            if (Collider)
            {
                Collider.enabled = true;
            }
            if (Rigidbody)
            {
                var velocity = Vector2.zero;
                var gravity = 0f;
                if (PointInTimes.Count > 0)
                {
                    var point = PointInTimes[0];
                    velocity = point.Velocity;
                    gravity = point.GravityScale;
                }
                Rigidbody.velocity = velocity;
                Rigidbody.gravityScale = gravity;
            }
            BroadcastMessage("OnRewindStop");
        }
        
        private void Rewind()
        {
            if (PointInTimes.Count <= 0) return;
            var point = PointInTimes[0];
            var tsf = transform;
            if (PointInTimes.Count > 0)
            {
                PointInTimes.RemoveAt(0);
            }
            tsf.position = point.Position;
            tsf.rotation = point.Rotation;
            tsf.localScale = point.LocalScale;
            if (Rigidbody)
            {
                Rigidbody.velocity = point.Velocity;
            }
        }

        private void Record()
        {
            var point = new PointInTime();
            var tsf = transform;
            point.Position = tsf.position;
            point.Rotation = tsf.rotation;
            point.LocalScale = tsf.localScale;
            if (Rigidbody)
            {
                point.Velocity = Rigidbody.velocity;
                point.GravityScale = Rigidbody.gravityScale;
            }
            PointInTimes.Insert(0, point);
        }
    }
}