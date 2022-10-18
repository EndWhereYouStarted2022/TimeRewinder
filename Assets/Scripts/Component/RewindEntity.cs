using Config;
using Mgr;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace Rewind
{
    public class RewindEntity : MonoBehaviour
    {
        private int _recordRate;
        public RewindData Data = new RewindData();

        public Rigidbody Rigidbody;
        public NavMeshAgent Agent;
        public Animator Animator;

        private void Start()
        {
            _recordRate = GameConfig.RecordRate;
            
            Rigidbody ??= GetComponentInChildren<Rigidbody>();
            Agent ??= GetComponentInChildren<NavMeshAgent>();
            Animator ??= GetComponentInChildren<Animator>();
            
            
        }

        IEnumerable Recording()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / _recordRate);
                if (RewindMgr.Instance.IsRunning)
                {
                    Data.Add(transform);
                }
            }
        }

        public void OnRewindStart()
        {
            
        }
    }
}
