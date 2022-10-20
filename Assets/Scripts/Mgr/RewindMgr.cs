using Config;
using DFramework;
using Rewind;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Mgr
{
    public class RewindMgr : MonoSingleton<RewindMgr>
    {
        #region Tag
        /// <summary>
        /// 是否在回放
        /// </summary>
        private bool IsRewinding
        {
            set
            {
                GameMgr.Instance.SetRewindState(value);
            }
            get
            {
                return GameMgr.Instance.IsRewinding;
            }
        }

        #endregion
        
        #region Action
        public Action OnRewindStart;
        public Action OnRewindStop;
        #endregion

        /// <summary>
        /// 存放所有实体角色的列表
        /// </summary>
        private Dictionary<int, RewindEntity> EntityDic;

        private RewindMgr()
        {
            EntityDic = new Dictionary<int, RewindEntity>();
            // TotalFrame = Mathf.Round(GameConfig.TotalGameTime / Time.fixedDeltaTime);
        }

        public void EnterGame()
        {
            
        }

        public void ReleaseGame()
        {
            EntityDic.Clear();
            OnRewindStart = null;
            OnRewindStop = null;
        }

        public void AddEntity(int uid, RewindEntity body)
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



        public void FixedUpdate()
        {
            // if (!GameMgr.Instance.IsRunning) return;
            foreach (var kv in EntityDic)
            {
                if (IsRewinding)
                {
                    kv.Value.Rewinding();
                }
                else
                {
                    kv.Value.Recording();
                }
            }
        }

        public void StartRewind()
        {
            if (IsRewinding == true) return;
            IsRewinding = true;
            OnRewindStart?.Invoke();
        }

        public void StopRewind()
        {
            if (IsRewinding == false) return;
            IsRewinding = false;
            OnRewindStop?.Invoke();
        }
        
        
    }
}
