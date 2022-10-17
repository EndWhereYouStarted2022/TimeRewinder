using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DFramework
{
    /// <summary>
    /// 简易Transform
    /// </summary>
    public static partial class TransformExtension
    {
        public static void Show(this Transform transform)
        {
            transform.gameObject.SetActive(true);
        }
        
        public static void Hide(this Transform transform)
        {
            transform.gameObject.SetActive(false);
        }

        public static void SetActive(this Transform transform,bool value)
        {
            transform.gameObject.SetActive(value);
        }
        
        /// <summary>
        /// 设置目标当地X坐标
        /// </summary>
        /// <param name="transform">目标transform</param>
        /// <param name="x">x值</param>
        public static void SetLocalPosX(this Transform transform,float x)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置目标当地Y坐标
        /// </summary>
        /// <param name="transform">目标transform</param>
        /// <param name="y">y值</param>
        public static void SetLocalPosY(this Transform transform, float y)
        {
            var localPos = transform.localPosition;
            localPos.y = y;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置目标当地Z坐标
        /// </summary>
        /// <param name="transform">目标transform</param>
        /// <param name="z">z值</param>
        public static void SetLocalPosZ(this Transform transform,float z)
        {
            var localPos = transform.localPosition;
            localPos.z = z;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置目标当地XY坐标
        /// </summary>
        /// <param name="transform">目标transform</param>
        /// <param name="x">x值</param>
        /// <param name="y">y值</param>
        public static void SetLocalPosXY(this Transform transform,float x,float y)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            localPos.y = y;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置目标当地XZ坐标
        /// </summary>
        /// <param name="transform">目标transform</param>
        /// <param name="x">x值</param>
        /// <param name="z">z值</param>
        public static void SetLocalPosXZ(this Transform transform,float x,float z)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            localPos.z = z;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置目标当地YZ坐标
        /// </summary>
        /// <param name="transform">目标transform</param>
        /// <param name="y">y值</param>
        /// <param name="z">z值</param>
        public static void SetLocalPosYZ(this Transform transform,float y,float z)
        {
            var localPos = transform.localPosition;
            localPos.y = y;
            localPos.z = z;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 重置目标Transform
        /// </summary>
        /// <param name="transform">目标Transfor</param>
        public static void Identity(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="parentTrans">父亲Transform</param>
        /// <param name="childTrans">孩子Transform</param>
        public static void AddChild(this Transform parentTrans, Transform childTrans)
        {
            childTrans.SetParent(parentTrans);
        }
    }   
}
