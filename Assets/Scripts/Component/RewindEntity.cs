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

        private float _gravity;

        private void Start()
        {
            Uid = GameMgr.Instance.GetGameOnlyId();
            RewindMgr.Instance.AddEntity(Uid, this);
            RewindMgr.Instance.OnRewindStart += OnRewindStart;
            RewindMgr.Instance.OnRewindStop += OnRewindEnd;
            rigidbody ??= GetComponentInChildren<Rigidbody2D>();
            animator ??= GetComponentInChildren<Animator>();
            collider ??= GetComponentInChildren<Collider2D>();

            if (rigidbody)
            {
                _gravity = rigidbody.gravityScale;
            }
        }

        private void OnDestroy()
        {
            RewindMgr.Instance.RemoveEntity(Uid);
            RewindMgr.Instance.OnRewindStart -= OnRewindStart;
            RewindMgr.Instance.OnRewindStop -= OnRewindEnd;
        }

        public void Recording()
        {
            data.Add(transform, animator);
        }

        public void Rewinding()
        {
            data.Set(transform, animator);
        }

        private void OnRewindStart()
        {
            onRewindStart?.Invoke();
            if (animator)
            {
                animator.speed = 0;
            }
            if (collider)
            {
                collider.enabled = false;
            }
            if (rigidbody)
            {
                rigidbody.gravityScale = 0;
            }
        }

        private void OnRewindEnd()
        {
            onRewindEnd?.Invoke();
            if (animator)
            {
                animator.speed = 1;
            }
            if (collider)
            {
                collider.enabled = true;
            }
            if (rigidbody)
            {
                rigidbody.velocity = Vector2.zero;
                rigidbody.gravityScale = _gravity;
            }
        }

    }
}
