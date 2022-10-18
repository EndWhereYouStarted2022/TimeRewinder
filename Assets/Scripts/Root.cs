using DFramework;
using Mgr;
using System;
namespace Game
{
    public class Root:MainManager
    {
        protected override void LaunchInDevelopingMode()
        {
            InitMgr();
        }
        protected override void LaunchInQAMode()
        {
            
        }
        protected override void LaunchInProduction()
        {
            
        }

        private void InitMgr()
        {
            GameTimeMgr.Instance.Init();
            GameManager.Instance.Init();
            UIManager.Instance.Init();
            AudioManager.Instance.CheckAudioListener();
        }

        private void OnApplicationQuit()
        {
            // NotificationMgr.Instance.UnRegisterAll();
        }
    }
}
