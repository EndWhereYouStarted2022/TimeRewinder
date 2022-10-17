using System.Collections.Generic;
using UnityEngine;
namespace DFramework.Framework.ResKit
{
    /// <summary>
    /// 负责维护全局的资源缓存池
    /// </summary>
    public class ResManager : MonoSingleton<ResManager>
    {
        public readonly List<Res> ShareLoaderRes = new List<Res>();

        private const string K_SIMULATION_MODE_KEY = "Simulation mode";

#if UNITY_EDITOR
        private static int _simulationMode = -1;
        public static bool SimulationMode
        {
            get
            {
                if (_simulationMode == -1)
                {
                    _simulationMode = UnityEditor.EditorPrefs.GetBool(K_SIMULATION_MODE_KEY, true) ? 1 : 0;
                }
                return _simulationMode != 0;
            }
            set
            {
                _simulationMode = value ? 1 : 0;
                UnityEditor.EditorPrefs.SetBool(K_SIMULATION_MODE_KEY, value);
            }
        }
#endif
        public static bool IsSimulationModeLogic
        {
            get
            {
#if UNITY_EDITOR
                return SimulationMode;
#else
                return false;
#endif
            }
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (Input.GetKey(KeyCode.F1))
            {
                GUILayout.BeginVertical("box");
                ShareLoaderRes.ForEach(loaderRes =>
                {
                    GUILayout.Label($"Name:{loaderRes.Name} RefCount:{loaderRes.RefCount} State:{loaderRes.State}");
                });
                GUILayout.EndVertical();
            }
        }
#endif
    }
}
