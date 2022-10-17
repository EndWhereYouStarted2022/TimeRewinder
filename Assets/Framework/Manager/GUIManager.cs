using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DFramework
{
    public enum UILayer
    {
        Bg,
        Common,
        Top
    }

    public class GUIManager : MonoBehaviour
    {
        private static Dictionary<string, GameObject> mPanelDic = new Dictionary<string, GameObject>();
        private static GameObject mPrivateUIRoot;
        public static GameObject UIRoot
        {
            get
            {
                if (mPrivateUIRoot == null)
                {
                    mPrivateUIRoot = Object.Instantiate(Resources.Load<GameObject>("UIRoot"));
                    mPrivateUIRoot.name = "UIRoot";
                }
                return mPrivateUIRoot;
            }
        }
        public static GameObject LoadPanel(string panelName, UILayer layer = UILayer.Common)
        {
            if (mPanelDic.ContainsKey(panelName))
            {
                return null;
            }
            var panelPrefab = Resources.Load<GameObject>(panelName);
            var panel = Instantiate(panelPrefab);
            panel.name = panelName;
            switch (layer)
            {
                case UILayer.Bg:
                    panel.transform.SetParent(UIRoot.transform.Find("Bg"));
                    break;
                case UILayer.Common:
                    panel.transform.SetParent(UIRoot.transform.Find("Common"));
                    break;
                case UILayer.Top:
                    panel.transform.SetParent(UIRoot.transform.Find("Top"));
                    break;
            }

            var panelRectTrans = panel.transform as RectTransform;
            panelRectTrans.offsetMax = Vector2.zero;
            panelRectTrans.offsetMin = Vector2.zero;
            panelRectTrans.anchoredPosition3D = Vector3.zero;
            panelRectTrans.anchorMin = Vector2.zero;
            panelRectTrans.anchorMax = Vector2.one;
            mPanelDic.Add(panelName, panel);
            return panel;
        }

        public static bool UnLoadPanel(string panelName)
        {
            bool contains = false;
            if (mPanelDic.ContainsKey(panelName))
            {
                Object.Destroy(mPanelDic[panelName]);
                mPanelDic.Remove(panelName);
                contains = true;
            }
            return contains;
        }

        public static GameObject getPanel(string panelName)
        {
            if (mPanelDic.ContainsKey(panelName))
            {
                return mPanelDic[panelName];
            }
            return null;
        }

        public static void SetResolution(float width = 1280, float height = 720, float matchWidthOrHeight = 0)
        {
            var canvasScaler = UIRoot.GetComponent<CanvasScaler>();
            canvasScaler.referenceResolution = new Vector2(width, height);
            canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
        }
    }
}
