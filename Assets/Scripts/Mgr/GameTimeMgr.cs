using Config;
using DFramework;
using Entity;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Mgr
{
    public class GameTimeMgr : MonoSingleton<GameTimeMgr>
    {

        /// <summary>
        /// 存放所有实体角色的列表
        /// </summary>
        private Dictionary<int, EntityBody> EntityDic;
        private float TotalFrame;

        private GameTimeMgr()
        {
            EntityDic = new Dictionary<int, EntityBody>();
            // TotalFrame = Mathf.Round(GameConfig.TotalGameTime / Time.fixedDeltaTime);
        }

        public void Init()
        {
            
        }

        public void Release()
        {
            
        }

        public void AddEntity(int uid, EntityBody body)
        {
            if (EntityDic.ContainsKey(uid))
            {
                Debug.LogError($"已经存在{uid}的角色了");
                return;
            }
            EntityDic.Add(uid, body);
        }

        public void RemoveEntity(int uid)
        {
            if (!EntityDic.ContainsKey(uid))
            {
                Debug.LogError($"没有找到{uid}对应的角色");
                return;
            }
            EntityDic.Remove(uid);
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.LogError("开始回放");
                foreach (var kv in EntityDic)
                {
                    kv.Value.IsRewinding = true;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Debug.LogError("结束回放");
                foreach (var kv in EntityDic)
                {
                    kv.Value.IsRewinding = false;
                }
            }
        }
    }
}
