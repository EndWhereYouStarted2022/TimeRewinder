using UnityEngine;
namespace DFramework
{
    public static partial class GameObjectExtension
    {
        public static void Show(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        public static void Hide(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}
