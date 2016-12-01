using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XLAF.Public
{
    /// <summary>
    /// FPS、内存信息（调试用）
    /// </summary>
    public class MgrFPS : MonoBehaviour
    {

        private static MgrFPS instance;
        private bool showFPS = true;
        private float fpsUpdateDelay = 0.2f;
        private float fpsTime = 0f;
        private int fps = 0;
        private int minFps;
        private int maxFps;
        private int fpsCacheCount = 50;
        private List<int> fpsList = new List<int> ();

        private float mKBSize = 1024.0f * 1024.0f;
        private float totalAllocatedMemory;
        private float totalReservedMemory;
        private float totalUnusedReservedMemory;

        static MgrFPS ()
        {
            instance = (new GameObject ("MgrFPS")).AddComponent<MgrFPS> ();
        }

        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }

        public static void ShowFPS ()
        {
            instance.showFPS = true;
        }

        public static void HideFPS ()
        {
            instance.showFPS = false;
        }




        void Update ()
        {
            if (showFPS) {
                if (fpsTime + fpsUpdateDelay < Time.unscaledTime) {
                    fpsTime = Time.unscaledTime;
                    fps = Mathf.RoundToInt (1f / Time.unscaledDeltaTime);
                    AddFPS (fps);
                    minFps = fpsList.Min ();
                    maxFps = fpsList.Max ();

                    totalReservedMemory = (float)Profiler.GetTotalReservedMemory () / mKBSize;
                    totalAllocatedMemory = (float)Profiler.GetTotalAllocatedMemory () / mKBSize;
                    totalUnusedReservedMemory = (float)Profiler.GetTotalUnusedReservedMemory () / mKBSize;
                }
            }
        }

        void OnGUI ()
        {
            if (showFPS) {
                GUILayout.BeginVertical ();

                //fps
                GUILayout.BeginHorizontal ();
                GUI.color = GetGUIColor (fps);
                GUILayout.Label ("fps: " + fps.ToString ("f0"));
                GUI.color = GetGUIColor (minFps);
                GUILayout.Label ("min: " + minFps.ToString ("f0"));
                GUI.color = GetGUIColor (maxFps);
                GUILayout.Label ("max: " + maxFps.ToString ("f0"));
                GUILayout.EndHorizontal ();

                //memory usage
                GUILayout.BeginHorizontal ();
                GUI.color = Color.red;
                GUILayout.Label ("reserved: " + totalReservedMemory.ToString ("f2") + "M");
                GUILayout.Label ("allocated: " + totalAllocatedMemory.ToString ("f2") + "M");
                GUILayout.Label ("unused: " + totalUnusedReservedMemory.ToString ("f2") + "M");
                GUILayout.EndHorizontal ();

                GUILayout.EndVertical ();
            }
        }

        Color GetGUIColor (int value)
        {
            if (value > 30f)
                return Color.green;
            else if (value > 15f)
                return Color.yellow;
            else
                return Color.red;
        }

        void AddFPS (int fpsValue)
        {
            if (fpsList.Count >= fpsCacheCount) {
                fpsList.RemoveAt (0);
            }
            fpsList.Add (fpsValue);
        }
    }
}