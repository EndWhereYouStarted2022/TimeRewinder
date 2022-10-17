using DFramework.Framework.ResKit;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace DFramework
{
    public class SimulationModeMenu : MonoBehaviour
    {
        private const string K_SIMULATION_MODE_PATH = "DFramework/Framework/ResKit/Simulation Mode";

#if UNITY_EDITOR
        private static bool SimulationMode
        {
            get { return ResManager.SimulationMode; }
            set { ResManager.SimulationMode = value; }
        }

        [MenuItem(K_SIMULATION_MODE_PATH)]
        private static void ToggleSimulationMode()
        {
            SimulationMode = !SimulationMode;
        }

        [MenuItem(K_SIMULATION_MODE_PATH, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(K_SIMULATION_MODE_PATH, SimulationMode);
            return true;
        }
#endif
    }
}

