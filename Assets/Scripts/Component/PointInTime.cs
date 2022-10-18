using Mgr;
using System;
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
        public AnimationCurve x;
        public AnimationCurve y;
        public AnimationCurve z;

        public void Add(Vector3 v3)
        {
            var time = RewindMgr.Instance.GetCurrentTime();
            x.AddKey(time, v3.x);
            y.AddKey(time, v3.y);
            z.AddKey(time, v3.z);
        }

        public Vector3 Get(float time)
        {
            return new Vector3(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time));
        }
    }

    [Serializable]
    public class TimeLinedQuaternion
    {
        public AnimationCurve x;
        public AnimationCurve y;
        public AnimationCurve z;
        public AnimationCurve w;

        public void Add(Quaternion v)
        {
            var time = RewindMgr.Instance.GetCurrentTime();
            x.AddKey(time, v.x);
            y.AddKey(time, v.y);
            z.AddKey(time, v.z);
            w.AddKey(time, v.w);
        }

        public Quaternion Get(float time)
        {
            return new Quaternion(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time), w.Evaluate(time));
        }
    }

    [Serializable]
    public class TimeLinedAnimator
    {
        
        public void Add(Animator animator)
        {
            // float time = RewindMgr.Instance.GetCurrentTime();
            // x.AddKey (time, v.x);
            // y.AddKey (time, v.y);
            // z.AddKey (time, v.z);
            // w.AddKey (time, v.w);
        }

        public void Set(float time, Animator ani)
        {

        }
    }

    public class RewindData
    {
        public TimeLinedVector3 Position;
        public TimeLinedQuaternion Rotation;
        public TimeLinedVector3 Scale;
        public TimeLinedAnimator Animator;

        public void Add(Transform tsf, Animator animator = null)
        {
            Position.Add(tsf.position);
            Rotation.Add(tsf.rotation);
            Scale.Add(tsf.localScale);
            if (animator)
            {
                Animator.Add(animator);
            }
        }

        public void Set(float time, Transform tsf, Animator animator = null)
        {
            tsf.position = Position.Get(time);
            tsf.rotation = Rotation.Get(time);
            tsf.localScale = Scale.Get(time);
            if (animator)
            {
                Animator.Set(time, animator);
            }
        }
    }
}
