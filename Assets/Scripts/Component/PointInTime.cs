using Mgr;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Rewind
{
    [Serializable]
    public class PointInTime
    {
        //TODO 需要弄一个缓存池
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 LocalScale;

        public Vector2 Velocity;
        public float GravityScale;
    }

    [Serializable]
    public class TimeLinedVector3
    {
        private List<Vector3> position = new List<Vector3>();

        public void Add(Vector3 v3)
        {
            position.Insert(0, v3);
        }

        public Vector3 Get()
        {
            var v3 = position[0];
            if (position.Count > 1)
            {
                position.RemoveAt(0);
            }
            return v3;
        }
    }

    [Serializable]
    public class TimeLinedQuaternion
    {
        private List<Quaternion> quaternion = new List<Quaternion>();

        public void Add(Quaternion q)
        {
            quaternion.Insert(0, q);
        }

        public Quaternion Get()
        {
            var q = quaternion[0];
            if (quaternion.Count > 1)
            {
                quaternion.RemoveAt(0);
            }
            return q;
        }
    }

    [Serializable]
    public class TimeLinedAnimator
    {
        private List<string> aniName = new List<string>();
        private List<float> normalizedTime = new List<float>();

        public void Add(Animator animator)
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var time = stateInfo.normalizedTime;
            normalizedTime.Insert(0, time);
            var clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (!stateInfo.IsName(clip.name)) continue;
                aniName.Insert(0, clip.name);
                break;
            }
        }

        public void Set(Animator ani)
        {
            var name = aniName[0];
            if (aniName.Count > 1)
            {
                aniName.RemoveAt(0);
            }
            var time = normalizedTime[0];
            if (normalizedTime.Count > 1)
            {
                normalizedTime.RemoveAt(0);
            }
            ani.speed = 1f;
            Debug.LogError(time);
            ani.Play(name, 0, time);
            ani.speed = 0f;
        }
    }

    public class RewindData
    {
        private readonly TimeLinedVector3 position = new TimeLinedVector3();
        private readonly TimeLinedQuaternion rotation = new TimeLinedQuaternion();
        private readonly TimeLinedVector3 scale = new TimeLinedVector3();
        private readonly TimeLinedAnimator animator = new TimeLinedAnimator();

        public void Add(Transform tsf, Animator ani = null)
        {
            position.Add(tsf.position);
            rotation.Add(tsf.rotation);
            scale.Add(tsf.localScale);
            if (ani)
            {
                this.animator.Add(ani);
            }
        }

        public void Set(Transform tsf, Animator ani = null)
        {
            tsf.position = position.Get();
            tsf.rotation = rotation.Get();
            tsf.localScale = scale.Get();
            if (ani)
            {
                this.animator.Set(ani);
            }
        }
    }
}
