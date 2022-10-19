using Config;
using Mgr;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
namespace Rewind
{
    public class RewindEntity : MonoBehaviour
    {
        public int Uid { private set; get; }
        
        public RewindData data = new RewindData();
        public Rigidbody2D rigidbody;
        public Collider2D collider;
        public Animator animator;

        public UnityEvent onRewindStart;
        public UnityEvent onRewindEnd;

        private void Start()
        {
            Uid = GameMgr.Instance.GetGameOnlyId();
            RewindMgr.Instance.AddEntity(Uid,this);
            RewindMgr.Instance.OnRewindStart += OnRewindStart;
            RewindMgr.Instance.OnRewindStop += OnRewindEnd;
            rigidbody ??= GetComponentInChildren<Rigidbody2D>();
            animator ??= GetComponentInChildren<Animator>();
            collider ??= GetComponentInChildren<Collider2D>();
        }

        public void Recording()
        {
            data.Add(transform, animator);
        }

        public void Rewinding()
        {
            data.Set(transform, animator);
        }

        public void OnRewindStart()
        {
            onRewindStart?.Invoke();
            collider.enabled = false;
        }

        public void OnRewindEnd()
        {
            onRewindEnd?.Invoke();
            collider.enabled = true;
        }

    }
}
