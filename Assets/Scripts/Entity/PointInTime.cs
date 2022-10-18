using UnityEngine;
namespace Entity
{
    [SerializeField]
    public class PointInTime
    {
        //TODO 需要弄一个缓存池
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 LocalScale;

        public Vector2 Velocity;
        public float GravityScale;
    }
}
