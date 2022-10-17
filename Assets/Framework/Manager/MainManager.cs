using UnityEngine;
using UnityEngine.Serialization;

namespace DFramework
{
    /// <summary>
    /// 开发时期
    /// </summary>
    public enum EnvironmentMode
    {
        Developing,
        QA,
        Production
    }
    public abstract class MainManager : MonoSingleton<MainManager>
    {
        [SerializeField] private EnvironmentMode Mode;
        public EnvironmentMode RunningMode { private set; get; }
        private bool mModeSetted = false;

        public MainManager() { }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            //设置锁 保证只设置一次
            if (!mModeSetted)
            {
                RunningMode = Mode;
                mModeSetted = true;
            }
            Init();
            switch (RunningMode)
            {
                case EnvironmentMode.Developing:
                    LaunchInDevelopingMode();
                    break;
                case EnvironmentMode.QA:
                    LaunchInQAMode();
                    break;
                case EnvironmentMode.Production:
                    LaunchInProduction();
                    break;
                default:
                    break;
            }
        }

        protected virtual void Init() { }

        /// <summary>
        /// 开发模式入口
        /// </summary>
        protected abstract void LaunchInDevelopingMode();
        /// <summary>
        /// 测试模式入口
        /// </summary>
        protected abstract void LaunchInQAMode();
        /// <summary>
        /// 发布模式入口
        /// </summary>
        protected abstract void LaunchInProduction();
    }
}
